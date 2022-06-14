namespace MisakiEQ.GUI
{
    partial class EEW_Compact
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
            this.SignalType = new System.Windows.Forms.Label();
            this.MaxIntensity = new System.Windows.Forms.Label();
            this.MaxIntensityLabel = new System.Windows.Forms.Label();
            this.SignalCount = new System.Windows.Forms.Label();
            this.Magnitude = new System.Windows.Forms.Label();
            this.Hypocenter = new System.Windows.Forms.Label();
            this.Depth = new System.Windows.Forms.Label();
            this.DepthLabel = new System.Windows.Forms.Label();
            this.OriginTime = new System.Windows.Forms.Label();
            this.AreaIntensityLabel = new System.Windows.Forms.Label();
            this.AreaIntensity = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // SignalType
            // 
            this.SignalType.AutoSize = true;
            this.SignalType.BackColor = System.Drawing.Color.Teal;
            this.SignalType.Font = new System.Drawing.Font("Meiryo UI", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.SignalType.ForeColor = System.Drawing.Color.White;
            this.SignalType.Location = new System.Drawing.Point(0, 0);
            this.SignalType.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.SignalType.Name = "SignalType";
            this.SignalType.Size = new System.Drawing.Size(302, 41);
            this.SignalType.TabIndex = 2;
            this.SignalType.Text = "緊急地震速報(予報)";
            // 
            // MaxIntensity
            // 
            this.MaxIntensity.BackColor = System.Drawing.Color.Teal;
            this.MaxIntensity.Font = new System.Drawing.Font("MS UI Gothic", 62.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.MaxIntensity.ForeColor = System.Drawing.Color.White;
            this.MaxIntensity.Location = new System.Drawing.Point(1, 80);
            this.MaxIntensity.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.MaxIntensity.Name = "MaxIntensity";
            this.MaxIntensity.Size = new System.Drawing.Size(122, 93);
            this.MaxIntensity.TabIndex = 3;
            this.MaxIntensity.Text = "1";
            this.MaxIntensity.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // MaxIntensityLabel
            // 
            this.MaxIntensityLabel.BackColor = System.Drawing.Color.Teal;
            this.MaxIntensityLabel.Font = new System.Drawing.Font("Meiryo UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.MaxIntensityLabel.ForeColor = System.Drawing.Color.White;
            this.MaxIntensityLabel.Location = new System.Drawing.Point(1, 46);
            this.MaxIntensityLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.MaxIntensityLabel.Name = "MaxIntensityLabel";
            this.MaxIntensityLabel.Size = new System.Drawing.Size(122, 34);
            this.MaxIntensityLabel.TabIndex = 4;
            this.MaxIntensityLabel.Text = "最大震度";
            this.MaxIntensityLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // SignalCount
            // 
            this.SignalCount.BackColor = System.Drawing.Color.Teal;
            this.SignalCount.Font = new System.Drawing.Font("Meiryo UI", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.SignalCount.ForeColor = System.Drawing.Color.White;
            this.SignalCount.Location = new System.Drawing.Point(0, 0);
            this.SignalCount.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.SignalCount.Name = "SignalCount";
            this.SignalCount.Size = new System.Drawing.Size(635, 45);
            this.SignalCount.TabIndex = 5;
            this.SignalCount.Text = "第 12 報 (最終報)";
            this.SignalCount.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // Magnitude
            // 
            this.Magnitude.BackColor = System.Drawing.Color.Teal;
            this.Magnitude.Font = new System.Drawing.Font("Meiryo UI", 27.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.Magnitude.Location = new System.Drawing.Point(211, 126);
            this.Magnitude.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Magnitude.Name = "Magnitude";
            this.Magnitude.Size = new System.Drawing.Size(198, 47);
            this.Magnitude.TabIndex = 7;
            this.Magnitude.Text = "M 3.3";
            this.Magnitude.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // Hypocenter
            // 
            this.Hypocenter.BackColor = System.Drawing.Color.Teal;
            this.Hypocenter.Font = new System.Drawing.Font("Meiryo UI", 26.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.Hypocenter.Location = new System.Drawing.Point(124, 46);
            this.Hypocenter.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Hypocenter.Name = "Hypocenter";
            this.Hypocenter.Size = new System.Drawing.Size(511, 46);
            this.Hypocenter.TabIndex = 10;
            this.Hypocenter.Text = "北海道地方";
            this.Hypocenter.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // Depth
            // 
            this.Depth.BackColor = System.Drawing.Color.Teal;
            this.Depth.Font = new System.Drawing.Font("Meiryo UI", 27.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.Depth.Location = new System.Drawing.Point(426, 115);
            this.Depth.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Depth.Name = "Depth";
            this.Depth.Size = new System.Drawing.Size(209, 58);
            this.Depth.TabIndex = 11;
            this.Depth.Text = "100 km";
            this.Depth.TextAlign = System.Drawing.ContentAlignment.BottomRight;
            // 
            // DepthLabel
            // 
            this.DepthLabel.BackColor = System.Drawing.Color.Teal;
            this.DepthLabel.Font = new System.Drawing.Font("Meiryo UI", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.DepthLabel.Location = new System.Drawing.Point(373, 115);
            this.DepthLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.DepthLabel.Name = "DepthLabel";
            this.DepthLabel.Size = new System.Drawing.Size(94, 58);
            this.DepthLabel.TabIndex = 12;
            this.DepthLabel.Text = "深さ";
            this.DepthLabel.TextAlign = System.Drawing.ContentAlignment.BottomRight;
            // 
            // OriginTime
            // 
            this.OriginTime.BackColor = System.Drawing.Color.Teal;
            this.OriginTime.Font = new System.Drawing.Font("Meiryo UI", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.OriginTime.Location = new System.Drawing.Point(211, 92);
            this.OriginTime.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.OriginTime.Name = "OriginTime";
            this.OriginTime.Size = new System.Drawing.Size(424, 35);
            this.OriginTime.TabIndex = 13;
            this.OriginTime.Text = "2022/12/31 12:34:56";
            this.OriginTime.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // AreaIntensityLabel
            // 
            this.AreaIntensityLabel.BackColor = System.Drawing.Color.Teal;
            this.AreaIntensityLabel.Font = new System.Drawing.Font("Meiryo UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.AreaIntensityLabel.ForeColor = System.Drawing.Color.White;
            this.AreaIntensityLabel.Location = new System.Drawing.Point(124, 93);
            this.AreaIntensityLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.AreaIntensityLabel.Name = "AreaIntensityLabel";
            this.AreaIntensityLabel.Size = new System.Drawing.Size(86, 28);
            this.AreaIntensityLabel.TabIndex = 15;
            this.AreaIntensityLabel.Text = "地域震度";
            this.AreaIntensityLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // AreaIntensity
            // 
            this.AreaIntensity.BackColor = System.Drawing.Color.Teal;
            this.AreaIntensity.Font = new System.Drawing.Font("MS UI Gothic", 42F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.AreaIntensity.ForeColor = System.Drawing.Color.White;
            this.AreaIntensity.Location = new System.Drawing.Point(124, 95);
            this.AreaIntensity.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.AreaIntensity.Name = "AreaIntensity";
            this.AreaIntensity.Size = new System.Drawing.Size(86, 78);
            this.AreaIntensity.TabIndex = 14;
            this.AreaIntensity.Text = "1";
            this.AreaIntensity.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // EEW_Compact
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(635, 174);
            this.Controls.Add(this.Hypocenter);
            this.Controls.Add(this.AreaIntensityLabel);
            this.Controls.Add(this.AreaIntensity);
            this.Controls.Add(this.OriginTime);
            this.Controls.Add(this.DepthLabel);
            this.Controls.Add(this.Depth);
            this.Controls.Add(this.Magnitude);
            this.Controls.Add(this.MaxIntensityLabel);
            this.Controls.Add(this.MaxIntensity);
            this.Controls.Add(this.SignalType);
            this.Controls.Add(this.SignalCount);
            this.ForeColor = System.Drawing.Color.White;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "EEW_Compact";
            this.Opacity = 0D;
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "MisakiEQ 緊急地震速報";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.EEW_Compact_FormClosing);
            this.Load += new System.EventHandler(this.EEW_Compact_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label SignalType;
        private System.Windows.Forms.Label MaxIntensity;
        private System.Windows.Forms.Label MaxIntensityLabel;
        private System.Windows.Forms.Label SignalCount;
        private System.Windows.Forms.Label Magnitude;
        private System.Windows.Forms.Label Hypocenter;
        private System.Windows.Forms.Label Depth;
        private System.Windows.Forms.Label DepthLabel;
        private System.Windows.Forms.Label OriginTime;
        private System.Windows.Forms.Label AreaIntensityLabel;
        private System.Windows.Forms.Label AreaIntensity;
    }
}