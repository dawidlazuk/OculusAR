using System;
using System.Drawing;
using System.Runtime.InteropServices;
using Emgu.CV;
using Emgu.CV.Structure;
using UnityEngine;
using System.IO;

namespace Assets.Transmitter
{
    public class TextureConverter : ITextureConverter
    {
        public Texture FromBitmap(Bitmap source)
        {
            var texture = new Texture2D(source.Width, source.Height, TextureFormat.RGB24, false);
            var bytes = ImageToByteArray(source);
            texture.LoadImage(bytes);
            return texture;
        }

        public Texture FromImage(Image<Bgr, byte> source)
        {
            return ImageToTexture2D(source, true);
        }

        public byte[] DataFromImage(Image<Bgr, byte> image)
        {
            return ImageToBytesForTexture(image, true);
        }

        public void LoadFromImage(Image<Bgr, byte> image, Texture text)
        {
           var data = ImageToBytesForTexture(image, true);
            var texture = (Texture2D) text;
            texture.LoadRawTextureData(data);
            texture.Apply();
        }

        private static byte[] ImageToBytesForTexture<TColor, TDepth>(Image<TColor, TDepth> image, bool correctForVerticleFlip)
              where TColor : struct, IColor
              where TDepth : new()
        {
            if (image == null)
                return null;

            Size size = image.Size;

            if (typeof(TColor) == typeof(Rgb) && typeof(TDepth) == typeof(Byte))
            {
                byte[] data = new byte[size.Width * size.Height * 3];
                GCHandle dataHandle = GCHandle.Alloc(data, GCHandleType.Pinned);
                using (Image<Rgb, byte> rgb = new Image<Rgb, byte>(size.Width, size.Height, size.Width * 3, dataHandle.AddrOfPinnedObject()))
                {
                    rgb.ConvertFrom(image);
                    if (correctForVerticleFlip)
                        CvInvoke.Flip(rgb, rgb, Emgu.CV.CvEnum.FlipType.Vertical);
                }
                dataHandle.Free();
                return data;
            }
            else //if (typeof(TColor) == typeof(Rgba) && typeof(TDepth) == typeof(Byte))
            {
                byte[] data = new byte[size.Width * size.Height * 4];
                GCHandle dataHandle = GCHandle.Alloc(data, GCHandleType.Pinned);
                using (Image<Rgba, byte> rgba = new Image<Rgba, byte>(size.Width, size.Height, size.Width * 4, dataHandle.AddrOfPinnedObject()))
                {
                    rgba.ConvertFrom(image);
                    if (correctForVerticleFlip)
                        CvInvoke.Flip(rgba, rgba, Emgu.CV.CvEnum.FlipType.Vertical);
                }
                dataHandle.Free();
                return data;
            }
        }

        private static Texture2D ImageToTexture2D<TColor, TDepth>(Image<TColor, TDepth> image, bool correctForVerticleFlip)
             where TColor : struct, IColor
             where TDepth : new()
        {
            if(image == null)
                return null;

            Size size = image.Size;

            Texture2D texture;
            if (typeof(TColor) == typeof(Rgb) && typeof(TDepth) == typeof(Byte))
            {
                texture = new Texture2D(size.Width, size.Height, TextureFormat.RGB24, false);
            }
            else //if (typeof(TColor) == typeof(Rgba) && typeof(TDepth) == typeof(Byte))
            {
                texture = new Texture2D(size.Width, size.Height, TextureFormat.RGBA32, false);
            }

            var data = ImageToBytesForTexture(image, correctForVerticleFlip);

            texture.LoadRawTextureData(data);
            texture.Apply();
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