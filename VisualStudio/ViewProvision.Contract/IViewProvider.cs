using System;
using System.Collections.Generic;
using Emgu.CV;
using Emgu.CV.Structure;
using System.ServiceModel;

namespace ViewProvision.Contract
{
    /// <summary>
    /// Responsible for the functionality of cameras calibration.
    /// </summary>
    public interface IViewCalibrator
    {
        /// <summary>
        /// Change rotation of the image for captureSide by 90 degrees into rotateSide.
        /// </summary>
        /// <param name="captureSide">Eye side which receives the image</param>
        /// <param name="rotateSide">Rotation side</param>
        void RotateImage(CaptureSide captureSide, RotateSide rotateSide);
    }

    /// <summary>
    /// Responsible for maintanance captures for each channels.
    /// </summary>
    public interface ICaptureManager
    {
        /// <summary>
        /// Change camera used for image aquisition for the captureSide channel.
        /// </summary>
        /// <param name="captureSide">Eye side which receives the image</param>
        /// <param name="cameraIndex">Index of the camera to set</param>
        void SetCapture(CaptureSide captureSide, int cameraIndex);

        /// <summary>
        /// Get details of the captured frames.
        /// </summary>
        /// <returns>Details of the captured frames.</returns>
        CaptureDetails GetCaptureDetails();
    }

    /// <summary>
    /// Responsible for functionality of image processing.
    /// </summary>
    public interface IImageProcessing
    {
        /// <summary>
        /// Get all available image processing alghoritms in the application.
        /// </summary>
        /// <returns>List of pairs {name of the alghoritm, is alghoritm enabled}</returns>
        List<Tuple<string,bool>> GetAllImageProcessors();

        /// <summary>
        /// Enable/disable the specified image processor.
        /// </summary>
        /// <param name="name">Image processor name</param>
        /// <param name="state">Demanded state</param>
        void SetProcessorState(string name, bool state);

        /// <summary>
        /// Change order of the image processors.
        /// </summary>
        /// <param name="processorName">Image processor name</param>
        /// <param name="increase">Value determining if the priority should be increased or decreased</param>
        void ChangeProcessorPriority(string processorName, bool increase);
    }

    /// <summary>
    /// Interface responsible for functionality of retrieving frames from the camera.
    /// </summary>
    public interface IViewProvider : IViewCalibrator, ICaptureManager
    {      
        /// <summary>
        /// Returns the last captured frames as System.Drawing.Bitmap objects.
        /// </summary>
        /// <returns>Frames as Bitmap objects.</returns>
        ViewDataBitmap GetCurrentViewAsBitmaps();

        //not exposed by service
        /// <summary>
        /// Returns the last captured frames as EmguCV Image objects.
        /// </summary>
        /// <returns>Frames as EmguCV image objects.</returns>
        ViewDataImage GetCurrentView();

        /// <summary>
        /// Refresh the last captured frames.
        /// </summary>
        void UpdateFrames();

        ///// <summary>
        ///// Use per each frames retrieving to prevent disposing capture objects in case of not using multi-threading capture frame way.
        ///// </summary>
        //void UpdateTimestamp();
    }

    /// <summary>
    /// Extends IViewProvider interface with functionality of image processing.
    /// </summary>
    public interface IProcessedViewProvider : IViewProvider, IImageProcessing
    {        
    }
}
