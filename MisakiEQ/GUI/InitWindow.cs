using System.Diagnostics;

namespace MisakiEQ
{
    public partial class InitWindow : Form
    {
        public InitWindow()
        {
            InitializeComponent();
            Icon = Properties.Resources.Logo_MainIcon;
        }
        private void InitWindow_Load(object sender, EventArgs e)
        {
            Lib.Animator.Animate(150, (frame, frequency) =>
            {
                if (!Visible || IsDisposed) return false;
                Opacity = (double)frame / frequency;
                return true;
            });
            InitialTask.RunWorkerAsync();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
        }

        private void InitialTask_ReportFunction(int percent,string report,Action action,Stopwatch stopwatch)
        {
            InitialTask.ReportProgress(percent, report+"中...");
            Log.Instance.Debug($"{stopwatch.Elapsed.TotalSeconds} - {percent}% {report}中...");
            action();
            InitialTask.ReportProgress(percent, report+"完了");
            Log.Instance.Debug($"{stopwatch.Elapsed.TotalSeconds} - {percent}% {report}完了");
        }

        private async void InitialTask_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            var stw = new Stopwatch();
            stw.Start();
            InitialTask_ReportFunction(11, "APIの起動処理", new(() => { _=Background.APIs.GetInstance(); }), stw);
            InitialTask_ReportFunction(22, "設定データ読込", new(() => {
                var config = Lib.Config.Funcs.GetInstance();
                config.ReadConfig();
            }), stw);
            InitialTask_ReportFunction(33, "設定データ適用", new(() => {
                var config = Lib.Config.Funcs.GetInstance();
                config.ApplyConfig();
            }), stw);
            InitialTask_ReportFunction(44, "APIスレッド起動", new(() => {
                Background.APIs.GetInstance().Run();
            }), stw);
            InitialTask_ReportFunction(56, "Discord RPC接続", new(() => {
                var discord = Lib.Discord.RichPresence.GetInstance();
                discord.Init();
                discord.Update(detail: "MisakiEQは地震監視中です。");
            }), stw);
            InitialTask_ReportFunction(67, "強震モニタのポイントを取得", new(() => {
                Lib.KyoshinAPI.KyoshinObervation.Init();
            }), stw);

            InitialTask_ReportFunction(78, "サウンドの読込", new(() => {
                Funcs.SoundCollective.Init();
            }), stw);
#if ADMIN || DEBUG
            InitialTask_ReportFunction(89, "Twitter API連携", new(async () => {
                try
                {
                    if (!File.Exists("TwitterAuth.cfg"))
                    {
                        Log.Instance.Warn("Twitter連携が未設定です。");
                        return;
                    }
                    using var reader = new StreamReader("TwitterAuth.cfg");
                    var text = reader.ReadToEnd();
                    var args = text.Split('\n');
                    await Lib.Twitter.APIs.GetInstance().AuthFromToken(args[0], args[1]);
                }
                catch (Exception ex)
                {
                    Log.Instance.Error(ex.Message);
                }
            }), stw);
            InitialTask_ReportFunction(90, "Misskey API連携", new(() =>
            {
                if (!File.Exists("MisskeyAccessToken.cfg"))
                {
                    Log.Instance.Warn("Misskeyのアクセストークンが設定されていません。\nアクセストークンを「MisskeyAccessToken.cfg」に設定してください。");
                    return;
                }
                using var reader = new StreamReader("MisskeyAccessToken.cfg");
                var text = reader.ReadToEnd();
                Lib.Misskey.APIData.accessToken = text;

            }), stw);
#endif
            InitialTask_ReportFunction(95, "イベントを設定", new(() =>
            {
#pragma warning disable CS8602 // null 参照の可能性があるものの逆参照です。
                GUI.TrayHub.GetInstance(false).SetEvent();
#pragma warning restore CS8602 // null 参照の可能性があるものの逆参照です。
            }),stw);
            e.Result = "OK";
            await Task.Delay(2000);
        }

        private void InitialTask_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {
            //Log.Instance.Debug($"起動初期化進捗 : {e.ProgressPercentage}% {e.UserState}");
            label1.Text = e.UserState as string;
            progressBar1.Value = e.ProgressPercentage;
        }

        private async void InitialTask_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            Log.Instance.Info($"起動処理実行完了 {e.Result}");
            label1.Text = "起動処理実行完了";
            progressBar1.Value = 100;
            await Task.Delay(2000);
            Lib.Animator.Animate(500, (frame, frequency) =>
            {
                if (!Visible || IsDisposed) return false;
                Opacity = 1.00 - (double)frame / frequency;
                return true;
            });
            await Task.Delay(800);
            Close();
        }

        
    }
}