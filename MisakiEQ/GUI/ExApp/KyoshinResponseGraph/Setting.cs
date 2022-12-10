using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MisakiEQ.GUI.ExApp.KyoshinGraphWindow
{
    public partial class Setting : Form
    {
        public Setting()
        {
            InitializeComponent();
            comboBox1.SelectedIndex = 0;
            CheckBoxTextChanging("", EventArgs.Empty);
        }
        public string WindowName { get; private set; } = "";
        public int DisplayMode { get; private set; }
        public bool Check1Mode { get; private set; }
        public bool Check2Mode { get; private set; }
        public bool Check3Mode { get; private set; }
        public bool Check4Mode { get; private set; }
        public bool MaxValueMode { get; private set; }

        private void WriteCommand_Click(object sender, EventArgs e)
        {
            var path = "Config/Graph";
            try
            {
                if (!Directory.Exists(path)) Directory.CreateDirectory(path);
                var writer=new StreamWriter($"{path}/{(int)numericUpDown1.Value}.cfg");
                writer.WriteLine($"Title={textBox1.Text.Replace("%","%%").Replace("=","%3D")}");
                writer.WriteLine($"DisplayMode={comboBox1.SelectedIndex}");
                writer.WriteLine($"Check1={(checkBox1.Checked?1:0)}");
                writer.WriteLine($"Check2={(checkBox2.Checked?1:0)}");
                writer.WriteLine($"Check3={(checkBox3.Checked?1:0)}");
                writer.WriteLine($"Check4={(checkBox4.Checked?1:0)}");
            }
            catch (Exception ex)
            {
                Log.Instance.Error(ex);
            }
        }
        private void CheckBoxTextChanging(object sender, EventArgs e)
        {
            checkBox1.Enabled = true;
            checkBox2.Enabled = true;
            var name = "";
            switch (comboBox1.SelectedIndex)
            {
                case 0:
                    name = "リアルタイム震度";
                    checkBox3.Text = "予測震度(EEW)を表示";
                    checkBox3.Enabled = true;
                    checkBox3.Visible = true;
                    checkBox4.Visible = true;
                    checkBox4.Text = "リアルタイム震度より予測震度を優先的に表示";
                    if (checkBox3.Checked) checkBox4.Enabled = true;
                    else
                    {
                        checkBox4.Enabled = false;
                        checkBox4.Checked = false;
                    }
                    break;
                case 1:
                    name = "最大加速度";
                    break;
                case 2:
                    name = "最大速度";
                    break;
                case 3:
                    name = "最大変位";
                    break;
                case 4:
                    name = "応答速度";
                    checkBox3.Visible = true;
                    checkBox3.Enabled = true;
                    checkBox3.Text = "表示順を反転する";
                    break;
            }
            switch (comboBox1.SelectedIndex)
            {
                case 1:
                case 2:
                case 3:
                    checkBox3.Visible = false;
                    checkBox3.Enabled = false;
                    checkBox3.Checked = false;
                    checkBox4.Visible = false;
                    checkBox4.Enabled = false;
                    checkBox4.Checked = false;
                    break;
                case 4:
                    checkBox4.Visible = false;
                    checkBox4.Enabled = false;
                    checkBox4.Checked = false;
                    break;
            }
            checkBox1.Text = $"{name}の地表と地中を両方表示";
            if (checkBox1.Checked) checkBox2.Text = "地表より地中を優先的に表示";
            else checkBox2.Text = "地表の代わりに地中を表示";
            WindowName=textBox1.Text;
            DisplayMode=comboBox1.SelectedIndex;
            Check1Mode = checkBox1.Checked;
            Check2Mode = checkBox2.Checked;
            Check3Mode = checkBox3.Checked;
            Check4Mode = checkBox4.Checked;
            MaxValueMode = checkBox5.Checked;
        }

        private void ReadCommand_Click(object sender, EventArgs e)
        {
            var path = "Config/Graph";
            try
            {
                textBox1.Text = "";
                comboBox1.SelectedIndex = 0;
                checkBox1.Checked = false;
                checkBox2.Checked = false;
                checkBox3.Checked = false;
                checkBox4.Checked = false;
                if (File.Exists($"{path}/{(int)numericUpDown1.Value}.cfg"))
                {
                    var reader = new StreamReader($"{path}/{(int)numericUpDown1.Value}.cfg");
                    while (!reader.EndOfStream)
                    {
                        var read = reader.ReadLine();
                        if (read == null) continue;
                        var cmd = read.Split('=');
                        if (cmd.Length != 2) continue;
                        switch (cmd[0])
                        {
                            case "Title":
                                textBox1.Text = cmd[1];
                                break;
                            case "DisplayMode":
                                if(int.TryParse(cmd[1], out int c))
                                    comboBox1.SelectedIndex = c;
                                break;
                            case "Check1":
                                if (cmd[1] == "1") checkBox1.Checked = true;
                                break;
                            case "Check2":
                                if (cmd[1] == "1") checkBox2.Checked = true;
                                break;
                            case "Check3":
                                if (cmd[1] == "1") checkBox3.Checked = true;
                                break;
                            case "Check4":
                                if (cmd[1] == "1") checkBox4.Checked = true;
                                break;
                        }

                    }
                }
            }
            catch(Exception ex)
            {
                Log.Instance.Error(ex);
            }
        }
        bool IsWillClose = false;
        public void DisposeSetting()
        {
            IsWillClose = true;
            Close();
        }
        private void Setting_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (IsWillClose)
            {
                e.Cancel = true;
                Hide();
            }
        }
    }
}
