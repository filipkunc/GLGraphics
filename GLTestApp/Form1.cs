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

namespace GLTestApp
{
    public partial class Form1 : Form
    {
        PointF[] points = new PointF[10000];
        float offset = 0.0f;
        GLTexture fontTexture = null;
        bool drawGDI = true;
        bool drawOpenGL = true;

        public Form1()
        {
            InitializeComponent();
        }

        private void glView1_PaintCanvas(object sender, CanvasEventArgs e)
        {
            if (!drawOpenGL)
                return;

            GLCanvas g = e.Canvas;

            if (fontTexture == null)
            {
                using (Bitmap bm = new Bitmap(256, 128))
                {
                    fontTexture = new GLTexture(bm);
                }
            }

            g.Clear(Color.White);
            g.DisableTexturing();            

            g.SetCurrentColor(Color.Black);
            g.EnableLineAntialiasing();
            g.DrawLines(points);

            g.EnableTexturing();
            g.SetCurrentColor(Color.White);

            fontTexture.GdiToTexture(gg =>
                {
                    gg.Clear(Color.Transparent);
                    gg.DrawString("Test string", this.Font, Brushes.Blue, 0.0f, 0.0f);
                });

            fontTexture.Draw(new PointF(10.0f, 10.0f));
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            if (!drawGDI)
                return;

            Graphics g = e.Graphics;
            g.Clear(Color.White);

            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.DrawLines(Pens.Black, points);

            using (Bitmap bitmap = new Bitmap(256, 128))
            {
                using (Graphics gg = Graphics.FromImage(bitmap))
                {
                    gg.Clear(Color.Empty);
                    gg.DrawString("Test string", this.Font, Brushes.Blue, 0.0f, 0.0f);
                }

                g.DrawImage(bitmap, new Point(10, 10));
            }
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

    public static class GLTextureExtensions
    {
        public static void GdiToTexture(this GLTexture texture, Action<Graphics> draw)
        {
            using (Bitmap bitmap = new Bitmap(texture.Width, texture.Height))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    draw(g);                    
                }
                texture.Update(bitmap);
            }
        }
    }
}
