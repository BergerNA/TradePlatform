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

// ������� ����

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

        // ����� ���� ������� ����
        private void btClasterWindow_Click(object sender, EventArgs e)
        {
               WindowCluster windowCluster = new WindowCluster(connector);
               windowCluster.Show();
        }

        // �������� �� ������� �� ��������� Quik
        private void Platform_Load(object sender, EventArgs e)
        {
            connector = new ConnectorQuik();
            connector.Event_GetConnect += Connector_Event_GetConnect;
        }

        // ���������� � ����������
        private void Connector_Event_GetConnect(bool connect)
        {
            if(connect) lbConnect.ForeColor = Color.Green;
            else lbConnect.ForeColor = Color.Red;
        }

        // ��������� � ����������
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

        // �������� �����
        private void Platform_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (connector != null)
                connector.Dispose();
            fontForPlot.Dispose();
            // this.Close();
        }

        // ����������� �������
        private void timer1_Tick(object sender, EventArgs e)
        {
            lbTime.Text = DateTime.Now.ToLongTimeString();
        }

        // ��������� ���������� ���� ������ ������������ ��� ������� ����� �� ��������� � ���������
        private void btSecurity_Click(object sender, EventArgs e)
        {
            SecurityManager secmen = new SecurityManager(connector);
            secmen.ShowDialog();
        }

        // ���������� � ����������
        private void btConnection_Click(object sender, EventArgs e)
        {
            connector.Connect();
        }
    }
}