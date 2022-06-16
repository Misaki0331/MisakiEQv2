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
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.TwitterAuthInfo = new System.Windows.Forms.Label();
            this.button3 = new System.Windows.Forms.Button();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.TestButton = new System.Windows.Forms.CheckBox();
            this.LabelTime = new System.Windows.Forms.Label();
            this.LabelDate = new System.Windows.Forms.Label();
            this.UpdateDataTimer = new System.Windows.Forms.Timer(this.components);
            this.ButtonApply = new System.Windows.Forms.Button();
            this.ButtonCancel = new System.Windows.Forms.Button();
            this.ButtonOK = new System.Windows.Forms.Button();
            this.SettingTabs.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // SettingTabs
            // 
            this.SettingTabs.Controls.Add(this.tabPage1);
            this.SettingTabs.Controls.Add(this.tabPage2);
            this.SettingTabs.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.SettingTabs.Location = new System.Drawing.Point(-5, 53);
            this.SettingTabs.Name = "SettingTabs";
            this.SettingTabs.SelectedIndex = 0;
            this.SettingTabs.Size = new System.Drawing.Size(795, 475);
            this.SettingTabs.TabIndex = 0;
            // 
            // tabPage1
            // 
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
            this.tabPage2.Controls.Add(this.TwitterAuthInfo);
            this.tabPage2.Controls.Add(this.button3);
            this.tabPage2.Controls.Add(this.textBox2);
            this.tabPage2.Controls.Add(this.button1);
            this.tabPage2.Controls.Add(this.TestButton);
            this.tabPage2.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.tabPage2.Location = new System.Drawing.Point(4, 24);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(787, 447);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "テスト用";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // TwitterAuthInfo
            // 
            this.TwitterAuthInfo.AutoSize = true;
            this.TwitterAuthInfo.Location = new System.Drawing.Point(116, 40);
            this.TwitterAuthInfo.Name = "TwitterAuthInfo";
            this.TwitterAuthInfo.Size = new System.Drawing.Size(0, 15);
            this.TwitterAuthInfo.TabIndex = 11;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(213, 94);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(70, 23);
            this.button3.TabIndex = 10;
            this.button3.Text = "ツイート";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(6, 94);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(201, 23);
            this.textBox2.TabIndex = 9;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(6, 36);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(104, 23);
            this.button1.TabIndex = 6;
            this.button1.Text = "TwitterAuth";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // TestButton
            // 
            this.TestButton.Appearance = System.Windows.Forms.Appearance.Button;
            this.TestButton.Location = new System.Drawing.Point(6, 6);
            this.TestButton.Name = "TestButton";
            this.TestButton.Size = new System.Drawing.Size(104, 24);
            this.TestButton.TabIndex = 5;
            this.TestButton.Text = "発信テスト";
            this.TestButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.TestButton.UseVisualStyleBackColor = true;
            this.TestButton.CheckedChanged += new System.EventHandler(this.TestButton_CheckedChanged);
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
            this.Text = "設定画面だにょ";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Config_Menu_FormClosed);
            this.Load += new System.EventHandler(this.Config_Menu_Load);
            this.SettingTabs.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
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
        private CheckBox TestButton;
        private Button button1;
        private Button button3;
        private TextBox textBox2;
        private Label TwitterAuthInfo;
        private GroupBox groupBox2;
    }
}