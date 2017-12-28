using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using System.Diagnostics;

namespace ViewProvision.Processors
{
    public class GrayImageProcessor : IImageProcessor
    {
        public void Process(ref Image<Bgr, byte> image)
        {
            if (image == null)
                return;
            try
            {
                image = image.Convert<Gray, byte>().Convert<Bgr,byte>();
            }
            catch(CvException ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        public string Name { get; } = "Gray";
        public bool Active { get; set; } = false;
    }

}