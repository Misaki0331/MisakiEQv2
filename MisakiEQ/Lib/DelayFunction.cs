using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MisakiEQ.Lib
{
    public class DelayFunction<T>
    {
        private Task tsk = new Task(() => { });
        private T? Context = default(T);
        private CancellationTokenSource tokenSource = new CancellationTokenSource();
        Action<T>? DelayFunc = null;
        bool IsUpdate = false;

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
                            Log.Instance.Info("遅延処理はキャンセルされました。");
                            return; 
                        }
                        Log.Instance.Info("遅延処理は実行しています。");
                    }
                    Log.Instance.Info("遅延処理実行完了");
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
