using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// для работы с библиотекой OpenGL 
using Tao.OpenGl;
// для работы с библиотекой FreeGLUT 
using Tao.FreeGlut;
// для работы с элементом управления SimpleOpenGLControl 
using Tao.Platform.Windows;

// Класс реализующий отображение шрифтов OpenGl

namespace Platform
{
    public class FontPlot
    {
        public class point
        {
            public Byte X;
            public Byte Y;
          //  private List<point> l; 
            public point(Byte x, Byte y)
            {
                X = x;
                Y = y;
            }
            public void Add(Byte x, Byte y)
            {
               // l.Add(new point());
            }
        }
        public static Dictionary<char,List<point>>  Symbols = new Dictionary<char, List<point>>();
        public static Dictionary<char, List<point>> Symbolsn = new Dictionary<char, List<point>>();
        private static int x0;
        private static int y0;
        private static int x1;
        private static int y1;
        

        public FontPlot()
        {
            List<point> O = new List<point>();
            // O.Add(new point(1, 1));
            /* O.Add(new point(1, 8));
             O.Add(new point(6, 8));
             O.Add(new point(6, 1));
             O.Add(new point(1, 1));
             O.Add(new point(1, 9));*/
            O.Add(new point(4, 1));
            O.Add(new point(3, 1));
            O.Add(new point(1, 3));
            O.Add(new point(1, 8));
            O.Add(new point(3, 10));
            O.Add(new point(4, 9));
            O.Add(new point(6, 7));
            O.Add(new point(6, 4));
            O.Add(new point(3, 1));
            Symbols.Add('0',O);

            List<point> L = new List<point>();
          /*  L.Add(new point(0, 8));
            L.Add(new point(3, 8));
            L.Add(new point(3, 1));
            L.Add(new point(1, 1));
            L.Add(new point(6, 1));
            L.Add(new point(6, 4));*/
            L.Add(new point(2, 1));
            L.Add(new point(5, 1));
            L.Add(new point(4, 1));
            L.Add(new point(4, 10));
            L.Add(new point(1, 6));
            Symbols.Add('1', L);

            List<point> Dva = new List<point>();
            /*Dva.Add(new point(1, 8));
            Dva.Add(new point(6, 8));
            Dva.Add(new point(6, 5));
            Dva.Add(new point(1, 5));
            Dva.Add(new point(1, 1));
            Dva.Add(new point(6, 1));*/
            Dva.Add(new point(6, 1));
            Dva.Add(new point(1, 1));
            Dva.Add(new point(1, 2));
            Dva.Add(new point(6, 7));
           // Dva.Add(new point(6, 8));
            Dva.Add(new point(3, 10));
            Dva.Add(new point(2, 9));
            Dva.Add(new point(0, 7));
            Symbols.Add('2', Dva);

            List<point> E = new List<point>();
            /*E.Add(new point(1, 8));
            E.Add(new point(6, 8));
            E.Add(new point(6, 6));
            E.Add(new point(5, 5));
            E.Add(new point(2, 5));
            E.Add(new point(5, 5));
            E.Add(new point(6, 4));
            E.Add(new point(6, 1));
            E.Add(new point(1, 1));*/
            E.Add(new point(1, 7));
            E.Add(new point(3, 10));
            E.Add(new point(4, 9));
            E.Add(new point(6, 7));
            E.Add(new point(4, 5));
            E.Add(new point(5, 5));
            E.Add(new point(6, 4));
            E.Add(new point(6, 1));
            E.Add(new point(1, 1));
            Symbols.Add('3', E);

            List<point> Y = new List<point>();
            Y.Add(new point(1, 9));
            Y.Add(new point(1, 4));
            Y.Add(new point(6, 4));
            Y.Add(new point(4, 4));
            Y.Add(new point(4, 7));
            Y.Add(new point(4, 1));
            Symbols.Add('4', Y);
            List<point> Pyt = new List<point>();
            Pyt.Add(new point(6, 8));
            Pyt.Add(new point(0, 8));
            Pyt.Add(new point(1, 5));
            Pyt.Add(new point(6, 5));
            Pyt.Add(new point(6, 1));
            Pyt.Add(new point(0, 1));
            Symbols.Add('5', Pyt);
            List<point> Hest = new List<point>();
            Hest.Add(new point(2, 8));
            Hest.Add(new point(0, 8));
            Hest.Add(new point(1, 1));
            Hest.Add(new point(6, 1));
            Hest.Add(new point(6, 4));
            Hest.Add(new point(1, 4));
            Symbols.Add('6', Hest);
            List<point> Sem = new List<point>();
            Sem.Add(new point(1, 8));
            Sem.Add(new point(6, 8));
            Sem.Add(new point(6, 6));
            Sem.Add(new point(4, 4));
            Sem.Add(new point(4, 1));
            Symbols.Add('7', Sem);
            List<point> B = new List<point>();
            B.Add(new point(5, 6));
            B.Add(new point(5, 8));
            B.Add(new point(1, 8));
            B.Add(new point(2, 5));
            B.Add(new point(0, 5));
            B.Add(new point(1, 1));
            B.Add(new point(6, 1));
            B.Add(new point(6, 5));
            B.Add(new point(1, 5));
            //B.Add(new point(3, 4));
            Symbols.Add('8', B);
            List<point> Q = new List<point>();
            Q.Add(new point(5, 4));
            Q.Add(new point(1, 4));
            Q.Add(new point(0, 8));
            Q.Add(new point(6, 8));
            Q.Add(new point(6, 1));
            Q.Add(new point(4, 1));
            Symbols.Add('9', Q);
            List<point> Zp = new List<point>();
            Zp.Add(new point(1, 2));
            Zp.Add(new point(0, 0));
            Symbols.Add(',', Zp);
            List<point> Dvoetochie = new List<point>();
            Dvoetochie.Add(new point(1, 8));
            Dvoetochie.Add(new point(1, 6));
            Dvoetochie.Add(new point(1, 4));
            Dvoetochie.Add(new point(1, 2));
            Dvoetochie.Add(new point(0, 8));
            Dvoetochie.Add(new point(0, 6));
            Dvoetochie.Add(new point(0, 4));
            Dvoetochie.Add(new point(0, 2));
            Symbols.Add(':', Dvoetochie);
        }

        public FontPlot(SimpleOpenGlControl sm,int j)
        {
            x0 = sm.Width;
            y0 = sm.Height / 2;
            List<point> O = new List<point>();
            // O.Add(new point(1, 1));
            O.Add(new point(4, 1));
            O.Add(new point(3, 1));
            O.Add(new point(1, 3));
            O.Add(new point(1, 8));
            O.Add(new point(3, 10));
            O.Add(new point(5, 10));
            O.Add(new point(6, 8));
            O.Add(new point(6, 3));
            O.Add(new point(4, 1));

            Symbolsn.Add('0', O);
            List<point> L = new List<point>();
            L.Add(new point(0, 8));
            L.Add(new point(3, 8));
            L.Add(new point(3, 1));
            L.Add(new point(1, 1));
            L.Add(new point(6, 1));
            L.Add(new point(6, 4));
            Symbols.Add('1', L);
            List<point> Dva = new List<point>();
            Dva.Add(new point(1, 8));
            Dva.Add(new point(6, 8));
            Dva.Add(new point(6, 5));
            Dva.Add(new point(1, 5));
            Dva.Add(new point(1, 1));
            Dva.Add(new point(6, 1));
            Symbols.Add('2', Dva);
            List<point> E = new List<point>();
            E.Add(new point(1, 8));
            E.Add(new point(6, 8));
            E.Add(new point(6, 6));
            E.Add(new point(5, 5));
            E.Add(new point(2, 5));
            E.Add(new point(5, 5));
            E.Add(new point(6, 4));
            E.Add(new point(6, 1));
            E.Add(new point(1, 1));
            Symbols.Add('3', E);
            List<point> Y = new List<point>();
            Y.Add(new point(1, 9));
            Y.Add(new point(1, 4));
            Y.Add(new point(6, 4));
            Y.Add(new point(4, 4));
            Y.Add(new point(4, 7));
            Y.Add(new point(4, 1));
            Symbols.Add('4', Y);
            List<point> Pyt = new List<point>();
            Pyt.Add(new point(6, 8));
            Pyt.Add(new point(0, 8));
            Pyt.Add(new point(1, 5));
            Pyt.Add(new point(6, 5));
            Pyt.Add(new point(6, 1));
            Pyt.Add(new point(0, 1));
            Symbols.Add('5', Pyt);
            List<point> Hest = new List<point>();
            Hest.Add(new point(2, 8));
            Hest.Add(new point(0, 8));
            Hest.Add(new point(1, 1));
            Hest.Add(new point(6, 1));
            Hest.Add(new point(6, 4));
            Hest.Add(new point(1, 4));
            Symbols.Add('6', Hest);
            List<point> Sem = new List<point>();
            Sem.Add(new point(1, 8));
            Sem.Add(new point(6, 8));
            Sem.Add(new point(6, 6));
            Sem.Add(new point(4, 4));
            Sem.Add(new point(4, 1));
            Symbols.Add('7', Sem);
            List<point> B = new List<point>();
            B.Add(new point(5, 6));
            B.Add(new point(5, 8));
            B.Add(new point(1, 8));
            B.Add(new point(2, 5));
            B.Add(new point(0, 5));
            B.Add(new point(1, 1));
            B.Add(new point(6, 1));
            B.Add(new point(6, 5));
            B.Add(new point(1, 5));
            //B.Add(new point(3, 4));
            Symbols.Add('8', B);
            List<point> Q = new List<point>();
            Q.Add(new point(5, 4));
            Q.Add(new point(1, 4));
            Q.Add(new point(0, 8));
            Q.Add(new point(6, 8));
            Q.Add(new point(6, 1));
            Q.Add(new point(4, 1));
            Symbols.Add('9', Q);
            List<point> Zp = new List<point>();
            Zp.Add(new point(1, 2));
            Zp.Add(new point(0, 0));
            Symbols.Add(',', Zp);
            List<point> Dvoetochie = new List<point>();
            Dvoetochie.Add(new point(1, 8));
            Dvoetochie.Add(new point(1, 6));
            Dvoetochie.Add(new point(1, 4));
            Dvoetochie.Add(new point(1, 2));
            Dvoetochie.Add(new point(0, 8));
            Dvoetochie.Add(new point(0, 6));
            Dvoetochie.Add(new point(0, 4));
            Dvoetochie.Add(new point(0, 2));
            Symbols.Add(':', Dvoetochie);
        }

        // Изменение размера шрифта
        public void Reload(SimpleOpenGlControl sm)
        {
            x0 = sm.Width;
            y0 = sm.Height / 2;
            y1 = 0;
            x1 = 0;
        }

        // Прорисовка шрифтов на экране
        public static void Plot(PaintPoint ds, double stPr, double delta, int hi, int we, int hiCl, int weCl, int X0, int Y0)
        {
             x1 += x0 - X0;
             y1 += y0 - Y0;

           // double ff = y1 / delta;
            int hag = (int) (y1/hiCl);//delta);         // К-во кластеров в дельта смещении по оси цен
            int ost = Convert.ToInt32(y1 % hiCl);       // Пикселей до следующего кластера цены
            double pr = stPr + (hag*delta);             // Цена для отображения в соответствии со смещением       
            int dobavka = (Paint.hightCl - 10)/2;       // Цена по центру кластера
            dobavka -= ost;
            int dobavka2 = dobavka;
            dobavka -= hiCl;
            dobavka2 += hiCl;

            int delPrice = 10;
            if (Paint.hightCl >= 7 && Paint.hightCl < 10)
                delPrice = 20;
            else if (Paint.hightCl >= 5 && Paint.hightCl < 7)
                delPrice = 40;
            else if(Paint.hightCl >= 3 && Paint.hightCl < 5)
                delPrice = 50;
            else if (Paint.hightCl >= 2 && Paint.hightCl < 3)
                delPrice = 100;
            else if (Paint.hightCl < 2)
                delPrice = 100;
            Gl.glColor3f(1.0f, 1.0f, 1.0f);             
            int d = 0;
            for (double j = 0; j <= hi/2; j+= hiCl)
            {
                dobavka += hiCl;
                double price = pr + d * delta;
                if ((price) % delPrice == 0)
                {
                    string str = (price).ToString();
                    Byte i = 0;
                    foreach (var ch in str)
                    {
                        Gl.glBegin(Gl.GL_LINE_STRIP);
                        foreach (var f in FontPlot.Symbols[ch])
                        {
                            Gl.glVertex2i(-Axis.widthY + i*9 + 2 + f.X, dobavka + f.Y);
                        }
                        Gl.glEnd();
                        i++;
                    }
                }
                d++;
            }
            d = 0;
            for (double j = 0; j >= -hi / 2; j -= hiCl)
            {
                if ((dobavka2 -= hiCl) < -hi / 2 + 20)
                    break;
                double price = pr - d*delta;
                if ((price) % delPrice == 0)
                {
                    string str = (price).ToString();
                    Byte i = 0;
                    foreach (var ch in str)
                    {

                        Gl.glBegin(Gl.GL_LINE_STRIP);
                        foreach (var f in FontPlot.Symbols[ch])
                        {
                            if (dobavka + f.Y < - hi / 2)
                                break;
                            Gl.glVertex2i(-Axis.widthY + i*9 + 2 + f.X, dobavka2 + f.Y);
                        }
                        Gl.glEnd();
                        i++;
                    }
                }
                d++;
            }
            x0 = X0;
            y0 = Y0;

            int sum = 0;
            int t = ds.Bars.Count - 1;
            int numBar = ds.Bars.Count - 1;
            int deltaNum = 60 / weCl + 1;
            int printNum = numBar - deltaNum;

            for (int i = ds.Bars.Count - 1; i > 0; i--)
            {
                if (ds.Bars[i].Point[0].Of.X >= x1 - Axis.widthY - weCl && ds.Bars[i].Point[0].Of.X < x1 - Axis.widthY)
                {
                    numBar = i;
                    break;
                }
            }

            for (int i = ds.Bars.Count - 1; i > 0; i--)
            {
               // sum += weCl;
                if (printNum <= numBar && numBar >= numBar - we / weCl )
                {
                    string str = (ds.Bars[printNum].date.Hour + ":" + ds.Bars[printNum].date.Minute).ToString();
                    int startPrint = ds.Bars[printNum].Point[0].Of.X - x1  + weCl/2 - 20;
                    byte h = 0;
                    
                    foreach (var ch in str)
                    {
                        if (ch == ':')
                        {
                            Gl.glBegin(Gl.GL_LINES);
                            foreach (var f in FontPlot.Symbols[ch])
                            {
                                Gl.glVertex2i(startPrint + h * 9 + 2 + f.X, -hi / 2 + 1 + f.Y);
                            }
                            Gl.glEnd();
                            startPrint += 6;
                            continue;
                        }
                        Gl.glBegin(Gl.GL_LINE_STRIP);
                        foreach (var f in FontPlot.Symbols[ch])
                        {
                            
                            Gl.glVertex2i(startPrint + h*9 + f.X, -hi/2 + 1 + f.Y);
                        }
                        Gl.glEnd();
                        h++;
                    }
                    
                }
                printNum -= deltaNum;
                if (printNum < 0)
                    break;
                // numBar--;
            }
        }

        public void Dispose()
        {
            Symbols.Clear();
        }
    }
}
