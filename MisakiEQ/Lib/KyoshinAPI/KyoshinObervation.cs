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
        public static async Task<AnalysisResult> GetPoints(Background.API.KyoshinAPI.KyoshinAPI.KyoshinImageType type, bool IsNeedSort=false)
        {
            try
            {
                Stopwatch sw = new();
                sw.Start();
                var img = await Background.APIs.GetInstance().KyoshinAPI.GetImage(type);
                List<AnalysisPoint> result = new();
                var re = new AnalysisResult();
                if (img == null) throw new ArgumentNullException(nameof(img), "画像が取得できませんでした。");
                if (img.Size != new Size(352,400))
#pragma warning disable CA2208 
                    throw new ArgumentOutOfRangeException(
                        nameof(img.Size), $"サイズが一致しません。352x400である必要がありますが、この画像は{img.Size.Width}x{img.Size.Height}です。");
#pragma warning restore CA2208 
                var ImageData = (Bitmap)img;
                var UserLAL = new Struct.Common.LAL(Background.APIs.GetInstance().KyoshinAPI.Config.UserLong, Background.APIs.GetInstance().KyoshinAPI.Config.UserLat);
                if (ImageData == null) throw new ArgumentNullException(nameof(ImageData), "Bitmapに変換できませんでした。");
                switch (type)
                {
                    case Background.API.KyoshinAPI.KyoshinAPI.KyoshinImageType.EstShindoImg:
                        re.Graph.ShortTitle = "EEW";
                        break;
                    case Background.API.KyoshinAPI.KyoshinAPI.KyoshinImageType.jma_s:
                    case Background.API.KyoshinAPI.KyoshinAPI.KyoshinImageType.acmap_s:
                    case Background.API.KyoshinAPI.KyoshinAPI.KyoshinImageType.vcmap_s:
                    case Background.API.KyoshinAPI.KyoshinAPI.KyoshinImageType.dcmap_s:
                        re.Graph.ShortTitle = "地表";
                        break;
                    case Background.API.KyoshinAPI.KyoshinAPI.KyoshinImageType.jma_b:
                    case Background.API.KyoshinAPI.KyoshinAPI.KyoshinImageType.acmap_b:
                    case Background.API.KyoshinAPI.KyoshinAPI.KyoshinImageType.vcmap_b:
                    case Background.API.KyoshinAPI.KyoshinAPI.KyoshinImageType.dcmap_b:
                        re.Graph.ShortTitle = "地中";
                        break;
                    case Background.API.KyoshinAPI.KyoshinAPI.KyoshinImageType.rsp0125_s:
                    case Background.API.KyoshinAPI.KyoshinAPI.KyoshinImageType.rsp0125_b:
                        re.Graph.ShortTitle = "⅛Hz";
                        break;
                    case Background.API.KyoshinAPI.KyoshinAPI.KyoshinImageType.rsp0250_s:
                    case Background.API.KyoshinAPI.KyoshinAPI.KyoshinImageType.rsp0250_b:
                        re.Graph.ShortTitle = "¼Hz";
                        break;
                    case Background.API.KyoshinAPI.KyoshinAPI.KyoshinImageType.rsp0500_s:
                    case Background.API.KyoshinAPI.KyoshinAPI.KyoshinImageType.rsp0500_b:
                        re.Graph.ShortTitle = "½Hz";
                        break;
                    case Background.API.KyoshinAPI.KyoshinAPI.KyoshinImageType.rsp1000_s:
                    case Background.API.KyoshinAPI.KyoshinAPI.KyoshinImageType.rsp1000_b:
                        re.Graph.ShortTitle = "1Hz";
                        break;
                    case Background.API.KyoshinAPI.KyoshinAPI.KyoshinImageType.rsp2000_s:
                    case Background.API.KyoshinAPI.KyoshinAPI.KyoshinImageType.rsp2000_b:
                        re.Graph.ShortTitle = "2Hz";
                        break;
                    case Background.API.KyoshinAPI.KyoshinAPI.KyoshinImageType.rsp4000_s:
                    case Background.API.KyoshinAPI.KyoshinAPI.KyoshinImageType.rsp4000_b:
                        re.Graph.ShortTitle = "4Hz";
                        break;
                }
                switch (type)
                {

                    case Background.API.KyoshinAPI.KyoshinAPI.KyoshinImageType.rsp0125_s:
                    case Background.API.KyoshinAPI.KyoshinAPI.KyoshinImageType.rsp0125_b:
                    case Background.API.KyoshinAPI.KyoshinAPI.KyoshinImageType.rsp0250_s:
                    case Background.API.KyoshinAPI.KyoshinAPI.KyoshinImageType.rsp0250_b:
                    case Background.API.KyoshinAPI.KyoshinAPI.KyoshinImageType.rsp0500_s:
                    case Background.API.KyoshinAPI.KyoshinAPI.KyoshinImageType.rsp0500_b:
                    case Background.API.KyoshinAPI.KyoshinAPI.KyoshinImageType.rsp1000_s:
                    case Background.API.KyoshinAPI.KyoshinAPI.KyoshinImageType.rsp1000_b:
                    case Background.API.KyoshinAPI.KyoshinAPI.KyoshinImageType.rsp2000_s:
                    case Background.API.KyoshinAPI.KyoshinAPI.KyoshinImageType.rsp2000_b:
                    case Background.API.KyoshinAPI.KyoshinAPI.KyoshinImageType.rsp4000_s:
                    case Background.API.KyoshinAPI.KyoshinAPI.KyoshinImageType.rsp4000_b:
                        re.Graph.ColorOffset = 0.2;
                        re.Graph.Format = "0.00";
                        break;
                    case Background.API.KyoshinAPI.KyoshinAPI.KyoshinImageType.dcmap_s:
                    case Background.API.KyoshinAPI.KyoshinAPI.KyoshinImageType.dcmap_b:
                        re.Graph.ColorOffset = 0.4;
                        re.Graph.Format = "0.000";
                        break;
                    default:
                        re.Graph.ColorOffset = 0;
                        re.Graph.Format = "0.00";
                        break;
                }
                double toNearDistance = double.PositiveInfinity;
                double maxvalue = double.NegativeInfinity;
                for (int i = 0; i < Points.Length; i++)
                {
                    var d = Points[i];
                    
                    if (d.Point != null)
                    {
                        var c = ImageData.GetPixel(d.Point.Value.X, d.Point.Value.Y);
                        if (c.A == 0) continue;
                        double v = Lib.KyoshinLib.GetValue(c);
                        var data = new AnalysisPoint
                        {
                            PointColor = c,
                            RawValue = v
                        };
                        switch (type)
                        {
                            case Background.API.KyoshinAPI.KyoshinAPI.KyoshinImageType.PSWaveImg:
                                throw new ArgumentException(nameof(type), $"{type}は地点ポイントが必要ありません。");
                            case Background.API.KyoshinAPI.KyoshinAPI.KyoshinImageType.jma_s:
                            case Background.API.KyoshinAPI.KyoshinAPI.KyoshinImageType.jma_b:
                            case Background.API.KyoshinAPI.KyoshinAPI.KyoshinImageType.EstShindoImg:
                                v = Lib.KyoshinLib.GetIntensity(c);
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
                            re.NearDistance = dis;
                            toNearDistance = dis;
                            re.NearPoint = data;
                        }
                        if (maxvalue < v)
                        {
                            maxvalue = v;
                            re.MaxPoint = data;
                        }
                        result.Add(data);
                    }
                }
                if(IsNeedSort)result.Sort((a, b) => (int)((b.Value * 100.0 - a.Value * 100.0)));
                re.Count = result.Count;
                re.Point = result;
                sw.Stop();
                return re;
            }
            catch(Exception ex)
            {
                var re = new AnalysisResult()
                {
                    IsError = true,
                    ErrorMessage = ex.Message
                };
                Log.Instance.Warn($"{ex.GetType} - {ex.Message}");
                return re;
            }
        }
        public class AnalysisResult
        {
            /// <summary>各地点のデータ</summary>
            public List<AnalysisPoint> Point=new();
            /// <summary>最寄り地点のデータ</summary>
            public AnalysisPoint NearPoint = new();
            /// <summary>最大の値を観測した地点のデータ</summary>
            public AnalysisPoint MaxPoint = new();
            /// <summary>最寄り地点からの距離(km)</summary>
            public double NearDistance = double.NaN;
            /// <summary>グラフ用変数</summary>
            public GraphList Graph = new();
            /// <summary>解析済地点数</summary>
            public int Count = 0;
            /// <summary>エラーフラグ</summary>
            public bool IsError = false;
            /// <summary>エラーの原因</summary>
            public string ErrorMessage = string.Empty;
        }
        public class GraphList
        {
            /// <summary>短いタイトル</summary>
            public string ShortTitle = "";
            /// <summary>色オフセット</summary>
            public double ColorOffset = 0;
            /// <summary>数値フォーマット</summary>
            public string Format = "";
        }
        public class AnalysisPoint
        {
            /// <summary>場所名</summary>
            public string Region="";
            /// <summary>データ値</summary>
            public double Value = 0;
            /// <summary>データの単位</summary>
            public string UnitName = "";
            /// <summary>文字列のフォーマット</summary>
            public string Format = "";
            /// <summary>強震モニタ地点の色</summary>
            public Color PointColor = Color.FromArgb(0);
            /// <summary>0～1までの生の値</summary>
            public double RawValue = 0;
            public string String { get { return Format.Replace("<VALUE>", $"{Value:0.00}"); } }
        }

    }
}
