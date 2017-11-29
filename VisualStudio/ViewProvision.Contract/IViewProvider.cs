using System;
using System.Collections.Generic;

namespace ViewProvision.Contract
{
    public interface IViewCalibrator
    {
        [Obsolete]
        bool IsCalibrated { get; }
        [Obsolete]
        void ResetCalibration();

        void RotateImage(CaptureSide captureSide, RotateSide rotateSide);
    }

    public interface ICaptureManager
    {
        void SetCapture(CaptureSide captureSide, int cameraIndex);
        IEnumerable<int> AvailableCaptureIndexes { get; }
    }

    public interface IViewProvider : IViewCalibrator, ICaptureManager
    {        
        //TODO remove with ViewDataInternal version
        ViewDataBitmap GetCurrentViewAsBitmaps();
        ViewDataImage GetCurrentView();

        void UpdateFrames();
    }
}
