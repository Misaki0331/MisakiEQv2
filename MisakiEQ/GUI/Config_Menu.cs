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
        Sub.AuthDmdata? dmdataAuth = null;
        public Config_Menu()
        {
            InitializeComponent();
            var config = Lib.Config.Funcs.GetInstance().Configs;
            ConfigSetting = new Lib.ConfigController.Controller(tabPage1);
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
            LabelVersion.Text = $"バージョン : {Properties.Version.Name}";
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
                        Log.Instance.Error(ex);
                    }
                });
                fc.ButtonEnable = true;
            });
            Funcs.GUI.TwitterGUI.SetInfotoConfigUI();
#endif
            var fd = Lib.Config.Funcs.GetInstance().GetConfigClass("DMDATA_AuthFunction");
            fd?.SetAction(() =>
            {
                this.Invoke(() =>
                {
                    dmdataAuth = new();
                    dmdataAuth.ShowDialog(this);
                });
            });
            UpdatePos.Interval = 200;
            UpdatePos.Tick += UpdateGeo;
            var lat = Lib.Config.Funcs.GetInstance().GetConfigClass("USER_Pos_Lat");
            var lon = Lib.Config.Funcs.GetInstance().GetConfigClass("USER_Pos_Long");
            if(lat!=null)lat.SetAction(new Action(() => { if (!IsUpdatePosBusy) { UpdatePos.Stop(); UpdatePos.Start(); } }));
            if(lon!=null)lon.SetAction(new Action(() => { if (!IsUpdatePosBusy) { UpdatePos.Stop(); UpdatePos.Start(); } }));
            UpdateGeo(null, EventArgs.Empty);
//#if DEBUG
            var function = Lib.Config.Funcs.GetInstance().GetConfigClass("Debug_Function");
            function?.SetAction(() =>
            {
                Background.APIs.GetInstance().EEW.DMData.GetData(new FileStream(path:Lib.Config.Funcs.GetInstance().GetConfigClass("Debug_Input")?.Value.ToString()??"", FileMode.Open,FileAccess.Read));
            });
//#endif
        }

        System.Windows.Forms.Timer UpdatePos = new();
        bool IsUpdatePosBusy = false;
        async void UpdateGeo(object? sender,EventArgs e)
        {
            try
            {
                IsUpdatePosBusy = true;
                UpdatePos.Stop();
                var res = Lib.Config.Funcs.GetInstance().GetConfigClass("USER_Pos_Result");
                var lat = Lib.Config.Funcs.GetInstance().GetConfigClass("USER_Pos_Lat");
                var lon = Lib.Config.Funcs.GetInstance().GetConfigClass("USER_Pos_Long");
                if (lon != null && lat != null)
                {
                    if (res != null) res.Value = "地点取得中...";
                    var index = await Lib.PrefecturesAPI.API.GetReverseGeo(new((long)lon.Value / 10000.0, (long)lat.Value / 10000.0));
                    var result = index.Prefcity;
                    if (result == string.Empty)
                    {
                        if (res != null) res.Value = "この地点はご利用いただけません。";
                    }
                    else
                    {
                        if (res != null) res.Value = result;
                    }

                }
                IsUpdatePosBusy = false;
            }
            catch(Exception ex)
            {
                Log.Instance.Error(ex);
            }
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
            var usingapi = Lib.Config.Funcs.GetInstance().GetConfigClass("AppInfo_UsingAPI");
            if (usingapi != null) usingapi.Value = $"{Background.APIs.GetInstance().EEW.CurrentAPI}";
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
            var lat = Lib.Config.Funcs.GetInstance().GetConfigClass("USER_Pos_Lat");
            var lon = Lib.Config.Funcs.GetInstance().GetConfigClass("USER_Pos_Long");
            if (lat != null) lat.SetAction(new Action(() => { return; }));
            if (lon != null) lon.SetAction(new Action(() => { return; }));
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
            Log.Instance.Debug($"リサイズ開始");
            tabPage1.AutoScroll = false;
            tabPage1.AutoScrollOffset = new Point(0, 0);
            Log.Instance.Debug($"再リサイズ中 : {st.Elapsed}");
            SettingTabs.Size = new Size(Width - 7, Height - 121);
            tabPage1.AutoScroll = true;
            st.Stop();
            Log.Instance.Debug($"リサイズ完了 : {st.Elapsed}");
            SettingTabs.Visible = true;
            SizeChange.Stop();
        }
    }
}
