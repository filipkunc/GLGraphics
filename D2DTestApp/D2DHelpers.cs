using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing.Drawing2D;
using System.Drawing;

namespace D2DTestApp
{
    static class D2DHelpers
    {
        internal static SharpDX.Color4 ToColor4(this Color color)
        {
            uint rgba = BitConverter.ToUInt32(new byte[] { color.R, color.G, color.B, color.A }, 0);
            return new SharpDX.Color4(rgba);
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
    }
}
