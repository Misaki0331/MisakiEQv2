using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Media.Devices;
using static MisakiEQ.Lib.Config.Funcs;

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
                    if (list2[j] is ReadonlyIndexData read) read.Value = string.Empty;
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
            gb.Size = new Size(gb.Width, 22 + 23 * list.Count + 12);
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
                if (tools[i].Type == typeof(LongIndexData))
                {
                    tools[i].ToolUnitLabel.Location = new Point(w - 28, 22 + i * 23);
                    tools[i].ToolNumUD.Location = new Point(w - 108, 22 + i * 23);
                    tools[i].ToolTrack.Size = new Size(w - 234, 23);
                }else if (tools[i].Type == typeof(StringIndexData))
                {
                    tools[i].ToolTextBox.Size = new Size(w - 151, 23);
                }else if (tools[i].Type == typeof(BoolIndexData))
                {
                    tools[i].ToolCheckBox.Size = new Size(w - 149, 23);
                }
                else if (tools[i].Type == typeof(FunctionIndexData))
                {
                    tools[i].ToolButton.Size = new Size(w - 149, 23);
                }
                else if (tools[i].Type == typeof(ReadonlyIndexData))
                {
                    tools[i].ToolTextBox.Size = new Size(w - 151, 23);
                }
                else if (tools[i].Type == typeof(ComboIndexData))
                {
                    tools[i].ToolComboBox.Size = new Size(w - 149, 23);
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
        public Type Type { get => cl.GetType();}
        void LongInit(GroupBox gb, Config.Funcs.LongIndexData data, int pos)
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
            ToolTrack.LargeChange = (int)data.DisplayMag;
            ToolTrack.AccessibleDescription = data.Description;
            ToolTrack.Scroll += new EventHandler(TrackChanged);
            ToolNumUD.ValueChanged += new EventHandler(NumUDChanged);
            ToolNumUD.Increment = 1;
            ToolNumUD.AccessibleDescription = data.Description;
            ToolNumUD.Minimum = long.MinValue;
            ToolNumUD.Maximum = long.MaxValue;
            ToolNumUD.Value = (decimal)((long)data.Value / data.DisplayMag);
            ToolNumUD.DecimalPlaces = (int)Math.Log10(data.DisplayMag);
            ToolNumUD.Minimum = (decimal)((long)data.Min / data.DisplayMag);
            ToolNumUD.Maximum = (decimal)((long)data.Max / data.DisplayMag);
            ToolNumUD.TextAlign = HorizontalAlignment.Right;
            ToolTrack.Minimum = int.MinValue;
            ToolTrack.Maximum = int.MaxValue;
            ToolTrack.Value = (int)(long)data.Value;
            ToolTrack.Minimum = (int)(long)data.Min;
            ToolTrack.Maximum = (int)(long)data.Max;
            ((ISupportInitialize)(ToolNumUD)).EndInit();
            ((ISupportInitialize)(ToolTrack)).EndInit();
        }
        void StringInit(GroupBox gb, Config.Funcs.StringIndexData data, int pos)
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
        void BoolInit(GroupBox gb, Config.Funcs.BoolIndexData data, int pos)
        {
            //338
            var w = gb.Width;
            gb.Controls.Add(ToolCheckBox);
            ToolCheckBox.Location = new Point(122, 22 + pos * 23);
            ToolCheckBox.Size = new Size(w-149, 23);
            ToolCheckBox.Checked = (bool)data.Value;
            ToolCheckBox.Text = ((BoolIndexData)cl).GetToggleText(ToolCheckBox.Checked);
            ToolCheckBox.Appearance = Appearance.Button;
            ToolCheckBox.CheckedChanged += new EventHandler(CheckBoxChanged);
            ToolCheckBox.TextAlign = ContentAlignment.MiddleCenter;
        }
        void FunctionInit(GroupBox gb, Config.Funcs.FunctionIndexData data, int pos)
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
        void ReadOnlyInit(GroupBox gb, Config.Funcs.ReadonlyIndexData data, int pos)
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
        void ComboInit(GroupBox gb, Config.Funcs.ComboIndexData data, int pos)
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
                if (cl is ReadonlyIndexData read) read.ValueChanged -= UpdateText;
                if (cl is FunctionIndexData func) func.ButtonChanged -= ButtonChanged;
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
            if (data is LongIndexData L) LongInit(gb, L, pos);
            else if (data is StringIndexData S) StringInit(gb, S, pos);
            else if (data is BoolIndexData B) BoolInit(gb, B, pos);
            else if (data is FunctionIndexData F) FunctionInit(gb, F, pos);
            else if (data is ReadonlyIndexData R) ReadOnlyInit(gb, R, pos);
            else if (data is ComboIndexData C) ComboInit(gb, C, pos);


        }
        void UpdateText(object? sender, EventArgs e)
        {
            string text = "";
            if (cl is StringIndexData str) text = str.GetValue();
            else if (cl is ReadonlyIndexData read) text = read.GetValue();
            if (gp.InvokeRequired)
            {
                gp.Invoke(() =>
                {
                    ToolTextBox.Text = text;
                });
            }
            else
            {
                ToolTextBox.Text = text;
            }
        }
        void ButtonClick(object? sender, EventArgs e)
        {
            ((FunctionIndexData)cl).ExecuteAction();
        }
        void ButtonChanged(object? sender, EventArgs e)
        {
            ToolButton.Invoke(() =>
            {
                ToolButton.Enabled = ((FunctionIndexData)cl).ButtonEnable;
                ToolButton.Text = ((FunctionIndexData)cl).GetButton(!((FunctionIndexData)cl).ButtonEnable);
            });
        }
        void TrackChanged(object? sender, EventArgs e)
        {
            if (cl is LongIndexData)
            {
                ToolNumUD.Value = (decimal)((double)ToolTrack.Value / ((LongIndexData)cl).DisplayMag);
                ((LongIndexData)cl).Value = (long)ToolTrack.Value;
            }

        }
        void NumUDChanged(object? sender, EventArgs e)
        {
                ToolTrack.Value = (int)((double)ToolNumUD.Value * ((LongIndexData)cl).DisplayMag);
                ((LongIndexData)cl).Value =(long)ToolTrack.Value;
        }
        void TextBoxChanged(object? sender, EventArgs e)
        {
            ((StringIndexData)cl).Value = ToolTextBox.Text;
        }
        void CheckBoxChanged(object? sender, EventArgs e)
        {
            ((BoolIndexData)cl).Value = ToolCheckBox.Checked;
            ToolCheckBox.Text = ((BoolIndexData)cl).GetToggleText(ToolCheckBox.Checked);
        }
        void ComboBoxChanged(object? sender, EventArgs e)
        {
            ((ComboIndexData)cl).Value = ToolComboBox.SelectedIndex;
        }
    }
}
