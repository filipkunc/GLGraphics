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
using GLTestApp.Properties;

namespace GLTestApp
{
    public partial class Form1 : Form
    {
        PointF[] points = new PointF[4000];
        float offset = 0.0f;

        Bitmap spaceship = Resources.spaceship;

        public Form1()
        {
            InitializeComponent();
            glView1.GLEnabled = false;
            //glView1.NeverInitGL = true;

            spaceship.MakeTransparent(Color.Magenta);
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

            Stopwatch watch = new Stopwatch();
            watch.Start();
            glView1.Refresh();
            watch.Stop();
            label1.Text = string.Format("FPS: {0}", 1.0 / watch.Elapsed.TotalSeconds);
        }

        Bitmap gridBitmap = null;

        Bitmap CreateGridBitmap(int width, int height, int step)
        {
            if (gridBitmap == null)
            {
                gridBitmap = new Bitmap(width, height);

                using (Graphics g = Graphics.FromImage(gridBitmap))
                {
                    DrawGrid(new GDIGraphics(g), width, height, step);
                }
            }

            return gridBitmap;
        }

        void DrawGrid(IGraphics g, int width, int height, int step)
        {
            for (int y = 0; y < height; y += step)
            {
                g.DrawLine(Pens.LightGray, 0, y, width, y);
            }

            for (int x = 0; x < width; x += step)
            {
                g.DrawLine(Pens.LightGray, x, 0, x, height);
            }
        }

        Point spaceshipPosition = new Point(400, 400);
        Point move = Point.Empty;

        void Draw(IGraphics g)
        {
            g.Clear(Color.White);

            /*Bitmap bitmap = CreateGridBitmap(glView1.Width, glView1.Height, 10);

            g.DrawImage(bitmap, Point.Empty);*/

            //DrawGrid(g, glView1.Width, glView1.Height, 10);

            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TranslateTransform(0.0f, -50.0f);
            g.DrawLines(Pens.Black, points);

            g.TranslateTransform(0.0f, 200.0f);
            g.DrawLines(Pens.Gray, points);

            g.ResetTransform();

            g.DrawString("Test string, ☺", new Font("Arial", 20.0f), Brushes.Blue, new PointF(10.0f, 10.0f));

            g.DrawLine(Pens.Black, new Point(30, 500), new Point(830, 500));

            for (int x = 1; x < 40; x++)
            {
                g.DrawLine(Pens.Black, new Point(30 + x * 20, 510), new Point(30 + x * 20, 490));

                g.DrawString(x.ToString(), this.Font, Brushes.Blue, new PointF(30.0f + x * 20.0f, 480.0f));
            }

            g.DrawImage(spaceship, spaceshipPosition);

            spaceshipPosition.X += move.X;
            spaceshipPosition.Y += move.Y;
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            const int speed = 3;

            if (keyData == Keys.Left)
            {
                move.X -= speed;
                if (move.X < -speed)
                    move.X = -speed;
            }
            else if (keyData == Keys.Right)
            {
                move.X += speed;
                if (move.X > speed)
                    move.X = speed;
            }

            if (keyData == Keys.Up)
            {
                move.Y -= speed;
                if (move.Y < -speed)
                    move.Y = -speed;
            }
            else if (keyData == Keys.Down)
            {
                move.Y += speed;
                if (move.Y > speed)
                    move.Y = speed;
            }

            return base.ProcessCmdKey(ref msg, keyData);
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
