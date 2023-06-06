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
            label7 = new Label();
            label8 = new Label();
            label6 = new Label();
            label5 = new Label();
            label4 = new Label();
            label3 = new Label();
            label1 = new Label();
            label2 = new Label();
            label9 = new Label();
            label10 = new Label();
            textBox1 = new TextBox();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            SuspendLayout();
            // 
            // splitter1
            // 
            splitter1.Location = new Point(216, 0);
            splitter1.Name = "splitter1";
            splitter1.Size = new Size(3, 540);
            splitter1.TabIndex = 0;
            splitter1.TabStop = false;
            // 
            // panel1
            // 
            panel1.Controls.Add(listBox1);
            panel1.Dock = DockStyle.Left;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(216, 540);
            panel1.TabIndex = 1;
            // 
            // listBox1
            // 
            listBox1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            listBox1.FormattingEnabled = true;
            listBox1.ItemHeight = 15;
            listBox1.Location = new Point(3, 36);
            listBox1.Name = "listBox1";
            listBox1.ScrollAlwaysVisible = true;
            listBox1.Size = new Size(210, 499);
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
            panel2.Controls.Add(label1);
            panel2.Controls.Add(label2);
            panel2.Dock = DockStyle.Fill;
            panel2.Location = new Point(219, 0);
            panel2.MinimumSize = new Size(550, 360);
            panel2.Name = "panel2";
            panel2.Size = new Size(652, 540);
            panel2.TabIndex = 2;
            // 
            // label7
            // 
            label7.Font = new Font("Meiryo UI", 18F, FontStyle.Bold, GraphicsUnit.Point);
            label7.Location = new Point(92, 73);
            label7.Name = "label7";
            label7.Size = new Size(136, 36);
            label7.TabIndex = 7;
            label7.Text = "M{Value}";
            label7.TextAlign = ContentAlignment.BottomLeft;
            // 
            // label8
            // 
            label8.Font = new Font("Meiryo UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point);
            label8.Location = new Point(0, 72);
            label8.Name = "label8";
            label8.Size = new Size(86, 36);
            label8.TabIndex = 6;
            label8.Text = "規模:";
            label8.TextAlign = ContentAlignment.BottomRight;
            // 
            // label6
            // 
            label6.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            label6.Font = new Font("Meiryo UI", 18F, FontStyle.Bold, GraphicsUnit.Point);
            label6.Location = new Point(399, 75);
            label6.Name = "label6";
            label6.Size = new Size(121, 36);
            label6.TabIndex = 5;
            label6.Text = "{Depth}";
            label6.TextAlign = ContentAlignment.BottomLeft;
            // 
            // label5
            // 
            label5.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            label5.Font = new Font("Meiryo UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point);
            label5.Location = new Point(336, 73);
            label5.Name = "label5";
            label5.Size = new Size(57, 36);
            label5.TabIndex = 4;
            label5.Text = "深さ:";
            label5.TextAlign = ContentAlignment.BottomRight;
            // 
            // label4
            // 
            label4.Font = new Font("Meiryo UI", 18F, FontStyle.Bold, GraphicsUnit.Point);
            label4.Location = new Point(92, 35);
            label4.Name = "label4";
            label4.Size = new Size(261, 36);
            label4.TabIndex = 3;
            label4.Text = "{AreaName}";
            label4.TextAlign = ContentAlignment.BottomLeft;
            // 
            // label3
            // 
            label3.Font = new Font("Meiryo UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point);
            label3.Location = new Point(0, 36);
            label3.Name = "label3";
            label3.Size = new Size(86, 36);
            label3.TabIndex = 2;
            label3.Text = "震源地:";
            label3.TextAlign = ContentAlignment.BottomRight;
            // 
            // label1
            // 
            label1.Font = new Font("Meiryo UI", 18F, FontStyle.Bold, GraphicsUnit.Point);
            label1.Location = new Point(3, 0);
            label1.Name = "label1";
            label1.Size = new Size(240, 30);
            label1.TabIndex = 0;
            label1.Text = "詳細情報";
            // 
            // label2
            // 
            label2.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            label2.Font = new Font("Meiryo UI", 15.75F, FontStyle.Regular, GraphicsUnit.Point);
            label2.Location = new Point(352, 0);
            label2.Name = "label2";
            label2.Size = new Size(300, 30);
            label2.TabIndex = 1;
            label2.Text = "2012/12/31 12:34:56発表";
            label2.TextAlign = ContentAlignment.TopRight;
            // 
            // label9
            // 
            label9.Font = new Font("Meiryo UI", 18F, FontStyle.Bold, GraphicsUnit.Point);
            label9.Location = new Point(92, 109);
            label9.Name = "label9";
            label9.Size = new Size(136, 36);
            label9.TabIndex = 9;
            label9.Text = "{Text}";
            label9.TextAlign = ContentAlignment.BottomLeft;
            // 
            // label10
            // 
            label10.Font = new Font("Meiryo UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point);
            label10.Location = new Point(0, 108);
            label10.Name = "label10";
            label10.Size = new Size(86, 36);
            label10.TabIndex = 8;
            label10.Text = "情報:";
            label10.TextAlign = ContentAlignment.BottomRight;
            // 
            // textBox1
            // 
            textBox1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            textBox1.Location = new Point(6, 160);
            textBox1.MaxLength = 100000;
            textBox1.Multiline = true;
            textBox1.Name = "textBox1";
            textBox1.ReadOnly = true;
            textBox1.ScrollBars = ScrollBars.Horizontal;
            textBox1.Size = new Size(643, 380);
            textBox1.TabIndex = 10;
            // 
            // Earthquake_Info
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(871, 540);
            Controls.Add(panel2);
            Controls.Add(splitter1);
            Controls.Add(panel1);
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
        private Label label1;
        private Label label2;
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