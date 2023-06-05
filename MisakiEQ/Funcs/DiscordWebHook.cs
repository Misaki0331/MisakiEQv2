using KyoshinMonitorLib.ApiResult.WebApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MisakiEQ.Funcs
{
    public class DiscordWebHook
    {
        public static bool EEW(Struct.EEW eew)
        {
            try
            {
                var token = Lib.Discord.WebHooks.Main.TokenData;
                if (token == null)
                {
                    Log.Instance.Warn("トークンが設定されていない為送信できませんでした。");
                    return false;
                }
                var content = new Lib.Discord.WebHooks.Main.Content();

                content.embeds.Add(new());
                switch (eew.Serial.Infomation)
                {
                    case Struct.EEW.InfomationLevel.Forecast:
                        content.embeds[0].color = 0x0000ff;
                        content.embeds[0].title = $"🔵緊急地震速報(予報) {(eew.Serial.IsFinal ? "最終報" : $"第 {eew.Serial.Number} 報")}";
                        content.embeds[0].description = $"震源地 : {eew.EarthQuake.Hypocenter} 深さ : {Struct.Common.DepthToString(eew.EarthQuake.Depth)}\n" +
                            $"地震の規模 : Ｍ{eew.EarthQuake.Magnitude:0.0}  最大震度 : {Struct.Common.IntToStringLong(eew.EarthQuake.MaxIntensity)}";

                        content.embeds[0].fields.Add(new()
                        {
                            name = "震源の場所",
                            value = $"{eew.EarthQuake.Hypocenter} ({eew.EarthQuake.Location.Long:0.0}E {eew.EarthQuake.Location.Lat:0.0}N) 深さ:{Struct.Common.DepthToString(eew.EarthQuake.Depth)}"
                        });

                        content.embeds[0].fields.Add(new()
                        {
                            name = "発表時刻",
                            value = $"{eew.Serial.UpdateTime:MM/dd HH:mm:ss}",
                            inline = true
                        });
                        content.embeds[0].fields.Add(new()
                        {
                            name = "発生時刻",
                            value = $"{(eew.EarthQuake.OriginTime != DateTime.MinValue ? "不明" : $"{eew.EarthQuake.OriginTime:MM/dd HH:mm:ss}")}",
                            inline = true
                        });

                        break;
                    case Struct.EEW.InfomationLevel.Warning:

                        content.embeds[0].color = 0xff0000;
                        content.embeds[0].title = $"⚠緊急地震速報(警報) {(eew.Serial.IsFinal ? "最終報" : $"第 {eew.Serial.Number} 報")}";
                        content.embeds[0].description = $"震源地 : {eew.EarthQuake.Hypocenter} 深さ : {Struct.Common.DepthToString(eew.EarthQuake.Depth)}\n" +
                            $"地震の規模 : Ｍ{eew.EarthQuake.Magnitude:0.0}  最大震度 : {Struct.Common.IntToStringLong(eew.EarthQuake.MaxIntensity)}\n" +
                            $"⚠対象地域：";
                        foreach (var area in eew.EarthQuake.ForecastArea.LocalAreas) content.embeds[0].description += $"{area} ";
                        content.embeds[0].fields.Add(new()
                        {
                            name = "震源の場所",
                            value = $"{eew.EarthQuake.Hypocenter} ({eew.EarthQuake.Location.Long:0.0}E {eew.EarthQuake.Location.Lat:0.0}N) 深さ:{Struct.Common.DepthToString(eew.EarthQuake.Depth)}"
                        });
                        var TweetIndex = "";
                        for (int i = 0; i < eew.AreasInfo.Count; i++)
                        {
                            string intensity = $"**震度{Struct.Common.IntToStringLong(eew.AreasInfo[i].Intensity).PadRight(2, 't').Replace("t", "  ")} ";
                            TweetIndex += $"{intensity}{(eew.AreasInfo[i].ExpectedArrival == DateTime.MinValue ? "到達済み" : $"{eew.AreasInfo[i].ExpectedArrival:HH:mm:ss}")}** {eew.AreasInfo[i].Name}";
                            if (TweetIndex.Length > 2000 || eew.AreasInfo.Count - 1 == i) break;
                            TweetIndex += "\n";
                        }
                        content.embeds[0].fields.Add(new()
                        {
                            name = "エリア予測情報",
                            value = TweetIndex
                        });
                        content.embeds[0].fields.Add(new()
                        {
                            name = "発表時刻",
                            value = $"{eew.Serial.UpdateTime:MM/dd HH:mm:ss}",
                            inline = true
                        });
                        content.embeds[0].fields.Add(new()
                        {
                            name = "発生時刻",
                            value = $"{(eew.EarthQuake.OriginTime != DateTime.MinValue ? "不明" : $"{eew.EarthQuake.OriginTime:MM/dd HH:mm:ss}")}",
                            inline = true
                        });
                        break;
                    case Struct.EEW.InfomationLevel.Cancelled:

                        content.embeds[0].color = 0x00ff00;
                        content.embeds[0].title = $"🟢緊急地震速報(取消)";
                        content.embeds[0].description = $"この緊急地震速報はキャンセルされました。";

                        content.embeds[0].fields.Add(new()
                        {
                            name = "発表時刻",
                            value = $"{eew.Serial.UpdateTime:MM/dd HH:mm:ss}",
                            inline = true
                        });
                        break;
                    default:
                        return false;
                }
                content.embeds[0].timestamp = eew.Serial.UpdateTime.AddHours(-9);
                Lib.Discord.WebHooks.Main.Sent(token, content);
            }catch(Exception ex)
            {
                Log.Instance.Error(ex);
                return false;
            }
            return true;
        }
    }
}
/*
 * string TweetIndex = string.Empty;
            switch (eew.Serial.Infomation)
            {
                case Struct.EEW.InfomationLevel.Forecast:
                    TweetIndex += $"🔵緊急地震速報(予報) 第 {eew.Serial.Number} 報 {(eew.Serial.IsFinal ? "(最終報)" : string.Empty)}\n";
                    break;
                case Struct.EEW.InfomationLevel.Warning:
                    TweetIndex += $"🔴⚠️緊急地震速報(警報) 第 {eew.Serial.Number} 報 {(eew.Serial.IsFinal ? "(最終報)" : string.Empty)}\n";
                    break;
                case Struct.EEW.InfomationLevel.Cancelled:
                    TweetIndex += $"🟢緊急地震速報(キャンセル)\n";
                    break;
                default:
                    return;
            }
            if (eew.Serial.Infomation != Struct.EEW.InfomationLevel.Cancelled)
            {
                TweetIndex += $"{eew.EarthQuake.Hypocenter} 深さ: {Struct.Common.DepthToString(eew.EarthQuake.Depth)} M {eew.EarthQuake.Magnitude:0.0}\n";
                TweetIndex += $"最大震度 : {Struct.Common.IntToStringLong(eew.EarthQuake.MaxIntensity)}\n";
                TweetIndex += $"発生時刻 : {eew.EarthQuake.OriginTime:M/dd HH:mm:ss}\n";
                if (eew.Serial.Infomation == Struct.EEW.InfomationLevel.Warning)
                {
                    TweetIndex += "\n⚠️以下の地域は強い揺れに注意⚠️\n";
                    for (int i = 0; i < eew.EarthQuake.ForecastArea.LocalAreas.Count; i++)
                    {
                        TweetIndex += $"{eew.EarthQuake.ForecastArea.LocalAreas[i]}";
                        if (i + 1 != eew.EarthQuake.ForecastArea.LocalAreas.Count)
                        {
                            if (i % 5 == 4) TweetIndex += "\n";
                            else TweetIndex += " ";
                        }
                    }
                    TweetIndex += "\n";
                }
            }
            else
            {
                TweetIndex += $"この緊急地震速報は取り消されました。\n";
            }
            TweetIndex += "\n";
            TweetIndex += $"発表時刻 : {eew.Serial.UpdateTime:M/dd HH:mm:ss}\n";
            TweetIndex += $"#MisakiEQ #緊急地震速報";
            using (await EEW_Lock.LockAsync())
            {
                try
                {
                    int Index = -1;
                    long LatestID = 0;
                    for (int i = 0; i < EEWReplyList.Count; i++)
                    {
                        if (EEWReplyList[i].EventID == eew.Serial.EventID)
                        {
                            LatestID = EEWReplyList[i].LatestTweet;
                            if (EEWReplyList[i].LatestSerial >= eew.Serial.Number)
                            {
                                EEWReplyList[i].DuplicateCount++;
                                Log.Instance.Warn($"この緊急地震速報は{EEWReplyList[i].DuplicateCount}回発信されています。\nEventID:{EEWReplyList[i].EventID} 情報番号:{EEWReplyList[i].LatestSerial}");
                                //return;
                            }
                            Index = i;
                        }
                    }
                    var twitter = APIs.GetInstance();
                    LatestID = await twitter.Tweet(reply: LatestID, tweet: TweetIndex);
                    Log.Instance.Debug($"ツイートしました。 ID:{LatestID}\n" + TweetIndex);
                    if (Index != -1)
                    {
                        if (LatestID != 1)
                        {
                            EEWReplyList[Index].LatestTweet = LatestID;
                        }
                        EEWReplyList[Index].LatestSerial = eew.Serial.Number;
                        EEWReplyList[Index].LatestTime = DateTime.Now;
                    }
                    else
                    {
                        EEWReplyList.Add(new(eew.Serial.EventID, LatestID,eew.Serial.Number));
                    }
                    for (int i = EEWReplyList.Count - 1; i >= 0; i--)
                    {
                        TimeSpan T = DateTime.Now - EEWReplyList[i].LatestTime;
                        if (T.Seconds > 180) EEWReplyList.RemoveAt(i);
                    }
                }
                catch (Exception ex)
                {
                    Log.Instance.Warn($"ツイート中にエラー : {ex.Message}");
                }
            }
*/
