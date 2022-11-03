﻿using System;
using System.Collections.Generic;
using System.Linq;
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
        public static void EEW(Struct.EEW eew)
        {
            string data = $"緊急地震速報が発報されました。\n" +
                $"発報時刻:{eew.Serial.UpdateTime}\n" +
                $"発報の種類:{eew.Serial.Infomation}\n" +
                $"情報番号:{eew.Serial.Number}{(eew.Serial.IsFinal ? "(最終報)" : string.Empty)}\n" +
                $"震源地:{eew.EarthQuake.Hypocenter}\n" +
                $"震源の深さ:{Struct.Common.DepthToString(eew.EarthQuake.Depth)}\n" +
                $"地震の規模:M{eew.EarthQuake.Magnitude:0.0}\n" +
                $"ユーザーの予測震度:{(eew.UserInfo.IntensityRaw<0?0:eew.UserInfo.IntensityRaw):0.0}(震度{Struct.Common.IntToStringLong(eew.UserInfo.LocalIntensity)})\n";
            if (eew.Serial.Infomation == Struct.EEW.InfomationLevel.Warning)
            {
                for (int i = 0; i < eew.AreasInfo.Count; i++)
                {
                    data += $"{eew.AreasInfo[i].Name} - 予測震度{Struct.Common.IntToStringLong(eew.AreasInfo[i].Intensity)} {(eew.AreasInfo[i].ExpectedArrival == DateTime.MinValue ? "到達済みの可能性" : eew.AreasInfo[i].ExpectedArrival)}\n";
                }
            }
            EventLogOut(1, data);
        }
        static void EventLogOut(short id, string data)
        {
            try
            {
                string sourceName = "MisakiEQ";
                if (!System.Diagnostics.EventLog.SourceExists(sourceName))
                {
                    System.Diagnostics.EventLog.CreateEventSource(sourceName, "");
                }
                System.Diagnostics.EventLog.WriteEntry(
                    sourceName, data,
                    System.Diagnostics.EventLogEntryType.Information, id, 32767);
            }
            catch (Exception ex)
            {
                Log.Instance.Error(ex);
            }
        }
    }
}
