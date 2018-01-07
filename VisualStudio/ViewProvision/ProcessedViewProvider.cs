using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Emgu.CV;
using Emgu.CV.Structure;
using ViewProvision.Contract;
using System.Threading;
using System.Diagnostics;

namespace ViewProvision
{
    public class ProcessedViewProvider : IProcessedViewProvider
    {
        private readonly IViewProvider _originViewProvider;
        private readonly List<IImageProcessor> _imageProcessors;

        private AutoResetEvent startLeftProcessingEvent = new AutoResetEvent(false);
        private AutoResetEvent startRightProcessingEvent = new AutoResetEvent(false);
        private AutoResetEvent finishLeftProcessingEvent = new AutoResetEvent(false);
        private AutoResetEvent finishRightProcessingEvent = new AutoResetEvent(false);

        private ViewDataImage currentFrames = new ViewDataImage(null, null);

        private Thread leftProcessingThread;
        private Thread rightProcessingThread;

        private object leftImageMutex = new object();
        private object rightImageMutex = new object();

        private DateTime leftImageUpdateTime;
        private DateTime rightImageUpdateTime;
        
        public ProcessedViewProvider(IViewProvider originViewProvider, List<IImageProcessor> imageProcessors = null)
        {
            _originViewProvider = originViewProvider;
            _imageProcessors = imageProcessors ?? new List<IImageProcessor>();
            
            InitLeftProcessingThread();
            InitRightProcessingThread();

            StartTimestampsChecking();
        }

        #region IProcessedViewProvider implementation

        public void RotateImage(CaptureSide captureSide, RotateSide rotateSide)
        {
            _originViewProvider.RotateImage(captureSide, rotateSide);
        }

        public void SetCapture(CaptureSide captureSide, int cameraIndex)
        {
            _originViewProvider.SetCapture(captureSide, cameraIndex);
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
            if (_imageProcessors.Any() == false)
                return _originViewProvider.GetCurrentView();               

            startLeftProcessingEvent.Set();
            startRightProcessingEvent.Set();

            finishLeftProcessingEvent.WaitOne();
            finishRightProcessingEvent.WaitOne();

            return currentFrames;
        }


        public void UpdateFrames()
        {
            _originViewProvider.UpdateFrames();
        }

        public void ChangeProcessorPriority(string processorName, bool increase)
        {
            var imageProcessor = _imageProcessors.Single(x => x.Name == processorName);
            var index = _imageProcessors.IndexOf(imageProcessor);

            if (increase)
            {
                if (index == 0) return;
                _imageProcessors.Remove(imageProcessor);

                --index;
                _imageProcessors.Insert(index, imageProcessor);
                return;
            }

            if (index == _imageProcessors.Count - 1) return;
            _imageProcessors.Remove(imageProcessor);

            ++index;
            _imageProcessors.Insert(index, imageProcessor);
        }

        public List<Tuple<string, bool>> GetAllImageProcessors()
        {
            return _imageProcessors.Select(x => new Tuple<string, bool>(x.Name, x.Active)).ToList();
        }

        public void SetProcessorState(string name, bool state)
        {
            var imageProcessor = _imageProcessors.Single(x => x.Name == name);
            imageProcessor.Active = state;
        }

#endregion

        private void InitLeftProcessingThread()
        {
            leftProcessingThread = new Thread(new ThreadStart(() =>
            {
                while (true)
                {
                    startLeftProcessingEvent.WaitOne();
                    try
                    {
                        var image = (_originViewProvider as ViewProvider).GetLeftFrameSynchronously();
                        leftImageUpdateTime = DateTime.Now;
                        if (image != null)
                            foreach (var imageProcessor in _imageProcessors)
                                if (imageProcessor.Active)
                                    imageProcessor.Process(ref image);
                        currentFrames.LeftImage = image;
                    }
                    catch(Exception ex)
                    {
                        Debug.WriteLine("Left image processing" + ex.Message);
                    }
                    finally
                    {
                        finishLeftProcessingEvent.Set();
                    }
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
                    try
                    {
                        var image = (_originViewProvider as ViewProvider).GetRightFrameSynchronously();
                        rightImageUpdateTime = DateTime.Now;
                        if (image != null)
                            foreach (var imageProcessor in _imageProcessors)
                                if(imageProcessor.Active)
                                imageProcessor.Process(ref image);
                        currentFrames.RightImage = image;
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine("Right image processing" + ex.Message);
                    }
                    finally
                    {
                        finishRightProcessingEvent.Set();
                    }
                }
            }));
            rightProcessingThread.Start();
        }


        private void StartTimestampsChecking()
        {
            Thread timestampCheckThread = new Thread(new ThreadStart(() =>
            {
                while (true)
                {
                    if (leftProcessingThread.IsAlive)
                        if ((DateTime.Now - leftImageUpdateTime).TotalMilliseconds > ViewProvider.CaptureTimeout)
                        {
                            KillAndCreateProcessingThread(CaptureSide.Left);
                        }

                    if (leftProcessingThread.IsAlive)
                        if ((DateTime.Now - rightImageUpdateTime).TotalMilliseconds > ViewProvider.CaptureTimeout)
                        {
                            KillAndCreateProcessingThread(CaptureSide.Right);
                        }
                    Thread.Sleep((int)ViewProvider.CaptureTimeout);
                }
            }));

            timestampCheckThread.Start();
        }

        private void KillAndCreateProcessingThread(CaptureSide side)
        {
            switch (side)
            {
                case CaptureSide.Left:
                    leftProcessingThread.Abort();
                    InitLeftProcessingThread();
                    break;

                case CaptureSide.Right:
                    rightProcessingThread.Abort();
                    InitRightProcessingThread();
                    break;
            }
        }

      
    }
}
