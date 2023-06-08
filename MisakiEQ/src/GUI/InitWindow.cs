using System.Diagnostics;
using System.Security;
using System.Windows.Markup;
using MisakiEQ;
using MisakiEQ.src.GUI;

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
            Log.Debug($"{stopwatch.Elapsed.TotalSeconds} - {percent}% {report}��...");
            action();
            InitialTask.ReportProgress(percent, report+"����");
            Log.Debug($"{stopwatch.Elapsed.TotalSeconds} - {percent}% {report}����");
        }

        private async void InitialTask_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
#if DEBUG
            this.Invoke(()=> {
                Hide();

                var Map = new Map();
                Map.Show();
            });

            await Task.Delay(2147483647);
            return;
#endif
            var stw = new Stopwatch();
            stw.Start();
            
            InitialTask_ReportFunction(11, "API�̋N������", new(() => { _=Background.APIs.Instance; }), stw);
            InitialTask_ReportFunction(22, "�ݒ�f�[�^�Ǎ�", new(() => {
                var config = Lib.Config.Funcs.GetInstance();
                config.ReadConfig();
            }), stw);
            InitialTask_ReportFunction(33, "�ݒ�f�[�^�K�p", new(() => {
                var config = Lib.Config.Funcs.GetInstance();
                config.ApplyConfig();
            }), stw);
            InitialTask_ReportFunction(44, "API�X���b�h�N��", new(() => {
                Background.APIs.Instance.Run();
            }), stw);
            InitialTask_ReportFunction(56, "Discord RPC�ڑ�", new(() => {
                var discord = Lib.Discord.RichPresence.GetInstance();
                discord.Init();
                discord.Update(detail: "MisakiEQ�͒n�k�Ď����ł��B");
            }), stw);
            InitialTask_ReportFunction(67, "���k���j�^�̃|�C���g���擾", new(() => {
                Lib.KyoshinAPI.KyoshinObervation.Init();
            }), stw);
            InitialTask_ReportFunction(70, "�n�k���f�[�^�擾", new(() => { _ = Struct.jma.Area.Static.EarthquakePos.Instance; }), stw);

            InitialTask_ReportFunction(78, "�T�E���h�̓Ǎ�", new(() => {
                Funcs.SoundCollective.Init();
            }), stw);
#if ADMIN || DEBUG
            InitialTask_ReportFunction(89, "Twitter API�A�g", new(async () => {
                try
                {
                    if (!File.Exists("TwitterAuth.cfg"))
                    {
                        Log.Warn("Twitter�A�g�����ݒ�ł��B");
                        return;
                    }
                    using var reader = new StreamReader("TwitterAuth.cfg");
                    var text = reader.ReadToEnd();
                    var args = text.Split('\n');
                    await Lib.Twitter.APIs.GetInstance().AuthFromToken(args[0], args[1]);
                }
                catch (Exception ex)
                {
                    Log.Error(ex.Message);
                }
            }), stw);
            InitialTask_ReportFunction(90, "Misskey API�A�g", new(() =>
            {
                if (!File.Exists("MisskeyAccessToken.cfg"))
                {
                    Log.Warn("Misskey�̃A�N�Z�X�g�[�N�����ݒ肳��Ă��܂���B\n�A�N�Z�X�g�[�N�����uMisskeyAccessToken.cfg�v�ɐݒ肵�Ă��������B");
                    return;
                }
                using var reader = new StreamReader("MisskeyAccessToken.cfg");
                var text = reader.ReadToEnd();
                Lib.Misskey.APIData.accessToken = text;

            }), stw);

            InitialTask_ReportFunction(91, "Discord WebHook�A�g", new(() =>
            {
                if (!File.Exists("DiscordWebHookToken.cfg"))
                {
                    Log.Warn("Discord WebHook�̃A�N�Z�X�g�[�N�����ݒ肳��Ă��܂���B\n�A�N�Z�X�g�[�N�����uDiscordWebHookToken.cfg�v�ɐݒ肵�Ă��������B");
                    return;
                }
                using var reader = new StreamReader("DiscordWebHookToken.cfg");
                var text = reader.ReadToEnd();
                if (!Lib.Discord.WebHooks.Main.SetToken(text))
                {
                    Log.Warn("Discord WebHook���A�g�ł��܂���ł����B");
                }

            }), stw);
#endif
            InitialTask_ReportFunction(95, "�C�x���g��ݒ�", new(() =>
            {
                GUI.TrayHub.GetInstance(false)?.SetEvent();
            }),stw);
            InitialTask_ReportFunction(99, "�C�x���g���O�֘A�m�F", new(() =>
            {
                string sourceName = "MisakiEQ";
                if (Lib.WinAPI.IsAdministrator())
                {
                    if (!EventLog.SourceExists(sourceName))
                    {
                        EventLog.CreateEventSource(sourceName, sourceName);
                    }
                }
                try
                {
                    EventLog.WriteEntry(
                        sourceName, $"{DateTime.Now} �N�����܂����B",
                        EventLogEntryType.Information, 0, 32767);
                }
                catch (SecurityException)
                {
                    Log.Warn("�C�x���g���O�o�͋@�\�͗��p�ł��܂���B���p����ɂ͈�x�Ǘ��Ҍ����ōċN�����Ă��������B");
                }
                catch (Exception ex)
                {
                    Log.Error($"Error: {ex.Message}");

                }
            }), stw);
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
            Log.Info($"�N���������s���� {e.Result}");
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