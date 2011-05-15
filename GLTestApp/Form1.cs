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
        GlyphTextureCache _glyphCache;

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
            if (_glyphCache == null)
            {
                _glyphCache = new GlyphTextureCache();
            }

            string text = textBox1.Text;

            if (g is GLGraphics)
            {
                GLGraphics gl = g as GLGraphics;
                gl.Canvas.CurrentColor = Color.White;
            }
            _glyphCache.DrawString(g, text, this.Font, new PointF(10.0f, 40.0f));
            if (g is GDIGraphics)
                g.DrawString(text, this.Font, Brushes.Black, new PointF(10.0f, 80.0f));
        }                
    }    
}
