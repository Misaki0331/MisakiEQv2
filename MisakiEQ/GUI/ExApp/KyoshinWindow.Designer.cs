namespace MisakiEQ.GUI.ExApp
{
    partial class KyoshinWindow
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
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.KyoshinType = new System.Windows.Forms.ComboBox();
            this.Position = new System.Windows.Forms.CheckBox();
            this.DisplayEEWCircle = new System.Windows.Forms.CheckBox();
            this.DisplayEEWShindo = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::MisakiEQ.Properties.Resources.K_moni_BaseMap;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(352, 400);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // KyoshinType
            // 
            this.KyoshinType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.KyoshinType.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.KyoshinType.FormattingEnabled = true;
            this.KyoshinType.Items.AddRange(new object[] {
            "リアルタイム震度",
            "最大加速度",
            "最大速度",
            "最大変位",
            "0.125Hz速度応答",
            "0.25Hz速度応答",
            "0.5Hz速度応答",
            "1.0Hz速度応答",
            "2.0Hz速度応答",
            "4.0Hz速度応答"});
            this.KyoshinType.Location = new System.Drawing.Point(232, 377);
            this.KyoshinType.Name = "KyoshinType";
            this.KyoshinType.Size = new System.Drawing.Size(120, 23);
            this.KyoshinType.TabIndex = 1;
            this.KyoshinType.SelectedIndexChanged += new System.EventHandler(this.KyoshinType_SelectedIndexChanged);
            // 
            // Position
            // 
            this.Position.Appearance = System.Windows.Forms.Appearance.Button;
            this.Position.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.Position.Location = new System.Drawing.Point(192, 377);
            this.Position.Name = "Position";
            this.Position.Size = new System.Drawing.Size(40, 23);
            this.Position.TabIndex = 2;
            this.Position.Text = "地表";
            this.Position.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.Position.UseVisualStyleBackColor = true;
            this.Position.CheckedChanged += new System.EventHandler(this.Position_CheckedChanged);
            // 
            // DisplayEEWCircle
            // 
            this.DisplayEEWCircle.Appearance = System.Windows.Forms.Appearance.Button;
            this.DisplayEEWCircle.Checked = true;
            this.DisplayEEWCircle.CheckState = System.Windows.Forms.CheckState.Checked;
            this.DisplayEEWCircle.Location = new System.Drawing.Point(272, 354);
            this.DisplayEEWCircle.Name = "DisplayEEWCircle";
            this.DisplayEEWCircle.Size = new System.Drawing.Size(80, 23);
            this.DisplayEEWCircle.TabIndex = 3;
            this.DisplayEEWCircle.Text = "予測円表示";
            this.DisplayEEWCircle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.DisplayEEWCircle.UseVisualStyleBackColor = true;
            this.DisplayEEWCircle.CheckedChanged += new System.EventHandler(this.DisplayEEWCircle_CheckedChanged);
            // 
            // DisplayEEWShindo
            // 
            this.DisplayEEWShindo.Appearance = System.Windows.Forms.Appearance.Button;
            this.DisplayEEWShindo.Location = new System.Drawing.Point(192, 354);
            this.DisplayEEWShindo.Name = "DisplayEEWShindo";
            this.DisplayEEWShindo.Size = new System.Drawing.Size(80, 23);
            this.DisplayEEWShindo.TabIndex = 4;
            this.DisplayEEWShindo.Text = "推定震度";
            this.DisplayEEWShindo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.DisplayEEWShindo.UseVisualStyleBackColor = true;
            this.DisplayEEWShindo.CheckedChanged += new System.EventHandler(this.DisplayEEWShindo_CheckedChanged);
            // 
            // KyoshinWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(353, 401);
            this.Controls.Add(this.DisplayEEWShindo);
            this.Controls.Add(this.DisplayEEWCircle);
            this.Controls.Add(this.Position);
            this.Controls.Add(this.KyoshinType);
            this.Controls.Add(this.pictureBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "KyoshinWindow";
            this.Text = "MisakiEQ 強震モニタ";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.KyoshinWindow_FormClosed);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private PictureBox pictureBox1;
        private ComboBox KyoshinType;
        private CheckBox Position;
        private CheckBox DisplayEEWCircle;
        private CheckBox DisplayEEWShindo;
    }
}