using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#pragma warning disable IDE1006 // 命名スタイル
namespace MisakiEQ.Background.API.EEW
{
    internal class _Config
    {
        ///<summary>通常時の遅延(ms)/summary>
        public uint Delay;
        ///<summary>検出時の遅延(ms)/summary>
        public uint DelayDetectMode;
        ///<summary>検出から通常時に戻る時間(ms)/summary>
        public uint DelayDetectCoolDown;
    }
}
