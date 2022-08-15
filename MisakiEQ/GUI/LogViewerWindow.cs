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
                        textBox1.AppendText("\n"+ lines[i][2..]);

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
            if(!IsDisposed&&!Disposing)Opacity = 0.8;
        }
    }
}
