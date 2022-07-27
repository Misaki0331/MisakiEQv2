using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MisakiEQ.Lib.ConfigController
{
    internal class Controller
    {
        readonly TabPage Tab;
        Size NowSize;
        readonly List<GroupBox> Groups = new();
        readonly List<ControllGroup> controllGroups = new();
        public Controller(TabPage tab)
        {
            Tab = tab;
            NowSize = tab.Size;
            var list = Config.Funcs.GetInstance().Configs.Data;
            tab.SizeChanged += SizeChanged;
            int cnt = 1;
            List<int> sizelist = new();
            while (tab.Width / cnt > 450) cnt++;
            if (cnt > 1) while (tab.Width / cnt < 300) cnt--;
            for (int i = 0; i < cnt; i++) sizelist.Add(0);
            for(int i = 0; i < list.Count; i++)
            {
                Groups.Add(new GroupBox());
                tab.Controls.Add(Groups[^1]);
                Groups[^1].Size = new Size((tab.Width-15) / cnt-10, 32);
                controllGroups.Add(new(Groups[^1], list[i].Setting, list[i].Name)) ;
                int pos = 0;
                int min = int.MaxValue;
                for(int j=0;j<sizelist.Count; j++)
                {
                    if (min > sizelist[j])
                    {
                        min = sizelist[j];
                        pos = j;
                    }
                }
                controllGroups[^1].Location = new Point((tab.Width - 15) / cnt * pos+5, sizelist[pos]);
                sizelist[pos] += controllGroups[^1].Height+5;
            }
        }
        public void FormEventDispose()
        {
            Stopwatch sw = new();
            sw.Start();
            var list = Config.Funcs.GetInstance().Configs.Data;
            
            for (int i = 0; i < list.Count; i++)
            {
                var list2 = list[i].Setting;
                for(int j = 0; j < list2.Count; j++)
                {
                    if (list2[j].Type == "readonly") list2[j].Value = string.Empty;
                }
            }
            for (int i = 0; i < controllGroups.Count; i++)
            {
                controllGroups[i].Dispose();
            }
            sw.Stop();
            Log.Instance.Info($"フォームイベントを破棄しました。計測:{sw.Elapsed}");
        }
        void SizeChanged(object? sender, EventArgs e)
        {
            if (NowSize.Width != Tab.Size.Width)
            {
                NowSize = Tab.Size;
                var list = Config.Funcs.GetInstance().Configs.Data;
                int cnt = 1;
                List<int> sizelist = new();
                while (Tab.Width / cnt > 450) cnt++;
                if (cnt > 1) while (Tab.Width / cnt < 300) cnt--;
                for (int i = 0; i < cnt; i++) sizelist.Add(0);
                for (int i = 0; i < list.Count; i++)
                {
                    Groups[i].Size = new Size((Tab.Width - 15) / cnt - 10, Groups[i].Size.Height);
                    int pos = 0;
                    int min = int.MaxValue;
                    for (int j = 0; j < sizelist.Count; j++)
                    {
                        if (min > sizelist[j])
                        {
                            min = sizelist[j];
                            pos = j;
                        }
                    }
                    controllGroups[i].Location = new Point((Tab.Width - 15) / cnt * pos + 5, sizelist[pos]);
                    sizelist[pos] += controllGroups[i].Height + 5;
                }
            }
            Tab.Update();
        }
        
    }

    class ControllGroup
    {
        public readonly GroupBox Box;
        readonly List<ToolBox> tools = new(); 
        public ControllGroup(GroupBox gb, List<Config.Funcs.IndexData> list,string name)
        {
            Box = gb;
            Box.Text = name;
            gb.Size=new Size(gb.Width, 22+23*list.Count+12);
            gb.SizeChanged += SizeChanged;
            for (int i = 0; i < list.Count; i++)
            {
                tools.Add(new ToolBox(gb, list[i], i));
            }
        }
        public int Width { get => Box.Width; set => Box.Width = value; }
        public int Height { get => Box.Height; }
        public Point Location { get => Box.Location; set => Box.Location = value; }
        public ControllGroup(GroupBox gb, string listName)
        {
            Box = gb;
            gb.SizeChanged += SizeChanged;
            var config = Config.Funcs.GetInstance().Configs.GetGroup(listName);
            if (config == null) return;
            for (int i = 0; i < config.Count; i++)
            {
                tools.Add(new ToolBox(gb, config[i], i));
            }
        }
        public void Dispose()
        {
            for (int i = 0; i < tools.Count; i++)
            {
                tools[i].Dispose();
            }
        }
        public void SizeChanged(object? sender, EventArgs e)
        {
            for (int i = 0; i < tools.Count; i++)
            {
                int w = Box.Width;
                switch (tools[i].Type)
                {
                    case "long":
                        tools[i].ToolUnitLabel.Location = new Point(w - 28, 22 + i * 23);
                        tools[i].ToolNumUD.Location = new Point(w - 108, 22 + i * 23);
                        tools[i].ToolTrack.Size = new Size(w - 234, 23);
                        break;
                    case "string":
                        tools[i].ToolTextBox.Size = new Size(w - 151, 23);
                        break;
                    case "bool":
                        tools[i].ToolCheckBox.Size = new Size(w - 149, 23);
                        break;
                    case "function":
                        tools[i].ToolButton.Size = new Size(w - 149, 23);
                        break;
                    case "readonly":
                        tools[i].ToolTextBox.Size = new Size(w - 151, 23);
                        break;
                    case "combo":
                        tools[i].ToolComboBox.Size = new Size(w - 149, 23);
                        break;
                }
            }
        }
    }
    class ToolBox
    {
        public Label ToolLabel = new();
        public Label ToolUnitLabel = new();
        public TrackBar ToolTrack = new();
        public NumericUpDown ToolNumUD = new();
        public TextBox ToolTextBox = new();
        public CheckBox ToolCheckBox = new();
        public Button ToolButton = new();
        public ComboBox ToolComboBox = new();
        readonly GroupBox gp;
        readonly Config.Funcs.IndexData cl;
        public string Type { get => cl.Type; }
        void LongInit(GroupBox gb, Config.Funcs.IndexData data, int pos)
        {
            //338
            var w = gb.Width;
            ((ISupportInitialize)(ToolNumUD)).BeginInit();
            ((ISupportInitialize)(ToolTrack)).BeginInit();
            gb.Controls.Add(ToolUnitLabel);
            gb.Controls.Add(ToolTrack);
            gb.Controls.Add(ToolNumUD);
            ToolUnitLabel.Location = new Point(w-28, 22 + pos * 23);
            ToolUnitLabel.Size = new Size(60, 23);
            ToolUnitLabel.Text = data.UnitName;
            ToolUnitLabel.TextAlign = ContentAlignment.MiddleLeft;
            ToolNumUD.Location = new Point(w-108, 22 + pos * 23);
            ToolNumUD.Size = new Size(80, 23);
            ToolTrack.AutoSize = false;
            ToolTrack.BackColor = Color.White;
            ToolTrack.Location = new Point(122, 22 + pos * 23);
            ToolTrack.Size = new Size(w-234, 23);
            ToolTrack.TickStyle = TickStyle.None;
            ToolTrack.LargeChange = (int)data.GetDisplayMag();
            ToolTrack.AccessibleDescription = data.Description;
            ToolTrack.Scroll += new EventHandler(TrackChanged);
            ToolNumUD.ValueChanged += new EventHandler(NumUDChanged);
            ToolNumUD.Increment = 1;
            ToolNumUD.AccessibleDescription = data.Description;
            ToolNumUD.Minimum = long.MinValue;
            ToolNumUD.Maximum = long.MaxValue;
            ToolNumUD.Value = (decimal)((long)data.Value / data.GetDisplayMag());
            ToolNumUD.DecimalPlaces = (int)Math.Log10(data.GetDisplayMag());
            ToolNumUD.Minimum = (decimal)((long)data.Min / data.GetDisplayMag());
            ToolNumUD.Maximum = (decimal)((long)data.Max / data.GetDisplayMag());
            ToolNumUD.TextAlign = HorizontalAlignment.Right;
            ToolTrack.Minimum = int.MinValue;
            ToolTrack.Maximum = int.MaxValue;
            ToolTrack.Value = (int)(long)data.Value;
            ToolTrack.Minimum = (int)(long)data.Min;
            ToolTrack.Maximum = (int)(long)data.Max;
            ((ISupportInitialize)(ToolNumUD)).EndInit();
            ((ISupportInitialize)(ToolTrack)).EndInit();
        }
        void StringInit(GroupBox gb, Config.Funcs.IndexData data, int pos)
        {
            //338
            var w = gb.Width;
            gb.Controls.Add(ToolTextBox); 
            ToolTextBox.Location = new Point(127, 22 + pos * 23);
            ToolTextBox.Size = new Size(w-151, 23);
            ToolTextBox.MaxLength = 255;
            ToolTextBox.Text = (string)data.Value;
            ToolTextBox.TextChanged += new EventHandler(TextBoxChanged);
        }
        void BoolInit(GroupBox gb, Config.Funcs.IndexData data, int pos)
        {
            //338
            var w = gb.Width;
            gb.Controls.Add(ToolCheckBox);
            ToolCheckBox.Location = new Point(122, 22 + pos * 23);
            ToolCheckBox.Size = new Size(w-149, 23);
            ToolCheckBox.Checked = (bool)data.Value;
            if (ToolCheckBox.Checked)
                ToolCheckBox.Text = cl.GetToggleOnText();
            else ToolCheckBox.Text = cl.GetToggleOffText();
            ToolCheckBox.Appearance = Appearance.Button;
            ToolCheckBox.CheckedChanged += new EventHandler(CheckBoxChanged);
            ToolCheckBox.TextAlign = ContentAlignment.MiddleCenter;
        }
        void FunctionInit(GroupBox gb, Config.Funcs.IndexData data, int pos)
        {
            //338
            var w = gb.Width;
            gb.Controls.Add(ToolButton);
            ToolButton.Location = new Point(122, 22 + pos * 23);
            ToolButton.Size = new Size(w-149, 23);
            ToolButton.Click += ButtonClick;
            ToolButton.Text = data.GetButton(!data.ButtonEnable);
            ToolButton.Enabled = data.ButtonEnable;
            data.ButtonChanged += ButtonChanged;
        }
        void ReadOnlyInit(GroupBox gb, Config.Funcs.IndexData data, int pos)
        {
            var w = gb.Width;
            gb.Controls.Add(ToolTextBox);
            ToolTextBox.Location = new Point(123, 22 + pos * 23);
            ToolTextBox.Size = new Size(w - 151, 23);
            ToolTextBox.MaxLength = 300;
            ToolTextBox.TextAlign = HorizontalAlignment.Center;
            ToolTextBox.Text = (string)data.Value;
            ToolTextBox.ReadOnly = true;
            data.ValueChanged += new EventHandler(UpdateText);
        }
        void ComboInit(GroupBox gb, Config.Funcs.IndexData data, int pos)
        {
            var w = gb.Width;
            gb.Controls.Add(ToolComboBox);
            ToolComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            ToolComboBox.FormattingEnabled = true;
            ToolComboBox.Items.AddRange(data.Items);
            ToolComboBox.Location = new Point(123, 22 + pos * 23);
            ToolComboBox.Size = new Size(w - 151, 23);
            ToolComboBox.SelectedIndex = (int)(long)data.Value;
            ToolComboBox.SelectedIndexChanged += ComboBoxChanged;
        }
        public void Dispose()
        {
            try
            {
                switch (cl.Type)
                {
                    case "readonly":
                        cl.ValueChanged -= UpdateText;
                        break;
                    case "function":
                        cl.ButtonChanged -= ButtonChanged;
                        break;
                }
            }
            catch(Exception ex)
            {
                Log.Instance.Warn(ex.Message);
            }
        }
        public ToolBox(GroupBox gb, Config.Funcs.IndexData data, int pos)//,)
        {
            gp = gb;
            cl = data;

            gb.Controls.Add(ToolLabel);
            ToolLabel.Location = new Point(6, 22 + pos * 23);
            ToolLabel.Size = new Size(116, 23);
            ToolLabel.Text = data.Title;
            ToolLabel.TextAlign = ContentAlignment.MiddleRight;
            switch (data.Type)
            {
                case "long":LongInit(gb,data, pos);break;
                case "string":StringInit(gb, data, pos); break;
                case "bool":BoolInit(gb, data, pos);break;
                case "function":FunctionInit(gb, data, pos);break;
                case "readonly":ReadOnlyInit(gb, data, pos);break;
                case "combo":ComboInit(gb, data, pos); break;
            }

            
        }
        void UpdateText(object? sender, EventArgs e)
        {
            if (gp.InvokeRequired)
            {
                gp.Invoke(() =>
                {
                    ToolTextBox.Text = (string)cl.Value;
                });
            }
            else
            {
                ToolTextBox.Text = (string)cl.Value;
            }
        }
        void ButtonClick(object? sender, EventArgs e)
        {
            cl.ExecuteAction();
        }
        void ButtonChanged(object? sender, EventArgs e)
        {
            ToolButton.Invoke(() =>
            {
                ToolButton.Enabled = cl.ButtonEnable;
                ToolButton.Text = cl.GetButton(!cl.ButtonEnable);
            });
        }
        void TrackChanged(object? sender, EventArgs e)
        {
                ToolNumUD.Value = (decimal)((double)ToolTrack.Value / cl.GetDisplayMag());
                cl.Value=(long)ToolTrack.Value;

        }
        void NumUDChanged(object? sender, EventArgs e)
        {
                ToolTrack.Value = (int)((double)ToolNumUD.Value * cl.GetDisplayMag());
                cl.Value=(long)ToolTrack.Value;
        }
        void TextBoxChanged(object? sender, EventArgs e)
        {
            cl.Value = ToolTextBox.Text;
        }
        void CheckBoxChanged(object? sender, EventArgs e)
        {
            cl.Value = ToolCheckBox.Checked;
            if (ToolCheckBox.Checked)
                ToolCheckBox.Text = cl.GetToggleOnText();
            else ToolCheckBox.Text = cl.GetToggleOffText();
        }
        void ComboBoxChanged(object? sender, EventArgs e)
        {
            cl.Value = ToolComboBox.SelectedIndex;
        }
    }
}
