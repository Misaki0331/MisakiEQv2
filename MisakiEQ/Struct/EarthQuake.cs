using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MisakiEQ.Struct
{
    public class EarthQuake
    {
        /// <summary>
        /// <para>P2Pの震度情報をEnumに変換します。</para>
        /// </summary>
        /// <param name="value">P2Pの震度値</param>
        /// <returns><para>Common.Intensityの列挙型</para>
        /// <para>失敗時 Common.Intensity.Unknown で返されます。</para></returns>
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
        /// <summary>
        /// <para>地震情報の種類を文字列に変換します。</para>
        /// </summary>
        /// <param name="type">変換したい列挙型Enum</param>
        /// <returns><para>日本語の文字列が返されます。</para></returns>
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
        /// <summary>
        /// <para>訂正・修正の種類を文字列に変換します。</para>
        /// </summary>
        /// <param name="correct">変換したい訂正・修正の種類</param>
        /// <returns>日本語の文字列が返されます。</returns>
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
        /// <summary>
        /// <para>国内地震による津波の影響の種類を文字列に変換します。</para>
        /// </summary>
        /// <param name="domestic">津波の影響の種類</param>
        /// <returns>日本語の文字列が返されます</returns>
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
        /// <summary>
        /// <para>海外地震、もしくは噴火等による津波情報を文字列に変換します。</para>
        /// </summary>
        /// <param name="foreign">海外からの津波の影響の種類</param>
        /// <returns>日本語の文字列が返されます。</returns>
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
        /// <summary>
        /// <para>apiのjsonデータ単体から地震情報を汎用クラスに代入します。</para>
        /// <para>※この関数は地震情報のみ対応しています。</para>
        /// </summary>
        /// <param name="data">jsonデータ単体</param>
        /// <param name="from"><para>書き換え元の地震情報汎用クラス</para>
        /// <para>nullの場合は新規クラスを返します。</para></param>
        /// <returns>地震情報汎用クラス</returns>
        /// <exception cref="ArgumentException">地震情報と互換性が無いjsonデータが渡された時に発生します。</exception>
        /// <exception cref="ArgumentNullException">jsonデータの中身がnullの場合に発生します。</exception>
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
                Log.Instance.Warn($"time を DateTime型に変換できませんでした。 値 = \"{data.time}\"");
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
                    Log.Instance.Warn($"Issue.Typeを変換した後nullが返されました。 値 = {data.issue.type}");
                    from.Issue.Type = EarthQuakeType.Unknown;
                }
            }
            else
            {
                Log.Instance.Warn($"Issue.Typeの変換に失敗しました。 値 = {data.issue.type}");
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
                    Log.Instance.Warn($"Issue.Correctを変換した後nullが返されました。 値 = {data.issue.correct}");
                    from.Issue.Correct = EarthQuakeCorrect.Unknown;
                }
            }
            else
            {
                Log.Instance.Warn($"Issue.Correctの変換に失敗しました。 値 = {data.issue.correct}");
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
                    Log.Instance.Warn($"EarthQuake.DomesticTsunamiを変換した後nullが返されました。 値 = {data.earthquake.domesticTsunami}");
                    from.Details.DomesticTsunami = EarthQuakeDomesticTsunami.Unknown;
                }
            }
            else
            {
                Log.Instance.Warn($"EarthQuake.DomesticTsunamiの変換に失敗しました。 値 = {data.earthquake.domesticTsunami}");
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
                    Log.Instance.Warn($"EarthQuake.ForeignTsunamiを変換した後nullが返されました。 値 = {data.earthquake.foreignTsunami}");
                    from.Details.ForeignTsunami = EarthQuakeForeignTsunami.Unknown;
                }
            }
            else
            {
                Log.Instance.Warn($"EarthQuake.ForeignTsunamiの変換に失敗しました。 値 = {data.earthquake.foreignTsunami}");
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
        /// <summary>
        /// 気象庁から発表される地震情報の種類です。
        /// </summary>
        public enum EarthQuakeType
        {
            /// <summary>
            /// 不明
            /// </summary>
            Unknown,
            /// <summary>
            /// <para>震度速報</para>
            /// <para>発生条件 : 震度3以上</para>
            /// <para>地震発生約１分半後に、震度３以上を観測した地域名（全国を188地域に区分）と地震の揺れの検知時刻を速報。</para>
            /// </summary>
            ScalePrompt,
            /// <summary>
            /// <para>震源に関する情報</para>
            /// <para>発生条件 : 震度3以上 かつ 津波警報・注意報が発表されていない</para>
            /// <para>「津波の心配がない」または「若干の海面変動があるかもしれないが被害の心配はない」旨を付加して、<br/>
            /// 地震の発生場所（震源）やその規模（マグニチュード）を発表。</para>
            /// </summary>
            Destination,
            /// <summary>
            /// <para>震度・震源に関する情報</para>
            /// <para>発生条件 : 以下のいずれかの内容に当てはまる場合<br/>
            /// ・震度３以上<br/>・津波警報・注意報発表<br/>
            /// ・若干の海面変動が予想される場合<br/>
            /// ・緊急地震速報（警報）を発表した場合</para>
            /// <para>地震の発生場所（震源）やその規模（マグニチュード）、震度３以上の地域名と市町村毎の観測した震度を発表。<br/>
            /// 震度５弱以上と考えられる地域で、震度を入手していない地点がある場合は、その市町村名を発表。</para>
            /// </summary>
            ScaleAndDestination,
            /// <summary>
            /// <para>各地の震度に関する情報</para>
            /// <para>発生条件 : 震度1以上</para>
            /// <para>震度１以上を観測した地点のほか、地震の発生場所（震源）やその規模（マグニチュード）を発表。<br/>
            /// 震度５弱以上と考えられる地域で、震度を入手していない地点がある場合は、その地点名を発表。<br/>
            /// ※ 地震が多数発生した場合には、震度３以上の地震についてのみ発表し、震度２以下の地震については、<br/>
            /// その発生回数を「その他の情報（地震回数に関する情報）」で発表します。</para>
            /// <para>※MisakiEQには「その他の情報」には対応していません。</para>
            /// </summary>
            DetailScale,
            /// <summary>
            /// <para>遠地地震に関する情報</para>
            /// <para>発生条件 : 国外で発生した地震について以下のいずれかを満たした場合等<br/>
            /// ・マグニチュード7.0以上<br/>
            /// ・都市部など著しい被害が発生する可能性がある地域で規模の大きな地震を観測した場合</para>
            /// <para>地震の発生時刻、発生場所（震源）やその規模（マグニチュード）を概ね30分以内に発表。<br/>
            /// 日本や国外への津波の影響に関しても記述して発表。</para>
            /// </summary>
            Foreign,
            /// <summary>
            /// <para>その他の情報</para>
            /// <para>発生条件 : 顕著な地震の震源要素を更新した場合や地震が多発した場合など</para>
            /// <para>顕著な地震の震源要素更新のお知らせや地震が多発した場合の震度１以上を観測した地震回数情報等を発表。</para>
            /// <para>※MisakiEQには「その他の情報」には対応していません。</para>
            /// </summary>
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
        /// <summary>
        /// 地震による国内の津波に関する情報
        /// </summary>
        public enum EarthQuakeDomesticTsunami
        {
            /// <summary>
            /// 津波の心配なし
            /// </summary>
            None,
            /// <summary>
            /// 不明
            /// </summary>
            Unknown,
            /// <summary>
            /// 調査中
            /// </summary>
            Checking,
            /// <summary>
            /// 若干の海面変動あり
            /// </summary>
            NonEffective,
            /// <summary>
            /// 津波注意報発表
            /// </summary>
            Watch,
            /// <summary>
            /// 津波警報もしくは大津波警報を発表
            /// </summary>
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
        /// <summary>
        /// <para>イベントID</para>
        /// <para>情報毎に現れるランダムなIDです。</para>
        /// </summary>
        public string EventID=string.Empty;
        /// <summary>
        /// <para>情報作成時刻</para>
        /// <para>P2P地震情報API内のデータベース内に作られた時間です。</para>
        /// <para>存在しなければ DateTime.MinValue が返されます。</para>
        /// </summary>
        public DateTime CreatedAt=DateTime.MinValue;
        /// <summary>
        /// 訂正・修正情報です
        /// </summary>
        public cEarthQuake.Issue Issue = new();
        /// <summary>
        /// 地震の詳細情報です。
        /// </summary>
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
        /// <summary>
        /// 地震の発生時刻
        /// </summary>
        public DateTime OriginTime=DateTime.MinValue;
        /// <summary>
        /// <para>震源地情報</para>
        /// <para>※震度速報では空の状態です。</para>
        /// </summary>
        public string Hypocenter = string.Empty;
        //ToDo: 震源地情報から震源地コードに変換する関数を作る
        /// <summary>
        /// <para>震源地コード</para>
        /// <para>※まだ未対応です。</para>
        /// </summary>
        public int HypocenterCode = -200;
        /// <summary>
        /// <para>震央の緯度経度から示される座標情報</para>
        /// <para>※震度速報では空の状態です。</para>
        /// </summary>
        public Common.Location Location = new();
        /// <summary>
        /// <para>震源の深さ</para>
        /// <para>※不明または震度速報の場合は-1が返されます。</para>
        /// </summary>
        public int Depth = -1;
        /// <summary>
        /// <para>地震の規模を示すマグニチュード</para>
        /// <para>※不明または震度速報の場合はNaNが返されます。</para>
        /// </summary>
        public double Magnitude = double.NaN;
        /// <summary>
        /// <para>最大震度</para>
        /// <para>※震源に関する情報の場合は不明と返されます。</para>
        /// </summary>
        public Common.Intensity MaxIntensity = Common.Intensity.Unknown;
        /// <summary>
        /// <para>国内で地震が発生した際の津波情報</para>
        /// <para>各地の震度に関する情報もしくは震度・震源に関する情報以外は不明もしくは調査中で返されます。</para>
        /// </summary>
        public EarthQuake.EarthQuakeDomesticTsunami DomesticTsunami = EarthQuake.EarthQuakeDomesticTsunami.Unknown;
        /// <summary>
        /// <para>遠地で地震・噴火による津波情報</para>
        /// <para>遠地地震の情報以外では不明が返されます。</para>
        /// </summary>
        public EarthQuake.EarthQuakeForeignTsunami ForeignTsunami = EarthQuake.EarthQuakeForeignTsunami.Unknown;
        /// <summary>
        /// <para>各地点の震度情報</para>
        /// </summary>
        public List<Point> Points = new();
        /// <summary>
        /// <para>都道府県ごとの震度情報</para>
        /// </summary>
        public Prefs PrefIntensity = new();
    }
    public class Point
    {
        /// <summary>
        /// 観測点の都道府県
        /// </summary>
        public Common.Prefectures Pref = Common.Prefectures.Unknown;
        /// <summary>
        /// 震度観測点名称
        /// </summary>
        public string Address = string.Empty;
        /// <summary>
        /// 区域名かどうか
        /// </summary>
        public bool IsArea = false;
        /// <summary>
        /// 震度情報
        /// </summary>
        public Common.Intensity Scale = Common.Intensity.Unknown;
    }

    public class Prefs
    {
        private readonly Common.Intensity[] Intensity = new Common.Intensity[Enum.GetNames(typeof(Common.Prefectures)).Length];
        /// <summary>
        /// 都道府県の震度情報を返します。
        /// </summary>
        /// <param name="pref">都道府県</param>
        /// <returns>震度</returns>
        public Common.Intensity GetIntensity(Common.Prefectures pref)
        {
            if (pref == Common.Prefectures.Unknown) return Common.Intensity.Unknown;
            return Intensity[(int)pref];
        }
        /// <summary>
        /// 都道府県の震度情報を設定します。<br/>
        /// 注意:この関数は使用しません。
        /// </summary>
        /// <param name="pref">設定する都道府県</param>
        /// <param name="intensity">都道府県に設定する最大震度</param>
        internal void SetIntensity(Common.Prefectures pref,Common.Intensity intensity)
        {
            if (pref == Common.Prefectures.Unknown) return;
            Intensity[(int)pref] = intensity;
        }
        /// <summary>
        /// 各都道府県の震度情報をリセットします。<br/>
        /// 注意:この関数は使用しません。
        /// </summary>
        internal void Clear()
        {
            for (int i = 0; i < Enum.GetNames(typeof(Common.Prefectures)).Length; i++)
                Intensity[i] = Common.Intensity.Int0;
        }
        /// <summary>
        /// 震度が高い順番から都道府県のリストを返します。
        /// </summary>
        /// <returns>震度と各都道府県のリスト</returns>
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
            /// <summary>
            /// 震度値
            /// </summary>
            public Common.Intensity Intensity;
            /// <summary>
            /// 震度に値する都道府県リスト
            /// </summary>
            public List<Common.Prefectures> Prefectures=new();
        }
    }
}