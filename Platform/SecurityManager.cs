using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Platform
{
    public partial class SecurityManager : Form
    {
        private ConnectorQuik connector;
        public SecurityManager(ConnectorQuik Q)
        {
            InitializeComponent();
            connector = Q;
            if (connector.connected)
            {
                connector.SendCom(SendCommand.GetClassCode, "");
            }
            connector.Event_GetClassCode += Connector_Event_GetClassCode;
            connector.Event_GetSecurity += Connector_Event_GetSecurity;
        }

        private void Connector_Event_GetSecurity(string[] str)
        {
            listName.Items.Clear();
            listName.Items.AddRange(str);
        }

        private void Connector_Event_GetClassCode(string[] str)
        {

                listClass.Items.AddRange(str);
                
            
            
        }


        private void lbSec_Paint(object sender, PaintEventArgs e)
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

        private void btAdd_Click(object sender, EventArgs e)
        {
            if (connector.connected)
            {
                connector.SendCom(SendCommand.GetClassCode, "");
            }
        }

        private void listClass_SelectedIndexChanged(object sender, EventArgs e)
        {
            connector.SendCom(SendCommand.GetSecurity, ";"+listClass.SelectedItem.ToString());
        }

        private void listName_SelectedIndexChanged(object sender, EventArgs e)
        {
            listSelect.Items.Add(listName.SelectedItem.ToString());
        }

        private void listSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            listSelect.Items.Remove(listSelect.SelectedItem);
        }
    }
}
