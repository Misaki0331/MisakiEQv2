using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MisakiEQ.GUI.ExApp
{
    public partial class UserESTWindow : Form
    {
        public UserESTWindow()
        {
            InitializeComponent();
            Icon = Properties.Resources.Logo_MainIcon;
        }

        private void UserESTWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            Hide();
        }
        double Raw = 0.0;
        void SetColor(double shindo)
        {
            var value=Struct.Common.FloatToInt(shindo);
            Raw = shindo;
            Color bg = Color.Gray;
            Color fc = Color.White;
            switch (value)
            {
                case Struct.Common.Intensity.Int0:
                    bg = Color.Gray;
                    fc = Color.White;
                    break;
                case Struct.Common.Intensity.Int1:
                    bg = Color.Gray;
                    fc = Color.White;
                    break;
                case Struct.Common.Intensity.Int2:
                    bg = Color.RoyalBlue;
                    fc = Color.White;
                    break;
                case Struct.Common.Intensity.Int3:
                    bg = Color.SeaGreen;
                    fc = Color.White;
                    break;
                case Struct.Common.Intensity.Int4:
                    bg = Color.FromArgb(255, 191, 191, 15);
                    fc = Color.White;
                    break;
                case Struct.Common.Intensity.Int5Down:
                case Struct.Common.Intensity.Int5Up:
                    bg = Color.OrangeRed;
                    fc = Color.White;
                    break;
                case Struct.Common.Intensity.Int6Down:
                case Struct.Common.Intensity.Int6Up:
                    bg = Color.Pink;
                    fc = Color.Red;
                    break;
                case Struct.Common.Intensity.Int7:
                    bg = Color.Purple;
                    fc = Color.White;
                    break;
            }
            Intensity.Text = Struct.Common.IntToStringShort(value);
            Intensity.BackColor = bg;
            Intensity.ForeColor = fc;
            IntensityLabel.BackColor = bg;
            IntensityLabel.ForeColor = fc;
            RawIntensity.BackColor = bg;
            RawIntensity.ForeColor = fc;
            RawIntensity.Text = $"{(shindo <= 0||double.IsNaN(shindo) ? "-.-" : shindo.ToString("0.0"))}";
        }
        public DateTime ESTTime;
        private async void KyoshinEvent(object? sender, EventArgs e)
        {
            await Invoke(async() =>
            {
                double shindo = await Background.API.KyoshinAPI.KyoshinAPI.GetUserRawIntensity();
                SetColor(shindo);
            });
        }

        private async void UserESTWindow_VisibleChanged(object sender, EventArgs e)
        {
            if (Visible)
            {
                Background.APIs.GetInstance().KyoshinAPI.UpdatedKyoshin += KyoshinEvent;
                UpdateTimer.Start();

                double shindo = await Background.API.KyoshinAPI.KyoshinAPI.GetUserRawIntensity();
                SetColor(shindo);
            }
            else
            {
                Background.APIs.GetInstance().KyoshinAPI.UpdatedKyoshin -= KyoshinEvent;
                UpdateTimer.Stop();
            }
        }
        void SetColor(Color bg,Color fc)
        {
            ESTLabel.BackColor = bg;
            ESTMilli.BackColor = bg;
            ESTSec.BackColor = bg;
            ESTLabel.ForeColor = fc;
            ESTMilli.ForeColor = fc;
            ESTSec.ForeColor = fc;
        }
        private void UpdateTimer_Tick(object sender, EventArgs e)
        {
            TimeSpan left=ESTTime-DateTime.Now;
            if (!double.IsNaN(Raw) && Raw > 0)
            {
                if (left.TotalMilliseconds >= 60000)
                {
                    SetColor(Color.Cyan, Color.Black);
                }
                else if (left.TotalMilliseconds > 30000)
                {
                    SetColor(Color.LimeGreen, Color.Black);
                }
                else if (left.TotalMilliseconds > 20000)
                {
                    SetColor(Color.Yellow, Color.Black);
                }
                else if (left.TotalMilliseconds > 10000)
                {
                    SetColor(Color.Pink, Color.Black);
                }
                else if (left.TotalMilliseconds >= 0)
                {
                    SetColor(Color.Pink, Color.FromArgb(255, (int)((double)255.0 * Math.Pow(left.Milliseconds / 1000.0, 2)), 0, 0));
                }
                else if (left.TotalMilliseconds >= -999999)
                {

                    SetColor(Color.Red, Color.White);
                }
                else
                {
                    SetColor(Color.Gray, Color.White);
                }
                if (left.TotalMilliseconds >= -999999)
                {
                    int t = (int)left.TotalMilliseconds;
                    if (t < 0) t *= -1;
                    ESTSec.Text = $"{t / 1000}";
                    ESTMilli.Text = $".{(t % 1000).ToString().PadLeft(3, '0')}";
                    if (left.TotalMilliseconds >= 0) ESTLabel.Text = "到達まで";
                    else ESTLabel.Text = "到達から";
                }
                else
                {
                    SetColor(Color.Gray, Color.White);
                    ESTLabel.Text = "到達まで";
                    ESTSec.Text = $"--";
                    ESTMilli.Text = $".---";
                }
            }
            else
            {
                SetColor(Color.Gray, Color.White);
                ESTLabel.Text = "到達まで";
                ESTSec.Text = $"--";
                ESTMilli.Text = $".---";
            }
        }
    }
}
