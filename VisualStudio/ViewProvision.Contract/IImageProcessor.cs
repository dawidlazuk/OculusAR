using Emgu.CV;
using Emgu.CV.Structure;

namespace ViewProvision.Contract
{
    /// <summary>
    /// Interface required to be implemented by classes with the image processing alghoritms
    /// </summary>
    public interface IImageProcessor
    {
        /// <summary>
        /// Apply the processor to image
        /// </summary>
        /// <param name="image">Image to be processed. Will be replaced by the result image.</param>
        void Process(ref Image<Bgr, byte> image);

        /// <summary>
        /// Name of the image processor
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Value determining if the processor will be applied to images.
        /// </summary>
        bool Active { get; set; }
    }
}