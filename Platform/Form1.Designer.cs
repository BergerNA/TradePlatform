namespace Platform
{
    partial class Platform
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.btClasterWindow = new System.Windows.Forms.Button();
            this.toolStripContainer1 = new System.Windows.Forms.ToolStripContainer();
            this.cbTf = new System.Windows.Forms.ComboBox();
            this.lbTf = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.tbIgnoreTickVol = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tbVolFilterMax = new System.Windows.Forms.TextBox();
            this.tbVolFilterMin = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cbSecurity = new System.Windows.Forms.ComboBox();
            this.menuMain = new System.Windows.Forms.MenuStrip();
            this.btComponent = new System.Windows.Forms.ToolStripMenuItem();
            this.btClusterProfile = new System.Windows.Forms.ToolStripMenuItem();
            this.btDom = new System.Windows.Forms.ToolStripMenuItem();
            this.btApply = new System.Windows.Forms.ToolStripMenuItem();
            this.btClose = new System.Windows.Forms.ToolStripMenuItem();
            this.btSetup = new System.Windows.Forms.ToolStripMenuItem();
            this.btConnection = new System.Windows.Forms.ToolStripMenuItem();
            this.btSecurity = new System.Windows.Forms.ToolStripMenuItem();
            this.lbTime = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.lbMain = new System.Windows.Forms.Label();
            this.ChartGl = new Tao.Platform.Windows.SimpleOpenGlControl();
            this.lbConnect = new System.Windows.Forms.Label();
            this.toolStripContainer1.ContentPanel.SuspendLayout();
            this.toolStripContainer1.TopToolStripPanel.SuspendLayout();
            this.toolStripContainer1.SuspendLayout();
            this.menuMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // btClasterWindow
            // 
            this.btClasterWindow.Location = new System.Drawing.Point(862, 372);
            this.btClasterWindow.Name = "btClasterWindow";
            this.btClasterWindow.Size = new System.Drawing.Size(118, 23);
            this.btClasterWindow.TabIndex = 0;
            this.btClasterWindow.Text = "New Window";
            this.btClasterWindow.UseVisualStyleBackColor = true;
            this.btClasterWindow.Click += new System.EventHandler(this.btClasterWindow_Click);
            // 
            // toolStripContainer1
            // 
            this.toolStripContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            // 
            // toolStripContainer1.ContentPanel
            // 
            this.toolStripContainer1.ContentPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.toolStripContainer1.ContentPanel.Controls.Add(this.cbTf);
            this.toolStripContainer1.ContentPanel.Controls.Add(this.lbTf);
            this.toolStripContainer1.ContentPanel.Controls.Add(this.label3);
            this.toolStripContainer1.ContentPanel.Controls.Add(this.tbIgnoreTickVol);
            this.toolStripContainer1.ContentPanel.Controls.Add(this.label2);
            this.toolStripContainer1.ContentPanel.Controls.Add(this.tbVolFilterMax);
            this.toolStripContainer1.ContentPanel.Controls.Add(this.tbVolFilterMin);
            this.toolStripContainer1.ContentPanel.Controls.Add(this.label1);
            this.toolStripContainer1.ContentPanel.Controls.Add(this.cbSecurity);
            this.toolStripContainer1.ContentPanel.Size = new System.Drawing.Size(1007, 26);
            this.toolStripContainer1.Location = new System.Drawing.Point(0, 30);
            this.toolStripContainer1.Name = "toolStripContainer1";
            this.toolStripContainer1.Size = new System.Drawing.Size(1007, 54);
            this.toolStripContainer1.TabIndex = 5;
            this.toolStripContainer1.Text = "toolStripContainer1";
            // 
            // toolStripContainer1.TopToolStripPanel
            // 
            this.toolStripContainer1.TopToolStripPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.toolStripContainer1.TopToolStripPanel.Controls.Add(this.menuMain);
            this.toolStripContainer1.TopToolStripPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.toolStripContainer1_TopToolStripPanel_Paint);
            // 
            // cbTf
            // 
            this.cbTf.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.cbTf.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.cbTf.FormattingEnabled = true;
            this.cbTf.Items.AddRange(new object[] {
            "5 SEC",
            "10 SEC",
            "15 SEC",
            "20 SEC",
            "30 SEC",
            "1 MIN",
            "2 MIN",
            "3 MIN",
            "4 MIN",
            "5 MIN",
            "6 MIN",
            "10 MIN",
            "15 MIN",
            "20 MIN",
            "30 MIN",
            "1 HOUR",
            "2 HOUR",
            "4 HOUR",
            "1 DAY"});
            this.cbTf.Location = new System.Drawing.Point(534, 2);
            this.cbTf.Name = "cbTf";
            this.cbTf.Size = new System.Drawing.Size(90, 21);
            this.cbTf.TabIndex = 8;
            this.cbTf.Text = "1 MIN";
            // 
            // lbTf
            // 
            this.lbTf.AutoSize = true;
            this.lbTf.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lbTf.ForeColor = System.Drawing.Color.Black;
            this.lbTf.Location = new System.Drawing.Point(500, 5);
            this.lbTf.Name = "lbTf";
            this.lbTf.Size = new System.Drawing.Size(28, 16);
            this.lbTf.TabIndex = 7;
            this.lbTf.Text = "TF:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label3.ForeColor = System.Drawing.Color.Black;
            this.label3.Location = new System.Drawing.Point(361, 5);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(64, 16);
            this.label3.TabIndex = 6;
            this.label3.Text = "Ignor tick:";
            // 
            // tbIgnoreTickVol
            // 
            this.tbIgnoreTickVol.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.tbIgnoreTickVol.Location = new System.Drawing.Point(430, 4);
            this.tbIgnoreTickVol.Name = "tbIgnoreTickVol";
            this.tbIgnoreTickVol.Size = new System.Drawing.Size(64, 20);
            this.tbIgnoreTickVol.TabIndex = 5;
            this.tbIgnoreTickVol.Text = "1";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.Location = new System.Drawing.Point(254, 5);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(19, 16);
            this.label2.TabIndex = 4;
            this.label2.Text = "to";
            // 
            // tbVolFilterMax
            // 
            this.tbVolFilterMax.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.tbVolFilterMax.Location = new System.Drawing.Point(279, 4);
            this.tbVolFilterMax.Name = "tbVolFilterMax";
            this.tbVolFilterMax.Size = new System.Drawing.Size(64, 20);
            this.tbVolFilterMax.TabIndex = 3;
            this.tbVolFilterMax.Text = "500";
            // 
            // tbVolFilterMin
            // 
            this.tbVolFilterMin.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.tbVolFilterMin.Location = new System.Drawing.Point(178, 3);
            this.tbVolFilterMin.Name = "tbVolFilterMin";
            this.tbVolFilterMin.Size = new System.Drawing.Size(64, 20);
            this.tbVolFilterMin.TabIndex = 2;
            this.tbVolFilterMin.Text = "300";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(108, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(63, 16);
            this.label1.TabIndex = 1;
            this.label1.Text = "Vol Filter:";
            // 
            // cbSecurity
            // 
            this.cbSecurity.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.cbSecurity.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.cbSecurity.FormattingEnabled = true;
            this.cbSecurity.Items.AddRange(new object[] {
            "RIZ6",
            "SiZ6",
            "SRZ6",
            "GZZ6"});
            this.cbSecurity.Location = new System.Drawing.Point(12, 3);
            this.cbSecurity.Name = "cbSecurity";
            this.cbSecurity.Size = new System.Drawing.Size(90, 21);
            this.cbSecurity.TabIndex = 0;
            this.cbSecurity.Text = "RIZ6";
            // 
            // menuMain
            // 
            this.menuMain.BackColor = System.Drawing.Color.Transparent;
            this.menuMain.Dock = System.Windows.Forms.DockStyle.None;
            this.menuMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btComponent,
            this.btDom,
            this.btApply,
            this.btClose,
            this.btSetup});
            this.menuMain.Location = new System.Drawing.Point(0, 0);
            this.menuMain.Name = "menuMain";
            this.menuMain.Size = new System.Drawing.Size(1007, 28);
            this.menuMain.TabIndex = 0;
            this.menuMain.Text = "mainMenu";
            // 
            // btComponent
            // 
            this.btComponent.BackColor = System.Drawing.Color.Transparent;
            this.btComponent.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btClusterProfile});
            this.btComponent.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btComponent.Name = "btComponent";
            this.btComponent.Size = new System.Drawing.Size(99, 24);
            this.btComponent.Text = "Component";
            // 
            // btClusterProfile
            // 
            this.btClusterProfile.BackColor = System.Drawing.Color.White;
            this.btClusterProfile.Name = "btClusterProfile";
            this.btClusterProfile.Size = new System.Drawing.Size(171, 24);
            this.btClusterProfile.Text = "Cluster profile";
            // 
            // btDom
            // 
            this.btDom.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btDom.Name = "btDom";
            this.btDom.Size = new System.Drawing.Size(54, 24);
            this.btDom.Text = "Dom";
            // 
            // btApply
            // 
            this.btApply.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.btApply.Font = new System.Drawing.Font("MS Reference Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btApply.ForeColor = System.Drawing.Color.White;
            this.btApply.Name = "btApply";
            this.btApply.Size = new System.Drawing.Size(66, 24);
            this.btApply.Text = "Apply";
            // 
            // btClose
            // 
            this.btClose.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.btClose.Font = new System.Drawing.Font("MS Reference Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btClose.ForeColor = System.Drawing.Color.White;
            this.btClose.Name = "btClose";
            this.btClose.Size = new System.Drawing.Size(66, 24);
            this.btClose.Text = "Close";
            // 
            // btSetup
            // 
            this.btSetup.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.btSetup.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btConnection,
            this.btSecurity});
            this.btSetup.Font = new System.Drawing.Font("MS Reference Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btSetup.ForeColor = System.Drawing.Color.White;
            this.btSetup.Name = "btSetup";
            this.btSetup.Size = new System.Drawing.Size(68, 24);
            this.btSetup.Text = "Setup";
            // 
            // btConnection
            // 
            this.btConnection.Name = "btConnection";
            this.btConnection.Size = new System.Drawing.Size(169, 24);
            this.btConnection.Text = "Connection";
            this.btConnection.Click += new System.EventHandler(this.btConnection_Click);
            // 
            // btSecurity
            // 
            this.btSecurity.Name = "btSecurity";
            this.btSecurity.Size = new System.Drawing.Size(169, 24);
            this.btSecurity.Text = "Paper";
            this.btSecurity.Click += new System.EventHandler(this.btSecurity_Click);
            // 
            // lbTime
            // 
            this.lbTime.AutoSize = true;
            this.lbTime.BackColor = System.Drawing.Color.Transparent;
            this.lbTime.Font = new System.Drawing.Font("MS Reference Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lbTime.ForeColor = System.Drawing.Color.White;
            this.lbTime.Location = new System.Drawing.Point(8, 3);
            this.lbTime.Name = "lbTime";
            this.lbTime.Size = new System.Drawing.Size(57, 24);
            this.lbTime.TabIndex = 6;
            this.lbTime.Text = "Time";
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // lbMain
            // 
            this.lbMain.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbMain.AutoSize = true;
            this.lbMain.BackColor = System.Drawing.Color.Transparent;
            this.lbMain.Font = new System.Drawing.Font("MS Reference Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lbMain.ForeColor = System.Drawing.Color.White;
            this.lbMain.Location = new System.Drawing.Point(440, 3);
            this.lbMain.Name = "lbMain";
            this.lbMain.Size = new System.Drawing.Size(136, 24);
            this.lbMain.TabIndex = 7;
            this.lbMain.Text = "Main window";
            // 
            // ChartGl
            // 
            this.ChartGl.AccumBits = ((byte)(0));
            this.ChartGl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ChartGl.AutoCheckErrors = false;
            this.ChartGl.AutoFinish = false;
            this.ChartGl.AutoMakeCurrent = true;
            this.ChartGl.AutoSwapBuffers = true;
            this.ChartGl.BackColor = System.Drawing.Color.Black;
            this.ChartGl.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ChartGl.ColorBits = ((byte)(32));
            this.ChartGl.DepthBits = ((byte)(16));
            this.ChartGl.Location = new System.Drawing.Point(1, 110);
            this.ChartGl.Name = "ChartGl";
            this.ChartGl.Size = new System.Drawing.Size(819, 636);
            this.ChartGl.StencilBits = ((byte)(0));
            this.ChartGl.TabIndex = 8;
            // 
            // lbConnect
            // 
            this.lbConnect.AutoSize = true;
            this.lbConnect.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lbConnect.ForeColor = System.Drawing.Color.Red;
            this.lbConnect.Location = new System.Drawing.Point(8, 87);
            this.lbConnect.Name = "lbConnect";
            this.lbConnect.Size = new System.Drawing.Size(69, 20);
            this.lbConnect.TabIndex = 9;
            this.lbConnect.Text = "Connect";
            // 
            // Platform
            // 
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(1008, 747);
            this.Controls.Add(this.lbConnect);
            this.Controls.Add(this.ChartGl);
            this.Controls.Add(this.lbMain);
            this.Controls.Add(this.lbTime);
            this.Controls.Add(this.toolStripContainer1);
            this.Controls.Add(this.btClasterWindow);
            this.Name = "Platform";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Platform_FormClosing);
            this.Load += new System.EventHandler(this.Platform_Load);
            this.toolStripContainer1.ContentPanel.ResumeLayout(false);
            this.toolStripContainer1.ContentPanel.PerformLayout();
            this.toolStripContainer1.TopToolStripPanel.ResumeLayout(false);
            this.toolStripContainer1.TopToolStripPanel.PerformLayout();
            this.toolStripContainer1.ResumeLayout(false);
            this.toolStripContainer1.PerformLayout();
            this.menuMain.ResumeLayout(false);
            this.menuMain.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        private System.Windows.Forms.Button btClasterWindow;
        private System.Windows.Forms.ToolStripContainer toolStripContainer1;
        private System.Windows.Forms.ComboBox cbTf;
        private System.Windows.Forms.Label lbTf;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbIgnoreTickVol;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbVolFilterMax;
        private System.Windows.Forms.TextBox tbVolFilterMin;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cbSecurity;
        private System.Windows.Forms.MenuStrip menuMain;
        private System.Windows.Forms.ToolStripMenuItem btComponent;
        private System.Windows.Forms.ToolStripMenuItem btDom;
        private System.Windows.Forms.ToolStripMenuItem btApply;
        private System.Windows.Forms.ToolStripMenuItem btClose;
        private System.Windows.Forms.ToolStripMenuItem btSetup;
        public System.Windows.Forms.Label lbTime;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.ToolStripMenuItem btClusterProfile;
        private System.Windows.Forms.ToolStripMenuItem btConnection;
        private System.Windows.Forms.ToolStripMenuItem btSecurity;
        public System.Windows.Forms.Label lbMain;
        private Tao.Platform.Windows.SimpleOpenGlControl ChartGl;
        private System.Windows.Forms.Label lbConnect;
    }
}

