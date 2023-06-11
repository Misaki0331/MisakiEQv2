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
            geoMap1 = new LiveCharts.WinForms.GeoMap();
            timer1 = new System.Windows.Forms.Timer();
            SuspendLayout();
            // 
            // geoMap1
            // 
            geoMap1.BackColor = Color.Black;
            geoMap1.Dock = DockStyle.Fill;
            geoMap1.ForeColor = Color.Black;
            geoMap1.Location = new Point(0, 0);
            geoMap1.Margin = new Padding(4, 5, 4, 5);
            geoMap1.Name = "geoMap1";
            geoMap1.Size = new Size(834, 720);
            geoMap1.TabIndex = 0;
            geoMap1.Text = "geoMap1";
            // 
            // Map
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(834, 720);
            Controls.Add(geoMap1);
            Margin = new Padding(4, 5, 4, 5);
            Name = "Map";
            Text = "Map";
            FormClosed += Map_FormClosed;
            //
            // timer1
            //
            timer1.Interval = 500;
            timer1.Tick += Map_Timer;
            ResumeLayout(false);
        }

        #endregion

        private LiveCharts.WinForms.GeoMap geoMap1;
        private System.Windows.Forms.Timer timer1;
    }
}