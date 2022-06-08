using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MisakiEQ.Struct
{
    public class EarthQuake
    {
        public static Common.Intensity GetP2PIntensity(int value)
        {
            return value switch
            {
                10 => Common.Intensity.Int1,
                20 => Common.Intensity.Int2,
                30 => Common.Intensity.Int3,
                40 => Common.Intensity.Int4,
                45 => Common.Intensity.Int5Down,
                46 => Common.Intensity.Int5Over,
                50 => Common.Intensity.Int5Up,
                55 => Common.Intensity.Int6Down,
                60 => Common.Intensity.Int6Up,
                70 => Common.Intensity.Int7,
                _ => Common.Intensity.Unknown,
            };
        }

        public static string TypeToString(EarthQuakeType type)
        {
            return type switch
            {
                EarthQuakeType.Unknown => "不明",
                EarthQuakeType.ScalePrompt => "震度速報",
                EarthQuakeType.Destination => "震源に関する情報",
                EarthQuakeType.ScaleAndDestination => "震度・震源に関する情報",
                EarthQuakeType.DetailScale => "各地の震度に関する情報",
                EarthQuakeType.Foreign => "遠地地震に関する情報",
                EarthQuakeType.Other => "その他に関する情報",
                _ => type.ToString(),
            };
        }
        public static string CorrectToString(EarthQuakeCorrect correct)
        {
            return correct switch
            {
                EarthQuakeCorrect.None => "なし",
                EarthQuakeCorrect.Unknown => "不明",
                EarthQuakeCorrect.DestinationOnly => "震源",
                EarthQuakeCorrect.ScaleOnly => "震度",
                EarthQuakeCorrect.ScaleAndDestination => "震度・震源",
                _ => correct.ToString(),
            };
        }
        public static string DomesticToString(EarthQuakeDomesticTsunami domestic)
        {
            return domestic switch
            {
                EarthQuakeDomesticTsunami.None => "津波の心配はありません。",
                EarthQuakeDomesticTsunami.Unknown => "津波情報は不明です。",
                EarthQuakeDomesticTsunami.Checking => "津波は現在調査中です。",
                EarthQuakeDomesticTsunami.NonEffective => "若干の海面変動があります。",
                EarthQuakeDomesticTsunami.Watch => "津波注意報が発令されています。",
                EarthQuakeDomesticTsunami.Warning => "津波警報が発令されています。",
                _ => $"コード:{domestic}",
            };
        }
        public static string ForeignToString(EarthQuakeForeignTsunami foreign)
        {
            return foreign switch
            {
                EarthQuakeForeignTsunami.None => "海外での津波はありません。",
                EarthQuakeForeignTsunami.Unknown => "海外での津波の有無は不明です。",
                EarthQuakeForeignTsunami.Checking => "海外での津波は現在調査中です。",
                EarthQuakeForeignTsunami.NonEffectiveNearby => "震源の近傍で小さな津波の可能性がありますが、被害の心配はありません。",
                EarthQuakeForeignTsunami.WarningNearby => "震源の近傍で津波の可能性があります。",
                EarthQuakeForeignTsunami.WarningPacofoc => "太平洋で津波の可能性があります。",
                EarthQuakeForeignTsunami.WarningPacofocWide => "太平洋の広域で津波の可能性があります。",
                EarthQuakeForeignTsunami.WarningIndian => "インド洋で津波の可能性があります。",
                EarthQuakeForeignTsunami.WarningIndianWide => "インド洋の広域で津波の可能性があります。",
                EarthQuakeForeignTsunami.Potential => "一般的にこの規模は津波の可能性があります。",
                _ => $"コード:{foreign}",
            };
        }
        public static EarthQuake GetData(Background.API.EQInfo.JSON.Root data, EarthQuake? from = null)
        {
            if (data.code != 551) throw new ArgumentException($"このコード値({data.code})は地震情報(551)と互換性がありません。");
            if (data.earthquake == null) throw new ArgumentNullException("data.earthquake",$"Earthquakeのクラスオブジェクトがnullです。");
            if (from == null) from = new();
            from.EventID = data.id;
            if (DateTime.TryParse(data.time, out DateTime date))
            {
                from.CreatedAt = date;
            }
            else
            {
                Log.Logger.GetInstance().Warn($"time を DateTime型に変換できませんでした。 値 = \"{data.time}\"");
            }
            from.Issue.Source = data.issue.source;
            from.Issue.Time = Common.GetDateTime(data.issue.time);
            if (Enum.TryParse(typeof(EarthQuakeType), data.issue.type, out var type))
            {
                if (type != null)
                {
                    from.Issue.Type = (EarthQuakeType)type;
                }
                else
                {
                    Log.Logger.GetInstance().Warn($"Issue.Typeを変換した後nullが返されました。 値 = {data.issue.type}");
                    from.Issue.Type = EarthQuakeType.Unknown;
                }
            }
            else
            {
                Log.Logger.GetInstance().Warn($"Issue.Typeの変換に失敗しました。 値 = {data.issue.type}");
                from.Issue.Type = EarthQuakeType.Unknown;
            }

            if (Enum.TryParse(typeof(EarthQuakeCorrect), data.issue.correct, out type))
            {
                if (type != null)
                {
                    from.Issue.Correct= (EarthQuakeCorrect)type;
                }
                else
                {
                    Log.Logger.GetInstance().Warn($"Issue.Correctを変換した後nullが返されました。 値 = {data.issue.correct}");
                    from.Issue.Correct = EarthQuakeCorrect.Unknown;
                }
            }
            else
            {
                Log.Logger.GetInstance().Warn($"Issue.Correctの変換に失敗しました。 値 = {data.issue.correct}");
                from.Issue.Correct = EarthQuakeCorrect.Unknown;
            }
            from.Details.Depth = data.earthquake.hypocenter.depth;
            from.Details.Hypocenter = data.earthquake.hypocenter.name;
            from.Details.HypocenterCode = int.MinValue;
            from.Details.Magnitude = data.earthquake.hypocenter.magnitude;
            from.Details.MaxIntensity = GetP2PIntensity(data.earthquake.maxScale);

            if (Enum.TryParse(typeof(EarthQuakeDomesticTsunami), data.earthquake.domesticTsunami, out type))
            {
                if (type != null)
                {
                    from.Details.DomesticTsunami = (EarthQuakeDomesticTsunami)type;
                }
                else
                {
                    Log.Logger.GetInstance().Warn($"EarthQuake.DomesticTsunamiを変換した後nullが返されました。 値 = {data.earthquake.domesticTsunami}");
                    from.Details.DomesticTsunami = EarthQuakeDomesticTsunami.Unknown;
                }
            }
            else
            {
                Log.Logger.GetInstance().Warn($"EarthQuake.DomesticTsunamiの変換に失敗しました。 値 = {data.earthquake.domesticTsunami}");
                from.Details.DomesticTsunami = EarthQuakeDomesticTsunami.Unknown;
            }
            if (Enum.TryParse(typeof(EarthQuakeForeignTsunami), data.earthquake.foreignTsunami, out type))
            {
                if (type != null)
                {
                    from.Details.ForeignTsunami = (EarthQuakeForeignTsunami)type;
                }
                else
                {
                    Log.Logger.GetInstance().Warn($"EarthQuake.ForeignTsunamiを変換した後nullが返されました。 値 = {data.earthquake.foreignTsunami}");
                    from.Details.ForeignTsunami = EarthQuakeForeignTsunami.Unknown;
                }
            }
            else
            {
                Log.Logger.GetInstance().Warn($"EarthQuake.ForeignTsunamiの変換に失敗しました。 値 = {data.earthquake.foreignTsunami}");
                from.Details.ForeignTsunami = EarthQuakeForeignTsunami.Unknown;
            }
            from.Details.OriginTime = Common.GetDateTime(data.earthquake.time);
            from.Details.Location.Lat = data.earthquake.hypocenter.latitude;
            from.Details.Location.Long = data.earthquake.hypocenter.longitude;
            from.Details.Points.Clear();
            from.Details.PrefIntensity.Clear();
            for (int i = 0; i < data.points.Count; i++)
            {
                var point = data.points[i];
                var args = new cEarthQuake.Point
                {
                    Address = point.addr,
                    IsArea = point.isArea,
                    Scale = GetP2PIntensity(point.scale),
                    Pref = Common.StringToPrefectures(point.pref)
                };
                if (from.Details.PrefIntensity.GetIntensity(args.Pref) < args.Scale)
                {
                    from.Details.PrefIntensity.SetIntensity(args.Pref,args.Scale);
                }
                from.Details.Points.Add(args);
            }
            return from;
        }
        
        public enum EarthQuakeType
        {
            Unknown,
            ScalePrompt,
            Destination,
            ScaleAndDestination,
            DetailScale,
            Foreign,
            Other
        }
        public enum EarthQuakeCorrect
        {
            None,
            Unknown,
            ScaleOnly,
            DestinationOnly,
            ScaleAndDestination
        }
        public enum EarthQuakeDomesticTsunami
        {
            None,
            Unknown,
            Checking,
            NonEffective,
            Watch,
            Warning
        }
        public enum EarthQuakeForeignTsunami
        {
            None,
            Unknown,
            Checking,
            NonEffectiveNearby,
            WarningNearby,
            WarningPacofoc,
            WarningPacofocWide,
            WarningIndian,
            WarningIndianWide,
            Potential
        }
        public string EventID=string.Empty;
        public DateTime CreatedAt=DateTime.MinValue;
        public cEarthQuake.Issue Issue = new();
        public cEarthQuake.Details Details = new();
    }

}

namespace MisakiEQ.Struct.cEarthQuake
{
    public class Issue
    {
        public string Source=string.Empty;
        public DateTime Time=DateTime.MinValue;
        public EarthQuake.EarthQuakeType Type=EarthQuake.EarthQuakeType.Unknown;
        public EarthQuake.EarthQuakeCorrect Correct = EarthQuake.EarthQuakeCorrect.Unknown;
    }
    public class Details
    {
        public DateTime OriginTime=DateTime.MinValue;
        public string Hypocenter = string.Empty;
        public int HypocenterCode = -200;
        public Common.Location Location = new();
        public int Depth = -1;
        public double Magnitude = double.NaN;
        public Common.Intensity MaxIntensity = Common.Intensity.Unknown;
        public EarthQuake.EarthQuakeDomesticTsunami DomesticTsunami = EarthQuake.EarthQuakeDomesticTsunami.Unknown;
        public EarthQuake.EarthQuakeForeignTsunami ForeignTsunami = EarthQuake.EarthQuakeForeignTsunami.Unknown;
        public List<Point> Points = new();
        public Prefs PrefIntensity = new();
    }
    public class Point
    {
        public Common.Prefectures Pref = Common.Prefectures.Unknown;
        public string Address = string.Empty;
        public bool IsArea = false;
        public Common.Intensity Scale = Common.Intensity.Unknown;
    }

    public class Prefs
    {
        private readonly Common.Intensity[] Intensity = new Common.Intensity[Enum.GetNames(typeof(Common.Prefectures)).Length];
        public Common.Intensity GetIntensity(Common.Prefectures pref)
        {
            if (pref == Common.Prefectures.Unknown) return Common.Intensity.Unknown;
            return Intensity[(int)pref];
        }
        public void SetIntensity(Common.Prefectures pref,Common.Intensity intensity)
        {
            if (pref == Common.Prefectures.Unknown) return;
            Intensity[(int)pref] = intensity;
        }
        public void Clear()
        {
            for (int i = 0; i < Enum.GetNames(typeof(Common.Prefectures)).Length; i++)
                Intensity[i] = Common.Intensity.Int0;
        }
        public List<IntensityPrefList> GetIntensityPrefectures()
        {
            var IntensityList = new Common.Intensity[]{Common.Intensity.Int7, Common.Intensity.Int6Up, Common.Intensity.Int6Down, Common.Intensity.Int5Up, Common.Intensity.Int5Down,Common.Intensity.Int5Over,
                Common.Intensity.Int4, Common.Intensity.Int3, Common.Intensity.Int2, Common.Intensity.Int1 };
            var list = new List<IntensityPrefList>();
            for (int j = 0; j < IntensityList.Length; j++) {
                
                var li = new IntensityPrefList();
                for (int i = 0; i < Enum.GetNames(typeof(Common.Prefectures)).Length; i++)
                    if (Intensity[i] == IntensityList[j]) li.Prefectures.Add((Common.Prefectures)i);
                li.Intensity = IntensityList[j];
                if (li.Prefectures.Count == 0) continue;
                list.Add(li);
            }
            return list;
        }
        public class IntensityPrefList
        {
            public Common.Intensity Intensity;
            public List<Common.Prefectures> Prefectures=new();
        }
    }
}