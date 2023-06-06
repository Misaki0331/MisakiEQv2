namespace MisakiEQ.GUI.Twitter
{
    partial class Auth
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
            this.label1 = new System.Windows.Forms.Label();
            this.Pincode = new System.Windows.Forms.TextBox();
            this.EnterButton = new System.Windows.Forms.Button();
            this.CheckButton = new System.Windows.Forms.Button();
            this.UnlockView = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("メイリオ", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(239, 24);
            this.label1.TabIndex = 0;
            this.label1.Text = "Please enter the pincode...";
            // 
            // Pincode
            // 
            this.Pincode.Font = new System.Drawing.Font("Meiryo UI", 27.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.Pincode.Location = new System.Drawing.Point(0, 27);
            this.Pincode.MaxLength = 7;
            this.Pincode.Name = "Pincode";
            this.Pincode.PasswordChar = '*';
            this.Pincode.Size = new System.Drawing.Size(187, 54);
            this.Pincode.TabIndex = 1;
            this.Pincode.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Pincode_KeyDown);
            this.Pincode.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Pincode_KeyPress);
            // 
            // EnterButton
            // 
            this.EnterButton.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.EnterButton.Location = new System.Drawing.Point(193, 54);
            this.EnterButton.Name = "EnterButton";
            this.EnterButton.Size = new System.Drawing.Size(63, 27);
            this.EnterButton.TabIndex = 2;
            this.EnterButton.Text = "Enter";
            this.EnterButton.UseVisualStyleBackColor = true;
            this.EnterButton.Click += new System.EventHandler(this.EnterButton_Click);
            // 
            // CheckButton
            // 
            this.CheckButton.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.CheckButton.Location = new System.Drawing.Point(193, 27);
            this.CheckButton.Name = "CheckButton";
            this.CheckButton.Size = new System.Drawing.Size(63, 27);
            this.CheckButton.TabIndex = 3;
            this.CheckButton.Text = "Check";
            this.CheckButton.UseVisualStyleBackColor = true;
            this.CheckButton.Click += new System.EventHandler(this.CheckButton_Click);
            // 
            // UnlockView
            // 
            this.UnlockView.Interval = 1000;
            this.UnlockView.Tick += new System.EventHandler(this.UnlockView_Tick);
            // 
            // Auth
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(257, 82);
            this.Controls.Add(this.CheckButton);
            this.Controls.Add(this.EnterButton);
            this.Controls.Add(this.Pincode);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "Auth";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Twitter認証";
            this.TopMost = true;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Label label1;
        private TextBox Pincode;
        private Button EnterButton;
        private Button CheckButton;
        private System.Windows.Forms.Timer UnlockView;
    }
}