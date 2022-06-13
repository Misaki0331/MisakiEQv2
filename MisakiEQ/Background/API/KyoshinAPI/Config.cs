using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MisakiEQ.Background.API.KyoshinAPI
{
    public class Config
    {
        /// <summary>強震モニタの最新時刻をn秒毎に修正する</summary>
        public int AutoAdjustKyoshinTime = 1800;
        /// <summary>強震モニタの最新時刻からn秒遅れて再生する</summary>
        public int KyoshinDelayTime = 1;
        /// <summary>強震モニタの更新頻度(n秒)</summary>
        public int KyoshinFrequency = 1;
    }
}
