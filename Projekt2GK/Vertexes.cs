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
    public class Vertexes
    {
        int size = 3;
        public int[] looking_for_vertex(Point point, int triangle_x, int triangle_y,List<List<Point>> points)
        {
            int[] tab = { -1, -1 };            //which graph and which vertex in graph

            for (int j = 0; j <= triangle_y; j++)
                for (int i = 0; i <= triangle_x; i++)
                {
                    if (point.X >= points[j][i].X - size && point.X <= (points[j][i].X + size) && point.Y >= points[j][i].Y - size && point.Y <= (points[j][i].Y + size))
                    {
                        tab[0] = j;
                        tab[1] = i;
                        break;
                    }
                }

            return tab;
        }
        public List<List<Point>> AddPoints(int triangle_x, int triangle_y,int width, int height)
        {
            List<List<Point>> points = new List<List<Point>>();
            double devide_x = width / triangle_x;
            double devide_y = height / triangle_y;
            for (int i = 0; i < triangle_y; i++)
            {
                points.Add(new List<Point>());
                for (int j = 0; j < triangle_x; j++)
                {
                    points[i].Add(new Point((int)Math.Floor(devide_x * j), (int)Math.Floor(devide_y * i)));
                }
                points[i].Add(new Point((int)Math.Floor(devide_x * triangle_x), (int)Math.Floor(devide_y * i)));
            }
            points.Add(new List<Point>());
            for (double j = 0; j < triangle_x; j++)
            {
                points[triangle_y].Add(new Point((int)Math.Floor(devide_x * j), (int)Math.Floor(devide_y *triangle_y)));
            }
            points[triangle_y].Add(new Point((int)Math.Floor(devide_x * triangle_x), (int)Math.Floor(devide_y * triangle_y)));
            return points;
        }
       
    }
}
