﻿using System;
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
using System.Collections;

namespace GraphicsImplementation
{
    public delegate void FillTextBackground(Bitmap textBackground, PointF textLocation, SizeF textSize);

    public class GLGraphics : IGraphics
    {
        #region Texture Cache

        static Dictionary<object, GLTexture> _textureCacheDictionary = new Dictionary<object, GLTexture>();
        static ImageAttributesCache _imageAttributesCache = new ImageAttributesCache();
        const int MaxCachedTextures = 100;

        public static void ResetAllCaches()
        {
            _textureCacheDictionary = new Dictionary<object, GLTexture>();
            _imageAttributesCache = new ImageAttributesCache();
        }

        private static void ClearTexturesIfNeeded()
        {
            if (_textureCacheDictionary.Count > MaxCachedTextures)
            {
                foreach (var texture in _textureCacheDictionary.Values)
                    texture.Dispose();

                _textureCacheDictionary.Clear();
            }
        }

        private static GLTexture GetCachedTexture(object key, Size originalSize, Action<Graphics> draw)
        {
            GLTexture texture = null;
            if (!_textureCacheDictionary.TryGetValue(key, out texture))
            {
                ClearTexturesIfNeeded();

                texture = GraphicsHelpers.GdiToTexture(originalSize, draw);
                _textureCacheDictionary.Add(key, texture);
            }
            return texture;
        }


        private static GLTexture GetCachedTexture(Image image)
        {
            return GetCachedTexture(image, image.Size, gdi =>
                {
                    using (Bitmap bitmap = new Bitmap(image))
                    {
                        bitmap.SetResolution(72.0f, 72.0f);
                        gdi.DrawImage(bitmap, Point.Empty);
                    }
                });
        }

        private static GLTexture GetCachedTexture(TextureBrush textureBrush)
        {
            return GetCachedTexture(textureBrush, textureBrush.Image.Size, gdi => gdi.DrawImage(textureBrush.Image, Point.Empty));
        }

        #endregion

        #region HatchStyle Cache

        static Dictionary<int, byte[]> _hatchStyleCacheDictionary = new Dictionary<int, byte[]>();

        public static void RegisterHatchStylePattern(HatchStyle hatchStyle, BitArray pattern_32x32)
        {
            _hatchStyleCacheDictionary[(int)hatchStyle] = pattern_32x32.ToBytes();
        }

        byte[] GetCachedHatchStylePattern(HatchStyle hatchStyle)
        {
            byte[] pattern;
            if (_hatchStyleCacheDictionary.TryGetValue((int)hatchStyle, out pattern))
            {
                return pattern;
            }
            return null;
        }

        #endregion

        #region HatchStyle Patterns

        static BitArray CreatePattern_LightUpwardDiagonal()
        {
            BitArray pattern = new BitArray(32 * 32);

            for (int y = 0; y < 32; y++)
            {
                for (int x = 0; x < 32; x++)
                {
                    if ((x + y % 4) % 4 == 0)
                    {
                        pattern[y * 32 + x] = true;
                    }
                }
            }

            return pattern;
        }

        static BitArray CreatePattern_LightDownwardDiagonal()
        {
            BitArray pattern = new BitArray(32 * 32);

            for (int y = 0; y < 32; y++)
            {
                for (int x = 0; x < 32; x++)
                {
                    if ((x + y % 4) % 4 == 0)
                    {
                        pattern[(31 - y) * 32 + x] = true;
                    }
                }
            }

            return pattern;
        }

        static GLGraphics()
        {
            RegisterHatchStylePattern(HatchStyle.LightUpwardDiagonal, CreatePattern_LightUpwardDiagonal());
            RegisterHatchStylePattern(HatchStyle.LightDownwardDiagonal, CreatePattern_LightDownwardDiagonal());
        }

        #endregion

        GLCanvas _canvas;
        Graphics _graphics;
        GraphicsUnit _pageUnit = GraphicsUnit.Pixel;
        Matrix _gdiTransform = new Matrix();
        GLMatrix2D _glTransform = new GLMatrix2D();

        public FillTextBackground FillTextBackground;
        public Color TextBackgroundColor;

        public GLGraphics(CanvasEventArgs e)
        {
            _canvas = e.Canvas;
            _graphics = e.Graphics;
            FillTextBackground = FillTextBackground_Default;
            TextBackgroundColor = _canvas.BackColor;
        }        

        public GLCanvas Canvas { get { return _canvas; } }

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
                if (_canvas.BlendEnabled)
                    return CompositingMode.SourceOver;
                return CompositingMode.SourceCopy;
            }
            set
            {
                _canvas.BlendEnabled = value == CompositingMode.SourceOver;
            }
        }

        public CompositingQuality CompositingQuality
        {
            get { return CompositingQuality.Default; }
            set { }
        }

        public float DpiX
        {
            get { return _canvas.Dpi.X; }
        }

        public float DpiY
        {
            get { return _canvas.Dpi.Y; }
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
                        _canvas.GlobalScale = new PointF(_canvas.Dpi.X / 25.4f, _canvas.Dpi.Y / 25.4f);
                        break;
                    case GraphicsUnit.Pixel:
                        _canvas.GlobalScale = new PointF(1.0f, 1.0f);
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
                        _canvas.AntialiasingEnabled = true;
                        break;
                    default:
                        _canvas.AntialiasingEnabled = false;
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
                _glTransform.SetFromGdiMatrix(value);
                _canvas.Transform(_glTransform);
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
            _canvas.Clear(color);
            TextBackgroundColor = color;
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
            SetCanvasFromPen(pen);
            _canvas.DrawArc(rect, startAngle, sweepAngle, false);
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
            SetCanvasFromPen(pen);
            _canvas.DrawEllipse(rect);
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
            _canvas.CurrentColor = Color.White;
            _canvas.Texture2DEnabled = true;
            var texture = GetCachedTexture(image);
            drawAction(texture);
            _canvas.Texture2DEnabled = false;
        }

        public void DrawImage(Image image, Point point)
        {
            DrawTexture(image, texture =>
                {
                    RectangleF srcRect = new RectangleF(0, 0, texture.Width, texture.Height);
                    RectangleF dstRect = srcRect;
                    dstRect.Offset(point);
                    texture.Draw(dstRect, srcRect);
                });
        }

        public void DrawImage(Image image, Point[] destPoints)
        {
            throw new NotImplementedException();
        }

        public void DrawImage(Image image, PointF point)
        {
            DrawTexture(image, texture =>
            {
                RectangleF srcRect = new RectangleF(0, 0, texture.Width, texture.Height);
                RectangleF dstRect = srcRect;
                dstRect.Offset(point);
                texture.Draw(dstRect, srcRect);
            });
        }

        public void DrawImage(Image image, PointF[] destPoints)
        {
            throw new NotImplementedException();
        }

        public void DrawImage(Image image, Rectangle rect)
        {
            DrawTexture(image, texture =>
            {
                RectangleF dstRect = rect.ToRectangleF();
                RectangleF srcRect = new RectangleF(0, 0, texture.Width, texture.Height);
                texture.Draw(dstRect, srcRect);
            });
        }

        public void DrawImage(Image image, RectangleF rect)
        {
            DrawTexture(image, texture =>
            {
                RectangleF dstRect = rect;
                RectangleF srcRect = new RectangleF(0, 0, texture.Width, texture.Height);
                texture.Draw(dstRect, srcRect);
            });
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

            Bitmap bitmap = _imageAttributesCache.GetOrCreateBitmapFromImageAndAttributes(image, imageAttr);

            DrawTexture(bitmap, texture =>
            {
                RectangleF dstRect = destRect.ToRectangleF();
                RectangleF srcRect = new RectangleF(0, 0, texture.Width, texture.Height);
                texture.Draw(dstRect, srcRect);
            });
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

        private void SetCanvasFromPen(Pen pen)
        {
            _canvas.CurrentColor = pen.Color;
            _canvas.LineWidth = pen.Width;
            switch (pen.DashStyle)
            {
                case DashStyle.Dash:
                    _canvas.SetLineStipplePattern(1, 0xEEEE);
                    break;
                case DashStyle.DashDot:
                    _canvas.SetLineStipplePattern(1, 0xEBAE);
                    break;
                case DashStyle.DashDotDot:
                    _canvas.SetLineStipplePattern(1, 0x5757);
                    break;
                case DashStyle.Dot:
                    _canvas.SetLineStipplePattern(1, 0xAAAA);
                    break;
                case DashStyle.Custom:
                case DashStyle.Solid:
                default:
                    _canvas.ResetLineStipplePattern();
                    break;
            }            
        }

        public void DrawLine(Pen pen, Point pt1, Point pt2)
        {
            SetCanvasFromPen(pen);
             _canvas.DrawLine(pt1, pt2);            
        }

        public void DrawLine(Pen pen, PointF pt1, PointF pt2)
        {
            SetCanvasFromPen(pen);
            _canvas.DrawLine(pt1, pt2);
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
            SetCanvasFromPen(pen);
            _canvas.DrawLines(points, true);
        }

        public void DrawLines(Pen pen, PointF[] points)
        {
            SetCanvasFromPen(pen);
            _canvas.DrawLines(points, true);
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
            SetCanvasFromPen(pen);
            _canvas.DrawArc(rect, startAngle, sweepAngle, true);
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
            SetCanvasFromPen(pen);
            _canvas.DrawRectangle(rect);
        }

        public void DrawRectangle(Pen pen, float x, float y, float width, float height)
        {
            SetCanvasFromPen(pen);
            _canvas.DrawRectangle(new RectangleF(x, y, width, height));
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
            SizeF textSize = _graphics.MeasureString(s, font, point, format);
            DrawStringByPixels(s, font, brush, textSize, new RectangleF(point, SizeF.Empty), format);
        }

        public void DrawString(string s, Font font, Brush brush, RectangleF layoutRectangle, StringFormat format)
        {
            SizeF layoutSize = layoutRectangle.Size;
            if (_pageUnit == GraphicsUnit.Millimeter)
            {
                layoutSize.Width *= _canvas.GlobalScale.X;
                layoutSize.Height *= _canvas.GlobalScale.Y;
            }

            SizeF textSize = _graphics.MeasureString(s, font, layoutSize, format);
            DrawStringByPixels(s, font, brush, textSize, layoutRectangle, format);
        }

        public void DrawString(string s, Font font, Brush brush, float x, float y, StringFormat format)
        {
            DrawString(s, font, brush, new PointF(x, y), format);
        }

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

        public void FillTextBackground_glReadPixels(Bitmap textBackground, PointF textLocation, SizeF textSize)
        {
            textLocation.Y = _canvas.CanvasSize.Height - textLocation.Y - textSize.Height;
            Point textLocationPixel = new Point(
                        (int)Math.Round(textLocation.X, MidpointRounding.AwayFromZero),
                        (int)Math.Round(textLocation.Y, MidpointRounding.AwayFromZero));

            _canvas.FillBitmap(textBackground, textLocationPixel);
            textBackground.RotateFlip(RotateFlipType.RotateNoneFlipY);
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

        private void DrawStringByPixels(string s, Font font, Brush brush, SizeF textSize, RectangleF layoutRectangle, StringFormat format)
        {
            if (_pageUnit == GraphicsUnit.Millimeter)
                layoutRectangle = layoutRectangle.ScaleRect(_canvas.GlobalScale);

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
                if (_textRenderingHint == TextRenderingHint.ClearTypeGridFit)
                {
                    FillTextBackground(bitmap, textLocation, textSize);
                }

                using (Graphics gdi = Graphics.FromImage(bitmap))
                {
                    gdi.TextRenderingHint = _textRenderingHint;
                    gdi.DrawString(s, font, brush, new RectangleF(PointF.Empty, textSize), format);
                }

                bitmap.RotateFlip(RotateFlipType.RotateNoneFlipY);

                _canvas.Texture2DEnabled = false;
                _canvas.CurrentColor = Color.White;

                textLocation.Y = _canvas.CanvasSize.Height - textLocation.Y - textSize.Height;
                _canvas.DrawPixels(bitmap, textLocation);
            }
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
                _canvas.CurrentColor = solid.Color;
                return null;
            }

            if (brush is LinearGradientBrush)
            {
                LinearGradientBrush gradient = (LinearGradientBrush)brush;
                Color[] colors = new Color[4];
                if (GraphicsHelpers.FloatEquals(gradient.Transform.Elements[2], -0.5f))
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
        
        private void SetPatternFromBrush(Brush brush)
        {
            if (brush is HatchBrush)
            {
                HatchBrush hatch = (HatchBrush)brush;
                var pattern = GetCachedHatchStylePattern(hatch.HatchStyle);
                if (pattern != null)
                    _canvas.SetPolygonStipplePattern(pattern);
                _canvas.CurrentColor = hatch.ForegroundColor;
            }
        }

        public void FillRectangle(Brush brush, Rectangle rect)
        {
            SetPatternFromBrush(brush);

            if (brush is TextureBrush)
            {
                _canvas.CurrentColor = Color.White;
                _canvas.Texture2DEnabled = true;
                TextureBrush textureBrush = (TextureBrush)brush;
                var texture = GetCachedTexture(textureBrush);
                texture.DrawTiled(rect);
                _canvas.Texture2DEnabled = false;
            }
            else
            {
                _canvas.FillRectangle(rect, ColorsFromBrush(brush));
            }

            _canvas.ResetPolygonStipplePattern();
        }

        public void FillRectangle(Brush brush, RectangleF rect)
        {
            SetPatternFromBrush(brush);

            _canvas.FillRectangle(rect, ColorsFromBrush(brush));

            _canvas.ResetPolygonStipplePattern();
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
                size.Width /= _canvas.GlobalScale.X;
                size.Height /= _canvas.GlobalScale.Y;
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
            _canvas.ResetClip();
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
            PointF currentScale = _canvas.GlobalScale;
            rect.X = (int)Math.Round(rect.X * currentScale.X);
            rect.Y = (int)Math.Round(rect.Y * currentScale.Y);
            rect.Width = (int)Math.Round(rect.Width * currentScale.X);
            rect.Height = (int)Math.Round(rect.Height * currentScale.Y);
            _canvas.SetClip(rect);
        }

        public void SetClip(RectangleF rect)
        {
            Rectangle clipRect = new Rectangle();
            PointF currentScale = _canvas.GlobalScale;
            clipRect.X = (int)Math.Round(rect.X * currentScale.X);
            clipRect.Y = (int)Math.Round(rect.Y * currentScale.Y);
            clipRect.Width = (int)Math.Round(rect.Width * currentScale.X);
            clipRect.Height = (int)Math.Round(rect.Height * currentScale.Y);
            _canvas.SetClip(clipRect);
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
