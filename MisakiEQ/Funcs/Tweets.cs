using MisakiEQ.Lib;
using MisakiEQ.Lib.Twitter;
using MisakiEQ.Struct.cEEW;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MisakiEQ.Funcs
{
    internal class Tweets
    {
        private class EEWTweet
        {
            public EEWTweet(string Event, long Latest, int Serial)
            {
                LatestTime = DateTime.Now;
                EventID = Event;
                LatestTweet = Latest;
                LatestSerial= Serial;
            }
            public string EventID { get; set; } = string.Empty;
            public long LatestTweet { get; set; } = 0;
            public int LatestSerial { get; set; } = 0;
            public int DuplicateCount { get; set; } = 0;
            public DateTime LatestTime { get; set; } = DateTime.Now;
        }

        private class EarthquakeTweet
        {
            public EarthquakeTweet(DateTime origin, long Latest)
            {
                LatestTime = DateTime.Now;
                OriginTime = origin;
                LatestTweet = Latest;
            }
            public DateTime OriginTime { get; set; } = DateTime.MinValue;
            public long LatestTweet { get; set; } = 0;
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
        readonly List<EEWTweet> EEWReplyList = new();
        readonly List<EarthquakeTweet> EQReplyList = new();
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
                        TweetIndex += $"{MisakiEQ.Struct.EEWArea.LocalAreasToStr(eew.EarthQuake.ForecastArea.LocalAreas[i])}";
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
                            int cnt = 0;
                            for (int i = 0; i < index.Count; i++) cnt += APIs.GetLen(index[i]) + 1;
                            var list = eq.Details.PrefIntensity.GetIntensityPrefectures();
                            for (int i = 0; i < list.Count; i++)
                            {
                                string txt = $"震度{Struct.Common.IntToStringLong(list[i].Intensity)}：";
                                for (int j = 0; j < list[i].Prefectures.Count; j++)
                                {
                                    txt += Struct.Common.PrefecturesToString(list[i].Prefectures[j]);
                                    if (list[i].Prefectures.Count - 1 != j && j % 6 == 5)
                                    {
                                        if (cnt + APIs.GetLen(txt) + 1 > 250 && txt.Contains('　'))
                                        {
                                            txt = $"震度{Struct.Common.IntToStringLong(list[i].Intensity)}：" + txt.Replace("　", "");
                                            cnt = 0;
                                        }
                                        index.Add(txt);
                                        cnt += APIs.GetLen(txt) + 1;
                                        txt = "";
                                        for (int k = 0; k < (Struct.Common.IntToStringLong(list[i].Intensity).Length + 3); k++) txt += "　";
                                    }
                                    else
                                    {
                                        txt += " ";
                                    }
                                }
                                cnt += APIs.GetLen(txt) + 1;
                                index.Add(txt);

                            }
                        }
                    }
                    bool IsExist = false;
                    long LatestTweet = 0;
                    EarthquakeTweet eqdata = new(eq.Details.OriginTime, 0);
                    for (int i = 0; i < EQReplyList.Count; i++)
                    {
                        if (eq.Details.OriginTime == EQReplyList[i].OriginTime)
                        {
                            IsExist = true;
                            LatestTweet = EQReplyList[i].LatestTweet;
                            eqdata = EQReplyList[i];
                            break;
                        }
                    }

                    List<string> TweetIndexs = new();
                    string Text = "";
                    for (int i = 0; i < index.Count; i++)
                    {
                        if (APIs.GetLen(Text + index[i]) + 1 > 250)
                        {
                            TweetIndexs.Add(Text + "\n#MisakiEQ #地震");
                            Text = $"{index[i]}\n";
                        }
                        else
                            Text += $"{index[i]}\n";
                    }
                    TweetIndexs.Add(Text + "\n#MisakiEQ #地震");
                    if (TweetIndexs.Count > 1)
                    {
                        for (int i = 0; i < TweetIndexs.Count; i++)
                            TweetIndexs[i] += $" ({i + 1}/{TweetIndexs.Count})";
                    }
                    for (int i = 0; i < TweetIndexs.Count; i++)
                    {
                        long id = await APIs.GetInstance().Tweet(TweetIndexs[i], eqdata.LatestTweet);
                        if (id != -1)
                        {
                            eqdata.LatestTweet = id;
                        }
                        else
                        {
                            Log.Instance.Warn($"ツイートできませんでした。\n{TweetIndexs[i]}");
                        }
                    }
                    if (!IsExist)
                        EQReplyList.Add(new(eq.Details.OriginTime, eqdata.LatestTweet));
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
                        for (int i = 0; i < index.Count; i++) cnt += APIs.GetLen(index[i]) + 1;
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
                                cnt += APIs.GetLen(index[^1]) + 1;
                                string text = string.Empty;
                                for (int j = 0; j < grades[i].Count; j++)
                                {
                                    if (text.Length + grades[i][j].Length + 1 > 20)
                                    {
                                        if (cnt + APIs.GetLen(text) + 1 > 250)
                                        {
                                            string t = string.Empty;
                                            for (int k = 0; k < index.Count; k++)
                                            {
                                                t += index[k] + '\n';
                                            }
                                            index.Clear();
                                            TweetList.Add(t);
                                            index.Add(tmp);
                                            cnt = APIs.GetLen(tmp);
                                        }
                                        index.Add(text);
                                        cnt += APIs.GetLen(text) + 1;
                                        text = $"{grades[i][j]}";
                                    }
                                    else
                                    {
                                        if (j != 0) text += " ";
                                        text += $"{grades[i][j]}";
                                    }
                                }
                                cnt += APIs.GetLen(text) + 1;
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
                    long Latest = 0;
                    for (int i = 0; i < TweetList.Count; i++)
                    {
                        TweetList[i] += "#MisakiEQ #津波";
                        if (TweetList.Count > 1) TweetList[i] += $" ({i + 1}/{TweetList.Count})";
                        Latest = await APIs.GetInstance().Tweet(TweetList[i], Latest);
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
                    for(int i = 0; i < data.Detail.Length; i += (220-APIs.GetLen(data.Title))/2)
                    {
                        if (i + 90 > data.Detail.Length)
                        {
                            index.Add(data.Detail[i..]);
                        }
                        else
                        {
                            index.Add(data.Detail.Substring(i, (220 - APIs.GetLen(data.Title)) / 2));
                        }
                    }
                    
                    string areas = ""; 
                    if (data.Areas.Count != 0) areas="発令地区: ";
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
                    int cnt = 0;
                    string tweet = "";
                    for(int i = 0; i < index.Count; i++)
                    {
                        if(cnt+APIs.GetLen(index[i] + 1) > 256)
                        {
                            TweetList.Add(tweet);
                            cnt = 0;
                            tweet = "";
                        }
                        cnt += APIs.GetLen(index[i] + 1);
                        tweet += $"{index[i]}\n";

                    }
                    if (tweet != "")
                    {
                        TweetList.Add(tweet);
                    }
                    long Latest = 0;
                    for (int i = 0; i < TweetList.Count; i++)
                    {
                        TweetList[i] += "#MisakiEQ #Jアラート";
                        if (TweetList.Count > 1) TweetList[i] += $" ({i + 1}/{TweetList.Count})";
                        Latest = await APIs.GetInstance().Tweet(TweetList[i], Latest);
                        Log.Instance.Debug($"ツイートしました。 ID:{Latest}\n" + TweetList[i]);
                    }
                }
                catch(Exception ex)
                {
                    Log.Instance.Error(ex);
                }
            }
        }
    }
}
