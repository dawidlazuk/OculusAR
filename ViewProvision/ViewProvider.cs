using Emgu.CV;
using Emgu.CV.Structure;
using System;

namespace ViewProvision
{
    public class ViewProvider : IViewProvider
    {
        public bool IsCalibrated { get; private set; }

        private ushort LeftImageRotationTimes { get; set; }
        private ushort RightImageRotationTimes { get; set; }
        
        Capture firstCapture = new Capture(0);
        Capture secondCapture = new Capture(1);

        public ViewData GetCurrentView()
        {
            var firstImage = firstCapture.QueryFrame()?.ToImage<Bgr, byte>();
            var secondImage = secondCapture.QueryFrame()?.ToImage<Bgr, byte>();

            var viewData = new ViewData(firstImage,secondImage);

            if (IsCalibrated == false)
                CalibrateCaptures(viewData);

            if (IsCalibrated == true)
                ApplyCalibrationParameters(viewData);

            return viewData;
        }

        private void ApplyCalibrationParameters(ViewData viewData)
        {
            viewData = viewData.RotateImages(LeftImageRotationTimes, RightImageRotationTimes);
        }

        private void CalibrateCaptures(ViewData viewData)
        {
            throw new NotImplementedException();
            //1. Detect ArUco markers on both images
            //   If markers are detected:
            //2.    Calculate rotation of each image (save it to instance field)
            //3.    After rotating the images determine which is right and left [if yes, swap captures]
            //4.    If both results are saved, set flag Calibrated on true;            
        }
    }
}
