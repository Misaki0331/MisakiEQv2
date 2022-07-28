using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#pragma warning disable IDE1006 // 命名スタイル

namespace MisakiEQ.Background.API.EEW.OLD.JSON
{
    public class Title
    {
        /// <summary>
        /// <para>1回出現</para>
        /// <para>電文種別コード</para>
        /// </summary>
        public int Code { get; set; }
        /// <summary>
        /// <para>1回出現</para>
        /// <para>「緊急地震速報（予報）」 / 「緊急地震速報（警報）」</para>
        /// </summary>
        public string String { get; set; } = string.Empty;
        /// <summary>
        /// <para>1回出現</para>
        /// <para>Title.Code の説明文</para>
        /// </summary>
        public string Detail { get; set; } = string.Empty;
    }

    public class Source
    {
        /// <summary>
        /// <para>1回出現</para>
        /// <para>発信官署コード</para>
        /// </summary>
        public int Code { get; set; }
        /// <summary>
        /// <para>1回出現</para>
        /// <para>発信官署名</para>
        /// </summary>
        public string String { get; set; } = string.Empty;
    }

    public class Status
    {
        /// <summary>
        /// <para>1回出現</para>
        /// <para>識別符コード</para>
        /// </summary>
        public string Code { get; set; } = string.Empty;
        /// <summary>
        /// <para>1回出現</para>
        /// <para>大まかな識別情報</para>
        /// </summary>
        public string String { get; set; } = string.Empty;
        /// <summary>
        /// <para>1回出現</para>
        /// <para>詳細の識別情報</para>
        /// </summary>
        public string Detail { get; set; } = string.Empty;
    }

    public class AnnouncedTime
    {
        /// <summary>
        /// <para>1回出現</para>
        /// <para>「yyyy/MM/dd HH:mm:ss」 の形式で記載</para>
        /// </summary>
        public string String { get; set; } = string.Empty;
        /// <summary>
        /// <para>1回出現</para>
        /// <para>UnixTime の形式で記載</para>
        /// </summary>
        public int UnixTime { get; set; }
        /// <summary>
        /// <para>1回出現</para>
        /// <para>「ddd, dd MMM yyyy HH:mm:ss 'UTC'」 の形式で記載</para>
        /// </summary>
        public string RFC1123 { get; set; } = string.Empty;
    }

    public class OriginTime
    {
        /// <summary>
        /// <para>1回出現</para>
        /// <para>「yyyy/MM/dd HH:mm:ss」 の形式で記載</para>
        /// </summary>
        public string String { get; set; } = string.Empty;
        /// <summary>
        /// <para>1回出現</para>
        /// <para>UnixTime の形式で記載</para>
        /// </summary>
        public int UnixTime { get; set; }
        /// <summary>
        /// <para>1回出現</para>
        /// <para>「ddd, dd MMM yyyy HH:mm:ss 'UTC'」 の形式で記載</para>
        /// </summary>
        public string RFC1123 { get; set; } = string.Empty;
    }

    public class Type
    {
        /// <summary>
        /// <para>1回出現</para>
        /// <para>状況識別コード</para>
        /// </summary>
        public int Code { get; set; }
        /// <summary>
        /// <para>1回出現</para>
        /// <para>大まかな発表状況</para>
        /// </summary>
        public string String { get; set; } = string.Empty;
        /// <summary>
        /// <para>1回出現</para>
        /// <para>詳細な発表状況</para>
        /// </summary>
        public string Detail { get; set; } = string.Empty;
    }

    public class Depth
    {
        /// <summary>
        /// <para>1回出現</para>
        /// <para>int型での震源の深さ (単位はkm)</para>
        /// </summary>
        public int Int { get; set; }
        /// <summary>
        /// <para>1回出現</para>
        /// <para>震源の深さ</para>
        /// </summary>
        public string String { get; set; } = string.Empty;
    }
    public class AccDepth
    {

        /// <summary>
        /// <para>1回出現</para>        
        /// <para>震源の深さの確からしさのコード</para>
        /// </summary>
        public int Code { get; set; }
        /// <summary>
        /// <para>1回出現</para>
        /// <para>震源の深さの確からしさの説明</para>
        /// </summary>
        public string String { get; set; } = string.Empty;
    }

    public class Location
    {
        /// <summary>
        /// <para>1回出現</para>
        /// <para>震源地の緯度</para>
        /// </summary>
        public double Lat { get; set; }
        /// <summary>
        /// <para>1回出現</para>
        /// <para>震源地の経度</para>
        /// </summary>
        public double Long { get; set; }
        /// <summary>
        /// <para>1回出現</para>
        /// <para>震源の深さに関する情報</para>
        /// </summary>
        public Depth Depth { get; set; } = new();
    }

    public class Magnitude
    {
        /// <summary>
        /// <para>1回出現</para>
        /// <para>float形式でのマグニチュード</para>
        /// </summary>
        public double Float { get; set; }
        /// <summary>
        /// <para>1回出現</para>
        /// <para>マグニチュード</para>
        /// </summary>
        public string String { get; set; } = string.Empty;
        /// <summary>
        /// <para>1回出現</para>
        /// <para>このバージョンでは Hypocenter.Magnitude.String と同じもの</para>
        /// </summary>
        public string LongString { get; set; } = string.Empty;
        /// <summary>
        /// <para>1回出現</para>
        /// <para>このバージョンでは Hypocenter.Magnitude.String と同じもの</para>
        /// </summary>
        public int Code { get; set; }
    }


    public class AccMagnitude
    {
        /// <summary>
        /// <para>1回出現</para>
        /// <para>マグニチュードの確からしさのコード</para>
        /// </summary>
        public int Code { get; set; }
        /// <summary>
        /// <para>1回出現</para>
        /// <para>マグニチュードの確からしさの説明</para>
        /// </summary>
        public string String { get; set; } = string.Empty;
    }
    public class Epicenter
    {
        /// <summary>
        /// <para>1回出現</para>
        /// <para>震央の確からしさのコード</para>
        /// </summary>
        public int Code { get; set; }
        /// <summary>
        /// <para>1回出現</para>
        /// <para>震央の確からしさの説明</para>
        /// </summary>
        public string String { get; set; } = string.Empty;
        /// <summary>
        /// <para>1回出現</para>
        /// <para>震源の深さの確からしさのコード (気象庁の部内システムでの利用)</para>
        /// </summary>
        public int Rank2 { get; set; }
        /// <summary>
        /// <para>1回出現</para>
        /// <para>震源の深さの確からしさの説明 (気象庁の部内システムでの利用)</para>
        /// </summary>
        public string String2 { get; set; } = string.Empty;
    }

    public class Accuracy
    {
        /// <summary>
        /// <para>1回出現</para>
        /// <para>震央の確からしさ</para>
        /// </summary>
        public Epicenter Epicenter { get; set; } = new();
        /// <summary>
        /// <para>1回出現</para>
        /// <para>震源の深さの確からしさ</para>
        /// </summary>
        public AccDepth Depth { get; set; } = new();
        /// <summary>
        /// <para>1回出現</para>
        /// <para>マグニチュードの確からしさ</para>
        /// </summary>
        public AccMagnitude Magnitude { get; set; } = new();
        /// <summary>
        /// <para>1回出現</para>
        /// <para>マグニチュード計算使用観測点数 (気象庁の部内システムでの利用)</para>
        /// </summary>
        public int NumberOfMagnitudeCalculation { get; set; }
    }

    public class Hypocenter
    {
        /// <summary>
        /// <para>1回出現</para>
        /// <para>震源地コード</para>
        /// </summary>
        public int Code { get; set; }
        /// <summary>
        /// <para>1回出現</para>
        /// <para>震源地名</para>
        /// </summary>
        public string Name { get; set; } = string.Empty;
        /// <summary>
        /// <para>1回出現</para>
        /// <para>PLUM法による予想かどうか</para>
        /// </summary>
        public bool isAssumption { get; set; }
        /// <summary>
        /// <para>1回出現</para>
        /// <para>震源地に関する情報</para>
        /// </summary>
        public Location Location { get; set; } = new();
        /// <summary>
        /// <para>1回出現</para>
        /// <para>マグニチュードに関する情報</para>
        /// </summary>
        public Magnitude Magnitude { get; set; } = new();
        /// <summary>
        /// <para>1回出現</para>
        /// <para>正確さ情報</para>
        /// </summary>
        public Accuracy Accuracy { get; set; } = new();
        /// <summary>
        /// <para>1回出現</para>
        /// <para>震源が海域かどうか</para>
        /// </summary>
        public bool isSea { get; set; }
    }

    public class MaxIntensity
    {
        /// <summary>
        /// <para>1回出現</para>
        /// <para>震度の下限</para>
        /// </summary>
        public string From { get; set; } = string.Empty;
        /// <summary>
        /// <para>1回出現</para>
        /// <para>震度の上限</para>
        /// </summary>
        public string To { get; set; } = string.Empty;
        /// <summary>
        /// <para>1回出現</para>
        /// <para>震度</para>
        /// </summary>
        public string String { get; set; } = string.Empty;
        /// <summary>
        /// <para>1回出現</para>
        /// <para>詳細の震度</para>
        /// </summary>
        public string LongString { get; set; } = string.Empty;
    }

    public class ForeHypocenter
    {
        /// <summary>
        /// <para>1回出現</para>
        /// <para>震源地コード/para>
        /// </summary>
        public int Code { get; set; }
        /// <summary>
        /// <para>1回出現</para>
        /// <para>震源地名</para>
        /// </summary>
        public string String { get; set; } = string.Empty;
    }
    public class WarnForecast
    {
        /// <summary>
        /// <para>1回出現</para>
        /// <para>震源地に関する情報</para>
        /// </summary>
        public ForeHypocenter Hypocenter { get; set; } = new();
        /// <summary>
        /// <para>1回出現</para>
        /// <para>警報対象地域 (地方単位)</para>
        /// </summary>
        public List<string> District { get; set; } = new();
        /// <summary>
        /// <para>1回出現</para>
        /// <para>警報対象地域 (都道府県単位)</para>
        /// </summary>
        public List<string> LocalAreas { get; set; } = new();
        /// <summary>
        /// <para>1回出現</para>
        /// <para>警報対象地域 (地域単位)</para>
        /// </summary>
        public List<string> Regions { get; set; } = new();
    }

    public class Reason
    {
        /// <summary>
        /// <para>1回出現</para>
        /// <para>変化の理由のコード</para>
        /// </summary>
        public int Code { get; set; }
        /// <summary>
        /// <para>1回出現</para>
        /// <para>変化の理由の説明</para>
        /// </summary>
        public string String { get; set; } = string.Empty;
    }

    public class Change
    {
        /// <summary>
        /// <para>1回出現</para>
        /// <para>最大予測震度の変化のコード</para>
        /// </summary>
        public int Code { get; set; }
        /// <summary>
        /// <para>1回出現</para>
        /// <para>最大予測震度の変化の説明</para>
        /// </summary>
        public string String { get; set; } = string.Empty;
        /// <summary>
        /// <para>1回出現</para>
        /// <para>変化の理由に関する情報</para>
        /// </summary>
        public Reason Reason { get; set; } = new();
    }

    public class Option
    {
        /// <summary>
        /// <para>1回出現</para>
        /// <para>変化に関する情報</para>
        /// </summary>
        public Change Change { get; set; } = new();
    }

    public class Intensity
    {
        /// <summary>
        /// <para>1回出現</para>
        /// <para>対象地域コード</para>
        /// </summary>
        public int Code { get; set; }
        /// <summary>
        /// <para>1回出現</para>
        /// <para>対象地域名</para>
        /// </summary>
        public string Name { get; set; } = string.Empty;
        /// <summary>
        /// <para>1回出現</para>
        /// <para>震度の下限</para>
        /// </summary>
        public string From { get; set; } = string.Empty;
        /// <summary>
        /// <para>1回出現</para>
        /// <para>震度の上限</para>
        /// </summary>
        public string To { get; set; } = string.Empty;
        /// <summary>
        /// <para>1回出現</para>
        /// <para>震度</para>
        /// </summary>
        public string Description { get; set; } = string.Empty;
    }

    public class Arrival
    {
        /// <summary>
        /// <para>0/1回出現 (PLUM法による予想の場合は出現しない)</para>
        /// <para>揺れが到達しているかどうか</para>
        /// </summary>
        public bool? Flag { get; set; }
        /// <summary>
        /// <para>1回出現</para>
        /// <para>到達状況に関する情報</para>
        /// </summary>
        public string Condition { get; set; } = string.Empty;
        /// <summary>
        /// <para>0/1回出現 (PLUM法による予想の場合は出現しない)</para>
        /// <para>揺れが到達しているかどうか</para>
        /// </summary>
        public string? Time { get; set; } = string.Empty;
    }

    public class Forecast
    {
        /// <summary>
        /// <para>1回出現</para>
        /// <para>対象地域と震源に関する情報</para>
        /// </summary>
        public Intensity Intensity { get; set; } = new();
        /// <summary>
        /// <para>1回出現</para>
        /// <para>この地域が警報対象かどうか</para>
        /// </summary>
        public bool Warn { get; set; }
        /// <summary>
        /// <para>1回出現</para>
        /// <para>到達状況に関する情報</para>
        /// </summary>
        public Arrival Arrival { get; set; } = new();
    }

    public class Root
    {
        /// <summary>
        /// <para>1回出現</para>
        /// <para>Success / Error</para>
        /// </summary>
        public string ParseStatus { get; set; } = string.Empty;
        /// <summary>
        /// <para>1回出現</para>
        /// <para>緊急地震速報のタイトル情報</para>
        /// </summary>
        public Title Title { get; set; } = new();
        /// <summary>
        /// <para>1回出現</para>
        /// <para>発信官署情報</para>
        /// </summary>
        public Source Source { get; set; } = new();
        /// <summary>
        /// <para>1回出現</para>
        /// <para>発訓練等の識別符</para>
        /// </summary>
        public Status Status { get; set; } = new();
        /// <summary>
        /// <para>1回出現</para>
        /// <para>配信時刻情報</para>
        /// </summary>
        public AnnouncedTime AnnouncedTime { get; set; } = new();
        /// <summary>
        /// <para>1回出現</para>
        /// <para>発生時刻情報</para>
        /// </summary>
        public OriginTime OriginTime { get; set; } = new();
        /// <summary>
        /// <para>1回出現</para>
        /// <para>地震識別ID</para>
        /// </summary>
        public string EventID { get; set; } = string.Empty;
        /// <summary>
        /// <para>1回出現</para>
        /// <para>発表状況情報</para>
        /// </summary>
        public Type Type { get; set; } = new();
        /// <summary>
        /// <para>1回出現</para>
        /// <para>情報番号</para>
        /// </summary>
        public int Serial { get; set; }
        /// <summary>
        /// <para>0/1回出現 キャンセル時は出現しない</para>
        /// <para>震源に関する情報</para>
        /// </summary>
        public Hypocenter? Hypocenter { get; set; } = new();
        /// <summary>
        /// <para>0/1回出現 キャンセル時は出現しない</para>
        /// <para>震度に関する情報</para>
        /// </summary>
        public MaxIntensity? MaxIntensity { get; set; } = new();
        /// <summary>
        /// <para>1回出現</para>
        /// <para>警報かどうか</para>
        /// </summary>
        public bool Warn { get; set; }
        /// <summary>
        /// <para>0/1回出現 (予報では出現しない)</para>
        /// <para>警報対象地域に関する情報</para>
        /// </summary>
        public WarnForecast? WarnForecast { get; set; } = new();
        /// <summary>
        /// <para>0/1回出現 キャンセル時は出現しない</para>
        /// <para>最大予測震度の変化に関する情報</para>
        /// </summary>
        public Option? Option { get; set; } = new();
        /// <summary>
        /// <para>0/1回出現 (予報では出現しない)</para>
        /// <para>警報対象地域それぞれに関する情報</para>
        /// </summary>
        public List<Forecast> Forecast { get; set; } = new();
        /// <summary>
        /// <para>1回出現</para>
        /// <para>電文</para>
        /// </summary>
        public string OriginalText { get; set; } = string.Empty;
    }
}
