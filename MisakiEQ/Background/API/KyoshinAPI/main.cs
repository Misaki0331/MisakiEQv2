using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MisakiEQ.Background.API.KyoshinAPI
{
    public class _KyoshinAPI
    {
        readonly Log.Logger log = Log.Logger.GetInstance();
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
        public readonly KyoshinMap EEWShindo = new(KyoshinImageType.EstShindoImg);
        public readonly List<KyoshinMap> ImageList = new();
        private readonly Lib.AsyncLock s_lock = new Lib.AsyncLock();
        public _KyoshinAPI()
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
        async void Init()
        {
            var message = Lib.WebAPI.GetString($"http://www.kmoni.bosai.go.jp/webservice/server/pros/latest.json");
            await message;
            if (!string.IsNullOrEmpty(message.Result))
            {
                Latest = JsonConvert.DeserializeObject<WebService.Latest.JSON.Root>(message.Result);
                if(Latest!=null&&DateTime.TryParse(Latest.latest_time,out var date)){
                    if (date.Year > 2000)
                    {
                        LatestDate = date;
                        TSW.Restart();
                    }
                }
            }
        }
        public async Task<bool> AddImageQueue(KyoshinImageType type)
        {
            using (await s_lock.LockAsync())
            {
                for (int i = 0; i < ImageList.Count; i++)
            {
                if (ImageList[i].ImageType == type)
                {
                    log.Warn($"既に\"{type}\"は存在する為、追加しませんでした。");
                    return false;
                }
            }
                ImageList.Add(new KyoshinMap(type));
            }
            log.Info($"\"{type}\"をキューに追加しました。");
            return true;
        }
        public async Task<bool> RemoveImageQueue(KyoshinImageType type)
        {
            using (await s_lock.LockAsync())
            {
                for (int i = 0; i < ImageList.Count; i++)
            {
                if (ImageList[i].ImageType == type)
                {
                    log.Info($"\"{type}\"をキューから削除しました。");
                        ImageList.RemoveAt(i);
                    return true;
                }
                }
            }
            log.Warn($"\"{type}\"は存在しませんでした。");
            return false;
        }
        public async Task<Image?> GetImage(KyoshinImageType type)
        {
            using (await s_lock.LockAsync())
            {
                for (int i = 0; i < ImageList.Count; i++)
                {
                    if (ImageList[i].ImageType == type)
                    {
                        return ImageList[i].GetImage();
                    }
                }
            return null;
            }
        }
        public async Task<List<KyoshinImageType>> getImageQueueList()
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
                log.Debug("スレッド開始の準備を開始します。");
                TSW.Restart();
                Threads = Task.Run(() => ThreadFunction(CancelToken));
            }
            else
            {
                log.Error("該当スレッドは動作中の為、起動ができませんでした。");
            }

        }
        public void AbortThread()
        {
            log.Debug("スレッド破棄の準備を開始します。");
            CancelTokenSource.Cancel();
        }
        public async Task AbortAndWait()
        {
            log.Debug("スレッドを終了しています...");
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
            if (from == null) from = new();
            if (KyoshinEEW != null)
            {
                log.Debug("汎用クラスに変換中...");
                from = Struct.EEW.GetData(KyoshinEEW, from);
                log.Debug("汎用クラスに変換完了");
            }
            else
            {
                log.Warn("APIの情報がありませんでした。");
            }
            return from;

        }
        private async Task ThreadFunction(CancellationToken token)
        {
            log.Info("スレッド開始");
            long TmpTimer = 0;
            long TmpErrTimer = 0;
            Init();
            while (true)
            {
                try
                {
                    if (LatestDate.Year > 2000)
                    {
                        if (TmpTimer != TSW.ElapsedMilliseconds / 1000%Config.KyoshinFrequency)
                        {
                            if(TmpTimer> TSW.ElapsedMilliseconds / (Config.KyoshinFrequency * 1000))
                            {
                                LatestDate.AddSeconds(TmpTimer - TSW.ElapsedMilliseconds / (Config.KyoshinFrequency * 1000));
                            }
                            TmpTimer = TSW.ElapsedMilliseconds / 1000; 
                            var eew = Lib.WebAPI.GetString($"http://www.kmoni.bosai.go.jp/webservice/hypo/eew/{LatestDate.AddSeconds(-Config.KyoshinDelayTime):yyyyMMddHHmmss}.json"); 
                            var eewimg = EEWShindo.AccessImage(LatestDate.AddSeconds(-Config.KyoshinDelayTime));
                            eew.Start();
                            eewimg.Start();
                            using (await s_lock.LockAsync())
                            {
                                Task[] tsk = new Task[ImageList.Count];
                                for (int i = 0; i < ImageList.Count; i++)
                                {
                                    tsk[i] = ImageList[i].AccessImage(LatestDate.AddSeconds(-Config.KyoshinDelayTime));
                                    tsk[i].Start();
                                }
                                try
                                {
                                    await eew;
                                    await eewimg;
                                    for (int i = 0; i < ImageList.Count; i++) await tsk[i];
                                }
                                catch (Exception ex)
                                {
                                    log.Warn($"取得時にエラーが発生しました。{ex.Message}");
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
                            Init();
                        }
                    }
                    await Task.Delay(10, token);
                }
                catch (TaskCanceledException ex)
                {
                    log.Info($"スレッドの処理を終了します。{ex.Message}");
                    return;
                }
                catch (Exception ex)
                {
                    log.Error(ex);
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
                    Data = Image.FromStream(ms);
                }
                catch(Exception ex)
                {
                    Log.Logger.GetInstance().Error(ex);
                }
            }
            public async Task AccessImage(DateTime time,CancellationToken? cancel=null)
            {
                try
                {
                    string URL = "";
                    switch (ImageType)
                    {
                        case KyoshinImageType.PSWaveImg:
                        case KyoshinImageType.EstShindoImg:
                            URL = $"http://www.kmoni.bosai.go.jp/data/map_img/{ImageType}/eew/{time:yyyyMMdd}/{time:yyyyMMddHHmmss}.eew.gif";
                            break;
                        default:
                            URL = $"http://www.kmoni.bosai.go.jp/data/map_img/RealTimeImg/{ImageType}/{time:yyyyMMdd}/{time:yyyyMMddHHmmss}.{ImageType}.gif";
                            break;
                    }
                    SetImage(await Lib.WebAPI.GetBytes(URL, cancel));
                }
                catch (TaskCanceledException)
                {
                    Log.Logger.GetInstance().Info("タスクが取り消されました。");
                }
            }
            public Image? GetImage()
            {
                return Data;
            }
        }
    }
}
