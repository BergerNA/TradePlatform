using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tao.Platform.Windows;

namespace Platform
{
    public enum Coordination
    {
        LeftLow, Center, LeftHi
    }

    public class PaintPoint
    {
        public bool Load;

        public class Cl
        {
            public class PointCl
            {
                public Point To;
                public Point Of;
            }

            public List<PointCl> Point;
            public DateTime date;
            public Cl(DateTime dt)
            {
                Point = new List<PointCl>();
                date = dt;
            }

            public void AddCl(PointCl v)
            {
                Point.Add(v);
            }
        }

        public List<Cl> Bars;
        public PaintPoint()
        {
            Bars = new List<Cl>();
            Load = false;
        }

//        public void Add(DataSeris ds, Coordination coord, SimpleOpenGlControl sm)
//        {
//            if (coord == Coordination.LeftLow)
//            {
//                int count = ds.Bars.Count;
//                foreach (var bar in ds.Bars)
//                {
//                    Cl l = new Cl(bar.Key);
//                    Point dr = new Point();
//                    Point drto = new Point();
//                    dr.X = -Axis.widthY - Paint.widthCl*(count--);
//
//                    foreach (var price in bar.Value.PriceVol)
//                    {
//                        dr.Y = 0 + Convert.ToInt32((price.Key - Paint.startPrice)/Paint.delta)*Paint.hightCl;
//                        drto = dr;
//                        if (price.Value > 300)
//                            drto.X = dr.X + Paint.widthCl - 1;
//                        else drto.X += Convert.ToInt32(((price.Value*100/300)*(Paint.widthCl - 1))/100);
//                        if (drto.X == dr.X) drto.X++;
//                        Cl.PointCl s = new Cl.PointCl();
//                        s.Of = dr;
//                        s.To = drto;
//                        l.AddCl(s);
//                    }
//                    Bars.Add(l);
//                }
//                Load = true;
//            }
//        }

        private double startPrice;
        private double deltaTick;
        public void Add(DataSeries ds)
        {
            Load = false;
            startPrice = ds.Bars.First().Value.Open;
            deltaTick = ds.deltaTick;
            int i = 0;
            Point dr = new Point();
            Point drto = new Point();
            foreach (var bar in ds.Bars)
            {
                Cl l = new Cl(bar.Key);
          //      dr.X = WindowGL.wiCl * (i++);
                foreach (var price in bar.Value.Price)
                {
          //          dr.Y = Convert.ToInt32((price.Key - startPrice) / deltaTick) * WindowGL.hiCl;
                    drto = dr;
                    if (price.Value.Volume > 300)
          //              drto.X = dr.X + WindowGL.wiCl - 1;
        //            else drto.X += Convert.ToInt32(((price.Value.Volume * 100 / 300) * (WindowGL.wiCl - 1)) / 100);
                    if (drto.X == dr.X) drto.X++;
                    Cl.PointCl s = new Cl.PointCl();
                    s.Of = dr;
                    s.To = drto;
                    l.AddCl(s);
                }
                Bars.Add(l);
            }
            Load = true;
        }

        Point dr = new Point();
        Point drto = new Point();
        
        public void AddReal(DataSeries ds, Tick tk)
        {
            Load = false;

            //int i = 0;
            dr = new Point(Bars.Last().Point.First().Of.X, Bars.Last().Point.First().Of.Y);
            drto = new Point();
            //  if (ds.Bars.Count != Bars.Count)
            {
                
                Cl l = new Cl(ds.Bars.Last().Key);
              //  Bars.Remove(l);
        //        dr.X = WindowGL.wiCl * (ds.Bars.Count-1);
                foreach (var price in ds.Bars.Last().Value.Price)
                {
        //            dr.Y = Convert.ToInt32((price.Key - startPrice) / deltaTick) * WindowGL.hiCl;
                    drto = dr;
                    if (price.Value.Volume > 300)
         //               drto.X = dr.X + WindowGL.wiCl - 1;
      //              else drto.X += Convert.ToInt32(((price.Value.Volume * 100/ 300) * (WindowGL.wiCl - 1))/100);
                    if (drto.X == dr.X) drto.X++;
                    Cl.PointCl s = new Cl.PointCl();
                    s.Of = dr;
                    s.To = drto;
                    l.AddCl(s);
                }
                Bars.Add(l);
            }
        //    else
            {
         //       Bars.Last().Point.Contains(tk.priceTick)
        //        Cl.PointCl s = new Cl.PointCl();
        //        s.Of = dr;
        //        s.To = drto;
        //        ds.Bars.Last().Value.Price[tk.priceTick].To.X = ds.Bars.Last().Value.Price[tk.priceTick].Of.X + Convert.ToInt32(((ds.Bars.Last().Value.Price[tk.priceTick].Volume * 100 / WindowCluster.volFilter) * (WindowGL.wiCl - 1)) / 100);
            }

            Load = true;
        }


        public void Dispose()
        {
            Load = false;
            Bars.Clear();
        }
    }
}
