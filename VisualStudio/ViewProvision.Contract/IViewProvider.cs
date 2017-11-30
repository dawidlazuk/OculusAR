using System;
using System.Collections.Generic;
using System.ServiceModel;

namespace ViewProvision.Contract
{
    [ServiceContract]
    public interface IViewCalibrator
    {
        //[Obsolete]
        //bool IsCalibrated { get; }
        //[Obsolete]
        //void ResetCalibration();

        [OperationContract]
        void RotateImage(CaptureSide captureSide, RotateSide rotateSide);
    }

    [ServiceContract]
    public interface ICaptureManager
    {
        [OperationContract]
        void SetCapture(CaptureSide captureSide, int cameraIndex);

        [OperationContract]
        IEnumerable<int> GetAvailableCaptureIndexes();
    }

    [ServiceContract]
    public interface IViewProvider : IViewCalibrator, ICaptureManager
    {      
	[OperationContract]  
        ViewDataBitmap GetCurrentViewAsBitmaps();

        ViewDataImage GetCurrentView();
    }
}
