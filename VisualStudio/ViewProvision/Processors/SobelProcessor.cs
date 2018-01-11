using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using System.Diagnostics;
using ViewProvision.Contract;

namespace ViewProvision.Processors
{
    public class SobelProcessor : IImageProcessor
    {
        public void Process(ref Image<Bgr, byte> image)
        {
            if (image == null)
                return;

            try
            {
                image = image.Sobel(0, 1, 3).Add(image.Sobel(1, 0, 3)).Convert<Bgr,byte>();
            }
            catch (CvException ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        public string Name { get; } = "Sobel";
        public bool Active { get; set; }
    }
}
