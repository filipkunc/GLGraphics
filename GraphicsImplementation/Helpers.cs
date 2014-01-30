using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using GLWrapper;
using System.Drawing.Imaging;
using System.Collections;
using System.Drawing.Drawing2D;

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
            GraphicsBase gl = g as GraphicsBase;
            if (gl != null)
                gl.FillTextBackground = gl.FillTextBackground_Default;
        }

        public static void HintTextBackgroundColor(this IGraphics g, Color textBackgroundColor)
        {
            GraphicsBase gl = g as GraphicsBase;
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

            GraphicsBase gl = g as GraphicsBase;
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
            GraphicsBase gl = g as GraphicsBase;
            if (gl != null)
                return gl.FillTextBackground;
            return null;
        }

        public static void RestoreHintTextBackgroundAction(this IGraphics g, FillTextBackground fillBackground)
        {
            GraphicsBase gl = g as GraphicsBase;
            if (gl != null)
                gl.FillTextBackground = fillBackground;
        }

        public static byte[] ToBytes(this BitArray bits)
        {
            byte[] ret = new byte[bits.Length / 8];
            bits.CopyTo(ret, 0);
            return ret;
        }

        public static List<PointF[]> GetPathPoints(this GraphicsPath path)
        {
            path.Flatten(new Matrix(), 0.1f);

            List<PointF[]> pathPoints = new List<PointF[]>();
            List<PointF> currentPoints = null;

            for (int i = 0; i < path.PointCount; i++)
            {
                var point = path.PathPoints[i];

                switch (path.PathTypes[i])
                {
                    case 0x00: // Indicates that the point is the start of a figure. 
                        {
                            if (currentPoints != null)
                            {
                                pathPoints.Add(currentPoints.ToArray());
                            }
                            currentPoints = new List<PointF>();
                            currentPoints.Add(point);
                        } break;
                    case 0x81:
                        {
                            currentPoints.Add(point);
                            currentPoints.Add(currentPoints[0]);
                        } break;
                    case 0x01: // Indicates that the point is one of the two endpoints of a line. 
                        {
                            currentPoints.Add(point);
                        } break;
                    case 0x83: //todo: todle je kombinace
                    case 0x03: // Indicates that the point is an endpoint or control point of a cubic Bézier spline. 
                        throw new NotImplementedException();
                    case 0x07: // Masks all bits except for the three low-order bits, which indicate the point type.
                        throw new NotImplementedException();
                    case 0x20: // Specifies that the point is a marker. 
                        throw new NotImplementedException();
                    case 0x80: // Specifies that the point is the last point in a closed subpath (figure).
                        currentPoints.Add(point);
                        break;
                    default:
                        throw new NotImplementedException();
                }
            }

            if (currentPoints != null)
                pathPoints.Add(currentPoints.ToArray());

            return pathPoints;
        }

        /// <summary>
        /// Triangulation using https://triangle.codeplex.com/
        /// </summary>
        /// <typeparam name="TVertex"></typeparam>
        /// <param name="path"></param>
        /// <param name="converter"></param>
        /// <returns></returns>
        public static List<TVertex> Triangulate<TVertex>(this GraphicsPath path, Func<TriangleNet.Data.Vertex, TVertex> converter)
        {
            List<TVertex> vertices = new List<TVertex>();
            
            var pathPoints = path.GetPathPoints();

            foreach (var points in pathPoints)
            {
                TriangleNet.Geometry.InputGeometry input = new TriangleNet.Geometry.InputGeometry();
                foreach (var point in points)
                    input.AddPoint(point.X, point.Y);
                TriangleNet.Mesh mesh = new TriangleNet.Mesh();
                mesh.Triangulate(input);

                var triangles = mesh.Triangles;

                foreach (var triangle in triangles)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        var vertex = triangle.GetVertex(i);
                        vertices.Add(converter(vertex));
                    }
                }
            }

            return vertices;
        }
    }
}
