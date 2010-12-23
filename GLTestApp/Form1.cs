using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GLWrapper;

namespace GLTestApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void glView1_PaintCanvas(object sender, CanvasEventArgs e)
        {
            GLCanvas g = e.Canvas;
            g.Clear(Color.White);

            g.SetCurrentColor(Color.Black);
            g.DrawRectangle(new RectangleF(10, 10, 100, 20));
        }
    }
}
