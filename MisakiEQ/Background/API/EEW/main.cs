using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#pragma warning disable CA1822
namespace MisakiEQ.Background.API
{
#pragma warning disable IDE1006 // 命名スタイル
    internal class _EEW
#pragma warning restore IDE1006 // 命名スタイル
    {
        readonly Log.Logger log = Log.Logger.GetInstance();
        private readonly Thread? thread = null;
        private readonly Stopwatch TSW = new();//thread起動状態兼タイマー
        public EEW._Config Config = new();
        public EEW.JSON.Root? Data = new();
        private bool IsUpdatedData=false;
        private string OldTemp = "";
        public event EventHandler<EEWEventArgs>? UpdateHandler;
        private bool IsFirst = true;

        Task? Threads;
        static readonly CancellationTokenSource CancelTokenSource = new();
        static readonly CancellationToken CancelToken = CancelTokenSource.Token;
        public _EEW()
        {
            Config.Delay = 1000;
            Config.DelayDetectMode = 200;
            Config.DelayDetectCoolDown = 3000;
        }
        public void Init()
        {
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
            if (thread == null) return false;
            return thread.IsAlive;
        }
        public long GetThreadTimer() //100ナノ秒単位
        {
            return TSW.ElapsedTicks;
        }

        public bool IsTest = false;





        private async Task ThreadFunction(CancellationToken token)
        {
            log.Info("スレッド開始");
            long TempDelay=0;
            long TempDetect = long.MinValue;
            while (true)
            {
                string json="";
                try
                {
                    if (TSW.ElapsedMilliseconds >= TempDelay)
                    {

                        var task = Lib.WebAPI.GetString("https://api.iedred7584.com/eew/json/",token);
                        try
                        {
                            await task;
                        }
                        catch (Exception ex)
                        {
                            log.Warn($"取得時にエラーが発生しました。{ex.Message}");
                        }
                        if (task.IsCompletedSuccessfully&&!string.IsNullOrEmpty(task.Result))
                        {
                            json = task.Result;
                            if (IsTest) json = Properties.Resources.testForecast;
                            if (OldTemp != json)
                            {
                                TempDetect=TSW.ElapsedMilliseconds;
                                OldTemp = json;
                                Data = JsonConvert.DeserializeObject<EEW.JSON.Root>(json);
                                if (Data != null && Data.ParseStatus == "Success")
                                {
                                    if (IsFirst)
                                    {
                                        IsFirst = false;
                                    }
                                    else
                                    {
                                        IsUpdatedData = true;
                                        if (UpdateHandler != null)
                                        {
                                            var args = new EEWEventArgs(Data, GetData());
                                            UpdateHandler(this, args);
                                        }
                                    }
                                }
                            }
                        }

                        long count = 0;
                        if (TempDetect > TSW.ElapsedMilliseconds - Config.DelayDetectCoolDown)
                        {
                            count = TSW.ElapsedMilliseconds / Config.DelayDetectMode;
                            TempDelay = Config.DelayDetectMode * (count + 1);
                        }
                        else
                        {
                            count = TSW.ElapsedMilliseconds / Config.Delay;
                            TempDelay = Config.Delay * (count + 1);
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
                    log.Error($"文字列データ : \"{json}\"");
                    log.Error(ex);
                }
            }

        }
        public Struct.EEW GetData(Struct.EEW? from=null)
        {
            if (from == null) from = new();
            if (Data != null)
            {
                log.Debug("汎用クラスに変換中...");
                from = Struct.EEW.GetData(Data, from);
                log.Debug("汎用クラスに変換完了");
            }
            else
            {
                log.Warn("APIの情報がありませんでした。");
            }
            return from;
            
        }
        public bool GetUpdated()
        {
            if (IsUpdatedData)
            {
                IsUpdatedData = false;
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    class EEWEventArgs
    {
        public EEW.JSON.Root? json = null;
        public Struct.EEW? eew = null;

        public EEWEventArgs(EEW.JSON.Root? root , Struct.EEW? eew)
        {
            this.json = root;
            this.eew = eew;
        }
    }
}
