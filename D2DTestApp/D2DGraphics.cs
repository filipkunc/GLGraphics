using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;
using System.Drawing.Imaging;
using GraphicsImplementation;

namespace D2DTestApp
{
    public class D2DGraphics : IGraphics
    {
        SharpDX.Direct2D1.RenderTarget _renderTarget;
        Graphics _graphics;
        GraphicsUnit _pageUnit = GraphicsUnit.Pixel;
        Matrix _gdiTransform = new Matrix();

        public FillTextBackground FillTextBackground;
        public Color TextBackgroundColor;

        public D2DGraphics(D2DView.RenderTargetEventArgs e)
        {
            _renderTarget = e.RenderTarget;
            _graphics = e.Graphics;
        }

        public SharpDX.Direct2D1.RenderTarget Canvas { get { return _renderTarget; } }

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
            get { return CompositingMode.SourceOver; }
            set { }
        }

        public CompositingQuality CompositingQuality
        {
            get { return CompositingQuality.Default; }
            set { }
        }

        public float DpiX
        {
            get { return _renderTarget.DotsPerInch.Width; }
        }

        public float DpiY
        {
            get { return _renderTarget.DotsPerInch.Height; }
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
                        _renderTarget.Transform = SharpDX.Matrix3x2.Scaling(new SharpDX.Vector2(DpiX / 25.4f, DpiY / 25.4f));
                        break;
                    case GraphicsUnit.Pixel:
                        _renderTarget.Transform = SharpDX.Matrix3x2.Identity;
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

        SmoothingMode _smoothingMode = SmoothingMode.None;

        public SmoothingMode SmoothingMode
        {
            get
            {
                return _smoothingMode;
            }
            set
            {
                _smoothingMode = value;
                switch (_smoothingMode)
                {
                    case SmoothingMode.AntiAlias:
                    case SmoothingMode.HighQuality:
                    case SmoothingMode.HighSpeed:
                        _renderTarget.AntialiasMode = SharpDX.Direct2D1.AntialiasMode.PerPrimitive;
                        break;
                    default:
                        _renderTarget.AntialiasMode = SharpDX.Direct2D1.AntialiasMode.Aliased;
                        break;
                }
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

        TextRenderingHint _textRenderingHint = TextRenderingHint.ClearTypeGridFit;

        public TextRenderingHint TextRenderingHint
        {
            get { return _textRenderingHint; }
            set { _textRenderingHint = value; }
        }

        public Matrix Transform
        {
            get
            {
                return _gdiTransform;
            }
            set
            {
                _gdiTransform = value;
                _renderTarget.Transform = new SharpDX.Matrix3x2(_gdiTransform.Elements);
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
            _renderTarget.Clear(color.ToColor4());
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
            //SetCanvasFromPen(pen);
            //_canvas.DrawArc(rect, startAngle, sweepAngle, false);
            throw new NotImplementedException("TODO: DrawArc"); //!-!
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
            //SetCanvasFromPen(pen);
            //_canvas.DrawEllipse(rect);
            throw new NotImplementedException("TODO: DrawEllipse"); //!-!
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

        public void DrawImage(Image image, Point point)
        {

        }

        public void DrawImage(Image image, Point[] destPoints)
        {
            throw new NotImplementedException();
        }

        public void DrawImage(Image image, PointF point)
        {

        }

        public void DrawImage(Image image, PointF[] destPoints)
        {
            throw new NotImplementedException();
        }

        public void DrawImage(Image image, Rectangle rect)
        {

        }

        public void DrawImage(Image image, RectangleF rect)
        {

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
            if (srcUnit != GraphicsUnit.Pixel)
                throw new NotImplementedException();

            throw new NotImplementedException("TODO: DrawImage"); //!-!
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

        //private void SetCanvasFromPen(Pen pen)
        //{
        //    _canvas.CurrentColor = pen.Color;
        //    _canvas.LineWidth = pen.Width;
        //    switch (pen.DashStyle)
        //    {
        //        case DashStyle.Dash:
        //            _canvas.SetLineStipplePattern(1, 0xEEEE);
        //            break;
        //        case DashStyle.DashDot:
        //            _canvas.SetLineStipplePattern(1, 0xEBAE);
        //            break;
        //        case DashStyle.DashDotDot:
        //            _canvas.SetLineStipplePattern(1, 0x5757);
        //            break;
        //        case DashStyle.Dot:
        //            _canvas.SetLineStipplePattern(1, 0xAAAA);
        //            break;
        //        case DashStyle.Custom:
        //        case DashStyle.Solid:
        //        default:
        //            _canvas.ResetLineStipplePattern();
        //            break;
        //    }
        //}

        public void DrawLine(Pen pen, Point pt1, Point pt2)
        {
            DrawLine(pen, new PointF(pt1.X, pt1.Y), new PointF(pt2.X, pt2.Y));
        }

        public void DrawLine(Pen pen, PointF pt1, PointF pt2)
        {
            float strokeWidth = pen.Width;
            if (strokeWidth == 0.0f)
                strokeWidth = 1.0f;

            _renderTarget.DrawLine(new SharpDX.Vector2(pt1.X, pt1.Y), new SharpDX.Vector2(pt2.X, pt2.Y),
                new SharpDX.Direct2D1.SolidColorBrush(_renderTarget, pen.Color.ToColor4()), 
                strokeWidth, pen.ToStrokeStyle(_renderTarget.Factory));
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
            using (var geometry = new SharpDX.Direct2D1.PathGeometry(_renderTarget.Factory))
            {
                var sink = geometry.Open();

                sink.BeginFigure(new SharpDX.Vector2(points[0].X, points[0].Y), SharpDX.Direct2D1.FigureBegin.Hollow);

                for (int i = 1; i < points.Length; i++)
                    sink.AddLine(new SharpDX.Vector2(points[i].X, points[i].Y));

                sink.EndFigure(SharpDX.Direct2D1.FigureEnd.Open);

                sink.Close();

                float strokeWidth = pen.Width;
                if (strokeWidth == 0.0f)
                    strokeWidth = 1.0f;

                _renderTarget.DrawGeometry(geometry, new SharpDX.Direct2D1.SolidColorBrush(_renderTarget, pen.Color.ToColor4()),
                    strokeWidth, pen.ToStrokeStyle(_renderTarget.Factory));
            }
        }

        public void DrawLines(Pen pen, PointF[] points)
        {
            using (var geometry = new SharpDX.Direct2D1.PathGeometry(_renderTarget.Factory))
            {
                var sink = geometry.Open();

                sink.BeginFigure(new SharpDX.Vector2(points[0].X, points[0].Y), SharpDX.Direct2D1.FigureBegin.Hollow);

                for (int i = 1; i < points.Length; i++)
                    sink.AddLine(new SharpDX.Vector2(points[i].X, points[i].Y));

                sink.EndFigure(SharpDX.Direct2D1.FigureEnd.Open);

                sink.Close();

                float strokeWidth = pen.Width;
                if (strokeWidth == 0.0f)
                    strokeWidth = 1.0f;

                _renderTarget.DrawGeometry(geometry, new SharpDX.Direct2D1.SolidColorBrush(_renderTarget, pen.Color.ToColor4()),
                    strokeWidth, pen.ToStrokeStyle(_renderTarget.Factory));
            }
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
            //SetCanvasFromPen(pen);
            //_canvas.DrawArc(rect, startAngle, sweepAngle, true);
            throw new NotImplementedException("TODO: DrawPie"); //!-!
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
            DrawRectangle(pen, rect.X, rect.Y, rect.Width, rect.Height);
        }

        public void DrawRectangle(Pen pen, float x, float y, float width, float height)
        {
            //SetCanvasFromPen(pen);
            //_canvas.DrawRectangle(new RectangleF(x, y, width, height));
            throw new NotImplementedException("TODO: DrawRectangle"); //!-!
        }

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
            //SizeF textSize = _graphics.MeasureString(s, font, point, format);
            //DrawStringByPixels(s, font, brush, textSize, new RectangleF(point, SizeF.Empty), format);

            throw new NotImplementedException("TODO: DrawString"); //!-!
        }

        public void DrawString(string s, Font font, Brush brush, RectangleF layoutRectangle, StringFormat format)
        {
            //SizeF layoutSize = layoutRectangle.Size;
            //if (_pageUnit == GraphicsUnit.Millimeter)
            //{
            //    layoutSize.Width *= _canvas.GlobalScale.X;
            //    layoutSize.Height *= _canvas.GlobalScale.Y;
            //}

            //SizeF textSize = _graphics.MeasureString(s, font, layoutSize, format);
            //DrawStringByPixels(s, font, brush, textSize, layoutRectangle, format);

            throw new NotImplementedException("TODO: DrawString"); //!-!
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

        public void FillRectangle(Brush brush, Rectangle rect)
        {
            FillRectangle(brush, new RectangleF(rect.X, rect.Y, rect.Width, rect.Height));
        }

        public void FillRectangle(Brush brush, RectangleF rect)
        {
            //SetPatternFromBrush(brush);

            //_canvas.FillRectangle(rect, ColorsFromBrush(brush));

            //_canvas.ResetPolygonStipplePattern();

            throw new NotImplementedException("TODO: FillRectangle");
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
            SizeF size = TextRenderer.MeasureText(text, font).ToSizeF();
            if (_pageUnit == GraphicsUnit.Millimeter)
            {
                size.Width /= _renderTarget.Transform.ScaleVector.X;
                size.Height /= _renderTarget.Transform.ScaleVector.Y;
            }
            return size;
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

        public void ResetClip()
        {
            throw new NotImplementedException("TODO: ResetClip"); //!-!
        }

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

        public void SetClip(Rectangle rect)
        {
            //PointF currentScale = _canvas.GlobalScale;
            //rect.X = (int)Math.Round(rect.X * currentScale.X);
            //rect.Y = (int)Math.Round(rect.Y * currentScale.Y);
            //rect.Width = (int)Math.Round(rect.Width * currentScale.X);
            //rect.Height = (int)Math.Round(rect.Height * currentScale.Y);
            //_canvas.SetClip(rect);

            throw new NotImplementedException("TODO: SetClip"); //!-!
        }

        public void SetClip(RectangleF rect)
        {
            //Rectangle clipRect = new Rectangle();
            //PointF currentScale = _canvas.GlobalScale;
            //clipRect.X = (int)Math.Round(rect.X * currentScale.X);
            //clipRect.Y = (int)Math.Round(rect.Y * currentScale.Y);
            //clipRect.Width = (int)Math.Round(rect.Width * currentScale.X);
            //clipRect.Height = (int)Math.Round(rect.Height * currentScale.Y);
            //_canvas.SetClip(clipRect);

            throw new NotImplementedException("TODO: SetClip"); //!-!
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
