using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MisakiEQ.Lib.PrefecturesAPI
{
    internal class PrefectureCodes
    {
        public static string? GetPrefectures(string? ID)
        {
            return ID;
        }
        public static Struct.Common.Prefectures GetPrefecture(string? ID)
        {
            if (ID == null) return Struct.Common.Prefectures.Unknown;
            if (ID.Length != 6 & int.TryParse(ID, out int code)) throw new ArgumentException("都道府県コード及び市区町村コードではありません。");
            code /= 10000;
            return code switch
            {
                1 => Struct.Common.Prefectures.Hokkaido,
                2 => Struct.Common.Prefectures.Aomori,
                _ => Struct.Common.Prefectures.Unknown
            };
        }
    }
}
