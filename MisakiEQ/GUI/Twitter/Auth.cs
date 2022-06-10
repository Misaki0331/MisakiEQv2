using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MisakiEQ.GUI.Twitter
{
    public partial class Auth : Form
    {
        public Auth()
        {
            InitializeComponent();
        }
        private void Pincode_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar == '\n')
            {
                EnterPincode();
            }
            if (e.KeyChar < '0' || '9' < e.KeyChar)
            {
                //押されたキーが 0～9でない場合は、イベントをキャンセルする
                e.Handled = true;
            }
        }
        int ViewTime = 0;
        private async void EnterPincode()
        {
            Pincode.ReadOnly = true;
            EnterButton.Enabled = false;
            label1.Text = "Authenticating...";
            bool IsSuccess=await Lib.Twitter.APIs.GetInstance().AuthFromPincode(Pincode.Text);
            if (IsSuccess)
            {
                label1.ForeColor = Color.DarkGreen;
                label1.Text = "Successed!";
                try
                {
                    using var stream = new StreamWriter("TwitterAuth.cfg");
                    stream.Write($"{Lib.Twitter.APIs.GetInstance().GetAccessToken()}\n{Lib.Twitter.APIs.GetInstance().GetAccessTokenSecret()}");
                    stream.Close();
                }
                catch (Exception ex)
                {
                    Log.Logger.GetInstance().Error(ex.Message);
                }
            }
            else
            {
                label1.ForeColor = Color.Red;
                label1.Text = "Failed! Try again.";
            }
            await Task.Delay(2000);
            Close();
        }
        private void EnterButton_Click(object sender, EventArgs e)
        {
            EnterPincode();
        }

        private void UnlockView_Tick(object sender, EventArgs e)
        {
            ViewTime--;
            if (ViewTime == 0)
            {
                CheckButton.Enabled = true;
                CheckButton.Text = "Check";
                UnlockView.Stop();
                Pincode.PasswordChar = '*';
            }
            else
            {
                CheckButton.Text = $"{ViewTime}s";
            }
        }

        private void CheckButton_Click(object sender, EventArgs e)
        {
            ViewTime = 5;
            UnlockView.Start();
            CheckButton.Enabled = false;
            CheckButton.Text = $"{ViewTime}s";
            Pincode.PasswordChar = '\0';
        }

        private void Pincode_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyData == Keys.Enter)
            {
                EnterPincode();
            }
        }
    }
}
