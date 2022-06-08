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
            InitialTask.ReportProgress(0,"API�̋N�������������s��");
            var api = Background.APIs.GetInstance();
            InitialTask.ReportProgress(25, "�R���t�B�O��ǂݍ���ł��܂�");
            var config = Lib.Config.Funcs.GetInstance();
            config.ReadConfig();
            InitialTask.ReportProgress(50, "�R���t�B�O��K�p���Ă��܂�");
            config.ApplyConfig();
            InitialTask.ReportProgress(75, "API�X���b�h�N����");
            api.Run();
            e.Result = "OK";

        }

        private void InitialTask_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {
            log.Debug($"�N���������i�� : {e.ProgressPercentage}% {e.UserState}");
            label1.Text = e.UserState as string;
            progressBar1.Value = e.ProgressPercentage;
        }

        private async void InitialTask_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            log.Info($"�N������������ {e.Result}");
            label1.Text = "�N������������";
            progressBar1.Value = 100;
            await Task.Delay(3000);
            Close();
        }

        
    }
}