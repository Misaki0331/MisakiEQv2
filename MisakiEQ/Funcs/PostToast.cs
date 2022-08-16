using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MisakiEQ.Struct;

namespace MisakiEQ.Funcs
{
    public class Toast
    {
        public static void Post(EEW data)
        {
            switch (data.Serial.Infomation)
            {
                case EEW.InfomationLevel.Forecast:
                    PostEEWForecast(data);
                    break;
                case EEW.InfomationLevel.Warning:
                    PostEEWWarning(data);
                    break;
                case EEW.InfomationLevel.Cancelled:
                    PostEEWCancelled(data);
                    break;

            }
        }
        public static void Post(EarthQuake data)
        {
            switch (data.Issue.Type)
            {
                case EarthQuake.EarthQuakeType.ScalePrompt:
                    PostEarthquakeScalePrompt(data);
                    break;
                case EarthQuake.EarthQuakeType.Destination:
                    PostEarthquakeDestination(data);
                    break;
                case EarthQuake.EarthQuakeType.ScaleAndDestination:
                    PostEarthquakeScaleAndDestination(data);
                    break;
                case EarthQuake.EarthQuakeType.DetailScale:
                    PostEarthquakeDetailScale(data);
                    break;
                case EarthQuake.EarthQuakeType.Foreign:
                    break;
            }
        }
        public static void Post(Tsunami data)
        {
            var attribution = $"津波情報";

            string title;
            var index = "";
            if (data.Cancelled)
            {
                title = "津波予報は解除されました。";
            }
            else
            {
                
                int watch=0, warn=0, mwarn=0;
                for(int i = 0; i < data.Areas.Count; i++)
                {
                    switch (data.Areas[i].Grade)
                    {
                        case Tsunami.TsunamiGrade.Watch:
                            watch++;
                            break;
                        case Tsunami.TsunamiGrade.Warning:
                            warn++;
                            break;
                        case Tsunami.TsunamiGrade.MajorWarning:
                            mwarn++;
                            break;
                    }
                }
                if (mwarn > 0)
                {
                    title = "大津波警報が発表された地域があります。";
                    index = "海岸にお住まいの方は速やかに安全で高い場所に避難してください。\n";
                }
                else if (warn > 0)
                {
                    title = "津波警報が発表された地域があります。";
                    index = "海岸にお住まいの方は速やかに安全で高い場所に避難してください。\n";
                }
                else if(watch > 0)
                {
                    title = "津波注意報が発表された地域があります。";
                    index = "海岸に近い方は速やかに海から離れてください。\n";
                }
                else
                {
                    title = "津波予報が発表されました。";
                }
                index += "詳細はテレビ放送もしくは気象庁HPをご覧ください。";
            }
            Lib.ToastNotification.PostNotification(title:title,index:index,attribution:attribution,customTime:data.CreatedAt);
        }

        private static void PostEEWForecast(EEW data)
        {
            var attribution = $"緊急地震速報(予報) 第 {data.Serial.Number} 報 {(data.Serial.IsFinal ? "(最終報)" : string.Empty)}";
            var title = $"最大震度{Common.IntToStringLong(data.EarthQuake.MaxIntensity)} {data.EarthQuake.Hypocenter}";
            var index = $"規模 : M {data.EarthQuake.Magnitude:0.0} 深さ : {Common.DepthToString(data.EarthQuake.Depth)}";
            if (data.EarthQuake.IsSea && data.EarthQuake.Depth < 40 && data.EarthQuake.Depth >= 0 && data.EarthQuake.Magnitude >= 6.0)
            {
                index += "\n"
                    + "津波発生の恐れがあります。\n最新の情報にご注意ください。";
            }
            Lib.ToastNotification.PostNotification(title, index: index, attribution: attribution,customTime:data.Serial.UpdateTime);
        }
        private static void PostEEWWarning(EEW data)
        {
            var attribution = $"緊急地震速報(警報) 第 {data.Serial.Number} 報 {(data.Serial.IsFinal ? "(最終報)" : string.Empty)}";
            var title = $"(⚠️警報) 最大震度{Common.IntToStringLong(data.EarthQuake.MaxIntensity)} {data.EarthQuake.Hypocenter}";
            var index = $"規模 : M {data.EarthQuake.Magnitude:0.0} 深さ : {Common.DepthToString(data.EarthQuake.Depth)}\n"
                + "次の地域は強い揺れに注意してください\n";
            if (data.EarthQuake.ForecastArea.Regions.Count < 6)
            {
                for (int i = 0; i < data.EarthQuake.ForecastArea.Regions.Count; i++) index += data.EarthQuake.ForecastArea.Regions[i] + " ";
            } else if (data.EarthQuake.ForecastArea.LocalAreas.Count < 10)
            {
                for (int i = 0; i < data.EarthQuake.ForecastArea.LocalAreas.Count; i++) index += data.EarthQuake.ForecastArea.LocalAreas[i] + " ";
            }
            else
            {
                for (int i = 0; i < data.EarthQuake.ForecastArea.District.Count; i++) index += data.EarthQuake.ForecastArea.District[i] + " ";
            }
            if (data.EarthQuake.IsSea && data.EarthQuake.Depth < 40 && data.EarthQuake.Depth >= 0 && data.EarthQuake.Magnitude >= 6.0)
            {
                index += "\n"
                    + "津波発生の恐れがあります。\n最新の情報にご注意ください。";
            }
            Lib.ToastNotification.PostNotification(title, index: index, attribution: attribution, customTime: data.Serial.UpdateTime);
        }
        private static void PostEEWCancelled(EEW data)
        {
            var attribution = $"緊急地震速報(キャンセル報)";
            var title = "この緊急地震速報はキャンセルされました。";
            Lib.ToastNotification.PostNotification(title, attribution: attribution, customTime: data.Serial.UpdateTime);
        }
        
        private static void PostEarthquakeScalePrompt(EarthQuake data)
        {
            var attribution = $"震度速報 - {data.Details.OriginTime:M/dd H:mm}発生";
            var title = $"最大震度{Common.IntToStringLong(data.Details.MaxIntensity)}の地震が発生しました。";
            var detail = "";

            var list = data.Details.PrefIntensity.GetIntensityPrefectures();
            for (int i =0;i<list.Count;i++)
            {
                if (list.Count == 0) continue;
                if(i!=0)detail += "\n";
                detail += $"震度{Common.IntToStringLong(list[i].Intensity)}:";
                for (int j = 0; j < list[i].Prefectures.Count; j++)
                {
                    detail += Common.PrefecturesToString(list[i].Prefectures[j]) +" ";
                }
            }
            Lib.ToastNotification.PostNotification(title, attribution: attribution, index: detail, customTime: data.CreatedAt);
        }
        private static void PostEarthquakeDestination(EarthQuake data)
        {
            var attribution = $"震源に関する情報 - {data.Details.OriginTime:M/dd H:mm}発生";
            var title = $"震源地 : {data.Details.Hypocenter}";
            var detail = $"震源の深さ : {Common.DepthToString(data.Details.Depth)} 地震の規模 : M {data.Details.Magnitude:0.0}";
            Lib.ToastNotification.PostNotification(title, attribution: attribution, index: detail, customTime: data.CreatedAt);
        }

        private static void PostEarthquakeScaleAndDestination(EarthQuake data)
        {
            var attribution = $"震源と震度に関する情報 - {data.Details.OriginTime:M/dd H:mm}発生";
            var title = $"震源地 : {data.Details.Hypocenter}";
            var detail = $"震源の深さ : {Common.DepthToString(data.Details.Depth)} 地震の規模 : M {data.Details.Magnitude:0.0}\n";

            var list = data.Details.PrefIntensity.GetIntensityPrefectures();
            for (int i = 0; i < list.Count; i++)
            {
                if (list.Count == 0) continue;
                detail += "\n";
                detail += $"震度{Common.IntToStringLong(list[i].Intensity)}:";
                for (int j = 0; j < list[i].Prefectures.Count; j++)
                {
                    detail += Common.PrefecturesToString(list[i].Prefectures[j]) + " ";
                }
            }
            Lib.ToastNotification.PostNotification(title, attribution: attribution, index: detail, customTime: data.CreatedAt);
        }
        private static void PostEarthquakeDetailScale(EarthQuake data)
        {
            var attribution = $"各地の震度に関する情報 - {data.Details.OriginTime:M/dd H:mm}発生";
            var title = $"最大震度{Common.IntToStringLong(data.Details.MaxIntensity)} {data.Details.Hypocenter}";
            var detail = $"震源の深さ : {Common.DepthToString(data.Details.Depth)} 地震の規模 : M {data.Details.Magnitude:0.0}\n";
            detail += $"この地震による{EarthQuake.DomesticToString(data.Details.DomesticTsunami)}\n";
            var list = data.Details.PrefIntensity.GetIntensityPrefectures();
            for (int i = 0; i < list.Count; i++)
            {
                if (list.Count == 0) continue;
                detail += "\n";
                detail += $"震度{Common.IntToStringLong(list[i].Intensity)}:";
                for (int j = 0; j < list[i].Prefectures.Count; j++)
                {
                    detail += Common.PrefecturesToString(list[i].Prefectures[j]) + " ";
                }
            }
            Lib.ToastNotification.PostNotification(title, attribution: attribution, index: detail, customTime: data.CreatedAt);
        }
    }
}
