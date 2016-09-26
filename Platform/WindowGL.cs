using System;
using System.Drawing;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tao.FreeGlut;
using Tao.OpenGl;
using Tao.Platform.Windows;
using Timer = System.Windows.Forms.Timer;

namespace Platform
{
    internal class WindowGL
    {
        #region Глобальные переменные
        public int widthY = 50;
        public int highX = 20;
        public int wiCl = 20;
        public int hiCl = 10;
        //public static int volFilter;                      // Фильтр объема для суммирования тиков в кластерах (Не суммируются меньше данного значения) 
        private int X0, Y0, ScaleX, ScaleY;                 // Координаты смещения
        private int ScreenW, ScreenH;                       // Размеры окна вывода
        private SimpleOpenGlControl Chart;                  // Окно для вывода OpenGl
        public DataSeries Sec;                              // Бумага: Название, тф, величина тика, OHLC, дата
      //  public static PaintPoint BarsDraw;                // Бумага переведенная в координаты, для более быстрого доступа
        public DrawSeries DrawSer;
        public double PriceOpen;
        #endregion
        enum Selected { Chart, X, Y, None }
        private int xS, yS;
        private int scX, scY;
        private Selected select;
        private WindowCluster Window;
        private Timer timer1;
        private bool real;
        

        #region Конструкторы
        public WindowGL(SimpleOpenGlControl Ch, WindowCluster wcl, Timer tm, bool reall)
        {
            real = reall;
            timer1 = tm;
            Window = wcl;
            select = Selected.None;
            Chart = Ch;

            Chart.Resize += delegate(object sender, EventArgs args) // Событие изменения размеров окна вывода
            {
                GluLoad();
                if (DrawSer != null && DrawSer.Load)
                {
                 //   BarsDraw.Dispose();
                 //   BarsDraw.Add(Sec);
                    Plot();
                }
            };

            #region События от мышки
            // Отслеживаем где была нажата кнопка мыши
            Chart.MouseDown += delegate(object sender, MouseEventArgs e)
            {
                xS = e.X;
                yS = e.Y;
                if (xS > Chart.Width - Axis.widthY)
                {
                    if (yS < Chart.Height - Axis.highX)
                    {
                        select = Selected.Y;
                        scY = yS;
                    }
                }
                else
                {
                    if (yS > Chart.Height - Axis.highX)
                    {
                        if (xS < Chart.Width - Axis.widthY)
                        {
                            select = Selected.X;
                            scX = xS;
                        }
                    }
                    else
                    {
                        if (timer1.Enabled)
                            timer1.Enabled = false;
                        select = Selected.Chart;
                    }
                }
            };
            Chart.MouseUp += delegate(object sender, MouseEventArgs e)
            {
                if(select == Selected.Chart)
                    //if (real)
                        timer1.Enabled = true;
                select = Selected.None;
            };
            // Перемещение мышки
            Chart.MouseMove += delegate(object sender, MouseEventArgs e)
            {
                switch (select)
                {
                    case Selected.None:     // Выбора нет  
                        break;
                    case Selected.Chart:    // По графику
                        X0 += e.Location.X - xS;
                        Y0 -= e.Location.Y - yS;
                        xS = e.Location.X;
                        yS = e.Location.Y;
                        if (DrawSer != null && DrawSer.Load)
                            Plot();
                        break;
                    case Selected.X:        // По оси времени
                        {
                            if (DrawSer.Load)
                            {
                                wiCl -= (e.Location.X - scX);
                                if (wiCl > 50)
                                    wiCl = 50;
                                else if (wiCl < 2)
                                    wiCl = 2;
                                else
                                {
                                    scX = e.Location.X;
                                    PlotResize();
                                }
                            }
                        }
                        break;
                    case Selected.Y:        // По оси цены
                        {
                            if (DrawSer.Load)//BarsDraw.Load)
                            {
                                hiCl -= (e.Location.Y - scY);
                                if (hiCl > 20)
                                    hiCl = 20;
                                else if (hiCl < 2)
                                    hiCl = 2;
                                else
                                {
                                    scY = e.Location.Y;
                                    PlotResize();
                                }
                            }
                        }
                        break;
                    default:
                        break;
                }
            };
            #endregion
        }
        #endregion

        // Новые серии для отрисовки
        public void ReloadSeries(DataSeries ds, DrawSeries drs, double open)
        {
            Sec = ds;
            DrawSer = drs;
            PriceOpen = open;
        }

        // Изменение масштаба
        public void ReloadSeriesOpen(double open)
        {
            PriceOpen = open;
        }

        #region Переменные для методов
        private int x0, y0, x, y;                           // Дельта смещения
        private double PriceForScaleY;                      // Цена от которой будет проискодить скалирование (середина оси цен)
        private int XPlot;                                  // Бар от которого будет проискодить скалирование (середина оси времени)
        #endregion

        #region Методы

        public void Load()
        {
            // инициализация режима экрана 
            Glut.glutInitDisplayMode(Glut.GLUT_RGB | Glut.GLUT_DOUBLE);

            // установка цвета очистки экрана (RGBA) 
            Gl.glClearColor(0, 0, 0, 1);

            // установка порта вывода 
            Gl.glViewport(0, 0, Chart.Width, Chart.Height);

            // активация проекционной матрицы 
            Gl.glMatrixMode(Gl.GL_PROJECTION);
            // очистка матрицы 
            Gl.glLoadIdentity();

            // определение параметров настройки проекции в зависимости от размеров сторон элемента AnT. 
            ScreenW = Chart.Width;//30.0 * (float)AnT.Width / (float)AnT.Height;
            ScreenH = Chart.Height;//30.0;
            Glu.gluOrtho2D(0.0, ScreenW, 0.0, ScreenH);

            // установка объектно-видовой матрицы 
            Gl.glMatrixMode(Gl.GL_MODELVIEW);
            X0 = 0;//Chart.Width;
            Y0 = 0;//Chart.Height / 2;
            ScaleX = 1;
            ScaleY = 1;
        }
        

        private void PlotResize()
        {
            if (DrawSer.Load && Sec.Load)
            {
                DrawSer.ClSize(wiCl,hiCl);
                DrawSer.Resize(Sec);
                if (select == Selected.Y)
                    Y0 = -Convert.ToInt32((PriceForScaleY - Sec.Bars.First().Value.Open)/Sec.deltaTick)*hiCl +
                         Convert.ToInt32(ScreenH/2);
                if (select == Selected.X)
                    X0 = -XPlot*wiCl + ScreenW/2;
                Plot(); // Рисуем 
            }
        }
     
        public void LookSet()
        {
                Y0 = -Convert.ToInt32((Sec.Bars.Last().Value.Open - Sec.Bars.First().Value.Open) / Sec.deltaTick) * hiCl + Convert.ToInt32(ScreenH / 2);
                X0 = -Sec.Bars.Count * wiCl - Axis.widthY - wiCl + ScreenW;
                x0 = 0;
                y0 = 0;
            x = 0;
            y = 0;
        }
        #endregion

        public bool PlotSer = false;

        #region Отображение Графика, осей и печать символов цен и времени
        public void Plot()
        {
            if (!Window.add)
            {
                PlotSer = true;
                //paint = new Paint(Rts, pnlCh);
                //paint.Plot(); //, pnlCh);
                //MessageBox.Show(pnlCh.Controls.Count.ToString());
                Chart.MakeCurrent();
                // очистка буфера цвета и буфера глубины 
                Gl.glClear(Gl.GL_COLOR_BUFFER_BIT | Gl.GL_DEPTH_BUFFER_BIT);

                // очищение текущей матрицы 
                Gl.glLoadIdentity();

                // установка черного цвета 
                Gl.glColor3f(100, 0, 0);

                // помещаем состояние матрицы в стек матриц 
                Gl.glPushMatrix();

                // выполняем перемещение в пространстве по осям X и Y 
                Gl.glTranslated(X0, Y0, 0);
                //  Gl.glScaled(scaleX, scaleY, 1);

                // Gl.glLineWidth(1);
                Gl.glBegin(Gl.GL_QUADS); //GL_LINES);
                // далее мы рисуем координатные оси и стрелки на их концах 
                int l = hiCl - 1;
                int From = -X0 - wiCl;
                int To = -X0 + ScreenW;
              //  string str;
                foreach (var ds in DrawSer.Bars)
                {
                    var gfgf = ds.Value.Price.ElementAt(0).Value[0];
                    if (gfgf >= From && gfgf <= To)
                    {
                        foreach (var pr in ds.Value.Price)
                        {
                            Gl.glVertex2d(pr.Value[0], pr.Value[2]);
                            Gl.glVertex2d(pr.Value[0], pr.Value[2] + l);
                            Gl.glVertex2d(pr.Value[1], pr.Value[2] + l);
                            Gl.glVertex2d(pr.Value[1], pr.Value[2]);
                        }
                    }
                }
                // завершаем режим рисования 
                Gl.glEnd();

                // установка черного цвета 
                Gl.glColor3f(0, 0, 50);
                Gl.glBegin(Gl.GL_POLYGON);
                Gl.glVertex2d(DrawSer.lastPriceX, DrawSer.lastPriceY);
                Gl.glVertex2d(DrawSer.lastPriceX + 10, DrawSer.lastPriceY + 10);
                Gl.glVertex2d(DrawSer.lastPriceX + 80, DrawSer.lastPriceY + 10);
                Gl.glVertex2d(DrawSer.lastPriceX + 80, DrawSer.lastPriceY - 10);
                Gl.glVertex2d(DrawSer.lastPriceX + 10, DrawSer.lastPriceY - 10);
                Gl.glEnd();

                Gl.glColor3f(1, 1, 1);
                string strr = DrawSer.lastPrice.ToString();
                int i = 0;
                foreach (var ch in strr)
                {
                    Gl.glBegin(Gl.GL_LINE_STRIP);
                    
                    foreach (var f in FontPlot.Symbols[ch])
                    {
                        //  Glut.glutStrokeCharacter(Glut.GLUT_STROKE_ROMAN, '3');
                        Gl.glVertex2i(DrawSer.lastPriceX + 15 + i * 9 + f.X, DrawSer.lastPriceY - 5 + Convert.ToInt32(f.Y));
                    }
                    Gl.glEnd();
                    i++;
                }
                Gl.glColor3f(0.172f, 0.2f, 0.215f);
                Gl.glBegin(Gl.GL_LINE_STRIP);
                    Gl.glVertex2d(DrawSer.lastPriceX, DrawSer.lastPriceY);
                    Gl.glVertex2d(0, DrawSer.lastPriceY);
                Gl.glEnd();

                // PlotNum();
                Gl.glPopMatrix();
                Gl.glFlush();
                Chart.Invalidate();
              //  PlotNum();
                PlotAxis();
                PlotSer = false;
            }
        }

        // функция визуализации текста 
        // Вердикт, медленно
        private void PrintText2D(float x, float y, string text)
        {

            // устанавливаем позицию вывода растровых символов 
            // в переданных координатах x и y. 
            Gl.glRasterPos2f(x, y);

            // в цикле foreach перебираем значения из массива text, 
            // который содержит значение строки для визуализации 
            foreach (char char_for_draw in text)
            {
                // символ C визуализируем с помощью функции glutBitmapCharacter, используя шрифт GLUT_BITMAP_9_BY_15. 
                Glut.glutBitmapCharacter(Glut.GLUT_BITMAP_HELVETICA_10, char_for_draw);
            }

        }

        private void PlotNum()
        {

            // очищение текущей матрицы 
            Gl.glLoadIdentity();

            // установка черного цвета 
            Gl.glColor3f(1, 1, 1);

            // помещаем состояние матрицы в стек матриц 
            Gl.glPushMatrix();

            // выполняем перемещение в пространстве по осям X и Y 
            Gl.glTranslated(X0, Y0, 0);

            // Gl.glLineWidth(1);
          //  Gl.glBegin(Gl.GL_QUADS);//GL_LINES);
            // далее мы рисуем координатные оси и стрелки на их концах 
            int l = hiCl - 1;
            int From = -X0 - wiCl;
            int To = -X0 + ScreenW;
            string str;
            double dob = 1;//hiCl/8;
            foreach (var ds in DrawSer.Bars)
            {
                var gfgf = ds.Value.Price.ElementAt(0).Value[0];
                if (gfgf >= From && gfgf <= To)
                {
                    foreach (var pr in ds.Value.Price)
                    {

                        str = (pr.Value[3]).ToString();
                    //    PrintText2D(pr.Value[0], pr.Value[2], str);
                        int i = 0;
                        foreach (var ch in str)
                        {
                            Gl.glBegin(Gl.GL_LINE_STRIP);
                            foreach (var f in FontPlot.Symbols[ch])
                            {
                              //  Glut.glutStrokeCharacter(Glut.GLUT_STROKE_ROMAN, '3');
                                Gl.glVertex2i(pr.Value[0] + i * 9 + f.X, pr.Value[2] + Convert.ToInt32(f.Y * dob));
                            }
                            Gl.glEnd();
                            i++;
                        }
                    }
                }
            }

            Gl.glPopMatrix();
            Gl.glFlush();
            Chart.Invalidate();
         //   PlotAxis();
          //  PlotNum();
        }

        // Отображение Осей
        private void PlotAxis()
        {
            x += x0 - X0;
            y += y0 - Y0;
            // очищение текущей матрицы 
            Gl.glLoadIdentity();

            // помещаем состояние матрицы в стек матриц 
            Gl.glPushMatrix();

            Gl.glColor3f(0.882f, 0.572f, 0.094f);
            Gl.glTranslated(X0 + x, Y0 + y, 0);
            // Gl.glScaled(sx, sy, 1);
            // Gl.glScaled(sx, sy, 1);
            Gl.glLineWidth(1);
            Gl.glBegin(Gl.GL_LINES);
            // далее мы рисуем координатные оси и стрелки на их концах 
            //Y
            Gl.glVertex2d(ScreenW - widthY, ScreenH);
            Gl.glVertex2d(ScreenW - widthY, highX);

            //X
            Gl.glVertex2d(0, highX);
            Gl.glVertex2d(ScreenW - widthY, highX);

            x0 = X0;
            y0 = Y0;
            //  sX = scaleX;
            //  sY = scaleY;
            // завершаем режим рисования 
            Gl.glEnd();
            // FontPlot.Plot(ds, startPrice, delta, hightWindow, widthWindow, Paint.hightCl, Paint.widthCl, X0, Y0);
            PlotAxisFont();

            Gl.glPopMatrix();
            Gl.glFlush();
            Chart.Invalidate();
        }

        // Отображение данных осей
        private void PlotAxisFont()
        {
            // Ось Цен
            int stPaint = y;                                                                // Смещение = Стартовая точка отрисовки видимой области графика
            double stPrice = (stPaint/hiCl)*Sec.deltaTick + PriceOpen;//Sec.Bars.ElementAt(0).Value.Open;    // Цена в нижней точке видимой области
            if (select != Selected.Y)
                PriceForScaleY = stPrice + ((ScreenH / 2) / hiCl) * Sec.deltaTick;              // Запоминаем центральную цену для удобного скейлинга по оси цен

            int dobavka = -y % hiCl;                                       // Пикселей до следующего кластера цены
            dobavka -= hiCl;
            dobavka += (hiCl - 10) / 2;                                                     // Цена по середине кластера
            double delPrice = Sec.deltaTick;
            if (hiCl >= 10)
                delPrice = Sec.deltaTick / 2;
            else if (hiCl >= 7 && hiCl < 10)
                delPrice = Sec.deltaTick * 2;
            else if (hiCl >= 5 && hiCl < 7)
                delPrice = Sec.deltaTick * 4;
            else if (hiCl >= 3 && hiCl < 5)
                delPrice = Sec.deltaTick * 5;
            else if (hiCl >= 2 && hiCl < 3)
                delPrice = Sec.deltaTick * 10;
            else if (hiCl < 2)
                delPrice = Sec.deltaTick * 10;

            Gl.glColor3f(1.0f, 1.0f, 1.0f);

            double PaintDo = stPaint + ScreenH;                                             // Крайняя видимая верхняя цена
            double price = stPrice - Sec.deltaTick;
            int addk = ScreenW - widthY + 2;
            string str;
            for (double j = stPaint; j <= PaintDo; j += hiCl)
            {
                dobavka += hiCl;
                price += Sec.deltaTick;
                if ((price) % delPrice == 0)
                {
                    str = (price).ToString();
                    Byte i = 0;
                    foreach (var ch in str)
                    {
                        Gl.glBegin(Gl.GL_LINE_STRIP);
                        foreach (var f in FontPlot.Symbols[ch])
                        {
                            Gl.glVertex2i(addk + i * 9 + f.X, dobavka + f.Y);
                        }
                        Gl.glEnd();
                        i++;
                    }
                }
            }
            // Ось времени

            int deltaNum = 60 / wiCl + 1;                       // К-во баров для пропуска между отрисовкой времени
            int numPrint = 0;                                   // Первый отрисованный индекс
            stPaint = x;
            int numb = stPaint / wiCl;
            for (int i = 0; i < DrawSer.Bars.Count - 1; i += deltaNum)
            {
                if (i <= numb && i + deltaNum >= numb)
                {
                    numPrint = i;
                    break;
                }
            }
            if (select != Selected.X)
                XPlot = numPrint + ((ScreenH - wiCl - widthY))/wiCl;
            int Yconst = -hiCl / 2 + 6;

            int endPrint = numPrint + ScreenW/wiCl + deltaNum;

            int ii = numPrint;
            
                    int jj = 0;
                    foreach (var bar in DrawSer.Bars)
                    {
                        if (jj != ii)
                        {
                            jj++;
                            continue;
                        }
                        else
                        {
                            str = bar.Key.Hour + ":" + bar.Key.Minute;
                            int startPrint = bar.Value.Price.ElementAt(0).Value[0] - x + wiCl / 2 - 20;
                           // int startPrint = jj*wiCl - x + wiCl/2 - 20;
                           int h = 0;

                            foreach (var ch in str)
                            {
                                if (ch == ':')
                                {
                                    Gl.glBegin(Gl.GL_LINES);
                                    foreach (var f in FontPlot.Symbols[ch])
                                    {
                                        Gl.glVertex2i(startPrint + h*9 + 2 + f.X, Yconst + f.Y);
                                    }
                                    Gl.glEnd();
                                    startPrint += 6;
                                    continue;
                                }
                                Gl.glBegin(Gl.GL_LINE_STRIP);
                                foreach (var f in FontPlot.Symbols[ch])
                                {

                                    Gl.glVertex2i(startPrint + h*9 + f.X, Yconst + f.Y);
                                }
                                Gl.glEnd();
                                h++;
                            }
                            jj++;
                            ii += deltaNum;
                            if (ii >= endPrint || ii > DrawSer.Bars.Count - 1) break;
                        }
                    }          
        }
        #endregion

        // Инициализация OpenGl
        private void GluLoad()
        {
            // инициализация режима экрана 
            Glut.glutInitDisplayMode(Glut.GLUT_RGB | Glut.GLUT_SINGLE);

            // установка цвета очистки экрана (RGBA) 
            Gl.glClearColor(0, 0, 0, 1);

            // установка порта вывода 
            Gl.glViewport(0, 0, Chart.Width, Chart.Height);

            // активация проекционной матрицы 
            Gl.glMatrixMode(Gl.GL_PROJECTION);
            // очистка матрицы 
            Gl.glLoadIdentity();

            // определение параметров настройки проекции в зависимости от размеров сторон элемента AnT. 
            ScreenW = Chart.Width; //30.0 * (float)AnT.Width / (float)AnT.Height;
            ScreenH = Chart.Height; //30.0;
            Glu.gluOrtho2D(0.0, ScreenW, 0.0, ScreenH);


            // установка объектно-видовой матрицы 
            Gl.glMatrixMode(Gl.GL_MODELVIEW);
            //  X0 = AnT.Width;
            //  Y0 = AnT.Height/2;
        }
    }
}
