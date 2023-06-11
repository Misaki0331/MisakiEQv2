using MisakiEQ.Background;
using MisakiEQ;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MisakiEQ.GUI
{
    public partial class LogViewerWindow : Form
    {
        public LogViewerWindow()
        {
            InitializeComponent();
        }

        private readonly object lockObj = new();
        private void LogViewerWindow_FormClosed(object sender, FormClosedEventArgs e)
        {
            Log.Instance.LogUpdateHandler -= LogUpdate;
        }
        readonly List<string>SendCommands= new();
        private void LogUpdate(object? sender, string e)
        {
            textBox1.Invoke(() =>
            {
                lock (lockObj)
                {
                    var lines = e.Split('\n');
                    //textBox1.Clear();
                    for (int i = 0; i < lines.Length; i++)
                    {
                        switch (lines[i][..2])
                        {
                            case "$D":
                                textBox1.SelectionBackColor = Color.Black;
                                textBox1.SelectionColor = Color.Gray;
                                break;
                            case "$I":
                                textBox1.SelectionBackColor = Color.Black;
                                textBox1.SelectionColor = Color.Cyan;
                                break;
                            case "$W":
                                textBox1.SelectionBackColor = Color.Black;
                                textBox1.SelectionColor = Color.Yellow;
                                break;
                            case "$E":
                                textBox1.SelectionBackColor = Color.Black;
                                textBox1.SelectionColor = Color.Red;
                                break;
                            case "$F":
                                textBox1.SelectionBackColor = Color.Pink;
                                textBox1.SelectionColor = Color.Red;
                                break;

                        }
                        textBox1.AppendText("\n" + lines[i][2..]);

                    }
                    /*
                    int cnt = 0;
                    while (textBox1.Lines.Length > 100)
                    {
                        cnt++;
                        if (cnt > 30) break;
                        textBox1.SelectionStart = textBox1.GetFirstCharIndexFromLine(0);
                        textBox1.SelectionLength = textBox1.Lines[0].Length + 1;
                        textBox1.ReadOnly = false;
                        textBox1.SelectedText = string.Empty;
                        textBox1.ReadOnly = true;
                    }
                    */
                    textBox1.SelectionStart = textBox1.Text.Length;
                    textBox1.ScrollToCaret();
                }
            });
        }

        private void LogViewerWindow_Load(object sender, EventArgs e)
        {
            textBox1.Clear();
            LogUpdate(sender, Log.Instance.GetLogData());
            Log.Instance.LogUpdateHandler += LogUpdate;
            textBox1.SelectionStart = textBox1.Text.Length;
            textBox1.ScrollToCaret();
        }

        private void LogViewerWindow_Activated(object sender, EventArgs e)
        {
            if (!IsDisposed && !Disposing) Opacity = 1.0;
        }

        private void LogViewerWindow_Deactivate(object sender, EventArgs e)
        {
            if (!IsDisposed && !Disposing) Opacity = 0.8;
        }
        private void Execution_Click(object sender, EventArgs e)
        {
            var commands = Command.Text.Split(' ');
            if (commands.Length < 1)
            {
                Log.Error("コマンドの内容がありません。");
            }
            try
            {
                switch (commands[0].ToLower())
                {
                    case "/sendeew":
                        if (commands.Length < 2) throw new ArgumentException("/sendeew 任意のファイル名");
                        try
                        {
                            var task = new Task(() => {
                                Log.Debug($"{commands[1]}のデータ読込中...");
                                var reader = new StreamReader(commands[1]);
                                var str = reader.ReadToEnd();
                                APIs.Instance.EEW.DMData.Test(str);
                                reader.Close();
                                Log.Debug($"Done");
                            });
                            task.Start();
                        }
                        catch (Exception ex)
                        {
                            Log.Error(ex.ToString());
                        }
                        break;
                    case "/misskey-token":
                        Log.Info(Lib.Misskey.APIData.accessToken);
                        break;
                    case "/config_view":
                        Lib.Config.Funcs.GetInstance().Configs.OutputLog();
                        break;
                    case "/crash":
                        throw new NullReferenceException(); 
                    default:
                        Log.Warn("不明なコマンドです。" + commands[0]);
                        break;
                }
            }
            catch (NullReferenceException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
            }
            SendCommands.Add(Command.Text);
            Command.Text = "";
            if(SendCommands.Count > 100) SendCommands.RemoveAt(0);
            textBox1.Focus();
            panel1.Visible = false;
        }
        readonly int pos = 0;
        private void Command_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar == (char)Keys.Up)
            {
                if (SendCommands.Count>0)Command.Text= SendCommands[^1];
            }
            if (e.KeyChar == (char)Keys.Enter || e.KeyChar == (char)Keys.Escape)
            {
                e.Handled = true;
                if (e.KeyChar == (char)Keys.Enter)
                {
                    Execution_Click(sender, e);
                }
                panel1.Visible = false;
                textBox1.Focus();
            }

        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)'/')
            {
                
                panel1.Visible = true;
                Command.Focus();
                Command.AppendText("/");
            }
        }

        private void Command_DragDrop(object sender, DragEventArgs e)
        {
            if (!panel1.Visible) return;
            if (e.Data == null) return;
            if (!e.Data.GetDataPresent(DataFormats.FileDrop)) return;

            string[] dragFilePathArr = (string[])e.Data.GetData(DataFormats.FileDrop, false);
            Command.Text += dragFilePathArr[0];
        }

        private void Command_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.All;
        }

        private void LogViewerWindow_DragDrop(object sender, DragEventArgs e)
        {

            if (!panel1.Visible) return;
            if (e.Data == null) return;
            if (!e.Data.GetDataPresent(DataFormats.FileDrop)) return;

            string[] dragFilePathArr = (string[])e.Data.GetData(DataFormats.FileDrop, false);
            Command.Text += dragFilePathArr[0];
        }

        private void LogViewerWindow_DragEnter(object sender, DragEventArgs e)
        {
            if (panel1.Visible) e.Effect = DragDropEffects.All;
        }
    }
}
