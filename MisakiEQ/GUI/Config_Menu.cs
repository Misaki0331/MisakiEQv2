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
        readonly Lib.ConfigController.Controller? controller;
#pragma warning restore IDE0052 // 読み取られていないプライベート メンバーを削除

        Twitter.Auth? TwitterAuthGUI = null;
        public Config_Menu()
        {
            InitializeComponent();
            var config = Lib.Config.Funcs.GetInstance().Configs;
            controller = new Lib.ConfigController.Controller(groupBox1, config.Connections);
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
            UpdateDataTimer.Start();
        }

        private void TestButton_CheckedChanged(object sender, EventArgs e)
        {
            Background.APIs.GetInstance().EEW.IsTest = TestButton.Checked;
        }
        private async void button1_Click(object sender, EventArgs e)
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
            }catch(Exception ex)
            {
                Log.Logger.GetInstance().Error(ex);
            }
        }
        private async void button3_Click(object sender, EventArgs e)
        {
            var id=await Lib.Twitter.APIs.GetInstance().Tweet(textBox2.Text);
            Log.Logger.GetInstance().Debug($"Tweet ID : {id}");
        }


        private void Config_Menu_FormClosed(object sender, FormClosedEventArgs e)
        {
        }

    }
}
