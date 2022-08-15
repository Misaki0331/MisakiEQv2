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

        private void LogViewerWindow_FormClosed(object sender, FormClosedEventArgs e)
        {
            Log.Instance.LogUpdateHandler -= LogUpdate;
        }
        private void LogUpdate(object? sender, EventArgs e)
        {
            textBox1.Invoke(() =>
            {
                textBox1.Lines = Log.Instance.GetLogData().Split('\n');
                textBox1.SelectionStart = textBox1.Text.Length;
                textBox1.ScrollToCaret();
            });
        }

        private void LogViewerWindow_Load(object sender, EventArgs e)
        {
            LogUpdate(sender, e);
            Log.Instance.LogUpdateHandler += LogUpdate;
        }
    }
}
