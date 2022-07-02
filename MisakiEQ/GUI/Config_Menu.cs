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
        readonly List<Funcs.ConfigUI.LinkButton> LinkButtons = new();
        public Config_Menu()
        {
            InitializeComponent();
            var config = Lib.Config.Funcs.GetInstance().Configs;
            ConfigSetting = new Lib.ConfigController.Controller(tabPage1);
            LabelVersion.Text = $"バージョン : {Properties.Version.Name}";
            Icon = Properties.Resources.Logo_MainIcon;
            string[] objects = {"GitHub", "https://github.com/Misaki0331/MisakiEQv2",
            "Twitter Bot","https://twitter.com/MisakiEQ",
            "Devs Twitter","https://twitter.com/0x7FF",
            "Ko-Fi","https://ko-fi.com/misaki0331"
            };
            for (int i = 0; i < objects.Length; i += 2)
            {
                var a = new Funcs.ConfigUI.LinkButton(tabPage3, new(48 * i, 157), objects[i], objects[i + 1]);
                LinkButtons.Add(a);
            }
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
                        Funcs.GUI.TwitterGUI.SetInfotoConfigUI();
                    }
                    catch (Exception ex)
                    {
                        Log.Logger.GetInstance().Error(ex);
                    }
                });
                fc.ButtonEnable = true;
            });
            Funcs.GUI.TwitterGUI.SetInfotoConfigUI();
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
            var uptime = Lib.Config.Funcs.GetInstance().GetConfigClass("AppInfo_Uptime");
            if (uptime != null) uptime.Value = $"{TrayHub.GetInstance()?.AppTimer.ToString(@"dd\.hh\:mm\:ss")}";
            var kyoshin = Background.APIs.GetInstance().KyoshinAPI;
            uptime = Lib.Config.Funcs.GetInstance().GetConfigClass("Kyoshin_Time");
            if (kyoshin.KyoshinLatest.Year > 2000)
            {
                if (uptime != null) uptime.Value = $"{kyoshin.KyoshinLatest:yyyy/MM/dd HH:mm:ss}";
            }
            else
            {
                if (uptime != null) uptime.Value = "不明";
            }
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
            SettingTabs.Visible = false;
            Log.Logger.GetInstance().Debug($"リサイズ開始");
            tabPage1.AutoScroll = false;
            tabPage1.AutoScrollOffset = new Point(0, 0);
            Log.Logger.GetInstance().Debug($"再リサイズ中 : {st.Elapsed}");
            SettingTabs.Size = new Size(Width - 7, Height - 121);
            tabPage1.AutoScroll = true;
            st.Stop();
            Log.Logger.GetInstance().Debug($"リサイズ完了 : {st.Elapsed}");
            SettingTabs.Visible = true;
            SizeChange.Stop();
        }
    }
}
