using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace MisakiEQ.Background.API.EEW
{
    internal class Config
    {
        ///<summary>通常時の遅延(ms)/summary>
        public uint Delay;
        ///<summary>検出時の遅延(ms)/summary>
        public uint DelayDetectMode;
        ///<summary>検出から通常時に戻る時間(ms)/summary>
        public uint DelayDetectCoolDown;
    }
}
