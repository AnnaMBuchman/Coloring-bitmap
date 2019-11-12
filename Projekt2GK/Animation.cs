using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Numerics;
using System.Windows;
using System.Threading;

namespace Projekt2GK
{
    class Animation
    {
        Color sun_color = Color.Red;
        int siz = 5;
        public int x = 0,z=0, y = 0;
       public void Draw_sun(int x, int y,PaintEventArgs e,int width, int height)
        {
            int i = x - siz;
            int j = y - siz;
            if (x - siz < 0) i = 0;
            if (y - siz < 0) j = 0;
            for (;i<x+siz&i<width;i++)
            {
                if (y - siz < 0) j = 0;
                else
                    j = y - siz;
                for (;j<y+siz&j<height;j++)
                {
                    PutPixel(i, j, sun_color, e);
                }
            }            
        }
        private static void PutPixel(int x, int y, Color color, PaintEventArgs e)
        {
            using (SolidBrush brush = new SolidBrush(color))
                e.Graphics.FillRectangle(brush, x, y, 1, 1);
        }
        public void Sleep_for(int sek)
        {
            Thread.Sleep(sek * 100);
        }
        public int Count_S(int x, int r)
        {
            int x0 = x;
            if(x>r)
            {
                x0 = 2*r - x; 
            }
            z= (int)Math.Sqrt(r * r - (r-x) * (r-x));
            this.x = x;
            return z;
        }
    }
}
