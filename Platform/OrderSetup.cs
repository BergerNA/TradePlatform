using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

// Окно настройки свойств торговой операции: 
// Измерение: cash, pips, %
// Значение StopLoss, TakeProfit, TrailingStop

namespace Platform
{
    public partial class OrderSetup : Form
    {
        public OrderSetup()
        {
            InitializeComponent();
        }

        private void lbEnable_Click(object sender, EventArgs e)
        {
            if (lbEnable.Text == "OFF")
            {
                lbEnable.Text = "ON";
                lbEnable.BackColor = Color.DarkOrange;
            }
            else
            {
                lbEnable.Text = "OFF";
                lbEnable.BackColor = Color.Silver;
            }
        }

        private void tb_KeyPress(object sender, KeyPressEventArgs e)
        {
            switch (cmUnit.Text)
            {
                
                case "Percent":
                    if (!(Char.IsDigit(e.KeyChar)) && !((e.KeyChar == '.') &&
                    (((TextBox)sender).Text.IndexOf(".") == -1) &&
                    (((TextBox)sender).Text.Length != 0)))
                    {
                        if (e.KeyChar != (char)Keys.Back)
                        {
                            e.Handled = true;
                        }
                    }
                    break;
                case "Cash":
                case "Pips":
                    if ((e.KeyChar <= 47 || e.KeyChar >= 58) && e.KeyChar != 8)
                        e.Handled = true;
                    break;
                default:

                    break;   
            }
        }
    }
}
