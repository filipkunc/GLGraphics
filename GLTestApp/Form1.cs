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

namespace GLTestApp
{
    public partial class Form1 : Form
    {
        Bitmap transparentBitmap;
        TextureBrush gridBrush;

        public Form1()
        {
            InitializeComponent();
            glView1.GLEnabled = false;
            glView1.MouseWheel += new MouseEventHandler(glView1_MouseWheel);
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
            GLGraphics g = new GLGraphics(e);
            Draw(g);
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            glView1.GLEnabled = checkBox1.Checked;
            glView1.Refresh();
        }

        Font font1 = SystemFonts.DefaultFont;
        Font font2 = new Font(SystemFonts.DefaultFont, FontStyle.Bold);

        void HintBackground(Graphics g, Point textLocationPixel, Rectangle backgroundRect)
        {
            gridBrush.ResetTransform();
            gridBrush.TranslateTransform(-textLocationPixel.X, -textLocationPixel.Y);
            g.FillRectangle(gridBrush, backgroundRect);

            g.TranslateTransform(-textLocationPixel.X, -textLocationPixel.Y);

            g.DrawRectangle(Pens.Black, new Rectangle(100, 50, 400, 150));
            g.FillRectangle(Brushes.Black, new Rectangle(230, 50, 140, 40));
        }

        void Draw(IGraphics g)
        {
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            g.SmoothingMode = SmoothingMode.HighQuality;

            //g.SetClip(new Rectangle(50, 50, 200, 400));

            if (gridBrush == null)
            {
                gridBrush = new TextureBrush(Images.Grid_37px);
            }

            //gridBrush.ResetTransform();
            //gridBrush.TranslateTransform(100, 100);

            g.CompositingMode = CompositingMode.SourceCopy;
            gridBrush.ResetTransform();
            g.FillRectangle(gridBrush, glView1.ClientRectangle);
            g.CompositingMode = CompositingMode.SourceOver;

            g.HintTextBackgroundAction(HintBackground);

            //g.PageUnit = GraphicsUnit.Millimeter;
            //g.SmoothingMode = SmoothingMode.AntiAlias;
            string text = "12.5% 2/5 (1) + ms test...";

            g.DrawString(g.DpiX.ToString(), font1, Brushes.Black, new PointF(2, 2));

            StringFormat sf = new StringFormat();
            Rectangle rect = new Rectangle(100, 50, 400, 150);

            g.DrawRectangle(Pens.Black, rect);

            g.DrawString(text, font2, Brushes.Red, rect, sf);

            sf.Alignment = StringAlignment.Center;

            g.FillRectangle(Brushes.Black, new Rectangle(230, 50, 140, 40));
            g.DrawString(text, font1, Brushes.Yellow, rect, sf);

            sf.Alignment = StringAlignment.Far;

            g.DrawString(text, font2, Brushes.Green, rect, sf);

            sf.Alignment = StringAlignment.Near;
            sf.LineAlignment = StringAlignment.Center;

            g.DrawString(text, font1, Brushes.Blue, rect, sf);

            sf = new StringFormat();
            sf.Alignment = StringAlignment.Far;
            sf.LineAlignment = StringAlignment.Far;

            g.DrawString("Far, Far, PointF", font2, Brushes.Green, new PointF(100.0f, 150.0f), sf);

            sf = new StringFormat();
            sf.LineAlignment = StringAlignment.Far;

            g.DrawString(text, font1, Brushes.DarkCyan, rect, sf);

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
                    m.Translate(x * 50.0f + 200.0f, y * 50.0f + 300.0f);
                    g.Transform = m;
                    g.DrawLines(new Pen(Color.Blue, 0.0f), points.ToArray());
                }
            }

            g.ResetTransform();

            g.DrawRectangle(new Pen(Color.Red, 0), new Rectangle(50, 150, 24, 24));
            g.DrawImage(Images.NavigateDown_48px, new Rectangle(50, 150, 24, 24));

            if (transparentBitmap == null)
            {
                ColorMatrix matrix = new ColorMatrix();
                matrix.Matrix33 = 0.4f; //opacity 0 = completely transparent, 1 = completely opaque
                transparentBitmap = GraphicsHelpers.BitmapFromImageAndColorMatrix(Images.NavigateDown_48px, matrix);
            }

            g.DrawRectangle(new Pen(Color.Red, 0), new Rectangle(100, 150, 24, 24));
            g.DrawImage(transparentBitmap, new Rectangle(100, 150, 24, 24));

            using (HatchBrush hatchBrush = new HatchBrush(HatchStyle.LightUpwardDiagonal, Color.Black, Color.Transparent))
            {
                g.FillRectangle(hatchBrush, new Rectangle(10, 10, 100, 20));
            }

            using (HatchBrush hatchBrush = new HatchBrush(HatchStyle.LightDownwardDiagonal, Color.Black, Color.Transparent))
            {
                g.FillRectangle(hatchBrush, new Rectangle(110, 10, 200, 20));
            }

            g.SmoothingMode = SmoothingMode.None;

            g.FillRectangle(Brushes.White, new Rectangle(0, 210, 510, 50));

            Rectangle rc = new Rectangle(520, 250, 300, 200);
            g.FillRectangle(Brushes.White, rc);

            g.HintTextBackgroundColor(Color.White);
            g.DrawString("OpenGL 3D Stuff", font1, Brushes.Black, 600.0f, 250.0f);

            if (g is GLGraphics)
                Draw3dStuff(g as GLGraphics, rc);
            
            Pen pen = new Pen(Color.Black, 0);
            pen.DashStyle = DashStyle.Dot;
            g.DrawLine(pen, new Point(10, 220), new Point(500, 220));
            pen.DashStyle = DashStyle.Dash;
            g.DrawLine(pen, new Point(10, 230), new Point(500, 230));
            pen.DashStyle = DashStyle.DashDot;
            g.DrawLine(pen, new Point(10, 240), new Point(500, 240));
            pen.DashStyle = DashStyle.DashDotDot;
            g.DrawLine(pen, new Point(10, 250), new Point(500, 250));

            //g.ResetClip();
        }

        GLCamera camera = new GLCamera(new GLVector3(), -30.0f, 20.0f, 80.0f);
        
        private void Draw3dStuff(GLGraphics g, Rectangle rc)
        {
            var gl = g.Canvas;
            
            gl.BeginPerspective(rc, 45.0, 0.2, 1000.0);
            //gl.BeginOrtho(rc, -100, 100, -100, 100, -100, 100);
            gl.SetCamera(camera);

            Draw3dGrid(gl, 100, 20);

            List<GLPoint> vertices = new List<GLPoint>();

            float s = 10.0f;

            vertices.Add(new GLPoint(s, s, -s, Color.Red));
            vertices.Add(new GLPoint(-s, s, -s, Color.Green));
            vertices.Add(new GLPoint(-s, s, s, Color.Blue));
            vertices.Add(new GLPoint(s, s, s, Color.Cyan));

            vertices.Add(new GLPoint(s, -s, s, Color.Green));
            vertices.Add(new GLPoint(-s, -s, s, Color.Green));
            vertices.Add(new GLPoint(-s, -s, -s, Color.Green));
            vertices.Add(new GLPoint(s, -s, -s, Color.Green));

            vertices.Add(new GLPoint(s, s, s, Color.Blue));
            vertices.Add(new GLPoint(-s, s, s, Color.Blue));
            vertices.Add(new GLPoint(-s, -s, s, Color.Blue));
            vertices.Add(new GLPoint(s, -s, s, Color.Blue));

            vertices.Add(new GLPoint(s, -s, -s, Color.Cyan));
            vertices.Add(new GLPoint(-s, -s, -s, Color.Cyan));
            vertices.Add(new GLPoint(-s, s, -s, Color.Cyan));
            vertices.Add(new GLPoint(s, s, -s, Color.Cyan));

            vertices.Add(new GLPoint(-s, s, s, Color.Magenta));
            vertices.Add(new GLPoint(-s, s, -s, Color.Magenta));
            vertices.Add(new GLPoint(-s, -s, -s, Color.Magenta));
            vertices.Add(new GLPoint(-s, -s, s, Color.Magenta));

            vertices.Add(new GLPoint(s, s, -s, Color.Yellow));
            vertices.Add(new GLPoint(s, s, s, Color.Yellow));
            vertices.Add(new GLPoint(s, -s, s, Color.Yellow));
            vertices.Add(new GLPoint(s, -s, -s, Color.Yellow));
            
            gl.DrawQuads(vertices.ToArray(), false);

            gl.EndPerspectiveOrOrtho();
        }

        private void Draw3dGrid(GLCanvas gl, int size, int step)
        {
            Color dark = Color.Black;
            Color light = Color.DarkGray;
            Color color;

            List<GLPoint> vertices = new List<GLPoint>();

            for (int x = -size; x <= size; x += step)
            {
                if (x == 0)
                    color = dark;
                else
                    color = light;

                vertices.Add(new GLPoint(x, 0, -size, color));
                vertices.Add(new GLPoint(x, 0, size, color));

            }
            for (int z = -size; z <= size; z += step)
            {

                if (z == 0)
                    color = dark;
                else
                    color = light;

                vertices.Add(new GLPoint(-size, 0, z, color));
                vertices.Add(new GLPoint(size, 0, z, color));
            }

            gl.DrawLines(vertices.ToArray(), false);
        }

        Point lastPos = Point.Empty;

        private void glView1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                camera.DegreesX -= (e.Y - lastPos.Y);
                camera.DegreesY -= (e.X - lastPos.X);
            }
            else if (e.Button == MouseButtons.Middle)
            {
                RectangleF bounds = glView1.ClientRectangle;
                float w = bounds.Width;
                float h = bounds.Height;
                float sensitivity = (w + h) / 2.0f;
                sensitivity = 1.0f / sensitivity;
                sensitivity *= camera.Zoom * 2.0f;
                camera.Move(-(e.X - lastPos.X) * sensitivity, -(e.Y - lastPos.Y) * sensitivity);
            }
            lastPos = e.Location;
        }

        private void glView1_MouseDown(object sender, MouseEventArgs e)
        {
            lastPos = e.Location;
        }

        private void glView1_MouseUp(object sender, MouseEventArgs e)
        {

        }

        void glView1_MouseWheel(object sender, MouseEventArgs e)
        {
            camera.Zoom -= e.Delta * 0.2f;
        }
    }
}
