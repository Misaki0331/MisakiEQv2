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
#pragma warning disable IDE0052 // 読み取られていないプライベート メンバーを削除
        readonly Lib.ConfigController.Controller? ConnectionSetting;
        readonly Lib.ConfigController.Controller? UserSetting;
        readonly Lib.ConfigController.Controller? SNSSetting;
#pragma warning restore IDE0052 // 読み取られていないプライベート メンバーを削除

        Twitter.Auth? TwitterAuthGUI = null;
        public Config_Menu()
        {
            InitializeComponent();
            var config = Lib.Config.Funcs.GetInstance().Configs;
            ConnectionSetting = new Lib.ConfigController.Controller(groupBox1, config.Connections);
            UserSetting = new Lib.ConfigController.Controller(groupBox2, config.UserSetting);
            SNSSetting = new Lib.ConfigController.Controller(groupBox3, config.SNSSetting);

            TwitterAuthInfo.Text = $"@{Lib.Twitter.APIs.GetInstance().GetUserScreenID()} - {Lib.Twitter.APIs.GetInstance().GetUserName()} " +
            $"(Follower:{Lib.Twitter.APIs.GetInstance().GetUserFollowers()} Tweet:{Lib.Twitter.APIs.GetInstance().GetUserTweets()})";
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

        private async void button1_Click(object sender, EventArgs e)
        {
            (await Lib.Sounds.GetInstance().GetSound("EEW_None")).Replay();
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }
    }
}
