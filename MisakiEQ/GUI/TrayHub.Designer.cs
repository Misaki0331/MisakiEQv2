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
            this.DisplayEEWInfo = new System.Windows.Forms.ToolStripMenuItem();
            this.OpenKmoni = new System.Windows.Forms.ToolStripMenuItem();
            this.OpenAreaESTMonitor = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.OpenConfig = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.ExitApplication = new System.Windows.Forms.ToolStripMenuItem();
            this.速度応答グラフToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
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
            this.DisplayEEWInfo,
            this.OpenKmoni,
            this.OpenAreaESTMonitor,
            this.速度応答グラフToolStripMenuItem,
            this.toolStripSeparator1,
            this.OpenConfig,
            this.toolStripSeparator2,
            this.ExitApplication});
            this.ApplicationMenu.Name = "ApplicationMenu";
            this.ApplicationMenu.Size = new System.Drawing.Size(181, 170);
            this.ApplicationMenu.Opening += new System.ComponentModel.CancelEventHandler(this.ApplicationMenu_Opening);
            // 
            // DisplayEEWInfo
            // 
            this.DisplayEEWInfo.Name = "DisplayEEWInfo";
            this.DisplayEEWInfo.Size = new System.Drawing.Size(180, 22);
            this.DisplayEEWInfo.Text = "EEW簡易表示";
            this.DisplayEEWInfo.Click += new System.EventHandler(this.DisplayEEWInfo_Click);
            // 
            // OpenKmoni
            // 
            this.OpenKmoni.Name = "OpenKmoni";
            this.OpenKmoni.Size = new System.Drawing.Size(180, 22);
            this.OpenKmoni.Text = "強震モニタ";
            this.OpenKmoni.Click += new System.EventHandler(this.OpenKmoni_Click);
            // 
            // OpenAreaESTMonitor
            // 
            this.OpenAreaESTMonitor.Name = "OpenAreaESTMonitor";
            this.OpenAreaESTMonitor.Size = new System.Drawing.Size(180, 22);
            this.OpenAreaESTMonitor.Text = "エリア到達モニタ";
            this.OpenAreaESTMonitor.Click += new System.EventHandler(this.OpenAreaESTMonitor_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(177, 6);
            // 
            // OpenConfig
            // 
            this.OpenConfig.Name = "OpenConfig";
            this.OpenConfig.Size = new System.Drawing.Size(180, 22);
            this.OpenConfig.Text = "設定";
            this.OpenConfig.Click += new System.EventHandler(this.OpenConfig_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(177, 6);
            // 
            // ExitApplication
            // 
            this.ExitApplication.Name = "ExitApplication";
            this.ExitApplication.Size = new System.Drawing.Size(180, 22);
            this.ExitApplication.Text = "終了";
            this.ExitApplication.Click += new System.EventHandler(this.ExitApplication_Click);
            // 
            // 速度応答グラフToolStripMenuItem
            // 
            this.速度応答グラフToolStripMenuItem.Name = "速度応答グラフToolStripMenuItem";
            this.速度応答グラフToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.速度応答グラフToolStripMenuItem.Text = "速度応答グラフ";
            this.速度応答グラフToolStripMenuItem.Click += new System.EventHandler(this.速度応答グラフToolStripMenuItem_Click);
            // 
            // TrayHub
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(508, 331);
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
        private ToolStripMenuItem OpenKmoni;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripSeparator toolStripSeparator2;
        private ToolStripMenuItem OpenAreaESTMonitor;
        private ToolStripMenuItem 速度応答グラフToolStripMenuItem;
    }
}