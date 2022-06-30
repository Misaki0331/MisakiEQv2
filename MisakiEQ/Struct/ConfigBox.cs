using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MisakiEQ.Struct
{
    public class ConfigBox
    {
        public class Notification_EEW_Nationwide
        {
            public enum Enums
            {
                Int7=Common.Intensity.Int7,
                Int6Up=Common.Intensity.Int6Up,
                Int6Down=Common.Intensity.Int6Down,
                Int5Up=Common.Intensity.Int5Up,
                Int5Down=Common.Intensity.Int5Down,
                Int4=Common.Intensity.Int4,
                Int3=Common.Intensity.Int3,
                Int2=Common.Intensity.Int2,
                Int1=Common.Intensity.Int1,
                ALL=0,
                None=int.MaxValue,
                WarnOnly=int.MaxValue-1
            }
            public static Enums GetIndex(long i)
            {
                return i switch
                {
                    0 => Enums.Int7,
                    1 => Enums.Int6Up,
                    2 => Enums.Int6Down,
                    3 => Enums.Int5Up,
                    4 => Enums.Int5Down,
                    5 => Enums.Int4,
                    6 => Enums.Int3,
                    7 => Enums.Int2,
                    8 => Enums.Int1,
                    9 => Enums.ALL,
                    10 => Enums.None,
                    11 => Enums.WarnOnly,
                    _ => throw new ArgumentOutOfRangeException($"{i} に対応するIndexは存在しません。")
                };
            } 
        }
        public class Notification_EEW_Area
        {
            public enum Enums
            {
                Int7 = Common.Intensity.Int7,
                Int6Up = Common.Intensity.Int6Up,
                Int6Down = Common.Intensity.Int6Down,
                Int5Up = Common.Intensity.Int5Up,
                Int5Down = Common.Intensity.Int5Down,
                Int4 = Common.Intensity.Int4,
                Int3 = Common.Intensity.Int3,
                Int2 = Common.Intensity.Int2,
                Int1 = Common.Intensity.Int1,
                Int0 = Common.Intensity.Int0
            }
            public static Enums GetIndex(long i)
            {
                return i switch
                {
                    0 => Enums.Int7,
                    1 => Enums.Int6Up,
                    2 => Enums.Int6Down,
                    3 => Enums.Int5Up,
                    4 => Enums.Int5Down,
                    5 => Enums.Int4,
                    6 => Enums.Int3,
                    7 => Enums.Int2,
                    8 => Enums.Int1,
                    9 => Enums.Int0,
                    _ => throw new ArgumentOutOfRangeException($"{i} に対応するIndexは存在しません。")
                };
            }
        }
    }
}
