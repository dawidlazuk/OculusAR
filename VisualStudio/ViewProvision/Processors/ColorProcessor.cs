using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emgu.CV;
using Emgu.CV.Structure;
using ViewProvision.Contract;

namespace ViewProvision.Processors
{
    public class ColorProcessor: IImageProcessor
    {
        public static Image<Bgr, byte> BackgroundToGreen(Image<Bgr, byte> rgbimage)
        {
            Image<Bgr, byte> ret = rgbimage;
            var image = rgbimage.InRange(new Bgr(0, 0, 0), new Bgr(50, 50, 50));
            var mat = rgbimage.Mat;
            mat.SetTo(new MCvScalar(50, 50, 200), image);
            mat.CopyTo(ret);
            return ret;
        }

        public void Process(ref Image<Bgr, byte> image)
        {
            image = BackgroundToGreen(image);
        }

        public string Name { get; } = "Color";
        public bool Active { get; set; }
    }
}
