using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing.Imaging;
using System.Drawing;

namespace GraphicsImplementation
{
    public class ImageAttributesCache
    {
        class Key
        {
            public Image Image;
            public ImageAttributes Attributes;

            public override bool Equals(object obj)
            {
                Key other = obj as Key;
                if (other != null)
                {
                    if (other.Image == Image &&
                        other.Attributes == Attributes)
                    {
                        return true;
                    }
                }
                return false;
            }

            public override int GetHashCode()
            {
                return Image.GetHashCode() ^ Attributes.GetHashCode();
            }            
        }

        Dictionary<Key, Bitmap> _attributesCache = new Dictionary<Key, Bitmap>();

        public ImageAttributesCache()
        {

        }

        public Bitmap GetOrCreateBitmapFromImageAndAttributes(Image image, ImageAttributes attributes)
        {
            Bitmap bitmap;
            Key key = new Key { Image = image, Attributes = attributes };

            if (!_attributesCache.TryGetValue(key, out bitmap))
            {
                bitmap = GraphicsHelpers.BitmapFromImageAndAttributes(image, attributes);
                _attributesCache[key] = bitmap;
            }

            return bitmap;
        }
    }
}
