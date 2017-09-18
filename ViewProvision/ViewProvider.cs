using Emgu.CV;
using Emgu.CV.Structure;

namespace ViewProvision
{
    public class ViewProvider : IViewProvider
    {
        Capture firstCapture = new Capture(0);
        Capture secondCapture = new Capture(1);

        public ViewData GetCurrentView()
        {
            var firstImage = firstCapture.QueryFrame().ToImage<Bgr, byte>();
            var secondImage = secondCapture.QueryFrame().ToImage<Bgr, byte>();

            var result = new ViewData(firstImage,secondImage);
            return result;
        }
    }
}
