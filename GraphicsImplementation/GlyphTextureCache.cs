﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using GLWrapper;
using System.Windows.Forms;
using System.Drawing.Text;

namespace GraphicsImplementation
{
    public class GlyphTextureCache : IDisposable
    {
        class GlyphInfo
        {
            public Rectangle GlyphRect;
            public SizeF RealSize;
        };

        Dictionary<char, GlyphInfo> _glyphCoords;
        GLTexture _texture;
        Bitmap _bitmap;
        Point _lastTextureCoord;
        int _maxY;
        int _lastGlyphCount;

        public GlyphTextureCache()
        {
            _glyphCoords = new Dictionary<char, GlyphInfo>();
            _texture = new GLTexture();
            _bitmap = new Bitmap(512, 512);
            _lastTextureCoord = new Point(1, 1);
            _lastGlyphCount = 0;
            _maxY = 1;
        }

        private void AddString(string text, Font font, Brush brush, TextRenderingHint textRenderingHint)
        {
            using (Graphics g = Graphics.FromImage(_bitmap))
            {
                g.TextRenderingHint = textRenderingHint;

                StringFormat sf = StringFormat.GenericTypographic;
                sf.FormatFlags = StringFormatFlags.MeasureTrailingSpaces;

                foreach (var c in text)
                {
                    if (_glyphCoords.ContainsKey(c))
                        continue;

                    string s = c.ToString();
                    var size = g.MeasureString(s, font, PointF.Empty, sf);
                    Size integerSize = new Size((int)size.Width + 1, (int)size.Height + 1);
                    g.DrawString(s, font, brush, _lastTextureCoord, sf);
                    var rc = new Rectangle(_lastTextureCoord, integerSize);

                    _glyphCoords.Add(c, new GlyphInfo { GlyphRect = rc, RealSize = size });

                    _lastTextureCoord.X += rc.Width;
                    _maxY = Math.Max(_maxY, rc.Bottom);

                    if (_lastTextureCoord.X > _bitmap.Width)
                    {
                        _lastTextureCoord.X = 1;
                        _lastTextureCoord.Y = _maxY + 1;
                    }
                }
            }
        }

        private void UpdateTextureIfNeeded()
        {
            if (_lastGlyphCount != _glyphCoords.Count)
            {
                using (Bitmap copy = new Bitmap(_bitmap))
                {
                    _texture.Update(copy, _bitmap.Width, _bitmap.Height);
                }
                _lastGlyphCount = _glyphCoords.Count;
            }
        }

        public void DrawBitmap(Graphics g)
        {
            g.DrawImage(_bitmap, Point.Empty);
        }

        public void DrawTexture(GLCanvas g)
        {
            RectangleF rc = new RectangleF(0, 0, _texture.OriginalWidth, _texture.OriginalHeight);

            _texture.Draw(rc, rc);
        }

        public void DrawString(IGraphics g, string text, Font font, Brush brush, PointF location)
        {
            int oldCount = _glyphCoords.Count;
            AddString(text, font, brush, g.TextRenderingHint);
            if (g is GLGraphics)
                UpdateTextureIfNeeded();

            List<RectangleF> glyphDst = new List<RectangleF>();
            List<Rectangle> glyphSrc = new List<Rectangle>();

            location.X += 2.0f; // magic ... really don't know how to get this offset from MeasureString

            foreach (var c in text)
            {
                var glyphCoord = _glyphCoords[c];

                var glyphSize = glyphCoord.RealSize;
                glyphSize.Width = (float)Math.Ceiling(glyphSize.Width);
                glyphSize.Height = (float)Math.Ceiling(glyphSize.Height);

                glyphDst.Add(new RectangleF(location, glyphSize));
                glyphSrc.Add(glyphCoord.GlyphRect);

                if (g is GDIGraphics)
                    g.DrawImage(_bitmap, location.X, location.Y, glyphCoord.GlyphRect.ToRectangleF(), GraphicsUnit.Pixel);

                location.X += (float)Math.Round(glyphCoord.RealSize.Width + 0.2f); // another magic
            }

            if (g is GLGraphics)
                _texture.DrawGlyphs(glyphDst, glyphSrc);
        }

        public void Dispose()
        {
            _texture.Dispose();
        }
    }
}
