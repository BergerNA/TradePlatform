using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

// Класс, переводит ценовые и временные данные фин. инструмента в систему координат для отображения.

namespace Platform
{
    public class DrawSeries
    {
        
        public class BarDraw
        {
            public Dictionary<double, int[]> Price;
            public BarDraw()
            {
                Price = new Dictionary<double, int[]>();
            }
        }
        public Dictionary<DateTime, BarDraw> Bars;
        public DrawSeries(double pr, double delta, int minV, int maxV)
        {
            Bars = new Dictionary<DateTime, BarDraw>();
            startPrice = pr;
            deltaTick = delta;
            minVol = minV;
            maxVol = maxV;
            
        }

        public void ClSize(int wCl, int hCl)
        {
            wiCl = wCl;
            hiCl = hCl;
        }
        private double startPrice;
        private double deltaTick;
        private int wiCl = 20;
        private int hiCl = 10;
        private int minVol;
        private int maxVol;
        public bool Load;
        public double lastPrice = 0;
        public int lastPriceY = 0;
        public int lastPriceX = 0;
        private int xTo, xOf, yTo, yOf;
        int i = 0;
        public void Add(DataSeries ds)
        {
            Load = false;
            i = 0;
            foreach (var bar in ds.Bars)
            {
                Bars.Add(bar.Key,new BarDraw());
                xOf = wiCl * (i++);
                foreach (var price in bar.Value.Price)
                {
                    yTo = Convert.ToInt32((price.Key - startPrice) / deltaTick) * hiCl;
                    if (price.Value.Volume > maxVol)
                        xTo = xOf + wiCl - 1;
                    else if (price.Value.Volume > minVol) xTo = xOf + ((price.Value.Volume * 100 / minVol) * (wiCl/2 - 1))/100;
                    else xTo = xOf + ((price.Value.Volume * 100 / minVol) * (wiCl/3 - 1))/100;
                    if (xTo == xOf) xTo++;
                    Bars[bar.Key].Price.Add(price.Key,new int[4]{xOf,xTo,yTo,price.Value.Volume});
                }
            }
            Load = true;
        }

        public void Resize(DataSeries ds)
        {
            Load = false;
            i = 0;
            if (ds.Load)
            {

                foreach (var bar in ds.Bars)
                {
                    //Bars.Add(bar.Key, new BarDraw());
                    DateTime d = bar.Key;
                    var b = Bars[d];
                    xOf = wiCl*(i++);
                    foreach (var price in bar.Value.Price)
                    {
                        var p = b.Price[price.Key];
                        yTo = Convert.ToInt32((price.Key - startPrice)/deltaTick)*hiCl;

                        if (price.Value.Volume > maxVol)
                            xTo = xOf + wiCl - 1;
                        else if (price.Value.Volume > minVol)
                            xTo = xOf + ((price.Value.Volume*100/minVol)*(wiCl/2 - 1))/100;
                        else xTo = xOf + ((price.Value.Volume*100/minVol)*(wiCl/3 - 1))/100;
                        if (xTo == xOf) xTo++;

                        p[0] = xOf;
                        p[1] = xTo;
                        p[2] = yTo;
                    }
                }

            }
            Load = true;
            
        }

        public void AddRealTick(DataSeries ds, Tick tk)
        {
            Load = false;
            //int i = 0;
            if (Bars.Count != ds.Bars.Count)
            {
               // var bar = ds.Bars.Last();
                {
                    Bars.Add(tk.dateTimeTick, new BarDraw());
                    xOf = wiCl*(Bars.Count-1);

                    yTo = Convert.ToInt32((tk.priceTick - startPrice)/deltaTick)*hiCl;
                    if (tk.volumeTick > maxVol)
                        xTo = xOf + wiCl - 1;
                    else if (tk.volumeTick > minVol)
                        xTo = xOf + ((tk.volumeTick * 100 / minVol) * (wiCl / 2 - 1)) / 100;
                    else xTo = xOf + ((tk.volumeTick * 100 / minVol) * (wiCl / 3 - 1)) / 100;
                    if (xTo == xOf) xTo++;
                        Bars.Last().Value.Price.Add(tk.priceTick, new int[4] {xOf, xTo, yTo, tk.volumeTick});
                }
            }
            else
            {
                var bar = Bars.Last();
                if (bar.Value.Price.ContainsKey(tk.priceTick))
                {
                    xOf = bar.Value.Price[tk.priceTick][0];
                    int vol = Convert.ToInt32(ds.Bars.Last().Value.Price[tk.priceTick].Volume);
                    if (vol > maxVol)
                        xTo = xOf + wiCl - 1;
                    else if (vol > minVol)
                        xTo = xOf + ((vol * 100 / minVol) * (wiCl / 2 - 1)) / 100;
                    else xTo = xOf + ((vol * 100 / minVol) * (wiCl / 3 - 1)) / 100;
                    if (xTo == xOf) xTo++;
                    bar.Value.Price[tk.priceTick][1] = xTo;
                    bar.Value.Price[tk.priceTick][3] = vol;
                    yTo = bar.Value.Price[tk.priceTick][2];
                }
                else
                {
                    xOf = wiCl * (ds.Bars.Count - 1);
                    yTo = Convert.ToInt32((tk.priceTick - startPrice) / deltaTick) * hiCl;
                    if (tk.volumeTick > maxVol)
                        xTo = xOf + wiCl - 1;
                    else if (tk.volumeTick > minVol)
                        xTo = xOf + ((tk.volumeTick * 100 / minVol) * (wiCl / 2 - 1)) / 100;
                    else xTo = xOf + ((tk.volumeTick * 100 / minVol) * (wiCl / 3 - 1)) / 100;
                    if (xTo == xOf) xTo++;
                    Bars.Last().Value.Price.Add(tk.priceTick, new int[4] { xOf, xTo, yTo, tk.volumeTick });
                }
                
            }
            lastPrice = tk.priceTick;
            lastPriceY = yTo + hiCl / 2;
            lastPriceX = xOf + wiCl;
            Load = true;
        }

        public void Dispose()
        {
            Load = false;
            Bars.Clear();
            
        }
    }
}
