namespace MisakiEQ
{
    public partial class InitWindow : Form
    {
        readonly Log.Logger log = Log.Logger.GetInstance();
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

#if ADMIN || DEBUG
        private async void InitialTask_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
#else
        private void InitialTask_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
#endif
        {
            InitialTask.ReportProgress(0,"API�̋N�������������s��");
            var api = Background.APIs.GetInstance();
            InitialTask.ReportProgress(2, "�R���t�B�O��ǂݍ���ł��܂�");
            var config = Lib.Config.Funcs.GetInstance();
            config.ReadConfig();
            InitialTask.ReportProgress(25, "�R���t�B�O��K�p���Ă��܂�");
            config.ApplyConfig();
            InitialTask.ReportProgress(40, "API�X���b�h�N����");
            api.Run();
            InitialTask.ReportProgress(55, "Discord RPC�ڑ���");
            var discord = Lib.Discord.RichPresence.GetInstance();
            discord.Init();
            discord.Update(detail: "MisakiEQ�͒n�k�Ď����ł��B");
            InitialTask.ReportProgress(55, "���k���j�^�̃|�C���g���擾��");
            Lib.KyoshinAPI.KyoshinObervation.Init();
            InitialTask.ReportProgress(60, "�T�E���h�Ǎ���");
            Funcs.SoundCollective.Init();
#if ADMIN || DEBUG
            InitialTask.ReportProgress(70, "Twitter API�F�ؒ�");
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
#endif
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