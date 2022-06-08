using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#pragma warning disable IDE1006 // 命名スタイル
namespace MisakiEQ.Background.API.EQInfo
{
    internal class _Config
    {
        public uint Delay;   //通常時の遅延(ms)
        public uint Limit;   //取得時の配列の数
    }
}