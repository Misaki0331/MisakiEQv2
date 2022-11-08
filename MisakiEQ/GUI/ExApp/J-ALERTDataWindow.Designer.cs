namespace MisakiEQ.GUI.ExApp
{
    partial class J_ALERTDataWindow
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
            this.title = new System.Windows.Forms.Label();
            this.BarLine = new System.Windows.Forms.Label();
            this.index = new System.Windows.Forms.Label();
            this.ReciveTime = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // title
            // 
            this.title.Dock = System.Windows.Forms.DockStyle.Top;
            this.title.Font = new System.Drawing.Font("Meiryo UI", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.title.ForeColor = System.Drawing.Color.White;
            this.title.Location = new System.Drawing.Point(0, 0);
            this.title.Name = "title";
            this.title.Size = new System.Drawing.Size(784, 62);
            this.title.TabIndex = 0;
            this.title.Text = "J-ALERT";
            this.title.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // BarLine
            // 
            this.BarLine.BackColor = System.Drawing.Color.Black;
            this.BarLine.Dock = System.Windows.Forms.DockStyle.Top;
            this.BarLine.Font = new System.Drawing.Font("Yu Gothic UI", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.BarLine.Location = new System.Drawing.Point(0, 62);
            this.BarLine.Name = "BarLine";
            this.BarLine.Size = new System.Drawing.Size(784, 60);
            this.BarLine.TabIndex = 1;
            this.BarLine.Text = "Data";
            this.BarLine.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // index
            // 
            this.index.BackColor = System.Drawing.Color.Black;
            this.index.Dock = System.Windows.Forms.DockStyle.Top;
            this.index.Font = new System.Drawing.Font("Yu Gothic UI", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.index.ForeColor = System.Drawing.Color.White;
            this.index.Location = new System.Drawing.Point(0, 122);
            this.index.Name = "index";
            this.index.Size = new System.Drawing.Size(784, 286);
            this.index.TabIndex = 2;
            this.index.Text = "72時間以内に受信されたデータはありません。\r\n更新次第ここに表示されます。";
            // 
            // ReciveTime
            // 
            this.ReciveTime.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.ReciveTime.Font = new System.Drawing.Font("Meiryo UI", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.ReciveTime.ForeColor = System.Drawing.Color.White;
            this.ReciveTime.Location = new System.Drawing.Point(0, 382);
            this.ReciveTime.Name = "ReciveTime";
            this.ReciveTime.Size = new System.Drawing.Size(784, 29);
            this.ReciveTime.TabIndex = 3;
            this.ReciveTime.Text = "----年--月--日--時--分受信";
            this.ReciveTime.TextAlign = System.Drawing.ContentAlignment.BottomRight;
            // 
            // J_ALERTDataWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(784, 411);
            this.Controls.Add(this.ReciveTime);
            this.Controls.Add(this.index);
            this.Controls.Add(this.BarLine);
            this.Controls.Add(this.title);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "J_ALERTDataWindow";
            this.Text = "J-ALERT";
            this.ResumeLayout(false);

        }

        #endregion

        private Label title;
        private Label BarLine;
        private Label index;
        private Label ReciveTime;
    }
}