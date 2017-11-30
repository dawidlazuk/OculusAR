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

    public interface IViewProvider : IViewCalibrator, ICaptureManager
    {      
        ViewDataBitmap GetCurrentViewAsBitmaps();

        //not exposed by service
        ViewDataImage GetCurrentView();
    }
}
