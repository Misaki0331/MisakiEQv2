using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MisakiEQ;

namespace MisakiEQ.GUI.Sub
{
    public partial class AuthDmdata : Form
    {
        public AuthDmdata()
        {
            InitializeComponent();
        }
        readonly CancellationTokenSource cancel = new();
        private void AuthDmdata_FormClosed(object sender, FormClosedEventArgs e)
        {
            cancel.Cancel();
        }

        private async void AuthDmdata_Load(object sender, EventArgs e)
        {
            string? token = await Background.APIs.Instance.EEW.DMData.Authentication(cancel.Token);
            if(token == null)
            {
                Log.Warn("トークンの取得に失敗しました。");
                Text = "認証失敗";
                label1.ForeColor = Color.Red;
                label1.Text = "トークンの取得に失敗しました。";
                MessageBox.Show("DMDATAのリフレッシュトークンの取得に失敗しました。","認証失敗",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
            else
            {
                try
                {
                    using var writer = new StreamWriter("DMDataToken.cfg",false);
                    writer.Write(token);
                    Text = "認証成功";
                    label1.ForeColor = Color.LimeGreen;
                    label1.Text = "認証は認可されました。\nMisakiEQを再起動してください。";
                    MessageBox.Show("リフレッシュトークンを設定しました。\n取得APIを変更するには再起動してください。\n(設定の「アプリ情報」で現在使用しているAPIが確認できます。)", "認証成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch(Exception ex)
                {
                    Log.Error(ex);
                    Text = "認証データ保存失敗";
                    label1.ForeColor = Color.Red;
                    label1.Text = $"リフレッシュトークンの保存に失敗しました。\n{ex.Message}";
                    MessageBox.Show("リフレッシュトークンの保存に失敗しました。", "トークン保存失敗", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            Close();
        }
    }
}
