using System;
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
        List<Tuple<string,bool>> GetAllImageProcessors();
        void SetProcessorState(string name, bool state);
        void ChangeProcessorPriority(string processorName, bool increase);
    }

    public interface IViewProvider : IViewCalibrator, ICaptureManager
    {      
        ViewDataBitmap GetCurrentViewAsBitmaps();
                
        //not exposed by service
        ViewDataImage GetCurrentView();
        void UpdateFrames();
    }

    public interface IProcessedViewProvider : IViewProvider, IImageProcessing
    {
        
    }
}
