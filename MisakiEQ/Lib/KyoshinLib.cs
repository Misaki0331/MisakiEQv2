using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MisakiEQ.Lib
{
    internal class KyoshinLib
    {
        private static double PosXtoLon(double x,bool IsExtend)
        {
            var val = x * 0.0493;
            if (IsExtend) return val + 122.52675;
            return val + 128.62400;
        }
        private static double PosYtoLat(double y, bool IsExtend)
        {
            var val = y * 0.04065;
            if (IsExtend) return 32.11100 - val;
            return 46.19100 - val;
        }
        private static double LontoPosX(double lon,bool IsExtend)
        {
            var val = (IsExtend ? -122.52675 : -128.624) + lon;
            return val / 0.0493;
        }
        private static double LattoPosY(double lat,bool IsExtend)
        {
            var val = (IsExtend ? -32.111 : -46.191) + lat;
            return -val / 0.04065;

        }
        public static Struct.Common.LAL KyoshinMapToLAL(Struct.Common.Point Map)
        {
            try
            {
                bool IsExtend = false;
                if (Map.Y > 38 && Map.Y < 208 && Map.X >= 0 && Map.X <= 110) IsExtend = true;
                else if (Map.Y > 38 && Map.Y < 135 && Map.X >= 110 && Map.X < 172) IsExtend = true;
                else if ((Map.X > 110 && Map.X < 172 && Map.Y >= 135 && Map.Y < 208) &&
                    (Map.X - 110) * 72 / 61 < (73 - (Map.Y - 135))) IsExtend = true;
                Struct.Common.LAL Pos = new(PosXtoLon(Map.X, IsExtend), PosYtoLat(Map.Y, IsExtend));
                return Pos;
            }
            catch
            {
                return new Struct.Common.LAL(double.NaN, double.NaN);
            }
        }

        public static Struct.Common.Point LALtoKyoshinMap(Struct.Common.LAL Map)
        {
            try
            {
                bool IsExtend = false;
                if (LattoPosY(Map.Lat,true) > 38 && LattoPosY(Map.Lat,true) < 208 && LontoPosX(Map.Lon,true) >= 0 && LontoPosX(Map.Lon,true) <= 110) IsExtend = true;
                else if (LattoPosY(Map.Lat,true) > 38 && LattoPosY(Map.Lat,true) < 135 && LontoPosX(Map.Lon,true) >= 110 && LontoPosX(Map.Lon,true) < 172) IsExtend = true;
                else if ((LontoPosX(Map.Lon,true) > 110 && LontoPosX(Map.Lon,true) < 172 && LattoPosY(Map.Lat,true) >= 135 && LattoPosY(Map.Lat,true) < 208) &&
                    (LontoPosX(Map.Lon,true) - 110) * 72 / 61 < (73 - (LattoPosY(Map.Lat,true) - 135))) IsExtend = true;
                Struct.Common.Point Pos = new(LontoPosX(Map.Lon,IsExtend), LattoPosY(Map.Lat,IsExtend));
                return Pos;
            }
            catch
            {
                return new Struct.Common.Point(double.NaN, double.NaN);
            }
        }

        //引用元
        //多項式補間を使用して強震モニタ画像から数値データを決定する - NoneType1
        //https://qiita.com/NoneType1/items/a4d2cf932e20b56ca444
        private static double GetValue(Color col)
        {
            int max = Math.Max(col.R, Math.Max(col.G, col.B)), min = Math.Min(col.R, Math.Min(col.G, col.B));
            double h = col.GetHue() / 360.0, s = (max == 0) ? 0 : 1d - (1d * min / max), v = max / 255d, p = 0;
            if (v > 0.1 && s > 0.75)
            {
                if (h > 0.1476) p = 280.31 * Math.Pow(h, 6) - 916.05 * Math.Pow(h, 5) + 1142.6 * Math.Pow(h, 4) - 709.95 * Math.Pow(h, 3) + 234.65 * Math.Pow(h, 2) - 40.27 * h + 3.2217;
                if (h <= 0.1476 && h > 0.001) p = 151.4 * Math.Pow(h, 4) - 49.32 * Math.Pow(h, 3) + 6.753 * Math.Pow(h, 2) - 2.481 * h + 0.9033;
                if (h <= 0.001) p = -0.005171 * Math.Pow(v, 2) - 0.3282 * v + 1.2236;
            }
            if (p < 0) p = 0;
            return p;
        }
        public static double GetIntensity(Color col)
        {
            return 10.0 * GetValue(col) - 3.0;
        }
        public static double GetPGA(Color col)
        {
            return Math.Pow(10, 5.0 * GetValue(col) - 2.0);
        }
        public static double GetPGV(Color col)
        {
            return Math.Pow(10, 5.0 * GetValue(col) - 3.0);
        }
        public static double GetPGD(Color col)
        {
            return Math.Pow(10, 5.0 * GetValue(col) - 4.0);
        }
    }
}
