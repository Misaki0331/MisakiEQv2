using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace MisakiEQ.Background.API.EEW
{
    public class Config
    {
        ///<summary>通常時の遅延(ms)/summary>
        public long Delay;
        ///<summary>検出時の遅延(ms)/summary>
        public long DelayDetectMode;
        ///<summary>検出から通常時に戻る時間(ms)</summary>
        public long DelayDetectCoolDown;
        ///<summary>DMDATAでの警報のみ受け取るか</summary>
        public bool IsWarningOnlyInDMDATA { 
            get => Background.APIs.Instance.EEW.DMData.IsWarnOnly; 
            set => Background.APIs.Instance.EEW.DMData.IsWarnOnly = value; 
        }
    }
}
