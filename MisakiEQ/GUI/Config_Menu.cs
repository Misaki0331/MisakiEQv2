using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MisakiEQ.GUI
{
    public partial class Config_Menu : Form
    {
        readonly Lib.ConfigController.Controller? ConfigSetting;

        Twitter.Auth? TwitterAuthGUI = null;
        public Config_Menu()
        {
            InitializeComponent();
            var config = Lib.Config.Funcs.GetInstance().Configs;
            ConfigSetting = new Lib.ConfigController.Controller(tabPage1);
            LabelVersion.Text = $"バージョン : {Properties.Version.Name}";
            Icon = Properties.Resources.Logo_MainIcon;
#if ADMIN
            LabelVersion.Text += "(Admin)";
#elif DEBUG
            LabelVersion.Text += "(Debug)";
#endif
#if ADMIN || DEBUG
            var fc = Lib.Config.Funcs.GetInstance().GetConfigClass("Twitter_Auth");
            fc?.SetAction(() =>
            {
                fc.ButtonEnable = false;
                Process.Start(new ProcessStartInfo(Lib.Twitter.APIs.GetInstance().GetAuthURL().Result)
                {
                    UseShellExecute = true
                });
                this.Invoke(() =>
                {
                    try
                    {
                        if (TwitterAuthGUI != null && TwitterAuthGUI.Visible) TwitterAuthGUI.Close();
                        TwitterAuthGUI = new();
                        TwitterAuthGUI.ShowDialog();
                        var twi = Lib.Twitter.APIs.GetInstance();
                        fc = Lib.Config.Funcs.GetInstance().GetConfigClass("Twitter_Auth_Info");
                        if (twi.GetUserScreenID() != null)
                        {
                            if (fc != null) fc.Value = "認証済";
                            fc = Lib.Config.Funcs.GetInstance().GetConfigClass("Twitter_Auth_UserID");
                            if (fc != null) fc.Value = $"@{twi.GetUserScreenID()}";
                            fc = Lib.Config.Funcs.GetInstance().GetConfigClass("Twitter_Auth_UserName");
                            if (fc != null) fc.Value = $"{twi.GetUserName()}";
                            fc = Lib.Config.Funcs.GetInstance().GetConfigClass("Twitter_Auth_Tweet");
                            if (fc != null) fc.Value = $"{twi.GetUserTweets()}";
                            fc = Lib.Config.Funcs.GetInstance().GetConfigClass("Twitter_Auth_Follower");
                            if (fc != null) fc.Value = $"{twi.GetUserFollowers()}";
                        }
                        else
                        {
                            if (fc != null) fc.Value = "認証失敗";
                            fc = Lib.Config.Funcs.GetInstance().GetConfigClass("Twitter_Auth_UserID");
                            if (fc != null) fc.Value = $"";
                            fc = Lib.Config.Funcs.GetInstance().GetConfigClass("Twitter_Auth_UserName");
                            if (fc != null) fc.Value = $"";
                            fc = Lib.Config.Funcs.GetInstance().GetConfigClass("Twitter_Auth_Tweet");
                            if (fc != null) fc.Value = $"";
                            fc = Lib.Config.Funcs.GetInstance().GetConfigClass("Twitter_Auth_Follower");
                            if (fc != null) fc.Value = $"";
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.Logger.GetInstance().Error(ex);
                    }
                });
                fc.ButtonEnable = true;
            });
            TwitterAuthInfo.Text = $"@{Lib.Twitter.APIs.GetInstance().GetUserScreenID()} - {Lib.Twitter.APIs.GetInstance().GetUserName()} " +
            $"(Follower:{Lib.Twitter.APIs.GetInstance().GetUserFollowers()} Tweet:{Lib.Twitter.APIs.GetInstance().GetUserTweets()})";
            AuthTwitter.Visible = true;
            TweetBox.Visible = true;
            TweetButton.Visible = true;
            var twi = Lib.Twitter.APIs.GetInstance();
            fc = Lib.Config.Funcs.GetInstance().GetConfigClass("Twitter_Auth_Info");
            if (twi.GetUserScreenID() != null)
            {
                if(fc!=null)fc.Value = "認証済";
                fc = Lib.Config.Funcs.GetInstance().GetConfigClass("Twitter_Auth_UserID");
                if (fc != null) fc.Value = $"@{twi.GetUserScreenID()}";
                fc = Lib.Config.Funcs.GetInstance().GetConfigClass("Twitter_Auth_UserName");
                if (fc != null) fc.Value = $"{twi.GetUserName()}";
                fc = Lib.Config.Funcs.GetInstance().GetConfigClass("Twitter_Auth_Tweet");
                if (fc != null) fc.Value = $"{twi.GetUserTweets()}";
                fc = Lib.Config.Funcs.GetInstance().GetConfigClass("Twitter_Auth_Follower");
                if (fc != null) fc.Value = $"{twi.GetUserFollowers()}";
            }
            else
            {
                if (fc != null) fc.Value = "未認証"; 
                fc = Lib.Config.Funcs.GetInstance().GetConfigClass("Twitter_Auth_UserID");
                if (fc != null) fc.Value = $"";
                fc = Lib.Config.Funcs.GetInstance().GetConfigClass("Twitter_Auth_UserName");
                if (fc != null) fc.Value = $"";
                fc = Lib.Config.Funcs.GetInstance().GetConfigClass("Twitter_Auth_Tweet");
                if (fc != null) fc.Value = $"";
                fc = Lib.Config.Funcs.GetInstance().GetConfigClass("Twitter_Auth_Follower");
                if (fc != null) fc.Value = $"";
            }
#else
            AuthTwitter.Visible = false;
            TweetBox.Visible = false;
            TweetButton.Visible = false;
#endif
        }



        private void ButtonApply_Click(object sender, EventArgs e)
        {
            Lib.Config.Funcs.GetInstance().SaveConfig();
            Lib.Config.Funcs.GetInstance().ApplyConfig();
        }

        private void ButtonOK_Click(object sender, EventArgs e)
        {
            Lib.Config.Funcs.GetInstance().SaveConfig();
            Lib.Config.Funcs.GetInstance().ApplyConfig();
            Close();
        }

        private void ButtonCancel_Click(object sender, EventArgs e)
        {
            Lib.Config.Funcs.GetInstance().DiscardConfig();
            Close();
        }

        private void UpdateDataTimer_Tick(object sender, EventArgs e)
        {
            LabelDate.Text = DateTime.Now.ToString("yyyy/MM/dd (ddd)");
            LabelTime.Text = DateTime.Now.ToString("HH:mm:ss");
        }

        private void Config_Menu_Load(object sender, EventArgs e)
        {
            UpdateDataTimer.Interval = 150;
            UpdateDataTimer.Start();
        }
        private void Config_Menu_FormClosed(object sender, FormClosedEventArgs e)
        {
            Lib.Config.Funcs.GetInstance().DiscardConfig();
            ConfigSetting?.FormEventDispose();
        }

        private async void OpenAuthTwitter(object sender, EventArgs e)
        {
            try
            {
                var task = Lib.Twitter.APIs.GetInstance().GetAuthURL();
                await task;
                Process.Start(new ProcessStartInfo(task.Result)
                {
                    UseShellExecute = true
                });
                if (TwitterAuthGUI != null && TwitterAuthGUI.Visible) TwitterAuthGUI.Close();
                TwitterAuthGUI = new();
                TwitterAuthGUI.ShowDialog();

                TwitterAuthInfo.Text = $"@{Lib.Twitter.APIs.GetInstance().GetUserScreenID()} - {Lib.Twitter.APIs.GetInstance().GetUserName()} " +
                $"(Follower:{Lib.Twitter.APIs.GetInstance().GetUserFollowers()} Tweet:{Lib.Twitter.APIs.GetInstance().GetUserTweets()})";
            }
            catch (Exception ex)
            {
                Log.Logger.GetInstance().Error(ex);
            }
        }

        private async void SendTweet(object sender, EventArgs e)
        {
            var id = await Lib.Twitter.APIs.GetInstance().Tweet(TweetBox.Text);
            Log.Logger.GetInstance().Debug($"Tweet ID : {id}");
        }

        private async void FixKyoshinTime_Click(object sender, EventArgs e)
        {
            await Background.APIs.GetInstance().KyoshinAPI.FixKyoshinTime();
        }

        private void LinkToGitHub_Click(object sender, EventArgs e)
        {
            OpenLink("https://github.com/Misaki0331/MisakiEQv2");
        }

        private void LinkToTwitterBot_Click(object sender, EventArgs e)
        {
            OpenLink("https://twitter.com/MisakiEQ");
        }

        private void LinkToDevTwitter_Click(object sender, EventArgs e)
        {
            OpenLink("https://twitter.com/0x7FF");
        }

        private void LinkToKoFi_Click(object sender, EventArgs e)
        {
            OpenLink("https://ko-fi.com/misaki0331");
        }
        private static void OpenLink(string url)
        {
            ProcessStartInfo pi = new()
            {
                FileName = url,
                UseShellExecute = true,
            };
            Process.Start(pi);
        }

        private void Config_Menu_SizeChanged(object sender, EventArgs e)
        {
            SizeChange.Stop();
            SizeChange.Start();
            //796,596
            LabelTime.Location = new Point(Width - 140, 27);
            LabelDate.Location = new Point(Width - 203, 0);
            ButtonOK.Location = new Point(Width - 255, Height-64);
            ButtonCancel.Location = new Point(Width - 175, Height - 64);
            ButtonApply.Location = new Point(Width-95, Height - 64);
        }

        private void SizeChange_Tick(object sender, EventArgs e)
        {
            Stopwatch st = new();
            st.Start();
            Log.Logger.GetInstance().Debug($"リサイズ開始");
            tabPage1.AutoScroll = false;
            tabPage1.AutoScrollOffset = new Point(0, 0);
            Log.Logger.GetInstance().Debug($"再リサイズ中 : {st.Elapsed}");
            SettingTabs.Size = new Size(Width - 7, Height - 121);
            tabPage1.AutoScroll = true;
            st.Stop();
            Log.Logger.GetInstance().Debug($"リサイズ完了 : {st.Elapsed}");
            SizeChange.Stop();
        }
    }
}
