﻿using MisakiEQ.Funcs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MisakiEQ.GUI
{
    public partial class TrayHub : Form
    {
        readonly InitWindow? Init=null;
        Config_Menu? Config = null;
        ExApp.KyoshinWindow? Kyoshin = null;
        readonly EEW_Compact EEW_Compact = new();
        readonly ExApp.UserESTWindow ESTWindow = new();
        private TrayHub()
        {
            InitializeComponent();
            TrayIcon.Icon = Properties.Resources.Logo_MainIcon;
            Init = new();
            Init.Show();
            Background.APIs.GetInstance().EEW.UpdateHandler += EventEEW;
            Background.APIs.GetInstance().EQInfo.EarthQuakeUpdateHandler += EventEarthQuake;
            Background.APIs.GetInstance().EQInfo.TsunamiUpdateHandler += EventTsunami;
            EEW_Compact.Show();
            EEW_Compact.Hide();
            ESTWindow.Show();
            ESTWindow.Hide();
        }
        static TrayHub? Instance = null;

        public static TrayHub GetInstance()
        {
            if (Instance == null || Instance.IsDisposed)
            {
                Instance = new TrayHub();
            }
            return Instance;
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
            Log.Logger.GetInstance().Debug("APIスレッドを終了中です...");
            var ApiStop = Background.APIs.GetInstance().Abort();
            await ApiStop;
            Log.Logger.GetInstance().Debug("APIスレッド終了完了");
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
            Log.Logger.GetInstance().Debug("APIスレッドを終了中です...");
            var ApiStop = Background.APIs.GetInstance().Abort();
            await ApiStop;
            Log.Logger.GetInstance().Debug("APIスレッド終了完了");
            TrayIcon.Visible = false;
            Log.Logger.GetInstance().Info("Stop!");
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
            EEW_Compact.Show();
            EEW_Compact.SetInfomation(Background.APIs.GetInstance().EEW.GetData());
            EEW_Compact.Activate();
        }
        private async void EventEEW(object? sender,Background.API.EEWEventArgs e)
        {
            try
            {
                if (e.eew == null) return;
                double distance = Struct.EEW.GetDistance(e.eew, new(Background.APIs.GetInstance().KyoshinAPI.Config.UserLong, Background.APIs.GetInstance().KyoshinAPI.Config.UserLat));
                e.eew.UserInfo.ArrivalTime = e.eew.EarthQuake.OriginTime.AddSeconds(distance / 3.5);
#if DEBUG||ADMIN
                if (Lib.Twitter.APIs.GetInstance().Config.TweetEnabled) Tweets.GetInstance().EEWPost(e.eew);
#endif
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
                    EEW_Compact.Show();
                    EEW_Compact.Activate();
                });
                Toast.Post(e.eew);
                EventLog.EEW(e.eew);
                Funcs.DiscordRPC.PostEEW(e.eew);
                ESTWindow.ESTTime = e.eew.UserInfo.ArrivalTime;
                if (e.eew.UserInfo.LocalIntensity >= Struct.Common.Intensity.Int1)
                {
                    ESTWindow.Invoke(() =>
                    {
                        if (!ESTWindow.Visible)
                        {
                            ESTWindow.Show();
                            ESTWindow.Location = new Point(0,214);
                        }
                        ESTWindow.Activate();
                    });
                }
                await SoundCollective.GetInstance().SoundEEW(e.eew);
            }catch(Exception ex)
            {
                Log.Logger.GetInstance().Error(ex);
            }
        }
        

        private void EventEarthQuake(object? sender, Background.API.EarthQuakeEventArgs e)
        {
            try
            {
                if (e.data == null) return;
                Log.Logger.GetInstance().Debug($"地震情報のイベントが発生: {e.data.Details.OriginTime:d日HH:mm} {Struct.EarthQuake.TypeToString(e.data.Issue.Type)}");
#if DEBUG||ADMIN
                if (Lib.Twitter.APIs.GetInstance().Config.TweetEnabled) Tweets.GetInstance().EarthquakePost(e.data);
#endif
                Toast.Post(e.data);
                Funcs.DiscordRPC.PostEarthquake(e.data);
                SoundCollective.SoundEarthquake(e.data);
            }
            catch (Exception ex)
            {
                Log.Logger.GetInstance().Error(ex);
            }
        }
        private void EventTsunami(object? sender, Background.API.TsunamiEventArgs e)
        {
            try
            {
                if (e.data == null) return;
                Log.Logger.GetInstance().Debug($"津波情報のイベントが発生: {e.data.CreatedAt:d日HH:mm} 津波発表エリア数:{e.data.Areas.Count}件");
#if DEBUG||ADMIN
                if (Lib.Twitter.APIs.GetInstance().Config.TweetEnabled) Tweets.GetInstance().TsunamiPost(e.data);
#endif
                Toast.Post(e.data);
                SoundCollective.SoundTsunami(e.data);
            }
            catch (Exception ex)
            {
                Log.Logger.GetInstance().Error(ex);
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
                ESTWindow.Location = new Point(0,214);
            }
            ESTWindow.Activate();
        }

    }
}
