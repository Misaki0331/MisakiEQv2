using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MisakiEQ.Struct
{
    public class EEW
    {
        public static string InfoToString(InfomationLevel level)
        {
            return level switch
            {
                InfomationLevel.Default => "現在発表されていません。",
                InfomationLevel.Forecast => "緊急地震速報(予報)",
                InfomationLevel.Warning => "緊急地震速報(警報)",
                InfomationLevel.Cancelled => "緊急地震速報(キャンセル)",
                InfomationLevel.CancelledTest => "緊急地震速報(訓練取消)",
                InfomationLevel.Test => "緊急地震速報(訓練)",
                InfomationLevel.Unknown => "不明",
                _ => $"コード:{level}",
            };
        }
        /// <summary>
        /// Userと震源の直線距離を計算します。
        /// </summary>
        /// <param name="eew">緊急地震速報データ</param>
        /// <param name="User">ユーザーの場所</param>
        /// <returns>距離(km)</returns>
        public static double GetDistance(EEW eew, Common.LAL User)
        {
            try
            {
                double r = 6378136.59;//地球の半径
                var distance = new Common.LAL(eew.EarthQuake.Location.Long, eew.EarthQuake.Location.Lat).GetDistanceTo(User);//円周距離
                double di = Math.Sqrt(Math.Pow(Math.Abs(r * Math.Sin(Math.PI * 2 * (distance / (r * 2 * Math.PI)))), 2) +
                    Math.Pow(Math.Abs(r - eew.EarthQuake.Depth * 1000.0 - r * Math.Cos(Math.PI * 2 * (distance / (r * 2 * Math.PI)))), 2));//直線距離
                return di / 1000.0;//km換算
            }catch(Exception ex)
            {
                Log.Instance.Error(ex);
                return double.NaN;
            }
        }

        /// <summary>
        /// <para>apiのjsonデータ単体からの緊急地震速報を汎用クラスに代入します。</para>
        /// </summary>
        /// <param name="Data">jsonデータ単体</param>
        /// <param name="from"><para>書き換え元の緊急地震速報汎用クラス</para>
        /// <para>nullの場合は新規クラスを返します。</para></param>
        /// <returns>緊急地震速報汎用クラス</returns>
        /// <exception cref="ArgumentNullException">jsonデータの中身がnullの場合に発生します。</exception>
        public static EEW GetData(Background.API.EEW.OLD.JSON.Root Data, EEW? from = null)
        {
            if (Data == null) throw new ArgumentNullException(nameof(Data), "Jsonデータがnullです。");
            if (from == null) from = new EEW();
            if (Data.Hypocenter != null)
            {
                from.EarthQuake.Depth = Data.Hypocenter.Location.Depth.Int;
                from.EarthQuake.Location.Lat = Data.Hypocenter.Location.Lat;
                from.EarthQuake.Location.Long = Data.Hypocenter.Location.Long;
                from.EarthQuake.IsSea = Data.Hypocenter.isSea;
                from.EarthQuake.HypocenterCode = Data.Hypocenter.Code;
                from.EarthQuake.Hypocenter = Data.Hypocenter.Name;
                from.EarthQuake.Magnitude = Data.Hypocenter.Magnitude.Float;


            }
            if (Data.MaxIntensity != null)
                from.EarthQuake.MaxIntensity = Common.StringToInt(Data.MaxIntensity.To);
            from.EarthQuake.OriginTime = Common.GetDateTime(Data.OriginTime.String);
            from.Serial.UpdateTime = Common.GetDateTime(Data.AnnouncedTime.String);
            from.Serial.Number = Data.Serial;
            from.Serial.IsFinal = Data.Type.Code == 9;
            switch (Data.Status.Code)
            {
                case "00":
                    if (Data.Warn)
                    {
                        from.Serial.Infomation = Struct.EEW.InfomationLevel.Warning;
                    }
                    else
                    {
                        from.Serial.Infomation = Struct.EEW.InfomationLevel.Forecast;
                    }
                    break;
                case "01":
                case "20":
                case "30":
                    from.Serial.Infomation = Struct.EEW.InfomationLevel.Test;
                    break;
                case "10":
                    from.Serial.Infomation = Struct.EEW.InfomationLevel.Cancelled;
                    break;
                case "11":
                    from.Serial.Infomation = Struct.EEW.InfomationLevel.CancelledTest;
                    break;
            }
            from.Serial.EventID = Data.EventID;
            from.EarthQuake.ForecastArea.District.Clear();
            from.EarthQuake.ForecastArea.LocalAreas.Clear();
            from.EarthQuake.ForecastArea.Regions.Clear();
            if (Data.WarnForecast != null)
            {
                for (int i = 0; i < Data.WarnForecast.District.Count; i++)
                    from.EarthQuake.ForecastArea.District.Add(EEWArea.StrToDistrict(Data.WarnForecast.District[i]));
                for (int i = 0; i < Data.WarnForecast.LocalAreas.Count; i++)
                    from.EarthQuake.ForecastArea.LocalAreas.Add(EEWArea.StrToLocal(Data.WarnForecast.LocalAreas[i]));
                for (int i = 0; i < Data.WarnForecast.Regions.Count; i++)
                    from.EarthQuake.ForecastArea.Regions.Add(EEWArea.StrToRegion(Data.WarnForecast.Regions[i]));
            }
            return from;
        }
        public static EEW GetData(Background.API.KyoshinAPI.EEW.JSON.Root Data, EEW? from = null)
        {
            if (Data == null) throw new ArgumentNullException(nameof(Data), "Jsonデータがnullです。");
            if (from == null) from = new EEW();
            switch (Data.Alertflg)
            {
                case null:
                    from.Serial.Infomation = InfomationLevel.Default;
                    break;
                case "予報":
                    from.Serial.Infomation = InfomationLevel.Forecast;
                    break;
                case "警報":
                    from.Serial.Infomation = InfomationLevel.Warning;
                    break;
            }
            if (Data.Alertflg != null)
            {
                if (Data.IsCancel) from.Serial.Infomation = InfomationLevel.Cancelled;
                if (Data.IsTraining) from.Serial.Infomation = InfomationLevel.Test;
                if (Data.IsCancel && Data.IsTraining) from.Serial.Infomation = InfomationLevel.CancelledTest;

                if (double.TryParse(Data.Latitude, out double value))
                    from.EarthQuake.Location.Lat = value;
                else from.EarthQuake.Location.Lat = double.NaN;
                if(double.TryParse(Data.Longitude, out value))
                    from.EarthQuake.Location.Long=value;
                else from.EarthQuake.Location.Long = double.NaN;
                if (int.TryParse(Data.ReportNum, out int valint))
                    from.Serial.Number = valint;
                else from.Serial.Number = -1;
                string str = Data.Depth.Replace("km", string.Empty);
                if (int.TryParse(str, out valint))
                    from.EarthQuake.Depth = valint;
                else from.EarthQuake.Depth = -1;
                from.EarthQuake.Hypocenter = Data.RegionName;
                from.EarthQuake.MaxIntensity = Common.StringToInt(Data.Calcintensity);
                from.Serial.IsFinal = Data.IsFinal;
                if (double.TryParse(Data.Magunitude, out value))
                    from.EarthQuake.Magnitude = value;
                else from.EarthQuake.Magnitude = double.NaN;
                if (Data.OriginTime.Length == 14)
                {
                    str = Data.OriginTime.Insert(12, ":").Insert(10, ":").Insert(8, " ").Insert(6, "/").Insert(4, "/");
                    if(DateTime.TryParse(str,out DateTime outdate))
                        from.EarthQuake.OriginTime = outdate;
                    else from.EarthQuake.OriginTime = DateTime.MinValue;
                }
                if (DateTime.TryParse(Data.ReportTime, out DateTime outdate2))
                    from.Serial.UpdateTime = outdate2;
                else from.Serial.UpdateTime = DateTime.MinValue;
                
            }
            return from;
        }




        ///<summary>発表状態</summary>
        public enum InfomationLevel
        {
            ///<summary>発表なし</summary>
            Default,
            ///<summary>訓練のキャンセル</summary>
            CancelledTest,
            ///<summary>訓練</summary>
            Test,
            ///<summary>キャンセル報</summary>
            Cancelled,
            ///<summary>緊急地震速報(予報)</summary>
            OldForecast,
            ///<summary>緊急地震速報(地震動予報)</summary>
            Forecast,
            ///<summary>緊急地震速報(警報)</summary>
            Warning,
            ///<summary>不明</summary>
            Unknown
        }

        ///<summary>地震情報全般</summary>
        public cEEW.EarthQuake EarthQuake = new();
        ///<summary>発表情報</summary>
        public cEEW.Serial Serial = new();
        ///<summary>ユーザー地点の情報</summary>
        public cEEW.UserInfo UserInfo = new();
        ///<summary>各地のエリア情報</summary>
        public List<cEEW.AreaInfo> AreasInfo = new();
    }



}
namespace MisakiEQ.Struct.cEEW
{
    ///<summary>地震情報全般</summary>
    public class EarthQuake
    {
        ///<summary>地震発生時刻</summary>
        public DateTime OriginTime = DateTime.MinValue;
        ///<summary>地震の規模(マグニチュード)</summary>
        public double Magnitude = double.NaN;
        ///<summary>震源の深さ</summary>
        public int Depth = -1;
        ///<summary>震源地(日本の地理名)</summary>
        public string Hypocenter = string.Empty;
        ///<summary>震源地コード</summary>
        public int HypocenterCode = -1;
        ///<summary>震源地の座標</summary>
        public Common.Location Location = new();
        ///<summary>海底かどうか</summary>
        public bool IsSea = false;
        ///<summary>最大震度</summary>
        public Common.Intensity MaxIntensity = Common.Intensity.Unknown;

        public ForecastArea ForecastArea = new();
    }
    ///<summary>発表情報</summary>
    public class Serial
    {
        ///<summary>最終更新時間</summary>
        public DateTime UpdateTime = DateTime.MinValue;
        ///<summary>発表情報</summary>
        public EEW.InfomationLevel Infomation = EEW.InfomationLevel.Unknown;
        ///<summary>情報番号</summary>
        public int Number = -1;
        ///<summary>最終報かどうか</summary>
        public bool IsFinal = false;
        ///<summary>イベントID</summary>
        public string EventID = string.Empty;
    }
    ///<summary>ユーザー地点の情報</summary>
    public class UserInfo
    {
        ///<summary>ローカルでの震度情報</summary>
        public Common.Intensity LocalIntensity = Common.Intensity.Unknown;
        ///<summary>ローカルでの震度(生の値)</summary>
        public double IntensityRaw = double.NaN;
        ///<summary>到達予想時刻</summary>
        public DateTime ArrivalTime = DateTime.MaxValue;
        ///<summary>警報対象地域かどうか</summary>
        public bool IsWarn = false;
    }
    public class ForecastArea
    {
        /// <summary><para>
        /// エリア名<br/>
        /// 範囲がその地方全域になる場合は地方名に置き換わる場合があります。<br/>
        /// ※NHKの緊急地震速報本家と同じ
        /// </para></summary>
        public List<EEWArea.District> District = new();
        /// <summary><para>
        /// 都道府県名<br/>
        /// 都道府県単位での発令地域です。
        /// </para></summary>
        public List<EEWArea.LocalAreas> LocalAreas = new();
        /// <summary><para>
        /// 地域名<br/>
        /// 各地域毎にあるのでリストされるデータは大きくなります。
        /// </para></summary>
        public List<EEWArea.Regions> Regions = new();
    }
    ///<summary>エリア情報</summary>
    public class AreaInfo
    {
        ///<summary>エリアの都道府県データ</summary>
        public Common.Prefectures Prefectures = Common.Prefectures.Unknown;
        ///<summary>エリアの緯度経度データ</summary>
        public Common.Location Location = new();
        ///<summary>エリア上の最大震度</summary>
        public Common.Intensity Intensity = Common.Intensity.Unknown;
        ///<summary>エリアの名前</summary>
        public string Name = string.Empty;
        ///<summary>エリア上の到達予想時刻</summary>
        public DateTime ExpectedArrival = DateTime.MinValue;
    }
}