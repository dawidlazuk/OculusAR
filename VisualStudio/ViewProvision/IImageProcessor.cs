using Emgu.CV;
using Emgu.CV.Structure;

namespace ViewProvision
{
    public interface IImageProcessor
    {
        void Process(ref Image<Bgr, byte> image);
    }
}