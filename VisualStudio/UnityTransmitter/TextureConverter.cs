using System.Drawing;
using System.IO;
using UnityEngine;

namespace UnityTransmitter
{
    public class TextureConverter : ITextureConverter
    {
        public Texture FromBitmap(Bitmap bitmap)
        {
            var texture = new Texture2D(bitmap.Width, bitmap.Height);

            MemoryStream memoryStream = new MemoryStream();
            bitmap.Save(memoryStream, bitmap.RawFormat);

            texture.LoadImage(memoryStream.ToArray());

            return texture;
        }
    }
}