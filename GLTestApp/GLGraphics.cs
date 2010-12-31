using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GraphicsInterface;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using GLWrapper;

namespace GLTestApp
{
    public class GLGraphics : IGraphics
    {
        static GLTexture _gdiTexture = null;

        private static int nextPow2(int n)
        {
            int x = 2;
            while (x < n)
                x <<= 1;
            return x;
        }
        
        GLCanvas g;

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
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public CompositingQuality CompositingQuality
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

        public float DpiX
        {
            get { throw new NotImplementedException(); }
        }

        public float DpiY
        {
            get { throw new NotImplementedException(); }
        }

        public InterpolationMode InterpolationMode
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
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public GraphicsUnit PageUnit
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

        public PixelOffsetMode PixelOffsetMode
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

        public Point RenderingOrigin
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
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public Matrix Transform
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
            throw new NotImplementedException();
        }

        public void DrawArc(Pen pen, Rectangle rect, float startAngle, float sweepAngle)
        {
            throw new NotImplementedException();
        }

        public void DrawArc(Pen pen, RectangleF rect, float startAngle, float sweepAngle)
        {
            throw new NotImplementedException();
        }

        public void DrawArc(Pen pen, float x, float y, float width, float height, float startAngle, float sweepAngle)
        {
            throw new NotImplementedException();
        }

        public void DrawArc(Pen pen, int x, int y, int width, int height, int startAngle, int sweepAngle)
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

        public void DrawEllipse(Pen pen, RectangleF rect)
        {
            throw new NotImplementedException();
        }

        public void DrawEllipse(Pen pen, float x, float y, float width, float height)
        {
            throw new NotImplementedException();
        }

        public void DrawEllipse(Pen pen, int x, int y, int width, int height)
        {
            throw new NotImplementedException();
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

        public void DrawImage(Image image, Point point)
        {
            throw new NotImplementedException();
        }

        public void DrawImage(Image image, Point[] destPoints)
        {
            throw new NotImplementedException();
        }

        public void DrawImage(Image image, PointF point)
        {
            throw new NotImplementedException();
        }

        public void DrawImage(Image image, PointF[] destPoints)
        {
            throw new NotImplementedException();
        }

        public void DrawImage(Image image, Rectangle rect)
        {
            throw new NotImplementedException();
        }

        public void DrawImage(Image image, RectangleF rect)
        {
            throw new NotImplementedException();
        }

        public void DrawImage(Image image, float x, float y)
        {
            throw new NotImplementedException();
        }

        public void DrawImage(Image image, int x, int y)
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

        public void DrawImage(Image image, float x, float y, RectangleF srcRect, GraphicsUnit srcUnit)
        {
            throw new NotImplementedException();
        }

        public void DrawImage(Image image, int x, int y, int width, int height)
        {
            throw new NotImplementedException();
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

        public void DrawImageUnscaled(Image image, Rectangle rect)
        {
            throw new NotImplementedException();
        }

        public void DrawImageUnscaled(Image image, int x, int y)
        {
            throw new NotImplementedException();
        }

        public void DrawImageUnscaled(Image image, int x, int y, int width, int height)
        {
            throw new NotImplementedException();
        }

        public void DrawImageUnscaledAndClipped(Image image, Rectangle rect)
        {
            throw new NotImplementedException();
        }

        public void DrawLine(Pen pen, Point pt1, Point pt2)
        {
            throw new NotImplementedException();
        }

        public void DrawLine(Pen pen, PointF pt1, PointF pt2)
        {
            throw new NotImplementedException();
        }

        public void DrawLine(Pen pen, float x1, float y1, float x2, float y2)
        {
            throw new NotImplementedException();
        }

        public void DrawLine(Pen pen, int x1, int y1, int x2, int y2)
        {
            throw new NotImplementedException();
        }

        public void DrawLines(Pen pen, Point[] points)
        {
            throw new NotImplementedException();
        }

        public void DrawLines(Pen pen, PointF[] points)
        {
            g.CurrentColor = pen.Color;
            g.DrawLines(points);
        }

        public void DrawPath(Pen pen, GraphicsPath path)
        {
            throw new NotImplementedException();
        }

        public void DrawPie(Pen pen, Rectangle rect, float startAngle, float sweepAngle)
        {
            throw new NotImplementedException();
        }

        public void DrawPie(Pen pen, RectangleF rect, float startAngle, float sweepAngle)
        {
            throw new NotImplementedException();
        }

        public void DrawPie(Pen pen, float x, float y, float width, float height, float startAngle, float sweepAngle)
        {
            throw new NotImplementedException();
        }

        public void DrawPie(Pen pen, int x, int y, int width, int height, int startAngle, int sweepAngle)
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

        public void DrawRectangle(Pen pen, float x, float y, float width, float height)
        {
            throw new NotImplementedException();
        }

        public void DrawRectangle(Pen pen, int x, int y, int width, int height)
        {
            throw new NotImplementedException();
        }

        public void DrawRectangles(Pen pen, Rectangle[] rects)
        {
            throw new NotImplementedException();
        }

        public void DrawRectangles(Pen pen, RectangleF[] rects)
        {
            throw new NotImplementedException();
        }

        private void DrawGDITexture(Action<Graphics> draw)
        {
            int width = nextPow2(g.CanvasSize.Width);
            int height = nextPow2(g.CanvasSize.Height);

            using (Bitmap bitmap = new Bitmap(width, height))
            {
                using (Graphics gdi = Graphics.FromImage(bitmap))
                {
                    draw(gdi);
                }

                g.Texture2DEnabled = true;
                g.CurrentColor = Color.White;

                if (_gdiTexture == null)
                    _gdiTexture = new GLTexture(bitmap);
                else
                    _gdiTexture.Update(bitmap);

                _gdiTexture.Draw(PointF.Empty);
                g.Texture2DEnabled = false;
            }      
        }

        private void DrawGDIPixels(Action<Graphics> draw)
        {
            int width = g.CanvasSize.Width;
            int height = g.CanvasSize.Height;

            using (Bitmap bitmap = new Bitmap(width, height))
            {
                using (Graphics gdi = Graphics.FromImage(bitmap))
                {
                    draw(gdi);
                }

                g.DrawPixels(bitmap);
            }        
        }

        private void DrawGDI(Action<Graphics> draw)
        {
            DrawGDITexture(draw);
        }

        public void DrawString(string s, Font font, Brush brush, PointF point)
        {
            DrawGDI(gdi => gdi.DrawString(s, font, brush, point));
        }

        public void DrawString(string s, Font font, Brush brush, RectangleF layoutRectangle)
        {
            DrawGDI(gdi => gdi.DrawString(s, font, brush, layoutRectangle));
        }

        public void DrawString(string s, Font font, Brush brush, float x, float y)
        {
            DrawGDI(gdi => gdi.DrawString(s, font, brush, x, y));
        }

        public void DrawString(string s, Font font, Brush brush, PointF point, StringFormat format)
        {
            DrawGDI(gdi => gdi.DrawString(s, font, brush, point, format));
        }

        public void DrawString(string s, Font font, Brush brush, RectangleF layoutRectangle, StringFormat format)
        {
            DrawGDI(gdi => gdi.DrawString(s, font, brush, layoutRectangle, format));
        }

        public void DrawString(string s, Font font, Brush brush, float x, float y, StringFormat format)
        {
            DrawGDI(gdi => gdi.DrawString(s, font, brush, x, y, format));
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

        public void FillRectangle(Brush brush, Rectangle rect)
        {
            throw new NotImplementedException();
        }

        public void FillRectangle(Brush brush, RectangleF rect)
        {
            throw new NotImplementedException();
        }

        public void FillRectangle(Brush brush, float x, float y, float width, float height)
        {
            throw new NotImplementedException();
        }

        public void FillRectangle(Brush brush, int x, int y, int width, int height)
        {
            throw new NotImplementedException();
        }

        public void FillRectangles(Brush brush, Rectangle[] rects)
        {
            throw new NotImplementedException();
        }

        public void FillRectangles(Brush brush, RectangleF[] rects)
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

        public void Restore(GraphicsState gstate)
        {
            throw new NotImplementedException();
        }

        public void RotateTransform(float angle)
        {
            throw new NotImplementedException();
        }

        public void RotateTransform(float angle, MatrixOrder order)
        {
            throw new NotImplementedException();
        }

        public GraphicsState Save()
        {
            throw new NotImplementedException();
        }

        public void ScaleTransform(float sx, float sy)
        {
            throw new NotImplementedException();
        }

        public void ScaleTransform(float sx, float sy, MatrixOrder order)
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

        public void TranslateTransform(float dx, float dy, MatrixOrder order)
        {
            throw new NotImplementedException();
        }
    }
}
