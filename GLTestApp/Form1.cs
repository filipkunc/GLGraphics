using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GLWrapper;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Diagnostics;

namespace GLTestApp
{
    public partial class Form1 : Form
    {
        PointF[] points = new PointF[4000];
        float offset = 0.0f;
        bool drawGDI = false;
        bool drawOpenGL = false;

        public Form1()
        {
            InitializeComponent();
        }

        private void glView1_PaintCanvas(object sender, CanvasEventArgs e)
        {
            if (!drawOpenGL)
                return;

            Stopwatch watch = new Stopwatch();
            watch.Start();

            GLCanvas g = e.Canvas;

            g.Clear(Color.White);
            g.CurrentColor = Color.Black;
            g.AntialiasingEnabled = true;
            
            g.DrawLines(points);

            g.CurrentColor = Color.Blue;
            g.DrawString("Test string, ☺", new Font("Arial", 20.0f), new PointF(10.0f, 10.0f));

            watch.Stop();
            labelGL.Text = "OpenGL " + (1.0 / watch.Elapsed.TotalSeconds);
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            if (!drawGDI)
                return;

            Stopwatch watch = new Stopwatch();
            watch.Start();

            Graphics g = e.Graphics;
            g.Clear(Color.White);

            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.DrawLines(Pens.Black, points);

            g.DrawString("Test string, ☺", new Font("Arial", 20.0f), Brushes.Blue, new PointF(10.0f, 10.0f));

            watch.Stop();
            labelGDI.Text = "GDI+ " + (1.0 / watch.Elapsed.TotalSeconds);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            float x = offset;
            float longX = offset;
            
            for (int i = 0; i < points.Length; i++)
            {
                points[i].X = x;
                points[i].Y = 200.0f - (float)Math.Sin(x) * (float)Math.Sin(longX) * 100.0f;
                x += 0.5f;
                longX += 0.01f;
            }

            offset += 0.1f;
            if (offset > 2.0f * (float)Math.PI)
                offset -= 2.0f * (float)Math.PI;

            if (drawGDI)
                pictureBox1.Refresh();
            if (drawOpenGL)
                glView1.Refresh();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            drawGDI = !drawGDI;
        }

        private void glView1_Click(object sender, EventArgs e)
        {
            drawOpenGL = !drawOpenGL;
        }
    }    
}
