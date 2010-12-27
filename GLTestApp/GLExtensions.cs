using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GLWrapper;
using System.Drawing;
using System.Windows.Forms;

namespace GLTestApp
{
    public static class GLExtensions
    {
        static GLTexture _fontTexture;

        public static void GdiToTexture(this GLTexture texture, int width, int height, Action<Graphics> draw)
        {
            using (Bitmap bitmap = new Bitmap(width, height))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    draw(g);
                }
                texture.Update(bitmap);
            }
        }

        public static void DrawString(this GLCanvas canvas, string s, Font font, PointF location)
        {
            bool textureEnabled = canvas.Texture2DEnabled;
            canvas.Texture2DEnabled = true;

            if (_fontTexture == null)
                _fontTexture = new GLTexture();

            Size size = TextRenderer.MeasureText(s, font);
            size.Width = nextPow2(size.Width);
            size.Height = nextPow2(size.Height);

            _fontTexture.GdiToTexture(size.Width, size.Height, g =>
            {
                using (SolidBrush brush = new SolidBrush(canvas.CurrentColor))
                {
                    g.DrawString(s, font, brush, 0.0f, 0.0f);
                }
            });

            Color oldColor = canvas.CurrentColor;
            canvas.CurrentColor = Color.White;
            _fontTexture.Draw(location);
            canvas.CurrentColor = oldColor;
            canvas.Texture2DEnabled = textureEnabled;
        }

        private static int nextPow2(int n)
        {
            int x = 2;
            while (x < n)
                x <<= 1;
            return x;
        }
    }
}
