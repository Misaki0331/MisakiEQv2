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

        private void InitialTask_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            InitialTask.ReportProgress(0,"APIの起動初期化を実行中");
            var api = Background.APIs.GetInstance();
            InitialTask.ReportProgress(25, "コンフィグを読み込んでいます");
            var config = Lib.Config.Funcs.GetInstance();
            config.ReadConfig();
            InitialTask.ReportProgress(50, "コンフィグを適用しています");
            config.ApplyConfig();
            InitialTask.ReportProgress(75, "APIスレッド起動中");
            api.Run();
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