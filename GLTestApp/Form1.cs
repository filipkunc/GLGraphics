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

namespace GLTestApp
{
    public partial class Form1 : Form
    {
        PointF[] points = new PointF[10000];
        float offset = 0.0f;

        public Form1()
        {
            InitializeComponent();
        }

        private void glView1_PaintCanvas(object sender, CanvasEventArgs e)
        {
            GLCanvas g = e.Canvas;
            g.Clear(Color.White);

            g.SetCurrentColor(Color.Black);
            g.EnableLineAntialiasing();
            g.DrawLines(points);
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.Clear(Color.White);

            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.DrawLines(Pens.Black, points);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            float x = offset;
            float longX = offset;
            
            for (int i = 0; i < points.Length; i++)
            {
                points[i].X = x;
                points[i].Y = 200.0f - (float)Math.Sin(x) * (float)Math.Sin(longX) * 100.0f;
                x += 0.1f;
                longX += 0.01f;
            }

            offset += 0.1f;
            if (offset > 2.0f * (float)Math.PI)
                offset -= 2.0f * (float)Math.PI;

            //pictureBox1.Refresh();
            glView1.Refresh();
        }
    }
}
