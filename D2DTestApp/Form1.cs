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
using System.Runtime.InteropServices;
using System.Reflection;
using System.Threading;
using System.IO;
using System.Drawing.Imaging;
using GLWrapper;
using D2DTestApp.Properties;

namespace D2DTestApp
{
    public partial class Form1 : Form
    {
        List<PointF> _points = SignalPoints._points;
        Bitmap _testImage = new Bitmap(Resources.testImage);

        public Form1()
        {
            InitializeComponent();
            panelGdipDrawLines.Paint += new PaintEventHandler(panelGdipDrawLines_Paint);
            panelGdipForDrawLine.Paint += new PaintEventHandler(panelGdipForDrawLine_Paint);
            panelD2DHW.PaintCanvas += new EventHandler<D2DView.RenderTargetEventArgs>(panelD2DHW_PaintCanvas);
            panelD2DSW.PaintCanvas += new EventHandler<D2DView.RenderTargetEventArgs>(panelD2DSW_PaintCanvas);
            panelGL.PaintCanvas += new EventHandler<GLWrapper.CanvasEventArgs>(panelGL_PaintCanvas);
            panelGdi32.Paint += new PaintEventHandler(panelGdi32_Paint);

            panelD2DHW._renderTargetType = SharpDX.Direct2D1.RenderTargetType.Hardware;
            panelD2DSW._renderTargetType = SharpDX.Direct2D1.RenderTargetType.Software;
            panelD2DHW.D2DEnabled = true;
            panelD2DSW.D2DEnabled = true;
            panelGL.GLEnabled = true;

            var last = _points.Last();
            _points.AddRange(_points.ToArray().Select(pt => new PointF(last.X + pt.X, pt.Y)));            
        }

        static Color _gridBackgroundColor = Color.PapayaWhip;
        static Color _gridColor = Color.Black;
        static Color _signalColor = Color.Blue;

        #region Millimeter Grid

        public struct Dpi
        {
            const float inchMM = 25.4f;
            const float mmInch = 0.0393700787f;

            /// <summary>
            /// Vychozi Dpi - 96, 96
            /// </summary>
            public static readonly Dpi DefaultDpi = new Dpi(96.0f, 96.0f);

            private float _dpiX, _dpiY;

            public float DpiX
            {
                get { return _dpiX; }
            }

            public float DpiY
            {
                get { return _dpiY; }
            }

            public int GetPxFromMM_X(float mm)
            {
                return (int)(mm * _dpiX * mmInch);
            }

            public int GetPxFromMM_Y(float mm)
            {
                return (int)(mm * _dpiY * mmInch);
            }

            public float GetMMFromPx_X(float px)
            {
                return (inchMM * px) / _dpiX;
            }

            public float GetMMFromPx_Y(float px)
            {
                return (inchMM * px) / _dpiY;
            }

            public Dpi(float dpiX, float dpiY)
            {
                _dpiX = dpiX;
                _dpiY = dpiY;
            }
        }

        static Bitmap _millimeterGridBitmap = null;
        static TextureBrush _millimeterGridBrush = null;

        public static void ResetMillimeterGrid()
        {
            _millimeterGridBitmap = null;
            _millimeterGridBrush = null;
        }

        private static void CreateMillimiterGridBrush(Dpi dpi)
        {
            SizeF sizeMM = new SizeF(500.0f, 250.0f);
            Size sizePx = new Size(dpi.GetPxFromMM_X(sizeMM.Width), dpi.GetPxFromMM_Y(sizeMM.Height));

            // 30.07.2011 (FK) PixelFormat.Format32bppPArgb by mel byt nejoptimalnejsi format bitmapy pro GDI+
            _millimeterGridBitmap = new Bitmap(sizePx.Width, sizePx.Height, PixelFormat.Format32bppPArgb);
            _millimeterGridBitmap.SetResolution(dpi.DpiX, dpi.DpiY);

            using (Graphics bg = Graphics.FromImage(_millimeterGridBitmap))
            {
                RectangleF gridRectMM = new RectangleF(PointF.Empty, sizeMM);

                bg.Clear(_gridBackgroundColor);

                bg.PageUnit = GraphicsUnit.Millimeter;
                bg.SmoothingMode = SmoothingMode.HighQuality;

                Pen gridPen1 = new Pen(Color.FromArgb(20, _gridColor), 0.0f);
                DrawGrid(bg, gridPen1, gridRectMM, 1.0f);

                bg.SmoothingMode = SmoothingMode.None;

                Pen gridPen5 = new Pen(Color.FromArgb(30, _gridColor), 0.0f);
                DrawGrid(bg, gridPen5, gridRectMM, 5.0f);

                Pen gridPen10 = new Pen(Color.FromArgb(40, _gridColor), 0.0f);
                DrawGrid(bg, gridPen10, gridRectMM, 10.0f);
            }

            // 30.07.2011 (FK) Texture brush je asi nejrychlejsi moznost jak kreslit vetsi plochy stejnou bitmapou v GDI+
            // viz http://stackoverflow.com/questions/264720/gdi-graphicsdrawimage-really-slow
            _millimeterGridBrush = new TextureBrush(_millimeterGridBitmap, WrapMode.Tile);
        }

        public static void DrawMillimeterGridInPixels(IGraphics g, Rectangle rectPx)
        {
            Dpi dpi = new Dpi(g.DpiX, g.DpiY);

            if (_millimeterGridBitmap == null)
                CreateMillimiterGridBrush(dpi);

            g.CompositingMode = CompositingMode.SourceCopy;

            _millimeterGridBrush.ResetTransform();
            _millimeterGridBrush.TranslateTransform(rectPx.Left, rectPx.Top);
            g.FillRectangle(_millimeterGridBrush, rectPx);

            g.CompositingMode = CompositingMode.SourceOver;
        }

        public static void DrawGrid(Graphics g, Pen pen, RectangleF rect, float step)
        {
            for (float x = rect.Left; x < rect.Right; x += step)
                g.DrawLine(pen, x, rect.Top, x, rect.Bottom);

            for (float y = rect.Top; y < rect.Bottom; y += step)
                g.DrawLine(pen, rect.Left, y, rect.Right, y);
        }

        #endregion

        enum DrawingMode
        {
            DrawLines,
            GdiPlusDrawLine,
            Gdi32,
        }

        public static GraphicsPath CreateRoundedRectanglePath(RectangleF rect, int cornerRadius)
        {
            GraphicsPath roundedRect = new GraphicsPath();

            roundedRect.AddArc(rect.X, rect.Y, cornerRadius * 2, cornerRadius * 2, 180, 90);
            roundedRect.AddLine(rect.X + cornerRadius, rect.Y, rect.Right - cornerRadius * 2, rect.Y);
            roundedRect.AddArc(rect.X + rect.Width - cornerRadius * 2, rect.Y, cornerRadius * 2, cornerRadius * 2, 270, 90);
            roundedRect.AddLine(rect.Right, rect.Y + cornerRadius * 2, rect.Right, rect.Y + rect.Height - cornerRadius * 2);

            roundedRect.AddArc(rect.X + rect.Width - cornerRadius * 2, rect.Y + rect.Height - cornerRadius * 2, cornerRadius * 2, cornerRadius * 2, 0, 90);

            roundedRect.AddLine(rect.Right - cornerRadius * 2, rect.Bottom, rect.X + cornerRadius * 2, rect.Bottom);
            roundedRect.AddArc(rect.X, rect.Bottom - cornerRadius * 2, cornerRadius * 2, cornerRadius * 2, 90, 90);
            roundedRect.AddLine(rect.X, rect.Bottom - cornerRadius * 2, rect.X, rect.Y + cornerRadius * 2);
            roundedRect.CloseFigure();

            return roundedRect;
        }

        void DrawLines(IGraphics g, List<PointF> points, DrawingMode mode)
        {
            const int count = 5;
            const float offsetY = 40;

            if (points != null)
            {
                using (Pen pen = new Pen(_signalColor, 0.0f))
                {
                    switch (mode)
                    {
                        case DrawingMode.DrawLines:
                            {
                                var pointsArray = points.ToArray();
                                for (int j = 0; j < count; j++)
                                {
                                    g.TranslateTransform(0, offsetY);
                                    g.DrawLines(pen, pointsArray);
                                }
                            } break;
                        case DrawingMode.GdiPlusDrawLine:
                            {
                                var gdip = ((GDIGraphics)g).Graphics;
                                for (int j = 0; j < count; j++)
                                {
                                    g.TranslateTransform(0, offsetY);
                                    for (int i = 1; i < points.Count; i++)
                                        gdip.DrawLine(pen, points[i - 1], points[i]);
                                }
                            } break;
                        case DrawingMode.Gdi32:
                            {
                                var gdip = ((GDIGraphics)g).Graphics;
                                for (int j = 0; j < count; j++)
                                {
                                    g.TranslateTransform(0, offsetY);
                                    gdip.DrawLinesGdi32(pen, points);
                                }
                            } break;
                        default:
                            break;
                    }
                }
            }
        }       

        void Draw(IGraphics g, DrawingMode mode)
        {
            g.SmoothingMode = checkBoxAntialiasing.Checked ? SmoothingMode.HighQuality : SmoothingMode.None;
            g.Clear(Color.White);

            DrawMillimeterGridInPixels(g, tabControl1.SelectedTab.Controls[0].ClientRectangle);

            g.PageUnit = GraphicsUnit.Millimeter;

            var points = _noisePoints;

            DrawLines(g, points, mode);

            g.ResetTransform();

            var path = CreateRoundedRectanglePath(new RectangleF(10, 10, 100, 40), 10);

            g.FillPath(new SolidBrush(Color.FromArgb(120, Color.LightSlateGray)), path);
            g.DrawPath(new Pen(Color.LightSlateGray, 0.0f), path);

            g.FillPie(new SolidBrush(Color.FromArgb(120, Color.CadetBlue)), new Rectangle(30, 20, 100, 100), 45.0f, 90.0f);
            g.DrawPie(new Pen(Color.CadetBlue, 0.0f), new Rectangle(30, 20, 100, 100), 45.0f, 90.0f);

            //GLGraphics gl = g as GLGraphics;
            //if (gl != null)
            //    gl.FillTextBackground = gl.FillTextBackground_glReadPixels;

            g.PageUnit = GraphicsUnit.Pixel;
            RectangleF rect = new RectangleF(30.0f, 15.0f, _testImage.Width, _testImage.Height);
            g.DrawImage(_testImage, rect);
            g.DrawRectangle(Pens.Black, rect.X, rect.Y, rect.Width, rect.Height);

            g.PageUnit = GraphicsUnit.Millimeter;

            g.HintTextBackgroundAction((gdi, textLocation, textRect) =>
            {
                _millimeterGridBrush.ResetTransform();
                _millimeterGridBrush.TranslateTransform(-textLocation.X, -textLocation.Y);
                gdi.FillRectangle(_millimeterGridBrush, textRect);
            });

            g.DrawString("Testovací řetězec pro odzkoušení správné implementace IGraphics - metody DrawString.",
                new Font("Arial", 12.0f), Brushes.Black, new PointF(100.0f, 58.0f));
        }

        void panelGdi32_Paint(object sender, PaintEventArgs e)
        {
            Draw(new GDIGraphics(e.Graphics), DrawingMode.Gdi32);
        }

        void panelGdipDrawLines_Paint(object sender, PaintEventArgs e)
        {
            Draw(new GDIGraphics(e.Graphics), DrawingMode.DrawLines);
        }

        void panelGdipForDrawLine_Paint(object sender, PaintEventArgs e)
        {
            Draw(new GDIGraphics(e.Graphics), DrawingMode.GdiPlusDrawLine);
        }

        void panelD2DHW_PaintCanvas(object sender, D2DView.RenderTargetEventArgs e)
        {
            Draw(new D2DGraphics(e), DrawingMode.DrawLines);
        }

        void panelD2DSW_PaintCanvas(object sender, D2DView.RenderTargetEventArgs e)
        {
            Draw(new D2DGraphics(e), DrawingMode.DrawLines);
        }

        void panelGL_PaintCanvas(object sender, GLWrapper.CanvasEventArgs e)
        {
            Draw(new GLGraphics(e), DrawingMode.DrawLines);
        }

        float _offset = 0.0f;
        List<PointF> _noisePoints = null;

        Dictionary<int, List<TimeSpan>> _tabFrames;
        const int _maxFrames = 100;

        private void timer1_Tick(object sender, EventArgs e)
        {
            _noisePoints = new List<PointF>();

            float step = (float)numericUpDownStep.Value;
            float amplitude = (float)numericUpDownAmplitude.Value;

            for (int i = 0; i < _points.Count; i++)
            {
                float x = _points[i].X;
                float y = _points[i].Y;
                y += (amplitude / 25.0f) * (float)Math.Sin(x * step);
                _noisePoints.Add(new PointF(x, y));
            }

            if (checkBoxDownsample.Checked)
                _noisePoints = Downsample(_noisePoints, Dpi.DefaultDpi.GetMMFromPx_X(1.0f));

            var selectedIndex = tabControl1.SelectedIndex;

            Stopwatch watch = Stopwatch.StartNew();
            tabControl1.SelectedTab.Controls[0].Refresh();
            watch.Stop();

            _offset += 1.5f;
            if (_offset > Math.PI * 24.0f)
                _offset = 0.0f;            

            var elapsed = watch.Elapsed;

            if (_tabFrames != null && _tabFrames[selectedIndex].Count < _maxFrames)
                _tabFrames[selectedIndex].Add(elapsed);

            labelFPS.Text = string.Format("ms: {0}", elapsed.TotalMilliseconds);
        }

        public static List<PointF> Downsample(IList<PointF> points, float minDistanceX)
        {
            if (points.Count <= 2)
                return new List<PointF>(points);

            List<PointF> downsampled = new List<PointF>();

            float startX = points[0].X;
            PointF minPoint = points[0];
            PointF maxPoint = points[0];

            for (int i = 1; i < points.Count; i++)
            {
                if (points[i].Y > maxPoint.Y)
                    maxPoint = points[i];

                if (points[i].Y < minPoint.Y)
                    minPoint = points[i];

                if (points[i].X - startX >= minDistanceX)
                {
                    if (minPoint.X < maxPoint.X)
                    {
                        downsampled.Add(minPoint);
                        downsampled.Add(maxPoint);
                    }
                    else
                    {
                        downsampled.Add(maxPoint);
                        downsampled.Add(minPoint);
                    }

                    minPoint = maxPoint = points[i];
                    startX = points[i].X;
                }
            }

            return downsampled;
        }

        private void buttonRunAutomaticTest_Click(object sender, EventArgs e)
        {
            buttonRunAutomaticTest.Enabled = false;
            ThreadPool.QueueUserWorkItem(AutomaticTest);
        }

        void DoTestAntialiasing(bool antialiasing)
        {
            this.Invoke(new Action(() => checkBoxAntialiasing.Checked = antialiasing));
            var tabCount = (int)this.Invoke(new Func<int>(() => tabControl1.TabCount));

            _tabFrames = new Dictionary<int, List<TimeSpan>>();
            for (int i = 0; i < tabCount; i++)
            {
                _tabFrames[i] = new List<TimeSpan>();
            }

            for (int i = 0; i < tabCount; i++)
            {
                this.Invoke(new Action<int>(x => tabControl1.SelectedIndex = x), i);
                while (_tabFrames[i].Count < _maxFrames)
                    Thread.Sleep(10);
            }
            this.Invoke(new Action(() =>
            {
                StringBuilder sb = new StringBuilder();

                for (int i = 0; i < tabControl1.TabCount; i++)
                {
                    sb.Append("\"" + tabControl1.TabPages[i].Text + " [ms]\"");
                    sb.Append(";");
                }

                sb.AppendLine();

                int row = 0;
                bool finished = false;
                while (!finished)
                {
                    finished = true;
                    for (int i = 0; i < tabControl1.TabCount; i++)
                    {
                        if (row < _tabFrames[i].Count)
                        {
                            sb.Append(_tabFrames[i][row].TotalMilliseconds.ToString());
                            finished = false;
                        }
                        sb.Append(";");
                    }
                    sb.AppendLine();
                    row++;
                }

                string path = @"C:\automaticTest_" + (antialiasing ? "antialias" : "alias") + ".csv";

                File.WriteAllText(path, sb.ToString());
                Process.Start(path);

                _tabFrames = null;
            }));
        }

        private void AutomaticTest(object userState)
        {
            this.Invoke(new Action(() => progressBar1.Visible = true));

            DoTestAntialiasing(true);

            DoTestAntialiasing(false);

            this.Invoke(new Action(() =>
                {
                    progressBar1.Visible = false;
                    buttonRunAutomaticTest.Enabled = true;
                }));
        }
    }
}
