using MisakiEQ.Funcs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MisakiEQ.GUI
{
    public partial class TrayHub : Form
    {
        readonly Overlay.FullScreenWarning J_ALERT_Display = new();
        readonly InitWindow? Init = null;
        internal readonly Config ConfigData = new();
        Config_Menu? Config = null;
        ExApp.KyoshinWindow? Kyoshin = null;
        List<ExApp.KyoshinGraphWindow.Main> KyoshinResponseGraph = new();
        readonly EEW_Compact EEW_Compact = new();
        readonly ExApp.UserESTWindow ESTWindow = new();
        readonly Stopwatch apptimer = new();
        ExApp.J_ALERTDataWindow? JAlert = null;
        public TimeSpan AppTimer { get => apptimer.Elapsed; }

        private readonly object WindowLock = new();

        private TrayHub()
        {
            apptimer.Start();
            Instance = this;
            InitializeComponent();
            TrayIcon.Icon = Properties.Resources.Logo_MainIcon;
            Init = new();
            Init.Show();
            EEW_Compact.Show();
            EEW_Compact.Hide();
            ESTWindow.Show();
            ESTWindow.Hide();
            J_ALERT_Display.Init();
            Lib.ToastNotification.InitNotify(TrayIcon);
#if DEBUG
            実行ログToolStripMenuItem_Click("debug", EventArgs.Empty);
#endif
            versionName.Text = $"{Properties.Version.Name}";
#if ADMIN
            versionName.Text += "(Admin)";
#elif DEBUG
            versionName.Text += "(Debug)";
#endif
        }
        static TrayHub? Instance = null;

        public static TrayHub? GetInstance(bool IsCreate = false)
        {
            if (IsCreate && Instance == null) Instance = new();
            return Instance;
        }
        public void SetEvent()
        {
            Background.APIs.GetInstance().EEW.UpdateHandler -= EventEEW;
            Background.APIs.GetInstance().EQInfo.EarthQuakeUpdateHandler -= EventEarthQuake;
            Background.APIs.GetInstance().EQInfo.TsunamiUpdateHandler -= EventTsunami;
            Background.APIs.GetInstance().EEW.UpdateHandler += EventEEW;
            Background.APIs.GetInstance().EQInfo.EarthQuakeUpdateHandler += EventEarthQuake;
            Background.APIs.GetInstance().EQInfo.TsunamiUpdateHandler += EventTsunami;
            Background.APIs.GetInstance().Jalert.J_AlertUpdateHandler += EventJAlert;
        }
        public void ResetEventEEW()
        {
            Background.APIs.GetInstance().EEW.UpdateHandler -= EventEEW;
            Background.APIs.GetInstance().EEW.UpdateHandler += EventEEW;
        }
        public static void DisposeInstance()
        {
            if (Instance != null && !Instance.IsDisposed)
            {
                Instance.ErrorClosing();
                Instance.Close();

            }
        }
        public async void ErrorClosing()
        {
            TrayIcon.Visible = false;
            if (Config != null && !Config.IsDisposed) Config.Close();
            if (Kyoshin != null && !Kyoshin.IsDisposed) Kyoshin.Close();
            if (EEW_Compact != null && !EEW_Compact.IsDisposed) EEW_Compact.Close();
            if (Init != null && !Init.IsDisposed) Init.Close();
            Log.Instance.Debug("APIスレッドを終了中です...");
            var ApiStop = Background.APIs.GetInstance().Abort();
            await ApiStop;
            Log.Instance.Debug("APIスレッド終了完了");
        }
        public static bool IsAlliveInstance()
        {
            if (Instance == null || Instance.IsDisposed)
                return false;
            return true;
        }

        private async void ExitApplication_Click(object sender, EventArgs e)
        {
            if (Init != null && !Init.IsDisposed) Init.Close();
            Log.Instance.Debug("APIスレッドを終了中です...");
            var ApiStop = Background.APIs.GetInstance().Abort();
            await ApiStop;
            Log.Instance.Debug("APIスレッド終了完了");
            TrayIcon.Visible = false;
            Log.Instance.Info("Stop!");
            Environment.Exit(0);
        }

        private void OpenConfig_Click(object sender, EventArgs e)
        {
            if (Config == null || Config.IsDisposed) Config = new();
            Config.Show();
            Config.Activate();
        }

        private void DisplayEEWInfo_Click(object sender, EventArgs e)
        {
            EEW_Compact.TopMost = ConfigData.IsTopSimpleEEW;
            EEW_Compact.Show();
            EEW_Compact.SetInfomation(Background.APIs.GetInstance().EEW.GetData());
            EEW_Compact.Activate();
        }
        private async void EventEEW(object? sender, Background.API.EEWEventArgs e)
        {
            try
            {
                Log.Instance.Debug("EEWイベント受信");
                if (e.eew == null) return;
                double distance = Struct.EEW.GetDistance(e.eew, new(Background.APIs.GetInstance().KyoshinAPI.Config.UserLong, Background.APIs.GetInstance().KyoshinAPI.Config.UserLat));
                e.eew.UserInfo.ArrivalTime = e.eew.EarthQuake.OriginTime.AddSeconds(distance / 4.2);
                Log.Instance.Debug($"距離 = {distance} km");
#if DEBUG||ADMIN
                if (Lib.Twitter.APIs.GetInstance().Config.TweetEnabled) Tweets.GetInstance().EEWPost(e.eew);
#endif
                e.eew.UserInfo.LocalIntensity = await Background.API.KyoshinAPI.KyoshinAPI.GetUserIntensity();
                e.eew.UserInfo.IntensityRaw = await Background.API.KyoshinAPI.KyoshinAPI.GetUserRawIntensity();
                Funcs.EventLog.EEW(e.eew);
                Log.Instance.Debug($"イベントログ書き込み完了");
                Funcs.DiscordRPC.PostEEW(e.eew);
                Log.Instance.Debug($"DiscordRPC書き込み完了");
                ESTWindow.ESTTime = e.eew.UserInfo.ArrivalTime;
                lock (WindowLock)
                {
                    if (((int)ConfigData.NoticeNationWide <= (int)e.eew.EarthQuake.MaxIntensity ||
                    (ConfigData.NoticeNationWide == Struct.ConfigBox.Notification_EEW_Nationwide.Enums.WarnOnly && e.eew.Serial.Infomation == Struct.EEW.InfomationLevel.Warning) ||
                    (int)ConfigData.NoticeArea <= (int)e.eew.UserInfo.LocalIntensity)||
                    ConfigData.NoticeNationWide==Struct.ConfigBox.Notification_EEW_Nationwide.Enums.ALL)
                {
                    Toast.Post(e.eew);
                        /*
                        if (e.eew.UserInfo.LocalIntensity >= Struct.Common.Intensity.Int1)
                        {
                            ESTWindow.Invoke(() =>
                            {
                                if (!ESTWindow.Visible)
                                {
                                    ESTWindow.Show();
                                    ESTWindow.Location = new Point(0, 214);
                                }
                                ESTWindow.Activate();
                            });
                        }*/
                        var a=SoundCollective.GetInstance().SoundEEW(e.eew).Result;
                        Log.Instance.Debug($"サウンド再生処理完了");
                    }

                    EEW_Compact.Invoke(() =>
                    {
                        if (!EEW_Compact.Visible && !EEW_Compact.IsShowFromEEW)
                        {
                            EEW_Compact.IsShowFromEEW = true;
                            EEW_Compact.HideTimer.Start();
                        }
                        else if (EEW_Compact.IsShowFromEEW)
                        {
                            EEW_Compact.HideTimer.Stop();
                            EEW_Compact.HideTimer.Start();
                        }
                        EEW_Compact.SetInfomation(e.eew);
                        if (ConfigData.IsWakeSimpleEEW)
                        {
                            if ((int)ConfigData.NoticeNationWide <= (int)e.eew.EarthQuake.MaxIntensity ||
                        (ConfigData.NoticeNationWide == Struct.ConfigBox.Notification_EEW_Nationwide.Enums.WarnOnly
                        && e.eew.Serial.Infomation == Struct.EEW.InfomationLevel.Warning) ||
                        (int)ConfigData.NoticeArea <= (int)e.eew.UserInfo.LocalIntensity)
                            {
                                EEW_Compact.TopMost = ConfigData.IsTopSimpleEEW;
                                EEW_Compact.Show();
                                EEW_Compact.Activate();
                            }
                        }
                    });
                }
                Log.Instance.Debug($"EEW_Compact書き込み完了");
            }
            catch (Exception ex)
            {
                Log.Instance.Error(ex);
            }
        }

        private void EventJAlert(object? sender, Background.API.J_AlertEventArgs e)
        {
            try
            {
                if (e.data == null||!e.data.IsValid) return;
                Log.Instance.Debug($"Jアラートのイベントが発生: {e.data.AnnounceTime:d日HH:mm} {e.data.Title}");
                #if DEBUG||ADMIN
                if (Lib.Twitter.APIs.GetInstance().Config.TweetEnabled&& Lib.Twitter.APIs.GetInstance().Config.IsTweetJ_ALERT) Tweets.GetInstance().JALERTPost(e.data);
#endif
                Funcs.DiscordRPC.PostJAlert(e.data);
                Funcs.EventLog.J_ALERT(e.data);
                if (Background.APIs.GetInstance().Jalert.Config.IsDisplay)
                {
                    Toast.Post(e.data);
                    J_ALERT_Display.Invoke(()=> { J_ALERT_Display.TopShow(); });
                }
                if (JAlert != null && !JAlert.IsDisposed) JAlert.Invoke(() => { JAlert.UpdateData(); });
            }
            catch(Exception ex)
            {
                Log.Instance.Error(ex);
            }
        }
        private void EventEarthQuake(object? sender, Background.API.EarthQuakeEventArgs e)
        {
            try
            {
                if (e.data == null) return;
                Log.Instance.Debug($"地震情報のイベントが発生: {e.data.Details.OriginTime:d日HH:mm} {Struct.EarthQuake.TypeToString(e.data.Issue.Type)}");
#if DEBUG||ADMIN
                if (Lib.Twitter.APIs.GetInstance().Config.TweetEnabled) Tweets.GetInstance().EarthquakePost(e.data);
#endif
                Toast.Post(e.data);
                Funcs.DiscordRPC.PostEarthquake(e.data);
                SoundCollective.SoundEarthquake(e.data);
            }
            catch (Exception ex)
            {
                Log.Instance.Error(ex);
            }
        }
        private void EventTsunami(object? sender, Background.API.TsunamiEventArgs e)
        {
            try
            {
                if (e.data == null) return;
                Log.Instance.Debug($"津波情報のイベントが発生: {e.data.CreatedAt:d日HH:mm} 津波発表エリア数:{e.data.Areas.Count}件");
#if DEBUG||ADMIN
                if (Lib.Twitter.APIs.GetInstance().Config.TweetEnabled) Tweets.GetInstance().TsunamiPost(e.data);
#endif
                Toast.Post(e.data);
                SoundCollective.SoundTsunami(e.data);
            }
            catch (Exception ex)
            {
                Log.Instance.Error(ex);
            }
        }

        private void TrayIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {

        }

        private void ApplicationMenu_Opening(object sender, CancelEventArgs e)
        {

        }

        private void OpenKmoni_Click(object sender, EventArgs e)
        {

            if (Kyoshin == null || Kyoshin.IsDisposed) Kyoshin = new();
            Kyoshin.Show();
            Kyoshin.Activate();
        }

        private void OpenAreaESTMonitor_Click(object sender, EventArgs e)
        {
            if (!ESTWindow.Visible)
            {
                ESTWindow.Show();
                ESTWindow.Location = new Point(0, 214);
            }
            ESTWindow.Activate();
        }
        public void KyoshinResponseGraphRelease()
        {
            for(int i = KyoshinResponseGraph.Count - 1; i >= 0; i--)
            {
                if (KyoshinResponseGraph[i].IsDisposed)
                {
                    KyoshinResponseGraph.RemoveAt(i);
                    Log.Instance.Debug($"グラフウィンドウ#{i+1}を削除しました。");
                }
            }
        }
        public ExApp.KyoshinGraphWindow.Main KyoshinResponseGraphCreate(int value=-1)
        {
            KyoshinResponseGraphRelease();
            if (KyoshinResponseGraph.Count > 0 && (value < 0 || value > 9999))
            {
                value = KyoshinResponseGraph[KyoshinResponseGraph.Count - 1].ConfigNumber+1;
                if (value > 9999) value = -1;
            }
            var app = new ExApp.KyoshinGraphWindow.Main(value);
            KyoshinResponseGraph.Add(app);
            KyoshinResponseGraph[KyoshinResponseGraph.Count - 1].Show();
            KyoshinResponseGraph[KyoshinResponseGraph.Count - 1].Activate();
            Log.Instance.Debug($"グラフウィンドウ#{KyoshinResponseGraph.Count}を作成しました。");
            return app;
        }
        LogViewerWindow? LogViewer = null;
        private void 実行ログToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (LogViewer == null || LogViewer.IsDisposed)
                {
                    LogViewer = new();
                    LogViewer.Show();
                }
                LogViewer.WindowState = FormWindowState.Normal;
                LogViewer.Activate();
            }
            catch (Exception ex)
            {
                Log.Instance.Error(ex);
            }
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (JAlert == null || JAlert.IsDisposed)
            {
                JAlert = new();
                JAlert.Show();
                JAlert.Activate();
            }
            else
            {
                JAlert.Show();
                JAlert.WindowState=FormWindowState.Normal;
                JAlert.Activate();
            }
        }

        private void KyoshinGraphToolStripMenuItem_Click(object sender, EventArgs e)
        {
            KyoshinResponseGraphRelease();
            if (KyoshinResponseGraph.Count == 0)
            {
                var window = KyoshinResponseGraphCreate(0);
                window.Show();
                window.Activate();
            }
            else
            {
                KyoshinResponseGraph[KyoshinResponseGraph.Count - 1].Show();
                KyoshinResponseGraph[KyoshinResponseGraph.Count - 1].Activate();
            }
        }
    }
}
