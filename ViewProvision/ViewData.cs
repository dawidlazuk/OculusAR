using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emgu.CV;
using Emgu.CV.Structure;

namespace ViewProvision
{
    public class ViewData
    {
        public Image<Bgr,Byte> LeftImage { get; private set; }
        public Image<Bgr,Byte> RightImage { get; private set; }

        public ViewData(Image<Bgr,Byte> leftImage, Image<Bgr,Byte> rightImage)
        {
            this.LeftImage = leftImage;
            this.RightImage = rightImage;
        }
    }
}
