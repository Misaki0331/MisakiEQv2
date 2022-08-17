﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace MisakiEQ.Background.API.EEW
{
    public class Config
    {
        ///<summary>通常時の遅延(ms)/summary>
        public uint Delay;
        ///<summary>検出時の遅延(ms)/summary>
        public uint DelayDetectMode;
        ///<summary>検出から通常時に戻る時間(ms)</summary>
        public uint DelayDetectCoolDown;
        ///<summary>DMDATAでの警報のみ受け取るか</summary>
        public bool IsWarningOnlyInDMDATA { get => Background.APIs.GetInstance().EEW.DMData.IsWarnOnly; set => Background.APIs.GetInstance().EEW.DMData.IsWarnOnly = value; }
    }
}
