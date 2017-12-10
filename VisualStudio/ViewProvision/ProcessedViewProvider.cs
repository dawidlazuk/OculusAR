using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Emgu.CV;
using Emgu.CV.Structure;
using ViewProvision.Contract;

namespace ViewProvision
{
    public class ProcessedViewProvider : IViewProvider
    {
        private readonly IViewProvider _originViewProvider;
        private readonly List<IImageProcessor> _imageProcessors;


        public ProcessedViewProvider(IViewProvider originViewProvider, List<IImageProcessor> imageProcessors = null)
        {
            _originViewProvider = originViewProvider;
            _imageProcessors = imageProcessors ?? new List<IImageProcessor>();
        }
        public void RotateImage(CaptureSide captureSide, RotateSide rotateSide)
        {
            _originViewProvider.RotateImage(captureSide, rotateSide);
        }

        public void SetCapture(CaptureSide captureSide, int cameraIndex)
        {
            _originViewProvider.SetCapture(captureSide, cameraIndex);
            var image = new Image<Bgr, byte>(new byte[100,100,100]);
        }

        public CaptureDetails GetCaptureDetails()
        {
            return _originViewProvider.GetCaptureDetails();
        }

        public ViewDataBitmap GetCurrentViewAsBitmaps()
        {
            return GetCurrentView().Bitmaps;
        }

        public ViewDataImage GetCurrentView()
        {
            var data = _originViewProvider.GetCurrentView();
            foreach (var imageProcessor in _imageProcessors)
            {
                imageProcessor.Process(data.LeftImage);
                imageProcessor.Process(data.RightImage);
            }

            return data;
        }

        public void UpdateFrames()
        {
            _originViewProvider.UpdateFrames();
        }
    }
}
