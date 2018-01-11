using System.Drawing;
using Emgu.CV;
using Emgu.CV.Structure;
using UnityEngine;

namespace Assets.Transmitter
{
    /// <summary>
    /// Responsible for converting EmguCV image into texture
    /// </summary>
    public interface ITextureConverter
    {
        /// <summary>
        ///Creates new texture object from given image
        /// </summary>
        /// <param name="source">Image to convert</param>
        Texture FromImage(Image<Bgr, byte> source);

        /// <summary>
        /// Loads image into existing texture
        /// </summary>
        /// <param name="sourceImage">Image to convert</param>
        /// <param name="targetTexture">Existing texture for image loading</param>
        void LoadFromImage(Image<Bgr, byte> sourceImage, Texture targetTexture);
    }
}
