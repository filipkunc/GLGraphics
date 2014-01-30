using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing.Drawing2D;
using System.Drawing;
using System.Runtime.InteropServices;

namespace D2DTestApp
{
    static class D2DHelpers
    {
        internal static SharpDX.Color4 ToColor4(this Color color)
        {
            uint rgba = BitConverter.ToUInt32(new byte[] { color.R, color.G, color.B, color.A }, 0);
            return new SharpDX.Color4(rgba);
        }

        internal static SharpDX.RectangleF ToD2DRectangleF(this RectangleF rect)
        {
            return new SharpDX.RectangleF(rect.X, rect.Y, rect.Width, rect.Height);
        }

        static Dictionary<DashCap, SharpDX.Direct2D1.CapStyle> _dashCapMap = new Dictionary<DashCap, SharpDX.Direct2D1.CapStyle>
        {
            { DashCap.Flat, SharpDX.Direct2D1.CapStyle.Flat },
            { DashCap.Round, SharpDX.Direct2D1.CapStyle.Round },
            { DashCap.Triangle, SharpDX.Direct2D1.CapStyle.Triangle },
        };

        static Dictionary<LineCap, SharpDX.Direct2D1.CapStyle> _lineCapMap = new Dictionary<LineCap, SharpDX.Direct2D1.CapStyle>
        {
            { LineCap.Flat, SharpDX.Direct2D1.CapStyle.Flat },
            { LineCap.Round, SharpDX.Direct2D1.CapStyle.Round },
            { LineCap.Triangle, SharpDX.Direct2D1.CapStyle.Triangle },
            { LineCap.Square, SharpDX.Direct2D1.CapStyle.Square },
        };

        static Dictionary<DashStyle, SharpDX.Direct2D1.DashStyle> _dashStyleMap = new Dictionary<DashStyle, SharpDX.Direct2D1.DashStyle>
        {
            { DashStyle.Custom, SharpDX.Direct2D1.DashStyle.Custom },
            { DashStyle.Dash, SharpDX.Direct2D1.DashStyle.Dash },
            { DashStyle.DashDot, SharpDX.Direct2D1.DashStyle.DashDot },
            { DashStyle.DashDotDot, SharpDX.Direct2D1.DashStyle.DashDotDot },
            { DashStyle.Dot, SharpDX.Direct2D1.DashStyle.Dot },
            { DashStyle.Solid, SharpDX.Direct2D1.DashStyle.Solid },
        };

        static Dictionary<LineJoin, SharpDX.Direct2D1.LineJoin> _lineJoinMap = new Dictionary<LineJoin, SharpDX.Direct2D1.LineJoin>
        {
            { LineJoin.Bevel, SharpDX.Direct2D1.LineJoin.Bevel },
            { LineJoin.Miter, SharpDX.Direct2D1.LineJoin.Miter },
            { LineJoin.MiterClipped, SharpDX.Direct2D1.LineJoin.MiterOrBevel },
            { LineJoin.Round, SharpDX.Direct2D1.LineJoin.Round },
        };

        internal static SharpDX.Direct2D1.StrokeStyle ToStrokeStyle(this Pen pen, SharpDX.Direct2D1.Factory factory)
        {
            SharpDX.Direct2D1.StrokeStyleProperties properties = new SharpDX.Direct2D1.StrokeStyleProperties
            {
                DashCap = _dashCapMap[pen.DashCap],
                DashOffset = pen.DashOffset,
                DashStyle = _dashStyleMap[pen.DashStyle],
                EndCap = _lineCapMap[pen.EndCap],
                LineJoin = _lineJoinMap[pen.LineJoin],
                MiterLimit = pen.MiterLimit,
                StartCap = _lineCapMap[pen.StartCap],
            };

            if (pen.DashStyle == DashStyle.Custom)
                return new SharpDX.Direct2D1.StrokeStyle(factory, properties, pen.DashPattern);
            return new SharpDX.Direct2D1.StrokeStyle(factory, properties);
        }

        internal static SharpDX.Direct2D1.Bitmap ToD2DBitmap(this System.Drawing.Bitmap bitmap, SharpDX.Direct2D1.RenderTarget renderTarget)
        {
            var sourceArea = new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height);
            var bitmapProperties = new SharpDX.Direct2D1.BitmapProperties(
                new SharpDX.Direct2D1.PixelFormat(SharpDX.DXGI.Format.B8G8R8A8_UNorm, SharpDX.Direct2D1.AlphaMode.Premultiplied));
            var size = new SharpDX.Size2(bitmap.Width, bitmap.Height);

            // Transform pixels from BGRA to RGBA
            int stride = bitmap.Width * sizeof(int);
            using (var tempStream = new SharpDX.DataStream(bitmap.Height * stride, true, true))
            {
                // Lock System.Drawing.Bitmap
                var bitmapData = bitmap.LockBits(sourceArea, System.Drawing.Imaging.ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppPArgb);

                // Convert all pixels 
                for (int y = 0; y < bitmap.Height; y++)
                {
                    int offset = bitmapData.Stride * y;
                    for (int x = 0; x < bitmap.Width; x++)
                    {
                        // Not optimized 
                        byte B = Marshal.ReadByte(bitmapData.Scan0, offset++);
                        byte G = Marshal.ReadByte(bitmapData.Scan0, offset++);
                        byte R = Marshal.ReadByte(bitmapData.Scan0, offset++);
                        byte A = Marshal.ReadByte(bitmapData.Scan0, offset++);
                        int rgba = B | (G << 8) | (R << 16) | (A << 24);
                        tempStream.Write(rgba);
                    }

                }
                bitmap.UnlockBits(bitmapData);
                tempStream.Position = 0;

                return new SharpDX.Direct2D1.Bitmap(renderTarget, size, tempStream, stride, bitmapProperties);
            }
        }
    }
}
