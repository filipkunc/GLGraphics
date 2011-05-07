using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using GLWrapper;
using System.Windows.Forms;
using System.Diagnostics;

namespace GraphicsImplementation
{
    static class GraphicsExtensions
    {
        public static RectangleF ToRectangleF(this Rectangle rect)
        {
            return new RectangleF(rect.X, rect.Y, rect.Width, rect.Height);
        }
    }

    public class GLGraphics : IGraphics
    {
        private static int NextPow2(int n)
        {
            int x = 2;
            while (x < n)
                x <<= 1;
            return x;
        }

        private static bool FloatEquals(float a, float b)
        {
            if (Math.Abs(a - b) < 0.01f)
                return true;
            return false;
        }

        #region Texture Cache

        private static GLTexture GdiToTexture(Size originalSize, Action<Graphics> draw)
        {
            Size power2Size = new Size(NextPow2(originalSize.Width), NextPow2(originalSize.Height));
            using (Bitmap bitmap = new Bitmap(power2Size.Width, power2Size.Height))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.Clear(Color.White);
                    g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
                    draw(g);                    
                }
                bitmap.MakeTransparent(Color.White);
                GLTexture texture = new GLTexture(bitmap, originalSize.Width, originalSize.Height);
                return texture;
            }
        }

        static Dictionary<object, GLTexture> _textureCache = new Dictionary<object, GLTexture>();
        const int MaxCachedTextures = 100;

        private static void ClearTexturesIfNeeded()
        {
            if (_textureCache.Count > MaxCachedTextures)
            {
                foreach (var texture in _textureCache.Values)
                    texture.Dispose();

                _textureCache.Clear();
            }
        }

        private static GLTexture GetCachedTexture(object key, Size originalSize, Action<Graphics> draw)
        {
            GLTexture texture = null;
            if (!_textureCache.TryGetValue(key, out texture))
            {
                ClearTexturesIfNeeded();

                texture = GdiToTexture(originalSize, draw);
                _textureCache.Add(key, texture);
            }
            return texture;
        }


        private static GLTexture GetCachedTexture(Image image)
        {
            return GetCachedTexture(image, image.Size, gdi => gdi.DrawImageUnscaled(image, Point.Empty));
        }

        private static GLTexture GetCachedTexture(string s, Font font, Brush brush, SizeF layoutSize, StringFormat stringFormat)
        {
            Size originalSize = layoutSize.ToSize();
            if (originalSize.IsEmpty)
                originalSize = TextRenderer.MeasureText(s, font);

            SolidBrush solid = (SolidBrush)brush;

            object key;

            if (stringFormat != null)
                key = new { s, font, solid.Color, stringFormat.Alignment, stringFormat.LineAlignment };
            else
                key = new { s, font, solid.Color };

            return GetCachedTexture(key, originalSize, gdi =>
                {
                    if (layoutSize.IsEmpty)
                    {
                        gdi.DrawString(s, font, brush, PointF.Empty);
                    }
                    else
                    {
                        if (stringFormat != null)
                            gdi.DrawString(s, font, brush, new RectangleF(PointF.Empty, layoutSize), stringFormat);
                        else
                            gdi.DrawString(s, font, brush, new RectangleF(PointF.Empty, layoutSize));
                    }
                });
        }

        #endregion

        GLCanvas g;
        GraphicsUnit _pageUnit = GraphicsUnit.Pixel;
        Matrix _transform = new Matrix();
        GLMatrix2D _glTransform = new GLMatrix2D();

        public GLGraphics(GLCanvas canvas)
        {
            g = canvas;
        }

        public Region Clip
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public RectangleF ClipBounds
        {
            get { throw new NotImplementedException(); }
        }

        public CompositingMode CompositingMode
        {
            get
            {
                if (g.BlendEnabled)
                    return CompositingMode.SourceOver;
                return CompositingMode.SourceCopy;
            }
            set
            {
                g.BlendEnabled = value == CompositingMode.SourceOver;
            }
        }

        public CompositingQuality CompositingQuality
        {
            get { return CompositingQuality.Default; }
            set { }
        }

        public float DpiX
        {
            get { return g.Dpi.X; }
        }

        public float DpiY
        {
            get { return g.Dpi.Y; }
        }

        public InterpolationMode InterpolationMode
        {
            get { return InterpolationMode.Bilinear; }
            set { }
        }

        public bool IsClipEmpty
        {
            get { throw new NotImplementedException(); }
        }

        public bool IsVisibleClipEmpty
        {
            get { throw new NotImplementedException(); }
        }

        public float PageScale
        {
            get { return 1.0f; }
            set { }
        }

        public GraphicsUnit PageUnit
        {
            get { return _pageUnit; }
            set
            {
                if (_pageUnit == value)
                    return;

                _pageUnit = value;
                switch (_pageUnit)
                {
                    case GraphicsUnit.Millimeter:
                        g.GlobalScale = new PointF(g.Dpi.X / 25.4f, g.Dpi.Y / 25.4f);
                        break;
                    case GraphicsUnit.Pixel:
                        g.GlobalScale = new PointF(1.0f, 1.0f);
                        break;
                    default:
                        throw new NotImplementedException();
                }
            }
        }

        public PixelOffsetMode PixelOffsetMode
        {
            get { return PixelOffsetMode.None; }
            set { }
        }

        public Point RenderingOrigin
        {
            get { return Point.Empty; }
            set { }
        }

        public SmoothingMode SmoothingMode
        {
            get
            {
                if (g.AntialiasingEnabled)
                    return SmoothingMode.AntiAlias;
                return SmoothingMode.None;
            }
            set
            {
                g.AntialiasingEnabled = value == SmoothingMode.AntiAlias;
            }
        }

        public int TextContrast
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public TextRenderingHint TextRenderingHint
        {
            get
            {
                return TextRenderingHint.AntiAliasGridFit;
            }
            set
            {

            }
        }

        public Matrix Transform
        {
            get
            {
                return _transform;
            }
            set
            {
                _transform = value;
                _glTransform.SetFromGdiMatrix(value);
                g.Transform(_glTransform);
            }
        }

        public RectangleF VisibleClipBounds
        {
            get { throw new NotImplementedException(); }
        }

        public void AddMetafileComment(byte[] data)
        {
            throw new NotImplementedException();
        }

        public GraphicsContainer BeginContainer()
        {
            throw new NotImplementedException();
        }

        public GraphicsContainer BeginContainer(Rectangle dstrect, Rectangle srcrect, GraphicsUnit unit)
        {
            throw new NotImplementedException();
        }

        public GraphicsContainer BeginContainer(RectangleF dstrect, RectangleF srcrect, GraphicsUnit unit)
        {
            throw new NotImplementedException();
        }

        public void Clear(Color color)
        {
            g.Clear(color);
        }

        public void CopyFromScreen(Point upperLeftSource, Point upperLeftDestination, Size blockRegionSize)
        {
            throw new NotImplementedException();
        }

        public void CopyFromScreen(Point upperLeftSource, Point upperLeftDestination, Size blockRegionSize, CopyPixelOperation copyPixelOperation)
        {
            throw new NotImplementedException();
        }

        public void CopyFromScreen(int sourceX, int sourceY, int destinationX, int destinationY, Size blockRegionSize)
        {
            throw new NotImplementedException();
        }

        public void CopyFromScreen(int sourceX, int sourceY, int destinationX, int destinationY, Size blockRegionSize, CopyPixelOperation copyPixelOperation)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {

        }

        public void DrawArc(Pen pen, Rectangle rect, float startAngle, float sweepAngle)
        {
            DrawArc(pen, rect.ToRectangleF(), startAngle, sweepAngle);
        }

        public void DrawArc(Pen pen, RectangleF rect, float startAngle, float sweepAngle)
        {
            g.CurrentColor = pen.Color;
            g.LineWidth = pen.Width;
            g.DrawArc(rect, startAngle, sweepAngle, false);
        }

        public void DrawArc(Pen pen, float x, float y, float width, float height, float startAngle, float sweepAngle)
        {
            DrawArc(pen, new RectangleF(x, y, width, height), startAngle, sweepAngle);
        }

        public void DrawArc(Pen pen, int x, int y, int width, int height, int startAngle, int sweepAngle)
        {
            DrawArc(pen, new Rectangle(x, y, width, height), startAngle, sweepAngle);
        }

        public void DrawBezier(Pen pen, Point pt1, Point pt2, Point pt3, Point pt4)
        {
            throw new NotImplementedException();
        }

        public void DrawBezier(Pen pen, PointF pt1, PointF pt2, PointF pt3, PointF pt4)
        {
            throw new NotImplementedException();
        }

        public void DrawBezier(Pen pen, float x1, float y1, float x2, float y2, float x3, float y3, float x4, float y4)
        {
            throw new NotImplementedException();
        }

        public void DrawBeziers(Pen pen, Point[] points)
        {
            throw new NotImplementedException();
        }

        public void DrawBeziers(Pen pen, PointF[] points)
        {
            throw new NotImplementedException();
        }

        public void DrawClosedCurve(Pen pen, Point[] points)
        {
            throw new NotImplementedException();
        }

        public void DrawClosedCurve(Pen pen, PointF[] points)
        {
            throw new NotImplementedException();
        }

        public void DrawClosedCurve(Pen pen, Point[] points, float tension, FillMode fillmode)
        {
            throw new NotImplementedException();
        }

        public void DrawClosedCurve(Pen pen, PointF[] points, float tension, FillMode fillmode)
        {
            throw new NotImplementedException();
        }

        public void DrawCurve(Pen pen, Point[] points)
        {
            throw new NotImplementedException();
        }

        public void DrawCurve(Pen pen, PointF[] points)
        {
            throw new NotImplementedException();
        }

        public void DrawCurve(Pen pen, Point[] points, float tension)
        {
            throw new NotImplementedException();
        }

        public void DrawCurve(Pen pen, PointF[] points, float tension)
        {
            throw new NotImplementedException();
        }

        public void DrawCurve(Pen pen, PointF[] points, int offset, int numberOfSegments)
        {
            throw new NotImplementedException();
        }

        public void DrawCurve(Pen pen, Point[] points, int offset, int numberOfSegments, float tension)
        {
            throw new NotImplementedException();
        }

        public void DrawCurve(Pen pen, PointF[] points, int offset, int numberOfSegments, float tension)
        {
            throw new NotImplementedException();
        }

        public void DrawEllipse(Pen pen, Rectangle rect)
        {
            DrawEllipse(pen, rect.ToRectangleF());
        }

        public void DrawEllipse(Pen pen, RectangleF rect)
        {
            g.CurrentColor = pen.Color;
            g.LineWidth = pen.Width;
            g.DrawEllipse(rect);
        }

        public void DrawEllipse(Pen pen, float x, float y, float width, float height)
        {
            DrawEllipse(pen, new RectangleF(x, y, width, height));
        }

        public void DrawEllipse(Pen pen, int x, int y, int width, int height)
        {
            DrawEllipse(pen, new Rectangle(x, y, width, height));
        }

        public void DrawIcon(Icon icon, Rectangle targetRect)
        {
            throw new NotImplementedException();
        }

        public void DrawIcon(Icon icon, int x, int y)
        {
            throw new NotImplementedException();
        }

        public void DrawIconUnstretched(Icon icon, Rectangle targetRect)
        {
            throw new NotImplementedException();
        }

        private void DrawTexture(Image image, Action<GLTexture> drawAction)
        {
            g.CurrentColor = Color.White;
            g.Texture2DEnabled = true;
            var texture = GetCachedTexture(image);
            drawAction(texture);
            g.Texture2DEnabled = false;
        }

        public void DrawImage(Image image, Point point)
        {
            DrawTexture(image, texture => texture.Draw(point));
        }

        public void DrawImage(Image image, Point[] destPoints)
        {
            throw new NotImplementedException();
        }

        public void DrawImage(Image image, PointF point)
        {
            DrawTexture(image, texture => texture.Draw(point));
        }

        public void DrawImage(Image image, PointF[] destPoints)
        {
            throw new NotImplementedException();
        }

        public void DrawImage(Image image, Rectangle rect)
        {
            DrawTexture(image, texture => texture.Draw(rect));
        }

        public void DrawImage(Image image, RectangleF rect)
        {
            DrawTexture(image, texture => texture.Draw(rect));
        }

        public void DrawImage(Image image, float x, float y)
        {
            DrawImage(image, new PointF(x, y));
        }

        public void DrawImage(Image image, int x, int y)
        {
            DrawImage(image, new Point(x, y));
        }

        public void DrawImage(Image image, Point[] destPoints, Rectangle srcRect, GraphicsUnit srcUnit)
        {
            throw new NotImplementedException();
        }

        public void DrawImage(Image image, PointF[] destPoints, RectangleF srcRect, GraphicsUnit srcUnit)
        {
            throw new NotImplementedException();
        }

        public void DrawImage(Image image, Rectangle destRect, Rectangle srcRect, GraphicsUnit srcUnit)
        {
            throw new NotImplementedException();
        }

        public void DrawImage(Image image, RectangleF destRect, RectangleF srcRect, GraphicsUnit srcUnit)
        {
            throw new NotImplementedException();
        }

        public void DrawImage(Image image, float x, float y, float width, float height)
        {
            DrawImage(image, new RectangleF(x, y, width, height));
        }

        public void DrawImage(Image image, float x, float y, RectangleF srcRect, GraphicsUnit srcUnit)
        {
            throw new NotImplementedException();
        }

        public void DrawImage(Image image, int x, int y, int width, int height)
        {
            DrawImage(image, new Rectangle(x, y, width, height));
        }

        public void DrawImage(Image image, int x, int y, Rectangle srcRect, GraphicsUnit srcUnit)
        {
            throw new NotImplementedException();
        }

        public void DrawImage(Image image, Point[] destPoints, Rectangle srcRect, GraphicsUnit srcUnit, ImageAttributes imageAttr)
        {
            throw new NotImplementedException();
        }

        public void DrawImage(Image image, PointF[] destPoints, RectangleF srcRect, GraphicsUnit srcUnit, ImageAttributes imageAttr)
        {
            throw new NotImplementedException();
        }

        public void DrawImage(Image image, Point[] destPoints, Rectangle srcRect, GraphicsUnit srcUnit, ImageAttributes imageAttr, Graphics.DrawImageAbort callback)
        {
            throw new NotImplementedException();
        }

        public void DrawImage(Image image, PointF[] destPoints, RectangleF srcRect, GraphicsUnit srcUnit, ImageAttributes imageAttr, Graphics.DrawImageAbort callback)
        {
            throw new NotImplementedException();
        }

        public void DrawImage(Image image, Point[] destPoints, Rectangle srcRect, GraphicsUnit srcUnit, ImageAttributes imageAttr, Graphics.DrawImageAbort callback, int callbackData)
        {
            throw new NotImplementedException();
        }

        public void DrawImage(Image image, PointF[] destPoints, RectangleF srcRect, GraphicsUnit srcUnit, ImageAttributes imageAttr, Graphics.DrawImageAbort callback, int callbackData)
        {
            throw new NotImplementedException();
        }

        public void DrawImage(Image image, Rectangle destRect, float srcX, float srcY, float srcWidth, float srcHeight, GraphicsUnit srcUnit)
        {
            throw new NotImplementedException();
        }

        public void DrawImage(Image image, Rectangle destRect, int srcX, int srcY, int srcWidth, int srcHeight, GraphicsUnit srcUnit)
        {
            throw new NotImplementedException();
        }

        public void DrawImage(Image image, Rectangle destRect, float srcX, float srcY, float srcWidth, float srcHeight, GraphicsUnit srcUnit, ImageAttributes imageAttrs)
        {
            throw new NotImplementedException();
        }

        public void DrawImage(Image image, Rectangle destRect, int srcX, int srcY, int srcWidth, int srcHeight, GraphicsUnit srcUnit, ImageAttributes imageAttr)
        {
            throw new NotImplementedException();
        }

        public void DrawImage(Image image, Rectangle destRect, float srcX, float srcY, float srcWidth, float srcHeight, GraphicsUnit srcUnit, ImageAttributes imageAttrs, Graphics.DrawImageAbort callback)
        {
            throw new NotImplementedException();
        }

        public void DrawImage(Image image, Rectangle destRect, int srcX, int srcY, int srcWidth, int srcHeight, GraphicsUnit srcUnit, ImageAttributes imageAttr, Graphics.DrawImageAbort callback)
        {
            throw new NotImplementedException();
        }

        public void DrawImage(Image image, Rectangle destRect, float srcX, float srcY, float srcWidth, float srcHeight, GraphicsUnit srcUnit, ImageAttributes imageAttrs, Graphics.DrawImageAbort callback, IntPtr callbackData)
        {
            throw new NotImplementedException();
        }

        public void DrawImage(Image image, Rectangle destRect, int srcX, int srcY, int srcWidth, int srcHeight, GraphicsUnit srcUnit, ImageAttributes imageAttrs, Graphics.DrawImageAbort callback, IntPtr callbackData)
        {
            throw new NotImplementedException();
        }

        public void DrawImageUnscaled(Image image, Point point)
        {
            throw new NotImplementedException();
        }

        public void DrawImageUnscaled(Image image, Rectangle rect)
        {
            throw new NotImplementedException();
        }

        public void DrawImageUnscaled(Image image, int x, int y)
        {
            DrawImage(image, new Point(x, y));
        }

        public void DrawImageUnscaled(Image image, int x, int y, int width, int height)
        {
            DrawImageUnscaled(image, new Rectangle(x, y, width, height));
        }

        public void DrawImageUnscaledAndClipped(Image image, Rectangle rect)
        {
            throw new NotImplementedException();
        }

        public void DrawLine(Pen pen, Point pt1, Point pt2)
        {
            g.CurrentColor = pen.Color;
            g.LineWidth = pen.Width;
            g.DrawLine(pt1, pt2);
        }

        public void DrawLine(Pen pen, PointF pt1, PointF pt2)
        {
            g.CurrentColor = pen.Color;
            g.LineWidth = pen.Width;
            g.DrawLine(pt1, pt2);
        }

        public void DrawLine(Pen pen, float x1, float y1, float x2, float y2)
        {
            this.DrawLine(pen, new PointF(x1, y1), new PointF(x2, y2));
        }

        public void DrawLine(Pen pen, int x1, int y1, int x2, int y2)
        {
            this.DrawLine(pen, new Point(x1, y1), new Point(x2, y2));
        }

        public void DrawLines(Pen pen, Point[] points)
        {
            g.CurrentColor = pen.Color;
            g.LineWidth = pen.Width;
            g.DrawLines(points);
        }

        public void DrawLines(Pen pen, PointF[] points)
        {
            g.CurrentColor = pen.Color;
            g.LineWidth = pen.Width;
            g.DrawLines(points);
        }

        public void DrawPath(Pen pen, GraphicsPath path)
        {
            throw new NotImplementedException();
        }

        public void DrawPie(Pen pen, Rectangle rect, float startAngle, float sweepAngle)
        {
            DrawPie(pen, rect.ToRectangleF(), startAngle, sweepAngle);
        }

        public void DrawPie(Pen pen, RectangleF rect, float startAngle, float sweepAngle)
        {
            g.CurrentColor = pen.Color;
            g.LineWidth = pen.Width;
            g.DrawArc(rect, startAngle, sweepAngle, true);
        }

        public void DrawPie(Pen pen, float x, float y, float width, float height, float startAngle, float sweepAngle)
        {
            DrawPie(pen, new RectangleF(x, y, width, height), startAngle, sweepAngle);
        }

        public void DrawPie(Pen pen, int x, int y, int width, int height, int startAngle, int sweepAngle)
        {
            DrawPie(pen, new Rectangle(x, y, width, height), startAngle, sweepAngle);
        }

        public void DrawPolygon(Pen pen, Point[] points)
        {
            throw new NotImplementedException();
        }

        public void DrawPolygon(Pen pen, PointF[] points)
        {
            throw new NotImplementedException();
        }

        public void DrawRectangle(Pen pen, Rectangle rect)
        {
            g.CurrentColor = pen.Color;
            g.LineWidth = pen.Width;
            g.DrawRectangle(rect);
        }

        public void DrawRectangle(Pen pen, float x, float y, float width, float height)
        {
            g.CurrentColor = pen.Color;
            g.LineWidth = pen.Width;
            g.DrawRectangle(new RectangleF(x, y, width, height));
        }

        public void DrawRectangle(Pen pen, int x, int y, int width, int height)
        {
            g.CurrentColor = pen.Color;
            g.LineWidth = pen.Width;
            g.DrawRectangle(new Rectangle(x, y, width, height));
        }

        public void DrawRectangles(Pen pen, Rectangle[] rects)
        {
            foreach (var rect in rects)
                DrawRectangle(pen, rect);
        }

        public void DrawRectangles(Pen pen, RectangleF[] rects)
        {
            foreach (var rect in rects)
                DrawRectangle(pen, rect.X, rect.Y, rect.Width, rect.Height);
        }

        public void DrawString(string s, Font font, Brush brush, PointF point)
        {
            DrawString(s, font, brush, point, null);
        }

        public void DrawString(string s, Font font, Brush brush, RectangleF layoutRectangle)
        {
            DrawString(s, font, brush, layoutRectangle, null);
        }

        public void DrawString(string s, Font font, Brush brush, float x, float y)
        {
            DrawString(s, font, brush, new PointF(x, y));
        }

        public void DrawString(string s, Font font, Brush brush, PointF point, StringFormat format)
        {
            DrawString(s, font, brush, new RectangleF(point, Size.Empty), format);
        }

        public void DrawString(string s, Font font, Brush brush, RectangleF layoutRectangle, StringFormat format)
        {
            PointF currentScale = g.GlobalScale;
            g.GlobalScale = new PointF(1.0f, 1.0f);

            g.CurrentColor = Color.White;
            g.Texture2DEnabled = true;

            PointF textLocation = layoutRectangle.Location;

            if (this._pageUnit == GraphicsUnit.Millimeter)
            {
                textLocation.X *= currentScale.X;
                textLocation.Y *= currentScale.Y;
                layoutRectangle.Width *= currentScale.X;
                layoutRectangle.Height *= currentScale.Y;
            }

            Stopwatch sw = Stopwatch.StartNew();
            
            var texture = GetCachedTexture(s, font, brush, layoutRectangle.Size, format);

            Trace.WriteLine(sw.ElapsedMilliseconds);

            if (layoutRectangle.Size.IsEmpty && format != null)
            {
                switch (format.Alignment)
                {
                    case StringAlignment.Center:
                        textLocation.X -= (float)Math.Round(texture.OriginalWidth / 2.0, MidpointRounding.AwayFromZero);
                        break;
                    case StringAlignment.Far:
                        textLocation.X -= texture.OriginalWidth;
                        break;
                    case StringAlignment.Near:
                    default:
                        break;
                }

                switch (format.LineAlignment)
                {
                    case StringAlignment.Center:
                        textLocation.Y -= (float)Math.Round(texture.OriginalHeight / 2.0, MidpointRounding.AwayFromZero);
                        break;
                    case StringAlignment.Far:
                        textLocation.Y -= texture.OriginalHeight;
                        break;
                    case StringAlignment.Near:
                    default:
                        break;
                }
            }

            textLocation.X = (float)Math.Round(textLocation.X);
            textLocation.Y = (float)Math.Round(textLocation.Y);
            textLocation.X -= 0.5f;
            textLocation.Y -= 0.5f;

            texture.Draw(textLocation);            
            
            g.Texture2DEnabled = false;
            g.GlobalScale = currentScale;
        }

        public void DrawString(string s, Font font, Brush brush, float x, float y, StringFormat format)
        {
            DrawString(s, font, brush, new PointF(x, y), format);
        }

        public void EndContainer(GraphicsContainer container)
        {
            throw new NotImplementedException();
        }

        public void EnumerateMetafile(Metafile metafile, Point destPoint, Graphics.EnumerateMetafileProc callback)
        {
            throw new NotImplementedException();
        }

        public void EnumerateMetafile(Metafile metafile, Point[] destPoints, Graphics.EnumerateMetafileProc callback)
        {
            throw new NotImplementedException();
        }

        public void EnumerateMetafile(Metafile metafile, PointF destPoint, Graphics.EnumerateMetafileProc callback)
        {
            throw new NotImplementedException();
        }

        public void EnumerateMetafile(Metafile metafile, PointF[] destPoints, Graphics.EnumerateMetafileProc callback)
        {
            throw new NotImplementedException();
        }

        public void EnumerateMetafile(Metafile metafile, Rectangle destRect, Graphics.EnumerateMetafileProc callback)
        {
            throw new NotImplementedException();
        }

        public void EnumerateMetafile(Metafile metafile, RectangleF destRect, Graphics.EnumerateMetafileProc callback)
        {
            throw new NotImplementedException();
        }

        public void EnumerateMetafile(Metafile metafile, Point destPoint, Graphics.EnumerateMetafileProc callback, IntPtr callbackData)
        {
            throw new NotImplementedException();
        }

        public void EnumerateMetafile(Metafile metafile, Point[] destPoints, Graphics.EnumerateMetafileProc callback, IntPtr callbackData)
        {
            throw new NotImplementedException();
        }

        public void EnumerateMetafile(Metafile metafile, PointF destPoint, Graphics.EnumerateMetafileProc callback, IntPtr callbackData)
        {
            throw new NotImplementedException();
        }

        public void EnumerateMetafile(Metafile metafile, PointF[] destPoints, Graphics.EnumerateMetafileProc callback, IntPtr callbackData)
        {
            throw new NotImplementedException();
        }

        public void EnumerateMetafile(Metafile metafile, Rectangle destRect, Graphics.EnumerateMetafileProc callback, IntPtr callbackData)
        {
            throw new NotImplementedException();
        }

        public void EnumerateMetafile(Metafile metafile, RectangleF destRect, Graphics.EnumerateMetafileProc callback, IntPtr callbackData)
        {
            throw new NotImplementedException();
        }

        public void EnumerateMetafile(Metafile metafile, Point destPoint, Graphics.EnumerateMetafileProc callback, IntPtr callbackData, ImageAttributes imageAttr)
        {
            throw new NotImplementedException();
        }

        public void EnumerateMetafile(Metafile metafile, Point destPoint, Rectangle srcRect, GraphicsUnit srcUnit, Graphics.EnumerateMetafileProc callback)
        {
            throw new NotImplementedException();
        }

        public void EnumerateMetafile(Metafile metafile, Point[] destPoints, Graphics.EnumerateMetafileProc callback, IntPtr callbackData, ImageAttributes imageAttr)
        {
            throw new NotImplementedException();
        }

        public void EnumerateMetafile(Metafile metafile, Point[] destPoints, Rectangle srcRect, GraphicsUnit srcUnit, Graphics.EnumerateMetafileProc callback)
        {
            throw new NotImplementedException();
        }

        public void EnumerateMetafile(Metafile metafile, PointF destPoint, Graphics.EnumerateMetafileProc callback, IntPtr callbackData, ImageAttributes imageAttr)
        {
            throw new NotImplementedException();
        }

        public void EnumerateMetafile(Metafile metafile, PointF destPoint, RectangleF srcRect, GraphicsUnit srcUnit, Graphics.EnumerateMetafileProc callback)
        {
            throw new NotImplementedException();
        }

        public void EnumerateMetafile(Metafile metafile, PointF[] destPoints, Graphics.EnumerateMetafileProc callback, IntPtr callbackData, ImageAttributes imageAttr)
        {
            throw new NotImplementedException();
        }

        public void EnumerateMetafile(Metafile metafile, PointF[] destPoints, RectangleF srcRect, GraphicsUnit srcUnit, Graphics.EnumerateMetafileProc callback)
        {
            throw new NotImplementedException();
        }

        public void EnumerateMetafile(Metafile metafile, Rectangle destRect, Graphics.EnumerateMetafileProc callback, IntPtr callbackData, ImageAttributes imageAttr)
        {
            throw new NotImplementedException();
        }

        public void EnumerateMetafile(Metafile metafile, Rectangle destRect, Rectangle srcRect, GraphicsUnit srcUnit, Graphics.EnumerateMetafileProc callback)
        {
            throw new NotImplementedException();
        }

        public void EnumerateMetafile(Metafile metafile, RectangleF destRect, Graphics.EnumerateMetafileProc callback, IntPtr callbackData, ImageAttributes imageAttr)
        {
            throw new NotImplementedException();
        }

        public void EnumerateMetafile(Metafile metafile, RectangleF destRect, RectangleF srcRect, GraphicsUnit srcUnit, Graphics.EnumerateMetafileProc callback)
        {
            throw new NotImplementedException();
        }

        public void EnumerateMetafile(Metafile metafile, Point destPoint, Rectangle srcRect, GraphicsUnit srcUnit, Graphics.EnumerateMetafileProc callback, IntPtr callbackData)
        {
            throw new NotImplementedException();
        }

        public void EnumerateMetafile(Metafile metafile, Point[] destPoints, Rectangle srcRect, GraphicsUnit srcUnit, Graphics.EnumerateMetafileProc callback, IntPtr callbackData)
        {
            throw new NotImplementedException();
        }

        public void EnumerateMetafile(Metafile metafile, PointF destPoint, RectangleF srcRect, GraphicsUnit srcUnit, Graphics.EnumerateMetafileProc callback, IntPtr callbackData)
        {
            throw new NotImplementedException();
        }

        public void EnumerateMetafile(Metafile metafile, PointF[] destPoints, RectangleF srcRect, GraphicsUnit srcUnit, Graphics.EnumerateMetafileProc callback, IntPtr callbackData)
        {
            throw new NotImplementedException();
        }

        public void EnumerateMetafile(Metafile metafile, Rectangle destRect, Rectangle srcRect, GraphicsUnit srcUnit, Graphics.EnumerateMetafileProc callback, IntPtr callbackData)
        {
            throw new NotImplementedException();
        }

        public void EnumerateMetafile(Metafile metafile, RectangleF destRect, RectangleF srcRect, GraphicsUnit srcUnit, Graphics.EnumerateMetafileProc callback, IntPtr callbackData)
        {
            throw new NotImplementedException();
        }

        public void EnumerateMetafile(Metafile metafile, Point destPoint, Rectangle srcRect, GraphicsUnit unit, Graphics.EnumerateMetafileProc callback, IntPtr callbackData, ImageAttributes imageAttr)
        {
            throw new NotImplementedException();
        }

        public void EnumerateMetafile(Metafile metafile, Point[] destPoints, Rectangle srcRect, GraphicsUnit unit, Graphics.EnumerateMetafileProc callback, IntPtr callbackData, ImageAttributes imageAttr)
        {
            throw new NotImplementedException();
        }

        public void EnumerateMetafile(Metafile metafile, PointF destPoint, RectangleF srcRect, GraphicsUnit unit, Graphics.EnumerateMetafileProc callback, IntPtr callbackData, ImageAttributes imageAttr)
        {
            throw new NotImplementedException();
        }

        public void EnumerateMetafile(Metafile metafile, PointF[] destPoints, RectangleF srcRect, GraphicsUnit unit, Graphics.EnumerateMetafileProc callback, IntPtr callbackData, ImageAttributes imageAttr)
        {
            throw new NotImplementedException();
        }

        public void EnumerateMetafile(Metafile metafile, Rectangle destRect, Rectangle srcRect, GraphicsUnit unit, Graphics.EnumerateMetafileProc callback, IntPtr callbackData, ImageAttributes imageAttr)
        {
            throw new NotImplementedException();
        }

        public void EnumerateMetafile(Metafile metafile, RectangleF destRect, RectangleF srcRect, GraphicsUnit unit, Graphics.EnumerateMetafileProc callback, IntPtr callbackData, ImageAttributes imageAttr)
        {
            throw new NotImplementedException();
        }

        public void ExcludeClip(Rectangle rect)
        {
            throw new NotImplementedException();
        }

        public void ExcludeClip(Region region)
        {
            throw new NotImplementedException();
        }

        public void FillClosedCurve(Brush brush, Point[] points)
        {
            throw new NotImplementedException();
        }

        public void FillClosedCurve(Brush brush, PointF[] points)
        {
            throw new NotImplementedException();
        }

        public void FillClosedCurve(Brush brush, Point[] points, FillMode fillmode)
        {
            throw new NotImplementedException();
        }

        public void FillClosedCurve(Brush brush, PointF[] points, FillMode fillmode)
        {
            throw new NotImplementedException();
        }

        public void FillClosedCurve(Brush brush, Point[] points, FillMode fillmode, float tension)
        {
            throw new NotImplementedException();
        }

        public void FillClosedCurve(Brush brush, PointF[] points, FillMode fillmode, float tension)
        {
            throw new NotImplementedException();
        }

        public void FillEllipse(Brush brush, Rectangle rect)
        {
            throw new NotImplementedException();
        }

        public void FillEllipse(Brush brush, RectangleF rect)
        {
            throw new NotImplementedException();
        }

        public void FillEllipse(Brush brush, float x, float y, float width, float height)
        {
            throw new NotImplementedException();
        }

        public void FillEllipse(Brush brush, int x, int y, int width, int height)
        {
            throw new NotImplementedException();
        }

        public void FillPath(Brush brush, GraphicsPath path)
        {
            throw new NotImplementedException();
        }

        public void FillPie(Brush brush, Rectangle rect, float startAngle, float sweepAngle)
        {
            throw new NotImplementedException();
        }

        public void FillPie(Brush brush, float x, float y, float width, float height, float startAngle, float sweepAngle)
        {
            throw new NotImplementedException();
        }

        public void FillPie(Brush brush, int x, int y, int width, int height, int startAngle, int sweepAngle)
        {
            throw new NotImplementedException();
        }

        public void FillPolygon(Brush brush, Point[] points)
        {
            throw new NotImplementedException();
        }

        public void FillPolygon(Brush brush, PointF[] points)
        {
            throw new NotImplementedException();
        }

        public void FillPolygon(Brush brush, Point[] points, FillMode fillMode)
        {
            throw new NotImplementedException();
        }

        public void FillPolygon(Brush brush, PointF[] points, FillMode fillMode)
        {
            throw new NotImplementedException();
        }

        private Color[] ColorsFromBrush(Brush brush)
        {
            if (brush is SolidBrush)
            {
                SolidBrush solid = (SolidBrush)brush;
                g.CurrentColor = solid.Color;
                return null;
            }

            if (brush is LinearGradientBrush)
            {
                LinearGradientBrush gradient = (LinearGradientBrush)brush;
                Color[] colors = new Color[4];
                if (FloatEquals(gradient.Transform.Elements[2], -0.5f))
                {
                    colors[0] = gradient.LinearColors[0];
                    colors[1] = gradient.LinearColors[0];
                    colors[2] = gradient.LinearColors[1];
                    colors[3] = gradient.LinearColors[1];
                }
                else
                {
                    colors[0] = gradient.LinearColors[0];
                    colors[1] = gradient.LinearColors[1];
                    colors[2] = gradient.LinearColors[1];
                    colors[3] = gradient.LinearColors[0];
                }
                return colors;
            }

            return null;
        }

        public void FillRectangle(Brush brush, Rectangle rect)
        {
            g.FillRectangle(rect, ColorsFromBrush(brush));
        }

        public void FillRectangle(Brush brush, RectangleF rect)
        {
            g.FillRectangle(rect, ColorsFromBrush(brush));
        }

        public void FillRectangle(Brush brush, float x, float y, float width, float height)
        {
            FillRectangle(brush, new RectangleF(x, y, width, height));
        }

        public void FillRectangle(Brush brush, int x, int y, int width, int height)
        {
            FillRectangle(brush, new Rectangle(x, y, width, height));
        }

        public void FillRectangles(Brush brush, Rectangle[] rects)
        {
            foreach (var rect in rects)
            {
                FillRectangle(brush, rect);
            }
        }

        public void FillRectangles(Brush brush, RectangleF[] rects)
        {
            foreach (var rect in rects)
            {
                FillRectangle(brush, rect);
            }
        }

        public void FillRegion(Brush brush, Region region)
        {
            throw new NotImplementedException();
        }

        public void Flush()
        {
            throw new NotImplementedException();
        }

        public void Flush(FlushIntention intention)
        {
            throw new NotImplementedException();
        }

        public object GetContextInfo()
        {
            throw new NotImplementedException();
        }

        public IntPtr GetHdc()
        {
            throw new NotImplementedException();
        }

        public Color GetNearestColor(Color color)
        {
            throw new NotImplementedException();
        }

        public void IntersectClip(Rectangle rect)
        {
            throw new NotImplementedException();
        }

        public void IntersectClip(RectangleF rect)
        {
            throw new NotImplementedException();
        }

        public void IntersectClip(Region region)
        {
            throw new NotImplementedException();
        }

        public bool IsVisible(Point point)
        {
            throw new NotImplementedException();
        }

        public bool IsVisible(PointF point)
        {
            throw new NotImplementedException();
        }

        public bool IsVisible(Rectangle rect)
        {
            throw new NotImplementedException();
        }

        public bool IsVisible(RectangleF rect)
        {
            throw new NotImplementedException();
        }

        public bool IsVisible(float x, float y)
        {
            throw new NotImplementedException();
        }

        public bool IsVisible(int x, int y)
        {
            throw new NotImplementedException();
        }

        public bool IsVisible(float x, float y, float width, float height)
        {
            throw new NotImplementedException();
        }

        public bool IsVisible(int x, int y, int width, int height)
        {
            throw new NotImplementedException();
        }

        public Region[] MeasureCharacterRanges(string text, Font font, RectangleF layoutRect, StringFormat stringFormat)
        {
            throw new NotImplementedException();
        }

        public SizeF MeasureString(string text, Font font)
        {
            throw new NotImplementedException();
        }

        public SizeF MeasureString(string text, Font font, int width)
        {
            throw new NotImplementedException();
        }

        public SizeF MeasureString(string text, Font font, SizeF layoutArea)
        {
            throw new NotImplementedException();
        }

        public SizeF MeasureString(string text, Font font, int width, StringFormat format)
        {
            throw new NotImplementedException();
        }

        public SizeF MeasureString(string text, Font font, PointF origin, StringFormat stringFormat)
        {
            throw new NotImplementedException();
        }

        public SizeF MeasureString(string text, Font font, SizeF layoutArea, StringFormat stringFormat)
        {
            throw new NotImplementedException();
        }

        public SizeF MeasureString(string text, Font font, SizeF layoutArea, StringFormat stringFormat, out int charactersFitted, out int linesFilled)
        {
            throw new NotImplementedException();
        }

        public void MultiplyTransform(Matrix matrix)
        {
            throw new NotImplementedException();
        }

        public void MultiplyTransform(Matrix matrix, MatrixOrder order)
        {
            throw new NotImplementedException();
        }

        public void ReleaseHdc()
        {
            throw new NotImplementedException();
        }

        public void ReleaseHdc(IntPtr hdc)
        {
            throw new NotImplementedException();
        }

        public void ReleaseHdcInternal(IntPtr hdc)
        {
            throw new NotImplementedException();
        }

        public void ResetClip()
        {
            throw new NotImplementedException();
        }

        public void ResetTransform()
        {
            _transform.Reset();
            Transform = _transform;
        }

        public void Restore(GraphicsState gstate)
        {
            throw new NotImplementedException();
        }

        public void RotateTransform(float angle)
        {
            _transform.Rotate(angle);
            Transform = _transform;
        }

        public void RotateTransform(float angle, MatrixOrder order)
        {
            _transform.Rotate(angle, order);
            Transform = _transform;
        }

        public GraphicsState Save()
        {
            throw new NotImplementedException();
        }

        public void ScaleTransform(float sx, float sy)
        {
            _transform.Scale(sx, sy);
            Transform = _transform;
        }

        public void ScaleTransform(float sx, float sy, MatrixOrder order)
        {
            _transform.Scale(sx, sy, order);
            Transform = _transform;
        }

        public void SetClip(Graphics g)
        {
            throw new NotImplementedException();
        }

        public void SetClip(GraphicsPath path)
        {
            throw new NotImplementedException();
        }

        public void SetClip(Rectangle rect)
        {
            throw new NotImplementedException();
        }

        public void SetClip(RectangleF rect)
        {
            throw new NotImplementedException();
        }

        public void SetClip(Graphics g, CombineMode combineMode)
        {
            throw new NotImplementedException();
        }

        public void SetClip(GraphicsPath path, CombineMode combineMode)
        {
            throw new NotImplementedException();
        }

        public void SetClip(Rectangle rect, CombineMode combineMode)
        {
            throw new NotImplementedException();
        }

        public void SetClip(RectangleF rect, CombineMode combineMode)
        {
            throw new NotImplementedException();
        }

        public void SetClip(Region region, CombineMode combineMode)
        {
            throw new NotImplementedException();
        }

        public void TransformPoints(CoordinateSpace destSpace, CoordinateSpace srcSpace, Point[] pts)
        {
            throw new NotImplementedException();
        }

        public void TransformPoints(CoordinateSpace destSpace, CoordinateSpace srcSpace, PointF[] pts)
        {
            throw new NotImplementedException();
        }

        public void TranslateClip(float dx, float dy)
        {
            throw new NotImplementedException();
        }

        public void TranslateClip(int dx, int dy)
        {
            throw new NotImplementedException();
        }

        public void TranslateTransform(float dx, float dy)
        {
            _transform.Translate(dx, dy);
            Transform = _transform;
        }

        public void TranslateTransform(float dx, float dy, MatrixOrder order)
        {
            _transform.Translate(dx, dy, order);
            Transform = _transform;
        }
    }
}
