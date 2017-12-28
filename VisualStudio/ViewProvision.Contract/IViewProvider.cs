using System.Collections.Generic;
using Emgu.CV;
using Emgu.CV.Structure;
using System.ServiceModel;

namespace ViewProvision.Contract
{
    public interface IViewCalibrator
    {
        void RotateImage(CaptureSide captureSide, RotateSide rotateSide);
    }

    public interface ICaptureManager
    {
        void SetCapture(CaptureSide captureSide, int cameraIndex);

        CaptureDetails GetCaptureDetails();
    }

    public interface IImageProcessing
    {
        List<string> GetAllImageProcessors();
        void ToggleImageProcessor(string name);
    }

    public interface IViewProvider : IViewCalibrator, ICaptureManager, IImageProcessing
    {      
        ViewDataBitmap GetCurrentViewAsBitmaps();
                
        //not exposed by service
        ViewDataImage GetCurrentView();
        void UpdateFrames();
    }
}
