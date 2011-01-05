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
using ManyGraphics;
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
            //g.PageUnit = GraphicsUnit.Millimeter;

            g.DrawRectangle(new Pen(Color.Black, 2.0f), new Rectangle(50, 50, 100, 50));
            g.TranslateTransform(0, 100);
            g.DrawRectangle(new Pen(Color.Black, 1.0f), new Rectangle(50, 50, 100, 50));

            g.ResetTransform();

            List<PointF> points = new List<PointF>();

            for (float x = 0.0f; x < (float)Math.PI * 6.0f; x += 0.01f)
            {
                points.Add(new PointF(x * (float)Math.Cos(x) * 5.0f + 300.0f, x * (float)Math.Sin(x) * 5.0f + 400.0f));
            }

            g.DrawLines(new Pen(Color.Blue, 2.0f), points.ToArray());

            Rectangle gradientRect1 = new Rectangle(600, 50, 50, 100);
            LinearGradientBrush brush1 = new LinearGradientBrush(gradientRect1, Color.Red, Color.Yellow, LinearGradientMode.Vertical);

            Rectangle gradientRect2 = new Rectangle(700, 50, 50, 100);
            LinearGradientBrush brush2 = new LinearGradientBrush(gradientRect2, Color.Red, Color.Yellow, LinearGradientMode.Horizontal);

            g.FillRectangle(brush1, gradientRect1);
            g.FillRectangle(brush2, gradientRect2);

            brush1.Dispose();
            brush2.Dispose();
            
            g.DrawEllipse(Pens.DarkGreen, new Rectangle(200, 50, 100, 60));

            g.DrawArc(Pens.DimGray, new Rectangle(200, 150, 100, 100), 50.0f, 180.0f);
            //g.FillEllipse(Brushes.Blue, new Rectangle(350, 50, 100, 60));

            g.DrawString("Test string", new Font("Arial Black", 30.0f), Brushes.Purple, new PointF(500.0f, 400.0f));
        }
    }    
}
