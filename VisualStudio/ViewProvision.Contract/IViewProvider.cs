using System;
using System.Collections.Generic;
using System.ServiceModel;

namespace ViewProvision.Contract
{
    [ServiceContract]
    public interface IViewCalibrator
    {
        [OperationContract]
        void RotateImage(CaptureSide captureSide, RotateSide rotateSide);
    }

    [ServiceContract]
    public interface ICaptureManager
    {
        [OperationContract]
        void SetCapture(CaptureSide captureSide, int cameraIndex);

        [OperationContract]
        CaptureDetails GetCaptureDetails();
    }

    [ServiceContract]
    public interface IViewProvider : IViewCalibrator, ICaptureManager
    {      
	    [OperationContract]  
        ViewDataBitmap GetCurrentViewAsBitmaps();

        ViewDataImage GetCurrentView();
    }
}
