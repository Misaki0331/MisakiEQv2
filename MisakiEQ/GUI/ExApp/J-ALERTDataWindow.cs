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
    public partial class J_ALERTDataWindow : Form
    {
        public J_ALERTDataWindow()
        {
            InitializeComponent();
            UpdateData();
        }
        public void UpdateData()
        {
            var data = Background.APIs.GetInstance().Jalert.LatestJAlert;
            if (data.IsValid)
            {
                BarLine.BackColor = Color.FromArgb(184, 0, 2);
                title.Text = data.Title;
                index.Text = data.Detail;
                ReciveTime.Text = $"{data.AnnounceTime:yyyy年MM月dd日HH時mm分受信}";
            }
        }

    }
}
