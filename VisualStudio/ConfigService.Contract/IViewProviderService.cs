using System.ServiceModel;

using ViewProvision.Contract;

namespace ConfigService.Contract
{
    [ServiceContract]
    public interface IViewCalibratorService
    {
        [OperationContract]
        void RotateImage(CaptureSide captureSide, RotateSide rotateSide);
    }

    [ServiceContract]
    public interface ICaptureManagerService
    {
        [OperationContract]
        void SetCapture(CaptureSide captureSide, int cameraIndex);

        [OperationContract]
        CaptureDetails GetCaptureDetails();
    }

    [ServiceContract]
    public interface IViewProviderService : IViewCalibratorService, ICaptureManagerService
    {
        [OperationContract]
        ViewDataBitmap GetCurrentViewAsBitmaps();
    }
}
