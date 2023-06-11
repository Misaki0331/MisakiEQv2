using MisakiEQ;
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
    internal class _EQInfo
#pragma warning restore IDE1006 // 命名スタイル
    {
        private readonly Stopwatch TSW = new();//thread起動時間
        public EQInfo.Config Config = new();
        public List<EQInfo.JSON.Root>? Data = null;
        private string OldTemp = "";
        private bool IsUpdatedData = false;
        public event EventHandler<EarthQuakeEventArgs>? EarthQuakeUpdateHandler;
        public event EventHandler<TsunamiEventArgs>? TsunamiUpdateHandler;
        Task? Threads;
        static readonly CancellationTokenSource CancelTokenSource = new();
        static readonly CancellationToken CancelToken = CancelTokenSource.Token;
        DateTime LatestInfomation = DateTime.MinValue;
        bool IsFirst = true;


        public _EQInfo()
        {
            Config.Delay = 5000;
            Config.Limit = 10;
        }
        public void Init()
        {
        }
        public void RunThread()
        {

            if (Threads == null || Threads.Status != TaskStatus.Running)
            {
                Log.Debug("スレッド開始の準備を開始します。");
                TSW.Restart();
                Threads = Task.Run(() => ThreadFunction(CancelToken));
            }
            else
            {
                Log.Error("該当スレッドは動作中の為、起動ができませんでした。");
            }

        }
        public void AbortThread()
        {
            Log.Debug("スレッド破棄の準備を開始します。");
            TSW.Stop();
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
        private async void ThreadFunction(CancellationToken token)
        {
            Log.Info("スレッド開始");
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
                        var task = Lib.WebAPI.GetString($"https://api.p2pquake.net/v2/history?codes=551&codes=552&limit={Config.Limit}",token);
                        try
                        {
                            await task;
                        }
                        catch (Exception ex)
                        {
                            Log.Warn($"取得時にエラーが発生しました。{ex.Message}");
                        }
                        if (task.IsCompletedSuccessfully && !string.IsNullOrEmpty(task.Result))
                        { 
                            json = task.Result;
                            if (!string.Equals(OldTemp,json))
                            {
                                OldTemp = json;
                                IsUpdatedData = true; 
                                Data = JsonConvert.DeserializeObject<List<EQInfo.JSON.Root>>(json);
                                if (Data != null)
                                {
                                    for (int i = Data.Count - 1; i >= 0; i--)
                                    {
                                        if(DateTime.TryParse(Data[i].time,out DateTime res)){
                                            if (LatestInfomation <res)
                                            {
                                                if (!IsFirst)
                                                {
                                                    switch (Data[i].code)
                                                    {
                                                        case 551:
                                                            if (EarthQuakeUpdateHandler != null)
                                                            {
                                                                var args = new EarthQuakeEventArgs(Struct.EarthQuake.GetData(Data[i]));
                                                                EarthQuakeUpdateHandler(this, args);
                                                            }
                                                            break;
                                                        case 552:
                                                            if (TsunamiUpdateHandler != null)
                                                            {
                                                                var args = new TsunamiEventArgs(Struct.Tsunami.GetData(Data[i]));
                                                                TsunamiUpdateHandler(this, args);
                                                            }
                                                            break;
                                                    }
                                                }
                                                LatestInfomation = res;
                                            }
                                        }
                                    }
                                    IsFirst= false;
                                }
                            }
                        }
                    }
                    await Task.Delay(10, token);
                }
                catch (TaskCanceledException ex)
                {
                    Log.Info($"スレッドの処理を終了します。{ex.Message}");
                    return;
                }
                catch (Exception ex)
                {
                    Log.Error($"文字列データ : \"{json}\"");
                    Log.Error(ex);
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
        public void Test(string json)
        {
            var Data = JsonConvert.DeserializeObject<EQInfo.JSON.Root>(json);
            if (Data == null) return;
            switch (Data.code)
            {
                case 551:
                    if (EarthQuakeUpdateHandler != null)
                    {
                        var args = new EarthQuakeEventArgs(Struct.EarthQuake.GetData(Data));
                        EarthQuakeUpdateHandler(this, args);
                    }
                    break;
                case 552:
                    if (TsunamiUpdateHandler != null)
                    {
                        var args = new TsunamiEventArgs(Struct.Tsunami.GetData(Data));
                        TsunamiUpdateHandler(this, args);
                    }
                    break;
            }
        }
    }
    class EarthQuakeEventArgs
    {
        public Struct.EarthQuake? data = null;

        public EarthQuakeEventArgs(Struct.EarthQuake? root)
        {
            this.data = root;
        }
    }
    class TsunamiEventArgs
    {
        public Struct.Tsunami? data = null;

        public TsunamiEventArgs(Struct.Tsunami root)
        {
            this.data = root;
        }
    }
}
