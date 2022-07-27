using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KyoshinMonitorLib;
using KyoshinMonitorLib.Images;

namespace MisakiEQ.Lib.KyoshinAPI
{
    public static class KyoshinObervation
    {
        static ObservationPoint[] Points=Array.Empty<ObservationPoint>();
        public static void Init()
        {
            string tempFileName = Path.GetTempFileName();
            using BinaryWriter sw=new(File.OpenWrite(tempFileName));
            sw.Write(Properties.Resources.ShindoObsPoints_mpk);
            sw.Close();
            ObservationPoint[] points = ObservationPoint.LoadFromMpk(tempFileName,true);
            Points = points;
            File.Delete(tempFileName);
        }
        public static async Task<AnalysisResult> GetPoints(Background.API.KyoshinAPI.KyoshinAPI.KyoshinImageType type)
        {
            Stopwatch sw = new();
            sw.Start();
            var img= await Background.APIs.GetInstance().KyoshinAPI.GetImage(type);
            List<AnalysisPoint> result = new();
            var re = new AnalysisResult();
            if (img == null) return re;
            if (img.Height != 400 || img.Width != 352) return re;
            var ImageData = (Bitmap)img;
            var UserLAL = new Struct.Common.LAL(Background.APIs.GetInstance().KyoshinAPI.Config.UserLong, Background.APIs.GetInstance().KyoshinAPI.Config.UserLat);
            if (ImageData == null) return re;
            double toNearDistance = double.PositiveInfinity;
            for (int i = 0; i < Points.Length; i++)
            {
                var d = Points[i];
                if (d.Point != null)
                {
                    var c=ImageData.GetPixel(d.Point.Value.X, d.Point.Value.Y);
                    if (c.A == 0) continue;
                    double v = Lib.KyoshinLib.GetValue(c);
                    var data = new AnalysisPoint();
                    switch (type)
                    {
                        case Background.API.KyoshinAPI.KyoshinAPI.KyoshinImageType.PSWaveImg:
                            break;
                        case Background.API.KyoshinAPI.KyoshinAPI.KyoshinImageType.jma_s:
                        case Background.API.KyoshinAPI.KyoshinAPI.KyoshinImageType.jma_b:
                        case Background.API.KyoshinAPI.KyoshinAPI.KyoshinImageType.EstShindoImg:
                            v=Lib.KyoshinLib.GetIntensity(c);
                            data.Format = "震度 <VALUE>";
                            break;
                        case Background.API.KyoshinAPI.KyoshinAPI.KyoshinImageType.acmap_s:
                        case Background.API.KyoshinAPI.KyoshinAPI.KyoshinImageType.acmap_b:
                            v = Lib.KyoshinLib.GetPGA(c);
                            data.Format = "PGA <VALUE> gal";
                            data.UnitName = "gal";
                            break;
                        case Background.API.KyoshinAPI.KyoshinAPI.KyoshinImageType.dcmap_s:
                        case Background.API.KyoshinAPI.KyoshinAPI.KyoshinImageType.dcmap_b:
                            v = Lib.KyoshinLib.GetPGD(c);
                            data.Format = "PGD <VALUE> cm";
                            data.UnitName = "cm";
                            break;
                        default:
                            v = Lib.KyoshinLib.GetPGV(c);
                            data.Format = "PGV <VALUE> cm/s";
                            data.UnitName = "cm/s";
                            break;
                    }
                    var dis = UserLAL.GetDistanceTo(new(d.OldLocation.Longitude, d.OldLocation.Latitude));
                    data.Region = d.Region + " " + d.Name;
                    data.Value = v;
                    if (dis < toNearDistance)
                    {
                        toNearDistance = dis;
                        re.NearPoint = data;
                    }
                    result.Add(data);
                }
            }
            result.Sort((a, b) => (int)((b.Value*100.0 - a.Value*100.0)));
            re.Count = result.Count;
            re.Point = result;
            sw.Stop();

            return re;
        }
        public class AnalysisResult
        {
            public List<AnalysisPoint> Point=new();
            public AnalysisPoint NearPoint = new();
            public int Count = 0;
        }
        public class AnalysisPoint
        {
            public string Region="";
            public double Value = 0;
            public string UnitName = "";
            public string Format = "";
            public string String { get { return Format.Replace("<VALUE>", $"{Value:0.00}"); } }
        }

    }
}
