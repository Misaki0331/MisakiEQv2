namespace MisakiEQ.GUI
{
    partial class Earthquake_Info
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
            splitter1 = new Splitter();
            panel1 = new Panel();
            listBox1 = new ListBox();
            panel2 = new Panel();
            textBox1 = new TextBox();
            label9 = new Label();
            label10 = new Label();
            label7 = new Label();
            label8 = new Label();
            label6 = new Label();
            label5 = new Label();
            label4 = new Label();
            label3 = new Label();
            DetailInfo = new Label();
            DetailOriginTime = new Label();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            SuspendLayout();
            // 
            // splitter1
            // 
            splitter1.Location = new Point(309, 0);
            splitter1.Margin = new Padding(4, 5, 4, 5);
            splitter1.Name = "splitter1";
            splitter1.Size = new Size(4, 900);
            splitter1.TabIndex = 0;
            splitter1.TabStop = false;
            // 
            // panel1
            // 
            panel1.Controls.Add(listBox1);
            panel1.Dock = DockStyle.Left;
            panel1.Location = new Point(0, 0);
            panel1.Margin = new Padding(4, 5, 4, 5);
            panel1.Name = "panel1";
            panel1.Size = new Size(309, 900);
            panel1.TabIndex = 1;
            // 
            // listBox1
            // 
            listBox1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            listBox1.FormattingEnabled = true;
            listBox1.ItemHeight = 25;
            listBox1.Location = new Point(4, 60);
            listBox1.Margin = new Padding(4, 5, 4, 5);
            listBox1.Name = "listBox1";
            listBox1.ScrollAlwaysVisible = true;
            listBox1.Size = new Size(298, 829);
            listBox1.TabIndex = 0;
            // 
            // panel2
            // 
            panel2.Controls.Add(textBox1);
            panel2.Controls.Add(label9);
            panel2.Controls.Add(label10);
            panel2.Controls.Add(label7);
            panel2.Controls.Add(label8);
            panel2.Controls.Add(label6);
            panel2.Controls.Add(label5);
            panel2.Controls.Add(label4);
            panel2.Controls.Add(label3);
            panel2.Controls.Add(DetailInfo);
            panel2.Controls.Add(DetailOriginTime);
            panel2.Dock = DockStyle.Fill;
            panel2.Location = new Point(313, 0);
            panel2.Margin = new Padding(4, 5, 4, 5);
            panel2.MinimumSize = new Size(786, 600);
            panel2.Name = "panel2";
            panel2.Size = new Size(931, 900);
            panel2.TabIndex = 2;
            // 
            // textBox1
            // 
            textBox1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            textBox1.Location = new Point(9, 267);
            textBox1.Margin = new Padding(4, 5, 4, 5);
            textBox1.MaxLength = 100000;
            textBox1.Multiline = true;
            textBox1.Name = "textBox1";
            textBox1.ReadOnly = true;
            textBox1.ScrollBars = ScrollBars.Horizontal;
            textBox1.Size = new Size(917, 631);
            textBox1.TabIndex = 10;
            // 
            // label9
            // 
            label9.Font = new Font("Meiryo UI", 18F, FontStyle.Bold, GraphicsUnit.Point);
            label9.Location = new Point(131, 182);
            label9.Margin = new Padding(4, 0, 4, 0);
            label9.Name = "label9";
            label9.Size = new Size(194, 60);
            label9.TabIndex = 9;
            label9.Text = "{Text}";
            label9.TextAlign = ContentAlignment.BottomLeft;
            // 
            // label10
            // 
            label10.Font = new Font("Meiryo UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point);
            label10.Location = new Point(0, 180);
            label10.Margin = new Padding(4, 0, 4, 0);
            label10.Name = "label10";
            label10.Size = new Size(123, 60);
            label10.TabIndex = 8;
            label10.Text = "情報:";
            label10.TextAlign = ContentAlignment.BottomRight;
            // 
            // label7
            // 
            label7.Font = new Font("Meiryo UI", 18F, FontStyle.Bold, GraphicsUnit.Point);
            label7.Location = new Point(131, 122);
            label7.Margin = new Padding(4, 0, 4, 0);
            label7.Name = "label7";
            label7.Size = new Size(194, 60);
            label7.TabIndex = 7;
            label7.Text = "M{Value}";
            label7.TextAlign = ContentAlignment.BottomLeft;
            // 
            // label8
            // 
            label8.Font = new Font("Meiryo UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point);
            label8.Location = new Point(0, 120);
            label8.Margin = new Padding(4, 0, 4, 0);
            label8.Name = "label8";
            label8.Size = new Size(123, 60);
            label8.TabIndex = 6;
            label8.Text = "規模:";
            label8.TextAlign = ContentAlignment.BottomRight;
            // 
            // label6
            // 
            label6.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            label6.Font = new Font("Meiryo UI", 18F, FontStyle.Bold, GraphicsUnit.Point);
            label6.Location = new Point(570, 125);
            label6.Margin = new Padding(4, 0, 4, 0);
            label6.Name = "label6";
            label6.Size = new Size(173, 60);
            label6.TabIndex = 5;
            label6.Text = "{Depth}";
            label6.TextAlign = ContentAlignment.BottomLeft;
            // 
            // label5
            // 
            label5.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            label5.Font = new Font("Meiryo UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point);
            label5.Location = new Point(480, 122);
            label5.Margin = new Padding(4, 0, 4, 0);
            label5.Name = "label5";
            label5.Size = new Size(81, 60);
            label5.TabIndex = 4;
            label5.Text = "深さ:";
            label5.TextAlign = ContentAlignment.BottomRight;
            // 
            // label4
            // 
            label4.Font = new Font("Meiryo UI", 18F, FontStyle.Bold, GraphicsUnit.Point);
            label4.Location = new Point(131, 58);
            label4.Margin = new Padding(4, 0, 4, 0);
            label4.Name = "label4";
            label4.Size = new Size(373, 60);
            label4.TabIndex = 3;
            label4.Text = "{AreaName}";
            label4.TextAlign = ContentAlignment.BottomLeft;
            // 
            // label3
            // 
            label3.Font = new Font("Meiryo UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point);
            label3.Location = new Point(0, 60);
            label3.Margin = new Padding(4, 0, 4, 0);
            label3.Name = "label3";
            label3.Size = new Size(123, 60);
            label3.TabIndex = 2;
            label3.Text = "震源地:";
            label3.TextAlign = ContentAlignment.BottomRight;
            // 
            // DetailInfo
            // 
            DetailInfo.Font = new Font("Meiryo UI", 18F, FontStyle.Bold, GraphicsUnit.Point);
            DetailInfo.Location = new Point(4, 0);
            DetailInfo.Margin = new Padding(4, 0, 4, 0);
            DetailInfo.Name = "DetailInfo";
            DetailInfo.Size = new Size(343, 50);
            DetailInfo.TabIndex = 0;
            DetailInfo.Text = "詳細情報";
            // 
            // DetailOriginTime
            // 
            DetailOriginTime.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            DetailOriginTime.Font = new Font("Meiryo UI", 15.75F, FontStyle.Regular, GraphicsUnit.Point);
            DetailOriginTime.Location = new Point(503, 0);
            DetailOriginTime.Margin = new Padding(4, 0, 4, 0);
            DetailOriginTime.Name = "DetailOriginTime";
            DetailOriginTime.Size = new Size(429, 50);
            DetailOriginTime.TabIndex = 1;
            DetailOriginTime.Text = "2012/12/31 12:34:56発表";
            DetailOriginTime.TextAlign = ContentAlignment.TopRight;
            // 
            // Earthquake_Info
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1244, 900);
            Controls.Add(panel2);
            Controls.Add(splitter1);
            Controls.Add(panel1);
            Margin = new Padding(4, 5, 4, 5);
            Name = "Earthquake_Info";
            Text = "Earthquake_Info";
            panel1.ResumeLayout(false);
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Splitter splitter1;
        private Panel panel1;
        private ListBox listBox1;
        private Panel panel2;
        private Label DetailInfo;
        private Label DetailOriginTime;
        private Label label4;
        private Label label3;
        private Label label6;
        private Label label5;
        private Label label7;
        private Label label8;
        private TextBox textBox1;
        private Label label9;
        private Label label10;
    }
}