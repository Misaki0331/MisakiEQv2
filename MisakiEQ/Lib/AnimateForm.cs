using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MisakiEQ.Lib
{
    public static class Animator
    {
        /// <summary>
        /// 1 フレームの時間とフレーム数を指定してアニメーション機能を提供します。
        /// </summary>
        /// <param name="interval">1 フレームの時間をミリ秒単位で指定します。</param>
        /// <param name="frequency">
        /// frequency はコールバックが呼ばれる回数から 1 を引いたものです。例えば frequency が 10 の時には 11 回呼ばれます。
        /// </param>
        /// <param name="callback">
        /// bool callback(int frame, int frequency) の形でコールバックを指定します。
        /// frame は 0 から frequency の値まで 1 ずつ増加します。
        /// frequency は引数と同じものです。
        /// </param>
        public static void Animate(int interval, int frequency, Func<int, int, bool> callback)
        {
            var timer = new System.Windows.Forms.Timer
            {
                Interval = interval
            };
            int frame = 0;
            timer.Tick += (sender, e) =>
            {
                if (callback(frame, frequency) == false || frame >= frequency)
                {
                    timer.Stop();
                }
                frame++;
            };
            timer.Start();
        }

        /// <summary>
        /// 持続時間を指定してアニメーション機能を提供します。
        /// </summary>
        /// <param name="duration">持続時間をミリ秒単位で指定します。</param>
        /// <param name="callback">
        /// bool callback(int frame, int frequency) の形でコールバックを指定します。
        /// frame は 0 から frequency の値まで 1 ずつ増加します。
        /// frequency はコールバックが呼ばれる回数から 1 を引いたものです。例えば frequency が 10 の時には 11 回呼ばれます。
        /// </param>
        public static void Animate(int duration, Func<int, int, bool> callback)
        {
            const int interval = 25;
            if (duration < interval) duration = interval;
            Animate(25, duration / interval, callback);
        }
    }

}
