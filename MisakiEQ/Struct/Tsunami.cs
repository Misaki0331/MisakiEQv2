using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MisakiEQ.Struct
{
    public class Tsunami
    {
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
        public enum TsunamiGrade
        {
            MajorWarning,
            Warning,
            Watch,
            Unknown
        }
        public string EventID = string.Empty;
        public DateTime CreatedAt = DateTime.MinValue;
        public bool Cancelled = false;
        public cTsunami.Issue Issue = new();
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
        public string Name = string.Empty;
        public bool Immediate = false;
        public Tsunami.TsunamiGrade Grade = Tsunami.TsunamiGrade.Unknown;

    }
}