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
    class Sort_ver
    {
        public List<List<Point>> Sort_vertexes(List<List<Point>> points, int triangle_x,int triangle_y)
        {
            List<List<Point>> srt = new List<List<Point>>();
            List<Point> tmp = new List<Point>();
            for(int i=0;i<triangle_y;i++)
            {
                
                for(int j=0;j<triangle_x;j++)
                {
                    srt.Add(new List<Point>());
                    srt.Add(new List<Point>());

                    if (points[i][j+1].Y<points[i+1][j].Y)
                    {
                        srt[2 * j+i*triangle_x*2].Add(points[i][j+1]);
                        srt[2 * j+1+i * triangle_x * 2].Add(points[i][j + 1]);
                        srt[2 * j+ i * triangle_x * 2].Add(points[i+1][j]);
                        srt[2 * j + 1+ i * triangle_x * 2].Add(points[i+1][j]);
                    }
                    else
                    {
                        srt[2 * j+ i * triangle_x * 2].Add(points[i + 1][j]);
                        srt[2 * j + 1+ i * triangle_x * 2].Add(points[i + 1][j]);
                        srt[2 * j+ i * triangle_x * 2].Add(points[i][j + 1]);
                        srt[2 * j + 1+ i * triangle_x * 2].Add(points[i][j + 1]);                        
                    }
                    if(srt[2 * j+ i * triangle_x * 2][1].Y<=points[i][j].Y)
                    {
                        srt[2 * j+ i * triangle_x * 2].Add(points[i][j]);
                    }
                    else if(srt[2*j+ i * triangle_x * 2][0].Y<=points[i][j].Y)
                    {
                        tmp = srt[2 * j+ i * triangle_x * 2];
                        srt[2 * j+ i * triangle_x * 2] =new List<Point>();
                        srt[2 * j+ i * triangle_x * 2].Add(tmp[0]);
                        srt[2 * j+ i * triangle_x * 2].Add(points[i][j]);
                        srt[2 * j+ i * triangle_x * 2].Add(tmp[1]);
                    }
                    else
                    {
                        tmp = srt[2 * j+ i * triangle_x * 2];
                        srt[2 * j+ i * triangle_x * 2] =new List<Point>();
                        srt[2 * j+ i * triangle_x * 2].Add(points[i][j]);
                        srt[2 * j+ i * triangle_x * 2].Add(tmp[0]);
                        srt[2 * j+ i * triangle_x * 2].Add(tmp[1]);
                    }
                    if (srt[2 * j+1+ i * triangle_x * 2][1].Y <= points[i+1][j+1].Y)
                    {
                        srt[2 * j+1+ i * triangle_x * 2].Add(points[i+1][j+1]);
                    }
                    else if (srt[2 * j+1+ i * triangle_x * 2][0].Y <= points[i+1][j+1].Y)
                    {
                        tmp = srt[2 * j + 1+i *triangle_x*2];
                        srt[2 * j + 1+i *triangle_x*2]=new List<Point>();
                        srt[2 * j + 1+i *triangle_x*2].Add(tmp[0]);
                        srt[2 * j + 1+i *triangle_x*2].Add(points[i+1][j+1]);
                        srt[2 * j + 1+i *triangle_x*2].Add(tmp[1]);
                    }
                    else
                    {
                        tmp = srt[2 * j + 1+i *triangle_x*2];
                        srt[2 * j + 1+i *triangle_x*2] = new List<Point>();
                        srt[2 * j + 1+i *triangle_x*2].Add(points[i+1][j+1]);
                        srt[2 * j + 1+i *triangle_x*2].Add(tmp[0]);
                        srt[2 * j + 1+i *triangle_x*2].Add(tmp[1]);
                    }
                }
            }
            return srt;

        }
    }
}
