using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MisakiEQ.Funcs.GUI
{
    internal class TwitterGUI
    {
        public static void SetInfotoConfigUI()
        {
            var twi = Lib.Twitter.APIs.GetInstance();
            var fc = Lib.Config.Funcs.GetInstance().GetConfigClass("Twitter_Auth_Info");
            if (twi.GetUserScreenID() != null)
            {
                if (fc != null) fc.SetValue("認証済");
                fc = Lib.Config.Funcs.GetInstance().GetConfigClass("Twitter_Auth_UserID");
                if (fc != null) fc.SetValue($"@{twi.GetUserScreenID()}");
                fc = Lib.Config.Funcs.GetInstance().GetConfigClass("Twitter_Auth_UserName");
                if (fc != null) fc.SetValue($"{twi.GetUserName()}");
                fc = Lib.Config.Funcs.GetInstance().GetConfigClass("Twitter_Auth_Tweet");
                if (fc != null) fc.SetValue($"{twi.GetUserTweets()}");
                fc = Lib.Config.Funcs.GetInstance().GetConfigClass("Twitter_Auth_Follower");
                if (fc != null) fc.SetValue($"{twi.GetUserFollowers()}");
            }
            else
            {
                if (fc != null) fc.SetValue("認証失敗");
                fc = Lib.Config.Funcs.GetInstance().GetConfigClass("Twitter_Auth_UserID");
                if (fc != null) fc.SetValue($"");
                fc = Lib.Config.Funcs.GetInstance().GetConfigClass("Twitter_Auth_UserName");
                if (fc != null) fc.SetValue($"");
                fc = Lib.Config.Funcs.GetInstance().GetConfigClass("Twitter_Auth_Tweet");
                if (fc != null) fc.SetValue($"");
                fc = Lib.Config.Funcs.GetInstance().GetConfigClass("Twitter_Auth_Follower");
                if (fc != null) fc.SetValue($"");
            }
        }
    }
}
