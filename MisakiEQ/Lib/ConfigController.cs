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
        readonly Config.Funcs.IndexData cl;
        void LongInit(GroupBox gb, Config.Funcs.IndexData data, int pos)
        {

            ((ISupportInitialize)(ToolNumUD)).BeginInit();
            ((ISupportInitialize)(ToolTrack)).BeginInit();
            gb.Controls.Add(ToolUnitLabel);
            gb.Controls.Add(ToolTrack);
            gb.Controls.Add(ToolNumUD);
            ToolUnitLabel.Location = new Point(300, 22 + pos * 23);
            ToolUnitLabel.Size = new Size(60, 23);
            ToolUnitLabel.Text = data.GetUnitName();
            ToolUnitLabel.TextAlign = ContentAlignment.MiddleLeft;
            ToolNumUD.Location = new Point(230, 22 + pos * 23);
            ToolNumUD.Size = new Size(70, 23);
            ToolTrack.AutoSize = false;
            ToolTrack.BackColor = Color.White;
            ToolTrack.Location = new Point(122, 22 + pos * 23);
            ToolTrack.Size = new Size(104, 23);
            ToolTrack.TickStyle = TickStyle.None;
            ToolTrack.LargeChange = (int)data.GetDisplayMag();
            ToolTrack.AccessibleDescription = data.GetDescription();
            ToolTrack.Scroll += new EventHandler(TrackChanged);
            ToolNumUD.ValueChanged += new EventHandler(NumUDChanged);
            ToolNumUD.Increment = 1;
            ToolNumUD.AccessibleDescription = data.GetDescription();
            ToolNumUD.Minimum = long.MinValue;
            ToolNumUD.Maximum = long.MaxValue;
            ToolNumUD.Value = (decimal)((long)data.GetValue() / data.GetDisplayMag());
            ToolNumUD.DecimalPlaces = (int)Math.Log10(data.GetDisplayMag());
            ToolNumUD.Minimum = (decimal)((long)data.GetMin() / data.GetDisplayMag());
            ToolNumUD.Maximum = (decimal)((long)data.GetMax() / data.GetDisplayMag());
            ToolNumUD.TextAlign = HorizontalAlignment.Right;
            ToolTrack.Minimum = int.MinValue;
            ToolTrack.Maximum = int.MaxValue;
            ToolTrack.Value = (int)(long)data.GetValue();
            ToolTrack.Minimum = (int)(long)data.GetMin();
            ToolTrack.Maximum = (int)(long)data.GetMax();
            ((ISupportInitialize)(ToolNumUD)).EndInit();
            ((ISupportInitialize)(ToolTrack)).EndInit();
        }
        void StringInit(GroupBox gb, Config.Funcs.IndexData data, int pos)
        {
            gb.Controls.Add(ToolTextBox); 
            ToolTextBox.Location = new Point(122, 22 + pos * 23);
            ToolTextBox.Size = new Size(200, 23);
            ToolTextBox.MaxLength = 255;
            ToolTextBox.Text = (string)data.GetValue();
            ToolTextBox.TextChanged += new EventHandler(TextBoxChanged);
        }
        public ToolBox(GroupBox gb, Config.Funcs.IndexData data, int pos)//,)
        {
            cl = data;

            gb.Controls.Add(ToolLabel);
            ToolLabel.Location = new Point(6, 22 + pos * 23);
            ToolLabel.Size = new Size(116, 23);
            ToolLabel.Text = data.GetTitle();
            ToolLabel.TextAlign = ContentAlignment.MiddleRight;
            switch (data.GetManageType())
            {
                case "long":LongInit(gb,data, pos);break;
                case "string":StringInit(gb, data, pos); break;
            }

            
        }
        bool IsChanged = true;
        void TrackChanged(object? sender, EventArgs e)
        {
            if (!IsChanged)
            {
                IsChanged = true;
                ToolNumUD.Value = (decimal)((double)ToolTrack.Value / cl.GetDisplayMag());
                cl.SetValue((long)ToolTrack.Value);

            }
            else
            {
                IsChanged = false;
            }

        }
        void NumUDChanged(object? sender, EventArgs e)
        {
            if (!IsChanged)
            {
                IsChanged = true;
                ToolTrack.Value = (int)((double)ToolNumUD.Value * cl.GetDisplayMag());
                cl.SetValue((long)ToolTrack.Value);
            }
            else
            {
                IsChanged = false;
            }
        }
        void TextBoxChanged(object? sender, EventArgs e)
        {
            cl.SetValue(ToolTextBox.Text);
        }
    }
}
