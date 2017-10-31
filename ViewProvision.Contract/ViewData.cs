using System.Drawing;

namespace ViewProvision.Contract
{
    public class ViewData
    {
        const double StraightAngle = 90.0;

        public Bitmap LeftImage { get; internal set; }
        public Bitmap RightImage { get; internal set; }

        public ViewData(Bitmap leftImage, Bitmap rightImage)
        {
            this.LeftImage = leftImage;
            this.RightImage = rightImage;
        }
    }
}
