
namespace AutoTyper
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.AutoTypeTextBut = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.DelayStartSecsNum = new System.Windows.Forms.NumericUpDown();
            this.DelayCharsSendNum = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.TxtTimer = new System.Windows.Forms.Timer(this.components);
            this.TextBuffer = new System.Windows.Forms.TextBox();
            this.StartTimer = new System.Windows.Forms.Timer(this.components);
            this.IconTray = new System.Windows.Forms.NotifyIcon(this.components);
            this.PopMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ConfigMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SendClipboardTextMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.AboutMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ExitAppMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1 = new System.Windows.Forms.Panel();
            this.TurboTypeChk = new System.Windows.Forms.CheckBox();
            this.TopPos = new System.Windows.Forms.Label();
            this.AutoTypeClipboardBut = new System.Windows.Forms.Button();
            this.ProgBar = new System.Windows.Forms.ProgressBar();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.ProgTimer = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.DelayStartSecsNum)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DelayCharsSendNum)).BeginInit();
            this.PopMenu.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // AutoTypeTextBut
            // 
            this.AutoTypeTextBut.Location = new System.Drawing.Point(6, 65);
            this.AutoTypeTextBut.Name = "AutoTypeTextBut";
            this.AutoTypeTextBut.Size = new System.Drawing.Size(125, 23);
            this.AutoTypeTextBut.TabIndex = 0;
            this.AutoTypeTextBut.Text = "Auto Type Text Below";
            this.AutoTypeTextBut.UseVisualStyleBackColor = true;
            this.AutoTypeTextBut.Click += new System.EventHandler(this.AutoTypeTextBut_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(160, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Delay seconds before auto type:";
            // 
            // DelayStartSecsNum
            // 
            this.DelayStartSecsNum.Location = new System.Drawing.Point(232, 23);
            this.DelayStartSecsNum.Maximum = new decimal(new int[] {
            99999,
            0,
            0,
            0});
            this.DelayStartSecsNum.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.DelayStartSecsNum.Name = "DelayStartSecsNum";
            this.DelayStartSecsNum.Size = new System.Drawing.Size(120, 20);
            this.DelayStartSecsNum.TabIndex = 3;
            this.DelayStartSecsNum.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // DelayCharsSendNum
            // 
            this.DelayCharsSendNum.Location = new System.Drawing.Point(232, 44);
            this.DelayCharsSendNum.Maximum = new decimal(new int[] {
            99999,
            0,
            0,
            0});
            this.DelayCharsSendNum.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.DelayCharsSendNum.Name = "DelayCharsSendNum";
            this.DelayCharsSendNum.Size = new System.Drawing.Size(120, 20);
            this.DelayCharsSendNum.TabIndex = 5;
            this.DelayCharsSendNum.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 46);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(219, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Delay milli-seconds between auto type chars:";
            // 
            // TxtTimer
            // 
            this.TxtTimer.Tick += new System.EventHandler(this.TxtTimer_Tick);
            // 
            // TextBuffer
            // 
            this.TextBuffer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TextBuffer.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TextBuffer.Location = new System.Drawing.Point(3, 103);
            this.TextBuffer.Multiline = true;
            this.TextBuffer.Name = "TextBuffer";
            this.TextBuffer.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.TextBuffer.Size = new System.Drawing.Size(404, 123);
            this.TextBuffer.TabIndex = 6;
            // 
            // StartTimer
            // 
            this.StartTimer.Tick += new System.EventHandler(this.StartTimer_Tick);
            // 
            // IconTray
            // 
            this.IconTray.BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.IconTray.BalloonTipText = "Written by Steven De Toni 2020\r\nDouble Click System Tray icon to auto type text f" +
    "rom the Clipboard into an input field.\r\n";
            this.IconTray.BalloonTipTitle = "Auto Typer";
            this.IconTray.ContextMenuStrip = this.PopMenu;
            this.IconTray.Icon = ((System.Drawing.Icon)(resources.GetObject("IconTray.Icon")));
            this.IconTray.Text = "Auto Typer for Vault";
            this.IconTray.Visible = true;
            this.IconTray.DoubleClick += new System.EventHandler(this.IconTray_DoubleClick);
            // 
            // PopMenu
            // 
            this.PopMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ConfigMenuItem,
            this.SendClipboardTextMenuItem,
            this.AboutMenuItem,
            this.ExitAppMenuItem});
            this.PopMenu.Name = "PopMenu";
            this.PopMenu.Size = new System.Drawing.Size(207, 92);
            // 
            // ConfigMenuItem
            // 
            this.ConfigMenuItem.Name = "ConfigMenuItem";
            this.ConfigMenuItem.Size = new System.Drawing.Size(206, 22);
            this.ConfigMenuItem.Text = "Config/Text to Auto Type";
            this.ConfigMenuItem.Click += new System.EventHandler(this.ConfigMenuItem_Click);
            // 
            // SendClipboardTextMenuItem
            // 
            this.SendClipboardTextMenuItem.Name = "SendClipboardTextMenuItem";
            this.SendClipboardTextMenuItem.Size = new System.Drawing.Size(206, 22);
            this.SendClipboardTextMenuItem.Text = "Auto Type Clipboard Text";
            this.SendClipboardTextMenuItem.Click += new System.EventHandler(this.IconTray_DoubleClick);
            // 
            // AboutMenuItem
            // 
            this.AboutMenuItem.Name = "AboutMenuItem";
            this.AboutMenuItem.Size = new System.Drawing.Size(206, 22);
            this.AboutMenuItem.Text = "About";
            this.AboutMenuItem.Click += new System.EventHandler(this.AboutMenuItem_Click);
            // 
            // ExitAppMenuItem
            // 
            this.ExitAppMenuItem.Name = "ExitAppMenuItem";
            this.ExitAppMenuItem.Size = new System.Drawing.Size(206, 22);
            this.ExitAppMenuItem.Text = "Exit App";
            this.ExitAppMenuItem.Click += new System.EventHandler(this.ExitAppMenuItem_Click);
            // 
            // panel1
            // 
            this.panel1.AutoSize = true;
            this.panel1.Controls.Add(this.TurboTypeChk);
            this.panel1.Controls.Add(this.TopPos);
            this.panel1.Controls.Add(this.AutoTypeClipboardBut);
            this.panel1.Controls.Add(this.ProgBar);
            this.panel1.Controls.Add(this.DelayStartSecsNum);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.AutoTypeTextBut);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.DelayCharsSendNum);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(404, 94);
            this.panel1.TabIndex = 8;
            // 
            // TurboTypeChk
            // 
            this.TurboTypeChk.AutoSize = true;
            this.TurboTypeChk.Location = new System.Drawing.Point(266, 70);
            this.TurboTypeChk.Name = "TurboTypeChk";
            this.TurboTypeChk.Size = new System.Drawing.Size(129, 17);
            this.TurboTypeChk.TabIndex = 9;
            this.TurboTypeChk.Text = "Enable TURBO mode";
            this.TurboTypeChk.UseVisualStyleBackColor = true;
            this.TurboTypeChk.CheckedChanged += new System.EventHandler(this.TurboTyperChkBox_Changed);
            // 
            // TopPos
            // 
            this.TopPos.AutoSize = true;
            this.TopPos.Location = new System.Drawing.Point(358, 28);
            this.TopPos.Name = "TopPos";
            this.TopPos.Size = new System.Drawing.Size(22, 13);
            this.TopPos.TabIndex = 8;
            this.TopPos.Text = "top";
            this.TopPos.Visible = false;
            // 
            // AutoTypeClipboardBut
            // 
            this.AutoTypeClipboardBut.Location = new System.Drawing.Point(136, 66);
            this.AutoTypeClipboardBut.Name = "AutoTypeClipboardBut";
            this.AutoTypeClipboardBut.Size = new System.Drawing.Size(125, 23);
            this.AutoTypeClipboardBut.TabIndex = 7;
            this.AutoTypeClipboardBut.Text = "Auto Type Clipboard Text";
            this.AutoTypeClipboardBut.UseVisualStyleBackColor = true;
            this.AutoTypeClipboardBut.Click += new System.EventHandler(this.AutoTypeClipboardBut_Click);
            // 
            // ProgBar
            // 
            this.ProgBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ProgBar.Location = new System.Drawing.Point(1, 0);
            this.ProgBar.MarqueeAnimationSpeed = 16;
            this.ProgBar.Name = "ProgBar";
            this.ProgBar.Size = new System.Drawing.Size(403, 19);
            this.ProgBar.Step = 1;
            this.ProgBar.TabIndex = 6;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.TextBuffer, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(410, 229);
            this.tableLayoutPanel1.TabIndex = 9;
            // 
            // ProgTimer
            // 
            this.ProgTimer.Interval = 200;
            this.ProgTimer.Tick += new System.EventHandler(this.ProgTimer_Tick);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(410, 229);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(420, 250);
            this.Name = "MainForm";
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Auto Typer";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.MainForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.DelayStartSecsNum)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DelayCharsSendNum)).EndInit();
            this.PopMenu.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button AutoTypeTextBut;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown DelayStartSecsNum;
        private System.Windows.Forms.NumericUpDown DelayCharsSendNum;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Timer TxtTimer;
        private System.Windows.Forms.TextBox TextBuffer;
        private System.Windows.Forms.Timer StartTimer;
        private System.Windows.Forms.NotifyIcon IconTray;
        private System.Windows.Forms.ContextMenuStrip PopMenu;
        private System.Windows.Forms.ToolStripMenuItem ConfigMenuItem;
        private System.Windows.Forms.ToolStripMenuItem SendClipboardTextMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ExitAppMenuItem;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button AutoTypeClipboardBut;
        private System.Windows.Forms.ProgressBar ProgBar;
        private System.Windows.Forms.Timer ProgTimer;
        private System.Windows.Forms.Label TopPos;
        private System.Windows.Forms.ToolStripMenuItem AboutMenuItem;
        private System.Windows.Forms.CheckBox TurboTypeChk;
    }
}

