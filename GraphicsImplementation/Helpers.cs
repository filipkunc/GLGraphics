using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using GLWrapper;

namespace GraphicsImplementation
{
    public static class Helpers
    {
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

        public static GLTexture GdiToTexture(Color backColor, Size originalSize, Action<Graphics> draw)
        {
            Size power2Size = new Size(Helpers.NextPow2(originalSize.Width), Helpers.NextPow2(originalSize.Height));
            using (Bitmap bitmap = new Bitmap(power2Size.Width, power2Size.Height))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    if (backColor != Color.Transparent)
                        g.Clear(backColor);
                    draw(g);
                }
                if (backColor != Color.Transparent)
                    bitmap.MakeTransparent(backColor);
                GLTexture texture = new GLTexture(bitmap, originalSize.Width, originalSize.Height);
                return texture;
            }
        }

    }
}
