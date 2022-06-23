using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MisakiEQ.Lib.ConfigController
{
    internal class Controller
    {
        readonly List<ToolBox> tools = new();
        public Controller(GroupBox gb,List<Config.Funcs.IndexData> list)
        {
            for(int i=0;i< list.Count; i++)
            {
                tools.Add(new ToolBox(gb, list[i], i));
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
        readonly Config.Funcs.IndexData cl;
        void LongInit(GroupBox gb, Config.Funcs.IndexData data, int pos)
        {

            ((ISupportInitialize)(ToolNumUD)).BeginInit();
            ((ISupportInitialize)(ToolTrack)).BeginInit();
            gb.Controls.Add(ToolUnitLabel);
            gb.Controls.Add(ToolTrack);
            gb.Controls.Add(ToolNumUD);
            ToolUnitLabel.Location = new Point(310, 22 + pos * 23);
            ToolUnitLabel.Size = new Size(60, 23);
            ToolUnitLabel.Text = data.UnitName;
            ToolUnitLabel.TextAlign = ContentAlignment.MiddleLeft;
            ToolNumUD.Location = new Point(230, 22 + pos * 23);
            ToolNumUD.Size = new Size(80, 23);
            ToolTrack.AutoSize = false;
            ToolTrack.BackColor = Color.White;
            ToolTrack.Location = new Point(122, 22 + pos * 23);
            ToolTrack.Size = new Size(104, 23);
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
            gb.Controls.Add(ToolTextBox); 
            ToolTextBox.Location = new Point(122, 22 + pos * 23);
            ToolTextBox.Size = new Size(200, 23);
            ToolTextBox.MaxLength = 255;
            ToolTextBox.Text = (string)data.Value;
            ToolTextBox.TextChanged += new EventHandler(TextBoxChanged);
        }
        void BoolInit(GroupBox gb, Config.Funcs.IndexData data, int pos)
        {
            gb.Controls.Add(ToolCheckBox);
            ToolCheckBox.Location = new Point(122, 22 + pos * 23);
            ToolCheckBox.Size = new Size(189, 23);
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
            gb.Controls.Add(ToolButton);
            ToolButton.Location = new Point(122, 22 + pos * 23);
            ToolButton.Size = new Size(189, 23);
            ToolButton.Click += ButtonClick;
            ToolButton.Text = data.GetButton(!data.ButtonEnable);
            ToolButton.Enabled = data.ButtonEnable;
            data.ButtonChanged += ButtonChanged;
        }
        public ToolBox(GroupBox gb, Config.Funcs.IndexData data, int pos)//,)
        {
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
    }
}
