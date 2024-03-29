﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MisakiEQ.Struct
{
    internal class J_Alert
    {
        static public cJAlert.J_Alert GetJAlertData(string Title, string RawText)
        {
            try
            {
                var alertdata = new cJAlert.J_Alert();
                if (string.IsNullOrWhiteSpace(Title)||string.IsNullOrWhiteSpace(RawText))
                {
                    return new();
                }
                alertdata.Title = Title;
                var texts = RawText.Split('\n');
                int len1 = -1, len2 = -1, len3 = -1; //【】内の情報
                for (int i = 0; i < texts.Length; i++)
                {
                    switch (texts[i])
                    {
                        case "【発表時間】":
                            len1 = i;
                            break;
                        case "【内容】":
                            len2 = i;
                            break;
                        case "【対象地域】":
                            len3 = i;
                            break;
                    }
                }
                if (len1 != -1)
                {
                    DateTime.TryParse(texts[len1 + 1].Replace("年", "/")
                        .Replace("月", "/")
                        .Replace("日", "")
                        .Replace("時", ":")
                        .Replace("分", ""), out alertdata.AnnounceTime);
                    alertdata.SourceName = texts[len1 + 2];
                }
                for(int i = len2 + 1; i < len3 - 1; i++)
                {
                    alertdata.Detail += texts[i];
                    if (i != len3 - 2) alertdata.Detail += "\n";
                }
                for (int i = len3 + 1; i < texts.Length; i++)
                {
                    alertdata.Areas.Add(texts[i].Trim());
                }
                alertdata.IsValid = true;
                Log.Debug($"Title : {alertdata.Title}\nTime : {alertdata.AnnounceTime}\nSource : {alertdata.SourceName}\nDetail : {alertdata.Detail}\nAreas : {alertdata.Areas.Count}");
                return alertdata;
            }
            catch(Exception ex)
            {
                Log.Error(ex);
            }
            return new();
        } 
    }

}
namespace MisakiEQ.Struct.cJAlert
{
    ///<summary>Jアラート情報</summary>
    public class J_Alert
    {
        public bool IsValid = false;
        ///<summary>タイトル名</summary>
        public string Title = string.Empty;
        ///<summary>発表時刻</summary>
        public DateTime AnnounceTime = DateTime.MinValue;
        /// <summary>発表元</summary>
        public string SourceName = string.Empty;
        /// <summary>詳細情報</summary>
        public string Detail = string.Empty;
        /// <summary>対象地域</summary>
        public List<string> Areas = new();
    }
}
