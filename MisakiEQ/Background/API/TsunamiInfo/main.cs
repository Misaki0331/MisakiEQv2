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
    internal class _TsunamiInfo
#pragma warning restore IDE1006 // 命名スタイル
    {
        readonly Log.Logger log = Log.Logger.GetInstance();
        private readonly Thread? thread = default;
        private readonly Stopwatch TSW = new();//thread起動時間
        public TsunamiInfo._Config Config = new();
        public List<TsunamiInfo.JSON.Root>? Data = null;
        private string OldTemp = "";
        private bool IsUpdatedData = false;
        public event EventHandler<TsunamiInfoEventArgs>? UpdateHandler;
        Task? Threads;
        static readonly CancellationTokenSource CancelTokenSource = new();
        static CancellationToken CancelToken = CancelTokenSource.Token;
        public _TsunamiInfo()
        {
            Config.Delay = 5000;
            CancelToken = CancelTokenSource.Token;
        }
        public void Init()
        {
        }
        public void RunThread()
        {
            if (Threads == null || Threads.Status!=TaskStatus.Running)
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
        private async Task ThreadFunction(CancellationToken token)
        {
            log.Info("スレッド開始");
            long TempDelay = 0;
            while (true)
            {
                string json = "";
                try
                {
                    if (TSW.ElapsedMilliseconds >= TempDelay)
                    {
                        long count = TSW.ElapsedMilliseconds / Config.Delay;
                        TempDelay = Config.Delay * (count + 1);
                        var task = Lib.WebAPI.GetString("https://api.p2pquake.net/v2/jma/tsunami?limit=10&order=-1",token);
                        await task;
                        if (task.IsCompletedSuccessfully)
                        {
                            json = task.Result;
                            if (OldTemp != json)
                            {
                                OldTemp = json;
                                Data = JsonConvert.DeserializeObject<List<TsunamiInfo.JSON.Root>>(json);
                                if (UpdateHandler != null)
                                {
                                    var args = new TsunamiInfoEventArgs(Data);
                                    UpdateHandler(this, args);
                                }
                            }
                        }
                        else
                        {
                            log.Warn($"取得時にエラーが発生しました。{(task.Exception != null ? task.Exception.Message : "例外はnullで返されました。")}");
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
    class TsunamiInfoEventArgs
    {
        public List<TsunamiInfo.JSON.Root>? json = null;

        public TsunamiInfoEventArgs(List<TsunamiInfo.JSON.Root>? root)
        {
            this.json = root;
        }
    }
}
