namespace MisakiEQ.GUI
{
    partial class Config_Menu
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
            this.SettingTabs = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.FixKyoshinTime = new System.Windows.Forms.Button();
            this.TwitterAuthInfo = new System.Windows.Forms.Label();
            this.TweetButton = new System.Windows.Forms.Button();
            this.TweetBox = new System.Windows.Forms.TextBox();
            this.AuthTwitter = new System.Windows.Forms.Button();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.LabelVersion = new System.Windows.Forms.Label();
            this.LinkToKoFi = new System.Windows.Forms.Button();
            this.LinkToDevTwitter = new System.Windows.Forms.Button();
            this.LinkToTwitterBot = new System.Windows.Forms.Button();
            this.LinkToGitHub = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.LabelTime = new System.Windows.Forms.Label();
            this.LabelDate = new System.Windows.Forms.Label();
            this.UpdateDataTimer = new System.Windows.Forms.Timer(this.components);
            this.ButtonApply = new System.Windows.Forms.Button();
            this.ButtonCancel = new System.Windows.Forms.Button();
            this.ButtonOK = new System.Windows.Forms.Button();
            this.SettingTabs.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // SettingTabs
            // 
            this.SettingTabs.Controls.Add(this.tabPage1);
            this.SettingTabs.Controls.Add(this.tabPage2);
            this.SettingTabs.Controls.Add(this.tabPage3);
            this.SettingTabs.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.SettingTabs.Location = new System.Drawing.Point(-5, 53);
            this.SettingTabs.Name = "SettingTabs";
            this.SettingTabs.SelectedIndex = 0;
            this.SettingTabs.Size = new System.Drawing.Size(795, 475);
            this.SettingTabs.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.groupBox3);
            this.tabPage1.Controls.Add(this.groupBox2);
            this.tabPage1.Controls.Add(this.groupBox1);
            this.tabPage1.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.tabPage1.Location = new System.Drawing.Point(4, 24);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(787, 447);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "一般";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Location = new System.Drawing.Point(351, 6);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(338, 335);
            this.groupBox3.TabIndex = 6;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "SNS関連投稿の設定";
            // 
            // groupBox2
            // 
            this.groupBox2.Location = new System.Drawing.Point(6, 347);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(339, 97);
            this.groupBox2.TabIndex = 6;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "ユーザー設定";
            // 
            // groupBox1
            // 
            this.groupBox1.Location = new System.Drawing.Point(7, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(338, 335);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "通信設定";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.FixKyoshinTime);
            this.tabPage2.Controls.Add(this.TwitterAuthInfo);
            this.tabPage2.Controls.Add(this.TweetButton);
            this.tabPage2.Controls.Add(this.TweetBox);
            this.tabPage2.Controls.Add(this.AuthTwitter);
            this.tabPage2.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.tabPage2.Location = new System.Drawing.Point(4, 24);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(787, 447);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "調整";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // FixKyoshinTime
            // 
            this.FixKyoshinTime.Location = new System.Drawing.Point(3, 35);
            this.FixKyoshinTime.Name = "FixKyoshinTime";
            this.FixKyoshinTime.Size = new System.Drawing.Size(113, 23);
            this.FixKyoshinTime.TabIndex = 12;
            this.FixKyoshinTime.Text = "強震モニタ時刻補正";
            this.FixKyoshinTime.UseVisualStyleBackColor = true;
            this.FixKyoshinTime.Click += new System.EventHandler(this.FixKyoshinTime_Click);
            // 
            // TwitterAuthInfo
            // 
            this.TwitterAuthInfo.AutoSize = true;
            this.TwitterAuthInfo.Location = new System.Drawing.Point(396, 14);
            this.TwitterAuthInfo.Name = "TwitterAuthInfo";
            this.TwitterAuthInfo.Size = new System.Drawing.Size(0, 15);
            this.TwitterAuthInfo.TabIndex = 11;
            // 
            // TweetButton
            // 
            this.TweetButton.Location = new System.Drawing.Point(320, 6);
            this.TweetButton.Name = "TweetButton";
            this.TweetButton.Size = new System.Drawing.Size(70, 23);
            this.TweetButton.TabIndex = 10;
            this.TweetButton.Text = "ツイート";
            this.TweetButton.UseVisualStyleBackColor = true;
            this.TweetButton.Click += new System.EventHandler(this.SendTweet);
            // 
            // TweetBox
            // 
            this.TweetBox.Location = new System.Drawing.Point(113, 6);
            this.TweetBox.Name = "TweetBox";
            this.TweetBox.Size = new System.Drawing.Size(201, 23);
            this.TweetBox.TabIndex = 9;
            // 
            // AuthTwitter
            // 
            this.AuthTwitter.Location = new System.Drawing.Point(3, 6);
            this.AuthTwitter.Name = "AuthTwitter";
            this.AuthTwitter.Size = new System.Drawing.Size(104, 23);
            this.AuthTwitter.TabIndex = 6;
            this.AuthTwitter.Text = "TwitterAuth";
            this.AuthTwitter.UseVisualStyleBackColor = true;
            this.AuthTwitter.Click += new System.EventHandler(this.OpenAuthTwitter);
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.LabelVersion);
            this.tabPage3.Controls.Add(this.LinkToKoFi);
            this.tabPage3.Controls.Add(this.LinkToDevTwitter);
            this.tabPage3.Controls.Add(this.LinkToTwitterBot);
            this.tabPage3.Controls.Add(this.LinkToGitHub);
            this.tabPage3.Controls.Add(this.pictureBox1);
            this.tabPage3.Location = new System.Drawing.Point(4, 24);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(787, 447);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "About";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // LabelVersion
            // 
            this.LabelVersion.AutoSize = true;
            this.LabelVersion.Location = new System.Drawing.Point(6, 154);
            this.LabelVersion.Name = "LabelVersion";
            this.LabelVersion.Size = new System.Drawing.Size(65, 15);
            this.LabelVersion.TabIndex = 4;
            this.LabelVersion.Text = "バージョン : ";
            // 
            // LinkToKoFi
            // 
            this.LinkToKoFi.Location = new System.Drawing.Point(424, 157);
            this.LinkToKoFi.Name = "LinkToKoFi";
            this.LinkToKoFi.Size = new System.Drawing.Size(88, 23);
            this.LinkToKoFi.TabIndex = 3;
            this.LinkToKoFi.Text = "Ko-fi";
            this.LinkToKoFi.UseVisualStyleBackColor = true;
            this.LinkToKoFi.Click += new System.EventHandler(this.LinkToKoFi_Click);
            // 
            // LinkToDevTwitter
            // 
            this.LinkToDevTwitter.Location = new System.Drawing.Point(518, 157);
            this.LinkToDevTwitter.Name = "LinkToDevTwitter";
            this.LinkToDevTwitter.Size = new System.Drawing.Size(88, 23);
            this.LinkToDevTwitter.TabIndex = 3;
            this.LinkToDevTwitter.Text = "Dev Twitter";
            this.LinkToDevTwitter.UseVisualStyleBackColor = true;
            this.LinkToDevTwitter.Click += new System.EventHandler(this.LinkToDevTwitter_Click);
            // 
            // LinkToTwitterBot
            // 
            this.LinkToTwitterBot.Location = new System.Drawing.Point(608, 157);
            this.LinkToTwitterBot.Name = "LinkToTwitterBot";
            this.LinkToTwitterBot.Size = new System.Drawing.Size(88, 23);
            this.LinkToTwitterBot.TabIndex = 2;
            this.LinkToTwitterBot.Text = "Twitter Bot";
            this.LinkToTwitterBot.UseVisualStyleBackColor = true;
            this.LinkToTwitterBot.Click += new System.EventHandler(this.LinkToTwitterBot_Click);
            // 
            // LinkToGitHub
            // 
            this.LinkToGitHub.Location = new System.Drawing.Point(697, 157);
            this.LinkToGitHub.Name = "LinkToGitHub";
            this.LinkToGitHub.Size = new System.Drawing.Size(80, 23);
            this.LinkToGitHub.TabIndex = 1;
            this.LinkToGitHub.Text = "GitHub";
            this.LinkToGitHub.UseVisualStyleBackColor = true;
            this.LinkToGitHub.Click += new System.EventHandler(this.LinkToGitHub_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::MisakiEQ.Properties.Images.header_logo;
            this.pictureBox1.Location = new System.Drawing.Point(0, 6);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(777, 145);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // LabelTime
            // 
            this.LabelTime.Font = new System.Drawing.Font("Meiryo UI", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.LabelTime.Location = new System.Drawing.Point(657, 27);
            this.LabelTime.Name = "LabelTime";
            this.LabelTime.Size = new System.Drawing.Size(123, 32);
            this.LabelTime.TabIndex = 1;
            this.LabelTime.Text = "--:--:--";
            this.LabelTime.TextAlign = System.Drawing.ContentAlignment.BottomRight;
            // 
            // LabelDate
            // 
            this.LabelDate.Font = new System.Drawing.Font("Meiryo UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.LabelDate.Location = new System.Drawing.Point(593, 0);
            this.LabelDate.Name = "LabelDate";
            this.LabelDate.Size = new System.Drawing.Size(187, 27);
            this.LabelDate.TabIndex = 2;
            this.LabelDate.Text = "----/--/-- (？)";
            this.LabelDate.TextAlign = System.Drawing.ContentAlignment.BottomRight;
            // 
            // UpdateDataTimer
            // 
            this.UpdateDataTimer.Tick += new System.EventHandler(this.UpdateDataTimer_Tick);
            // 
            // ButtonApply
            // 
            this.ButtonApply.Location = new System.Drawing.Point(701, 532);
            this.ButtonApply.Name = "ButtonApply";
            this.ButtonApply.Size = new System.Drawing.Size(75, 23);
            this.ButtonApply.TabIndex = 0;
            this.ButtonApply.Text = "適用";
            this.ButtonApply.UseVisualStyleBackColor = true;
            this.ButtonApply.Click += new System.EventHandler(this.ButtonApply_Click);
            // 
            // ButtonCancel
            // 
            this.ButtonCancel.Location = new System.Drawing.Point(620, 532);
            this.ButtonCancel.Name = "ButtonCancel";
            this.ButtonCancel.Size = new System.Drawing.Size(75, 23);
            this.ButtonCancel.TabIndex = 3;
            this.ButtonCancel.Text = "キャンセル";
            this.ButtonCancel.UseVisualStyleBackColor = true;
            this.ButtonCancel.Click += new System.EventHandler(this.ButtonCancel_Click);
            // 
            // ButtonOK
            // 
            this.ButtonOK.Location = new System.Drawing.Point(539, 532);
            this.ButtonOK.Name = "ButtonOK";
            this.ButtonOK.Size = new System.Drawing.Size(75, 23);
            this.ButtonOK.TabIndex = 4;
            this.ButtonOK.Text = "OK";
            this.ButtonOK.UseVisualStyleBackColor = true;
            this.ButtonOK.Click += new System.EventHandler(this.ButtonOK_Click);
            // 
            // Config_Menu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(780, 557);
            this.Controls.Add(this.ButtonOK);
            this.Controls.Add(this.ButtonCancel);
            this.Controls.Add(this.ButtonApply);
            this.Controls.Add(this.LabelDate);
            this.Controls.Add(this.LabelTime);
            this.Controls.Add(this.SettingTabs);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(800, 600);
            this.MinimumSize = new System.Drawing.Size(800, 600);
            this.Name = "Config_Menu";
            this.Text = "設定 - MisakIEQ";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Config_Menu_FormClosed);
            this.Load += new System.EventHandler(this.Config_Menu_Load);
            this.SettingTabs.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private TabControl SettingTabs;
        private TabPage tabPage1;
        private TabPage tabPage2;
        private Label LabelTime;
        private Label LabelDate;
        private System.Windows.Forms.Timer UpdateDataTimer;
        private Button ButtonApply;
        private Button ButtonCancel;
        private Button ButtonOK;
        private GroupBox groupBox1;
        private Button AuthTwitter;
        private Button TweetButton;
        private TextBox TweetBox;
        private Label TwitterAuthInfo;
        private GroupBox groupBox2;
        private GroupBox groupBox3;
        private Button FixKyoshinTime;
        private TabPage tabPage3;
        private PictureBox pictureBox1;
        private Button LinkToGitHub;
        private Button LinkToTwitterBot;
        private Button LinkToDevTwitter;
        private Button LinkToKoFi;
        private Label LabelVersion;
    }
}