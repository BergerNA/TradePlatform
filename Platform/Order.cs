using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Platform.Properties;

// Отображение панели, позволяющая совершать торговые операции

namespace Platform
{
    class Order
    {
        internal class OrderButton
        {
            enum Unit
            {
                Pips, Percent, Cash
            }

            internal enum TypeButton
            {
                BuyLimit, SellLimit, BuyMarket, SellMarket, CloseAll
            }

            private TypeButton typeButton;
            public PictureBox button;
            private Label lbSL;
            private Label lbTP;
            private Label lbTR;
            private Label lbSLV;
            private Label lbTPV;
            private Label lbTRV;
            private Panel panel;
            private Image image;
            private bool orderSetup = false;
            public static int i = 0;
            private double SL = 0;
            private double TP = 0;
            private double TR = 0;
            private Unit typeUnit = Unit.Pips;
            

            public OrderButton(Panel pnl, Image img, TypeButton tb)
            {
                typeButton = tb;
                panel = pnl;
                image = img;
            }

            // Инициализация компонент 1-й кнопки панели и её свойств (SL, TP, TralStop)
            public void Create()
            {
                button = new PictureBox();
                button.Size = new Size(65, 65);
                button.SizeMode = PictureBoxSizeMode.StretchImage;
                button.Image = image;
                button.Location = new Point(10, i + 10);
                button.Cursor = Cursors.Hand;
                button.Click += Button_Click; ;

                lbSL = new Label();
                lbSL.Font = new Font(FontFamily.GenericSansSerif, 9);
                lbSL.Location = new Point(85, i + 13);
                lbSL.Size = new Size(85, 20);
                lbSL.ForeColor = Color.Gray;
                lbSL.Cursor = Cursors.Hand;
                lbSL.Text = "Stop Loss - ";
                lbSL.Click += label_Clic;

                lbTP = new Label();
                lbTP.Font = new Font(FontFamily.GenericSansSerif, 9);
                lbTP.Location = new Point(85, i + 33);
                lbTP.Size = new Size(85, 20);
                lbTP.ForeColor = Color.Gray;
                lbTP.Cursor = Cursors.Hand;
                lbTP.Text = "Take Profit - ";
                lbTP.Click += label_Clic;

                lbTR = new Label();
                lbTR.Font = new Font(FontFamily.GenericSansSerif, 9);
                lbTR.Location = new Point(85, i + 53);
                lbTR.Size = new Size(85, 20);
                lbTR.ForeColor = Color.Gray;
                lbTR.Cursor = Cursors.Hand;
                lbTR.Text = "Trailing Stop - ";
                lbTR.Click += label_Clic;

                lbSLV = new Label();
                lbSLV.Font = new Font(FontFamily.GenericSansSerif, 9);
                lbSLV.Location = new Point(170, i + 13);
                lbSLV.Cursor = Cursors.Hand;
                if (orderSetup)
                {
                    lbSLV.ForeColor = Color.Red;
                    lbSLV.Text = SL.ToString();
                }
                else
                {
                    lbSLV.ForeColor = Color.Gray;
                    lbSLV.Text = SL.ToString();
                }
                lbSLV.Click += label_Clic;

                lbTPV = new Label();
                lbTPV.Font = new Font(FontFamily.GenericSansSerif, 9);
                lbTPV.Location = new Point(170, i + 33);
                lbTPV.Cursor = Cursors.Hand;
                if (orderSetup)
                {
                    lbTPV.ForeColor = Color.LawnGreen;
                    lbTPV.Text = TP.ToString();
                }
                else
                {
                    lbTPV.ForeColor = Color.Gray;
                    lbTPV.Text = TP.ToString();
                }
                lbTPV.Click += label_Clic;

                lbTRV = new Label();
                lbTRV.Font = new Font(FontFamily.GenericSansSerif, 9);
                lbTRV.Location = new Point(170, i + 53);
                lbTRV.Cursor = Cursors.Hand;
                if (orderSetup)
                {
                    lbTRV.ForeColor = Color.Red;
                    lbTRV.Text = TR.ToString();
                }
                else
                {
                    lbTRV.ForeColor = Color.Gray;
                    lbTRV.Text = TR.ToString();
                }
                lbTRV.Click += label_Clic;

                panel.Controls.Add(button);
                panel.Controls.Add(lbSL);
                panel.Controls.Add(lbTP);
                panel.Controls.Add(lbTR);
                panel.Controls.Add(lbSLV);
                panel.Controls.Add(lbTPV);
                panel.Controls.Add(lbTRV);
                i +=75;
            }

            // Обнуление глобальной переменной, для последующей отрисовки свойств 1-ой кнопки
            public void I()
            {
                i = 0;
            }

            private OrderSetup setup;

            private void label_Clic(object sender, EventArgs e)
            {
                setup = new OrderSetup();
                setup.lbEnable.Text = orderSetup ? "ON" : "OFF";
                setup.lbEnable.BackColor = orderSetup ? Color.DarkOrange : Color.Silver;
                setup.tbSL.Text = SL.ToString();
                setup.tbTP.Text = TP.ToString();
                setup.tbTR.Text = TR.ToString();
                setup.cmUnit.SelectedIndex = 0;
                setup.ShowDialog();
                setup.Closing += Setup_Closing;
            }

            private void Setup_Closing(object sender, System.ComponentModel.CancelEventArgs e)
            {
                orderSetup = setup.lbEnable.Text == "ON" ? true : false;
                SL = setup.tbSL.Text != "" ? Convert.ToDouble(setup.tbSL.Text) : 0;
                TP = setup.tbTP.Text != "" ? Convert.ToDouble(setup.tbTP.Text) : 0;
                TR = setup.tbTR.Text != "" ? Convert.ToDouble(setup.tbTR.Text) : 0;
                switch (setup.cmUnit.Text)
                {
                    case "Pips":
                        typeUnit = Unit.Pips;
                        break;
                    case "Percent":
                        typeUnit = Unit.Percent;
                        break;
                    case "Cash":
                        typeUnit = Unit.Cash;
                        break;
                }
                lbSLV.Text = SL.ToString();
                lbTPV.Text = TP.ToString();
                lbTRV.Text = TR.ToString();

                if (orderSetup)
                {
                    lbSLV.ForeColor = SL == 0? Color.Gray: Color.Red;
                    lbTPV.ForeColor = TP == 0 ? Color.Gray : Color.LawnGreen;
                    lbTRV.ForeColor = TR == 0 ? Color.Gray : Color.Red;
                }
                else
                {
                    lbSLV.ForeColor = Color.Gray;
                    lbTPV.ForeColor = Color.Gray;
                    lbTRV.ForeColor = Color.Gray;
                }
            }

            // Смена изображения кнопки
            private void Button_Click(object sender, EventArgs e)
            {
                if (typeButton == TypeButton.BuyLimit)
                {
                    button.Image = Resources.BuyLimitGreen;
                    Event_LimitClic.Invoke(true);
                }
                else if (typeButton == TypeButton.SellLimit)
                {
                    button.Image = Resources.SellLimitRed;
                    Event_LimitClic.Invoke(false);
                }

            }
            //Событие OnCount c типом делегата MethodContainer.
            public event LimitClic Event_LimitClic;
        }


        public delegate void LimitClic(bool buy);
        public void ChangeColor(bool buy)
        {
            if(buy)sellLimit.button.Image = Resources.SellLimitWhite;
            else buyLimit.button.Image = Resources.BuyLimitWhite;
        }

        private Panel panel;
        private PictureBox buyLimits;
        private Label lbBLSL;
        private Label lbBLTP;
        private Label lbBLTR;

       // private PictureBox sellLimit;
        private Label lbSLSL;
        private Label lbSLTP;
        private Label lbSLTR;

      //  private PictureBox sellMarket;
        private Label lbSML;
        private Label lbSMTP;
        private Label lbSMTR;

       // private PictureBox buyMarket;
        private Label lbBML;
        private Label lbBMTP;
        private Label lbBMTR;

        public OrderButton buyLimit;
        private OrderButton buyMarket;
        private OrderButton sellLimit;
        private OrderButton sellMarket;

        // Инициализация панели, подписка на события нажатий кнопок
        public Order()
        {
            panel = new Panel();
            
            panel.Location = new System.Drawing.Point(650, 100);
            panel.BackColor = Color.Transparent;
            panel.Name = "panel";
            panel.Size = new System.Drawing.Size(208, 498);
           // panel.Anchor = AnchorStyles.Right;
        
            sellLimit = new OrderButton(panel, Resources.SellLimitRed, OrderButton.TypeButton.SellLimit);
            sellLimit.Event_LimitClic += new LimitClic(ChangeColor);
            sellLimit.Create();

            buyLimit = new OrderButton(panel, Resources.BuyLimitGreen, OrderButton.TypeButton.BuyLimit);
            buyLimit.Event_LimitClic += new LimitClic(ChangeColor);
            buyLimit.Create();

            sellMarket = new OrderButton(panel, Resources.SellMarket, OrderButton.TypeButton.SellMarket);
            sellMarket.Create();

            buyMarket = new OrderButton(panel, Resources.BuyMarket, OrderButton.TypeButton.BuyMarket);
            buyMarket.Create();
            buyLimit.I();
        }

        public Control GetControl()
        {
            return panel;
        }
    }
}
