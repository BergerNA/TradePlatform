using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tao.Platform.Windows;

// Рабочий вариант

namespace Platform
{
    public class DataSeries
    {
        public double maxPrice;
        public double minPrice;
        public TimeFrame TFrame;
        private chartStyle ChStyle;
        public double deltaTick;
        public string nameSec;
        public bool Load = false;
        private Exchange exchange;
        protected int minVolume;
        private double startPrice;
        public Dictionary<DateTime, PriceClaster> Bars;
        public PaintPoint BarsDraw;

        public class PriceClaster
        {
            public Dictionary<double, Cluster> Price;
            //public double price;
            // public Int16 vol;
            public double Open;
            public double High;
            public double Low;
            public double Close;

            public class Cluster
            {
               // public Point Of, To;
                public int Volume;
                public Cluster(int v)
                {
                    Volume = v;
                }
            }

            public PriceClaster(double p, int v, int minVolume)
            {
                Price = new Dictionary<double, Cluster>();
                if (!Price.ContainsKey(p))
                    Price.Add(p, new Cluster(v));
                else
                {
                    if (v >= minVolume)
                        Price[p].Volume += v;
                }
            }

            public void AddPriceVol(double p, int v, int minVolume)
            {
                if (!Price.ContainsKey(p))
                    Price.Add(p, new Cluster(v));
                else
                {
                    if (v >= minVolume)
                        Price[p].Volume += v;
                }
            }
        }
        public DataSeries(string pNam, double dltT, Exchange ex, TimeFrame tf, chartStyle chart, int minVolumeForSum)
        {
            Bars = new Dictionary<DateTime, PriceClaster>();
            nameSec = pNam;
            deltaTick = dltT;
            exchange = ex;
            TFrame = tf;
            ChStyle = chart;
            maxPrice = 0;
            minPrice = 9999999;
            minVolume = minVolumeForSum;
        }

        // Переменные для методов
        private double maxPriceBar = 0;
        private double minPriceBar = 9999999;
        private double closeBar = 0;
        private DateTime dateTimePrevTk;

        public void AddTick(Tick tk)
        {
            Load = false;
            if (tk.priceTick > maxPrice)
                maxPrice = tk.priceTick;
            else if (tk.priceTick < minPrice)
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
                Bars.Add(tk.dateTimeTick, new PriceClaster(tk.priceTick, tk.volumeTick, minVolume));
                Bars[tk.dateTimeTick].Open = tk.priceTick;
                maxPriceBar = minPriceBar = tk.priceTick;
                dateTimePrevTk = tk.dateTimeTick;
            }
            else
            {
                Bars[tk.dateTimeTick].AddPriceVol(tk.priceTick, tk.volumeTick, minVolume);
                if (tk.priceTick > maxPriceBar)
                    maxPriceBar = tk.priceTick;
                if (tk.priceTick < minPriceBar)
                    minPriceBar = tk.priceTick;
            }
            Load = true;
        }

        public void AddLast()
        {
            Bars[dateTimePrevTk].Close = closeBar;
            Bars[dateTimePrevTk].High = maxPriceBar;
            Bars[dateTimePrevTk].Low = minPriceBar;
            Load = true;
        }

      /*  public void DrawPoint()
        {
            Load = false;
            startPrice = Bars.First().Value.Open;
            int i = 0;
            Point dr = new Point();
            Point drto = new Point();
            foreach (var bar in Bars)
            {
                dr.X = WindowGL.wiCl*(i++);
                foreach (var price in bar.Value.Price)
                {
                    dr.Y = Convert.ToInt32((price.Key - startPrice)/ deltaTick) * WindowGL.hiCl;
                    drto = dr;
                    if (price.Value.Volume > WindowCluster.volFilter)
                        drto.X = dr.X + WindowGL.wiCl - 1;
                    else drto.X += Convert.ToInt32(((price.Value.Volume*100/ WindowCluster.volFilter) *(WindowGL.wiCl - 1))/100);
                    if (drto.X == dr.X) drto.X++;
                    price.Value.Of = dr;
                    price.Value.To = drto;
                }
            }
            Load = true;
        }*/

        public void Dispose()
        {
            Bars.Clear();
        }
    }
}
