using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

// для работы с библиотекой OpenGL 
using Tao.OpenGl;
// для работы с библиотекой FreeGLUT 
using Tao.FreeGlut;
// для работы с элементом управления SimpleOpenGLControl 
using Tao.Platform.Windows;

namespace Platform
{
    
    class Paint
    {
        // ------------------------------ Данные---------------------------------
        private TimeFrame tf;
        public static byte widthCl;
        public static byte hightCl;
       // private UInt16 countBarDraw;
        private Axis yAxis;
        private Axis xAxis;
        public  static double delta;
        //public static int countBars;
        //public static Point start;
        public static double startPrice;
        private Point drawOf, drawTo, localDraw;
        //private Pen objPenLine = new Pen(Color.Orange, 5);
        private DataSeris bars;
        private int dx, dy, loNum, hiNum;
        public PaintPoint BarsDraw;
        private SimpleOpenGlControl Ant;
        private Axis axis;
        // ------------------------------ End Данные---------------------------------

        // Конструкторы
        public Paint(DataSeris ds, SimpleOpenGlControl p, int scX, int scY)
        {
            Ant = p;
            tf = ds.TFrame;
            widthCl = Convert.ToByte(scX);
            hightCl = Convert.ToByte(scY);
           // countBarDraw = Convert.ToUInt16((p.Size.Width - Axis.widthY)/widthCl);
            //countBars = ds.Bars.Count;
            delta = ds.deltaTick;
           // start.X = Ant.Size.Width - Axis.widthY - widthCl;
           // start.Y = (Ant.Height - Axis.highX)/2;
            startPrice = ds.Bars.Last().Value.Close;
            axis = new Axis(Ant);
           //bars = ds;
        }
        // End конструктор

        public void Plot(PaintPoint ds, int X0, int Y0, double scaleX, double scaleY)
        {
            //paint = new Paint(Rts, pnlCh);
            //paint.Plot(); //, pnlCh);
            //MessageBox.Show(pnlCh.Controls.Count.ToString());
            Ant.MakeCurrent();
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
            Gl.glBegin(Gl.GL_QUADS);//GL_LINES);
            // далее мы рисуем координатные оси и стрелки на их концах 
            foreach (var bar in BarsDraw.Bars)
            {
                foreach (var k in bar.Point)
                {
                    Gl.glVertex2d(k.Of.X, k.Of.Y);
                    Gl.glVertex2d(k.Of.X, k.Of.Y+ hightCl-1);
                    Gl.glVertex2d(k.To.X, k.To.Y+ hightCl-1);
                    Gl.glVertex2d(k.To.X, k.To.Y);
                }
            }

            // завершаем режим рисования 
            Gl.glEnd();
            //axis.PlotAxis(X0,Y0,scaleX,scaleY);

            Gl.glPopMatrix();
            Gl.glFlush();
            Ant.Invalidate();

            axis.PlotAxis1(ds, X0, Y0, scaleX, scaleY);
            //FontPlot.Plot(ds, startPrice, delta, Ant.Height, Ant.Width, Paint.hightCl, Paint.widthCl, X0, Y0);
            Ant.Invalidate();
        }
    }
}
