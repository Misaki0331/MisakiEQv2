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
            InitialTask.ReportProgress(percent, report+"中...");
            Log.Debug($"{stopwatch.Elapsed.TotalSeconds} - {percent}% {report}中...");
            action();
            InitialTask.ReportProgress(percent, report+"完了");
            Log.Debug($"{stopwatch.Elapsed.TotalSeconds} - {percent}% {report}完了");
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
            tasks.Add(new("APIの起動処理", new(() => { _ = Background.APIs.Instance; })));
            tasks.Add(new("設定データ読込", new(() => {
                var config = Lib.Config.Funcs.GetInstance();
                config.ReadConfig();
            })));
            var stw = new Stopwatch();
            stw.Start();

            tasks.Add(new("APIの起動処理", new(() => { _ = Background.APIs.Instance; })));
            tasks.Add(new("設定データ読込", new(() => {
                var config = Lib.Config.Funcs.GetInstance();
                config.ReadConfig();
            })));
            tasks.Add(new("設定データ適用", new(() => {
                var config = Lib.Config.Funcs.GetInstance();
                config.ApplyConfig();
            })));
            tasks.Add(new("APIスレッド起動", new(() => {
                Background.APIs.Instance.Run();
            })));
            tasks.Add(new("Discord RPC接続", new(() => {
                var discord = Lib.Discord.RichPresence.GetInstance();
                discord.Init();
                discord.Update(detail: "MisakiEQは地震監視中です。");
            })));
            tasks.Add(new("強震モニタのポイントを取得", new(() => {
                Lib.KyoshinAPI.KyoshinObervation.Init();
            })));
            tasks.Add(new("地震情報データ取得", new(() => { _ = Struct.jma.Area.Static.EarthquakePos.Instance; })));

            tasks.Add(new("サウンドの読込", new(() => {
                Funcs.SoundCollective.Init();
            })));
#if ADMIN || DEBUG
            tasks.Add(new("Twitter API連携", new(async () => {
                try
                {
                    if (!File.Exists("TwitterAuth.cfg"))
                    {
                        Log.Warn("Twitter連携が未設定です。");
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
            tasks.Add(new("Misskey API連携", new(() =>
            {
                if (!File.Exists("MisskeyAccessToken.cfg"))
                {
                    Log.Warn("Misskeyのアクセストークンが設定されていません。\nアクセストークンを「MisskeyAccessToken.cfg」に設定してください。");
                    return;
                }
                using var reader = new StreamReader("MisskeyAccessToken.cfg");
                var text = reader.ReadToEnd();
                Lib.Misskey.APIData.accessToken = text;

            })));

            tasks.Add(new("Discord WebHook連携", new(() =>
            {
                if (!File.Exists("DiscordWebHookToken.cfg"))
                {
                    Log.Warn("Discord WebHookのアクセストークンが設定されていません。\nアクセストークンを「DiscordWebHookToken.cfg」に設定してください。");
                    return;
                }
                using var reader = new StreamReader("DiscordWebHookToken.cfg");
                var text = reader.ReadToEnd();
                if (!Lib.Discord.WebHooks.Main.SetToken(text))
                {
                    Log.Warn("Discord WebHookが連携できませんでした。");
                }

            })));
#endif
            tasks.Add(new("イベントを設定", new(() =>
            {
                GUI.TrayHub.GetInstance(false)?.SetEvent();
            })));
            tasks.Add(new("イベントログ関連確認", new(() =>
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
                        sourceName, $"{DateTime.Now} 起動しました。",
                        EventLogEntryType.Information, 0, 32767);
                }
                catch (SecurityException)
                {
                    Log.Warn("イベントログ出力機能は利用できません。利用するには一度管理者権限で再起動してください。");
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
            //Log.Instance.Debug($"起動初期化進捗 : {e.ProgressPercentage}% {e.UserState}");
            label1.Text = e.UserState as string;
            progressBar1.Value = e.ProgressPercentage;
        }

        private async void InitialTask_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            Log.Info($"起動処理実行完了 {e.Result}");
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