using MisakiEQ.Background;
using MisakiEQ.Lib;
using MisakiEQ.Lib.PrefecturesAPI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;
using Tweetinvi.Models.V2;
using Windows.Storage.Streams;
using static System.Net.Mime.MediaTypeNames;

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
            public bool IsWarnFirstNoted { get; set; } = false;
            public int WarnAreaCount { get; set; } = 0;
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
        bool IsNextPublic = false;
        private Misskey()
        {
            EEWDelay.SetTask(new(async (SendNote note) =>
            {
                note.responseNote = await Lib.Misskey.APIData.CreateNote(replyid: ""/*LatestID*/, text: note.Note, visibility: note.Visibility);
                if (note.Visibility == Lib.Misskey.Setting.Visibility.Public) IsNextPublic = false;
            }));
        }
        private static Misskey? singleton = null;
        /// <summary>
        /// インスタンスを生成する
        /// </summary>
        public static Misskey GetInstance()
        {
            singleton ??= new Misskey();
            return singleton;
        }
        public int EEWDelayTime
        {
           get { return EEWDelay.DelayTime; }
           set { EEWDelay.DelayTime = value; }
        }
        public bool IsInterSend { get; set; } = true;
        //Todo : 重大インシデント
        private Lib.DelayFunction<SendNote> EEWDelay = new();
        class SendNote
        {
            public SendNote()
            {

            }
            public string responseNote = "";
            public string Note = "";
            public Lib.Misskey.Setting.Visibility Visibility = Lib.Misskey.Setting.Visibility.Specified;
        }
        readonly List<EEWNote> EEWReplyList = new();
        readonly List<EarthquakeNote> EQReplyList = new();
        private readonly AsyncLock EEW_Lock = new();
        private readonly AsyncLock EQ_Lock = new();
        private readonly AsyncLock Tsunami_Lock = new();
        private readonly AsyncLock JALERT_Lock = new();

        private string SetMisskeyColor(string BG,string FG, string str)
        {
            return $"$[bg.color={BG} $[fg.color={FG} {str}]]";
        }
        private string IntensityColor(Struct.Common.Intensity Intensity, string str) 
        {
            switch(Intensity)
            {
                case Struct.Common.Intensity.Int7:
                    return SetMisskeyColor("808", "FF0", str);
                case Struct.Common.Intensity.Int6Up:
                    return SetMisskeyColor("F0F", "FFF", str);
                case Struct.Common.Intensity.Int6Down:
                    return SetMisskeyColor("FFC0CB", "F00", str);
                case Struct.Common.Intensity.Int5Down:
                case Struct.Common.Intensity.Int5Up:
                case Struct.Common.Intensity.Int5Over:
                    return SetMisskeyColor("F00", "FFF", str);
                case Struct.Common.Intensity.Int4:
                    return SetMisskeyColor("FFA500", "000", str);
                case Struct.Common.Intensity.Int3:
                    return SetMisskeyColor("FFE600", "000", str);
                case Struct.Common.Intensity.Int2:
                    return SetMisskeyColor("90EE90", "000", str);
                case Struct.Common.Intensity.Int1:
                    return SetMisskeyColor("87CEEB", "000", str);
                case Struct.Common.Intensity.Int0:
                case Struct.Common.Intensity.Unknown:
                default:
                    return SetMisskeyColor("FFF","000",str);
            }
        }

        public async void EEWPost(Struct.EEW eew)
        {
            string TweetIndex = string.Empty;
            switch (eew.Serial.Infomation)
            {
                case Struct.EEW.InfomationLevel.Forecast:
                    TweetIndex += $"$[bg.color=44F $[fg.color=FFF 緊急地震速報(予報)]] 第 {eew.Serial.Number} 報 {(eew.Serial.IsFinal ? "(最終報)" : string.Empty)}\n";
                    break;
                case Struct.EEW.InfomationLevel.Warning:
                    TweetIndex += $"$[bg.color=F00 $[fg.color=FFF **緊急地震速報(警報)**]] 第 {eew.Serial.Number} 報 {(eew.Serial.IsFinal ? "(最終報)" : string.Empty)}\n";
                    break;
                case Struct.EEW.InfomationLevel.Cancelled:
                    TweetIndex += $"$[bg.color=4F4 $[fg.color=000 緊急地震速報(キャンセル)]]\n";
                    break;
                default:
                    return;
            }
            if (eew.Serial.Infomation != Struct.EEW.InfomationLevel.Cancelled)
            {
                TweetIndex += $"{eew.EarthQuake.Hypocenter} 深さ:{Struct.Common.DepthToString(eew.EarthQuake.Depth)} M{eew.EarthQuake.Magnitude:0.0}\n" +
                    $"{IntensityColor(eew.EarthQuake.MaxIntensity,$"最大震度 : {Struct.Common.IntToStringLong(eew.EarthQuake.MaxIntensity)}")}\n";
                if (eew.Serial.Infomation == Struct.EEW.InfomationLevel.Warning)
                {
                    TweetIndex += "$[bg.color=FF0 $[fg.color=F00 ⚠️以下の地域は強い揺れに注意⚠️]]\n";
                    string line = "**";
                    for (int i = 0; i < eew.EarthQuake.ForecastArea.Regions.Count; i++)
                    {
                        line += $"{eew.EarthQuake.ForecastArea.Regions[i]}";
                        if (i + 1 != eew.EarthQuake.ForecastArea.Regions.Count)
                        {
                            if (line.Length > 16)
                            {
                                TweetIndex += line + "\n";
                                line = "";
                            }
                            else
                            {
                                line += " ";
                            }
                        }
                    }
                    TweetIndex += $"{line}**\n\n";
                }
                TweetIndex += $"発生時刻 : <plain>{(eew.EarthQuake.OriginTime==DateTime.MinValue?"不明":$"{eew.EarthQuake.OriginTime:M/dd HH:mm:ss}")}</plain>\n";
            }
            else
            {
                TweetIndex += $"この緊急地震速報は取り消されました。\n";
            }
            TweetIndex += $"発表時刻 : <plain>{eew.Serial.UpdateTime:M/dd HH:mm:ss}</plain>\n";
            if (eew.Serial.Infomation == Struct.EEW.InfomationLevel.Warning)
            {
                TweetIndex += "\n**【エリア予測情報】**\n";
                for (int i = 0; i < eew.AreasInfo.Count; i++)
                {
                    string intensity = $"{IntensityColor(eew.AreasInfo[i].Intensity, $"震度{Struct.Common.IntToStringLong(eew.AreasInfo[i].Intensity).PadRight(2, 't').Replace("t", "  ")}")}";
                    TweetIndex += $"**{intensity}{(eew.AreasInfo[i].ExpectedArrival == DateTime.MinValue ? "$[bg.color=F00 $[fg.color=FFF 到達済み]]" : $"{eew.AreasInfo[i].ExpectedArrival:HH:mm:ss}")}** {eew.AreasInfo[i].Name}";
                    if (TweetIndex.Length > 2850||eew.AreasInfo.Count-1==i) break;
                    TweetIndex += "\n";
                }
                TweetIndex += "\n";
            }
            TweetIndex += $"#MisakiEQ #緊急地震速報";
            using (await EEW_Lock.LockAsync())
            {
                try
                {
                    EEWNote Current = new(eew.Serial.EventID, "", eew.Serial.Number);
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
                                //return;
                            }
                            Current= EEWReplyList[i];
                            Index = i;
                        }
                    }
                    var visibility = Lib.Misskey.Setting.Visibility.Home;
                    if (eew.Serial.Number == 1|| //第1報
                        eew.Serial.IsFinal|| // 最終報
                        (!Current.IsWarnFirstNoted &&//まだ規模が大きく変化した際の未発表の場合
                        (eew.EarthQuake.MaxIntensity>=Struct.Common.Intensity.Int5Down || // 震度5弱以上
                        eew.Serial.Infomation == Struct.EEW.InfomationLevel.Warning)) || // 警報
                        eew.Serial.Infomation == Struct.EEW.InfomationLevel.Cancelled ||//キャンセル
                        eew.EarthQuake.ForecastArea.LocalAreas.Count>Current.WarnAreaCount) //地名の数が更新されたとき
                    {
                        if(eew.Serial.Infomation == Struct.EEW.InfomationLevel.Warning)
                            Current.IsWarnFirstNoted = true;
                        visibility = Lib.Misskey.Setting.Visibility.Public;
                    }
                    Current.WarnAreaCount = eew.EarthQuake.ForecastArea.LocalAreas.Count;
                    var n = new SendNote();
                    n.Note = TweetIndex;
                    n.Visibility = visibility;
                    if (visibility == Lib.Misskey.Setting.Visibility.Public)
                    {
                        if(IsInterSend) EEWDelay.InterTask(n);
                        else
                        {
                            IsNextPublic= true;
                            EEWDelay.SendTask(n);
                        }
                    }
                    else
                    {
                        if (IsNextPublic) n.Visibility = Lib.Misskey.Setting.Visibility.Public;
                        EEWDelay.SendTask(n);
                    }
                    //n.responseNote = await Lib.Misskey.APIData.CreateNote(replyid: ""/*LatestID*/, text: TweetIndex, visibility: visibility);
                    //Log.Instance.Debug($"Noteしました。 ID:{n.responseNote}\n");

                    if (n.responseNote != string.Empty)
                    {
                        Current.LatestNote = n.responseNote;
                    }
                    Current.LatestSerial = eew.Serial.Number;
                    Current.LatestTime = DateTime.Now;
                    if (Index == -1)
                        EEWReplyList.Add(new(eew.Serial.EventID, LatestID, eew.Serial.Number));
                    
                    for (int i = EEWReplyList.Count - 1; i >= 0; i--)
                    {
                        TimeSpan T = DateTime.Now - EEWReplyList[i].LatestTime;
                        if (T.Seconds > 180) EEWReplyList.RemoveAt(i);
                    }
                }
                catch (Exception ex)
                {
                    Log.Instance.Warn($"Note中にエラー : {ex.Message}");
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
                            index.Add($"{IntensityColor(eq.Details.MaxIntensity, $"最大震度{Struct.Common.IntToStringLong(eq.Details.MaxIntensity)}を観測する地震が発生しました。")}");
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
                            index.Add($"{IntensityColor(eq.Details.MaxIntensity, $"最大震度: {Struct.Common.IntToStringLong(eq.Details.MaxIntensity)}")}");
                            index.Add($"この地震による{Struct.EarthQuake.DomesticToString(eq.Details.DomesticTsunami)}");
                            break;
                        case Struct.EarthQuake.EarthQuakeType.DetailScale:
                            index.Add($"詳細情報 - {eq.Details.OriginTime:M/dd H:mm}頃");
                            index.Add($"震源地: {eq.Details.Hypocenter}");
                            index.Add($"震源の深さ: {Struct.Common.DepthToString(eq.Details.Depth)}");
                            index.Add($"地震の規模: Ｍ{eq.Details.Magnitude:0.0}");
                            index.Add($"{IntensityColor(eq.Details.MaxIntensity, $"最大震度: {Struct.Common.IntToStringLong(eq.Details.MaxIntensity)}")}");
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
                                string txt = $"{IntensityColor(list[i].Intensity,$"震度{Struct.Common.IntToStringLong(list[i].Intensity)}")}：";
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
                        string id = await Lib.Misskey.APIData.CreateNote(TweetIndexs[i], Lib.Misskey.Setting.Visibility.Public , eqdata.LatestNote/*LatestID*/);
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
                        index.Add("**全ての津波予報が解除されました。**");
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
                                        index.Add("$[bg.color=FF0000 $[fg.color=FFFFFF **大津波警報 (まもなく到達)**]]");
                                        break;
                                    case 1:
                                        index.Add("$[bg.color=FF0000 $[fg.color=FFFFFF **大津波警報**]]");
                                        break;
                                    case 2:
                                        index.Add("$[bg.color=FF0000 $[fg.color=FFFFFF 津波警報 **(まもなく到達)**]]");
                                        break;
                                    case 3:
                                        index.Add("$[bg.color=FF0000 $[fg.color=FFFFFF 津波警報]]");
                                        break;
                                    case 4:
                                        index.Add("$[bg.color=FFFF00 $[fg.color=000000 津波注意報 **(まもなく到達)**]]");
                                        break;
                                    case 5:
                                        index.Add("$[bg.color=FFFF00 $[fg.color=000000 津波注意報]]");
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
                                if (cnt > 2950)
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
                        Latest = await Lib.Misskey.APIData.CreateNote(TweetList[i], Lib.Misskey.Setting.Visibility.Public,""/*Latest*/);
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
                        $"$[bg.color=FF0000 $[fg.color=FFFFFF **J-ALERT【{data.Title}】**]]",
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
                        Latest = await Lib.Misskey.APIData.CreateNote(TweetList[i],Lib.Misskey.Setting.Visibility.Public, ""/*Latest*/);
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
