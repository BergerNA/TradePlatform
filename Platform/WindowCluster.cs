using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tao.FreeGlut;
using Tao.OpenGl;
using Timer = System.Windows.Forms.Timer;

// Окно отображающее кластерный график
// TEST: Для тестов, отработка лучших сценариев

namespace Platform
{
    class WindowCluster : Form
    {
        #region Глобальные переменные

        private DataSeries Sec;
        private DrawSeries ds;
        private FontPlot fontForPlot;
        private Button btReal;
        private Tao.Platform.Windows.SimpleOpenGlControl ChartGl;
        private Button Load;
        private WindowGL Chart;
        private Tick Ticks;
        private PaintPoint BarsDraw;
        public Timer timer1;                    // Интервал перерисовки
        private System.ComponentModel.IContainer components;
        private Timer timer2;
        public Label lbTime;
        private ToolStripContainer toolStripContainer1;
        private TextBox tbVolFilterMin;
        private Label label1;
        private ComboBox cbSecurity;
        private MenuStrip menuMain;
        private ToolStripMenuItem fdgfdgToolStripMenuItem;
        private ToolStripMenuItem btDom;
        private ToolStripMenuItem btClose;
        private ToolStripMenuItem btSetup;
        private Label label2;
        private TextBox tbVolFilterMax;
        private Label label3;
        private TextBox tbIgnoreTickVol;
        private Label lbReal;
        private Label lbProfit;
        private ToolStripMenuItem btApply;
        public int volFilter = 300;
        private Order fg;
        private ConnectorQuik connector;

        #endregion

        public int minVol;
        public int maxVol;
        private int prevIgnorTickVol;
        public int ignorTickVol;
        public string secName;
        private string prevSecName;
        private bool real = false;
        private ConcurrentQueue<string> dealQueue = new ConcurrentQueue<string>();
        private ComboBox cbTf;
        private Label lbTf;
        private TimeFrame1 TF = new TimeFrame1();
        private TimeFrame1 TFPrev = new TimeFrame1();
        

        public WindowCluster(ConnectorQuik connect)
        {

            InitializeComponent();
            timer1 = new System.Windows.Forms.Timer(this.components);
            timer1.Interval = 50;
            timer1.Tick += new System.EventHandler(this.timer1_Tick);

            ChartGl.InitializeContexts();
            
            Chart = new WindowGL(ChartGl, this, timer1, real);
            Chart.Load();

            minVol = Convert.ToInt32(tbVolFilterMin.Text);
            maxVol = Convert.ToInt32(tbVolFilterMax.Text);
            ignorTickVol = Convert.ToInt32(tbIgnoreTickVol.Text);
            secName = cbSecurity.Text;
           // prevSecName = secName;
            prevIgnorTickVol = ignorTickVol;
            TF.frame = TfRange.MIN;
            TF.digit = 1;
            TFPrev = TF;

            connector = connect;
            connect.Connect();
            connector.Event_GetTick += Connector_Event_GetTick;
            connect.Event_GetTicks += Connect_Event_GetTicks;
            //   fg.GetControl().Anchor = AnchorStyles.Top;
            // Sec = new DataSeries("RTS-12.15", 10, Exchange.SPBFUT, TimeFrame.Hour, chartStyle.Cluster, ignorTickVol);
            //  WindowGL.Sec = Sec;
            //   if (Form1.Quik.connected)
            //  {

            //  }
            new Thread(() =>
            {
                LoadTick();
            }).Start();
        }

        private void Connect_Event_GetTicks(string str)
        {
            if (str.Contains(secName))
            {
                dealQueue.Enqueue(str);
            }
        }

        private bool run = true;

        // Отслеживание поступления новых данных, их обработка и отрисовка
        private void LoadTick()
        {
            Dictionary<string, string> DictionaryParam = new Dictionary<string, string>();
            Tick tic = new Tick();
            while (run)
            {
                if (dealQueue.Count > 0) // В очереди есть элементы?
                {
                    for (int j = 0; j < dealQueue.Count; j++) // Извлекаем данные из очереди
                    {
                        string str = "";
                        int timech = 0;
                        dealQueue.TryDequeue(out str);
                        DictionaryParam.Clear();
                        if (str != null)
                        {
                            if (str.IndexOf("NUM") == 0)
                            {
                                string[] Arr = str.Split(';');
                                for (int i = 0; i < Arr.Length; i++)
                                {
                                    string[] KeyValue = Arr[i].Split('=');
                                    DictionaryParam.Add(KeyValue[0], KeyValue[1]);
                                }
                                tic.priceTick = Convert.ToDouble(DictionaryParam["PRICE"]);
                                tic.dateTimeTick = Convert.ToDateTime(DictionaryParam["DATA"]);
                                tic.volumeTick = Convert.ToInt32(DictionaryParam["QTY"]);
                                tic.paperCode = DictionaryParam["SECCODE"];
                                DateTime t = DateTime.Now;
                                switch (TF.frame)
                                {
                                        case TfRange.SEC:
                                        timech = (tic.dateTimeTick.Second/TF.digit)*TF.digit;
                                        t = new DateTime(tic.dateTimeTick.Year, tic.dateTimeTick.Month, tic.dateTimeTick.Day,
                                                       tic.dateTimeTick.Hour, tic.dateTimeTick.Minute, timech);
                                        break;
                                        case TfRange.MIN:
                                        timech = (tic.dateTimeTick.Minute / TF.digit) * TF.digit;
                                        t = new DateTime(tic.dateTimeTick.Year, tic.dateTimeTick.Month, tic.dateTimeTick.Day, tic.dateTimeTick.Hour, timech, 0);
                                        break;
                                        case TfRange.HOUR:
                                        timech = (tic.dateTimeTick.Hour / TF.digit) * TF.digit;
                                        t = new DateTime(tic.dateTimeTick.Year, tic.dateTimeTick.Month, tic.dateTimeTick.Day, timech, 0, 0);
                                        break;
                                        case TfRange.DAY:
                                        timech = (tic.dateTimeTick.Day / TF.digit) * TF.digit;
                                        t = new DateTime(tic.dateTimeTick.Year, tic.dateTimeTick.Month, timech, 0, 0, 0);
                                        break;
                                }
                                tic.dateTimeTick = t;
                                Connector_Event_GetTick(tic);
                            }
                        }
                    }
                }
                Thread.Sleep(1);
            }
        }


        // Обработка события получения нового тика
        private void Connector_Event_GetTick(Tick tick)
        {
            if (Sec != null)                    // Проверка изменения инструмента в поле выбора
            {
                if (Sec.Load && tick.paperCode == secName)
                {
                    while (Chart.PlotSer) {}    // Пока коллекция занята отрисовкой, ждем

                    add = true;                 // Сообщаем о добавлении данных в коллекцию
                    Sec.AddTick(tick);
                    if (ds.Load)
                        ds.AddRealTick(Sec, tick);
                    add = false;                // Добавление завершено
                    //  WindowGL.BarsDraw.AddReal(Sec, tk);
                }
            }
            else if (tick.paperCode == secName) 
            {
                if (secName == "SiZ6" | secName == "SRZ6")
                    Sec = new DataSeries(secName, 1, Exchange.SPBFUT, TimeFrame.M1, chartStyle.Cluster, ignorTickVol);
                else Sec = new DataSeries(secName, 10, Exchange.SPBFUT, TimeFrame.M1, chartStyle.Cluster, ignorTickVol);
               
                add = true;
                DateTime t = new DateTime(tick.dateTimeTick.Year, tick.dateTimeTick.Month, tick.dateTimeTick.Day,
                    tick.dateTimeTick.Hour, tick.dateTimeTick.Minute, 0);
                tick.dateTimeTick = t;
                Sec.AddTick(tick);
                if (ds == null) // При отсутствии данных для отрисовки, создать новую DrawSeries
                {
                    if (secName == "SiZ6" | secName == "SRZ6")
                        ds = new DrawSeries(Sec.Bars.ElementAt(0).Value.Open,1,minVol,maxVol);
                    else ds = new DrawSeries(Sec.Bars.ElementAt(0).Value.Open, 10, minVol, maxVol);
                    ds.Add(Sec);
                    Chart.ReloadSeries(Sec, ds, Sec.Bars.ElementAt(0).Value.Open);
                    if (ds.Load)
                    {
                        ds.AddRealTick(Sec, tick);
                    }
                    add = false;
                }
                Chart.LookSet(); // Отобразить новые данные
            }
        }

        private void WindowCluster_Load(object sender, EventArgs e)
        {
               Chart.Load();
        }
        
        #region Инициализация окна
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.btReal = new System.Windows.Forms.Button();
            this.ChartGl = new Tao.Platform.Windows.SimpleOpenGlControl();
            this.Load = new System.Windows.Forms.Button();
            this.timer2 = new System.Windows.Forms.Timer(this.components);
            this.lbTime = new System.Windows.Forms.Label();
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
            this.fdgfdgToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.btDom = new System.Windows.Forms.ToolStripMenuItem();
            this.btApply = new System.Windows.Forms.ToolStripMenuItem();
            this.btClose = new System.Windows.Forms.ToolStripMenuItem();
            this.btSetup = new System.Windows.Forms.ToolStripMenuItem();
            this.lbReal = new System.Windows.Forms.Label();
            this.lbProfit = new System.Windows.Forms.Label();
            this.toolStripContainer1.ContentPanel.SuspendLayout();
            this.toolStripContainer1.TopToolStripPanel.SuspendLayout();
            this.toolStripContainer1.SuspendLayout();
            this.menuMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // btReal
            // 
            this.btReal.Location = new System.Drawing.Point(751, 74);
            this.btReal.Name = "btReal";
            this.btReal.Size = new System.Drawing.Size(75, 23);
            this.btReal.TabIndex = 0;
            this.btReal.Text = "Real";
            this.btReal.UseVisualStyleBackColor = true;
            this.btReal.Click += new System.EventHandler(this.btReal_Click);
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
            this.ChartGl.Location = new System.Drawing.Point(1, 93);
            this.ChartGl.Name = "ChartGl";
            this.ChartGl.Size = new System.Drawing.Size(641, 506);
            this.ChartGl.StencilBits = ((byte)(0));
            this.ChartGl.TabIndex = 1;
            // 
            // Load
            // 
            this.Load.Location = new System.Drawing.Point(751, 103);
            this.Load.Name = "Load";
            this.Load.Size = new System.Drawing.Size(75, 23);
            this.Load.TabIndex = 2;
            this.Load.Text = "btLoad";
            this.Load.UseVisualStyleBackColor = true;
            this.Load.Click += new System.EventHandler(this.Load_Click);
            // 
            // timer2
            // 
            this.timer2.Enabled = true;
            this.timer2.Interval = 1000;
            this.timer2.Tick += new System.EventHandler(this.timer2_Tick);
            // 
            // lbTime
            // 
            this.lbTime.AutoSize = true;
            this.lbTime.BackColor = System.Drawing.Color.Transparent;
            this.lbTime.Font = new System.Drawing.Font("MS Reference Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lbTime.ForeColor = System.Drawing.Color.White;
            this.lbTime.Location = new System.Drawing.Point(12, -1);
            this.lbTime.Name = "lbTime";
            this.lbTime.Size = new System.Drawing.Size(57, 24);
            this.lbTime.TabIndex = 3;
            this.lbTime.Text = "Time";
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
            this.toolStripContainer1.ContentPanel.Size = new System.Drawing.Size(860, 26);
            this.toolStripContainer1.Location = new System.Drawing.Point(0, 20);
            this.toolStripContainer1.Name = "toolStripContainer1";
            this.toolStripContainer1.Size = new System.Drawing.Size(860, 54);
            this.toolStripContainer1.TabIndex = 4;
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
            this.cbTf.SelectedValueChanged += new System.EventHandler(this.cbTf_SelectedValueChanged);
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
            this.tbIgnoreTickVol.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbIgnoreTickVol_KeyPress);
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
            this.tbVolFilterMax.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbVolFilterMax_KeyPress);
            // 
            // tbVolFilterMin
            // 
            this.tbVolFilterMin.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.tbVolFilterMin.Location = new System.Drawing.Point(178, 3);
            this.tbVolFilterMin.Name = "tbVolFilterMin";
            this.tbVolFilterMin.Size = new System.Drawing.Size(64, 20);
            this.tbVolFilterMin.TabIndex = 2;
            this.tbVolFilterMin.Text = "300";
            this.tbVolFilterMin.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbMinVol_KeyPress);
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
            this.cbSecurity.SelectedValueChanged += new System.EventHandler(this.cbSecurity_SelectedValueChanged);
            // 
            // menuMain
            // 
            this.menuMain.BackColor = System.Drawing.Color.Transparent;
            this.menuMain.Dock = System.Windows.Forms.DockStyle.None;
            this.menuMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fdgfdgToolStripMenuItem,
            this.btDom,
            this.btApply,
            this.btClose,
            this.btSetup});
            this.menuMain.Location = new System.Drawing.Point(0, 0);
            this.menuMain.Name = "menuMain";
            this.menuMain.Size = new System.Drawing.Size(860, 28);
            this.menuMain.TabIndex = 0;
            this.menuMain.Text = "mainMenu";
            // 
            // fdgfdgToolStripMenuItem
            // 
            this.fdgfdgToolStripMenuItem.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.fdgfdgToolStripMenuItem.Name = "fdgfdgToolStripMenuItem";
            this.fdgfdgToolStripMenuItem.Size = new System.Drawing.Size(50, 24);
            this.fdgfdgToolStripMenuItem.Text = "Real";
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
            this.btApply.Click += new System.EventHandler(this.btApply_Click);
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
            this.btSetup.Font = new System.Drawing.Font("MS Reference Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btSetup.ForeColor = System.Drawing.Color.White;
            this.btSetup.Name = "btSetup";
            this.btSetup.Size = new System.Drawing.Size(68, 24);
            this.btSetup.Text = "Setup";
            // 
            // lbReal
            // 
            this.lbReal.AutoSize = true;
            this.lbReal.BackColor = System.Drawing.Color.Transparent;
            this.lbReal.Font = new System.Drawing.Font("MS Reference Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lbReal.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.lbReal.Location = new System.Drawing.Point(9, 75);
            this.lbReal.Name = "lbReal";
            this.lbReal.Size = new System.Drawing.Size(35, 16);
            this.lbReal.TabIndex = 5;
            this.lbReal.Text = "Real";
            // 
            // lbProfit
            // 
            this.lbProfit.AutoSize = true;
            this.lbProfit.BackColor = System.Drawing.Color.Transparent;
            this.lbProfit.Font = new System.Drawing.Font("MS Reference Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lbProfit.ForeColor = System.Drawing.Color.White;
            this.lbProfit.Location = new System.Drawing.Point(553, 77);
            this.lbProfit.Name = "lbProfit";
            this.lbProfit.Size = new System.Drawing.Size(33, 16);
            this.lbProfit.TabIndex = 6;
            this.lbProfit.Text = "P/L:";
            // 
            // WindowCluster
            // 
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(860, 600);
            this.Controls.Add(this.lbProfit);
            this.Controls.Add(this.lbReal);
            this.Controls.Add(this.toolStripContainer1);
            this.Controls.Add(this.lbTime);
            this.Controls.Add(this.Load);
            this.Controls.Add(this.ChartGl);
            this.Controls.Add(this.btReal);
            this.DoubleBuffered = true;
            this.Name = "WindowCluster";
            this.Text = "Price Cluster";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.WindowCluster_FormClosing);
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
 
        // Тестовая кнопка, запуск обмена данными с торгового терминала
        private void btReal_Click(object sender, EventArgs e)
        {
            if (timer1.Enabled == false)
            timer1.Enabled = true;
            else
            {
                timer1.Enabled = false;
            }
            real = true;

        //    new Thread(() =>
       //     {
       //         TickGen();
       //     }).Start();
        }

        // Тестовая кнопка, загрузка тиковых данных из файла
        private void Load_Click(object sender, EventArgs e)
        {
            Sec = new DataSeries("RTS-3.15",10, Exchange.SPBFUT, TimeFrame.Hour, chartStyle.Cluster, ignorTickVol);
           // WindowGL.Sec = Sec;
            //  Rts.Bars[0].
            Ticks = new Tick();
            var lines = File.ReadAllLines("D:\\Market Data\\fut_deal.csv", System.Text.Encoding.Default);
            for (int i = 1; i < lines.Count(); i++)
            {
                string[] parts = lines[i].Split(';');
                DateTime dtNew = DateTime.Now;
                if (parts[2] == "RTS-3.15")
                {
                    string dt = parts[0] + ' ' + parts[1];
                    DateTime t = DateTime.ParseExact(dt, "dd.MM.yyyy HH:mm:ss", null);
                    //Ticks.dateTimeTick = t;
                    Ticks.priceTick = Convert.ToDouble(parts[3].Replace(".", ","));
                    Ticks.volumeTick = Convert.ToInt32(parts[4]);
                    dtNew = new DateTime(t.Year, t.Month, t.Day, t.Hour, t.Minute, 00);
                    Ticks.dateTimeTick = dtNew;
                    Sec.AddTick(Ticks);
                    // tickDBDataSet1.Tables.Add();
                }
            }
            Sec.AddLast();
            
          //  BarsDraw = new PaintPoint();
          //  BarsDraw.Add(Sec);
         //   WindowGL.BarsDraw = BarsDraw;

            ds = new DrawSeries(Sec.Bars.First().Value.Open, Sec.deltaTick,minVol,maxVol);
            ds.Add(Sec);
         //   WindowGL.DrawSer = ds;
           // WindowGL.PriceOpen = Sec.Bars.ElementAt(0).Value.Open;


          //  Sec.DrawPoint();
           // WindowGL.Sec = Sec;
            MessageBox.Show(Sec.Bars.Count().ToString());
            Chart.LookSet();
            Chart.Plot();
        }

        public bool add = false;

        public bool Real
        {
            get
            {
                return Real;
            }

            set
            {
                Real = value;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Chart.Plot();
        }

        private void TickGen()
        {
            Ticks = new Tick();
            var lines = File.ReadAllLines("D:\\Market Data\\fut_deal1.csv", System.Text.Encoding.Default);
            for (int i = 1; i < lines.Count(); i++)
            {
                string[] parts = lines[i].Split(';');
                DateTime dtNew = DateTime.Now;
                if (parts[2] == "RTS-12.16")
                {
                    string dt = parts[0] + ' ' + parts[1];
                    DateTime t = DateTime.ParseExact(dt, "dd.MM.yyyy HH:mm:ss", null);
                    //Ticks.dateTimeTick = t;
                    Ticks.priceTick = Convert.ToDouble(parts[3].Replace(".", ","));
                    Ticks.volumeTick = Convert.ToInt32(parts[4]);
                    dtNew = new DateTime(t.Year, t.Month, t.Day, t.Hour, t.Minute, 00);
                    Ticks.dateTimeTick = dtNew;
                    //GetTick(Ticks);
                    // Sec.AddTick(Ticks);
                    // tickDBDataSet1.Tables.Add();
                }
                Thread.Sleep(3);
            }
            
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            lbTime.Text = DateTime.Now.ToLongTimeString();
        }

        private void tbIgnoreTickVol_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(Char.IsDigit(e.KeyChar)) && (e.KeyChar != 45)) e.Handled = true;
        }

        private void tbMinVol_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(Char.IsDigit(e.KeyChar)) && (e.KeyChar != 45)) e.Handled = true;
        }

        private void tbVolFilterMax_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(Char.IsDigit(e.KeyChar)) && (e.KeyChar != 45)) e.Handled = true;
        }

        private void btApply_Click(object sender, EventArgs e)
        {
         //   Sec.Load = false;
          //  ds.Load = false;
            minVol = Convert.ToInt32(tbVolFilterMin.Text);
            maxVol = Convert.ToInt32(tbVolFilterMax.Text);
            ignorTickVol = Convert.ToInt32(tbIgnoreTickVol.Text);
            secName = cbSecurity.SelectedItem.ToString();
            string[] TfArg = cbTf.SelectedItem.ToString().Split(' ');
            switch (TfArg[1])
            {
                case "SEC":
                    TF.frame = TfRange.SEC;
                    break;
                case "MIN":
                    TF.frame = TfRange.MIN;
                    break;
                case "HOUR":
                    TF.frame = TfRange.HOUR;
                    break;
                case "DAY":
                    TF.frame = TfRange.DAY;
                    break;
            }
            TF.digit = Convert.ToInt32(TfArg[0]);

            if (ignorTickVol != prevIgnorTickVol || secName != prevSecName || TFPrev.frame != TF.frame || TF.digit != TFPrev.digit)
            {
                while (Chart.PlotSer)
                {

                }
                if (Sec != null)
                {
                    Sec.Dispose();
                    Sec = null;
                 //   if(secName == "SIH6")
                  //   Sec = new DataSeries(secName, 1, Exchange.SPBFUT, TimeFrame.M1, chartStyle.Cluster, ignorTickVol);
                 //   else Sec = new DataSeries(secName, 1, Exchange.SPBFUT, TimeFrame.M1, chartStyle.Cluster, ignorTickVol);
                }
                prevIgnorTickVol = ignorTickVol;
                TFPrev = TF;
            }
            if (ds != null)
            {
                ds.Dispose();
                ds = null;
                if(Sec != null)
                ds.Add(Sec);
               // if (secName == "SIH6")
               //     Sec = new DataSeries(secName, 1, Exchange.SPBFUT, TimeFrame.M1, chartStyle.Cluster, ignorTickVol);
               // else Sec = new DataSeries(secName, 1, Exchange.SPBFUT, TimeFrame.M1, chartStyle.Cluster, ignorTickVol);
            }
           // ds?.Dispose();
           
         //   Load_Click(sender, e);


         //   Chart.LookSet();
         //   Chart.Plot();
        }

        private void toolStripContainer1_TopToolStripPanel_Paint(object sender, PaintEventArgs e)
        {
            using (LinearGradientBrush brush = new LinearGradientBrush(new Point(10, 0),
                                                                new Point(10, 50),
                                                               Color.Red,
                                                               Color.Black
                                                               ))
            {
                e.Graphics.FillRectangle(brush, this.ClientRectangle);
            }
        }

        private void WindowCluster_FormClosing(object sender, FormClosingEventArgs e)
        {
            real = false;
            timer1.Enabled = false;
            run = false;
            if(Sec != null)
                Sec.Dispose();
            if(ds != null)
                ds.Dispose();
           // if(dealQueue != null)
           //     dealQueue.d();
            //this.Close();
        }

        private void cbSecurity_SelectedValueChanged(object sender, EventArgs e)
        {
            btApply_Click(sender, e);
        }

        private void cbTf_SelectedValueChanged(object sender, EventArgs e)
        {
            btApply_Click(sender, e);
        }
    }
}
