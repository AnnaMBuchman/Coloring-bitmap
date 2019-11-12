using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Projekt2GK
{
    public class Drawing
    {
        Color c = Color.AntiqueWhite;
        
        public void DrawEveryLine(int triangle_x,int triangle_y,List<List<Point>> points, PaintEventArgs e)
        {
            for (int j = 0; j <= triangle_x; j++)
            {
                for (int i = 0; i < triangle_y; i++)
                {
                    BresenhamLine(points[i][j].X, points[i][j].Y, points[i+1][j].X, points[i+1][j].Y, c, e);
                }
            }
            
            for (int j = 0; j <= triangle_y; j++)
            {
                for (int i = 0; i < triangle_x; i++)
                {
                    BresenhamLine(points[j][i].X, points[j][i].Y, points[j][i+1].X, points[j][i+1].Y, c, e);
                }
            }
            for(int i=0;i< triangle_y;i++)
            {
                for(int j=1;j<=triangle_x;j++)
                {
                    BresenhamLine(points[i][j].X, points[i][j].Y, points[i+1][j-1].X, points[i+1][j-1].Y, c, e);
                }
            }
        }
        private static void PutPixel(int x, int y, Color color, PaintEventArgs e)
        {
            using (SolidBrush brush = new SolidBrush(color))
                e.Graphics.FillRectangle(brush, x, y, 1, 1);
        }
        public void BresenhamLine(int x1, int y1, int x2, int y2, Color color, PaintEventArgs e)
        {
            // zmienne pomocnicze
            int d, dx, dy, ai, bi, xi, yi;
            int x = x1, y = y1;
            // ustalenie kierunku rysowania
            if (x1 < x2)
            {
                xi = 1;
                dx = x2 - x1;
            }
            else
            {
                xi = -1;
                dx = x1 - x2;
            }
            // ustalenie kierunku rysowania
            if (y1 < y2)
            {
                yi = 1;
                dy = y2 - y1;
            }
            else
            {
                yi = -1;
                dy = y1 - y2;
            }
            // pierwszy piksel
            PutPixel(x, y, color, e);
            //   glVertex2i(x, y);
            // oś wiodąca OX
            if (dx > dy)
            {
                ai = (dy - dx) * 2;
                bi = dy * 2;
                d = bi - dx;
                // pętla po kolejnych x
                while (x != x2)
                {
                    // test współczynnika
                    if (d >= 0)
                    {
                        x += xi;
                        y += yi;
                        d += ai;
                    }
                    else
                    {
                        d += bi;
                        x += xi;
                    }
                    PutPixel(x, y, color, e);
                    //   glVertex2i(x, y);
                }
            }
            // oś wiodąca OY
            else
            {
                ai = (dx - dy) * 2;
                bi = dx * 2;
                d = bi - dy;
                // pętla po kolejnych y
                while (y != y2)
                {
                    // test współczynnika
                    if (d >= 0)
                    {
                        x += xi;
                        y += yi;
                        d += ai;
                    }
                    else
                    {
                        d += bi;
                        y += yi;
                    }
                    PutPixel(x, y, color, e);
                    //  glVertex2i(x, y);
                }
            }
        }
    }
}
