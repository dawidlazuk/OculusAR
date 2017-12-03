using Emgu.CV;
using Emgu.CV.Structure;

namespace ViewProvision
{
    public interface IImageProcessor
    {
        void Process(Image<Bgr, byte> image);
    }
}