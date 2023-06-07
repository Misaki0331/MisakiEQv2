using KyoshinMonitorLib.ApiResult.WebApi;
using MisakiEQ.Background.API;
using MisakiEQ.Struct;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;
using static MisakiEQ.Struct.EEWArea;

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
                        content.embeds[0].description = $"{eew.EarthQuake.Hypocenter}\n" +
                            $"深さ : {Struct.Common.DepthToString(eew.EarthQuake.Depth)} " +
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
                            value = $"{(eew.EarthQuake.OriginTime == DateTime.MinValue ? "不明" : $"{eew.EarthQuake.OriginTime:MM/dd HH:mm:ss}")}",
                            inline = true
                        });

                        break;
                    case Struct.EEW.InfomationLevel.Warning:

                        content.embeds[0].color = 0xff0000;
                        content.embeds[0].title = $"\u26A0\uFE0F緊急地震速報(警報) {(eew.Serial.IsFinal ? "最終報" : $"第 {eew.Serial.Number} 報")}";
                        content.embeds[0].description = $"{eew.EarthQuake.Hypocenter}\n" +
                            $"深さ : {Struct.Common.DepthToString(eew.EarthQuake.Depth)} " +
                            $"地震の規模 : Ｍ{eew.EarthQuake.Magnitude:0.0}  最大震度 : {Struct.Common.IntToStringLong(eew.EarthQuake.MaxIntensity)}\n" +
                            $"\u26A0\uFE0F対象地域：";
                        foreach (var area in eew.EarthQuake.ForecastArea.LocalAreas) content.embeds[0].description += $"{LocalAreasToStr(area)} ";
                        content.embeds[0].fields.Add(new()
                        {
                            name = "震源の場所",
                            value = $"**{eew.EarthQuake.Hypocenter}** ({eew.EarthQuake.Location.Long:0.0}E {eew.EarthQuake.Location.Lat:0.0}N) 深さ:{Struct.Common.DepthToString(eew.EarthQuake.Depth)}"
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
                            value = $"{(eew.EarthQuake.OriginTime == DateTime.MinValue ? "不明" : $"{eew.EarthQuake.OriginTime:MM/dd HH:mm:ss}")}",
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
                Log.Instance.Debug("送信完了");

            } catch (Exception ex)
            {
                Log.Instance.Error(ex);
                return false;
            }
            return true;
        }
        public static bool Earthquake(Struct.EarthQuake eq)
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
                switch (eq.Issue.Type)
                {
                    case Struct.EarthQuake.EarthQuakeType.ScalePrompt:
                        content.embeds[0].title = $"震度速報 - {eq.Details.OriginTime:M/dd H:mm}頃";
                        content.embeds[0].description = $"最大震度{Struct.Common.IntToStringLong(eq.Details.MaxIntensity)}を観測する地震が発生しました。";
                        content.embeds[0].color = 0xFFFF00;
                        break;
                    case Struct.EarthQuake.EarthQuakeType.Destination:
                        content.embeds[0].title = $"震源情報 - {eq.Details.OriginTime:M/dd H:mm}頃";
                        content.embeds[0].fields.Add(new()
                        {
                            name = "震源地",
                            value = $"{eq.Details.Hypocenter}",
                            inline = true
                        });
                        content.embeds[0].fields.Add(new()
                        {
                            name = "震源の深さ",
                            value = $"{Struct.Common.DepthToString(eq.Details.Depth)}",
                            inline = true
                        });
                        content.embeds[0].fields.Add(new()
                        {
                            name = "地震の規模",
                            value = $"Ｍ{eq.Details.Magnitude:0.0}",
                            inline = true
                        });
                        content.embeds[0].fields.Add(new()
                        {
                            name = "情報",
                            value = $"この地震による{Struct.EarthQuake.DomesticToString(eq.Details.DomesticTsunami)}",
                            inline = false
                        });
                        content.embeds[0].color = 0xFFFF00;
                        break;
                    case Struct.EarthQuake.EarthQuakeType.ScaleAndDestination:
                        content.embeds[0].title = $"震度&震源情報 - {eq.Details.OriginTime:M/dd H:mm}頃";
                        content.embeds[0].fields.Add(new()
                        {
                            name = "震源地",
                            value = $"{eq.Details.Hypocenter}",
                            inline = true
                        });
                        content.embeds[0].fields.Add(new()
                        {
                            name = "震源の深さ",
                            value = $"{Struct.Common.DepthToString(eq.Details.Depth)}",
                            inline = true
                        });
                        content.embeds[0].fields.Add(new()
                        {
                            name = "地震の規模",
                            value = $"Ｍ{eq.Details.Magnitude:0.0}",
                            inline = true
                        });

                        content.embeds[0].fields.Add(new()
                        {
                            name = "最大震度",
                            value = $"{Struct.Common.IntToStringLong(eq.Details.MaxIntensity)}",
                            inline = true
                        });
                        content.embeds[0].fields.Add(new()
                        {
                            name = "情報",
                            value = $"この地震による{Struct.EarthQuake.DomesticToString(eq.Details.DomesticTsunami)}",
                            inline = false
                        });
                        content.embeds[0].color = 0xFFFF00;
                        break;
                    case Struct.EarthQuake.EarthQuakeType.DetailScale:
                        content.embeds[0].title = $"詳細情報 - {eq.Details.OriginTime:M/dd H:mm}頃";
                        content.embeds[0].fields.Add(new()
                        {
                            name = "震源地",
                            value = $"{eq.Details.Hypocenter}",
                            inline = true
                        });
                        content.embeds[0].fields.Add(new()
                        {
                            name = "震源の深さ",
                            value = $"{Struct.Common.DepthToString(eq.Details.Depth)}",
                            inline = true
                        });
                        content.embeds[0].fields.Add(new()
                        {
                            name = "地震の規模",
                            value = $"Ｍ{eq.Details.Magnitude:0.0}",
                            inline = true
                        });
                        content.embeds[0].fields.Add(new()
                        {
                            name = "最大震度",
                            value = $"{Struct.Common.IntToStringLong(eq.Details.MaxIntensity)}",
                            inline = true
                        });
                        content.embeds[0].fields.Add(new()
                        {
                            name = "情報",
                            value = $"この地震による{Struct.EarthQuake.DomesticToString(eq.Details.DomesticTsunami)}",
                            inline = false
                        });
                        content.embeds[0].color = 0x0000FF;
                        break;
                    default:
                        return false;
                }
                if (eq.Issue.Type == Struct.EarthQuake.EarthQuakeType.ScalePrompt ||
                       eq.Issue.Type == Struct.EarthQuake.EarthQuakeType.ScaleAndDestination ||
                       eq.Issue.Type == Struct.EarthQuake.EarthQuakeType.DetailScale)
                {
                    if (eq.Details.localAreaPoints.Count > 0)
                    {
                        var loop = new List<Struct.Common.Intensity>()
                        {
                            Struct.Common.Intensity.Int7,
                            Struct.Common.Intensity.Int6Up, Struct.Common.Intensity.Int6Down ,
                            Struct.Common.Intensity.Int5Up, Struct.Common.Intensity.Int5Down , Struct.Common.Intensity.Int5Over,
                            Struct.Common.Intensity.Int4, Struct.Common.Intensity.Int3,
                            Struct.Common.Intensity.Int2, Struct.Common.Intensity.Int1,
                        };
                        foreach (var intensity in loop)
                        {
                            var area = eq.Details.localAreaPoints.FindAll(eq => eq.Intensity == intensity);
                            if (area == null) continue;
                            string areastr = "";
                            foreach (var areaPoint in area) {
                                areastr += $"{areaPoint.Area.Name} ";
                            }                  
                            areastr = areastr.Trim();
                            if (string.IsNullOrWhiteSpace(areastr))continue;
                            content.embeds[0].fields.Add(new()
                            {
                                name = $"震度{Struct.Common.IntToStringLong(eq.Details.MaxIntensity)}",
                                value = areastr
                            });
                        }

                    }
                }

                content.embeds[0].timestamp = eq.CreatedAt.AddHours(-9);
                Lib.Discord.WebHooks.Main.Sent(token, content);
                Log.Instance.Debug("送信完了");
            }
            catch (Exception ex)
            {
                Log.Instance.Error(ex);
                return false;
            }
            return true;
        }

        public static bool Tsunami(Struct.Tsunami tsunami)
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
                content.embeds[0].title = $"津波情報 - {tsunami.Issue.Time:M/dd H:mm:ss}発表";
                if (tsunami.Cancelled)
                {
                    content.embeds[0].description = "全ての津波予報が解除されました。";
                }
                else
                {
                    List<string>[] grades = new List<string>[6];
                    for (int i = 0; i < 6; i++) grades[i] = new();
                    for (int i = 0; i < tsunami.Areas.Count; i++)
                    {
                        switch (tsunami.Areas[i].Grade)
                        {
                            case Struct.Tsunami.TsunamiGrade.MajorWarning:
                                if (tsunami.Areas[i].Immediate) grades[0].Add(tsunami.Areas[i].Name);
                                else grades[1].Add(tsunami.Areas[i].Name);
                                break;
                            case Struct.Tsunami.TsunamiGrade.Warning:
                                if (tsunami.Areas[i].Immediate) grades[2].Add(tsunami.Areas[i].Name);
                                else grades[3].Add(tsunami.Areas[i].Name);
                                break;
                            case Struct.Tsunami.TsunamiGrade.Watch:
                                if (tsunami.Areas[i].Immediate) grades[4].Add(tsunami.Areas[i].Name);
                                else grades[5].Add(tsunami.Areas[i].Name);
                                break;
                        }
                    }
                    for(int i=0;i<6;i++)
                    {
                        if (grades[i].Count == 0) continue;
                        var areastr = "";
                        foreach(var grade in grades[i])
                        {
                            areastr += $"{grade} ";
                        }
                        areastr = areastr.Trim();
                        var title = "";
                        switch (i)
                        {
                            case 0:
                                title = "大津波警報 (まもなく到達)";
                                break;
                            case 1:
                                title = "大津波警報";
                                break;
                            case 2:
                                title = "津波警報 (まもなく到達)";
                                break;
                            case 3:
                                title = "津波警報";
                                break;
                            case 4:
                                title = "津波注意報 (まもなく到達)";
                                break;
                            case 5:
                                title = "津波注意報";
                                break;

                        }
                        content.embeds[0].fields.Add(new()
                        {
                            name = title,
                            value = areastr
                        });
                    }
                }

                content.embeds[0].timestamp = tsunami.CreatedAt.AddHours(-9);
                Lib.Discord.WebHooks.Main.Sent(token, content);
                Log.Instance.Debug("送信完了");
            }
            catch (Exception ex)
            {
                Log.Instance.Error(ex);
                return false;
            }
            return true;
        }
        public static bool Jalert(Struct.cJAlert.J_Alert j)
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
                content.embeds[0].title = $"J-ALERT【{j.Title}】";
                content.embeds[0].description = j.Detail;
                var areas = "";
                foreach (var str in j.Areas) areas += $"{str} ";
                areas = areas.Trim();
                content.embeds[0].fields.Add(new()
                {
                    name = "対象地域",
                    value = areas
                });
                content.embeds[0].fields.Add(new()
                {
                    name = "発表元",
                    value = j.SourceName,
                    inline = true
                });
                content.embeds[0].fields.Add(new()
                {
                    name = "発表時刻",
                    value = $"{j.AnnounceTime:yyyy/MM/dd HH:mm}",
                    inline = true
                });
                content.embeds[0].timestamp = j.AnnounceTime.AddHours(-9);
                content.embeds[0].color = 0xFF0000;

                Lib.Discord.WebHooks.Main.Sent(token, content);
                Log.Instance.Debug("送信完了");
            }
            catch (Exception ex)
            {
                Log.Instance.Error(ex);
                return false;
            }
            return true;
        }
    }
}
