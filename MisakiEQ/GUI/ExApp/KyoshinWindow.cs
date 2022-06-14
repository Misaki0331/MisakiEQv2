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
    public partial class KyoshinWindow : Form
    {
        readonly Lib.AsyncLock s_lock = new();
        public KyoshinWindow()
        {
            InitializeComponent();
            KyoshinType.SelectedIndex = 0;
            Background.APIs.GetInstance().KyoshinAPI.UpdatedKyoshin += UpdateImage;
        }

        private void Position_CheckedChanged(object sender, EventArgs e)
        {
            if (Position.Checked)
                Position.Text = "地中";
            else Position.Text = "地表";

            UpdatePicture();
        }

        private void DisplayEEWShindo_CheckedChanged(object sender, EventArgs e)
        {
            UpdatePicture();
        }

        private void DisplayEEWCircle_CheckedChanged(object sender, EventArgs e)
        {
            UpdatePicture();
        }

        private void UpdateImage(object? sender, EventArgs e)
        {
            this.Invoke(new Action(() =>
            UpdatePicture()
            ));
        }
        private void KyoshinType_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdatePicture();
        }
        async void UpdatePicture()
        {
            using (await s_lock.LockAsync())
            {
                try
                {
                    var api = Background.APIs.GetInstance();
                    Bitmap img = new(Properties.Resources.K_moni_BaseMap);
                    Graphics g = Graphics.FromImage(img);
                    if (DisplayEEWShindo.Checked)
                    {
                        var image = await api.KyoshinAPI.GetImage(Background.API.KyoshinAPI.KyoshinAPI.KyoshinImageType.EstShindoImg);
                        if (image != null)
                        {
                            g.DrawImage(image, x: 0, y: 0);
                        }
                    }
                    var km = await api.KyoshinAPI.GetImage(GetEnumType());
                    if (km != null)
                    {
                        g.DrawImage(km, x: 0, y: 0);
                    }
                    if (DisplayEEWCircle.Checked)
                    {
                        var image = await api.KyoshinAPI.GetImage(Background.API.KyoshinAPI.KyoshinAPI.KyoshinImageType.PSWaveImg);
                        if (image != null)
                        {
                            g.DrawImage(image, x: 0, y: 0);
                        }
                    }
                    g.Dispose();
                    var tmp = pictureBox1.Image;
                    pictureBox1.Image = img;
                    tmp.Dispose();
                    gettext();
                }
                catch (Exception ex)
                {
                    Log.Logger.GetInstance().Warn($"強震モニタ描画中にエラー : {ex}");
                }
            }
        }
        Background.API.KyoshinAPI.KyoshinAPI.KyoshinImageType GetEnumType()
        {
            string type = "";
            switch (KyoshinType.SelectedIndex)
            {
                case 0:
                    type = "jma";
                    break;
                case 1:
                    type = "acmap";
                    break;
                case 2:
                    type = "vcmap";
                    break;
                case 3:
                    type = "dcmap";
                    break;
                case 4:
                    type = "rsp0125";
                    break;
                case 5:
                    type = "rsp0250";
                    break;
                case 6:
                    type = "rsp0500";
                    break;
                case 7:
                    type = "rsp1000";
                    break;
                case 8:
                    type = "rsp2000";
                    break;
                case 9:
                    type = "rsp4000";
                    break;
            }
            if (Position.Checked) type += "_b";
            else type += "_s";
            if(Enum.TryParse<Background.API.KyoshinAPI.KyoshinAPI.KyoshinImageType>(type,out var t))
            {
                return t;
            }
            else
            {
                throw new InvalidOperationException($"{type}はEnumに存在しません。");
            }
        }

        private void KyoshinWindow_FormClosed(object sender, FormClosedEventArgs e)
        {
            Background.APIs.GetInstance().KyoshinAPI.UpdatedKyoshin -= UpdateImage;
        }

        int x0=0, y0=0;
        private void gettext()
        {
            int x = x0;
            int y = y0;
            Bitmap b = (Bitmap)pictureBox1.Image;
            //Log.Logger.GetInstance().Debug($"{x} {y}");
            if (b.Width <= x || b.Height <= y) return;
            switch (KyoshinType.SelectedIndex)
            {
                case 0:
                    label1.Text = $"震度{Lib.KyoshinColor.GetIntensity(b.GetPixel(x,y)):0.00}";
                    break;
                case 1:
                    label1.Text = $"{Lib.KyoshinColor.GetPGA(b.GetPixel(x,y)):0.00}gal";
                    break;
                case 2:
                case 4:
                case 5:
                case 6:
                case 7:
                case 8:
                case 9:
                    label1.Text = $"{Lib.KyoshinColor.GetPGV(b.GetPixel(x,y)):0.00}cm/s";
                    break;
                case 3:
                    label1.Text = $"{Lib.KyoshinColor.GetPGD(b.GetPixel(x,y)):0.00}cm";
                    break;

            }
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            x0 = e.X;
            y0 = e.Y;
            gettext();
        }
    }
}
