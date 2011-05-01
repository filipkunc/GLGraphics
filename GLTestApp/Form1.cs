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
using GraphicsImplementation;
using GLTestApp.Properties;

namespace GLTestApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            glView1.GLEnabled = false;
            //glView1.NeverInitGL = true;
            //timer1.Enabled = false;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Stopwatch watch = new Stopwatch();
            watch.Start();
            glView1.Refresh();
            watch.Stop();
            label1.Text = string.Format("FPS: {0}", 1.0 / watch.Elapsed.TotalSeconds);
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
            glView1.Refresh();
        }

        void Draw(IGraphics g)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.PageUnit = GraphicsUnit.Millimeter;

            List<PointF> points = new List<PointF>();

            for (float x = 0.0f; x < (float)Math.PI * 6.0f; x += 0.01f)
            {
                points.Add(new PointF(x * (float)Math.Cos(x), x * (float)Math.Sin(x)));
            }

            Matrix m = new Matrix();

            for (int y = 0; y < 5; y++)
            {
                for (int x = 0; x < 5; x++)
                {
                    m.Reset();
                    m.Translate(x * 30.0f + 20.0f, y * 30.0f + 20.0f);
                    g.Transform = m;
                    g.DrawLines(new Pen(Color.Blue, 0.0f), points.ToArray());                    
                }
            }

            g.ResetTransform();

            g.DrawString("Test string", new Font("Arial Black", 30.0f), Brushes.Purple, new PointF(1.0f, 1.0f));

            g.DrawRectangle(Pens.Black, new Rectangle(10, 10, 50, 100));
        }
    }    
}
