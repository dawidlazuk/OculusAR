﻿using System.Collections.Generic;
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
    public interface IImageProcessingService
    {
        [OperationContract]
        List<string> GetAllImageProcessors();

        [OperationContract]
        void ToggleImageProcessor(string processorName);
    }

    [ServiceContract]
    public interface IViewProviderService : IViewCalibratorService, ICaptureManagerService, IImageProcessingService
    {
        [OperationContract]
        ViewDataBitmap GetCurrentViewAsBitmaps();


#if DEBUG
        //TODO remove this region - it's only for development & testing
        #region TODO Delete - only for development & testing!
        [OperationContract]
        void UpdateFrames();
        #endregion
#endif
    }
}
