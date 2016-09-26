using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// для работы с библиотекой OpenGL 
using Tao.OpenGl;
// для работы с библиотекой FreeGLUT 
using Tao.FreeGlut;
// для работы с элементом управления SimpleOpenGLControl 
using Tao.Platform.Windows;

/*
    Отрисовка осей, цены и даты 
*/

namespace Platform
{
    class Axis
    {
        // ------------------------------ Данные---------------------------------
        public const byte widthY = 50;
        public const byte highX = 20;

        private int widthWindow, hightWindow;
        private double startPrice, delta;
        private int x0, y0,x,y;
        private double sX, sY, sx, sy;

        // ------------------------------ End Данные---------------------------------
        public Axis(SimpleOpenGlControl sm)
        {
            widthWindow = sm.Width;
            hightWindow = sm.Height;
            startPrice = Paint.startPrice;
            delta = Paint.delta;
            x0 = widthWindow;
            y0 = hightWindow/2;
            sX = 1;
            sY = 1;
            sx = 1;

        }

        public void PlotAxis(DataSeris ds, int X0, int Y0, double scaleX, double scaleY)
        {
            x += x0 - X0;
            y += y0 - Y0;
            sx = sx + sX - scaleX;
            sy =  scaleY;

            Gl.glColor3f(0.882f, 0.572f, 0.094f);
            Gl.glTranslated(x, y, 0);
            Gl.glScaled(sx,sy,1);
            Gl.glScaled(sx, sy, 1);
            Gl.glLineWidth(1);
            Gl.glBegin(Gl.GL_LINES);
            // далее мы рисуем координатные оси и стрелки на их концах 
            //Y
            Gl.glVertex2d(-widthY, hightWindow / 2);
            Gl.glVertex2d(-widthY, -hightWindow / 2 + highX);

            //X
            Gl.glVertex2d(-widthWindow, -hightWindow / 2 + highX);
            Gl.glVertex2d(-widthY, -hightWindow / 2 + highX);
            x0 = X0;
            y0 = Y0;
            sX = scaleX;
            sY = scaleY;
            // завершаем режим рисования 
            Gl.glEnd();
        }
        public void PlotAxis1(PaintPoint ds, int X0, int Y0, double scaleX, double scaleY)
        {
            x += x0 - X0;
            y += y0 - Y0;
          //  sx = sx + sX - scaleX;
          //  sy = sy + sY - scaleY;
            // очищение текущей матрицы 
            Gl.glLoadIdentity();

            // установка черного цвета 
           // Gl.glColor3f(100, 0, 0);

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
            Gl.glVertex2d(-widthY, hightWindow / 2);
            Gl.glVertex2d(-widthY, -hightWindow / 2 + highX);

            //X
            Gl.glVertex2d(-widthWindow, -hightWindow / 2 + highX);
            Gl.glVertex2d(-widthY, -hightWindow / 2 + highX);

            x0 = X0;
            y0 = Y0;
          //  sX = scaleX;
          //  sY = scaleY;
            // завершаем режим рисования 
            Gl.glEnd();
            FontPlot.Plot(ds, startPrice, delta,  hightWindow, widthWindow, Paint.hightCl, Paint.widthCl, X0, Y0);


            // Gl.glBegin(Gl.GL_TEXCOORD1_BIT_PGI);
            // Gl.glRasterPos3d(-widthY, 0, 1);
            // .glutBitmapString(, startPrice.ToString());
            // Gl.glEnd();

            Gl.glPopMatrix();
            Gl.glFlush();
        }
    }
}
