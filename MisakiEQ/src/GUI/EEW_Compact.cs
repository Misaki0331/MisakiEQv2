using MisakiEQ.Background.API.EQInfo.JSON;
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
    public partial class EEW_Compact : Form
    {
        public EEW_Compact()
        {
            InitializeComponent();
            Icon = Properties.Resources.Logo_MainIcon;
        }
        public void SetInfomation(Struct.EEW eew)
        {
            Log.Debug("データの更新中...");
                SignalCount.Text = $"第 {eew.Serial.Number} 報{(eew.Serial.IsFinal?" (最終報)":"")}";
            string temp = string.Empty;
            switch (eew.Serial.Infomation)
            {
                case Struct.EEW.InfomationLevel.Default:
                    temp = "緊急地震速報は発表されていません。";
                    SignalCount.Text = string.Empty;
                    TopColor(Color.Teal, Color.White);
                    break;
                case Struct.EEW.InfomationLevel.Forecast:
                    temp = "緊急地震速報(予報)";
                    TopColor(Color.DarkOrange, Color.White);
                    Size = new Size(651,213);
                    break;
                case Struct.EEW.InfomationLevel.Warning:
                    temp = "緊急地震速報(警報)";
                    TopColor(Color.Red,Color.White);
                    Size = new Size(651, 375);
                    break;
                case Struct.EEW.InfomationLevel.Cancelled:
                    temp = "この緊急地震速報はキャンセルされました。";
                    SignalCount.Text = string.Empty;
                    TopColor(Color.Gray, Color.White);
                    break;
                case Struct.EEW.InfomationLevel.Test:
                    temp = "これは訓練です。";
                    TopColor(Color.DarkGreen, Color.White);
                    break;
                case Struct.EEW.InfomationLevel.CancelledTest:
                    temp = "この訓練はキャンセルされました。";
                    SignalCount.Text = string.Empty;
                    TopColor(Color.Gray, Color.White);
                    break;
                case Struct.EEW.InfomationLevel.Unknown:
                    temp = "不明な情報です。";
                    SignalCount.Text = string.Empty;
                    TopColor(Color.Gray, Color.White);
                    break;
            }
            SignalType.Text = temp;
            SetColor(false, eew.EarthQuake.MaxIntensity);
            Hypocenter.Text = eew.EarthQuake.Hypocenter;
            OriginTime.Text=eew.EarthQuake.OriginTime.ToString("yyyy/MM/dd HH:mm:ss");
            Magnitude.Text = $"M {eew.EarthQuake.Magnitude:0.0}";
            if (eew.EarthQuake.Depth < 0)
            {
                Depth.Text = "不明";
            }
            else if(eew.EarthQuake.Depth == 0)
            {
                Depth.Text = "ごく浅い";
            }
            else
            {
                Depth.Text = $"{eew.EarthQuake.Depth} km";
            }
            string args = "";
            for(int i=0;i< eew.EarthQuake.ForecastArea.LocalAreas.Count&&eew.Serial.Infomation==Struct.EEW.InfomationLevel.Warning; i++)
            {
                args += $"{MisakiEQ.Struct.EEWArea.LocalAreasToStr(eew.EarthQuake.ForecastArea.LocalAreas[i])} ";
            }
            Forecasts.Text=args;
            /*
            SetColor(true, eew.UserInfo.LocalIntensity);
            AreaIntensity.Text = Struct.Common.IntToStringShort(eew.UserInfo.LocalIntensity);
            */

            Log.Debug("データの更新完了");
        }
        void SetColor(bool IsArea, Struct.Common.Intensity value)
        {
            Color bg=Color.Gray;
            Color fc=Color.White;
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
            if (IsArea)
            {
                AreaColor(bg, fc);
                AreaIntensity.Text = Struct.Common.IntToStringShort(value);
            }
            else
            {
                InfoColor(bg, fc);
                MaxIntensity.Text = Struct.Common.IntToStringShort(value);
            }
        }
        void TopColor(Color bg,Color fr)
        {
            SignalType.BackColor = bg;
            SignalCount.BackColor = bg;
            SignalType.ForeColor = fr;
            SignalCount.ForeColor = fr;
            WarnLabel.BackColor = bg;
            Forecasts.BackColor = bg;
            WarnLabel.ForeColor = fr;
            Forecasts.ForeColor = fr;
        }
        void InfoColor(Color bg,Color fr)
        {
            MaxIntensityLabel.BackColor = bg;
            MaxIntensity.BackColor = bg;
            Hypocenter.BackColor = bg;
            OriginTime.BackColor = bg;
            Magnitude.BackColor = bg;
            DepthLabel.BackColor= bg;
            Depth.BackColor = bg;
            MaxIntensityLabel.ForeColor= fr;
            MaxIntensity.ForeColor= fr;
            Hypocenter.ForeColor= fr;
            OriginTime.ForeColor= fr;
            Magnitude.ForeColor= fr;
            DepthLabel.ForeColor= fr;
            Depth.ForeColor= fr;
        }
        void AreaColor(Color bg,Color fr)
        {
            AreaIntensity.BackColor= bg; 
            AreaIntensityLabel.BackColor= bg;
            AreaIntensity.ForeColor = fr;
            AreaIntensityLabel.ForeColor = fr;
        }

        private void EEW_Compact_FormClosing(object sender, FormClosingEventArgs e)
        {
            IsShowFromEEW = false;
            e.Cancel = true;
            Hide();
        }

        private async void EEW_Compact_Load(object sender, EventArgs e)
        {
            await Task.Delay(100);
            Opacity = 1;
        }

        private async void EEW_Compact_VisibleChanged(object sender, EventArgs e)
        {
            if (Visible)
            {
                Background.APIs.Instance.KyoshinAPI.UpdatedKyoshin += EEW_Compact_KyoshinEvent;
                SetColor(true, await Background.API.KyoshinAPI.KyoshinAPI.GetUserIntensity());
            }
            else Background.APIs.Instance.KyoshinAPI.UpdatedKyoshin -= EEW_Compact_KyoshinEvent;
        }
        private void EEW_Compact_KyoshinEvent(object? sender, EventArgs e)
        {
            Invoke(async () =>
            {
                SetColor(true, await Background.API.KyoshinAPI.KyoshinAPI.GetUserIntensity());
            });
            
        }
        public bool IsShowFromEEW = false;
        private void HideTimer_Tick(object sender, EventArgs e)
        {
            IsShowFromEEW = false;
            Hide();
            HideTimer.Stop();
        }
    }
}
