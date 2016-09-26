using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Platform
{
    public enum TimeFrame { Year, Month, Day, Hour, M60, M20, M15, M10, M5, M3, M2, M1, S30, S20, S15, S10 }

    public class DataSeris
    {
        public class PriceClaster : IEnumerable
        {
            public Dictionary<double, int> PriceVol;
            //public double price;
            // public Int16 vol;
            public double Open;
            public double High;
            public double Low;
            public double Close;
            public DateTime DateTimeBar;

            public PriceClaster(double p, int v)
            {
                PriceVol = new Dictionary<double, int>();
                if (!PriceVol.ContainsKey(p))
                    PriceVol.Add(p, v);
                else
                {
                    if (v > 1)
                        PriceVol[p] += v;
                }
            }

            public void AddPriceVol(double p, int v)
            {
                if (!PriceVol.ContainsKey(p))
                    PriceVol.Add(p, v);
                else
                {

                    if (v > 5)
                        PriceVol[p] += v;
                }
            }

            public IEnumerator GetEnumerator()
            {
                throw new NotImplementedException();
            }
        }

        public Dictionary<DateTime, PriceClaster> Bars;

        public double maxPrice;
        public double minPrice;
        public TimeFrame TFrame;
        private chartStyle ChStyle;
        public double deltaTick;
        private string paperName;
        private Exchange exchange;

        public DataSeris()
        {
            Bars = new Dictionary<DateTime, PriceClaster>();
        }
        public DataSeris(chartStyle ch, TimeFrame tf, string pNam, double dltT, Exchange ex)
        {
            Bars = new Dictionary<DateTime, PriceClaster>();
            paperName = pNam;
            deltaTick = dltT;
            exchange = ex;
            TFrame = tf;
            ChStyle = ch;
            maxPrice = 0;
            minPrice = 9999999;

        }

        private double maxPriceBar = 0;
        private double minPriceBar = 9999999;
        private double closeBar = 0;
        private DateTime dateTimePrevTk;
        public void AddTick(Tick tk)
        {
            if (tk.priceTick > maxPrice)
                maxPrice = tk.priceTick;
            if (tk.priceTick < minPrice)
                minPrice = tk.priceTick;


            closeBar = tk.priceTick;
            if (!Bars.ContainsKey(tk.dateTimeTick))
            {
                if (dateTimePrevTk != DateTime.MinValue && dateTimePrevTk != tk.dateTimeTick)
                {
                    Bars[dateTimePrevTk].Close = closeBar;
                    Bars[dateTimePrevTk].High = maxPriceBar;
                    Bars[dateTimePrevTk].Low = minPriceBar;
                }
                Bars.Add(tk.dateTimeTick, new PriceClaster(tk.priceTick, tk.volumeTick));
                Bars[tk.dateTimeTick].DateTimeBar = dateTimePrevTk.Date;
                Bars[tk.dateTimeTick].Open = tk.priceTick;
                maxPriceBar = minPriceBar = tk.priceTick;
                dateTimePrevTk = tk.dateTimeTick;
            }
            else
            {
                Bars[tk.dateTimeTick].AddPriceVol(tk.priceTick, tk.volumeTick);
                if (tk.priceTick > maxPriceBar)
                    maxPriceBar = tk.priceTick;
                if (tk.priceTick < minPriceBar)
                    minPriceBar = tk.priceTick;
            }
        }

        public void AddLast()
        {
            Bars[dateTimePrevTk].Close = closeBar;
            Bars[dateTimePrevTk].High = maxPriceBar;
            Bars[dateTimePrevTk].Low = minPriceBar;
        }
        public void Dispose()
        {
            Bars.Clear();
        }
    }
}
