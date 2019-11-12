using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Numerics;

namespace Projekt2GK
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        
        int triangle_x = 6;
        int triangle_y = 6;
        List<List<Point>> points = new List<List<Point>>();
        Color c = Color.AliceBlue;
        int deaf_height;
        int deaf_weight;
        int move_vertex = 0;
        int[] vertex_found = new int[] { -1, -1 };
       
        List<List<Point>> Sort_Vertex = new List<List<Point>>();
        Vertexes vertexes = new Vertexes();
        Sort_ver sort_Ver = new Sort_ver();
        Filling Filling = new Filling();
        Animation Animation = new Animation();
        DirectBitmap DirectBitmap;
        Color color = Color.FromArgb(0, 200, 0);
        Color light_color = Color.FromArgb(255, 255, 255);
        
        bool colord = false;
        Bitmap normalmap1;
        Bitmap normalmap2;
        Bitmap backbitmap1;
        Bitmap backbitmap2;
        Bitmap copy;

        bool animation_go = false;
        System.Timers.Timer timer = new System.Timers.Timer();

        private void Form1_Load(object sender, EventArgs e)
        {
            normalmap1 = new Bitmap(Image.FromFile(Application.StartupPath + @"\NormalMap\normal_map.jpg"),new Size(pictureBox1.Width+2,pictureBox1.Height+2));
            normalmap2 = new Bitmap(Image.FromFile(Application.StartupPath + "\\NormalMap\\brick_normalmap.png"), new Size(pictureBox1.Width+2, pictureBox1.Height+2));
            backbitmap1 = new Bitmap(Image.FromFile(Application.StartupPath + "\\NormalMap\\triangles-1430105_1280.png"), new Size(pictureBox1.Width+2, pictureBox1.Height+2));
            backbitmap2= new Bitmap(Image.FromFile(Application.StartupPath + "\\NormalMap\\download.jpg"), new Size(pictureBox1.Width+2, pictureBox1.Height+2));
            Filling.Normal_vector(1, normalmap1);
            Filling.Bitmap_back(3, backbitmap1);
            deaf_height = pictureBox1.Height;
            deaf_weight = pictureBox1.Width;
            points = vertexes.AddPoints(triangle_x, triangle_y, pictureBox1.Width, pictureBox1.Height);
            Sort_Vertex = sort_Ver.Sort_vertexes(points, triangle_x, triangle_y);
            pictureBox1.Width =(int)(deaf_weight / triangle_x) * triangle_x;
            pictureBox1.Height = (int)(deaf_height / triangle_y) * triangle_y;
            DirectBitmap = new DirectBitmap(pictureBox1.Width, pictureBox1.Height);
            refresh_bitmap();
        //    copy = (Bitmap)DirectBitmap.Bitmap.Clone();
            pictureBox1.Refresh();
            timer.Interval = 1000;
            timer.Elapsed +=timer1_Tick;
           // timer.Enabled = true;
            timer.AutoReset = true;


        }
        protected override void OnPaint(PaintEventArgs e)
        {
            // Call the OnPaint method of the base class.  
            //base.OnPaint(e);
            // Call methods of the System.Drawing.Graphics object.  
            //e.Graphics.DrawString(Text, Font, new SolidBrush(ForeColor), ClientRectangle);
        }
      //  private void 
      
        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            
            
            Drawing drawing = new Drawing();
            drawing.DrawEveryLine(triangle_x, triangle_y, points,e);
           if(animation_go)
            {
                Animation.Draw_sun(Animation.x, Animation.y,e,pictureBox1.Width,pictureBox1.Height);
            }
            pictureBox1.Image = copy;     
            
        }
        Filling Filling1 = new Filling();
        public void refresh_bitmap()
        {
            DirectBitmap.Dispose();
            DirectBitmap = new DirectBitmap(pictureBox1.Width, pictureBox1.Height);
            //if(animation)
            //{
            //    Filling1 = new Filling(Filling);
            //    Filling1.Fill_every_triangle(Sort_Vertex, DirectBitmap, color);
            //}
            //else
            Filling.Fill_every_triangle(Sort_Vertex, DirectBitmap, color);
            copy = (Bitmap)DirectBitmap.Bitmap.Clone();
            DirectBitmap.Dispose();

        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            if ((int)numericUpDown1.Value > 0)
            {
                triangle_x = (int)numericUpDown1.Value;
                points = new List<List<Point>>();
                //
                points = vertexes.AddPoints(triangle_x, triangle_y, deaf_weight, deaf_height);
                Sort_Vertex = sort_Ver.Sort_vertexes(points, triangle_x, triangle_y);
                pictureBox1.Width = (int)(deaf_weight / triangle_x) * triangle_x;
                pictureBox1.Height = (int)(deaf_height / triangle_y) * triangle_y;
                refresh_bitmap();
                pictureBox1.Refresh();
            }
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            if ((int)numericUpDown2.Value > 0)
            {
                triangle_y = (int)numericUpDown2.Value;
                points = new List<List<Point>>();
                points = vertexes.AddPoints(triangle_x, triangle_y, deaf_weight, deaf_height);
                Sort_Vertex = sort_Ver.Sort_vertexes(points, triangle_x, triangle_y);
                pictureBox1.Width = (int)(deaf_weight / triangle_x) * triangle_x;
                pictureBox1.Height = (int)(deaf_height / triangle_y) * triangle_y;
                refresh_bitmap();
                //pictureBox1.Refresh();
            }
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if(move_vertex==1& !colord)
            {
                if(vertex_found[0]!=-1)
                {
                   points[vertex_found[0]][vertex_found[1]] = e.Location;
                    //refresh_bitmap();
                   // pictureBox1.Refresh();

                }
            }
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            move_vertex = 1;
            
            vertex_found = vertexes.looking_for_vertex(e.Location,triangle_x,triangle_y,points);
            
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            if (move_vertex == 1)
            {
                vertex_found = new int[] { -1, -1 };
                move_vertex = 0;
                Sort_Vertex = new List<List<Point>>();
                Sort_Vertex = sort_Ver.Sort_vertexes(points, triangle_x, triangle_y);
                refresh_bitmap();
                //pictureBox1.Refresh();

            }
        }

       
        private void numericUpDown3_ValueChanged(object sender, EventArgs e)
        {
            color = Color.FromArgb((int)numericUpDown3.Value, (int)numericUpDown4.Value, (int)numericUpDown5.Value);
            refresh_bitmap();
          //  pictureBox1.Refresh();
        }

        private void numericUpDown4_ValueChanged(object sender, EventArgs e)
        {
            color = Color.FromArgb((int)numericUpDown3.Value, (int)numericUpDown4.Value, (int)numericUpDown5.Value);
            refresh_bitmap();
           // pictureBox1.Refresh();
        }

        private void numericUpDown5_ValueChanged(object sender, EventArgs e)
        {
            color = Color.FromArgb((int)numericUpDown3.Value, (int)numericUpDown4.Value, (int)numericUpDown5.Value);
            refresh_bitmap();
          //  pictureBox1.Refresh();
        }

        private void numericUpDown7_ValueChanged(object sender, EventArgs e)
        {

            Filling.Light((int)numericUpDown8.Value, (int)numericUpDown7.Value, (int)numericUpDown6.Value);
            refresh_bitmap();
            pictureBox1.Refresh();
        }

        private void numericUpDown6_ValueChanged(object sender, EventArgs e)
        {

            Filling.Light((int)numericUpDown8.Value, (int)numericUpDown7.Value, (int)numericUpDown6.Value);
            refresh_bitmap();
            pictureBox1.Refresh();
        }

        private void numericUpDown8_ValueChanged(object sender, EventArgs e)
        {
            Filling.Light((int)numericUpDown8.Value, (int)numericUpDown7.Value, (int)numericUpDown6.Value);
            refresh_bitmap();
            pictureBox1.Refresh();
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            
            Filling.ks = (double)trackBar1.Value/100;
            Filling.is_random = false;
            refresh_bitmap();
            pictureBox1.Refresh();
        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            Filling.kd = (double)trackBar2.Value/100;
            Filling.is_random = false;
            refresh_bitmap();
            pictureBox1.Refresh();
            
        }

        private void trackBar3_Scroll(object sender, EventArgs e)
        {
            Filling.m = trackBar3.Value;
            Filling.is_random = false;
            refresh_bitmap();
            pictureBox1.Refresh();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Filling.is_random = true;
            refresh_bitmap();
            pictureBox1.Refresh();
           // colorDialog1.ShowDialog();
           
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == 0)
                Filling.Normal_vector(1, normalmap1);
            else if (comboBox1.SelectedIndex == 1)
                Filling.Normal_vector(2, normalmap2);
            else
                Filling.Normal_vector(0, new Bitmap(pictureBox1.Width, pictureBox1.Height));
            refresh_bitmap();

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox2.SelectedIndex == 0)
                Filling.Bitmap_back(1, normalmap1);
            else if (comboBox2.SelectedIndex == 1)
                Filling.Bitmap_back(2,normalmap2);
            else if (comboBox2.SelectedIndex == 2)
                Filling.Bitmap_back(3, backbitmap1);
            else if (comboBox2.SelectedIndex == 3)
                Filling.Bitmap_back(4, backbitmap2);
            else
                Filling.Bitmap_back(0, new Bitmap(pictureBox1.Width, pictureBox1.Height));
            refresh_bitmap();
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox3.SelectedIndex == 2)
                Filling.filling_mode = 2;
            else if (comboBox3.SelectedIndex == 1)
                Filling.filling_mode = 1;
            else
                Filling.filling_mode = 0;
            refresh_bitmap();
            pictureBox1.Refresh();
        }
        int k = 0;
        private void button2_Click(object sender, EventArgs e)
        {
            timer.Enabled = true;
            timer.Start();
           // animation = true;
            Filling.is_random = false;
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //animation = false;
            timer.Stop();
            timer.Enabled = false;
           // timer.Dispose();
          //  timer = new System.Timers.Timer(1000);
            pictureBox1.Refresh();
        }
      
        private void timer1_Tick(object sender, EventArgs e)
        {

            k += 10;
            animation_go = true;
            Vector3 vector = new Vector3(0, pictureBox1.Height, 0);
            vector = new Vector3(k, pictureBox1.Height / 2, Animation.Count_S(k, pictureBox1.Width / 2));

            Animation.y = (int)vector.Y;

            Filling.L = new Vector3(vector.X / pictureBox1.Width, vector.Y / pictureBox1.Height, 2 * vector.Z / pictureBox1.Width);

            refresh_bitmap();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Filling.t_reflectors = !Filling.t_reflectors;
            refresh_bitmap();
        }

    }
}
