using MisakiEQ;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MisakiEQ.Background.API.KyoshinAPI
{
    public class KyoshinAPI
    {
        private readonly Stopwatch TSW = new();//thread起動時間
        public Config Config = new();
        public WebService.Latest.JSON.Root? Latest = null;
        public WebService.Message.JSON.Root? Message = null;
        public EEW.JSON.Root? KyoshinEEW = new();
        private string OldEEW = "";
        public event EventHandler? UpdatedKyoshin = null;
        public event EventHandler<EEWEventArgs>? UpdatedKyoshinEEW = null;
        Task? Threads;
        static readonly CancellationTokenSource CancelTokenSource = new();
        static readonly CancellationToken CancelToken = CancelTokenSource.Token;
        DateTime LatestDate = DateTime.MinValue;
        public DateTime KyoshinLatest { get => LatestDate; }
        public readonly List<KyoshinMap> ImageList = new();
        private readonly Lib.AsyncLock s_lock = new();
        DateTime LatestAdjustTime = DateTime.MinValue;
        public KyoshinAPI()
        {
            Config.KyoshinDelayTime = 1;
            Config.AutoAdjustKyoshinTime = 1800;
            Config.KyoshinFrequency = 1;
        }
        public enum KyoshinImageType{
            /// <summary>予報円</summary>
            PSWaveImg,
            /// <summary>予測震度</summary>
            EstShindoImg,
            /// <summary>リアルタイム震度(地上)</summary>
            jma_s,
            /// <summary>リアルタイム震度(地中)</summary>
            jma_b,
            /// <summary>最大加速度(地上)</summary>
            acmap_s,
            /// <summary>最大加速度(地中)</summary>
            acmap_b,
            /// <summary>最大速度(地上)</summary>
            vcmap_s,
            /// <summary>最大速度(地中)</summary>
            vcmap_b,
            /// <summary>最大変位(地上)</summary>
            dcmap_s,
            /// <summary>最大変位(地中)</summary>
            dcmap_b,
            /// <summary>0.125Hz速度応答(地上)</summary>
            rsp0125_s,
            /// <summary>0.125Hz速度応答(地中)</summary>
            rsp0125_b,
            /// <summary>0.25Hz速度応答(地上)</summary>
            rsp0250_s,
            /// <summary>0.25Hz速度応答(地中)</summary>
            rsp0250_b,
            /// <summary>0.5Hz速度応答(地上)</summary>
            rsp0500_s,
            /// <summary>0.5Hz速度応答(地中)</summary>
            rsp0500_b,
            /// <summary>1.0Hz速度応答(地上)</summary>
            rsp1000_s,
            /// <summary>1.0Hz速度応答(地中)</summary>
            rsp1000_b,
            /// <summary>2.0Hz速度応答(地上)</summary>
            rsp2000_s,
            /// <summary>2.0Hz速度応答(地中)</summary>
            rsp2000_b,
            /// <summary>4.0Hz速度応答(地上)</summary>
            rsp4000_s,
            /// <summary>4.0Hz速度応答(地中)</summary>
            rsp4000_b,

        }
        public async Task FixKyoshinTime()
        {
            try
            {
                var message = Lib.WebAPI.GetString($"http://www.kmoni.bosai.go.jp/webservice/server/pros/latest.json");
                await message;
                if (!string.IsNullOrEmpty(message.Result))
                {
                    Latest = JsonConvert.DeserializeObject<WebService.Latest.JSON.Root>(message.Result);
                    if (Latest != null && DateTime.TryParse(Latest.latest_time, out var date))
                    {
                        if (date.Year > 2000)
                        {
                            Log.Debug($"強震モニタ時刻取得完了 : {date}");
                            LatestDate = date;
                            LatestAdjustTime = DateTime.Now;
                            TSW.Restart();
                            return;
                        }
                    }
                }
                LatestAdjustTime = DateTime.Now.AddSeconds(-Config.AutoAdjustKyoshinTime + 10);
                Log.Warn($"強震モニタ時刻取得失敗");
            }catch(Exception ex)
            { 
                Log.Error(ex);
                await Task.Delay(5000);
            }
        }
        public static async Task<double> GetUserRawIntensity()
        {
            try
            {
                var api = await APIs.Instance.KyoshinAPI.GetImage(KyoshinImageType.EstShindoImg);
                if (api == null)
                {
                    Log.Warn("強震モニタの推定震度マップを取得できませんでした。");
                    return double.NaN;
                }
                Struct.Common.LAL lal = new(APIs.Instance.KyoshinAPI.Config.UserLong, APIs.Instance.KyoshinAPI.Config.UserLat);
                var pnt = Lib.KyoshinLib.LALtoKyoshinMap(lal);
                if (api.Width <= (int)pnt.X || api.Height <= (int)pnt.Y)
                {
                    return double.NaN;
                }
                var img = (Bitmap)api;
                return Lib.KyoshinLib.GetIntensity(img.GetPixel((int)pnt.X, (int)pnt.Y));
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return double.NaN;
            }
        }
        public static async Task<Struct.Common.Intensity> GetUserIntensity()
        {
            double raw = await GetUserRawIntensity();
            if (double.IsNaN(raw) || raw <= 0) return Struct.Common.Intensity.Unknown;
            else return Struct.Common.FloatToInt(raw);
        }
        public async Task<Image?> GetImage(KyoshinImageType type)
        {
            if (LatestDate.Year <= 2000) return null;
            using (await s_lock.LockAsync())
            {
                var res = ImageList.Find(a => a.ImageType == type);
                if (res != null) return res.GetImage();
                ImageList.Add(new KyoshinMap(type));
                Log.Debug($"{type}をキューに追加しました。");
                ImageList[^1]?.AccessImage(LatestDate.AddSeconds(-Config.KyoshinDelayTime));
                return ImageList[^1]?.GetImage();
            }
        }
        public async Task<List<KyoshinImageType>> GetImageQueueList()
        {
            using (await s_lock.LockAsync())
            {
                var list=new List<KyoshinImageType>();
                for (int i = 0; i < ImageList.Count; i++) list.Add(ImageList[i].ImageType);
                return list;
            }
        }
        public void RunThread()
        {

            if (Threads == null || Threads.Status != TaskStatus.Running)
            {
                Log.Debug("スレッド開始の準備を開始します。");
                TSW.Restart();
                Threads = Task.Run(() => ThreadFunction(CancelToken));
            }
            else Log.Error("該当スレッドは動作中の為、起動ができませんでした。");

        }
        public void AbortThread()
        {
            Log.Debug("スレッド破棄の準備を開始します。");
            CancelTokenSource.Cancel();
        }
        public async Task AbortAndWait()
        {
            Log.Debug("スレッドを終了しています...");
            CancelTokenSource.Cancel();
            if (Threads != null && !Threads.IsCompleted) await Threads;
        }
        public bool GetThreadWorking()
        {
            if (Threads == null) return false;
            return Threads.Status == TaskStatus.Running;
        }
        public long GetThreadTimer() //100ナノ秒単位
        {
            return TSW.ElapsedTicks;
        }
        public Struct.EEW GetData(Struct.EEW? from = null)
        {
            from ??= new();
            if (KyoshinEEW != null)
            {
                Log.Debug("汎用クラスに変換中...");
                from = Struct.EEW.GetData(KyoshinEEW, from);
                Log.Debug("汎用クラスに変換完了");
            }
            else Log.Warn("APIの情報がありませんでした。");
            return from;
        }


        private async void ThreadFunction(CancellationToken token)
        {
            Log.Info("スレッド開始");
            long TmpTimer = 0, TmpErrTimer = 0;
            await FixKyoshinTime();
            while (true)
            {
                try
                {
                    if (LatestDate.Year > 2000)
                    {
                        if (TmpTimer != TSW.ElapsedMilliseconds / 1000&&TSW.ElapsedMilliseconds/1000%Config.KyoshinFrequency==0)
                        {
                            if (TmpTimer< TSW.ElapsedMilliseconds / 1000)
                            {
                                LatestDate =LatestDate.AddSeconds( TSW.ElapsedMilliseconds /1000- TmpTimer);
                            }
                            TmpTimer = TSW.ElapsedMilliseconds / 1000;

                            var eew = Lib.WebAPI.GetString($"http://www.kmoni.bosai.go.jp/webservice/hypo/eew/{LatestDate.AddSeconds(-Config.KyoshinDelayTime):yyyyMMddHHmmss}.json"); 
                            using (await s_lock.LockAsync())
                            {

                                for (int i = 0; i < ImageList.Count; i++)
                                {
                                    if (ImageList[i].Lifetime < 0)
                                    {
                                        Log.Debug($"{ImageList[i].ImageType}は使われていない為キューから除外します。");
                                        ImageList.RemoveAt(i);
                                    }
                                }
                                Task[] tsk = new Task[ImageList.Count];
                                for (int i = 0; i < ImageList.Count; i++)
                                {
                                    tsk[i] = ImageList[i].AccessImage(LatestDate.AddSeconds(-Config.KyoshinDelayTime));
                                }
                                try
                                {
                                    await eew;
                                    for (int i = 0; i < ImageList.Count; i++) await tsk[i];
                                    try
                                    {
                                        UpdatedKyoshin?.Invoke(this, new EventArgs());
                                    }catch(Exception ex)
                                    {
                                        Log.Warn($"イベント送信時にエラーが発生しました。{ex.Message}");
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Log.Warn($"取得時にエラーが発生しました。{ex.Message}");
                                }

                            }
                            if (eew.IsCompletedSuccessfully && !string.IsNullOrEmpty(eew.Result))
                            {
                                if (OldEEW != eew.Result)
                                {
                                    OldEEW = eew.Result;
                                    KyoshinEEW = JsonConvert.DeserializeObject<EEW.JSON.Root>(eew.Result);
                                    if (KyoshinEEW != null && KyoshinEEW.Result.Status == "success" && KyoshinEEW.Alertflg != null)
                                    {
                                        if (UpdatedKyoshinEEW != null)
                                        {
                                            var args = new EEWEventArgs(KyoshinEEW, GetData());
                                            UpdatedKyoshinEEW(this, args);
                                        }

                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        if (TmpErrTimer / 10 != TSW.ElapsedMilliseconds / 10000)
                        {
                            TmpErrTimer = TSW.ElapsedMilliseconds / 1000;
                            await FixKyoshinTime();
                        }
                    }
                    if (LatestAdjustTime.AddSeconds(Config.AutoAdjustKyoshinTime) < DateTime.Now)
                    {
                        await FixKyoshinTime();
                    }
                    await Task.Delay(75, token);
                }
                catch (TaskCanceledException ex)
                {
                    Log.Info($"スレッドの処理を終了します。{ex.Message}");
                    return;
                }
                catch (Exception ex)
                {
                    Log.Error(ex);
                }
            }
        }
        public class EEWEventArgs
        {
            public EEW.JSON.Root? json = null;
            public Struct.EEW? eew = null;

            public EEWEventArgs(EEW.JSON.Root? root, Struct.EEW? eew)
            {
                this.json = root;
                this.eew = eew;
            }
        }
        public class KyoshinMap
        {
            Image Data;
            public KyoshinImageType ImageType { get; set; }
            public int Lifetime = 3;
            public KyoshinMap(KyoshinImageType type)
            {
                ImageType = type;
                Data = new Bitmap(352, 400);
                var g = Graphics.FromImage(Data);
                g.Clear(Color.FromArgb(0));
                g.Dispose();
            }
            void SetImage(byte[] data)
            {
                try
                {
                    using var ms = new MemoryStream(data);
                    if (Data != null) Data.Dispose();
                    Data = Image.FromStream(ms);
                }
                catch(Exception ex)
                {
                    Log.Error(ex);
                }
            }
            public async Task AccessImage(DateTime time,CancellationToken? cancel=null)
            {
                try
                {
                    Lifetime--;
                    string URL = "";
                    URL = ImageType switch
                    {
                        KyoshinImageType.PSWaveImg or KyoshinImageType.EstShindoImg => $"http://www.kmoni.bosai.go.jp/data/map_img/{ImageType}/eew/{time:yyyyMMdd}/{time:yyyyMMddHHmmss}.eew.gif",
                        _ => $"http://www.kmoni.bosai.go.jp/data/map_img/RealTimeImg/{ImageType}/{time:yyyyMMdd}/{time:yyyyMMddHHmmss}.{ImageType}.gif",
                    };
                    SetImage(await Lib.WebAPI.GetBytes(URL, cancel));
                }
                catch (TaskCanceledException)
                {
                    Log.Info("タスクが取り消されました。");
                }catch(Exception ex)
                {
                    Log.Warn($"{ImageType}のデータ取得中にエラー\n{ex.Message}");
                }
            }
            public Image? GetImage()
            {
                Lifetime = 3;
                if (Data == null)
                {
                    Log.Debug($"{ImageType}のデータはnullです");
                    return null;
                }
                return new Bitmap(Data);
            }
        }
    }
}
