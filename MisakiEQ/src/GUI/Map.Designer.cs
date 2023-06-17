namespace MisakiEQ.src.GUI
{
    partial class Map
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
            this.geoMap1 = new LiveCharts.WinForms.GeoMap();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // geoMap1
            // 
            this.geoMap1.BackColor = System.Drawing.Color.Black;
            this.geoMap1.ForeColor = System.Drawing.Color.Black;
            this.geoMap1.Location = new System.Drawing.Point(10, 10);
            this.geoMap1.Name = "geoMap1";
            this.geoMap1.Size = new System.Drawing.Size(316, 408);
            this.geoMap1.TabIndex = 0;
            this.geoMap1.Text = "geoMap1";
            this.geoMap1.Source = "C:\\Users\\Misaki\\fev\\MisakiEQ\\Resources\\Map\\1.xml";            // 
            // timer1
            // 
            this.timer1.Interval = 500;
            this.timer1.Tick += Map_Timer;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox1.Image = global::MisakiEQ.Properties.Utility.EEWWarning;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(1204, 427);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            // 
            // Map
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(1204, 427);
            this.Controls.Add(this.geoMap1);
            this.Controls.Add(this.pictureBox1);
            this.DoubleBuffered = true;
            this.Name = "Map";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Map";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private LiveCharts.WinForms.GeoMap geoMap1;
        private System.Windows.Forms.Timer timer1;
        private PictureBox pictureBox1;
    }
}