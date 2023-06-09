using System.Diagnostics;
using System.Security;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Markup;
using MisakiEQ;
using MisakiEQ.src.GUI;

namespace MisakiEQ
{
    public partial class InitWindow : Form
    {
        class Tasks
        {
            public Tasks(string function,Action act)
            {
                name = function;
                action = act;
            }
            public string name;
            public Action action;
        }
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
            List<Tasks> tasks = new();
            tasks.Add(new("API�̋N������", new(() => { _ = Background.APIs.Instance; })));
            tasks.Add(new("�ݒ�f�[�^�Ǎ�", new(() => {
                var config = Lib.Config.Funcs.GetInstance();
                config.ReadConfig();
            })));
            var stw = new Stopwatch();
            stw.Start();

            tasks.Add(new("API�̋N������", new(() => { _ = Background.APIs.Instance; })));
            tasks.Add(new("�ݒ�f�[�^�Ǎ�", new(() => {
                var config = Lib.Config.Funcs.GetInstance();
                config.ReadConfig();
            })));
            tasks.Add(new("�ݒ�f�[�^�K�p", new(() => {
                var config = Lib.Config.Funcs.GetInstance();
                config.ApplyConfig();
            })));
            tasks.Add(new("API�X���b�h�N��", new(() => {
                Background.APIs.Instance.Run();
            })));
            tasks.Add(new("Discord RPC�ڑ�", new(() => {
                var discord = Lib.Discord.RichPresence.GetInstance();
                discord.Init();
                discord.Update(detail: "MisakiEQ�͒n�k�Ď����ł��B");
            })));
            tasks.Add(new("���k���j�^�̃|�C���g���擾", new(() => {
                Lib.KyoshinAPI.KyoshinObervation.Init();
            })));
            tasks.Add(new("�n�k���f�[�^�擾", new(() => { _ = Struct.jma.Area.Static.EarthquakePos.Instance; })));

            tasks.Add(new("�T�E���h�̓Ǎ�", new(() => {
                Funcs.SoundCollective.Init();
            })));
#if ADMIN || DEBUG
            tasks.Add(new("Twitter API�A�g", new(async () => {
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
            })));
            tasks.Add(new("Misskey API�A�g", new(() =>
            {
                if (!File.Exists("MisskeyAccessToken.cfg"))
                {
                    Log.Warn("Misskey�̃A�N�Z�X�g�[�N�����ݒ肳��Ă��܂���B\n�A�N�Z�X�g�[�N�����uMisskeyAccessToken.cfg�v�ɐݒ肵�Ă��������B");
                    return;
                }
                using var reader = new StreamReader("MisskeyAccessToken.cfg");
                var text = reader.ReadToEnd();
                Lib.Misskey.APIData.accessToken = text;

            })));

            tasks.Add(new("Discord WebHook�A�g", new(() =>
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

            })));
#endif
            tasks.Add(new("�C�x���g��ݒ�", new(() =>
            {
                GUI.TrayHub.GetInstance(false)?.SetEvent();
            })));
            tasks.Add(new("�C�x���g���O�֘A�m�F", new(() =>
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
            })));
            for(int i=0; i < tasks.Count; i++)
            {
                var task = tasks[i];
                InitialTask_ReportFunction((int)((double)i / tasks.Count * 100.0), task.name, task.action, stw);
            }
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