using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MisakiEQ.GUI.Overlay
{
    public partial class FullScreenWarning : Form
    {
        public FullScreenWarning()
        {
            InitializeComponent();
        }
        public void TopShow()
        {
            Show();
            Activate();
            Size = new Size(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
            Location = new Point(Screen.PrimaryScreen.Bounds.X, Screen.PrimaryScreen.Bounds.Y);
            TopMost = true;
            //LabelIndexText.Location= new(0, 120);
            LabelIndexText.Size = new Size(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height - 290);
            var data = Background.APIs.GetInstance().Jalert.LatestJAlert;
            LabelTitle.Text=data.Title;
            string str = $"{data.Detail}\n\n対象地域:\n";
            for (int i = 0; i < data.Areas.Count; i++) str += $"{data.Areas[i]} ";
            LabelIndexText.Text = str;
            LabelDateTime.Text = $"{data.AnnounceTime:yyyy年MM月dd日HH時mm分受信}";
        }

        private void label2_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
