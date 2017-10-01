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
        public Image<Bgr,Byte> LeftImage { get; internal set; }
        public Image<Bgr,Byte> RightImage { get; internal set; }

        public ViewData(Image<Bgr,Byte> leftImage, Image<Bgr,Byte> rightImage)
        {
            this.LeftImage = leftImage;
            this.RightImage = rightImage;
        }

        /// <summary>
        /// Swap left and right images of the object.
        /// </summary>
        public void SwapImages()
        {
            var left = LeftImage;
            LeftImage = RightImage;
            RightImage = left;
        }
        
        /// <summary>
        /// Create new object with swapped image's sides.
        /// </summary>
        /// <returns>Swapped ViewData</returns>
        public ViewData GetSwapedImages()
        {
            return new ViewData(leftImage: RightImage, rightImage: LeftImage);
        }
    }
}
