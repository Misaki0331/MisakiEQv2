using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MisakiEQ;

namespace MisakiEQ.Lib
{
    public class DelayFunction<T>
    {
        private Task tsk = new Task(() => { });
        private T? Context = default(T);
        private CancellationTokenSource tokenSource = new CancellationTokenSource();
        Action<T>? DelayFunc = null;
        bool IsUpdate = false;
        private List<T> NeedDelayed= new List<T>();
        private object NeedDelayedLock = new object();

        /// <summary>遅延時間</summary>
        public int DelayTime = 1000;
        /// <summary>
        /// 実行する関数をセットします。
        /// </summary>
        /// <param name="task"></param>
        public void SetTask(Action<T> task)
        {
            DelayFunc = task;
            IsUpdate = true;
        }
        bool IsRunning = false;
        public void QueueTask(T send)
        {
            if (!IsRunning)
            {
                SendTask(send);
                Log.Debug("キューが空である為転送します。");
            }
            else
            {
                lock (NeedDelayedLock)
                {
                    NeedDelayed.Add(send);
                    Log.Debug($"キューに追加しました。待ち:{NeedDelayed.Count}");
                }
            }
        }
        /// <summary>
        /// 通常時の処理を一定時間ごとに実行します。
        /// </summary>
        /// <param name="send"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public void SendTask(T send)
        {
            if (DelayFunc == null) throw new ArgumentNullException(nameof(DelayFunc));
            Context = send;
            IsUpdate = true;
            if (!IsRunning)
            {
                tsk = new(async () =>
                {
                    IsRunning = true;
                    while (IsUpdate)
                    {
                        IsUpdate = false;
                        var c = Context;
                        if(c!=null)DelayFunc(c);
                        Context = default;
                        try
                        {
                            await Task.Delay(DelayTime, tokenSource.Token);
                        }
                        catch (TaskCanceledException)
                        {
                            Log.Info("遅延処理はキャンセルされました。");
                            return; 
                        }
                        Log.Info("遅延処理は実行しています。");
                        if (!IsUpdate)
                        {
                            lock (NeedDelayedLock)
                            {
                                if (NeedDelayed.Count > 0)
                                {
                                    IsUpdate = true;
                                    Context = NeedDelayed[0];
                                    NeedDelayed.RemoveAt(0);
                                    Log.Info($"キュー処理を実行します。残り:{NeedDelayed.Count}");
                                }
                            }
                        }

                    }
                    Log.Info("遅延処理実行完了");
                    IsRunning = false;
                    return;
                });
                tsk.Start();
            }
        }
        /// <summary>
        /// 今のキューを破棄して割り込み処理を行います。
        /// </summary>
        /// <param name="send"></param>
        public void InterTask(T send)
        {
            IsUpdate = false;
            tokenSource.Cancel();
            if (tsk.Status == TaskStatus.RanToCompletion) tsk.Dispose();
            tokenSource = new();
            IsRunning = false;
            SendTask(send);
        }
    }
}
