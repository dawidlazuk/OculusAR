using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using System.Diagnostics;

namespace ViewProvision
{
    public class GrayImageProcessor : IImageProcessor
    {
        public void Process(ref Image<Bgr, byte> image)
        {
            try
            {
                //for (var i = 0; i < image.Height; i++)
                //    for (var j = 0; j < image.Width; j++)
                //    {
                //        var pixel = image[i, j];
                //        var avg = (pixel.Red + pixel.Blue + pixel.Green) / 3;
                //        image[i, j] = new Bgr(avg, avg, avg);
                //    }
                image = image.Convert<Gray, byte>().Convert<Bgr,byte>();
            }
            catch(CvException ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
    }

}