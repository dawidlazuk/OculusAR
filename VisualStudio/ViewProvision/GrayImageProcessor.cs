using System.Xml;
using Emgu.CV;
using Emgu.CV.Structure;

namespace ViewProvision
{
    public class GrayImageProcessor : IImageProcessor
    {
        public void Process(Image<Bgr, byte> image)
        {
            for(var i =0; i < image.Width; i++)
            for (var j = 0; j < image.Height; j++)
            {
                var pixel = image[i, j];
                var avg = (pixel.Red + pixel.Blue + pixel.Green) / 3;
                image[i, j] = new Bgr(avg, avg, avg);
            }
        }
    }

}