using System;
using System.Drawing;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Assets.Transmitter
{
    public interface ITextureConverter
    {
        Texture FromBitmap(Bitmap source);
    }

    public class TextureConverter : ITextureConverter
    {
        public Texture FromBitmap(Bitmap source)
        {

            var texture = new Texture2D(source.Width, source.Height, TextureFormat.RGB24, false);
            var bytes = ImageToByteArray(source);
            texture.LoadImage(bytes);
            return texture;
        }

        public byte[] ImageToByteArray(System.Drawing.Image imageIn)
        {
            int t1 = Environment.TickCount;
            var o = System.Drawing.GraphicsUnit.Pixel;
            RectangleF r1 = imageIn.GetBounds(ref o);
            Rectangle r2 = new Rectangle((int)r1.X, (int)r1.Y, (int)r1.Width, (int)r1.Height);
            System.Drawing.Imaging.BitmapData omg = ((Bitmap)imageIn).LockBits(r2, System.Drawing.Imaging.ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            byte[] rgbValues = new byte[r2.Width * r2.Height * 4];
            Marshal.Copy((IntPtr)omg.Scan0, rgbValues, 0, rgbValues.Length);
            ((Bitmap)imageIn).UnlockBits(omg);
            Debug.Log("i2ba time: " + (Environment.TickCount - t1));
            return rgbValues;
        }
    }
}
