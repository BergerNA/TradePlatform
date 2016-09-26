using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

// Прототип
// v1.0

namespace Platform
{
    partial class DataSeriesSec
    {
        public double maxPrice;
        public double minPrice;
        private TimeFrame1 TF = new TimeFrame1();
        private chartStyle ChStyle;
        public double deltaTick;
        public string secName;
        public bool LoadSec = false;
        public bool LoadDraw = false;
        private Exchange exchange;
        protected int ignoreTick;
        private double startPrice;
        public Dictionary<DateTime, PriceClaster> Bars;
        public PaintPoint BarsDraw;
        private ConnectorQuik connector;
        private WindowGL chart;
        private ConcurrentQueue<string> dealQueue = new ConcurrentQueue<string>();

        public class PriceClaster
        {
            public Dictionary<double, Cluster> Price;
            public double Open;
            public double High;
            public double Low;
            public double Close;

            public class Cluster
            {
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

        public DataSeriesSec(string pNam, double dltT, Exchange ex, TimeFrame1 tf, chartStyle chartSt, int ignoreTickForSum, ConnectorQuik con, WindowGL ch)
        {
            Bars = new Dictionary<DateTime, PriceClaster>();
            chart = ch;
            connector = con;
            connector.Event_GetTicks += Connector_Event_GetTicks;
            secName = pNam;
            deltaTick = dltT;
            exchange = ex;
            TF = tf;
            ChStyle = chartSt;
            maxPrice = 0;
            minPrice = 9999999;
            ignoreTick = ignoreTickForSum;

            new Thread(() =>
            {
                LoadTick();
            }).Start();
        }

        private void Connector_Event_GetTicks(string str)
        {
            if (str.Contains(secName))
            {
                dealQueue.Enqueue(str);
            }
        }

        private bool run = true;
        private void LoadTick()
        {
            Dictionary<string, string> DictionaryParam = new Dictionary<string, string>();
            Tick tic = new Tick();
            int timech;
            string str = "";
            while (run)
            {
                if (dealQueue.Count > 0)
                {
                    for (int j = 0; j < dealQueue.Count; j++)
                    {
                        timech = 0;
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
                                tic.buy = Convert.ToByte(DictionaryParam["FLAGS"]);
                                DateTime t = DateTime.Now;
                                switch (TF.frame)
                                {
                                    case TfRange.SEC:
                                        timech = (tic.dateTimeTick.Second / TF.digit) * TF.digit;
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
                                //Connector_Event_GetTick(tic);
                            }
                        }
                    }
                }
                Thread.Sleep(1);
            }
        }

        public bool add = false;



        // Переменные для методов
        private double maxPriceBar = 0;
        private double minPriceBar = 9999999;
        private double closeBar = 0;
        private DateTime dateTimePrevTk;

        public void AddTick(Tick tk)
        {
            LoadSec = false;
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
                Bars.Add(tk.dateTimeTick, new PriceClaster(tk.priceTick, tk.volumeTick, ignoreTick));
                Bars[tk.dateTimeTick].Open = tk.priceTick;
                maxPriceBar = minPriceBar = tk.priceTick;
                dateTimePrevTk = tk.dateTimeTick;
            }
            else
            {
                Bars[tk.dateTimeTick].AddPriceVol(tk.priceTick, tk.volumeTick, ignoreTick);
                if (tk.priceTick > maxPriceBar)
                    maxPriceBar = tk.priceTick;
                if (tk.priceTick < minPriceBar)
                    minPriceBar = tk.priceTick;
            }
            LoadSec = true;
        }

        public void AddLast()
        {
            LoadSec = false;
            Bars[dateTimePrevTk].Close = closeBar;
            Bars[dateTimePrevTk].High = maxPriceBar;
            Bars[dateTimePrevTk].Low = minPriceBar;
            LoadSec = true;
        }


        public void Dispose()
        {
            Bars.Clear();
        }
    }
}
