using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Web;
using System.Management;
using System.IO;
namespace MisakiEQ.GUI.ErrorInfo
{
    public partial class UnhandledException : Form
    {
        readonly string index;
        int ErrCnt;
        int RestartTimerCount = 20;
        string SpecText = "";
        void GetString()
        {

            ManagementClass mc = new("Win32_OperatingSystem");
            var moc = mc.GetInstances();
            int cnt = 1;
            foreach (var mo in moc)
            {
                SpecText += $"- OS情報 No.{cnt} -\n"
                    + $"エディション : {mo["Caption"]}\n"
                    + $"バージョン : {mo["Version"]}\n"
                    + $"ビルド番号 : {mo["BuildNumber"]}\n"
                    + $"空き物理メモリ : {Convert.ToInt32(mo["FreePhysicalMemory"]) / 1024.0:#,##0.00} MB / "
                    + $"{Convert.ToInt32(mo["TotalVisibleMemorySize"]) / 1024.0:#,##0.00} MB\n"
                    + $"空き仮想メモリ : {Convert.ToInt32(mo["FreeVirtualMemory"]) / 1024.0:#,##0.00} MB / "
                    + $"{Convert.ToInt32(mo["TotalVirtualMemorySize"]) / 1024.0:#,##0.00} MB\n\n";
                cnt++;
                mo.Dispose();
            }
            SpecText += "\n";

            mc = new("Win32_ComputerSystemProduct");
            moc = mc.GetInstances();
            cnt = 1;
            foreach (var mo in moc)
            {
                SpecText += $"- コンピュータ情報 No.{cnt} -\n"
                    + $"製造会社名 : {mo.Properties["Vendor"].Value}\n"
                    + $"モデル名 : {mo.Properties["Name"].Value}\n"
                    + $"モデルバージョン : {mo.Properties["Version"].Value}\n"
                    + $"PCの概要 : {mo.Properties["Caption"].Value}\n\n";
                cnt++;
                mo.Dispose();
            }
            SpecText += "\n";

            mc = new("CIM_System");
            moc = mc.GetInstances();
            cnt = 1;
            foreach (var mo in moc)
            {
                SpecText += $"- 利用者情報 No.{cnt} -\n"
                    + $"ドメイン : {mo.Properties["Domain"].Value}\n"
                    + $"PCの名称 : {mo.Properties["Vendor"].Value}\n"
                    + $"管理者 : {mo.Properties["PrimaryOwnerName"].Value}\n\n";
                mo.Dispose();
                cnt++;
            }
            SpecText += $"使用ユーザー : {Environment.UserName}\n\n";
            mc = new("Win32_Processor");
            moc = mc.GetInstances();
            cnt = 1;
            foreach (ManagementObject mo in moc)
            {
                SpecText += $"- CPU情報 No.{cnt} -\n"
                    + $"デバイスID : {mo["DeviceID"]}\n"
                    + $"CPU型番 : {mo["Name"]}\n"
                    + $"最大クロック周波数 : {mo["MaxClockSpeed"]} MHz\n"
                    + $"L2キャッシュサイズ : {mo["L2CacheSize"]} KB\n\n";
                cnt++;
            }
        }
        private void CrashReport()
        {
            try
            {
                var save = $"ErrorLog/{DateTime.Now:yyyyMMdd-HHmmssfff}.txt";
                var dir = Path.GetDirectoryName(save);
                if (!Directory.Exists(dir) && dir != null) Directory.CreateDirectory(dir);
                var Enc = Encoding.GetEncoding("UTF-8");
                var writer = new StreamWriter(save, true, Enc);
                writer.WriteLine($"MisakiEQはエラーが発生したため、動作を停止しました。");
                writer.WriteLine($"発生時刻 : {DateTime.Now}");
                writer.WriteLine($"----------エラーの内容----------");
                writer.WriteLine(index.Replace("\r", string.Empty).Replace("\n", Environment.NewLine));
                writer.WriteLine($"----------ユーザー環境----------");
                writer.WriteLine(SpecText.Replace("\r", string.Empty).Replace("\n", Environment.NewLine));
                writer.WriteLine($"MisakiEQが起動された実行ファイル : {Environment.ProcessPath ?? "該当なし"}");
                writer.WriteLine($"MisakiEQの実行フォルダ : {Environment.CurrentDirectory}");
                writer.WriteLine($"MisakiEQのバージョン : {Properties.Version.Name}");
                writer.WriteLine($"正規OSバージョン名 : {Environment.OSVersion}");
                writer.WriteLine($"ランタイムバージョン名 : {Environment.Version}");
                writer.WriteLine($"OS : {(Environment.Is64BitOperatingSystem ? "64ビット" : "32ビット")} プロセッサ : {(Environment.Is64BitProcess ? "64ビット" : "32ビット")}");
                writer.WriteLine($"システム起動時間 : {TimeSpan.FromMilliseconds(Environment.TickCount64)}");
                writer.Close();

                Log.Logger.GetInstance().Info($"「{save}」にクラッシュレポートを保存しました。");
            }
            catch(Exception ex)
            {
                Log.Logger.GetInstance().Error("クラッシュレポートの保存に失敗しました。");
                Log.Logger.GetInstance().Error(ex);
            }

        }
        public UnhandledException(string str, int ErrorCount = 0, string CrashMethod = "")
        {
            InitializeComponent();
            Icon = Properties.Resources.Logo_MainIcon;
            string[] result = str.Split(new char[] { '\n' });
            ErrorIndex.Lines = result;
            index = str;
            ErrCnt = ErrorCount;
            if (ErrCnt >= 2)
            {
                ErrorInfomation.Text = "MisakiEQは " + (ErrCnt + 1).ToString() + " 回連続で\n予期しないエラーが発生しました。";
                if (ErrCnt > 4) ErrorInfomation.ForeColor = System.Drawing.Color.Red;
                label2.Text = "特定の手順で発生する場合は開発者にご報告ください。";
            }
            if (CrashMethod == "Void CauseException()")
            {
                ErrorInfomation.Text = "MisakiEQを意図的にクラッシュさせました。";
                ErrorInfomation.ForeColor = System.Drawing.Color.Black;
                label2.Text = "水咲ちゃんはとても困っているようです。";
                UserReport.Text = "意図的にクラッシュさせたため無効";
                UserReport.ReadOnly = true;
                UserReport.Enabled = false;
                button1.Enabled = false;
                label4.Text = "";
            }
            if (ErrCnt >= 9)
            {
                RestartTimer.Stop();
                RestartMassage.Text = "繰り返し動作が停止しましたので自動再起動は無効になりました。";
                Log.Logger.GetInstance().Error("強制終了が複数回検出された為自動再起動は無効になりました。");
            }
            GetString();
            CrashReport();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string UserReportStr = string.Join("\n", UserReport.Text);
            if (UserReport.Text == "") UserReportStr = "[未入力]\n";
            string link = HttpUtility.UrlEncode($"----------ユーザー報告----------\n"
                    + $"{UserReportStr}\n"
                    + $"----------エラーの内容----------\n"
                    + $"{index}\n" 
                    + $"----------ユーザー環境----------\n"
                    + $"{SpecText}\n"
                    + $"MisakiEQのバージョン\n" 
                    + $"{Properties.Version.Name}");
            System.Diagnostics.Process.Start("https://twitter.com/messages/compose?recipient_id=1129403055374340101&text=" + link);

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Log.Logger.GetInstance().Info("STOP!");
            Environment.Exit(-1);
        }

        private void ExceptionMassage_FormClosing(object sender, FormClosingEventArgs e)
        {
            Log.Logger.GetInstance().Info("STOP!");
            Environment.Exit(-1);
        }

        private void RestartButton_Click(object sender, EventArgs e)
        {
            ErrCnt++;
            Log.Logger.GetInstance().Info("RESTART!");
            System.Diagnostics.Process.Start(Application.ExecutablePath, "ErrorFlg=" + ErrCnt.ToString());
            Environment.Exit(-1);
        }

        private void UserReport_MouseClick(object sender, MouseEventArgs e)
        {
            RestartMassage.Text = "";
            RestartTimer.Stop();
        }

        private void RestartTimer_Tick(object sender, EventArgs e)
        {
            RestartTimerCount--;

            if (RestartTimerCount < 0)
            {
                ErrCnt++;
                System.Diagnostics.Process.Start(Application.ExecutablePath, "ErrorFlg=" + ErrCnt.ToString());
                Log.Logger.GetInstance().Info("TRIGGER RESTART!");
                Environment.Exit(-1);
            }
            RestartMassage.Text = "MisakiEQは " + RestartTimerCount.ToString() + " 秒後、自動的に再起動します。";
        }
    }
}
