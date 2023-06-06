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
            this.versionName = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.DisplayEEWInfo = new System.Windows.Forms.ToolStripMenuItem();
            this.OpenKmoni = new System.Windows.Forms.ToolStripMenuItem();
            this.KyoshinGraphToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.OpenAreaESTMonitor = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.実行ログToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.OpenConfig = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.ExitApplication = new System.Windows.Forms.ToolStripMenuItem();
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
            this.versionName,
            this.toolStripSeparator3,
            this.DisplayEEWInfo,
            this.OpenKmoni,
            this.KyoshinGraphToolStripMenuItem,
            this.OpenAreaESTMonitor,
            this.toolStripMenuItem1,
            this.実行ログToolStripMenuItem,
            this.toolStripSeparator1,
            this.OpenConfig,
            this.toolStripSeparator2,
            this.ExitApplication});
            this.ApplicationMenu.Name = "ApplicationMenu";
            this.ApplicationMenu.Size = new System.Drawing.Size(208, 220);
            this.ApplicationMenu.Opening += new System.ComponentModel.CancelEventHandler(this.ApplicationMenu_Opening);
            // 
            // versionName
            // 
            this.versionName.Enabled = false;
            this.versionName.Name = "versionName";
            this.versionName.Size = new System.Drawing.Size(207, 22);
            this.versionName.Text = "Version";
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(204, 6);
            // 
            // DisplayEEWInfo
            // 
            this.DisplayEEWInfo.Name = "DisplayEEWInfo";
            this.DisplayEEWInfo.Size = new System.Drawing.Size(207, 22);
            this.DisplayEEWInfo.Text = "EEW簡易表示";
            this.DisplayEEWInfo.Click += new System.EventHandler(this.DisplayEEWInfo_Click);
            // 
            // OpenKmoni
            // 
            this.OpenKmoni.Name = "OpenKmoni";
            this.OpenKmoni.Size = new System.Drawing.Size(207, 22);
            this.OpenKmoni.Text = "強震モニタ";
            this.OpenKmoni.Click += new System.EventHandler(this.OpenKmoni_Click);
            // 
            // KyoshinGraphToolStripMenuItem
            // 
            this.KyoshinGraphToolStripMenuItem.Name = "KyoshinGraphToolStripMenuItem";
            this.KyoshinGraphToolStripMenuItem.Size = new System.Drawing.Size(207, 22);
            this.KyoshinGraphToolStripMenuItem.Text = "強震モニタ(グラフ)";
            this.KyoshinGraphToolStripMenuItem.Click += new System.EventHandler(this.KyoshinGraphToolStripMenuItem_Click);
            // 
            // OpenAreaESTMonitor
            // 
            this.OpenAreaESTMonitor.Enabled = false;
            this.OpenAreaESTMonitor.Name = "OpenAreaESTMonitor";
            this.OpenAreaESTMonitor.Size = new System.Drawing.Size(207, 22);
            this.OpenAreaESTMonitor.Text = "エリア到達モニタ(利用不可)";
            this.OpenAreaESTMonitor.Visible = false;
            this.OpenAreaESTMonitor.Click += new System.EventHandler(this.OpenAreaESTMonitor_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(207, 22);
            this.toolStripMenuItem1.Text = "J-ALERT情報";
            this.toolStripMenuItem1.Click += new System.EventHandler(this.toolStripMenuItem1_Click);
            // 
            // 実行ログToolStripMenuItem
            // 
            this.実行ログToolStripMenuItem.Name = "実行ログToolStripMenuItem";
            this.実行ログToolStripMenuItem.Size = new System.Drawing.Size(207, 22);
            this.実行ログToolStripMenuItem.Text = "実行ログ";
            this.実行ログToolStripMenuItem.Click += new System.EventHandler(this.実行ログToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(204, 6);
            // 
            // OpenConfig
            // 
            this.OpenConfig.Name = "OpenConfig";
            this.OpenConfig.Size = new System.Drawing.Size(207, 22);
            this.OpenConfig.Text = "設定";
            this.OpenConfig.Click += new System.EventHandler(this.OpenConfig_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(204, 6);
            // 
            // ExitApplication
            // 
            this.ExitApplication.Name = "ExitApplication";
            this.ExitApplication.Size = new System.Drawing.Size(207, 22);
            this.ExitApplication.Text = "終了";
            this.ExitApplication.Click += new System.EventHandler(this.ExitApplication_Click);
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
        private ToolStripMenuItem KyoshinGraphToolStripMenuItem;
        private ToolStripMenuItem 実行ログToolStripMenuItem;
        private ToolStripMenuItem versionName;
        private ToolStripSeparator toolStripSeparator3;
        private ToolStripMenuItem toolStripMenuItem1;
    }
}