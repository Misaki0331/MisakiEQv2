﻿using MisakiEQ.Background;
using MisakiEQ.Lib;
using MisakiEQ.Lib.PrefecturesAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tweetinvi.Models.V2;

namespace MisakiEQ.Funcs
{
    internal class Misskey
    {

        private class EEWNote
        {
            public EEWNote(string Event, string Latest, int Serial)
            {
                LatestTime = DateTime.Now;
                EventID = Event;
                LatestNote = Latest;
                LatestSerial = Serial;
            }
            public string EventID { get; set; } = string.Empty;
            public string LatestNote { get; set; } = string.Empty;
            public int LatestSerial { get; set; } = 0;
            public int DuplicateCount { get; set; } = 0;
            public DateTime LatestTime { get; set; } = DateTime.Now;
        }

        private class EarthquakeNote
        {
            public EarthquakeNote(DateTime origin, string Latest)
            {
                LatestTime = DateTime.Now;
                OriginTime = origin;
                LatestNote = Latest;
            }
            public DateTime OriginTime { get; set; } = DateTime.MinValue;
            public string LatestNote { get; set; } = string.Empty;
            public DateTime LatestTime { get; set; } = DateTime.Now;
        }
        private static Tweets? singleton = null;
        /// <summary>
        /// インスタンスを生成する
        /// </summary>
        public static Tweets GetInstance()
        {
            singleton ??= new Tweets();
            return singleton;
        }
        readonly List<EEWNote> EEWReplyList = new();
        readonly List<EarthquakeNote> EQReplyList = new();
        private readonly AsyncLock EEW_Lock = new();
        private readonly AsyncLock EQ_Lock = new();
        private readonly AsyncLock Tsunami_Lock = new();
        private readonly AsyncLock JALERT_Lock = new();
        public async void EEWPost(Struct.EEW eew)
        {
            string TweetIndex = string.Empty;
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
                    string LatestID = string.Empty;
                    for (int i = 0; i < EEWReplyList.Count; i++)
                    {
                        if (EEWReplyList[i].EventID == eew.Serial.EventID)
                        {
                            LatestID = EEWReplyList[i].LatestNote;
                            if (EEWReplyList[i].LatestSerial >= eew.Serial.Number)
                            {
                                EEWReplyList[i].DuplicateCount++;
                                Log.Instance.Warn($"この緊急地震速報は{EEWReplyList[i].DuplicateCount}回発信されています。\nEventID:{EEWReplyList[i].EventID} 情報番号:{EEWReplyList[i].LatestSerial}");
                                return;
                            }
                            Index = i;
                        }
                    }
                    LatestID = await Lib.Misskey.APIData.CreateNote(replyid: LatestID, text: TweetIndex, visibility: Lib.Misskey.Setting.Visibility.Home);
                    Log.Instance.Debug($"Noteしました。 ID:{LatestID}\n");
                    if (Index != -1)
                    {
                        if (LatestID !=string.Empty)
                        {
                            EEWReplyList[Index].LatestNote = LatestID;
                        }
                        EEWReplyList[Index].LatestSerial = eew.Serial.Number;
                        EEWReplyList[Index].LatestTime = DateTime.Now;
                    }
                    else
                    {
                        EEWReplyList.Add(new(eew.Serial.EventID, LatestID, eew.Serial.Number));
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
        }
        public async void EarthquakePost(Struct.EarthQuake eq)
        {
            using (await EQ_Lock.LockAsync())
            {
                try
                {
                    List<string> index = new();
                    switch (eq.Issue.Type)
                    {
                        case Struct.EarthQuake.EarthQuakeType.ScalePrompt:
                            index.Add($"震度速報 - {eq.Details.OriginTime:M/dd H:mm}頃");
                            index.Add($"最大震度{Struct.Common.IntToStringLong(eq.Details.MaxIntensity)}を観測する地震が発生しました。");
                            break;
                        case Struct.EarthQuake.EarthQuakeType.Destination:
                            index.Add($"震源情報 - {eq.Details.OriginTime:M/dd H:mm}頃");
                            index.Add($"震源地: {eq.Details.Hypocenter}");
                            index.Add($"震源の深さ: {Struct.Common.DepthToString(eq.Details.Depth)}");
                            index.Add($"地震の規模: Ｍ{eq.Details.Magnitude:0.0}");
                            index.Add($"この地震による{Struct.EarthQuake.DomesticToString(eq.Details.DomesticTsunami)}");
                            break;
                        case Struct.EarthQuake.EarthQuakeType.ScaleAndDestination:
                            index.Add($"震度&震源情報 - {eq.Details.OriginTime:M/dd H:mm}頃");
                            index.Add($"震源地: {eq.Details.Hypocenter}");
                            index.Add($"震源の深さ: {Struct.Common.DepthToString(eq.Details.Depth)}");
                            index.Add($"地震の規模: Ｍ{eq.Details.Magnitude:0.0}");
                            index.Add($"最大震度: {Struct.Common.IntToStringLong(eq.Details.MaxIntensity)}");
                            index.Add($"この地震による{Struct.EarthQuake.DomesticToString(eq.Details.DomesticTsunami)}");
                            break;
                        case Struct.EarthQuake.EarthQuakeType.DetailScale:
                            index.Add($"詳細情報 - {eq.Details.OriginTime:M/dd H:mm}頃");
                            index.Add($"震源地: {eq.Details.Hypocenter}");
                            index.Add($"震源の深さ: {Struct.Common.DepthToString(eq.Details.Depth)}");
                            index.Add($"地震の規模: Ｍ{eq.Details.Magnitude:0.0}");
                            index.Add($"最大震度: {Struct.Common.IntToStringLong(eq.Details.MaxIntensity)}");
                            index.Add($"この地震による{Struct.EarthQuake.DomesticToString(eq.Details.DomesticTsunami)}");
                            break;
                        default:
                            return;
                    }
                    if (eq.Issue.Type == Struct.EarthQuake.EarthQuakeType.ScalePrompt ||
                       eq.Issue.Type == Struct.EarthQuake.EarthQuakeType.ScaleAndDestination ||
                       eq.Issue.Type == Struct.EarthQuake.EarthQuakeType.DetailScale)
                    {
                        if (eq.Details.Points.Count > 0)
                        {
                            index.Add(string.Empty);
                            index.Add("各地の震度は以下の通りです。");
                            index.Add(string.Empty);
                            var list = eq.Details.PrefIntensity.GetIntensityPrefectures();
                            for (int i = 0; i < list.Count; i++)
                            {
                                string txt = $"震度{Struct.Common.IntToStringLong(list[i].Intensity)}：";
                                for (int j = 0; j < list[i].Prefectures.Count; j++)
                                {
                                    txt += Struct.Common.PrefecturesToString(list[i].Prefectures[j]);
                                    if (list[i].Prefectures.Count - 1 != j && j % 6 == 5)
                                    {
                                        index.Add(txt);
                                        txt = "";
                                        for (int k = 0; k < (Struct.Common.IntToStringLong(list[i].Intensity).Length + 3); k++) txt += "　";
                                    }
                                    else
                                    {
                                        txt += " ";
                                    }
                                }
                                index.Add(txt);

                            }
                        }
                    }
                    bool IsExist = false;
                    string LatestTweet = string.Empty;
                    EarthquakeNote eqdata = new(eq.Details.OriginTime, string.Empty);
                    for (int i = 0; i < EQReplyList.Count; i++)
                    {
                        if (eq.Details.OriginTime == EQReplyList[i].OriginTime)
                        {
                            IsExist = true;
                            LatestTweet = EQReplyList[i].LatestNote;
                            eqdata = EQReplyList[i];
                            break;
                        }
                    }

                    List<string> TweetIndexs = new();
                    string Text = "";
                    for (int i = 0; i < index.Count; i++)
                    {
                            Text += $"{index[i]}\n";
                    }
                    TweetIndexs.Add(Text + "\n#MisakiEQ #地震");
                    for (int i = 0; i < TweetIndexs.Count; i++)
                    {
                        string id = await Lib.Misskey.APIData.CreateNote(TweetIndexs[i], Lib.Misskey.Setting.Visibility.Public ,eqdata.LatestNote);
                        if (!string.IsNullOrEmpty(id))
                        {
                            eqdata.LatestNote = id;
                        }
                        else
                        {
                            Log.Instance.Warn($"Note投稿できませんでした。\n{TweetIndexs[i]}");
                        }
                    }
                    if (!IsExist)
                        EQReplyList.Add(new(eq.Details.OriginTime, eqdata.LatestNote));
                    for (int i = EQReplyList.Count - 1; i >= 0; i--)
                    {
                        TimeSpan T = DateTime.Now - EQReplyList[i].LatestTime;
                        if (T.Seconds > 86400) EQReplyList.RemoveAt(i);
                    }
                }
                catch (Exception ex)
                {
                    Log.Instance.Error(ex);
                }
            }
        }
        public async void TsunamiPost(Struct.Tsunami tsunami)
        {

            using (await Tsunami_Lock.LockAsync())
            {
                try
                {
                    List<string> TweetList = new();
                    List<string> index = new()
                {
                    $"津波情報 - {tsunami.Issue.Time:M/dd H:mm:ss}発表"
                };
                    if (tsunami.Cancelled)
                    {
                        index.Add("");
                        index.Add("全ての津波予報が解除されました。");
                        index.Add("");
                        string l = string.Empty;
                        for (int k = 0; k < index.Count; k++)
                            l += index[k] + '\n';
                        index.Clear();
                        if (!string.IsNullOrWhiteSpace(l.Replace('\n', ' ')))
                            TweetList.Add(l);
                    }
                    else
                    {
                        index.Add("");
                        index.Add("現在以下の地域に津波予報が発表されています。");
                        index.Add("");
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
                        int cnt = 0;
                        bool IsFirst = false;
                        for (int i = 0; i < grades.Length; i++)
                        {
                            if (grades[i].Count != 0)
                            {
                                if (IsFirst)
                                {
                                    index.Add("");
                                    cnt++;
                                }
                                else
                                    IsFirst = true;
                                switch (i)
                                {
                                    case 0:
                                        index.Add("🟥⬜大津波警報 (まもなく到達)");
                                        break;
                                    case 1:
                                        index.Add("🟥⬜大津波警報");
                                        break;
                                    case 2:
                                        index.Add("🟥津波警報 (まもなく到達)");
                                        break;
                                    case 3:
                                        index.Add("🟥津波警報");
                                        break;
                                    case 4:
                                        index.Add("🟨津波注意報 (まもなく到達)");
                                        break;
                                    case 5:
                                        index.Add("🟨津波注意報");
                                        break;
                                }
                                string tmp = index[^1];
                                string text = string.Empty;
                                for (int j = 0; j < grades[i].Count; j++)
                                {
                                    if (text.Length + grades[i][j].Length + 1 > 20)
                                    {
                                        index.Add(text);
                                        text = $"{grades[i][j]}";
                                    }
                                    else
                                    {
                                        if (j != 0) text += " ";
                                        text += $"{grades[i][j]}";
                                    }
                                }
                                index.Add(text);
                                if (cnt > 230)
                                {
                                    string t = string.Empty;
                                    for (int k = 0; k < index.Count; k++)
                                    {
                                        t += index[k] + '\n';
                                    }
                                    cnt = 0;
                                    index.Clear();
                                    TweetList.Add(t);
                                }
                            }
                        }
                        string l = string.Empty;
                        for (int k = 0; k < index.Count; k++)
                            l += index[k] + '\n';
                        index.Clear();
                        if (!string.IsNullOrWhiteSpace(l.Replace('\n', ' ')))
                            TweetList.Add(l);
                    }
                    string Latest = "";
                    for (int i = 0; i < TweetList.Count; i++)
                    {
                        TweetList[i] += "#MisakiEQ #津波";
                        if (TweetList.Count > 1) TweetList[i] += $" ({i + 1}/{TweetList.Count})";
                        Latest = await Lib.Misskey.APIData.CreateNote(TweetList[i], Lib.Misskey.Setting.Visibility.Public,Latest);
                        Log.Instance.Debug($"ツイートしました。 ID:{Latest}\n" + TweetList[i]);
                    }
                }
                catch (Exception ex)
                {
                    Log.Instance.Error(ex);
                }
            }
        }
        public async void JALERTPost(Struct.cJAlert.J_Alert data)
        {

            using (await JALERT_Lock.LockAsync())
            {
                try
                {
                    List<string> TweetList = new();
                    List<string> index = new()
                    {
                        $"🔺J-ALERT🔺【{data.Title}】",
                        $"{data.AnnounceTime:M/dd H:mm}受信"
                    };
                    index.Add(data.Detail);

                    string areas = "";
                    if (data.Areas.Count != 0) areas = "発令地区: ";
                    for (int i = 0; i < data.Areas.Count; i++)
                    {
                        areas += data.Areas[i];
                        if (areas.Length > 18)
                        {
                            index.Add(areas);
                            areas = "";
                        }
                        else
                        if (i != data.Areas.Count - 1) areas += " ";
                    }
                    if (areas != "") index.Add(areas);
                    string tweet = "";
                    for (int i = 0; i < index.Count; i++)
                    {
                        tweet += $"{index[i]}\n";

                    }
                    if (tweet != "")
                    {
                        TweetList.Add(tweet);
                    }
                    string Latest = "";
                    for (int i = 0; i < TweetList.Count; i++)
                    {
                        TweetList[i] += "#MisakiEQ #Jアラート";
                        if (TweetList.Count > 1) TweetList[i] += $" ({i + 1}/{TweetList.Count})";
                        Latest = await Lib.Misskey.APIData.CreateNote(TweetList[i],Lib.Misskey.Setting.Visibility.Public, Latest);
                        Log.Instance.Debug($"Misskeyにノートを投稿しました。 ID:{Latest}");
                    }
                }
                catch (Exception ex)
                {
                    Log.Instance.Error(ex);
                }
            }
        }
    }
}
