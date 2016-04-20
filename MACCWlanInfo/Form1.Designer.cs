namespace MACCWlanInfo
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.tbResultOut = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnStopAuto = new System.Windows.Forms.Button();
            this.btnStartAuto = new System.Windows.Forms.Button();
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.关于ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.退出ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.label2 = new System.Windows.Forms.Label();
            this.tbGetDataInterval = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.cbMinimize = new System.Windows.Forms.CheckBox();
            this.cbAutoGet = new System.Windows.Forms.CheckBox();
            this.cbAutoRun = new System.Windows.Forms.CheckBox();
            this.groupBox2.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // tbResultOut
            // 
            this.tbResultOut.BackColor = System.Drawing.Color.Azure;
            this.tbResultOut.Location = new System.Drawing.Point(14, 15);
            this.tbResultOut.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tbResultOut.Multiline = true;
            this.tbResultOut.Name = "tbResultOut";
            this.tbResultOut.ReadOnly = true;
            this.tbResultOut.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tbResultOut.Size = new System.Drawing.Size(643, 335);
            this.tbResultOut.TabIndex = 1;
            this.tbResultOut.TabStop = false;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnStopAuto);
            this.groupBox2.Controls.Add(this.btnStartAuto);
            this.groupBox2.Location = new System.Drawing.Point(670, 15);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox2.Size = new System.Drawing.Size(185, 134);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "自动抓取";
            // 
            // btnStopAuto
            // 
            this.btnStopAuto.Enabled = false;
            this.btnStopAuto.Location = new System.Drawing.Point(25, 80);
            this.btnStopAuto.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnStopAuto.Name = "btnStopAuto";
            this.btnStopAuto.Size = new System.Drawing.Size(135, 37);
            this.btnStopAuto.TabIndex = 0;
            this.btnStopAuto.Text = "停止";
            this.btnStopAuto.UseVisualStyleBackColor = true;
            this.btnStopAuto.Click += new System.EventHandler(this.btnStopAuto_Click);
            // 
            // btnStartAuto
            // 
            this.btnStartAuto.Location = new System.Drawing.Point(25, 34);
            this.btnStartAuto.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnStartAuto.Name = "btnStartAuto";
            this.btnStartAuto.Size = new System.Drawing.Size(135, 37);
            this.btnStartAuto.TabIndex = 0;
            this.btnStartAuto.Text = "开始";
            this.btnStartAuto.UseVisualStyleBackColor = true;
            this.btnStartAuto.Click += new System.EventHandler(this.btnStartAuto_Click);
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.notifyIcon1.BalloonTipText = "d";
            this.notifyIcon1.BalloonTipTitle = "t";
            this.notifyIcon1.ContextMenuStrip = this.contextMenuStrip1;
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Text = "辰宜数据抓取系统";
            this.notifyIcon1.Visible = true;
            this.notifyIcon1.Click += new System.EventHandler(this.notifyIcon1_Click);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.关于ToolStripMenuItem,
            this.退出ToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(101, 48);
            // 
            // 关于ToolStripMenuItem
            // 
            this.关于ToolStripMenuItem.Name = "关于ToolStripMenuItem";
            this.关于ToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.关于ToolStripMenuItem.Text = "关于";
            this.关于ToolStripMenuItem.Click += new System.EventHandler(this.关于ToolStripMenuItem_Click);
            // 
            // 退出ToolStripMenuItem
            // 
            this.退出ToolStripMenuItem.Name = "退出ToolStripMenuItem";
            this.退出ToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.退出ToolStripMenuItem.Text = "退出";
            this.退出ToolStripMenuItem.Click += new System.EventHandler(this.退出ToolStripMenuItem_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(21, 34);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(107, 20);
            this.label2.TabIndex = 1;
            this.label2.Text = "抓取时间间隔：";
            // 
            // tbGetDataInterval
            // 
            this.tbGetDataInterval.Location = new System.Drawing.Point(23, 65);
            this.tbGetDataInterval.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tbGetDataInterval.Name = "tbGetDataInterval";
            this.tbGetDataInterval.Size = new System.Drawing.Size(94, 25);
            this.tbGetDataInterval.TabIndex = 2;
            this.tbGetDataInterval.Text = "10";
            this.tbGetDataInterval.TextChanged += new System.EventHandler(this.tbGetDataInterval_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(123, 68);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(37, 20);
            this.label3.TabIndex = 3;
            this.label3.Text = "分钟";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.cbMinimize);
            this.groupBox3.Controls.Add(this.cbAutoGet);
            this.groupBox3.Controls.Add(this.cbAutoRun);
            this.groupBox3.Controls.Add(this.tbGetDataInterval);
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Controls.Add(this.label2);
            this.groupBox3.Location = new System.Drawing.Point(670, 157);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox3.Size = new System.Drawing.Size(185, 228);
            this.groupBox3.TabIndex = 10;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "设置";
            // 
            // cbMinimize
            // 
            this.cbMinimize.AutoSize = true;
            this.cbMinimize.Location = new System.Drawing.Point(23, 179);
            this.cbMinimize.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cbMinimize.Name = "cbMinimize";
            this.cbMinimize.Size = new System.Drawing.Size(112, 24);
            this.cbMinimize.TabIndex = 6;
            this.cbMinimize.Text = "启动后最小化";
            this.cbMinimize.UseVisualStyleBackColor = true;
            this.cbMinimize.CheckedChanged += new System.EventHandler(this.cbMinimize_CheckedChanged);
            // 
            // cbAutoGet
            // 
            this.cbAutoGet.AutoSize = true;
            this.cbAutoGet.Location = new System.Drawing.Point(23, 147);
            this.cbAutoGet.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cbAutoGet.Name = "cbAutoGet";
            this.cbAutoGet.Size = new System.Drawing.Size(112, 24);
            this.cbAutoGet.TabIndex = 5;
            this.cbAutoGet.Text = "自动开启抓取";
            this.cbAutoGet.UseVisualStyleBackColor = true;
            this.cbAutoGet.CheckedChanged += new System.EventHandler(this.cbAutoGet_CheckedChanged);
            // 
            // cbAutoRun
            // 
            this.cbAutoRun.AutoSize = true;
            this.cbAutoRun.Location = new System.Drawing.Point(23, 115);
            this.cbAutoRun.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cbAutoRun.Name = "cbAutoRun";
            this.cbAutoRun.Size = new System.Drawing.Size(84, 24);
            this.cbAutoRun.TabIndex = 4;
            this.cbAutoRun.Text = "开机运行";
            this.cbAutoRun.UseVisualStyleBackColor = true;
            this.cbAutoRun.CheckedChanged += new System.EventHandler(this.cbAutoRun_CheckedChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 19F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LightBlue;
            this.ClientSize = new System.Drawing.Size(868, 397);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.tbResultOut);
            this.Font = new System.Drawing.Font("Microsoft YaHei", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "辰宜数据抓取系统";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Shown += new System.EventHandler(this.Form1_Shown);
            this.SizeChanged += new System.EventHandler(this.Form1_SizeChanged);
            this.groupBox2.ResumeLayout(false);
            this.contextMenuStrip1.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbResultOut;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnStopAuto;
        private System.Windows.Forms.Button btnStartAuto;
        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 退出ToolStripMenuItem;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbGetDataInterval;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.CheckBox cbAutoGet;
        private System.Windows.Forms.CheckBox cbAutoRun;
        private System.Windows.Forms.CheckBox cbMinimize;
        private System.Windows.Forms.ToolStripMenuItem 关于ToolStripMenuItem;
    }
}

