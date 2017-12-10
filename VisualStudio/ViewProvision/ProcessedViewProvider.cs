using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Emgu.CV;
using Emgu.CV.Structure;
using ViewProvision.Contract;
using System.Threading;

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
            
            InitLeftProcessingThread();
            InitRightProcessingThread();
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

        public void InvokeWithImagesLocked(Action action)
        {
            lock (leftImageMutex)
            {
                lock (rightImageMutex)
                {
                    action?.Invoke();
                }
            }
        }

        public ViewDataImage GetCurrentView()
        {
            InvokeWithImagesLocked(() => { currentFrames = _originViewProvider.GetCurrentView(); });

            startLeftProcessingEvent.Set();
            startRightProcessingEvent.Set();

            finishLeftProcessingEvent.WaitOne();
            finishRightProcessingEvent.WaitOne();

            return currentFrames;

            //var data = _originViewProvider.GetCurrentView();
            //foreach (var imageProcessor in _imageProcessors)
            //{
            //    imageProcessor.Process(data.LeftImage);
            //    imageProcessor.Process(data.RightImage);
            //}

            //return data;
        }

        private AutoResetEvent startLeftProcessingEvent = new AutoResetEvent(false);
        private AutoResetEvent startRightProcessingEvent = new AutoResetEvent(false);
        private AutoResetEvent finishLeftProcessingEvent = new AutoResetEvent(false);
        private AutoResetEvent finishRightProcessingEvent = new AutoResetEvent(false);

        private ViewDataImage currentFrames;
        
        private Thread leftProcessingThread;
        private Thread rightProcessingThread;

        private object leftImageMutex = new object();
        private object rightImageMutex = new object();

        private void InitLeftProcessingThread()
        {
            leftProcessingThread = new Thread(new ThreadStart(() =>
            {
                while (true)
                {
                    startLeftProcessingEvent.WaitOne();
                    lock (leftImageMutex)
                    {
                        var image = currentFrames.LeftImage;
                        foreach (var imageProcessor in _imageProcessors)
                        {
                            imageProcessor.Process(ref image);
                        }
                        currentFrames.LeftImage = image;
                    }

                    finishLeftProcessingEvent.Set();
                }
            }));
            leftProcessingThread.Start();
        }

        private void InitRightProcessingThread()
        {
            rightProcessingThread = new Thread(new ThreadStart(() =>
            {
                while (true)
                {
                    startRightProcessingEvent.WaitOne();

                    lock (rightImageMutex)
                    {
                        var image = currentFrames.RightImage;
                        foreach (var imageProcessor in _imageProcessors)
                        {
                            imageProcessor.Process(ref image);
                        }
                        currentFrames.RightImage = image;
                    }

                    finishRightProcessingEvent.Set();
                }
            }));
            rightProcessingThread.Start();
        }

        public void UpdateFrames()
        {
            _originViewProvider.UpdateFrames();
        }
    }
}
