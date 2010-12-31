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
using GraphicsInterface;

namespace GLTestApp
{
    public partial class Form1 : Form
    {
        PointF[] points = new PointF[4000];
        float offset = 0.0f;

        public Form1()
        {
            InitializeComponent();
            glView1.GLEnabled = false;
            //glView1.NeverInitGL = true;
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

            glView1.Refresh();
        }

        void Draw(IGraphics g)
        {
            g.Clear(Color.White);

            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.DrawLines(Pens.Black, points);

            g.DrawString("Test string, ☺", new Font("Arial", 20.0f), Brushes.Blue, new PointF(10.0f, 10.0f));

            g.DrawLine(Pens.Black, new PointF(30.0f, 400.0f), new PointF(830.0f, 400.0f));

            for (int x = 1; x < 40; x++)
            {
                g.DrawLine(Pens.Black, new PointF(30.0f + x * 20.0f, 410.0f), new PointF(30.0f + x * 20.0f, 390.0f));

                g.DrawString(x.ToString(), this.Font, Brushes.Blue, new PointF(30.0f + x * 20.0f, 380.0f));
            }            
        }

        private void glView1_Paint(object sender, PaintEventArgs e)
        {
            GDIGraphics g = new GDIGraphics(e.Graphics);
            Draw(g);
        }

        private void glView1_PaintCanvas(object sender, CanvasEventArgs e)
        {
            GLGraphics g = new GLGraphics(e.Canvas);
            Draw(g);            
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            glView1.GLEnabled = checkBox1.Checked;
        }
    }    
}
