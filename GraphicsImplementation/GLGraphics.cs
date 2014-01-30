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
using GLWrapper;

namespace GraphicsImplementation
{
    public delegate void FillTextBackground(Bitmap textBackground, PointF textLocation, SizeF textSize);    

    public class GLGraphics : GraphicsBase
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
                        bitmap.SetResolution(96.0f, 96.0f);
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

        public GLGraphics(CanvasEventArgs e)
        {
            _canvas = e.Canvas;
            _graphics = e.Graphics;
            FillTextBackground = FillTextBackground_Default;
            TextBackgroundColor = _canvas.BackColor;
        }

        public GLCanvas Canvas { get { return _canvas; } }

        public override CompositingMode CompositingMode
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

        public override float DpiX
        {
            get { return _canvas.Dpi.X; }
        }

        public override float DpiY
        {
            get { return _canvas.Dpi.Y; }
        }

        public override GraphicsUnit PageUnit
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

        SmoothingMode _smoothingMode = SmoothingMode.None;

        public override SmoothingMode SmoothingMode
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

        TextRenderingHint _textRenderingHint = TextRenderingHint.ClearTypeGridFit;

        public override TextRenderingHint TextRenderingHint
        {
            get { return _textRenderingHint; }
            set { _textRenderingHint = value; }
        }

        protected override void SetTransform(Matrix matrix)
        {
            _glTransform.SetFromGdiMatrix(matrix);
            _canvas.Transform(_glTransform);
        }

        public override void Clear(Color color)
        {
            _canvas.Clear(color);
            TextBackgroundColor = color;
        }

        public override void Dispose()
        {

        }

        private void DrawTexture(Image image, Action<GLTexture> drawAction)
        {
            _canvas.CurrentColor = Color.White;
            _canvas.Texture2DEnabled = true;
            var texture = GetCachedTexture(image);
            drawAction(texture);
            _canvas.Texture2DEnabled = false;
        }

        public override void DrawImage(Image image, RectangleF rect)
        {
            DrawTexture(image, texture =>
            {
                RectangleF dstRect = rect;
                RectangleF srcRect = new RectangleF(0, 0, texture.OriginalWidth, texture.OriginalHeight);

                texture.Draw(dstRect, srcRect);
            });
        }

        public override void DrawImage(Image image, Rectangle destRect, int srcX, int srcY, int srcWidth, int srcHeight, GraphicsUnit srcUnit, ImageAttributes imageAttr)
        {
            if (srcUnit != GraphicsUnit.Pixel)
                throw new NotImplementedException();

            Bitmap bitmap = _imageAttributesCache.GetOrCreateBitmapFromImageAndAttributes(image, imageAttr);

            DrawTexture(bitmap, texture =>
            {
                RectangleF dstRect = destRect.ToRectangleF();
                RectangleF srcRect = new RectangleF(0, 0, texture.OriginalWidth, texture.OriginalHeight);
                texture.Draw(dstRect, srcRect);
            });
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

        public override void DrawLine(Pen pen, Point pt1, Point pt2)
        {
            SetCanvasFromPen(pen);
            _canvas.DrawLine(pt1, pt2);
        }

        public override void DrawLine(Pen pen, PointF pt1, PointF pt2)
        {
            SetCanvasFromPen(pen);
            _canvas.DrawLine(pt1, pt2);
        }

        public override void DrawLines(Pen pen, Point[] points)
        {
            SetCanvasFromPen(pen);
            _canvas.DrawLines(points, true);
        }

        public override void DrawLines(Pen pen, PointF[] points)
        {
            SetCanvasFromPen(pen);
            _canvas.DrawLines(points, true);
        }

        public override void DrawPath(Pen pen, GraphicsPath path)
        {
            SetCanvasFromPen(pen);
            var pathPoints = path.GetPathPoints();

            foreach (var points in pathPoints)
                _canvas.DrawLines(points, true);
        }

        public override void DrawRectangle(Pen pen, Rectangle rect)
        {
            SetCanvasFromPen(pen);
            _canvas.DrawRectangle(rect);
        }

        public override void DrawRectangle(Pen pen, float x, float y, float width, float height)
        {
            SetCanvasFromPen(pen);
            _canvas.DrawRectangle(new RectangleF(x, y, width, height));
        }

        protected override void DrawStringImage(Bitmap bitmap, PointF textLocation, SizeF textSize)
        {
            bitmap.RotateFlip(RotateFlipType.RotateNoneFlipY);

            _canvas.Texture2DEnabled = false;
            _canvas.CurrentColor = Color.White;

            textLocation.Y = _canvas.CanvasSize.Height - textLocation.Y - textSize.Height;
            _canvas.DrawPixels(bitmap, textLocation);
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

        public override void FillRectangle(Brush brush, Rectangle rect)
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

        public override void FillRectangle(Brush brush, RectangleF rect)
        {
            SetPatternFromBrush(brush);

            if (brush is TextureBrush)
            {
                _canvas.CurrentColor = Color.White;
                _canvas.Texture2DEnabled = true;
                TextureBrush textureBrush = (TextureBrush)brush;
                var texture = GetCachedTexture(textureBrush);
                texture.DrawTiled(rect.ToRectangle());
                _canvas.Texture2DEnabled = false;
            }
            else
            {
                _canvas.FillRectangle(rect, ColorsFromBrush(brush));
            }

            _canvas.ResetPolygonStipplePattern();
        }

        public override void FillPath(Brush brush, GraphicsPath path)
        {
            var solidBrush = (SolidBrush)brush;
            var vertices = path.Triangulate(v => new GLPoint((float)v.X, (float)v.Y, 0.0f, solidBrush.Color));

            _canvas.DrawTriangles(vertices.ToArray(), false);
        }

        protected override Graphics GraphicsForMeasureString
        {
            get
            {
                _graphics.PageUnit = PageUnit;
                _graphics.Transform = Transform;                
                return _graphics;
            }
        }

        public override void ResetClip()
        {
            _canvas.ResetClip();
        }

        public override void SetClip(Rectangle rect)
        {
            Rectangle clipRect = new Rectangle();
            PointF currentScale = _canvas.GlobalScale;
            clipRect.X = (int)Math.Round(rect.X * currentScale.X);
            clipRect.Y = (int)Math.Round(rect.Y * currentScale.Y);
            clipRect.Width = (int)Math.Round(rect.Width * currentScale.X);
            clipRect.Height = (int)Math.Round(rect.Height * currentScale.Y);
            _canvas.SetClip(clipRect);
        }

        public override void SetClip(RectangleF rect)
        {
            Rectangle clipRect = new Rectangle();
            PointF currentScale = _canvas.GlobalScale;
            clipRect.X = (int)Math.Round(rect.X * currentScale.X);
            clipRect.Y = (int)Math.Round(rect.Y * currentScale.Y);
            clipRect.Width = (int)Math.Round(rect.Width * currentScale.X);
            clipRect.Height = (int)Math.Round(rect.Height * currentScale.Y);
            _canvas.SetClip(clipRect);
        }        
    }
}
