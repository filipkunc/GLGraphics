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
        Bitmap bm;

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

            label1.Text = string.Format("ms: {0}", watch.Elapsed.TotalMilliseconds);            
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
            //g.PageUnit = GraphicsUnit.Millimeter;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            g.DrawString(g.DpiX.ToString(), this.Font, Brushes.Black, new PointF(2, 2));

            StringFormat sf = new StringFormat();
            Rectangle rect = new Rectangle(50, 50, 400, 200);

            g.DrawRectangle(Pens.Black, rect);

            g.DrawString(label1.Text, this.Font, Brushes.Red, rect, sf);

            sf.Alignment = StringAlignment.Center;

            g.FillRectangle(Brushes.Black, new Rectangle(180, 50, 140, 40));
            g.DrawString(label1.Text, this.Font, Brushes.Yellow, rect, sf);

            sf.Alignment = StringAlignment.Far;
            
            g.DrawString(label1.Text, this.Font, Brushes.Green, rect, sf);

            sf.Alignment = StringAlignment.Near;
            sf.LineAlignment = StringAlignment.Center;

            g.DrawString(label1.Text, this.Font, Brushes.Blue, rect, sf);

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
                    m.Translate(x * 50.0f + 200.0f, y * 50.0f + 200.0f);
                    g.Transform = m;
                    g.DrawLines(new Pen(Color.Blue, 0.0f), points.ToArray());
                }
            }

            g.ResetTransform();

            g.DrawRectangle(new Pen(Color.Red, 0), new Rectangle(50, 50, 24, 24));
            g.DrawImage(Resources.navigate_down2, new Rectangle(50, 50, 24, 24));

            if (bm == null)
            {
                ColorMatrix matrix = new ColorMatrix();
                matrix.Matrix33 = 0.4f; //opacity 0 = completely transparent, 1 = completely opaque
                bm = Helpers.BitmapFromImageAndColorMatrix(Resources.navigate_down2, matrix);
            }

            g.DrawRectangle(new Pen(Color.Red, 0), new Rectangle(100, 50, 24, 24));
            g.DrawImage(bm, new Rectangle(100, 50, 24, 24));
        }
    }    
}
