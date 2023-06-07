using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace MisakiEQ.Funcs
{
    public class EventLog
    {
        public static void J_ALERT(Struct.cJAlert.J_Alert j)
        {
            string data = $"J-ALERTが発令されました。\n" +
                $"発報時刻:{j.AnnounceTime}\n" +
                $"発信元:{j.SourceName}\n" +
                $"タイトル:{j.Title}\n" +
                $"詳細:{j.Detail}\n" +
                $"配信エリア:{j.Areas.Count}地域\n";
            for(int i = 0; i < j.Areas.Count; i++)
            {
                data += $"{j.Areas[i]}";
                if (i != j.Areas.Count - 1) data += " ";
            }
            EventLogOut(1, data);
        }
        public static bool EEW(Struct.EEW eew)
        {
            string data = $"緊急地震速報が発報されました。\n" +
                $"発報時刻:{eew.Serial.UpdateTime}\n" +
                $"発報の種類:{eew.Serial.Infomation}\n" +
                $"情報番号:{eew.Serial.Number}{(eew.Serial.IsFinal ? "(最終報)" : string.Empty)}\n" +
                $"震源地:{eew.EarthQuake.Hypocenter}\n" +
                $"震源の深さ:{Struct.Common.DepthToString(eew.EarthQuake.Depth)}\n" +
                $"地震の規模:M{eew.EarthQuake.Magnitude:0.0}\n" +
                $"ユーザーの予測震度:{(eew.UserInfo.IntensityRaw<0?0:eew.UserInfo.IntensityRaw):0.0}" +
                $"(震度{Struct.Common.IntToStringLong(eew.UserInfo.LocalIntensity)})\n";
            if (eew.Serial.Infomation == Struct.EEW.InfomationLevel.Warning)
                foreach (var area in eew.AreasInfo)
                    data += $"{area.Name} - 予測震度{Struct.Common.IntToStringLong(area.Intensity)} " +
                        $"{(area.ExpectedArrival == DateTime.MinValue ? "到達済みの可能性" : area.ExpectedArrival)}\n";
            return EventLogOut(1, data);
        }
        static bool EventLogOut(short id, string data)
        {
            try
            {
                string sourceName = "MisakiEQ";
                System.Diagnostics.EventLog.WriteEntry(
                    sourceName, data,
                    System.Diagnostics.EventLogEntryType.Information, id, 32767);
                return true;
            }
            catch (SecurityException)
            {
                return false;
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return false;
            }
        }
    }
}
