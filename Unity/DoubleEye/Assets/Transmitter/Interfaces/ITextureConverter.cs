using System.Drawing;
using Emgu.CV;
using Emgu.CV.Structure;
using UnityEngine;

namespace Assets.Transmitter
{
    public interface ITextureConverter
    {
        Texture FromBitmap(Bitmap source);
        Texture FromImage(Image<Bgr, byte> source);
    }
}
