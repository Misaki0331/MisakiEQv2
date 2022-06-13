namespace MisakiEQ
{
    public partial class InitWindow : Form
    {
        readonly Log.Logger log = Log.Logger.GetInstance();
        public InitWindow()
        {
            InitializeComponent();
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

        private async void InitialTask_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            InitialTask.ReportProgress(0,"APIの起動初期化を実行中");
            var api = Background.APIs.GetInstance();
            InitialTask.ReportProgress(17, "コンフィグを読み込んでいます");
            var config = Lib.Config.Funcs.GetInstance();
            config.ReadConfig();
            InitialTask.ReportProgress(33, "コンフィグを適用しています");
            config.ApplyConfig();
            InitialTask.ReportProgress(50, "APIスレッド起動中");
            api.Run();
            InitialTask.ReportProgress(67, "Discord RPC接続中");
            var discord = Lib.Discord.RichPresence.GetInstance();
            discord.Init();
            discord.Update(detail:"MisakiEQのテスト実行です。");
            InitialTask.ReportProgress(83, "Twitter API認証中");
            try
            {
                using var reader = new StreamReader("TwitterAuth.cfg");
                var text = reader.ReadToEnd();
                var args = text.Split('\n');
                await Lib.Twitter.APIs.GetInstance().AuthFromToken(args[0], args[1]);
            }
            catch(Exception ex)
            {
                log.Error(ex.Message);
            }
            e.Result = "OK";

        }

        private void InitialTask_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {
            log.Debug($"起動初期化進捗 : {e.ProgressPercentage}% {e.UserState}");
            label1.Text = e.UserState as string;
            progressBar1.Value = e.ProgressPercentage;
        }

        private async void InitialTask_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            log.Info($"起動初期化完了 {e.Result}");
            label1.Text = "起動初期化完了";
            progressBar1.Value = 100;
            await Task.Delay(3000);
            Close();
        }

        
    }
}