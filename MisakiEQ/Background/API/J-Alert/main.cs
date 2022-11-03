using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MisakiEQ.Background.API
{
    public class JAlert
    {
        private readonly Stopwatch TSW = new();//thread起動時間
        public J_Alert.Config Config = new();
        public List<EQInfo.JSON.Root>? Data = null;
        private string OldTemp = "";
        private bool IsUpdatedData = false;
        public event EventHandler<J_AlertEventArgs>? J_AlertUpdateHandler;
        Task? Threads;
        static readonly CancellationTokenSource CancelTokenSource = new();
        static readonly CancellationToken CancelToken = CancelTokenSource.Token;
        DateTime LatestInfomation = DateTime.MinValue;
        bool IsFirst = true;
        Struct.cJAlert.J_Alert LatestData=new();
        public Struct.cJAlert.J_Alert LatestJAlert { get => LatestData;}

        public JAlert()
        {
            Config.Delay = 15000;
            Config.IsDisplay=true;
        }
        public void Init()
        {
        }
        public void RunThread()
        {

            if (Threads == null || Threads.Status != TaskStatus.Running)
            {
                Log.Instance.Debug("スレッド開始の準備を開始します。");
                TSW.Restart();
                Threads = Task.Run(() => ThreadFunction(CancelToken));
            }
            else
            {
                Log.Instance.Error("該当スレッドは動作中の為、起動ができませんでした。");
            }

        }
        public void AbortThread()
        {
            Log.Instance.Debug("スレッド破棄の準備を開始します。");
            TSW.Stop();
            CancelTokenSource.Cancel();
        }
        public async Task AbortAndWait()
        {
            Log.Instance.Debug("スレッドを終了しています...");
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
            Log.Instance.Info("スレッド開始");
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
                        var task = Lib.WebAPI.GetString($"https://emergency-weather.yahoo.co.jp/weather/jp/jalert/", token);
                        try
                        {
                            await task;
                        }
                        catch (Exception ex)
                        {
                            Log.Instance.Warn($"取得時にエラーが発生しました。{ex.Message}");
                        }
                        if (task.IsCompletedSuccessfully && !string.IsNullOrEmpty(task.Result))
                        {
                            string a = "<p class=\\\"jalertInfo-item\\\">(.*)</p>";
                            MatchCollection results  = Regex.Matches(task.Result, a);
                            foreach (Match m in results) // Matchと型を明示（varは不可）
                            {
                                int index = m.Index; // 発見した文字列の開始位置
                                string value = m.Value; // 発見した文字列
                            }
                            var str = results[0].Value;
                            a = "<div class=\\\"header large title\\\">[\\s]*<h1>(.*)?</h1>";
                            results = Regex.Matches(task.Result, a);
                            foreach (Match m in results) // Matchと型を明示（varは不可）
                            {
                                int index = m.Index; // 発見した文字列の開始位置
                                string value = m.Value; // 発見した文字列
                            }
                            var t = results[0].Value;
                            str = str.Replace("<p class=\"jalertInfo-item\">", "").Replace("</p>", "").Trim();
                            str = str.Replace("\r", "").Replace("\n", "");
                            str = str.Replace("<br>", "\n");
                            t = Regex.Replace(t, "<(\"[^\"]*\"|'[^']*'|[^'\">])*>", "").Trim();
                            str = Regex.Replace(str, "<(\"[^\"]*\"|'[^']*'|[^'\">])*>","");
                            if (string.IsNullOrEmpty(str) || string.IsNullOrEmpty(t)) continue;
                            //Log.Instance.Debug($"title: {t}\nIndex: {str}");
                            json = str;
                            if (OldTemp != json)
                            {
                                var data=Struct.J_Alert.GetJAlertData(t, str);
                                if (data.IsValid)
                                {
                                    OldTemp = json;
                                    LatestData = data;
                                    if (!IsFirst)
                                    {
                                        var args = new J_AlertEventArgs(data);
                                        if(J_AlertUpdateHandler!=null)J_AlertUpdateHandler(this, args);
                                    }
                                    IsFirst = false;
                                }
                            }
                        }
                    }
                    await Task.Delay(10, token);
                }
                catch (TaskCanceledException ex)
                {
                    Log.Instance.Info($"スレッドの処理を終了します。{ex.Message}");
                    return;
                }
                catch (Exception ex)
                {
                    Log.Instance.Error($"文字列データ : \"{json}\"");
                    Log.Instance.Error(ex);
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
    public class J_AlertEventArgs
    {
        public Struct.cJAlert.J_Alert? data = null;

        public J_AlertEventArgs(Struct.cJAlert.J_Alert? root)
        {
            this.data = root;
        }
    }
}
