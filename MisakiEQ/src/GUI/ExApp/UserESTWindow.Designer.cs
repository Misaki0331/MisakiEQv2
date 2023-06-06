namespace MisakiEQ.GUI.ExApp
{
    partial class UserESTWindow
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
            this.Intensity = new System.Windows.Forms.Label();
            this.IntensityLabel = new System.Windows.Forms.Label();
            this.ESTSec = new System.Windows.Forms.Label();
            this.ESTMilli = new System.Windows.Forms.Label();
            this.ESTLabel = new System.Windows.Forms.Label();
            this.UpdateTimer = new System.Windows.Forms.Timer(this.components);
            this.RawIntensity = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // Intensity
            // 
            this.Intensity.BackColor = System.Drawing.Color.Gray;
            this.Intensity.Font = new System.Drawing.Font("MS UI Gothic", 48F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.Intensity.ForeColor = System.Drawing.Color.White;
            this.Intensity.Location = new System.Drawing.Point(0, 19);
            this.Intensity.Name = "Intensity";
            this.Intensity.Size = new System.Drawing.Size(117, 67);
            this.Intensity.TabIndex = 0;
            this.Intensity.Text = "-";
            this.Intensity.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // IntensityLabel
            // 
            this.IntensityLabel.BackColor = System.Drawing.Color.Gray;
            this.IntensityLabel.Font = new System.Drawing.Font("Meiryo UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.IntensityLabel.ForeColor = System.Drawing.Color.White;
            this.IntensityLabel.Location = new System.Drawing.Point(0, -6);
            this.IntensityLabel.Name = "IntensityLabel";
            this.IntensityLabel.Size = new System.Drawing.Size(117, 37);
            this.IntensityLabel.TabIndex = 1;
            this.IntensityLabel.Text = "推定震度";
            this.IntensityLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ESTSec
            // 
            this.ESTSec.BackColor = System.Drawing.Color.Teal;
            this.ESTSec.Font = new System.Drawing.Font("MS UI Gothic", 72F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.ESTSec.ForeColor = System.Drawing.Color.White;
            this.ESTSec.Location = new System.Drawing.Point(118, 19);
            this.ESTSec.Name = "ESTSec";
            this.ESTSec.Size = new System.Drawing.Size(192, 87);
            this.ESTSec.TabIndex = 2;
            this.ESTSec.Text = "--";
            this.ESTSec.TextAlign = System.Drawing.ContentAlignment.BottomRight;
            // 
            // ESTMilli
            // 
            this.ESTMilli.BackColor = System.Drawing.Color.Teal;
            this.ESTMilli.Font = new System.Drawing.Font("MS UI Gothic", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.ESTMilli.ForeColor = System.Drawing.Color.White;
            this.ESTMilli.Location = new System.Drawing.Point(286, 19);
            this.ESTMilli.Name = "ESTMilli";
            this.ESTMilli.Size = new System.Drawing.Size(114, 87);
            this.ESTMilli.TabIndex = 3;
            this.ESTMilli.Text = ".---";
            this.ESTMilli.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // ESTLabel
            // 
            this.ESTLabel.BackColor = System.Drawing.Color.Teal;
            this.ESTLabel.Font = new System.Drawing.Font("Meiryo UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.ESTLabel.ForeColor = System.Drawing.Color.White;
            this.ESTLabel.Location = new System.Drawing.Point(118, -6);
            this.ESTLabel.Name = "ESTLabel";
            this.ESTLabel.Size = new System.Drawing.Size(273, 37);
            this.ESTLabel.TabIndex = 4;
            this.ESTLabel.Text = "到達まで";
            this.ESTLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // UpdateTimer
            // 
            this.UpdateTimer.Interval = 10;
            this.UpdateTimer.Tick += new System.EventHandler(this.UpdateTimer_Tick);
            // 
            // RawIntensity
            // 
            this.RawIntensity.BackColor = System.Drawing.Color.Gray;
            this.RawIntensity.Font = new System.Drawing.Font("MS UI Gothic", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.RawIntensity.ForeColor = System.Drawing.Color.White;
            this.RawIntensity.Location = new System.Drawing.Point(0, 76);
            this.RawIntensity.Name = "RawIntensity";
            this.RawIntensity.Size = new System.Drawing.Size(117, 30);
            this.RawIntensity.TabIndex = 5;
            this.RawIntensity.Text = "-.-";
            this.RawIntensity.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // UserESTWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(383, 106);
            this.Controls.Add(this.RawIntensity);
            this.Controls.Add(this.ESTLabel);
            this.Controls.Add(this.ESTMilli);
            this.Controls.Add(this.ESTSec);
            this.Controls.Add(this.IntensityLabel);
            this.Controls.Add(this.Intensity);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximumSize = new System.Drawing.Size(399, 145);
            this.MinimumSize = new System.Drawing.Size(199, 145);
            this.Name = "UserESTWindow";
            this.ShowInTaskbar = false;
            this.Text = "MisakiEQ 到達モニタ";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.UserESTWindow_FormClosing);
            this.VisibleChanged += new System.EventHandler(this.UserESTWindow_VisibleChanged);
            this.ResumeLayout(false);

        }

        #endregion

        private Label Intensity;
        private Label IntensityLabel;
        private Label ESTSec;
        private Label ESTMilli;
        private Label ESTLabel;
        private System.Windows.Forms.Timer UpdateTimer;
        private Label RawIntensity;
    }
}