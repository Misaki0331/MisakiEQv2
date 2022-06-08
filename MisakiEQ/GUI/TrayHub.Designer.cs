namespace MisakiEQ.GUI
{
    partial class TrayHub
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
            this.TrayIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.ApplicationMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.OpenConfig = new System.Windows.Forms.ToolStripMenuItem();
            this.ExitApplication = new System.Windows.Forms.ToolStripMenuItem();
            this.DisplayEEWInfo = new System.Windows.Forms.ToolStripMenuItem();
            this.ApplicationMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // TrayIcon
            // 
            this.TrayIcon.ContextMenuStrip = this.ApplicationMenu;
            this.TrayIcon.Text = "MisakiEQ v2";
            this.TrayIcon.Visible = true;
            this.TrayIcon.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.TrayIcon_MouseDoubleClick);
            // 
            // ApplicationMenu
            // 
            this.ApplicationMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.OpenConfig,
            this.ExitApplication,
            this.DisplayEEWInfo});
            this.ApplicationMenu.Name = "ApplicationMenu";
            this.ApplicationMenu.Size = new System.Drawing.Size(146, 70);
            // 
            // OpenConfig
            // 
            this.OpenConfig.Name = "OpenConfig";
            this.OpenConfig.Size = new System.Drawing.Size(145, 22);
            this.OpenConfig.Text = "設定";
            this.OpenConfig.Click += new System.EventHandler(this.OpenConfig_Click);
            // 
            // ExitApplication
            // 
            this.ExitApplication.Name = "ExitApplication";
            this.ExitApplication.Size = new System.Drawing.Size(145, 22);
            this.ExitApplication.Text = "終了";
            this.ExitApplication.Click += new System.EventHandler(this.ExitApplication_Click);
            // 
            // DisplayEEWInfo
            // 
            this.DisplayEEWInfo.Name = "DisplayEEWInfo";
            this.DisplayEEWInfo.Size = new System.Drawing.Size(145, 22);
            this.DisplayEEWInfo.Text = "EEW簡易表示";
            this.DisplayEEWInfo.Click += new System.EventHandler(this.DisplayEEWInfo_Click);
            // 
            // TrayHub
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(0, 0);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "TrayHub";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "TrayHub";
            this.ApplicationMenu.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private NotifyIcon TrayIcon;
        private ContextMenuStrip ApplicationMenu;
        private ToolStripMenuItem ExitApplication;
        private ToolStripMenuItem OpenConfig;
        private ToolStripMenuItem DisplayEEWInfo;
    }
}