using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;

namespace GraphicsImplementation
{
    public class GDIGraphics : IGraphics
    {
        Graphics g;

        public GDIGraphics(Graphics graphics)
        {
            g = graphics;
        }

        public Graphics Graphics
        {
            get { return g; }
        }

        public Region Clip
        {
            get { return g.Clip; }
            set { g.Clip = value; }
        }

        public RectangleF ClipBounds
        {
            get { return g.ClipBounds; }
        }

        public CompositingMode CompositingMode
        {
            get { return g.CompositingMode; }
            set { g.CompositingMode = value; }
        }

        public CompositingQuality CompositingQuality
        {
            get { return g.CompositingQuality; }
            set { g.CompositingQuality = value; }
        }

        public float DpiX
        {
            get { return g.DpiX; }
        }

        public float DpiY
        {
            get { return g.DpiY; }
        }

        public InterpolationMode InterpolationMode
        {
            get { return g.InterpolationMode; }
            set { g.InterpolationMode = value; }
        }

        public bool IsClipEmpty
        {
            get { return g.IsClipEmpty; }
        }

        public bool IsVisibleClipEmpty
        {
            get { return g.IsVisibleClipEmpty; }
        }

        public float PageScale
        {
            get { return g.PageScale; }
            set { g.PageScale = value; }
        }

        public GraphicsUnit PageUnit
        {
            get { return g.PageUnit; }
            set { g.PageUnit = value; }
        }

        public PixelOffsetMode PixelOffsetMode
        {
            get { return g.PixelOffsetMode; }
            set { g.PixelOffsetMode = value; }
        }

        public Point RenderingOrigin
        {
            get { return g.RenderingOrigin; }
            set { g.RenderingOrigin = value; }
        }

        public SmoothingMode SmoothingMode
        {
            get { return g.SmoothingMode; }
            set { g.SmoothingMode = value; }
        }

        public int TextContrast
        {
            get { return g.TextContrast; }
            set { g.TextContrast = value; }
        }

        public TextRenderingHint TextRenderingHint
        {
            get { return g.TextRenderingHint; }
            set { g.TextRenderingHint = value; }
        }

        public Matrix Transform
        {
            get { return g.Transform; }
            set { g.Transform = value; }
        }

        public RectangleF VisibleClipBounds
        {
            get { return g.VisibleClipBounds; }
        }

        public void AddMetafileComment(byte[] data)
        {
            g.AddMetafileComment(data);
        }

        public GraphicsContainer BeginContainer()
        {
            return g.BeginContainer();
        }

        public GraphicsContainer BeginContainer(Rectangle dstrect, Rectangle srcrect, GraphicsUnit unit)
        {
            return g.BeginContainer(dstrect, srcrect, unit);
        }

        public GraphicsContainer BeginContainer(RectangleF dstrect, RectangleF srcrect, GraphicsUnit unit)
        {
            return g.BeginContainer(dstrect, srcrect, unit);
        }

        public void Clear(Color color)
        {
            g.Clear(color);
        }

        public void CopyFromScreen(Point upperLeftSource, Point upperLeftDestination, Size blockRegionSize)
        {
            g.CopyFromScreen(upperLeftSource, upperLeftDestination, blockRegionSize);
        }

        public void CopyFromScreen(Point upperLeftSource, Point upperLeftDestination, Size blockRegionSize, CopyPixelOperation copyPixelOperation)
        {
            g.CopyFromScreen(upperLeftSource, upperLeftDestination, blockRegionSize, copyPixelOperation);
        }

        public void CopyFromScreen(int sourceX, int sourceY, int destinationX, int destinationY, Size blockRegionSize, CopyPixelOperation copyPixelOperation)
        {
            g.CopyFromScreen(sourceX, sourceY, destinationX, destinationY, blockRegionSize, copyPixelOperation);
        }

        public void CopyFromScreen(int sourceX, int sourceY, int destinationX, int destinationY, Size blockRegionSize)
        {
            g.CopyFromScreen(sourceX, sourceY, destinationX, destinationY, blockRegionSize);
        }        

        public void Dispose()
        {
            g.Dispose();
        }

        public void DrawArc(Pen pen, Rectangle rect, float startAngle, float sweepAngle)
        {
            g.DrawArc(pen, rect, startAngle, sweepAngle);
        }

        public void DrawArc(Pen pen, RectangleF rect, float startAngle, float sweepAngle)
        {
            g.DrawArc(pen, rect, startAngle, sweepAngle);
        }

        public void DrawArc(Pen pen, float x, float y, float width, float height, float startAngle, float sweepAngle)
        {
            g.DrawArc(pen, x, y, width, height, startAngle, sweepAngle);
        }

        public void DrawArc(Pen pen, int x, int y, int width, int height, int startAngle, int sweepAngle)
        {
            g.DrawArc(pen, x, y, width, height, startAngle, sweepAngle);
        }

        public void DrawBezier(Pen pen, Point pt1, Point pt2, Point pt3, Point pt4)
        {
            g.DrawBezier(pen, pt1, pt2, pt3, pt4);
        }

        public void DrawBezier(Pen pen, PointF pt1, PointF pt2, PointF pt3, PointF pt4)
        {
            g.DrawBezier(pen, pt1, pt2, pt3, pt4);
        }

        public void DrawBezier(Pen pen, float x1, float y1, float x2, float y2, float x3, float y3, float x4, float y4)
        {
            g.DrawBezier(pen, x1, y1, x2, y2, x3, y3, x4, y4);
        }

        public void DrawBeziers(Pen pen, Point[] points)
        {
            g.DrawBeziers(pen, points);
        }

        public void DrawBeziers(Pen pen, PointF[] points)
        {
            g.DrawBeziers(pen, points);
        }

        public void DrawClosedCurve(Pen pen, Point[] points)
        {
            g.DrawClosedCurve(pen, points);
        }

        public void DrawClosedCurve(Pen pen, PointF[] points)
        {
            g.DrawClosedCurve(pen, points);
        }

        public void DrawClosedCurve(Pen pen, Point[] points, float tension, FillMode fillmode)
        {
            g.DrawClosedCurve(pen, points, tension, fillmode);
        }

        public void DrawClosedCurve(Pen pen, PointF[] points, float tension, FillMode fillmode)
        {
            g.DrawClosedCurve(pen, points, tension, fillmode);
        }

        public void DrawCurve(Pen pen, Point[] points)
        {
            g.DrawCurve(pen, points);
        }

        public void DrawCurve(Pen pen, PointF[] points)
        {
            g.DrawCurve(pen, points);
        }

        public void DrawCurve(Pen pen, Point[] points, float tension)
        {
            g.DrawCurve(pen, points, tension);
        }

        public void DrawCurve(Pen pen, PointF[] points, float tension)
        {
            g.DrawCurve(pen, points, tension);
        }

        public void DrawCurve(Pen pen, PointF[] points, int offset, int numberOfSegments)
        {
            g.DrawCurve(pen, points, offset, numberOfSegments);
        }

        public void DrawCurve(Pen pen, Point[] points, int offset, int numberOfSegments, float tension)
        {
            g.DrawCurve(pen, points, offset, numberOfSegments, tension);
        }

        public void DrawCurve(Pen pen, PointF[] points, int offset, int numberOfSegments, float tension)
        {
            g.DrawCurve(pen, points, offset, numberOfSegments, tension);
        }

        public void DrawEllipse(Pen pen, Rectangle rect)
        {
            g.DrawEllipse(pen, rect);
        }

        public void DrawEllipse(Pen pen, RectangleF rect)
        {
            g.DrawEllipse(pen, rect);
        }

        public void DrawEllipse(Pen pen, float x, float y, float width, float height)
        {
            g.DrawEllipse(pen, x, y, width, height);
        }

        public void DrawEllipse(Pen pen, int x, int y, int width, int height)
        {
            g.DrawEllipse(pen, x, y, width, height);
        }

        public void DrawIcon(Icon icon, Rectangle targetRect)
        {
            g.DrawIcon(icon, targetRect);
        }

        public void DrawIcon(Icon icon, int x, int y)
        {
            g.DrawIcon(icon, x, y);
        }

        public void DrawIconUnstretched(Icon icon, Rectangle targetRect)
        {
            g.DrawIconUnstretched(icon, targetRect);
        }

        public void DrawImage(Image image, Point point)
        {
            g.DrawImage(image, point);
        }

        public void DrawImage(Image image, Point[] destPoints)
        {
            g.DrawImage(image, destPoints);
        }

        public void DrawImage(Image image, PointF point)
        {
            g.DrawImage(image, point);
        }

        public void DrawImage(Image image, PointF[] destPoints)
        {
            g.DrawImage(image, destPoints);
        }

        public void DrawImage(Image image, Rectangle rect)
        {
            g.DrawImage(image, rect);
        }

        public void DrawImage(Image image, RectangleF rect)
        {
            g.DrawImage(image, rect);
        }

        public void DrawImage(Image image, float x, float y)
        {
            g.DrawImage(image, x, y);
        }

        public void DrawImage(Image image, int x, int y)
        {
            g.DrawImage(image, x, y);
        }

        public void DrawImage(Image image, Point[] destPoints, Rectangle srcRect, GraphicsUnit srcUnit)
        {
            g.DrawImage(image, destPoints, srcRect, srcUnit);
        }

        public void DrawImage(Image image, PointF[] destPoints, RectangleF srcRect, GraphicsUnit srcUnit)
        {
            g.DrawImage(image, destPoints, srcRect, srcUnit);
        }

        public void DrawImage(Image image, Rectangle destRect, Rectangle srcRect, GraphicsUnit srcUnit)
        {
            g.DrawImage(image, destRect, srcRect, srcUnit);
        }

        public void DrawImage(Image image, RectangleF destRect, RectangleF srcRect, GraphicsUnit srcUnit)
        {
            g.DrawImage(image, destRect, srcRect, srcUnit);
        }

        public void DrawImage(Image image, float x, float y, float width, float height)
        {
            g.DrawImage(image, x, y, width, height);
        }

        public void DrawImage(Image image, float x, float y, RectangleF srcRect, GraphicsUnit srcUnit)
        {
            g.DrawImage(image, x, y, srcRect, srcUnit);
        }

        public void DrawImage(Image image, int x, int y, int width, int height)
        {
            g.DrawImage(image, x, y, width, height);
        }

        public void DrawImage(Image image, int x, int y, Rectangle srcRect, GraphicsUnit srcUnit)
        {
            g.DrawImage(image, x, y, srcRect, srcUnit);
        }

        public void DrawImage(Image image, Point[] destPoints, Rectangle srcRect, GraphicsUnit srcUnit, ImageAttributes imageAttr)
        {
            g.DrawImage(image, destPoints, srcRect, srcUnit, imageAttr);
        }

        public void DrawImage(Image image, PointF[] destPoints, RectangleF srcRect, GraphicsUnit srcUnit, ImageAttributes imageAttr)
        {
            g.DrawImage(image, destPoints, srcRect, srcUnit, imageAttr);
        }

        public void DrawImage(Image image, Point[] destPoints, Rectangle srcRect, GraphicsUnit srcUnit, ImageAttributes imageAttr, Graphics.DrawImageAbort callback)
        {
            g.DrawImage(image, destPoints, srcRect, srcUnit, imageAttr, callback);
        }

        public void DrawImage(Image image, PointF[] destPoints, RectangleF srcRect, GraphicsUnit srcUnit, ImageAttributes imageAttr, Graphics.DrawImageAbort callback)
        {
            g.DrawImage(image, destPoints, srcRect, srcUnit, imageAttr, callback);
        }

        public void DrawImage(Image image, Point[] destPoints, Rectangle srcRect, GraphicsUnit srcUnit, ImageAttributes imageAttr, Graphics.DrawImageAbort callback, int callbackData)
        {
            g.DrawImage(image, destPoints, srcRect, srcUnit, imageAttr, callback, callbackData);
        }

        public void DrawImage(Image image, PointF[] destPoints, RectangleF srcRect, GraphicsUnit srcUnit, ImageAttributes imageAttr, Graphics.DrawImageAbort callback, int callbackData)
        {
            g.DrawImage(image, destPoints, srcRect, srcUnit, imageAttr, callback, callbackData);
        }

        public void DrawImage(Image image, Rectangle destRect, float srcX, float srcY, float srcWidth, float srcHeight, GraphicsUnit srcUnit)
        {
            g.DrawImage(image, destRect, srcX, srcY, srcWidth, srcHeight, srcUnit);
        }

        public void DrawImage(Image image, Rectangle destRect, int srcX, int srcY, int srcWidth, int srcHeight, GraphicsUnit srcUnit)
        {
            g.DrawImage(image, destRect, srcX, srcY, srcWidth, srcHeight, srcUnit);
        }

        public void DrawImage(Image image, Rectangle destRect, float srcX, float srcY, float srcWidth, float srcHeight, GraphicsUnit srcUnit, ImageAttributes imageAttrs)
        {
            g.DrawImage(image, destRect, srcX, srcY, srcWidth, srcHeight, srcUnit, imageAttrs);
        }

        public void DrawImage(Image image, Rectangle destRect, int srcX, int srcY, int srcWidth, int srcHeight, GraphicsUnit srcUnit, ImageAttributes imageAttr)
        {
            g.DrawImage(image, destRect, srcX, srcY, srcWidth, srcHeight, srcUnit, imageAttr);
        }

        public void DrawImage(Image image, Rectangle destRect, float srcX, float srcY, float srcWidth, float srcHeight, GraphicsUnit srcUnit, ImageAttributes imageAttrs, Graphics.DrawImageAbort callback)
        {
            g.DrawImage(image, destRect, srcX, srcY, srcWidth, srcHeight, srcUnit, imageAttrs, callback);
        }

        public void DrawImage(Image image, Rectangle destRect, int srcX, int srcY, int srcWidth, int srcHeight, GraphicsUnit srcUnit, ImageAttributes imageAttr, Graphics.DrawImageAbort callback)
        {
            g.DrawImage(image, destRect, srcX, srcY, srcWidth, srcHeight, srcUnit, imageAttr, callback);
        }

        public void DrawImage(Image image, Rectangle destRect, float srcX, float srcY, float srcWidth, float srcHeight, GraphicsUnit srcUnit, ImageAttributes imageAttrs, Graphics.DrawImageAbort callback, IntPtr callbackData)
        {
            g.DrawImage(image, destRect, srcX, srcY, srcWidth, srcHeight, srcUnit, imageAttrs, callback, callbackData);
        }

        public void DrawImage(Image image, Rectangle destRect, int srcX, int srcY, int srcWidth, int srcHeight, GraphicsUnit srcUnit, ImageAttributes imageAttrs, Graphics.DrawImageAbort callback, IntPtr callbackData)
        {
            g.DrawImage(image, destRect, srcX, srcY, srcWidth, srcHeight, srcUnit, imageAttrs, callback, callbackData);
        }

        public void DrawImageUnscaled(Image image, Point point)
        {
            g.DrawImageUnscaled(image, point);
        }

        public void DrawImageUnscaled(Image image, Rectangle rect)
        {
            g.DrawImageUnscaled(image, rect);
        }

        public void DrawImageUnscaled(Image image, int x, int y)
        {
            g.DrawImageUnscaled(image, x, y);
        }

        public void DrawImageUnscaled(Image image, int x, int y, int width, int height)
        {
            g.DrawImageUnscaled(image, x, y, width, height);
        }

        public void DrawImageUnscaledAndClipped(Image image, Rectangle rect)
        {
            g.DrawImageUnscaledAndClipped(image, rect);
        }

        public void DrawLine(Pen pen, Point pt1, Point pt2)
        {
            g.DrawLine(pen, pt1, pt2);
        }

        public void DrawLine(Pen pen, PointF pt1, PointF pt2)
        {
            g.DrawLine(pen, pt1, pt2);
        }

        public void DrawLine(Pen pen, float x1, float y1, float x2, float y2)
        {
            g.DrawLine(pen, x1, y1, x2, y2);
        }

        public void DrawLine(Pen pen, int x1, int y1, int x2, int y2)
        {
            g.DrawLine(pen, x1, y1, x2, y2);
        }

        public void DrawLines(Pen pen, Point[] points)
        {
            g.DrawLines(pen, points);
        }

        public void DrawLines(Pen pen, PointF[] points)
        {
            g.DrawLines(pen, points);
        }

        public void DrawPath(Pen pen, GraphicsPath path)
        {
            g.DrawPath(pen, path);
        }

        public void DrawPie(Pen pen, Rectangle rect, float startAngle, float sweepAngle)
        {
            g.DrawPie(pen, rect, startAngle, sweepAngle);
        }

        public void DrawPie(Pen pen, RectangleF rect, float startAngle, float sweepAngle)
        {
            g.DrawPie(pen, rect, startAngle, sweepAngle);
        }

        public void DrawPie(Pen pen, float x, float y, float width, float height, float startAngle, float sweepAngle)
        {
            g.DrawPie(pen, x, y, width, height, startAngle, sweepAngle);
        }

        public void DrawPie(Pen pen, int x, int y, int width, int height, int startAngle, int sweepAngle)
        {
            g.DrawPie(pen, x, y, width, height, startAngle, sweepAngle);
        }

        public void DrawPolygon(Pen pen, Point[] points)
        {
            g.DrawPolygon(pen, points);
        }

        public void DrawPolygon(Pen pen, PointF[] points)
        {
            g.DrawPolygon(pen, points);
        }

        public void DrawRectangle(Pen pen, Rectangle rect)
        {
            g.DrawRectangle(pen, rect);
        }

        public void DrawRectangle(Pen pen, float x, float y, float width, float height)
        {
            g.DrawRectangle(pen, x, y, width, height);
        }

        public void DrawRectangle(Pen pen, int x, int y, int width, int height)
        {
            g.DrawRectangle(pen, x, y, width, height);
        }

        public void DrawRectangles(Pen pen, Rectangle[] rects)
        {
            g.DrawRectangles(pen, rects);
        }

        public void DrawRectangles(Pen pen, RectangleF[] rects)
        {
            g.DrawRectangles(pen, rects);
        }

        public void DrawString(string s, Font font, Brush brush, PointF point)
        {
            g.DrawString(s, font, brush, point);
        }

        public void DrawString(string s, Font font, Brush brush, RectangleF layoutRectangle)
        {
            g.DrawString(s, font, brush, layoutRectangle);
        }

        public void DrawString(string s, Font font, Brush brush, float x, float y)
        {
            g.DrawString(s, font, brush, x, y);
        }

        public void DrawString(string s, Font font, Brush brush, PointF point, StringFormat format)
        {
            g.DrawString(s, font, brush, point, format);
        }

        public void DrawString(string s, Font font, Brush brush, RectangleF layoutRectangle, StringFormat format)
        {
            g.DrawString(s, font, brush, layoutRectangle, format);
        }

        public void DrawString(string s, Font font, Brush brush, float x, float y, StringFormat format)
        {
            g.DrawString(s, font, brush, x, y, format);
        }

        public void EndContainer(GraphicsContainer container)
        {
            g.EndContainer(container);
        }

        public void EnumerateMetafile(Metafile metafile, Point destPoint, Graphics.EnumerateMetafileProc callback)
        {
            g.EnumerateMetafile(metafile, destPoint, callback);
        }

        public void EnumerateMetafile(Metafile metafile, Point[] destPoints, Graphics.EnumerateMetafileProc callback)
        {
            g.EnumerateMetafile(metafile, destPoints, callback);
        }

        public void EnumerateMetafile(Metafile metafile, PointF destPoint, Graphics.EnumerateMetafileProc callback)
        {
            g.EnumerateMetafile(metafile, destPoint, callback);
        }

        public void EnumerateMetafile(Metafile metafile, PointF[] destPoints, Graphics.EnumerateMetafileProc callback)
        {
            g.EnumerateMetafile(metafile, destPoints, callback);
        }

        public void EnumerateMetafile(Metafile metafile, Rectangle destRect, Graphics.EnumerateMetafileProc callback)
        {
            g.EnumerateMetafile(metafile, destRect, callback);
        }

        public void EnumerateMetafile(Metafile metafile, RectangleF destRect, Graphics.EnumerateMetafileProc callback)
        {
            g.EnumerateMetafile(metafile, destRect, callback);
        }

        public void EnumerateMetafile(Metafile metafile, Point destPoint, Graphics.EnumerateMetafileProc callback, IntPtr callbackData)
        {
            g.EnumerateMetafile(metafile, destPoint, callback, callbackData);
        }

        public void EnumerateMetafile(Metafile metafile, Point[] destPoints, Graphics.EnumerateMetafileProc callback, IntPtr callbackData)
        {
            g.EnumerateMetafile(metafile, destPoints, callback, callbackData);
        }

        public void EnumerateMetafile(Metafile metafile, PointF destPoint, Graphics.EnumerateMetafileProc callback, IntPtr callbackData)
        {
            g.EnumerateMetafile(metafile, destPoint, callback, callbackData);
        }

        public void EnumerateMetafile(Metafile metafile, PointF[] destPoints, Graphics.EnumerateMetafileProc callback, IntPtr callbackData)
        {
            g.EnumerateMetafile(metafile, destPoints, callback, callbackData);
        }

        public void EnumerateMetafile(Metafile metafile, Rectangle destRect, Graphics.EnumerateMetafileProc callback, IntPtr callbackData)
        {
            g.EnumerateMetafile(metafile, destRect, callback, callbackData);
        }

        public void EnumerateMetafile(Metafile metafile, RectangleF destRect, Graphics.EnumerateMetafileProc callback, IntPtr callbackData)
        {
            g.EnumerateMetafile(metafile, destRect, callback, callbackData);
        }

        public void EnumerateMetafile(Metafile metafile, Point destPoint, Graphics.EnumerateMetafileProc callback, IntPtr callbackData, ImageAttributes imageAttr)
        {
            g.EnumerateMetafile(metafile, destPoint, callback, callbackData, imageAttr);
        }

        public void EnumerateMetafile(Metafile metafile, Point destPoint, Rectangle srcRect, GraphicsUnit srcUnit, Graphics.EnumerateMetafileProc callback)
        {
            g.EnumerateMetafile(metafile, destPoint, srcRect, srcUnit, callback);
        }

        public void EnumerateMetafile(Metafile metafile, Point[] destPoints, Graphics.EnumerateMetafileProc callback, IntPtr callbackData, ImageAttributes imageAttr)
        {
            g.EnumerateMetafile(metafile, destPoints, callback, callbackData, imageAttr);
        }

        public void EnumerateMetafile(Metafile metafile, Point[] destPoints, Rectangle srcRect, GraphicsUnit srcUnit, Graphics.EnumerateMetafileProc callback)
        {
            g.EnumerateMetafile(metafile, destPoints, srcRect, srcUnit, callback);
        }

        public void EnumerateMetafile(Metafile metafile, PointF destPoint, Graphics.EnumerateMetafileProc callback, IntPtr callbackData, ImageAttributes imageAttr)
        {
            g.EnumerateMetafile(metafile, destPoint, callback, callbackData, imageAttr);
        }

        public void EnumerateMetafile(Metafile metafile, PointF destPoint, RectangleF srcRect, GraphicsUnit srcUnit, Graphics.EnumerateMetafileProc callback)
        {
            g.EnumerateMetafile(metafile, destPoint, srcRect, srcUnit, callback);
        }

        public void EnumerateMetafile(Metafile metafile, PointF[] destPoints, Graphics.EnumerateMetafileProc callback, IntPtr callbackData, ImageAttributes imageAttr)
        {
            g.EnumerateMetafile(metafile, destPoints, callback, callbackData, imageAttr);
        }

        public void EnumerateMetafile(Metafile metafile, PointF[] destPoints, RectangleF srcRect, GraphicsUnit srcUnit, Graphics.EnumerateMetafileProc callback)
        {
            g.EnumerateMetafile(metafile, destPoints, srcRect, srcUnit, callback);
        }

        public void EnumerateMetafile(Metafile metafile, Rectangle destRect, Graphics.EnumerateMetafileProc callback, IntPtr callbackData, ImageAttributes imageAttr)
        {
            g.EnumerateMetafile(metafile, destRect, callback, callbackData, imageAttr);
        }

        public void EnumerateMetafile(Metafile metafile, Rectangle destRect, Rectangle srcRect, GraphicsUnit srcUnit, Graphics.EnumerateMetafileProc callback)
        {
            g.EnumerateMetafile(metafile, destRect, srcRect, srcUnit, callback);
        }

        public void EnumerateMetafile(Metafile metafile, RectangleF destRect, Graphics.EnumerateMetafileProc callback, IntPtr callbackData, ImageAttributes imageAttr)
        {
            g.EnumerateMetafile(metafile, destRect, callback, callbackData, imageAttr);
        }

        public void EnumerateMetafile(Metafile metafile, RectangleF destRect, RectangleF srcRect, GraphicsUnit srcUnit, Graphics.EnumerateMetafileProc callback)
        {
            g.EnumerateMetafile(metafile, destRect, srcRect, srcUnit, callback);
        }

        public void EnumerateMetafile(Metafile metafile, Point destPoint, Rectangle srcRect, GraphicsUnit srcUnit, Graphics.EnumerateMetafileProc callback, IntPtr callbackData)
        {
            g.EnumerateMetafile(metafile, destPoint, srcRect, srcUnit, callback, callbackData);
        }

        public void EnumerateMetafile(Metafile metafile, Point[] destPoints, Rectangle srcRect, GraphicsUnit srcUnit, Graphics.EnumerateMetafileProc callback, IntPtr callbackData)
        {
            g.EnumerateMetafile(metafile, destPoints, srcRect, srcUnit, callback, callbackData);
        }

        public void EnumerateMetafile(Metafile metafile, PointF destPoint, RectangleF srcRect, GraphicsUnit srcUnit, Graphics.EnumerateMetafileProc callback, IntPtr callbackData)
        {
            g.EnumerateMetafile(metafile, destPoint, srcRect, srcUnit, callback, callbackData);
        }

        public void EnumerateMetafile(Metafile metafile, PointF[] destPoints, RectangleF srcRect, GraphicsUnit srcUnit, Graphics.EnumerateMetafileProc callback, IntPtr callbackData)
        {
            g.EnumerateMetafile(metafile, destPoints, srcRect, srcUnit, callback, callbackData);
        }

        public void EnumerateMetafile(Metafile metafile, Rectangle destRect, Rectangle srcRect, GraphicsUnit srcUnit, Graphics.EnumerateMetafileProc callback, IntPtr callbackData)
        {
            g.EnumerateMetafile(metafile, destRect, srcRect, srcUnit, callback, callbackData);
        }

        public void EnumerateMetafile(Metafile metafile, RectangleF destRect, RectangleF srcRect, GraphicsUnit srcUnit, Graphics.EnumerateMetafileProc callback, IntPtr callbackData)
        {
            g.EnumerateMetafile(metafile, destRect, srcRect, srcUnit, callback, callbackData);
        }

        public void EnumerateMetafile(Metafile metafile, Point destPoint, Rectangle srcRect, GraphicsUnit unit, Graphics.EnumerateMetafileProc callback, IntPtr callbackData, ImageAttributes imageAttr)
        {
            g.EnumerateMetafile(metafile, destPoint, srcRect, unit, callback, callbackData, imageAttr);
        }

        public void EnumerateMetafile(Metafile metafile, Point[] destPoints, Rectangle srcRect, GraphicsUnit unit, Graphics.EnumerateMetafileProc callback, IntPtr callbackData, ImageAttributes imageAttr)
        {
            g.EnumerateMetafile(metafile, destPoints, srcRect, unit, callback, callbackData, imageAttr);
        }

        public void EnumerateMetafile(Metafile metafile, PointF destPoint, RectangleF srcRect, GraphicsUnit unit, Graphics.EnumerateMetafileProc callback, IntPtr callbackData, ImageAttributes imageAttr)
        {
            g.EnumerateMetafile(metafile, destPoint, srcRect, unit, callback, callbackData, imageAttr);
        }

        public void EnumerateMetafile(Metafile metafile, PointF[] destPoints, RectangleF srcRect, GraphicsUnit unit, Graphics.EnumerateMetafileProc callback, IntPtr callbackData, ImageAttributes imageAttr)
        {
            g.EnumerateMetafile(metafile, destPoints, srcRect, unit, callback, callbackData, imageAttr);
        }

        public void EnumerateMetafile(Metafile metafile, Rectangle destRect, Rectangle srcRect, GraphicsUnit unit, Graphics.EnumerateMetafileProc callback, IntPtr callbackData, ImageAttributes imageAttr)
        {
            g.EnumerateMetafile(metafile, destRect, srcRect, unit, callback, callbackData, imageAttr);
        }

        public void EnumerateMetafile(Metafile metafile, RectangleF destRect, RectangleF srcRect, GraphicsUnit unit, Graphics.EnumerateMetafileProc callback, IntPtr callbackData, ImageAttributes imageAttr)
        {
            g.EnumerateMetafile(metafile, destRect, srcRect, unit, callback, callbackData, imageAttr);
        }

        public void ExcludeClip(Rectangle rect)
        {
            g.ExcludeClip(rect);
        }

        public void ExcludeClip(Region region)
        {
            g.ExcludeClip(region);
        }

        public void FillClosedCurve(Brush brush, Point[] points)
        {
            g.FillClosedCurve(brush, points);
        }

        public void FillClosedCurve(Brush brush, PointF[] points)
        {
            g.FillClosedCurve(brush, points);
        }

        public void FillClosedCurve(Brush brush, Point[] points, FillMode fillmode)
        {
            g.FillClosedCurve(brush, points, fillmode);
        }

        public void FillClosedCurve(Brush brush, PointF[] points, FillMode fillmode)
        {
            g.FillClosedCurve(brush, points, fillmode);
        }

        public void FillClosedCurve(Brush brush, Point[] points, FillMode fillmode, float tension)
        {
            g.FillClosedCurve(brush, points, fillmode, tension);
        }

        public void FillClosedCurve(Brush brush, PointF[] points, FillMode fillmode, float tension)
        {
            g.FillClosedCurve(brush, points, fillmode, tension);
        }

        public void FillEllipse(Brush brush, Rectangle rect)
        {
            g.FillEllipse(brush, rect);
        }

        public void FillEllipse(Brush brush, RectangleF rect)
        {
            g.FillEllipse(brush, rect);
        }

        public void FillEllipse(Brush brush, float x, float y, float width, float height)
        {
            g.FillEllipse(brush, x, y, width, height);
        }

        public void FillEllipse(Brush brush, int x, int y, int width, int height)
        {
            g.FillEllipse(brush, x, y, width, height);
        }

        public void FillPath(Brush brush, GraphicsPath path)
        {
            g.FillPath(brush, path);
        }

        public void FillPie(Brush brush, Rectangle rect, float startAngle, float sweepAngle)
        {
            g.FillPie(brush, rect, startAngle, sweepAngle);
        }

        public void FillPie(Brush brush, float x, float y, float width, float height, float startAngle, float sweepAngle)
        {
            g.FillPie(brush, x, y, width, height, startAngle, sweepAngle);
        }

        public void FillPie(Brush brush, int x, int y, int width, int height, int startAngle, int sweepAngle)
        {
            g.FillPie(brush, x, y, width, height, startAngle, sweepAngle);
        }

        public void FillPolygon(Brush brush, Point[] points)
        {
            g.FillPolygon(brush, points);
        }

        public void FillPolygon(Brush brush, PointF[] points)
        {
            g.FillPolygon(brush, points);
        }

        public void FillPolygon(Brush brush, Point[] points, FillMode fillMode)
        {
            g.FillPolygon(brush, points, fillMode);
        }

        public void FillPolygon(Brush brush, PointF[] points, FillMode fillMode)
        {
            g.FillPolygon(brush, points, fillMode);
        }

        public void FillRectangle(Brush brush, Rectangle rect)
        {
            g.FillRectangle(brush, rect);
        }

        public void FillRectangle(Brush brush, RectangleF rect)
        {
            g.FillRectangle(brush, rect);
        }

        public void FillRectangle(Brush brush, float x, float y, float width, float height)
        {
            g.FillRectangle(brush, x, y, width, height);
        }

        public void FillRectangle(Brush brush, int x, int y, int width, int height)
        {
            g.FillRectangle(brush, x, y, width, height);
        }

        public void FillRectangles(Brush brush, Rectangle[] rects)
        {
            g.FillRectangles(brush, rects);
        }

        public void FillRectangles(Brush brush, RectangleF[] rects)
        {
            g.FillRectangles(brush, rects);
        }

        public void FillRegion(Brush brush, Region region)
        {
            g.FillRegion(brush, region);
        }

        public void Flush()
        {
            g.Flush();
        }

        public void Flush(FlushIntention intention)
        {
            g.Flush(intention);
        }

        public object GetContextInfo()
        {
            return g.GetContextInfo();
        }

        public IntPtr GetHdc()
        {
            return g.GetHdc();
        }

        public Color GetNearestColor(Color color)
        {
            return g.GetNearestColor(color);
        }

        public void IntersectClip(Rectangle rect)
        {
            g.IntersectClip(rect);
        }

        public void IntersectClip(RectangleF rect)
        {
            g.IntersectClip(rect);
        }

        public void IntersectClip(Region region)
        {
            g.IntersectClip(region);            
        }

        public bool IsVisible(Point point)
        {
            return g.IsVisible(point);
        }

        public bool IsVisible(PointF point)
        {
            return g.IsVisible(point);
        }

        public bool IsVisible(Rectangle rect)
        {
            return g.IsVisible(rect);
        }

        public bool IsVisible(RectangleF rect)
        {
            return g.IsVisible(rect);
        }

        public bool IsVisible(float x, float y)
        {
            return g.IsVisible(x, y);
        }

        public bool IsVisible(int x, int y)
        {
            return g.IsVisible(x, y);
        }

        public bool IsVisible(float x, float y, float width, float height)
        {
            return g.IsVisible(x, y, width, height);
        }

        public bool IsVisible(int x, int y, int width, int height)
        {
            return g.IsVisible(x, y, width, height);
        }

        public Region[] MeasureCharacterRanges(string text, Font font, RectangleF layoutRect, StringFormat stringFormat)
        {
            return g.MeasureCharacterRanges(text, font, layoutRect, stringFormat);
        }

        public SizeF MeasureString(string text, Font font)
        {
            return g.MeasureString(text, font);
        }

        public SizeF MeasureString(string text, Font font, int width)
        {
            return g.MeasureString(text, font, width);
        }

        public SizeF MeasureString(string text, Font font, SizeF layoutArea)
        {
            return g.MeasureString(text, font, layoutArea);
        }

        public SizeF MeasureString(string text, Font font, int width, StringFormat format)
        {
            return g.MeasureString(text, font, width, format);
        }

        public SizeF MeasureString(string text, Font font, PointF origin, StringFormat stringFormat)
        {
            return g.MeasureString(text, font, origin, stringFormat);
        }

        public SizeF MeasureString(string text, Font font, SizeF layoutArea, StringFormat stringFormat)
        {
            return g.MeasureString(text, font, layoutArea, stringFormat);
        }

        public SizeF MeasureString(string text, Font font, SizeF layoutArea, StringFormat stringFormat, out int charactersFitted, out int linesFilled)
        {
            return g.MeasureString(text, font, layoutArea, stringFormat, out charactersFitted, out linesFilled);
        }

        public void MultiplyTransform(Matrix matrix)
        {
            g.MultiplyTransform(matrix);
        }

        public void MultiplyTransform(Matrix matrix, MatrixOrder order)
        {
            g.MultiplyTransform(matrix, order);
        }

        public void ReleaseHdc()
        {
            g.ReleaseHdc();
        }

        public void ReleaseHdc(IntPtr hdc)
        {
            g.ReleaseHdc(hdc);
        }

        public void ReleaseHdcInternal(IntPtr hdc)
        {
            g.ReleaseHdcInternal(hdc);
        }

        public void ResetClip()
        {
            g.ResetClip();
        }

        public void ResetTransform()
        {
            g.ResetTransform();
        }

        public void Restore(GraphicsState gstate)
        {
            g.Restore(gstate);
        }

        public void RotateTransform(float angle)
        {
            g.RotateTransform(angle);
        }

        public void RotateTransform(float angle, MatrixOrder order)
        {
            g.RotateTransform(angle, order);
        }

        public GraphicsState Save()
        {
            return g.Save();
        }

        public void ScaleTransform(float sx, float sy)
        {
            g.ScaleTransform(sx, sy);
        }

        public void ScaleTransform(float sx, float sy, MatrixOrder order)
        {
            g.ScaleTransform(sx, sy, order);
        }

        public void SetClip(Graphics g)
        {
            this.g.SetClip(g);
        }

        public void SetClip(GraphicsPath path)
        {
            g.SetClip(path);
        }

        public void SetClip(Rectangle rect)
        {
            g.SetClip(rect);
        }

        public void SetClip(RectangleF rect)
        {
            g.SetClip(rect);
        }

        public void SetClip(Graphics g, CombineMode combineMode)
        {
            this.g.SetClip(g, combineMode);
        }

        public void SetClip(GraphicsPath path, CombineMode combineMode)
        {
            g.SetClip(path, combineMode);
        }

        public void SetClip(Rectangle rect, CombineMode combineMode)
        {
            g.SetClip(rect, combineMode);
        }

        public void SetClip(RectangleF rect, CombineMode combineMode)
        {
            g.SetClip(rect, combineMode);
        }

        public void SetClip(Region region, CombineMode combineMode)
        {
            g.SetClip(region, combineMode);
        }

        public void TransformPoints(CoordinateSpace destSpace, CoordinateSpace srcSpace, Point[] pts)
        {
            g.TransformPoints(destSpace, srcSpace, pts);
        }

        public void TransformPoints(CoordinateSpace destSpace, CoordinateSpace srcSpace, PointF[] pts)
        {
            g.TransformPoints(destSpace, srcSpace, pts);
        }

        public void TranslateClip(float dx, float dy)
        {
            g.TranslateClip(dx, dy);
        }

        public void TranslateClip(int dx, int dy)
        {
            g.TranslateClip(dx, dy);
        }

        public void TranslateTransform(float dx, float dy)
        {
            g.TranslateTransform(dx, dy);
        }

        public void TranslateTransform(float dx, float dy, MatrixOrder order)
        {
            g.TranslateTransform(dx, dy, order);
        }
    }
}
