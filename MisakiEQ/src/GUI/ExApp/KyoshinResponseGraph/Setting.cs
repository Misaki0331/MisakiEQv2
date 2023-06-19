using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MisakiEQ;

namespace MisakiEQ.GUI.ExApp.KyoshinGraphWindow
{
    public partial class Setting : Form
    {
        public Setting(int ConfigNum=-1)
        {
            InitializeComponent();
            comboBox1.SelectedIndex = 0;
            CheckBoxTextChanging("", EventArgs.Empty);
            ReadConfig(ConfigNum);
            CurrentNum = ConfigNum;
            if (ConfigNum >= 0 && ConfigNum <= 9999) numericUpDown1.Value = ConfigNum;
        }
        public string WindowName { get; private set; } = "";
        public int DisplayMode { get; private set; }
        public bool Check1Mode { get; private set; }
        public bool Check2Mode { get; private set; }
        public bool Check3Mode { get; private set; }
        public bool Check4Mode { get; private set; }
        public bool MaxValueMode { get; private set; }
        public bool LinearMode { get; private set; }

        public int WindowX { get; set; }
        public int WindowY { get; set; }
        public int WindowW { get; set; } = 350;
        public int WindowH { get; set; } = 140;
        public bool NeedMove { get; set; }
        public int CurrentNum { get; set; }

        private void WriteCommand_Click(object sender, EventArgs e)
        {
            var path = "Config/Graph";
            try
            {
                if (!Directory.Exists(path)) Directory.CreateDirectory(path);
                var writer=new StreamWriter($"{path}/{(int)numericUpDown1.Value}.cfg");
                writer.WriteLine($"Title={textBox1.Text.Replace("%","%%").Replace("=","%3D")}");
                writer.WriteLine($"DisplayMode={comboBox1.SelectedIndex}");
                writer.WriteLine($"Linear={(checkBox6.Checked ? 1 : 0)}");
                writer.WriteLine($"MaxValue={(checkBox5.Checked ? 1 : 0)}");
                writer.WriteLine($"Check1={(checkBox1.Checked?1:0)}");
                writer.WriteLine($"Check2={(checkBox2.Checked?1:0)}");
                writer.WriteLine($"Check3={(checkBox3.Checked?1:0)}");
                writer.WriteLine($"Check4={(checkBox4.Checked?1:0)}");
                writer.WriteLine($"WindowX={WindowX}");
                writer.WriteLine($"WindowY={WindowY}");
                writer.WriteLine($"WindowW={WindowW}");
                writer.WriteLine($"WindowH={WindowH}");
                writer.Close();
                CurrentNum = (int)numericUpDown1.Value;
                Log.Debug($"No.{(int)numericUpDown1.Value}のメモリを書き込みました。");
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
        }
        private void CheckBoxTextChanging(object sender, EventArgs e)
        {
            checkBox1.Enabled = true;
            checkBox2.Enabled = true;
            checkBox6.Enabled = true;
            var name = "";
            switch (comboBox1.SelectedIndex)
            {
                case 0:
                    name = "リアルタイム震度";
                    checkBox3.Text = "予測震度(EEW)を表示";
                    /*checkBox3.Enabled = true;
                    checkBox3.Visible = true;
                    checkBox4.Visible = true;
                    checkBox4.Text = "リアルタイム震度より予測震度を優先的に表示";
                    if (checkBox3.Checked) checkBox4.Enabled = true;
                    else
                    {
                        checkBox4.Enabled = false;
                        checkBox4.Checked = false;
                    }*/
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
                    checkBox4.Visible = true;
                    checkBox4.Text = "下側の表示順のみを反転する";
                    if (checkBox1.Checked) checkBox4.Enabled = true;
                    else
                    {
                        checkBox4.Enabled = false;
                        checkBox4.Checked = false;
                    }
                    break;
            }
            switch (comboBox1.SelectedIndex)
            {
                case 0:
                    checkBox6.Enabled = false;
                    checkBox6.Checked = false;
                    checkBox3.Visible = false;
                    checkBox3.Enabled = false;
                    checkBox3.Checked = false;
                    checkBox4.Visible = false;
                    checkBox4.Enabled = false;
                    checkBox4.Checked = false;
                    break;
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
            LinearMode = checkBox6.Checked;
        }

        private void ReadCommand_Click(object sender, EventArgs e)
        {
            ReadConfig((int)numericUpDown1.Value);
        }
        void ReadConfig(int ConfigNum)
        {
            var path = "Config/Graph";
            try
            {
                if (ConfigNum >= 0 && ConfigNum <= 9999 && File.Exists($"{path}/{ConfigNum}.cfg"))
                {
                    textBox1.Text = "";
                    comboBox1.SelectedIndex = 0;
                    checkBox1.Checked = false;
                    checkBox2.Checked = false;
                    checkBox3.Checked = false;
                    checkBox4.Checked = false;
                    checkBox5.Checked = false;
                    checkBox6.Checked = false;
                    WindowX = 100;
                    WindowY = 100;
                    WindowW = 350;
                    WindowH = 140;
                    var reader = new StreamReader($"{path}/{ConfigNum}.cfg");
                    while (!reader.EndOfStream)
                    {
                        var read = reader.ReadLine();
                        if (read == null) continue;
                        var cmd = read.Split('=');
                        if (cmd.Length != 2) continue;
                        switch (cmd[0])
                        {
                            case "Title":
                                textBox1.Text = cmd[1].Replace("%3D","=").Replace("%%","%");
                                break;
                            case "DisplayMode":
                                if (int.TryParse(cmd[1], out int c))
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
                            case "MaxValue":
                                if (cmd[1] == "1") checkBox5.Checked = true;
                                break;
                            case "Linear":
                                if (cmd[1] == "1") checkBox6.Checked = true;
                                break;
                            case "WindowX":
                                if(int.TryParse(cmd[1], out int data))WindowX=data;
                                NeedMove = true;
                                break;
                            case "WindowY":
                                if (int.TryParse(cmd[1], out data)) WindowY = data;
                                NeedMove = true;
                                break;
                            case "WindowW":
                                if (int.TryParse(cmd[1], out data)) WindowW = data;
                                NeedMove = true;
                                break;
                            case "WindowH":
                                if (int.TryParse(cmd[1], out data)) WindowH = data;
                                NeedMove = true;
                                break;
                        }

                    }
                    CurrentNum = ConfigNum;
                    reader.Close();
                    Log.Debug($"No.{ConfigNum}のメモリを読み込みました。");
                }
                else
                {
                    textBox1.Text = "<NAME>(<POSITION>) <VALUE1><UNIT> / <VALUE2><UNIT>";
                    comboBox1.SelectedIndex = 0;
                    checkBox1.Checked = true;
                    checkBox2.Checked = false;
                    checkBox3.Checked = false;
                    checkBox4.Checked = false;
                    checkBox5.Checked = false;
                    checkBox6.Checked = false;
                    WindowX = 100;
                    WindowY = 100;
                    WindowW = 350;
                    WindowH = 140;
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex);
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

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            WindowName = textBox1.Text;
        }
    }
}
