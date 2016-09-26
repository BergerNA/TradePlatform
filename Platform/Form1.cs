using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

// Главное окно

namespace Platform
{
    public enum TfRange { SEC, MIN, HOUR, DAY}
    public enum chartStyle { Cluster, Tick, VolumeRange };
    public struct TimeFrame1
    {
        public TfRange frame;
        public int digit;

        public TimeFrame1(TfRange fr, int dig)
        {
            frame = fr;
            digit = dig;
        }
    }

    public partial class Platform : Form
	{
	    private ConnectorQuik connector;
	    private FontPlot fontForPlot;
        public Platform()
        {
            InitializeComponent();
            ChartGl.InitializeContexts();
            fontForPlot = new FontPlot();
        }

        // Вызов окна Кластер чарт
        private void btClasterWindow_Click(object sender, EventArgs e)
        {
               WindowCluster windowCluster = new WindowCluster(connector);
               windowCluster.Show();
        }

        // Подписка на события от терминала Quik
        private void Platform_Load(object sender, EventArgs e)
        {
            connector = new ConnectorQuik();
            connector.Event_GetConnect += Connector_Event_GetConnect;
        }

        // Соединение с терминалом
        private void Connector_Event_GetConnect(bool connect)
        {
            if(connect) lbConnect.ForeColor = Color.Green;
            else lbConnect.ForeColor = Color.Red;
        }

        // Отрисовка с градиентом
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

        // Закрытие формы
        private void Platform_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (connector != null)
                connector.Dispose();
            fontForPlot.Dispose();
            // this.Close();
        }

        // Отображение времени
        private void timer1_Tick(object sender, EventArgs e)
        {
            lbTime.Text = DateTime.Now.ToLongTimeString();
        }

        // Модальное диалоговое окно выбора инструментов для пердачи тиков от терминала в программу
        private void btSecurity_Click(object sender, EventArgs e)
        {
            SecurityManager secmen = new SecurityManager(connector);
            secmen.ShowDialog();
        }

        // Соединение с терминалом
        private void btConnection_Click(object sender, EventArgs e)
        {
            connector.Connect();
        }
    }
}