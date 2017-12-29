using Emgu.CV;
using Emgu.CV.Structure;

namespace ViewProvision
{
    public interface IImageProcessor
    {
        void Process(ref Image<Bgr, byte> image);
        string Name { get; }
        bool Active { get; set; }
    }
}