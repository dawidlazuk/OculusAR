using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using System.Diagnostics;

namespace ViewProvision.Processors
{
    public class SmoothBilateralProcessor : IImageProcessor
    {
        private readonly int kernelSize;
        private readonly int colorSigma;
        private readonly int spaceSigma;

        public SmoothBilateralProcessor(int kernelSize, int colorSigma, int spaceSigma)
        {
            this.kernelSize = kernelSize;
            this.colorSigma = colorSigma;
            this.spaceSigma = spaceSigma;
        }

        public void Process(ref Image<Bgr, byte> image)
        {
            if (image == null)
                return;

            try
            {
                image = image.SmoothBilatral(kernelSize, colorSigma, spaceSigma);
            }
            catch (CvException ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        public string Name { get; } = "SmoothBiratelar";
        public bool Active { get; set; }
    }
}
