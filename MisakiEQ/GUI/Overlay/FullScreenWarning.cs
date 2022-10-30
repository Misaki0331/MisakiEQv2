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
        }

        private void label2_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
