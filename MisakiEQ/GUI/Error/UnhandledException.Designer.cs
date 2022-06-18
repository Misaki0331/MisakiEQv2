namespace MisakiEQ.GUI.ErrorInfo
{
    partial class UnhandledException
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }


        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.ErrorInfomation = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.ErrorIndex = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.UserReport = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.RestartButton = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.Misaki_Image = new System.Windows.Forms.PictureBox();
            this.label4 = new System.Windows.Forms.Label();
            this.RestartMassage = new System.Windows.Forms.Label();
            this.RestartTimer = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.Misaki_Image)).BeginInit();
            this.SuspendLayout();
            // 
            // ErrorInfomation
            // 
            this.ErrorInfomation.AutoSize = true;
            this.ErrorInfomation.Font = new System.Drawing.Font("Meiryo UI", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.ErrorInfomation.Location = new System.Drawing.Point(114, 0);
            this.ErrorInfomation.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.ErrorInfomation.Name = "ErrorInfomation";
            this.ErrorInfomation.Size = new System.Drawing.Size(442, 70);
            this.ErrorInfomation.TabIndex = 0;
            this.ErrorInfomation.Text = "予期しないエラーが発生したため\r\nアプリケーションは動作を停止しました。";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label2.Location = new System.Drawing.Point(114, 70);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(315, 19);
            this.label2.TabIndex = 1;
            this.label2.Text = "どうやら水咲ちゃんはパニックになってしまったようです。";
            // 
            // ErrorIndex
            // 
            this.ErrorIndex.Location = new System.Drawing.Point(0, 312);
            this.ErrorIndex.Margin = new System.Windows.Forms.Padding(4);
            this.ErrorIndex.MaxLength = 65535;
            this.ErrorIndex.Multiline = true;
            this.ErrorIndex.Name = "ErrorIndex";
            this.ErrorIndex.ReadOnly = true;
            this.ErrorIndex.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.ErrorIndex.Size = new System.Drawing.Size(811, 219);
            this.ErrorIndex.TabIndex = 2;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(4, 542);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(76, 15);
            this.label3.TabIndex = 3;
            this.label3.Text = "お役立ちリンク";
            // 
            // UserReport
            // 
            this.UserReport.Location = new System.Drawing.Point(0, 130);
            this.UserReport.Margin = new System.Windows.Forms.Padding(4);
            this.UserReport.MaxLength = 65535;
            this.UserReport.Multiline = true;
            this.UserReport.Name = "UserReport";
            this.UserReport.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.UserReport.Size = new System.Drawing.Size(811, 156);
            this.UserReport.TabIndex = 4;
            this.UserReport.MouseClick += new System.Windows.Forms.MouseEventHandler(this.UserReport_MouseClick);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(94, 536);
            this.button1.Margin = new System.Windows.Forms.Padding(4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(177, 28);
            this.button1.TabIndex = 6;
            this.button1.Text = "開発者TwitterのDMに送信";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(4, 291);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(80, 15);
            this.label5.TabIndex = 7;
            this.label5.Text = "エラー詳細情報";
            // 
            // RestartButton
            // 
            this.RestartButton.Location = new System.Drawing.Point(623, 535);
            this.RestartButton.Margin = new System.Windows.Forms.Padding(4);
            this.RestartButton.Name = "RestartButton";
            this.RestartButton.Size = new System.Drawing.Size(91, 29);
            this.RestartButton.TabIndex = 9;
            this.RestartButton.Text = "再起動";
            this.RestartButton.UseVisualStyleBackColor = true;
            this.RestartButton.Click += new System.EventHandler(this.RestartButton_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(721, 535);
            this.button2.Margin = new System.Windows.Forms.Padding(4);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(91, 29);
            this.button2.TabIndex = 10;
            this.button2.Text = "終了";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // Misaki_Image
            // 
            this.Misaki_Image.Image = global::MisakiEQ.Properties.Images.help;
            this.Misaki_Image.Location = new System.Drawing.Point(0, -4);
            this.Misaki_Image.Margin = new System.Windows.Forms.Padding(4);
            this.Misaki_Image.Name = "Misaki_Image";
            this.Misaki_Image.Size = new System.Drawing.Size(170, 138);
            this.Misaki_Image.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.Misaki_Image.TabIndex = 11;
            this.Misaki_Image.TabStop = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label4.Location = new System.Drawing.Point(130, 111);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(307, 15);
            this.label4.TabIndex = 5;
            this.label4.Text = "下のテキストボックスにエラーが発生する具体的な動作を書こう！";
            // 
            // RestartMassage
            // 
            this.RestartMassage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.RestartMassage.AutoSize = true;
            this.RestartMassage.Location = new System.Drawing.Point(352, 541);
            this.RestartMassage.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.RestartMassage.Name = "RestartMassage";
            this.RestartMassage.Size = new System.Drawing.Size(232, 15);
            this.RestartMassage.TabIndex = 12;
            this.RestartMassage.Text = "MisakiEQは 20 秒後、自動的に再起動します。";
            this.RestartMassage.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // RestartTimer
            // 
            this.RestartTimer.Enabled = true;
            this.RestartTimer.Interval = 1000;
            this.RestartTimer.Tick += new System.EventHandler(this.RestartTimer_Tick);
            // 
            // UnhandledException
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(812, 562);
            this.Controls.Add(this.RestartMassage);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.UserReport);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.ErrorInfomation);
            this.Controls.Add(this.Misaki_Image);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.RestartButton);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.ErrorIndex);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "UnhandledException";
            this.Text = "エラー - MisakiEQ";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ExceptionMassage_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.Misaki_Image)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label ErrorInfomation;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox ErrorIndex;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox UserReport;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label5;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.Button RestartButton;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.PictureBox Misaki_Image;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label RestartMassage;
        private System.Windows.Forms.Timer RestartTimer;
    }
}