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
            InitialTask.ReportProgress(percent, report+"��...");
            Log.Instance.Debug($"{stopwatch.Elapsed.TotalSeconds} - {percent}% {report}��...");
            action();
            InitialTask.ReportProgress(percent, report+"����");
            Log.Instance.Debug($"{stopwatch.Elapsed.TotalSeconds} - {percent}% {report}����");
        }

        private async void InitialTask_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            var stw = new Stopwatch();
            stw.Start();
            InitialTask_ReportFunction(11, "API�̋N������", new(() => { _=Background.APIs.GetInstance(); }), stw);
            InitialTask_ReportFunction(22, "�ݒ�f�[�^�Ǎ�", new(() => {
                var config = Lib.Config.Funcs.GetInstance();
                config.ReadConfig();
            }), stw);
            InitialTask_ReportFunction(33, "�ݒ�f�[�^�K�p", new(() => {
                var config = Lib.Config.Funcs.GetInstance();
                config.ApplyConfig();
            }), stw);
            InitialTask_ReportFunction(44, "API�X���b�h�N��", new(() => {
                Background.APIs.GetInstance().Run();
            }), stw);
            InitialTask_ReportFunction(56, "Discord RPC�ڑ�", new(() => {
                var discord = Lib.Discord.RichPresence.GetInstance();
                discord.Init();
                discord.Update(detail: "MisakiEQ�͒n�k�Ď����ł��B");
            }), stw);
            InitialTask_ReportFunction(67, "���k���j�^�̃|�C���g���擾", new(() => {
                Lib.KyoshinAPI.KyoshinObervation.Init();
            }), stw);

            InitialTask_ReportFunction(78, "�T�E���h�̓Ǎ�", new(() => {
                Funcs.SoundCollective.Init();
            }), stw);
#if ADMIN || DEBUG
            InitialTask_ReportFunction(89, "Twitter API�A�g", new(async () => {
                try
                {
                    if (!File.Exists("TwitterAuth.cfg"))
                    {
                        Log.Instance.Warn("Twitter�A�g�����ݒ�ł��B");
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
            InitialTask_ReportFunction(90, "Misskey API�A�g", new(() =>
            {
                if (!File.Exists("MisskeyAccessToken.cfg"))
                {
                    Log.Instance.Warn("Misskey�̃A�N�Z�X�g�[�N�����ݒ肳��Ă��܂���B\n�A�N�Z�X�g�[�N�����uMisskeyAccessToken.cfg�v�ɐݒ肵�Ă��������B");
                    return;
                }
                using var reader = new StreamReader("MisskeyAccessToken.cfg");
                var text = reader.ReadToEnd();
                Lib.Misskey.APIData.accessToken = text;

            }), stw);
#endif
            InitialTask_ReportFunction(95, "�C�x���g��ݒ�", new(() =>
            {
#pragma warning disable CS8602 // null �Q�Ƃ̉\����������̂̋t�Q�Ƃł��B
                GUI.TrayHub.GetInstance(false).SetEvent();
#pragma warning restore CS8602 // null �Q�Ƃ̉\����������̂̋t�Q�Ƃł��B
            }),stw);
            e.Result = "OK";
            await Task.Delay(2000);
        }

        private void InitialTask_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {
            //Log.Instance.Debug($"�N���������i�� : {e.ProgressPercentage}% {e.UserState}");
            label1.Text = e.UserState as string;
            progressBar1.Value = e.ProgressPercentage;
        }

        private async void InitialTask_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            Log.Instance.Info($"�N���������s���� {e.Result}");
            label1.Text = "�N���������s����";
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