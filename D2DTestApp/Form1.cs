using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using GraphicsImplementation;
using System.Diagnostics;

namespace D2DTestApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            panel1.Paint += new PaintEventHandler(panel1_Paint);
            panel1.PaintCanvas += new EventHandler<D2DView.RenderTargetEventArgs>(panel1_PaintCanvas);
        }

        void panel1_Paint(object sender, PaintEventArgs e)
        {
            Draw(new GDIGraphics(e.Graphics));
        }

        private void checkBoxD2DEnabled_CheckedChanged(object sender, EventArgs e)
        {
            panel1.D2DEnabled = checkBoxD2DEnabled.Checked;
        }

        void panel1_PaintCanvas(object sender, D2DView.RenderTargetEventArgs e)
        {
            Draw(new D2DGraphics(e));
        }

        float _offset = 0.0f;

        void Draw(IGraphics g)
        {
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.DrawLine(Pens.Black, new System.Drawing.Point(10, 10), new System.Drawing.Point(200, 100));

            for (int y = 0; y < 2; y++)
            {
                for (int x = 0; x < 2; x++)
                {
                    var points = new List<PointF>();

                    for (float a = _offset; a < (float)Math.PI * 12.0f + _offset; a += 0.01f)
                    {
                        points.Add(new PointF(x * 120.0f + 200.0f + a * (float)Math.Cos(a), y * 120.0f + 200.0f + a * (float)Math.Sin(a)));
                    }

                    g.DrawLines(new Pen(Color.Blue, 0.0f), points.ToArray());
                }
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Stopwatch watch = new Stopwatch();
            watch.Start();
            panel1.Refresh();
            watch.Stop();

            _offset += 0.1f;
            if (_offset > Math.PI * 24.0f)
                _offset = 0.0f;

            labelFPS.Text = string.Format("ms: {0}", watch.Elapsed.TotalMilliseconds);
        }
    }
}
