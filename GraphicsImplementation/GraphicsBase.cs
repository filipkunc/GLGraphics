using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Collections;

namespace GraphicsImplementation
{
    public abstract class GraphicsBase : IGraphics
    {
        Matrix _gdiTransform = new Matrix();

        public FillTextBackground FillTextBackground;
        public Color TextBackgroundColor;

        public GraphicsBase()
        {
            FillTextBackground = FillTextBackground_Default;
            TextBackgroundColor = Color.White;
        }

        public void FillTextBackground_Default(Bitmap textBackground, PointF textLocation, SizeF textSize)
        {
            Point textLocationPixel = new Point(
                        (int)Math.Round(textLocation.X, MidpointRounding.AwayFromZero),
                        (int)Math.Round(textLocation.Y, MidpointRounding.AwayFromZero));


            using (Graphics g = Graphics.FromImage(textBackground))
            {
                g.Clear(TextBackgroundColor);
            }
        }

        public Region Clip
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public RectangleF ClipBounds
        {
            get { throw new NotImplementedException(); }
        }

        public abstract CompositingMode CompositingMode { get; set; }

        public CompositingQuality CompositingQuality
        {
            get { return CompositingQuality.Default; }
            set { }
        }

        public abstract float DpiX { get; }

        public abstract float DpiY { get; }

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

        public abstract GraphicsUnit PageUnit { get; set; }

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

        public abstract SmoothingMode SmoothingMode { get; set; }

        public int TextContrast
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public abstract TextRenderingHint TextRenderingHint { get; set; }

        public Matrix Transform
        {
            get { return _gdiTransform; }
            set
            {
                _gdiTransform = value;
                SetTransform(_gdiTransform);
            }
        }

        protected abstract void SetTransform(Matrix matrix);

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

        public abstract void Clear(Color color);

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

        public abstract void Dispose();

        public void DrawArc(Pen pen, Rectangle rect, float startAngle, float sweepAngle)
        {
            DrawArc(pen, rect.ToRectangleF(), startAngle, sweepAngle);
        }

        public void DrawArc(Pen pen, RectangleF rect, float startAngle, float sweepAngle)
        {
            using (var path = new GraphicsPath())
            {
                path.AddArc(rect, startAngle, sweepAngle);
                DrawPath(pen, path);
            }
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
            using (var path = new GraphicsPath())
            {
                path.AddBezier(pt1, pt2, pt3, pt4);
                DrawPath(pen, path);
            }
        }

        public void DrawBezier(Pen pen, PointF pt1, PointF pt2, PointF pt3, PointF pt4)
        {
            using (var path = new GraphicsPath())
            {
                path.AddBezier(pt1, pt2, pt3, pt4);
                DrawPath(pen, path);
            }
        }

        public void DrawBezier(Pen pen, float x1, float y1, float x2, float y2, float x3, float y3, float x4, float y4)
        {
            using (var path = new GraphicsPath())
            {
                path.AddBezier(x1, y1, x2, y2, x3, y3, x4, y4);
                DrawPath(pen, path);
            }
        }

        public void DrawBeziers(Pen pen, Point[] points)
        {
            using (var path = new GraphicsPath())
            {
                path.AddBeziers(points);
                DrawPath(pen, path);
            }
        }

        public void DrawBeziers(Pen pen, PointF[] points)
        {
            using (var path = new GraphicsPath())
            {
                path.AddBeziers(points);
                DrawPath(pen, path);
            }
        }

        public void DrawClosedCurve(Pen pen, Point[] points)
        {
            using (var path = new GraphicsPath())
            {
                path.AddClosedCurve(points);
                DrawPath(pen, path);
            }
        }

        public void DrawClosedCurve(Pen pen, PointF[] points)
        {
            using (var path = new GraphicsPath())
            {
                path.AddClosedCurve(points);
                DrawPath(pen, path);
            }
        }

        public void DrawClosedCurve(Pen pen, Point[] points, float tension, FillMode fillmode)
        {
            using (var path = new GraphicsPath())
            {
                path.AddClosedCurve(points, tension);
                path.FillMode = fillmode;
                DrawPath(pen, path);
            }
        }

        public void DrawClosedCurve(Pen pen, PointF[] points, float tension, FillMode fillmode)
        {
            using (var path = new GraphicsPath())
            {
                path.AddClosedCurve(points, tension);
                path.FillMode = fillmode;
                DrawPath(pen, path);
            }
        }

        public void DrawCurve(Pen pen, Point[] points)
        {
            using (var path = new GraphicsPath())
            {
                path.AddCurve(points);
                DrawPath(pen, path);
            }
        }

        public void DrawCurve(Pen pen, PointF[] points)
        {
            using (var path = new GraphicsPath())
            {
                path.AddCurve(points);
                DrawPath(pen, path);
            }
        }

        public void DrawCurve(Pen pen, Point[] points, float tension)
        {
            using (var path = new GraphicsPath())
            {
                path.AddCurve(points, tension);
                DrawPath(pen, path);
            }
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
            using (var path = new GraphicsPath())
            {
                path.AddEllipse(rect);
                DrawPath(pen, path);
            }
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
            DrawImage(icon.ToBitmap(), targetRect);
        }

        public void DrawIcon(Icon icon, int x, int y)
        {
            DrawImage(icon.ToBitmap(), x, y);
        }

        public void DrawIconUnstretched(Icon icon, Rectangle targetRect)
        {
            DrawImageUnscaled(icon.ToBitmap(), targetRect);
        }

        public void DrawImage(Image image, Point point)
        {
            DrawImage(image, new PointF(point.X, point.Y));
        }

        public void DrawImage(Image image, Point[] destPoints)
        {
            throw new NotImplementedException();
        }

        public void DrawImage(Image image, PointF point)
        {
            DrawImage(image, new RectangleF(point, image.Size));
        }

        public void DrawImage(Image image, PointF[] destPoints)
        {
            throw new NotImplementedException();
        }

        public void DrawImage(Image image, Rectangle rect)
        {
            DrawImage(image, rect.ToRectangleF());
        }

        public abstract void DrawImage(Image image, RectangleF rect);

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

        public abstract void DrawImage(Image image, Rectangle destRect, int srcX, int srcY, int srcWidth, int srcHeight, GraphicsUnit srcUnit, ImageAttributes imageAttr);


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


        public abstract void DrawLine(Pen pen, Point pt1, Point pt2);

        public abstract void DrawLine(Pen pen, PointF pt1, PointF pt2);

        public void DrawLine(Pen pen, float x1, float y1, float x2, float y2)
        {
            DrawLine(pen, new PointF(x1, y1), new PointF(x2, y2));
        }

        public void DrawLine(Pen pen, int x1, int y1, int x2, int y2)
        {
            DrawLine(pen, new Point(x1, y1), new Point(x2, y2));
        }

        public abstract void DrawLines(Pen pen, Point[] points);

        public abstract void DrawLines(Pen pen, PointF[] points);

        public abstract void DrawPath(Pen pen, GraphicsPath path);

        public void DrawPie(Pen pen, Rectangle rect, float startAngle, float sweepAngle)
        {
            DrawPie(pen, rect.ToRectangleF(), startAngle, sweepAngle);
        }

        public void DrawPie(Pen pen, RectangleF rect, float startAngle, float sweepAngle)
        {
            using (var path = new GraphicsPath())
            {
                path.AddPie(rect.X, rect.Y, rect.Width, rect.Height, startAngle, sweepAngle);
                DrawPath(pen, path);
            }
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
            using (var path = new GraphicsPath())
            {
                path.AddPolygon(points);
                DrawPath(pen, path);
            }
        }

        public void DrawPolygon(Pen pen, PointF[] points)
        {
            using (var path = new GraphicsPath())
            {
                path.AddPolygon(points);
                DrawPath(pen, path);
            }
        }

        public abstract void DrawRectangle(Pen pen, Rectangle rect);

        public abstract void DrawRectangle(Pen pen, float x, float y, float width, float height);

        public void DrawRectangle(Pen pen, int x, int y, int width, int height)
        {
            DrawRectangle(pen, new Rectangle(x, y, width, height));
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
            DrawString(s, font, brush, new RectangleF(point, SizeF.Empty), format);
        }

        public PointF PageUnitScale
        {
            get
            {
                if (PageUnit == GraphicsUnit.Millimeter)
                    return new PointF(DpiX / 25.4f, DpiY / 25.4f);
                return new PointF(1.0f, 1.0f);
            }
        }

        public void DrawString(string s, Font font, Brush brush, RectangleF layoutRectangle, StringFormat format)
        {
            SizeF textSize = GraphicsForMeasureString.MeasureString(s, font, layoutRectangle.Size, format);
            var scale = PageUnitScale;
            textSize.Width *= scale.X;
            textSize.Height *= scale.Y;
            DrawStringByPixels(s, font, brush, textSize, layoutRectangle, format);
        }

        private void DrawStringByPixels(string s, Font font, Brush brush, SizeF textSize, RectangleF layoutRectangle, StringFormat format)
        {
            layoutRectangle = layoutRectangle.ScaleRect(PageUnitScale);

            PointF textLocation = layoutRectangle.Location;

            if (format != null)
            {
                if (layoutRectangle.IsEmpty)
                    UpdateTextLocationByPoint(ref textLocation, textSize, format);
                else
                    UpdateTextLocationByRectangle(ref textLocation, layoutRectangle, textSize, format);
            }

            RectangleF textRectangle = new RectangleF(textLocation, textSize);

            PointF fillLocation = textLocation;

            using (Bitmap bitmap = new Bitmap((int)textSize.Width + 1, (int)textSize.Height + 1))
            {
                if (TextRenderingHint == TextRenderingHint.ClearTypeGridFit)
                {
                    FillTextBackground(bitmap, textLocation, textSize);
                }

                using (Graphics gdi = Graphics.FromImage(bitmap))
                {
                    gdi.TextRenderingHint = TextRenderingHint;
                    gdi.DrawString(s, font, brush, new RectangleF(PointF.Empty, textSize), format);
                }

                DrawStringImage(bitmap, textLocation, textSize);
            }
        }

        protected abstract void DrawStringImage(Bitmap bitmap, PointF textLocation, SizeF textSize);

        private void UpdateTextLocationByPoint(ref PointF textLocation, SizeF textSize, StringFormat format)
        {
            switch (format.Alignment)
            {
                case StringAlignment.Center:
                    textLocation.X -= (float)Math.Round(textSize.Width / 2.0f, MidpointRounding.AwayFromZero);
                    break;
                case StringAlignment.Far:
                    textLocation.X -= textSize.Width;
                    break;
                case StringAlignment.Near:
                default:
                    break;
            }

            switch (format.LineAlignment)
            {
                case StringAlignment.Center:
                    textLocation.Y -= (float)Math.Round(textSize.Height / 2.0f, MidpointRounding.AwayFromZero);
                    break;
                case StringAlignment.Far:
                    textLocation.Y -= textSize.Height;
                    break;
                case StringAlignment.Near:
                default:
                    break;
            }
        }

        private void UpdateTextLocationByRectangle(ref PointF textLocation, RectangleF rect, SizeF textSize, StringFormat format)
        {
            switch (format.Alignment)
            {
                case StringAlignment.Center:
                    textLocation.X += (float)Math.Round(rect.Width / 2.0f, MidpointRounding.AwayFromZero);
                    textLocation.X -= (float)Math.Round(textSize.Width / 2.0f, MidpointRounding.AwayFromZero);
                    break;
                case StringAlignment.Far:
                    textLocation.X = rect.Right;
                    textLocation.X -= textSize.Width;
                    break;
                case StringAlignment.Near:
                default:
                    break;
            }

            switch (format.LineAlignment)
            {
                case StringAlignment.Center:
                    textLocation.Y += (float)Math.Round(rect.Height / 2.0f, MidpointRounding.AwayFromZero);
                    textLocation.Y -= (float)Math.Round(textSize.Height / 2.0f, MidpointRounding.AwayFromZero);
                    break;
                case StringAlignment.Far:
                    textLocation.Y = rect.Bottom;
                    textLocation.Y -= textSize.Height;
                    break;
                case StringAlignment.Near:
                default:
                    break;
            }
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
            using (var path = new GraphicsPath())
            {
                path.AddClosedCurve(points);
                FillPath(brush, path);
            }
        }

        public void FillClosedCurve(Brush brush, PointF[] points)
        {
            using (var path = new GraphicsPath())
            {
                path.AddClosedCurve(points);
                FillPath(brush, path);
            }
        }

        public void FillClosedCurve(Brush brush, Point[] points, FillMode fillmode)
        {
            using (var path = new GraphicsPath())
            {
                path.AddClosedCurve(points);
                path.FillMode = fillmode;
                FillPath(brush, path);
            }
        }

        public void FillClosedCurve(Brush brush, PointF[] points, FillMode fillmode)
        {
            using (var path = new GraphicsPath())
            {
                path.AddClosedCurve(points);
                path.FillMode = fillmode;
                FillPath(brush, path);
            }
        }

        public void FillClosedCurve(Brush brush, Point[] points, FillMode fillmode, float tension)
        {
            using (var path = new GraphicsPath())
            {
                path.AddClosedCurve(points, tension);
                path.FillMode = fillmode;
                FillPath(brush, path);
            }
        }

        public void FillClosedCurve(Brush brush, PointF[] points, FillMode fillmode, float tension)
        {
            using (var path = new GraphicsPath())
            {
                path.AddClosedCurve(points, tension);
                path.FillMode = fillmode;
                FillPath(brush, path);
            }
        }

        public void FillEllipse(Brush brush, Rectangle rect)
        {
            using (var path = new GraphicsPath())
            {
                path.AddEllipse(rect);
                FillPath(brush, path);
            }
        }

        public void FillEllipse(Brush brush, RectangleF rect)
        {
            using (var path = new GraphicsPath())
            {
                path.AddEllipse(rect);
                FillPath(brush, path);
            }
        }

        public void FillEllipse(Brush brush, float x, float y, float width, float height)
        {
            FillEllipse(brush, new RectangleF(x, y, width, height));
        }

        public void FillEllipse(Brush brush, int x, int y, int width, int height)
        {
            FillEllipse(brush, new Rectangle(x, y, width, height));
        }

        public abstract void FillPath(Brush brush, GraphicsPath path);

        public void FillPie(Brush brush, Rectangle rect, float startAngle, float sweepAngle)
        {
            using (var path = new GraphicsPath())
            {
                path.AddPie(rect.X, rect.Y, rect.Width, rect.Height, startAngle, sweepAngle);
                FillPath(brush, path);
            }
        }

        public void FillPie(Brush brush, float x, float y, float width, float height, float startAngle, float sweepAngle)
        {
            using (var path = new GraphicsPath())
            {
                path.AddPie(x, y, width, height, startAngle, sweepAngle);
                FillPath(brush, path);
            }
        }

        public void FillPie(Brush brush, int x, int y, int width, int height, int startAngle, int sweepAngle)
        {
            using (var path = new GraphicsPath())
            {
                path.AddPie(x, y, width, height, startAngle, sweepAngle);
                FillPath(brush, path);
            }
        }

        public void FillPolygon(Brush brush, Point[] points)
        {
            using (var path = new GraphicsPath())
            {
                path.AddPolygon(points);
                FillPath(brush, path);
            }
        }

        public void FillPolygon(Brush brush, PointF[] points)
        {
            using (var path = new GraphicsPath())
            {
                path.AddPolygon(points);
                FillPath(brush, path);
            }
        }

        public void FillPolygon(Brush brush, Point[] points, FillMode fillMode)
        {
            using (var path = new GraphicsPath())
            {
                path.AddPolygon(points);
                path.FillMode = fillMode;
                FillPath(brush, path);
            }
        }

        public void FillPolygon(Brush brush, PointF[] points, FillMode fillMode)
        {
            using (var path = new GraphicsPath())
            {
                path.AddPolygon(points);
                path.FillMode = fillMode;
                FillPath(brush, path);
            }
        }

        public abstract void FillRectangle(Brush brush, Rectangle rect);

        public abstract void FillRectangle(Brush brush, RectangleF rect);

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

        protected abstract Graphics GraphicsForMeasureString { get; }

        public Region[] MeasureCharacterRanges(string text, Font font, RectangleF layoutRect, StringFormat stringFormat)
        {
            return GraphicsForMeasureString.MeasureCharacterRanges(text, font, layoutRect, stringFormat);
        }

        public SizeF MeasureString(string text, Font font)
        {
            return GraphicsForMeasureString.MeasureString(text, font);
        }

        public SizeF MeasureString(string text, Font font, int width)
        {
            return GraphicsForMeasureString.MeasureString(text, font, width);
        }

        public SizeF MeasureString(string text, Font font, SizeF layoutArea)
        {
            return GraphicsForMeasureString.MeasureString(text, font, layoutArea);
        }

        public SizeF MeasureString(string text, Font font, int width, StringFormat format)
        {
            return GraphicsForMeasureString.MeasureString(text, font, width, format);
        }

        public SizeF MeasureString(string text, Font font, PointF origin, StringFormat stringFormat)
        {
            return GraphicsForMeasureString.MeasureString(text, font, origin, stringFormat);
        }

        public SizeF MeasureString(string text, Font font, SizeF layoutArea, StringFormat stringFormat)
        {
            return GraphicsForMeasureString.MeasureString(text, font, layoutArea, stringFormat);
        }

        public SizeF MeasureString(string text, Font font, SizeF layoutArea, StringFormat stringFormat, out int charactersFitted, out int linesFilled)
        {
            return GraphicsForMeasureString.MeasureString(text, font, layoutArea, stringFormat, out charactersFitted, out linesFilled);
        }

        public void MultiplyTransform(Matrix matrix)
        {
            _gdiTransform.Multiply(matrix);
            Transform = _gdiTransform;
        }

        public void MultiplyTransform(Matrix matrix, MatrixOrder order)
        {
            _gdiTransform.Multiply(matrix, order);
            Transform = _gdiTransform;
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

        public abstract void ResetClip();

        public void ResetTransform()
        {
            _gdiTransform.Reset();
            Transform = _gdiTransform;
        }

        public void Restore(GraphicsState gstate)
        {
            throw new NotImplementedException();
        }

        public void RotateTransform(float angle)
        {
            _gdiTransform.Rotate(angle);
            Transform = _gdiTransform;
        }

        public void RotateTransform(float angle, MatrixOrder order)
        {
            _gdiTransform.Rotate(angle, order);
            Transform = _gdiTransform;
        }

        public GraphicsState Save()
        {
            throw new NotImplementedException();
        }

        public void ScaleTransform(float sx, float sy)
        {
            _gdiTransform.Scale(sx, sy);
            Transform = _gdiTransform;
        }

        public void ScaleTransform(float sx, float sy, MatrixOrder order)
        {
            _gdiTransform.Scale(sx, sy, order);
            Transform = _gdiTransform;
        }

        public void SetClip(Graphics g)
        {
            throw new NotImplementedException();
        }

        public void SetClip(GraphicsPath path)
        {
            throw new NotImplementedException();
        }

        public abstract void SetClip(Rectangle rect);

        public abstract void SetClip(RectangleF rect);

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
            _gdiTransform.Translate(dx, dy);
            Transform = _gdiTransform;
        }

        public void TranslateTransform(float dx, float dy, MatrixOrder order)
        {
            _gdiTransform.Translate(dx, dy, order);
            Transform = _gdiTransform;
        }
    }
}
