using Emgu.CV;
using Emgu.CV.Structure;

namespace ViewProvision
{
    static class ImageExtension
    {
        const double StraightAngle = 90.0;

        public static Image<TColor, TDepth> RotateImage<TColor, TDepth>(this Image<TColor, TDepth> image, short rotationTimes)
            where TColor : struct, IColor
            where TDepth : new()
        {
            if (rotationTimes % 4 == 0)
                return image;
            return image.Rotate(GetRotatonAngle(rotationTimes), default(TColor), true);
        }

        private static double GetRotatonAngle(short rotationTimes)
        {
            return (rotationTimes % 4) * StraightAngle;
        }
    }
}
