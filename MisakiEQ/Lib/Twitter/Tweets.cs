using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MisakiEQ.Lib.Twitter
{
    internal class Tweets
    {
        private class EEWTweet {
            public EEWTweet(string Event,long Latest)
            {
                LatestTime=DateTime.Now;
                EventID = Event;
                LatestTweet = Latest;
            }
            public string EventID { get; set; }=string.Empty;
            public long LatestTweet { get; set; } = 0;
            public DateTime LatestTime { get; set; }= DateTime.Now;
        }
        private static Tweets? singleton = null;
        /// <summary>
        /// インスタンスを生成する
        /// </summary>
        public static Tweets GetInstance()
        {
            if (singleton == null)
            {
                singleton = new Tweets();
            }
            return singleton;
        }
        readonly List<EEWTweet> EEWReplyList=new();
        private readonly Lib.AsyncLock EEW_Lock = new();
        public async void EEWPost(Struct.EEW eew)
        {
            string TweetIndex =string.Empty;
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
                TweetIndex += $"{eew.EarthQuake.Hypocenter} 深さ: {Struct.Common.DepthToString(eew.EarthQuake.Depth)} M {eew.EarthQuake.Magnitude}\n";
                TweetIndex += $"最大震度 : {Struct.Common.IntToStringLong(eew.EarthQuake.MaxIntensity)}\n";
                TweetIndex += $"発生時刻 : {eew.EarthQuake.OriginTime:M/dd HH:mm:ss}\n";
                if (eew.Serial.Infomation == Struct.EEW.InfomationLevel.Warning)
                {
                    TweetIndex += "\n⚠️以下の地域は強い揺れに注意⚠️\n";
                    for(int i = 0; i < eew.EarthQuake.ForecastArea.LocalAreas.Count; i++)
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
                            Index = i;

                        }
                    }
                    var twitter = APIs.GetInstance();
                    LatestID = await twitter.Tweet(reply: LatestID, tweet: "@null "+TweetIndex);
                    if (Index!=-1)
                    {
                        EEWReplyList[Index].LatestTweet = LatestID;
                        EEWReplyList[Index].LatestTime = DateTime.Now;
                    }else
                    {
                        EEWReplyList.Add(new(eew.Serial.EventID, LatestID));
                    }
                    for(int i= EEWReplyList.Count-1; i>=0; i--)
                    {
                        TimeSpan T = DateTime.Now - EEWReplyList[i].LatestTime;
                        if (T.Seconds > 180) EEWReplyList.RemoveAt(i);
                    }
                }catch(Exception ex)
                {
                    Log.Logger.GetInstance().Warn($"ツイート中にエラー : {ex.Message}");
                }
            }
        }
    }
}
