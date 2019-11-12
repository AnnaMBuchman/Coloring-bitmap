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
    class Filling
    {
        Vector3 N = new Vector3(0, 0, 1);
        Vector3 light = new Vector3(1, 1, 1);
        public Vector3 L = new Vector3(0, 0, 1);
        Vector3 V = new Vector3(0, 0, 1);
        Vector3 R = new Vector3();

        int bitmap_normal=0;
        int bitmap_back = 0;
        Bitmap back;
        Bitmap normal;
        public double kd = 0.5;
        public double ks = 0.5;
        public int m = 40;

        Color c = Color.Green;
        Color cc = Color.Red;
        public bool is_random = false;
        public int filling_mode = 0;

        public bool t_reflectors = false;
        public double reflector_h = 0.5;
        public void R_vector(Vector3 vector,Vector3 light)
        {
            R = Vector3.Add(Vector3.Multiply(Vector3.Multiply(2, Vector3.Multiply(vector, light)), vector), Vector3.Negate(light));
        }
        public Vector3 R_vector2(Vector3 vector, Vector3 light)
        {
           return Vector3.Add(Vector3.Multiply(Vector3.Multiply(2, Vector3.Multiply(vector, light)), vector), Vector3.Negate(light));
        }
        public void Normal_vector(int bitmap,Bitmap b)
        {
            bitmap_normal = bitmap;
            if (bitmap == 0)
            {
                N.X = 0;
                N.Y = 0;
                N.Z = 1;
            }
            else 
                normal = b;            
        }
        public void Bitmap_back(int bitmap, Bitmap b)
        {
            bitmap_back = bitmap;
            if (bitmap != 0)
            {
                back = b;
            }            
        }
        public float Cosinus(Vector3 vector, Vector3 v)
        {
            return (vector.X * v.X + vector.Y * v.Y + vector.Z * v.Z);
        }
        public void Light(int R, int G, int B)
        {
            light = Vector3.Normalize(new Vector3(R, G, B));
        }
        public void bitmap_to_vector(int x,int y,Bitmap bitmap)
        {
            N.X = bitmap.GetPixel(x, y).R - 127;
            N.Y = bitmap.GetPixel(x, y).G - 127;
            N.Z = bitmap.GetPixel(x, y).B;
            N=Vector3.Normalize(N);
        }
        public Vector3 reflector_light(int x,int y,DirectBitmap directBitmap)
        {
            Vector3 r = new Vector3(1, 1, (float)reflector_h);
            Vector3 g = new Vector3(-1, 1, (float)reflector_h);
            Vector3 b= new Vector3(0, -1, (float)reflector_h);

            Vector3 rl = new Vector3(x/directBitmap.Width, y/directBitmap.Height, (float)reflector_h);
            Vector3 gl = new Vector3(-(directBitmap.Width-x)/directBitmap.Width,y / directBitmap.Height, (float)reflector_h);
            Vector3 bl = new Vector3(0,-2 * (y - 127) / directBitmap.Height, (float)reflector_h);
            Vector3 vector = new Vector3(0, 0, 0);
            vector.X = (float) Cosinus(rl,r);
            vector.Y = (float) Cosinus(gl, g);
            vector.Z = (float) Cosinus(bl, b);
            return vector;
        }
        public Vector3 reflector_wersor(int x,int y,DirectBitmap directBitmap,Vector3 L)
        {
            Vector3 L2 = new Vector3(L.X, L.Y, L.Z);

            return L2;
        }
        public Color count_rgb(Color RGB,double kd, double ks, int m,Vector3 N,Vector3 L, Vector3 R, Vector3 V,Vector3 light,int x,int y,DirectBitmap directBitmap)
        {
            Vector3 color_o = new Vector3((float)RGB.R/255, (float)RGB.G/255, (float)RGB.B/255);
            double s1_R = 0;
            double s1_G = 0;
            double s1_B = 0;
            double s2_R = 0;
            double s2_G = 0;
            double s2_B = 0;
            
                s1_R = (kd * light.X * color_o.X * Cosinus(N, L));
                s1_G = (kd * light.Y * color_o.Y * Cosinus(N, L));
                s1_B = (kd * light.Z * color_o.Z * Cosinus(N, L));
                s2_R = (ks * light.X * color_o.X * Math.Pow(Cosinus(V, R), m));
                s2_G = (ks * light.Y * color_o.Y * Math.Pow(Cosinus(V, R), m));
                s2_B = (ks * light.Z * color_o.Z * Math.Pow(Cosinus(V, R), m));
            

            color_o = new Vector3((float)(s1_R + s2_R), (float)(s1_G + s2_G), (float)(s1_B + s2_B));
            //color_o = Vector3.Normalize(color_o);
            if (color_o.Z > 1) color_o.Z = 1;
            if (color_o.X > 1) color_o.X = 1;
            if (color_o.Y > 1) color_o.Y = 1;
            if (color_o.Z < 0) color_o.Z = 0;
            Color col = Color.FromArgb((int)(color_o.X*127)+127,(int)(color_o.Y*127)+127,(int)(color_o.Z*255));
            return col;
        }
        public void Fill_every_triangle(List<List<Point>> sor, DirectBitmap directBitmap,Color color)
        {
            foreach(var list in sor)
            {
                FillTriangle(list, directBitmap,color);
            }
        }
        Random random = new Random();
        public void random_ks_kd_m()
        {
           
            ks = random.NextDouble();
            kd = random.NextDouble();
            m = random.Next(0, 100);
        }
        public void FillTriangle(List<Point> triangle, DirectBitmap directBitmap,Color color)
        {

            if (triangle[1].Y == triangle[2].Y & triangle[1].Y == triangle[0].Y) return;
            if (triangle[0].Y==triangle[1].Y)
            {
                if(is_random)
                {
                    random_ks_kd_m();
                }
                if (filling_mode == 0)
                    Fill_down_triangle(triangle, directBitmap, color);
                else
                    Fill_down_triangle_interpolation(triangle, directBitmap, color);                 

                
            }
            else if(triangle[1].Y==triangle[2].Y)
            {
                if (is_random)
                {
                    random_ks_kd_m();
                }
                if (filling_mode == 0)
                    Fill_top_triangle(triangle, directBitmap, color);
                else
                    Fill_top_triangle_interpolation(triangle, directBitmap, color);

            }            
            else
            {
                if (is_random)
                {
                    random_ks_kd_m();
                }
                List<Point> list_p = new List<Point>();
                list_p.Add(triangle[0]);
                list_p.Add(triangle[1]);
                int dy01 = triangle[1].Y - triangle[0].Y;
                decimal dy02 = triangle[2].Y - triangle[0].Y;
                decimal dx02 = triangle[2].X - triangle[0].X;
                decimal r = (dx02 / dy02) * dy01;
                Point p = new Point((int)(triangle[0].X+r), triangle[1].Y);
                list_p.Add(p);
                Fill_top_triangle(list_p, directBitmap,color);
                list_p.RemoveAt(0);
                list_p.Add(triangle[2]);
                Fill_down_triangle(list_p, directBitmap,color);

            }
           
        }
        public void Fill_top_triangle(List<Point> triangle, DirectBitmap directBitmap,Color color)
        {
            decimal dx01 = triangle[1].X - triangle[0].X;
            decimal dy01 = triangle[1].Y - triangle[0].Y;
            decimal dx02 = triangle[2].X - triangle[0].X;
            decimal dy02 = triangle[2].Y - triangle[0].Y;
            decimal m01 = dx01 / dy01;
            decimal m02 = dx02 / dy02;
            int[] XX = { triangle[0].X, triangle[0].X };
            for (int i=triangle[0].Y+1;i<triangle[2].Y;i++)
            {
                XX[0] = triangle[0].X + (int)((i - triangle[0].Y) * m01);
                XX[1] = triangle[0].X + (int)((i - triangle[0].Y) * m02);
                if (XX[0] > XX[1])
                {
                    int tmp = XX[0];
                    XX[0] = XX[1];
                    XX[1] = tmp;
                }
                for (int k=XX[0];k<XX[1];k++)
                {
                    R_vector(N, L);
                    if(bitmap_back!=0&k<directBitmap.Width&i<directBitmap.Height&k>=0&i>=0)
                    {

                        color = back.GetPixel(k, i);
                    }
                    if(bitmap_normal!=0)
                    {
                        
                        bitmap_to_vector(k, i, normal);
                    }
                    directBitmap.SetPixel(k, i, count_rgb(color, kd, ks, m, N,  L,  R,  V,  light,k,i,directBitmap));
                }
                //e.Graphics.DrawLine(pen, XX[0], i, XX[1], i);
            }
        }
        public void Fill_top_triangle_interpolation(List<Point> triangle, DirectBitmap directBitmap, Color color)
        {
            int y0 = triangle[0].Y;
            int y1 = triangle[1].Y;
            int y2 = triangle[2].Y;
            if (triangle[0].Y >= directBitmap.Height) y0 = directBitmap.Height-1;
            if (triangle[1].Y >= directBitmap.Height) y1 = directBitmap.Height-1;
            if (triangle[2].Y >= directBitmap.Height) y2 = directBitmap.Height-1;
            int X0 = triangle[0].X;
            int X1 = triangle[1].X;
            int X2 = triangle[2].X;
            if (triangle[0].X >= directBitmap.Width) X0 = directBitmap.Width - 1;
            if (triangle[1].X >= directBitmap.Width) X1 = directBitmap.Width - 1;
            if (triangle[2].X >= directBitmap.Width) X2 = directBitmap.Width - 1;

            decimal dx01 = triangle[1].X - X0;
            decimal dy01 = y1- y0;
            decimal dx02 = X2 - X0;
            decimal dy02 = y2 - y0;
            decimal m01 = dx01 / dy01;
            decimal m02 = dx02 / dy02;
            int[] XX = { X0, triangle[0].X };
            List<Color> colors = new List<Color>();
            if (bitmap_back != 0)
            {
               
                directBitmap.SetPixel(X0, y0, back.GetPixel(X0, y0));
                directBitmap.SetPixel(X1, y1, back.GetPixel(X1, y1));
                directBitmap.SetPixel(X2, y2, back.GetPixel(X2, y2));
            }
            else
            {
                directBitmap.SetPixel(X0, y0, color);
                directBitmap.SetPixel(X1, y1, color);
                directBitmap.SetPixel(X2, y2, color);
            }
            colors.Add(directBitmap.GetPixel(X0, y0));
            colors.Add(directBitmap.GetPixel(X1, y1));
            colors.Add(directBitmap.GetPixel(X2, y2));

            
            for (int i = y0 + 1; i < y2; i++)
            {
                XX[0] = X0 + (int)((i - y0) * m01);
                XX[1] = X0 + (int)((i - y0) * m02);
                directBitmap.SetPixel(XX[0], i, Color.FromArgb((int)(((i - y0) * colors[1].R + (y1 - i) * colors[0].R) / dy01), (int)(((i - y0) * colors[1].G + (y1 - i) * colors[0].G) / dy01), (int)(((i - y0) * colors[1].R + (y1 - i) * colors[0].R) / dy01)));
                directBitmap.SetPixel(XX[1], i, Color.FromArgb((int)(((i - y0) * colors[2].R + (y2 - i) * colors[0].R) / dy02), (int)(((i - y0) * colors[2].G + (y2 - i) * colors[0].G) / dy02), (int)(((i - y0) * colors[2].R + (y2 - i) * colors[0].R) / dy02)));

                if (XX[0] > XX[1])
                {
                    int tmp = XX[0];
                    XX[0] = XX[1];
                    XX[1] = tmp;
                }
                Color[] two_colors = { directBitmap.GetPixel(XX[0], i), directBitmap.GetPixel(XX[1], i) };
                int diference = XX[1] - XX[0];
                for (int k = XX[0]; k < XX[1]; k++)
                {
                    R_vector(N, L);
                    if (bitmap_normal != 0)
                    {
                        bitmap_to_vector(k, i, normal);
                    }
                    Color pixel_color = Color.FromArgb((int)(((k - XX[0]) * two_colors[1].R + (XX[1] - k) * two_colors[0].R) / diference), (int)(((k - XX[0]) * two_colors[1].G + (XX[1] - k) * two_colors[0].G) / diference), (int)(((k - XX[0]) * two_colors[1].B + (XX[1] - k) * two_colors[0].B) / diference));
                    if(filling_mode==1)
                        directBitmap.SetPixel(k, i, pixel_color);
                    else
                        directBitmap.SetPixel(k, i, count_rgb(pixel_color, kd, ks, m, N, L, R, V, light,k,i,directBitmap));
                   
                   // directBitmap.SetPixel(k, i, count_rgb(color, kd, ks, m, N, L, R, V, light));
                }
                //e.Graphics.DrawLine(pen, XX[0], i, XX[1], i);
            }
        }
        public void Fill_down_triangle(List<Point> triangle, DirectBitmap directBitmap,Color color)
        {
            decimal dx02 = triangle[2].X - triangle[0].X;
            decimal dy02 = triangle[2].Y - triangle[0].Y;
            decimal dx12 = triangle[2].X - triangle[1].X;
            decimal dy12 = triangle[2].Y - triangle[1].Y;
            decimal m02 = dx02 / dy02;
            decimal m12 = dx12 / dy12;
            int[] XX = { triangle[0].X, triangle[0].X };
            for (int i = triangle[0].Y; i < triangle[2].Y; i++)
            {
                XX[0] = triangle[0].X + (int)((i - triangle[0].Y) * m02);
                XX[1] = triangle[1].X + (int)((i - triangle[0].Y) * m12);
                
                if(XX[0]>XX[1])
                {
                    int tmp = XX[0];
                    XX[0] = XX[1];
                    XX[1]= tmp;                }
                for (int k = XX[0]; k < XX[1]; k++)
                {
                    if (bitmap_back != 0&k < directBitmap.Width & i < directBitmap.Height&k>=0&i>=0)
                    {
                        color = back.GetPixel(k, i);
                    }
                    if (bitmap_normal != 0)
                    {
                        bitmap_to_vector(k, i, normal);
                    }
                    R_vector(N, L);
                    directBitmap.SetPixel(k, i, count_rgb(color, kd, ks, m, N, L, R, V, light,k,i,directBitmap));
                    
                }
               
            }
        }
        public void Fill_down_triangle_interpolation(List<Point> triangle, DirectBitmap directBitmap, Color color)
        {
            int y0 = triangle[0].Y;
            int y1 = triangle[1].Y;
            int y2 = triangle[2].Y;
            if (triangle[0].Y >= directBitmap.Height) y0 = directBitmap.Height - 1;
            if (triangle[1].Y >= directBitmap.Height) y1 = directBitmap.Height - 1;
            if (triangle[2].Y >= directBitmap.Height) y2 = directBitmap.Height - 1;
            int X0 = triangle[0].X;
            int X1 = triangle[1].X;
            int X2 = triangle[2].X;
            if (triangle[0].X >= directBitmap.Width) X0 = directBitmap.Width - 1;
            if (triangle[1].X >= directBitmap.Width) X1 = directBitmap.Width - 1;
            if (triangle[2].X >= directBitmap.Width) X2 = directBitmap.Width - 1;

            decimal dx02 = X2 - X0;
            decimal dy02 = y2 -y0;
            decimal dx12 = X2 - X1;
            decimal dy12 = y2 - y1;
            decimal m02 = dx02 / dy02;
            decimal m12 = dx12 / dy12;
            int[] XX = { X0, triangle[0].X };
            List<Color> colors = new List<Color>();
            if (bitmap_back != 0)
            {

                directBitmap.SetPixel(X0, y0, back.GetPixel(X0, y0));
                directBitmap.SetPixel(X1, y1, back.GetPixel(X1, y1));
                directBitmap.SetPixel(X2, y2, back.GetPixel(X2, y2));
            }
            else
            {
                directBitmap.SetPixel(X0, y0, color);
                directBitmap.SetPixel(X1, y1, color);
                directBitmap.SetPixel(X2, y2, color);
            }
            colors.Add(directBitmap.GetPixel(X0, y0));
            colors.Add(directBitmap.GetPixel(X1, y1));
            colors.Add(directBitmap.GetPixel(X2, y2));


            for (int i = y0 + 1; i < y2; i++)
            {
                XX[0] = X0 + (int)((i - y0) * m02);
                XX[1] = X1 + (int)((i - y0) * m12);
                directBitmap.SetPixel(XX[0], i, Color.FromArgb((int)(((i - y0) * colors[2].R + (y2 - i) * colors[0].R) / dy02), (int)(((i - y0) * colors[2].G + (y2 - i) * colors[0].G) / dy02), (int)(((i - y0) * colors[2].R + (y2 - i) * colors[0].R) / dy02)));
                directBitmap.SetPixel(XX[1], i, Color.FromArgb((int)(((i - y1) * colors[2].R + (y2 - i) * colors[1].R) / dy12), (int)(((i - y1) * colors[2].G + (y2 - i) * colors[1].G) / dy12), (int)(((i - y1) * colors[2].R + (y2 - i) * colors[1].R) / dy12)));

                if (XX[0] > XX[1])
                {
                    int tmp = XX[0];
                    XX[0] = XX[1];
                    XX[1] = tmp;
                }
                Color[] two_colors = { directBitmap.GetPixel(XX[0], i), directBitmap.GetPixel(XX[1], i) };
                int diference = XX[1] - XX[0];
                for (int k = XX[0]; k < XX[1]; k++)
                {
                    R_vector(N, L);
                    if (bitmap_normal != 0)
                    {
                        bitmap_to_vector(k, i, normal);
                    }
                    Color pixel_color = Color.FromArgb((int)(((k - XX[0]) * two_colors[1].R + (XX[1] - k) * two_colors[0].R) / diference), (int)(((k - XX[0]) * two_colors[1].G + (XX[1] - k) * two_colors[0].G) / diference), (int)(((k - XX[0]) * two_colors[1].B + (XX[1] - k) * two_colors[0].B) / diference));
                   if(filling_mode==1)
                       directBitmap.SetPixel(k, i, pixel_color);
                   else
                        directBitmap.SetPixel(k, i, count_rgb(pixel_color, kd, ks, m, N, L, R, V, light,k,i,directBitmap));


                }
            }
        }

    }
}
