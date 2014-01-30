using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Drawing;
using GraphicsImplementation;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Drawing.Text;

namespace GraphicsImplementation
{
    /// <summary>
    /// Wrapper class for the gdi32.dll.
    /// </summary>
    class Gdi32
    {
        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public int x;
            public int y;
            public POINT(int xx, int yy)
            { x = xx; y = yy; }
            public POINT(Point pt)
            { x = pt.X; y = pt.Y; }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int left;
            public int top;
            public int right;
            public int bottom;

            public RECT(int l, int t, int r, int b)
            { left = l; top = t; right = r; bottom = b; }

            public int Width()
            { return right - left; }
            public int Height()
            { return bottom - top; }
            public void Inflate(int dx, int dy)
            {
                if ((dx < 0) && (Width() + 2 * dx < 0))
                {
                    dx = (-Width() / 2);
                }
                left -= dx; right += dx;

                if ((dy < 0) && (Height() + 2 * dy < 0))
                {
                    dy = (-Height() / 2);
                }
                top -= dy; bottom += dy;
            }
            public void Deflate(int dx, int dy)
            {
                Inflate(-dx, -dy);
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct SIZE
        {
            public int cx;
            public int cy;
        };

        [StructLayout(LayoutKind.Sequential)]
        public struct SIZEL
        {
            public int cx;
            public int cy;
        };

        // from WinGDI.h
        public const int DEFAULT_PITCH = 0;
        public const int FIXED_PITCH = 1;
        public const int VARIABLE_PITCH = 2;

        // Stock Logical Objects */
        public const int WHITE_BRUSH = 0;
        public const int LTGRAY_BRUSH = 1;
        public const int GRAY_BRUSH = 2;
        public const int DKGRAY_BRUSH = 3;
        public const int BLACK_BRUSH = 4;
        public const int NULL_BRUSH = 5;
        public const int HOLLOW_BRUSH = NULL_BRUSH;
        public const int WHITE_PEN = 6;
        public const int BLACK_PEN = 7;
        public const int NULL_PEN = 8;
        public const int OEM_FIXED_FONT = 10;
        public const int ANSI_FIXED_FONT = 11;
        public const int ANSI_VAR_FONT = 12;
        public const int SYSTEM_FONT = 13;
        public const int DEVICE_DEFAULT_FONT = 14;
        public const int DEFAULT_PALETTE = 15;
        public const int SYSTEM_FIXED_FONT = 16;

        // Font Families
        public const int FF_DONTCARE = (0 << 4);  // Don't care or don't know.
        public const int FF_ROMAN = (1 << 4);  // Variable stroke width, serifed.
        // Times Roman, Century Schoolbook, etc.
        public const int FF_SWISS = (2 << 4);  // Variable stroke width, sans-serifed.
        // Helvetica, Swiss, etc.
        public const int FF_MODERN = (3 << 4);  // Constant stroke width, serifed or sans-serifed.
        // Pica, Elite, Courier, etc.
        public const int FF_SCRIPT = (4 << 4);  // Cursive, etc.
        public const int FF_DECORATIVE = (5 << 4);  // Old English, etc.


        // Font Weights
        public const int FW_DONTCARE = 0;
        public const int FW_THIN = 100;
        public const int FW_EXTRALIGHT = 200;
        public const int FW_LIGHT = 300;
        public const int FW_NORMAL = 400;
        public const int FW_MEDIUM = 500;
        public const int FW_SEMIBOLD = 600;
        public const int FW_BOLD = 700;
        public const int FW_EXTRABOLD = 800;
        public const int FW_HEAVY = 900;

        public const int FW_ULTRALIGHT = FW_EXTRALIGHT;
        public const int FW_REGULAR = FW_NORMAL;
        public const int FW_DEMIBOLD = FW_SEMIBOLD;
        public const int FW_ULTRABOLD = FW_EXTRABOLD;
        public const int FW_BLACK = FW_HEAVY;


        private const int LF_FACESIZE = 32;

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public class LOGFONT
        {
            public int lfHeight;
            public int lfWidth;
            public int lfEscapement;
            public int lfOrientation;
            public int lfWeight;
            public byte lfItalic;
            public byte lfUnderline;
            public byte lfStrikeOut;
            public byte lfCharSet;
            public byte lfOutPrecision;
            public byte lfClipPrecision;
            public byte lfQuality;
            public byte lfPitchAndFamily;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = LF_FACESIZE)]
            public string lfFaceName = null;
        };

        /// <summary>
        /// Enumeration for the raster operations used in BitBlt.
        /// In C++ these are actually #define. But to use these
        /// constants with C#, a new enumeration type is defined.
        /// </summary>
        public enum TernaryRasterOperations
        {
            SRCCOPY = 0x00CC0020, // dest = source
            SRCPAINT = 0x00EE0086, // dest = source OR dest
            SRCAND = 0x008800C6, // dest = source AND dest
            SRCINVERT = 0x00660046, // dest = source XOR dest
            SRCERASE = 0x00440328, // dest = source AND (NOT dest)
            NOTSRCCOPY = 0x00330008, // dest = (NOT source)
            NOTSRCERASE = 0x001100A6, // dest = (NOT src) AND (NOT dest)
            MERGECOPY = 0x00C000CA, // dest = (source AND pattern)
            MERGEPAINT = 0x00BB0226, // dest = (NOT source) OR dest
            PATCOPY = 0x00F00021, // dest = pattern
            PATPAINT = 0x00FB0A09, // dest = DPSnoo
            PATINVERT = 0x005A0049, // dest = pattern XOR dest
            DSTINVERT = 0x00550009, // dest = (NOT dest)
            BLACKNESS = 0x00000042, // dest = BLACK
            WHITENESS = 0x00FF0062, // dest = WHITE
        };

        // [DllImport("gdi32", EntryPoint="GetObject")] 
        [DllImport("gdi32", CharSet = CharSet.Auto, EntryPoint = "GetObject")]
        public static extern int GetObjectLOGFONT(
            IntPtr hObject,
            int nSize,
            [In, Out] [MarshalAs(UnmanagedType.LPStruct)]
            LOGFONT lpLogFont);

        [DllImport("gdi32", CharSet = CharSet.Auto)]
        public static extern IntPtr CreateFontIndirect(
            [In, MarshalAs(UnmanagedType.LPStruct)]
            LOGFONT lplf);

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public class TEXTMETRIC
        {
            public int tmHeight;
            public int tmAscent;
            public int tmDescent;
            public int tmInternalLeading;
            public int tmExternalLeading;
            public int tmAveCharWidth;
            public int tmMaxCharWidth;
            public int tmWeight;
            public int tmOverhang;
            public int tmDigitizedAspectX;
            public int tmDigitizedAspectY;
            public char tmFirstChar;
            public char tmLastChar;
            public char tmDefaultChar;
            public char tmBreakChar;
            public byte tmItalic;
            public byte tmUnderlined;
            public byte tmStruckOut;
            public byte tmPitchAndFamily;
            public byte tmCharSet;
        }
        [DllImport("gdi32", CharSet = CharSet.Auto, EntryPoint = "GetTextMetrics")]
        public static extern int GetTextMetrics(
            IntPtr hDC,
            [In, Out] [MarshalAs(UnmanagedType.LPStruct)]
            TEXTMETRIC lpMetrics);

        [DllImport("gdi32")]
        public static extern uint GdiSetBatchLimit(
            uint dwLimit);

        // constants for GetDeviceCaps
        public const int LOGPIXELSX = 88;    // Logical pixels/inch in X
        public const int LOGPIXELSY = 90;    // Logical pixels/inch in Y

        [DllImport("gdi32")]
        public static extern int GetDeviceCaps(IntPtr hDC, int nIndex);

        [DllImport("gdi32", EntryPoint = "GetTextExtentPoint32")]
        public static extern int GetTextExtentPoint32(
            IntPtr hDC,
            string lpsz,
            int cbString,
            ref SIZE lpSize);

        [DllImport("gdi32")]
        public static extern bool Rectangle(IntPtr hDC, int left, int top, int right, int bottom);

        [DllImport("gdi32")]
        public static extern int FillRgn(IntPtr hDC, IntPtr hRgn, IntPtr hBrush);

        [DllImport("gdi32")]
        public static extern IntPtr CreateRectRgn(int X1, int Y1, int X2, int Y2);

        [DllImport("gdi32")]
        public static extern int SetROP2(IntPtr hDC, int fnDrawMode);

        [DllImport("gdi32")]
        public static extern bool MoveToEx(IntPtr hDC, int x, int y, ref Point p);

        [DllImport("gdi32")]
        public static extern bool LineTo(IntPtr hDC, int x, int y);

        [DllImport("gdi32")]
        public static extern IntPtr CreatePen(int fnPenStyle, int nWidth, int crColor);

        [DllImport("gdi32")]
        public static extern IntPtr CreateSolidBrush(int crColor);

        [DllImport("gdi32")]
        public static extern IntPtr GetStockObject(int fnObject);

        [DllImport("gdi32")]
        public static extern IntPtr SelectObject(IntPtr hDC, IntPtr hObject);

        [DllImport("gdi32")]
        public static extern bool DeleteObject(IntPtr hObject);

        [DllImport("gdi32")]
        public static extern bool BitBlt(
            IntPtr hObject,
            int nXDest, int nYDest, int nWidth, int nHeight,
            IntPtr hObjectSource,
            int nXSrc, int nYSrc,
            TernaryRasterOperations dwRop);

        [DllImport("gdi32", EntryPoint = "CreateCompatibleBitmap")]
        public static extern IntPtr CreateCompatibleBitmap(IntPtr hdc, int nWidth, int nHeight);

        [DllImport("gdi32", EntryPoint = "CreateCompatibleDC")]
        public static extern IntPtr CreateCompatibleDC(IntPtr hdc);

        [DllImport("gdi32")]
        public static extern bool DeleteDC(IntPtr hDC);

        public const int PHYSICALOFFSETX = 112;
        public const int PHYSICALOFFSETY = 113;        
    }

    public static class Gdi32Extensions
    {
        static private Point _nullPoint = new Point(0, 0);

        // Convert the Argb from .NET to a gdi32 RGB
        public static int ToGdi32RGB(this Color color)
        {
            var argb = color.ToArgb();
            var rgb = ((argb >> 16 & 0x0000FF) | (argb & 0x00FF00) | (argb << 16 & 0xFF0000));
            return rgb;
        }

        public static void DrawLinesGdi32(this Graphics g, Pen pen, List<PointF> points)
        {
            float scaleX, scaleY;

            if (g.PageUnit == GraphicsUnit.Millimeter)
            {
                scaleX = g.DpiX / 25.4f;
                scaleY = g.DpiY / 25.4f;
            }
            else
            {
                scaleX = 1.0f;
                scaleY = 1.0f;
            }

            Matrix transform = g.Transform;
            Matrix scale = new Matrix();
            scale.Scale(scaleX, scaleY);

            transform.Multiply(scale, MatrixOrder.Append);
            
            IntPtr hDC = g.GetHdc();
            IntPtr hPen = Gdi32.CreatePen(0, (int)pen.Width, pen.Color.ToGdi32RGB());
            Gdi32.SelectObject(hDC, hPen);

            //var pointsArray = points.ToArray();
            //transform.TransformPoints(pointsArray); // <-- extremely slow, faster is to manually transform each point

            float[] m = transform.Elements;
            PointF pt;
            int x, y;

            // | m[0] m[1] m[4] |
            // | m[2] m[3] m[5] |

            // x' = x * m[0] + y * m[1] + m[4]
            // y' = x * m[2] + y * m[3] + m[5]

            pt = points[0];
            x = (int)(pt.X * m[0] + pt.Y * m[1] + m[4] + 0.5f);
            y = (int)(pt.X * m[2] + pt.Y * m[3] + m[5] + 0.5f);
            Gdi32.MoveToEx(hDC, x, y, ref _nullPoint);

            for (int i = 1; i < points.Count; i++)
            {
                pt = points[i];
                x = (int)(pt.X * m[0] + pt.Y * m[1] + m[4] + 0.5f);
                y = (int)(pt.X * m[2] + pt.Y * m[3] + m[5] + 0.5f);
                Gdi32.LineTo(hDC, x, y);
            }
            
            Gdi32.DeleteObject(hPen);
            g.ReleaseHdc(hDC);
        }
    }    
}
