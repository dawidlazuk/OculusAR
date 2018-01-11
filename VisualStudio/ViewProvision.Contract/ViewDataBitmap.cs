using System.Drawing;
using System.Runtime.Serialization;

namespace ViewProvision.Contract
{

    /// <summary>
    /// Frames class with use of the System.Drawing.Bitmap as image type
    /// </summary>
    [DataContract]
    public class ViewDataBitmap
    {
        const double StraightAngle = 90.0;
        
        [DataMember]
        private BitmapDTO leftImage;
        [DataMember]
        private BitmapDTO rightImage;

        public Bitmap LeftImage
        {
            get { return leftImage.Bitmap; }
            internal set { leftImage = new BitmapDTO(value); }
        }
        
        public Bitmap RightImage
        {
            get { return rightImage.Bitmap; }
            internal set { rightImage = new BitmapDTO(value); }
        }

        public ViewDataBitmap(Bitmap leftImage, Bitmap rightImage)
        {
            this.LeftImage = leftImage;
            this.RightImage = rightImage;
        }    
    }
}
