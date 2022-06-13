using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MisakiEQ.Struct
{
    public class Tsunami
    {
        /// <summary> 
        /// <para>apiのjsonデータ単体から津波情報を汎用クラスに代入します。</para>
        /// <para>※この関数は津波情報のみ対応しています。</para>
        /// </summary>
        /// <param name="data">jsonデータ単体</param>
        /// <param name="from"><para>書き換え元の地震情報汎用クラス</para>
        /// <para>nullの場合は新規クラスを返します。</para></param>
        /// <returns>地震情報汎用クラス</returns>
        /// <exception cref="ArgumentException">津波情報と互換性が無いjsonデータが渡された時に発生します。</exception>
        public static Tsunami GetData(Background.API.EQInfo.JSON.Root data,Tsunami? from = null)
        {
            if (data.code != 552) throw new ArgumentException($"このコード値({data.code})は津波情報(552)と互換性がありません。");
            if (from == null) from = new();
            from.EventID = data.id;
            from.Cancelled = (bool)(data.cancelled!=null?data.cancelled:false);
            from.CreatedAt = Common.GetDateTime(data.time);
            from.Issue.Source = data.issue.source;
            from.Issue.Time = Common.GetDateTime(data.issue.time);
            from.Issue.Type = data.issue.type;
            from.Areas.Clear();
            for(int i = 0; i < data.areas.Count; i++)
            {
                var arg = new cTsunami.Areas
                {
                    Immediate = data.areas[i].immediate,
                    Name = data.areas[i].name
                };
                if (Enum.TryParse(typeof(TsunamiGrade), data.areas[i].grade, out var type))
                {
                    if (type != null)
                    {
                        arg.Grade = (TsunamiGrade)type;
                    }
                    else
                    {
                        Log.Logger.GetInstance().Warn($"areas[{i}].gradeを変換した後nullが返されました。 値 = {data.areas[i].grade}");
                        arg.Grade = TsunamiGrade.Unknown;
                    }
                }
                else
                {
                    Log.Logger.GetInstance().Warn($"areas[{i}].gradeの変換に失敗しました。 値 = {data.areas[i].grade}");
                    arg.Grade = TsunamiGrade.Unknown;
                }
                from.Areas.Add(arg);
            }
            return from;
        }
        /// <summary>
        /// 津波情報のEnumから文字列に変換します。
        /// </summary>
        /// <param name="grade">津波予報発令タイプ</param>
        /// <returns>日本語の文字列が返されます。</returns>
        public static string GradeToString(TsunamiGrade grade)
        {
            return grade switch
            {
                TsunamiGrade.Unknown => "不明",
                TsunamiGrade.Watch => "津波注意報",
                TsunamiGrade.Warning => "津波警報",
                TsunamiGrade.MajorWarning => "大津波警報",
                _ => $"コード:{grade}",
            };
        }
        /// <summary>
        /// 津波予報発令タイプ
        /// </summary>
        public enum TsunamiGrade
        {
            /// <summary>大津波警報</summary>
            MajorWarning,
            /// <summary>津波警報</summary>
            Warning,
            /// <summary>津波注意報</summary>
            Watch,
            /// <summary>不明</summary>
            Unknown
        }
        /// <summary>
        /// <para>イベントID</para>
        /// <para>情報毎に現れるランダムなIDです。</para>
        /// </summary>
        public string EventID = string.Empty;
        /// <summary>
        /// <para>情報作成時刻</para>
        /// <para>P2P地震情報API内のデータベース内に作られた時間です。</para>
        /// <para>存在しなければ DateTime.MinValue が返されます。</para>
        /// </summary>
        public DateTime CreatedAt = DateTime.MinValue;
        /// <summary>
        /// <para>津波予報発令解除フラグ</para>
        /// <para>発令解除したときはAreasが空の配列になります。</para>
        /// </summary>
        public bool Cancelled = false;

        public cTsunami.Issue Issue = new();
        /// <summary>
        /// <para>津波予報が発令されている地域と発令種類、直ちに来るかどうかのリストです。</para>
        /// </summary>
        public List<cTsunami.Areas> Areas = new();
    }
}
namespace MisakiEQ.Struct.cTsunami
{
    public class Issue
    {
        public string Source=string.Empty;
        public DateTime Time = DateTime.MinValue;
        public string Type = string.Empty;

    }
    public class Areas
    {
        /// <summary>地域名</summary>
        public string Name = string.Empty;
        /// <summary>すぐに到達するかのフラグ</summary>
        public bool Immediate = false;
        /// <summary>津波予報情報</summary>
        public Tsunami.TsunamiGrade Grade = Tsunami.TsunamiGrade.Unknown;

    }
}