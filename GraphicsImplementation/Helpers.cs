using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using GLWrapper;
using System.Drawing.Imaging;
using System.Collections;

namespace GraphicsImplementation
{
    public delegate void FillTextBackground2(Graphics g, Point textLocationPixel, Rectangle backgroundRect);

    public static class GraphicsHelpers
    {
        public static RectangleF ScaleRect(this RectangleF rect, PointF scale)
        {
            return new RectangleF(rect.X * scale.X, rect.Y * scale.Y, rect.Width * scale.X, rect.Height * scale.Y);
        }

        public static PointF ToPointF(this Point point)
        {
            return new PointF(point.X, point.Y);
        }

        public static SizeF ToSizeF(this Size size)
        {
            return new SizeF(size.Width, size.Height);
        }

        public static RectangleF ToRectangleF(this Rectangle rect)
        {
            return new RectangleF(rect.X, rect.Y, rect.Width, rect.Height);
        }

        public static Rectangle ToRectangle(this RectangleF rect)
        {
            return Rectangle.Round(rect);
        }

        public static int NextPow2(int n)
        {
            int x = 2;
            while (x < n)
                x <<= 1;
            return x;
        }

        public static bool FloatEquals(float a, float b)
        {
            if (Math.Abs(a - b) < 0.01f)
                return true;
            return false;
        }

        public static GLTexture GdiToTexture(Size originalSize, Action<Graphics> draw)
        {
            Size power2Size = new Size(GraphicsHelpers.NextPow2(originalSize.Width), GraphicsHelpers.NextPow2(originalSize.Height));
            using (Bitmap bitmap = new Bitmap(power2Size.Width, power2Size.Height, PixelFormat.Format32bppArgb))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    draw(g);
                }
                GLTexture texture = new GLTexture(bitmap, originalSize.Width, originalSize.Height);
                return texture;
            }
        }

        public static Bitmap BitmapFromImageAndColorMatrix(Image image, ColorMatrix matrix)
        {
            ImageAttributes attributes = new ImageAttributes();
            attributes.SetColorMatrix(matrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);

            return BitmapFromImageAndAttributes(image, attributes);
        }

        public static Bitmap BitmapFromImageAndAttributes(Image image, ImageAttributes attributes)
        {
            Bitmap bitmap = new Bitmap(image.Width, image.Height);
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                g.Clear(Color.Transparent);

                Rectangle rc = new Rectangle(0, 0, image.Width, image.Height);

                g.DrawImage(image, rc,
                    rc.X, rc.Y, rc.Width, rc.Height,
                    GraphicsUnit.Pixel, attributes);
            }

            return bitmap;
        }

        public static void HintTextBackgroundReset(this IGraphics g)
        {
            GLGraphics gl = g as GLGraphics;
            if (gl != null)
                gl.FillTextBackground = gl.FillTextBackground_Default;

        }

        public static void HintTextBackgroundColor(this IGraphics g, Color textBackgroundColor)
        {
            GLGraphics gl = g as GLGraphics;
            if (gl != null)
            {
                gl.FillTextBackground = gl.FillTextBackground_Default;
                gl.TextBackgroundColor = textBackgroundColor;
            }
        }        

        public static void HintTextBackgroundAction(this IGraphics g, FillTextBackground2 fillBackground)
        {
            if (fillBackground == null)
                return;

            GLGraphics gl = g as GLGraphics;
            if (gl != null)
            {
                gl.FillTextBackground = (textBackground, textLocation, textSize) =>
                {
                    Point textLocationPixel = new Point(
                    (int)Math.Round(textLocation.X, MidpointRounding.AwayFromZero),
                    (int)Math.Round(textLocation.Y, MidpointRounding.AwayFromZero));

                    using (Graphics gdi = Graphics.FromImage(textBackground))
                    {
                        fillBackground(gdi, textLocationPixel, new Rectangle(0, 0, textBackground.Width, textBackground.Height));
                    }
                };
            }
        }

        public static FillTextBackground SaveHintTextBackgroundAction(this IGraphics g)
        {
            GLGraphics gl = g as GLGraphics;
            if (gl != null)
                return gl.FillTextBackground;
            return null;
        }

        public static void RestoreHintTextBackgroundAction(this IGraphics g, FillTextBackground fillBackground)
        {
            GLGraphics gl = g as GLGraphics;
            if (gl != null)
                gl.FillTextBackground = fillBackground;
        }

        public static byte[] ToBytes(this BitArray bits)
        {
            byte[] ret = new byte[bits.Length / 8];
            bits.CopyTo(ret, 0);
            return ret;
        }
    }
}
