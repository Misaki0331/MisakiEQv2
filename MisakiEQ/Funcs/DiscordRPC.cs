using MisakiEQ.Struct;

namespace MisakiEQ.Funcs
{
    internal class DiscordRPC
    {
        public static void PostEEW(EEW eew)
        {
            var discord = Lib.Discord.RichPresence.GetInstance();
            string detail = "", status = "";
            string img = "", imgtxt = "";
            string mimg = "", mimgtxt = "";
            switch (eew.Serial.Infomation)
            {
                case EEW.InfomationLevel.Cancelled:
                    detail = "緊急地震速報(キャンセル報)";
                    status = "取り消されました。";
                    break;
                case EEW.InfomationLevel.Forecast:
                    detail = $"緊急地震速報(予報) 第 {eew.Serial.Number} 報{(eew.Serial.IsFinal?" (最終報)":string.Empty)}";
                    status = $"震源地 : {eew.EarthQuake.Hypocenter}";
                    img = $"eew_n_{Common.IntToStringShort(eew.EarthQuake.MaxIntensity).Replace('+','_')}";
                    imgtxt = $"深さ:{Common.DepthToString(eew.EarthQuake.Depth)} M{eew.EarthQuake.Magnitude:0.0}";
                    break;
                case EEW.InfomationLevel.Warning:
                    detail = $"緊急地震速報(警報) 第 {eew.Serial.Number} 報{(eew.Serial.IsFinal ? " (最終報)" : string.Empty)}";
                    status = $"震源地 : {eew.EarthQuake.Hypocenter}";
                    img = $"eew_w_{Common.IntToStringShort(eew.EarthQuake.MaxIntensity).Replace('+', '_')}";
                    imgtxt = $"深さ:{Common.DepthToString(eew.EarthQuake.Depth)} M{eew.EarthQuake.Magnitude:0.0}";
                    mimg = "warning";
                    mimgtxt = $"〈強い揺れに警戒〉";
                    for (int i = 0; i < eew.EarthQuake.ForecastArea.District.Count; i++) mimgtxt += $"{eew.EarthQuake.ForecastArea.District[i]} ";
                    break;
            }
            discord.Update(detail:detail,status:status,LImgKey:img,LImgText:imgtxt,SImgKey:mimg,SImgText:mimgtxt);
        } 
        public static void PostJAlert(Struct.cJAlert.J_Alert data)
        {
            var discord = Lib.Discord.RichPresence.GetInstance();
            string detail = "", status = "";
            detail = $"J-ALERT【{data.Title}】";
            if (data.Detail.Length > 32)
            {
                status=data.Detail.Substring(0,32)+"...";
            }
            else
            {
                status=data.Detail;
            }
            discord.Update(detail: detail, status: status);
        }
        public static void PostEarthquake(EarthQuake eq)
        {
            var discord = Lib.Discord.RichPresence.GetInstance();
            string detail = "", status = "";
            string imgtxt = "";
            switch (eq.Issue.Type)
            {
                case EarthQuake.EarthQuakeType.ScalePrompt:
                    detail = $"【震度】{eq.Details.OriginTime:d日H:mm}頃 最大:{Common.IntToStringLong(eq.Details.MaxIntensity)}";
                    var list = eq.Details.PrefIntensity.GetIntensityPrefectures();
                    if (list.Count > 0)
                    {
                        status = $"震度{Common.IntToStringLong(list[0].Intensity)}:";
                        for (int i = 0; i < list[0].Prefectures.Count; i++)
                        {
                            status += $"{list[0].Prefectures[i]} ";
                        }
                    }
                    else
                    {
                        status = "震度情報がありません";
                    }
                    break;
                case EarthQuake.EarthQuakeType.Destination:
                    detail = $"【震源情報】{eq.Details.OriginTime:d日H:mm}頃";
                    status = $"{eq.Details.Hypocenter} 深さ:{Common.DepthToString(eq.Details.Depth)} M{eq.Details.Magnitude:0.0}";
                    break;
                case EarthQuake.EarthQuakeType.ScaleAndDestination:
                    detail = $"【震度・震源】{eq.Details.OriginTime:d日H:mm}頃";
                    status = $"{eq.Details.Hypocenter} 深さ:{Common.DepthToString(eq.Details.Depth)} M{eq.Details.Magnitude:0.0}";
                    break;
                case EarthQuake.EarthQuakeType.DetailScale:
                    detail = $"【詳細】{eq.Details.OriginTime:d日H:mm}頃 最大:{Common.IntToStringLong(eq.Details.MaxIntensity)}";
                    status = $"{eq.Details.Hypocenter} 深さ:{Common.DepthToString(eq.Details.Depth)} M{eq.Details.Magnitude:0.0}"; 
                    list = eq.Details.PrefIntensity.GetIntensityPrefectures();
                    if (list.Count > 0)
                    {
                        imgtxt = $"震度{Common.IntToStringLong(list[0].Intensity)}:";
                        for (int i = 0; i < list[0].Prefectures.Count; i++)
                        {
                            imgtxt += $"{list[0].Prefectures[i]} ";
                        }
                    }
                    else
                    {
                        imgtxt = "震度情報がありません";
                    }
                    break;
            }
            discord.Update(detail: detail, status: status, LImgKey: "", LImgText: imgtxt, SImgKey: "", SImgText: "");
        }
    }
}
