using System.Drawing;
using Emgu.CV;
using Emgu.CV.Structure;

namespace ViewProvision.Contract
{

    /// <summary>
    /// Frames class with use of the Emgu.CV.Image as image type
    /// </summary>
    public class ViewDataImage
    {
        const double StraightAngle = 90.0;

        public Image<Bgr,byte> LeftImage { get; set; }
        public Image<Bgr, byte> RightImage { get; set; }

        public ViewDataImage(Image<Bgr, byte> leftImage, Image<Bgr, byte> rightImage)
        {
            this.LeftImage = leftImage;
            this.RightImage = rightImage;
        }              
       
        /// <summary>
        /// Get ViewDataBitmap with the same frames
        /// </summary>
        public ViewDataBitmap Bitmaps
        {
            get
            {
                Bitmap leftBitmap = null, rightBitmap = null;
                if (LeftImage != null && LeftImage.Width > 0 && LeftImage.Height > 0)
                    leftBitmap = LeftImage.Bitmap;
                if (RightImage != null && RightImage.Width > 0 && RightImage.Height > 0)
                    rightBitmap = RightImage.Bitmap;

                return new ViewDataBitmap(leftBitmap, rightBitmap);
            }
        }
    }
}
