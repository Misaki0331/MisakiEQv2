namespace MisakiEQ.GUI.ExApp.KyoshinGraphWindow
{
    partial class Setting
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
            this.label1 = new System.Windows.Forms.Label();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.ReadCommand = new System.Windows.Forms.Button();
            this.WriteCommand = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.checkBox3 = new System.Windows.Forms.CheckBox();
            this.checkBox4 = new System.Windows.Forms.CheckBox();
            this.checkBox5 = new System.Windows.Forms.CheckBox();
            this.checkBox6 = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(-1, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(54, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "メモリNo :";
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Location = new System.Drawing.Point(59, 7);
            this.numericUpDown1.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(45, 23);
            this.numericUpDown1.TabIndex = 1;
            this.numericUpDown1.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // ReadCommand
            // 
            this.ReadCommand.Location = new System.Drawing.Point(110, 7);
            this.ReadCommand.Name = "ReadCommand";
            this.ReadCommand.Size = new System.Drawing.Size(43, 23);
            this.ReadCommand.TabIndex = 2;
            this.ReadCommand.Text = "読取";
            this.ReadCommand.UseVisualStyleBackColor = true;
            this.ReadCommand.Click += new System.EventHandler(this.ReadCommand_Click);
            // 
            // WriteCommand
            // 
            this.WriteCommand.Location = new System.Drawing.Point(159, 7);
            this.WriteCommand.Name = "WriteCommand";
            this.WriteCommand.Size = new System.Drawing.Size(43, 23);
            this.WriteCommand.TabIndex = 3;
            this.WriteCommand.Text = "書込";
            this.WriteCommand.UseVisualStyleBackColor = true;
            this.WriteCommand.Click += new System.EventHandler(this.WriteCommand_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(-1, 33);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 15);
            this.label2.TabIndex = 4;
            this.label2.Text = "ウィンドウ名 :";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(72, 30);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(190, 23);
            this.textBox1.TabIndex = 5;
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // comboBox1
            // 
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "震度",
            "最大加速度",
            "最大速度",
            "最大変位",
            "応答速度"});
            this.comboBox1.Location = new System.Drawing.Point(72, 54);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(121, 23);
            this.comboBox1.TabIndex = 6;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.CheckBoxTextChanging);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(-1, 57);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(73, 15);
            this.label3.TabIndex = 7;
            this.label3.Text = "表示項目名 :";
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(12, 133);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(80, 19);
            this.checkBox1.TabIndex = 8;
            this.checkBox1.Text = "設定項目1";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.CheckBoxTextChanging);
            // 
            // checkBox2
            // 
            this.checkBox2.AutoSize = true;
            this.checkBox2.Location = new System.Drawing.Point(12, 158);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(80, 19);
            this.checkBox2.TabIndex = 9;
            this.checkBox2.Text = "設定項目2";
            this.checkBox2.UseVisualStyleBackColor = true;
            this.checkBox2.CheckedChanged += new System.EventHandler(this.CheckBoxTextChanging);
            // 
            // checkBox3
            // 
            this.checkBox3.AutoSize = true;
            this.checkBox3.Location = new System.Drawing.Point(12, 183);
            this.checkBox3.Name = "checkBox3";
            this.checkBox3.Size = new System.Drawing.Size(80, 19);
            this.checkBox3.TabIndex = 10;
            this.checkBox3.Text = "設定項目3";
            this.checkBox3.UseVisualStyleBackColor = true;
            this.checkBox3.CheckedChanged += new System.EventHandler(this.CheckBoxTextChanging);
            // 
            // checkBox4
            // 
            this.checkBox4.AutoSize = true;
            this.checkBox4.Location = new System.Drawing.Point(12, 208);
            this.checkBox4.Name = "checkBox4";
            this.checkBox4.Size = new System.Drawing.Size(80, 19);
            this.checkBox4.TabIndex = 11;
            this.checkBox4.Text = "設定項目4";
            this.checkBox4.UseVisualStyleBackColor = true;
            this.checkBox4.CheckedChanged += new System.EventHandler(this.CheckBoxTextChanging);
            // 
            // checkBox5
            // 
            this.checkBox5.AutoSize = true;
            this.checkBox5.Location = new System.Drawing.Point(12, 108);
            this.checkBox5.Name = "checkBox5";
            this.checkBox5.Size = new System.Drawing.Size(207, 19);
            this.checkBox5.TabIndex = 12;
            this.checkBox5.Text = "最寄り地点ではなく最大値を取得する";
            this.checkBox5.UseVisualStyleBackColor = true;
            this.checkBox5.CheckedChanged += new System.EventHandler(this.CheckBoxTextChanging);
            // 
            // checkBox6
            // 
            this.checkBox6.AutoSize = true;
            this.checkBox6.Location = new System.Drawing.Point(12, 83);
            this.checkBox6.Name = "checkBox6";
            this.checkBox6.Size = new System.Drawing.Size(176, 19);
            this.checkBox6.TabIndex = 13;
            this.checkBox6.Text = "棒グラフは一次関数の値を表示";
            this.checkBox6.UseVisualStyleBackColor = true;
            // 
            // Setting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(264, 241);
            this.Controls.Add(this.checkBox6);
            this.Controls.Add(this.checkBox5);
            this.Controls.Add(this.checkBox4);
            this.Controls.Add(this.checkBox3);
            this.Controls.Add(this.checkBox2);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.WriteCommand);
            this.Controls.Add(this.ReadCommand);
            this.Controls.Add(this.numericUpDown1);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "Setting";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "設定";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Setting_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Label label1;
        private NumericUpDown numericUpDown1;
        private Button ReadCommand;
        private Button WriteCommand;
        private Label label2;
        private TextBox textBox1;
        private ComboBox comboBox1;
        private Label label3;
        private CheckBox checkBox1;
        private CheckBox checkBox2;
        private CheckBox checkBox3;
        private CheckBox checkBox4;
        private CheckBox checkBox5;
        private CheckBox checkBox6;
    }
}