using System.Drawing;
using Emgu.CV;
using Emgu.CV.Structure;
using ViewProvision.Contract;

namespace ViewProvision
{
    class ViewDataInternal
    {
        const double StraightAngle = 90.0;

        public Image<Bgr,byte> LeftImage { get; internal set; }
        public Image<Bgr, byte> RightImage { get; internal set; }

        public ViewDataInternal(Image<Bgr, byte> leftImage, Image<Bgr, byte> rightImage)
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
        public ViewDataInternal GetSwapedImages()
        {
            return new ViewDataInternal(leftImage: RightImage, rightImage: LeftImage);
        }

        public ViewDataInternal RotateImages(short leftImageRotationTimes, short rightImageRotationTimes)
        {
            if (leftImageRotationTimes % 4 != 0)
                LeftImage = LeftImage.Rotate(GetRotatonAngle(leftImageRotationTimes), new Bgr(0, 0, 0), true);
            if(rightImageRotationTimes % 4 != 0)
                RightImage = RightImage.Rotate(GetRotatonAngle(rightImageRotationTimes), new Bgr(0, 0, 0), true);
            return this;
        }

        private static double GetRotatonAngle(short rotationTimes)
        {
            return (rotationTimes % 4) * StraightAngle;
        }

        public ViewData External
        {
            get
            {
                Bitmap leftBitmap = null, rightBitmap = null;
                if (LeftImage != null && LeftImage.Width > 0 && LeftImage.Height > 0)
                    leftBitmap = LeftImage.Bitmap;
                if (RightImage != null && RightImage.Width > 0 && RightImage.Height > 0)
                    rightBitmap = RightImage.Bitmap;

                return new ViewData(leftBitmap, rightBitmap);
            }
        }
    }
}
