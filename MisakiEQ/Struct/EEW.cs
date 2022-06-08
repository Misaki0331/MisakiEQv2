﻿using System;
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
        public static EEW GetData(Background.API.EEW.JSON.Root Data,EEW? from = null)
        {
            if (from == null) from = new EEW();
            if (Data != null)
            {
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
                if (Data.WarnForecast != null) {
                    for (int i = 0; i < Data.WarnForecast.District.Count; i++)
                        from.EarthQuake.ForecastArea.District.Add(Data.WarnForecast.District[i]);
                    for (int i = 0; i < Data.WarnForecast.LocalAreas.Count; i++)
                        from.EarthQuake.ForecastArea.LocalAreas.Add(Data.WarnForecast.LocalAreas[i]);
                    for (int i = 0; i < Data.WarnForecast.Regions.Count; i++)
                        from.EarthQuake.ForecastArea.Regions.Add(Data.WarnForecast.Regions[i]);
                }
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
        public string EventID= string.Empty;
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
        public List<string> District = new();
        public List<string> LocalAreas = new();
        public List<string> Regions = new();
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
        public string Name=string.Empty;
        ///<summary>エリア上の到達予想時刻</summary>
        public DateTime ExpectedArrival = DateTime.MinValue;
    }
}