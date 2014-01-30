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
    public class D2DGraphics : GraphicsBase
    {
        SharpDX.Direct2D1.RenderTarget _renderTarget;
        Graphics _graphics;
        GraphicsUnit _pageUnit = GraphicsUnit.Pixel;
        Matrix _gdiTransform = new Matrix();

        #region RenderTarget Cache

        static Dictionary<SharpDX.Direct2D1.RenderTarget, RenderTargetCache> _renderTargetCache =
            new Dictionary<SharpDX.Direct2D1.RenderTarget, RenderTargetCache>();

        class RenderTargetCache
        {
            public Dictionary<TextureBrush, SharpDX.Direct2D1.BitmapBrush> TextureBrushCache =
                new Dictionary<TextureBrush, SharpDX.Direct2D1.BitmapBrush>();
        }

        #endregion

        public D2DGraphics(D2DView.RenderTargetEventArgs e)
        {
            _renderTarget = e.RenderTarget;
            _graphics = e.Graphics;

            _renderTarget.Transform = SharpDX.Matrix3x2.Identity;

            if (!_renderTargetCache.ContainsKey(_renderTarget))
                _renderTargetCache[_renderTarget] = new RenderTargetCache();
        }

        public SharpDX.Direct2D1.RenderTarget Canvas { get { return _renderTarget; } }

        public override CompositingMode CompositingMode
        {
            get { return CompositingMode.SourceOver; }
            set { }
        }

        public override float DpiX
        {
            get { return _renderTarget.DotsPerInch.Width; }
        }

        public override float DpiY
        {
            get { return _renderTarget.DotsPerInch.Height; }
        }

        SharpDX.Matrix3x2 _pageUnitMatrix = SharpDX.Matrix3x2.Identity;
        SharpDX.Matrix3x2 _transformMatrix = SharpDX.Matrix3x2.Identity;

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
                        _pageUnitMatrix = SharpDX.Matrix3x2.Scaling(new SharpDX.Vector2(DpiX / 25.4f, DpiY / 25.4f));
                        _renderTarget.Transform = _transformMatrix * _pageUnitMatrix;
                        break;
                    case GraphicsUnit.Pixel:
                        _pageUnitMatrix = SharpDX.Matrix3x2.Identity;
                        _renderTarget.Transform = _transformMatrix * _pageUnitMatrix;
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
                        _renderTarget.AntialiasMode = SharpDX.Direct2D1.AntialiasMode.PerPrimitive;
                        break;
                    default:
                        _renderTarget.AntialiasMode = SharpDX.Direct2D1.AntialiasMode.Aliased;
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
            _transformMatrix = new SharpDX.Matrix3x2(matrix.Elements);
            _renderTarget.Transform = _transformMatrix * _pageUnitMatrix;
        }

        public override void Clear(Color color)
        {
            _renderTarget.Clear(color.ToColor4());
            TextBackgroundColor = color;
        }

        public override void Dispose()
        {

        }

        public override void DrawImage(Image image, RectangleF rect)
        {
            using (var bitmap = new Bitmap(image))
            using (var d2dBitmap = bitmap.ToD2DBitmap(_renderTarget))
            {
                _renderTarget.DrawBitmap(d2dBitmap, rect.ToD2DRectangleF(), 1.0f, SharpDX.Direct2D1.BitmapInterpolationMode.Linear);
            }
        }

        public override void DrawImage(Image image, Rectangle destRect, int srcX, int srcY, int srcWidth, int srcHeight, GraphicsUnit srcUnit, ImageAttributes imageAttr)
        {
            throw new NotImplementedException();
        }

        float StrokeWidthFromPen(Pen pen)
        {
            var strokeWidth = pen.Width == 0.0f ? 1.0f : pen.Width;

            if (_pageUnit == GraphicsUnit.Millimeter)
                strokeWidth *= 25.4f / _renderTarget.DotsPerInch.Width;

            return strokeWidth;
        }

        public override void DrawLine(Pen pen, Point pt1, Point pt2)
        {
            DrawLine(pen, new PointF(pt1.X, pt1.Y), new PointF(pt2.X, pt2.Y));
        }

        public override void DrawLine(Pen pen, PointF pt1, PointF pt2)
        {
            var strokeWidth = StrokeWidthFromPen(pen);
            var strokeStyle = pen.ToStrokeStyle(_renderTarget.Factory);

            _renderTarget.DrawLine(new SharpDX.Vector2(pt1.X, pt1.Y), new SharpDX.Vector2(pt2.X, pt2.Y),
                new SharpDX.Direct2D1.SolidColorBrush(_renderTarget, pen.Color.ToColor4()),
                strokeWidth, strokeStyle);
        }

        public override void DrawLines(Pen pen, Point[] points)
        {
            var brush = new SharpDX.Direct2D1.SolidColorBrush(_renderTarget, pen.Color.ToColor4());

            var strokeStyle = pen.ToStrokeStyle(_renderTarget.Factory);
            var strokeWidth = StrokeWidthFromPen(pen);

            for (int i = 1; i < points.Length; i++)
            {
                _renderTarget.DrawLine(
                    new SharpDX.Vector2(points[i - 1].X, points[i - 1].Y),
                    new SharpDX.Vector2(points[i].X, points[i].Y),
                    brush, strokeWidth, strokeStyle);
            }
        }

        public override void DrawLines(Pen pen, PointF[] points)
        {
            var brush = new SharpDX.Direct2D1.SolidColorBrush(_renderTarget, pen.Color.ToColor4());

            var strokeStyle = pen.ToStrokeStyle(_renderTarget.Factory);
            var strokeWidth = StrokeWidthFromPen(pen);

            for (int i = 1; i < points.Length; i++)
            {
                _renderTarget.DrawLine(
                    new SharpDX.Vector2(points[i - 1].X, points[i - 1].Y),
                    new SharpDX.Vector2(points[i].X, points[i].Y),
                    brush, strokeWidth, strokeStyle);
            }
        }

        public override void DrawPath(Pen pen, GraphicsPath path)
        {
            var pathPoints = path.GetPathPoints();

            foreach (var points in pathPoints)
                DrawLines(pen, points);
        }

        public override void DrawRectangle(Pen pen, Rectangle rect)
        {
            DrawRectangle(pen, (float)rect.X, (float)rect.Y, (float)rect.Width, (float)rect.Height);
        }

        public override void DrawRectangle(Pen pen, float x, float y, float width, float height)
        {
            using (var brush = new SharpDX.Direct2D1.SolidColorBrush(_renderTarget, pen.Color.ToColor4()))
            {
                _renderTarget.DrawRectangle(new SharpDX.RectangleF(x, y, width, height), brush);
            }
        }

        protected override void DrawStringImage(Bitmap bitmap, PointF textLocation, SizeF textSize)
        {
            var oldTransform = _renderTarget.Transform;
            _renderTarget.Transform = SharpDX.Matrix3x2.Identity;

            using (var d2dBitmap = bitmap.ToD2DBitmap(_renderTarget))
            {
                _renderTarget.DrawBitmap(d2dBitmap, new RectangleF(textLocation, textSize).ToD2DRectangleF(), 1.0f, SharpDX.Direct2D1.BitmapInterpolationMode.NearestNeighbor);
            }

            _renderTarget.Transform = oldTransform;
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

        public override void FillPath(Brush brush, GraphicsPath path)
        {
            var vertices = path.Triangulate(v => new SharpDX.Vector2((float)v.X, (float)v.Y));
            var triangles = new List<SharpDX.Direct2D1.Triangle>();

            for (int i = 0; i < vertices.Count; i += 3)
            {
                SharpDX.Direct2D1.Triangle triangle = new SharpDX.Direct2D1.Triangle();
                triangle.Point1 = vertices[i];
                triangle.Point2 = vertices[i + 1];
                triangle.Point3 = vertices[i + 2];
                triangles.Add(triangle);
            }

            using (var mesh = new SharpDX.Direct2D1.Mesh(_renderTarget, triangles.ToArray()))
            {
                var oldMode = _renderTarget.AntialiasMode;
                _renderTarget.AntialiasMode = SharpDX.Direct2D1.AntialiasMode.Aliased;
                _renderTarget.FillMesh(mesh, new SharpDX.Direct2D1.SolidColorBrush(_renderTarget, ((SolidBrush)brush).Color.ToColor4()));
                _renderTarget.AntialiasMode = oldMode;
            }
        }

        public override void FillRectangle(Brush brush, Rectangle rect)
        {
            FillRectangle(brush, rect.ToRectangleF());
        }

        public override void FillRectangle(Brush brush, RectangleF rect)
        {
            var textureBrushCache = _renderTargetCache[_renderTarget].TextureBrushCache;
            var textureBrush = brush as TextureBrush;

            SharpDX.Direct2D1.BitmapBrush bitmapBrush;
            if (!textureBrushCache.TryGetValue(textureBrush, out bitmapBrush))
            {
                Bitmap bitmap = textureBrush.Image as Bitmap;

                bitmapBrush = new SharpDX.Direct2D1.BitmapBrush(_renderTarget, bitmap.ToD2DBitmap(_renderTarget),
                    new SharpDX.Direct2D1.BitmapBrushProperties
                    {
                        ExtendModeX = SharpDX.Direct2D1.ExtendMode.Wrap,
                        ExtendModeY = SharpDX.Direct2D1.ExtendMode.Wrap,
                        InterpolationMode = SharpDX.Direct2D1.BitmapInterpolationMode.Linear
                    });

                textureBrushCache[textureBrush] = bitmapBrush;
            }

            _renderTarget.FillRectangle(rect.ToD2DRectangleF(), bitmapBrush);
        }

        public override void ResetClip()
        {
            throw new NotImplementedException();
        }

        public override void SetClip(Rectangle rect)
        {
            throw new NotImplementedException();
        }

        public override void SetClip(RectangleF rect)
        {
            throw new NotImplementedException();
        }
    }
}
