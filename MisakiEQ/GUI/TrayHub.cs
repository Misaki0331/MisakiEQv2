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
                Instance.Close();
            }
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
            if (e.eew == null) return;
            e.eew.UserInfo.LocalIntensity= await Background.API.KyoshinAPI.KyoshinAPI.GetUserIntensity();
            Log.Logger.GetInstance().Debug($"緊急地震速報のイベントが発生: {e.eew.Serial.Infomation}");
            //ToDo: 緊急地震速報のイベント処理を入れる
            EEW_Compact.Invoke(() =>
            {
                EEW_Compact.SetInfomation(e.eew);
                EEW_Compact.Show();
                EEW_Compact.Activate();
            });
            Funcs.Toast.Post(e.eew);
        }
        

        private void EventEarthQuake(object? sender, Background.API.EarthQuakeEventArgs e)
        {
            if (e.data == null) return;
            Log.Logger.GetInstance().Debug($"地震情報のイベントが発生: {e.data.Details.OriginTime:d日HH:mm} {Struct.EarthQuake.TypeToString(e.data.Issue.Type)}");
            Funcs.Toast.Post(e.data);
        }
        private void EventTsunami(object? sender, Background.API.TsunamiEventArgs e)
        {
            if (e.data == null) return;
            Log.Logger.GetInstance().Debug($"津波情報のイベントが発生: {e.data.CreatedAt:d日HH:mm} 津波発表エリア数:{e.data.Areas.Count}件");
            Funcs.Toast.Post(e.data);
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
    }
}
