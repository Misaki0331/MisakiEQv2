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
            title = new Label();
            BarLine = new Label();
            index = new Label();
            ReciveTime = new Label();
            SuspendLayout();
            // 
            // title
            // 
            title.Dock = DockStyle.Top;
            title.Font = new Font("Meiryo UI", 36F, FontStyle.Regular, GraphicsUnit.Point);
            title.ForeColor = Color.White;
            title.Location = new Point(0, 0);
            title.Margin = new Padding(4, 0, 4, 0);
            title.Name = "title";
            title.Size = new Size(1120, 103);
            title.TabIndex = 0;
            title.Text = "J-ALERT";
            title.TextAlign = ContentAlignment.TopCenter;
            // 
            // BarLine
            // 
            BarLine.BackColor = Color.Black;
            BarLine.Dock = DockStyle.Top;
            BarLine.Font = new Font("Yu Gothic UI", 36F, FontStyle.Regular, GraphicsUnit.Point);
            BarLine.Location = new Point(0, 103);
            BarLine.Margin = new Padding(4, 0, 4, 0);
            BarLine.Name = "BarLine";
            BarLine.Size = new Size(1120, 100);
            BarLine.TabIndex = 1;
            BarLine.TextAlign = ContentAlignment.TopCenter;
            // 
            // index
            // 
            index.BackColor = Color.Black;
            index.Dock = DockStyle.Top;
            index.Font = new Font("Yu Gothic UI", 21.75F, FontStyle.Bold, GraphicsUnit.Point);
            index.ForeColor = Color.White;
            index.Location = new Point(0, 203);
            index.Margin = new Padding(4, 0, 4, 0);
            index.Name = "index";
            index.Size = new Size(1120, 477);
            index.TabIndex = 2;
            index.Text = "72時間以内に受信されたデータはありません。\r\n更新次第ここに表示されます。";
            // 
            // ReciveTime
            // 
            ReciveTime.Dock = DockStyle.Bottom;
            ReciveTime.Font = new Font("Meiryo UI", 15.75F, FontStyle.Bold, GraphicsUnit.Point);
            ReciveTime.ForeColor = Color.White;
            ReciveTime.Location = new Point(0, 637);
            ReciveTime.Margin = new Padding(4, 0, 4, 0);
            ReciveTime.Name = "ReciveTime";
            ReciveTime.Size = new Size(1120, 48);
            ReciveTime.TabIndex = 3;
            ReciveTime.Text = "----年--月--日--時--分受信";
            ReciveTime.TextAlign = ContentAlignment.BottomRight;
            // 
            // J_ALERTDataWindow
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Black;
            ClientSize = new Size(1120, 685);
            Controls.Add(ReciveTime);
            Controls.Add(index);
            Controls.Add(BarLine);
            Controls.Add(title);
            FormBorderStyle = FormBorderStyle.SizableToolWindow;
            Margin = new Padding(4, 5, 4, 5);
            Name = "J_ALERTDataWindow";
            Text = "J-ALERT";
            ResumeLayout(false);
        }

        #endregion

        private Label title;
        private Label BarLine;
        private Label index;
        private Label ReciveTime;
    }
}