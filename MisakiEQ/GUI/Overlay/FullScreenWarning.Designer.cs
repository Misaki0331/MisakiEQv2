namespace MisakiEQ.GUI.Overlay
{
    partial class FullScreenWarning
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
            this.LabelTitle = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.LabelIndexText = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.LabelDateTime = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // LabelTitle
            // 
            this.LabelTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.LabelTitle.Font = new System.Drawing.Font("Meiryo UI", 80.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.LabelTitle.ForeColor = System.Drawing.Color.White;
            this.LabelTitle.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.LabelTitle.Location = new System.Drawing.Point(0, 0);
            this.LabelTitle.Name = "LabelTitle";
            this.LabelTitle.Size = new System.Drawing.Size(800, 150);
            this.LabelTitle.TabIndex = 0;
            this.LabelTitle.Text = "警告";
            this.LabelTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label2.Font = new System.Drawing.Font("Yu Gothic UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label2.ForeColor = System.Drawing.Color.Yellow;
            this.label2.Location = new System.Drawing.Point(0, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(60, 27);
            this.label2.TabIndex = 1;
            this.label2.Text = "閉じる";
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // LabelIndexText
            // 
            this.LabelIndexText.Dock = System.Windows.Forms.DockStyle.Top;
            this.LabelIndexText.Font = new System.Drawing.Font("Meiryo UI", 54F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.LabelIndexText.ForeColor = System.Drawing.Color.White;
            this.LabelIndexText.Location = new System.Drawing.Point(0, 250);
            this.LabelIndexText.Name = "LabelIndexText";
            this.LabelIndexText.Size = new System.Drawing.Size(800, 400);
            this.LabelIndexText.TabIndex = 2;
            this.LabelIndexText.Text = "IndexText";
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(184)))), ((int)(((byte)(0)))), ((int)(((byte)(2)))));
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(0, 150);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(800, 100);
            this.label1.TabIndex = 2;
            // 
            // LabelDateTime
            // 
            this.LabelDateTime.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.LabelDateTime.Font = new System.Drawing.Font("Meiryo UI", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.LabelDateTime.ForeColor = System.Drawing.Color.White;
            this.LabelDateTime.Location = new System.Drawing.Point(0, 410);
            this.LabelDateTime.Name = "LabelDateTime";
            this.LabelDateTime.Size = new System.Drawing.Size(800, 40);
            this.LabelDateTime.TabIndex = 3;
            this.LabelDateTime.TextAlign = System.Drawing.ContentAlignment.BottomRight;
            // 
            // FullScreenWarning
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.LabelDateTime);
            this.Controls.Add(this.LabelIndexText);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.LabelTitle);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FullScreenWarning";
            this.Text = "FullScreenWarning";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Label LabelTitle;
        private Label label2;
        private Label LabelIndexText;
        private Label label1;
        private Label LabelDateTime;
    }
}