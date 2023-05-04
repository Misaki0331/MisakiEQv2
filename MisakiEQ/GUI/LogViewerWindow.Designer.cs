namespace MisakiEQ.GUI
{
    partial class LogViewerWindow
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
            this.textBox1 = new System.Windows.Forms.RichTextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.Command = new System.Windows.Forms.TextBox();
            this.Execution = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.BackColor = System.Drawing.Color.Black;
            this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox1.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.textBox1.ForeColor = System.Drawing.Color.Gray;
            this.textBox1.Location = new System.Drawing.Point(0, 0);
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.ForcedVertical;
            this.textBox1.Size = new System.Drawing.Size(984, 409);
            this.textBox1.TabIndex = 1;
            this.textBox1.Text = "テキスト見本";
            this.textBox1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox1_KeyPress);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.Command);
            this.panel1.Controls.Add(this.Execution);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 409);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(984, 22);
            this.panel1.TabIndex = 2;
            this.panel1.Visible = false;
            // 
            // Command
            // 
            this.Command.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Command.Location = new System.Drawing.Point(0, 0);
            this.Command.Name = "Command";
            this.Command.Size = new System.Drawing.Size(909, 23);
            this.Command.TabIndex = 1;
            this.Command.DragDrop += new System.Windows.Forms.DragEventHandler(this.Command_DragDrop);
            this.Command.DragEnter += new System.Windows.Forms.DragEventHandler(this.Command_DragEnter);
            this.Command.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Command_KeyPress);
            // 
            // Execution
            // 
            this.Execution.Dock = System.Windows.Forms.DockStyle.Right;
            this.Execution.Location = new System.Drawing.Point(909, 0);
            this.Execution.Name = "Execution";
            this.Execution.Size = new System.Drawing.Size(75, 22);
            this.Execution.TabIndex = 0;
            this.Execution.Text = "Execute";
            this.Execution.UseVisualStyleBackColor = true;
            this.Execution.Click += new System.EventHandler(this.Execution_Click);
            // 
            // LogViewerWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(984, 431);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "LogViewerWindow";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "MisakiEQ Log";
            this.Activated += new System.EventHandler(this.LogViewerWindow_Activated);
            this.Deactivate += new System.EventHandler(this.LogViewerWindow_Deactivate);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.LogViewerWindow_FormClosed);
            this.Load += new System.EventHandler(this.LogViewerWindow_Load);
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.LogViewerWindow_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.LogViewerWindow_DragEnter);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private RichTextBox textBox1;
        private Panel panel1;
        private TextBox Command;
        private Button Execution;
    }
}