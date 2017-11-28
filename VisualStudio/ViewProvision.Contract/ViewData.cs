using System.Drawing;
using System.Runtime.Serialization;

namespace ViewProvision.Contract
{
    [DataContract]
    public class ViewData
    {
        const double StraightAngle = 90.0;

        [DataMember]
        public Bitmap LeftImage { get; internal set; }

        [DataMember]
        public Bitmap RightImage { get; internal set; }

        public ViewData(Bitmap leftImage, Bitmap rightImage)
        {
            this.LeftImage = leftImage;
            this.RightImage = rightImage;
        }

        ///// <summary>
        ///// Swap left and right images of the object.
        ///// </summary>
        //public void SwapImages()
        //{
        //    var left = LeftImage;
        //    LeftImage = RightImage;
        //    RightImage = left;
        //}
        
        ///// <summary>
        ///// Create new object with swapped image's sides.
        ///// </summary>
        ///// <returns>Swapped ViewData</returns>
        //public ViewData GetSwapedImages()
        //{
        //    return new ViewData(leftImage: RightImage, rightImage: LeftImage);
        //}

        //public ViewData RotateImages(ushort leftImageRotationTimes, ushort rightImageRotationTimes)
        //{
        //    //LeftImage = LeftImage.Rotate(leftImageRotationTimes * StraightAngle, new Bgr(0, 0, 0), true);
        //    //RightImage = RightImage.Rotate(rightImageRotationTimes * StraightAngle, new Bgr(0, 0, 0), true);

        //    return this;
        //}
    }
}
