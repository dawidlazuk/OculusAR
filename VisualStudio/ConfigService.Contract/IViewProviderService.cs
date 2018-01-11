using System;
using System.Collections.Generic;
using System.ServiceModel;

using ViewProvision.Contract;

namespace ConfigService.Contract
{
    /// <summary>
    /// Responsible for the functionality of cameras calibration with use of the WCF service.
    /// </summary>
    [ServiceContract]
    public interface IViewCalibratorService
    {

        /// <summary>
        /// Change rotation of the image for captureSide by 90 degrees into rotateSide.
        /// </summary>
        /// <param name="captureSide">Eye side which receives the image</param>
        /// <param name="rotateSide">Rotation side</param>
        [OperationContract]
        void RotateImage(CaptureSide captureSide, RotateSide rotateSide);
    }       

    /// <summary>
    /// Responsible for maintanance captures for each channels.
    /// </summary>
    [ServiceContract]
    public interface ICaptureManagerService
    {

        /// <summary>
        /// Change camera used for image aquisition for the captureSide channel.
        /// </summary>
        /// <param name="captureSide">Eye side which receives the image</param>
        /// <param name="cameraIndex">Index of the camera to set</param>
        [OperationContract]
        void SetCapture(CaptureSide captureSide, int cameraIndex);
        
        /// <summary>
        /// Get details of the captured frames.
        /// </summary>
        /// <returns>Details of the captured frames.</returns>
        [OperationContract]
        CaptureDetails GetCaptureDetails();
    }


    /// <summary>
    /// Responsible for functionality of image processing.
    /// </summary>
    [ServiceContract]
    public interface IImageProcessingService
    {
        /// <summary>
        /// Get all available image processing alghoritms in the application.
        /// </summary>
        /// <returns>List of pairs {name of the alghoritm, is alghoritm enabled}</returns>
        [OperationContract]
        List<Tuple<string, bool>> GetAllImageProcessors();

        /// <summary>
        /// Enable/disable the specified image processor.
        /// </summary>
        /// <param name="name">Image processor name</param>
        /// <param name="state">Demanded state</param>
        [OperationContract]
        void SetProcessorState(string processorName, bool state);

        /// <summary>
        /// Change order of the image processors.
        /// </summary>
        /// <param name="processorName">Image processor name</param>
        /// <param name="increase">Value determining if the priority should be increased or decreased</param>
        [OperationContract]
        void ChangeProcessorPriority(string selectedProcessorName, bool increase);
    }

    /// <summary>
    /// Interface responsible for functionality of retrieving frames from the camera and processing them.
    /// </summary>
    [ServiceContract]
    public interface IViewProviderService : IViewCalibratorService, ICaptureManagerService, IImageProcessingService
    {
        /// <summary>
        /// Returns the last captured frames as System.Drawing.Bitmap objects.
        /// </summary>
        /// <returns>Frames as Bitmap objects.</returns>
        [OperationContract]
        ViewDataBitmap GetCurrentViewAsBitmaps();


#if DEBUG
        //TODO remove this region - it's only for development & testing
        #region TODO Delete - only for development & testing!
        /// <summary>
        /// Refresh the last captured frames.
        /// </summary>
        [OperationContract]
        void UpdateFrames();
        #endregion
#endif
    }
}
