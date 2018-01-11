using System;
using System.Collections.Generic;
using System.Threading;
using Emgu.CV;
using Emgu.CV.Structure;
using ViewProvision.Contract;
using System.Diagnostics;
using System.Runtime.ExceptionServices;

namespace ViewProvision
{
    public class ViewProvider : IViewProvider
    {
        const int ResolutionWidth = 1440;
        const int ResolutionHeight = 1080;

        /// <summary>
        /// Capture timeout in miliseconds
        /// </summary>
        internal static double CaptureTimeout = 10000;

        private VideoCapture leftCapture;
        private VideoCapture rightCapture;

        private int leftCaptureIndex = -1;
        private int rightCaptureIndex = -1;

        private DateTime leftImageUpdateTime;
        private DateTime rightImageUpdateTime;

        private Thread leftCaptureThread;
        private Thread rightCaptureThread;

        private AutoResetEvent leftWaitEvent;
        private AutoResetEvent rightWaitEvent;

        private ViewDataImage currentFrames;

        private object leftCaptureMutex = new object();
        private object rightCaptureMutex = new object();
                      
        
        public ViewProvider(bool initService = false)
        {
            OpenedCaptures = new Dictionary<int, VideoCapture>();

            currentFrames = new ViewDataImage(null,null);

            leftWaitEvent = new AutoResetEvent(true);
            rightWaitEvent = new AutoResetEvent(true);
            
            InitLeftCaptureThread();
            InitRightCaptureThread();

            StartTimestampsChecking();

            SetCapture(CaptureSide.Left, 0);
            SetCapture(CaptureSide.Right, 1);
        }

        public ViewDataBitmap GetCurrentViewAsBitmaps()
        {
            lock (leftCaptureMutex)
            {
                lock (rightCaptureMutex)
                {
                    return currentFrames.Bitmaps;
                }
            }
        }

        public ViewDataImage GetCurrentView()
        {
            lock (leftCaptureMutex)
            {
                lock (rightCaptureMutex)
                {
                    return currentFrames;
                }
            }
        }


        public Image<Bgr, byte> GetLeftFrameSynchronously()
        {
            var image = GetFrame(leftCapture);
            image = image.RotateImage(leftImageRotationTimes);
            return image;
        }

        public Image<Bgr, byte> GetRightFrameSynchronously()
        {
            var image = GetFrame(rightCapture);
            image = image.RotateImage(rightImageRotationTimes);
            return image;
        }

        public void UpdateFrames()
        {
            leftWaitEvent.Set();
            rightWaitEvent.Set();
        }

        public void UpdateTimestamp()
        {
            leftImageUpdateTime = DateTime.Now;
            rightImageUpdateTime = DateTime.Now;
        }

        private void InitLeftCaptureThread()
        {
            leftCaptureThread = new Thread(new ThreadStart(() =>
            {
                while (true)
                {
                    leftWaitEvent.WaitOne();
                    var image = GetFrame(leftCapture);
                    if (image != null)
                    {
                        image = image.RotateImage(leftImageRotationTimes);
                        lock (leftCaptureMutex)
                        {
                            currentFrames.LeftImage?.Dispose();
                            currentFrames.LeftImage = image;
                        }
                        leftImageUpdateTime = DateTime.Now;
                    }
                }
            }));
            leftCaptureThread.Start();
        }

        private void InitRightCaptureThread()
        {
            rightCaptureThread = new Thread(new ThreadStart(() =>
            {
                while (true)
                {
                    rightWaitEvent.WaitOne();
                    var image = GetFrame(rightCapture);
                    if (image != null)
                    {
                        image = image.RotateImage(rightImageRotationTimes);
                        lock (rightCaptureMutex)
                        {
                            currentFrames.RightImage?.Dispose();
                            currentFrames.RightImage = image;
                        }
                        rightImageUpdateTime = DateTime.Now;
                    }
                }
            }));
            rightCaptureThread.Start();
        }

        private void StartCaptureThreads()
        {        
            leftCaptureThread.Start();
            rightCaptureThread.Start();
        }

        private void StartTimestampsChecking()
        {
            leftImageUpdateTime = DateTime.Now;
            rightImageUpdateTime = DateTime.Now;

            Thread timestampCheckThread = new Thread(new ThreadStart(() =>
            {                
                while (true)
                {
                    if (leftCaptureThread.IsAlive)
                        if ((DateTime.Now - leftImageUpdateTime).TotalMilliseconds > CaptureTimeout)
                        {                        
                            KillAndCreateCaptureThread(CaptureSide.Left);
                            leftCapture = null;
                            if (leftCaptureIndex != -1)
                                SetCapture(CaptureSide.Left, leftCaptureIndex);
                        }

                    if (rightCaptureThread.IsAlive)
                        if ((DateTime.Now - rightImageUpdateTime).TotalMilliseconds > CaptureTimeout)
                        {
                            KillAndCreateCaptureThread(CaptureSide.Right);
                            rightCapture = null;
                            if (rightCaptureIndex != -1)
                                SetCapture(CaptureSide.Right, rightCaptureIndex);
                        }
                    Thread.Sleep((int)CaptureTimeout);
                }
            }));

            timestampCheckThread.Start();
        }

        private void KillAndCreateCaptureThread(CaptureSide side)
        {
            switch (side)
            {
                case CaptureSide.Left:
                    leftCaptureThread.Abort();
                    InitLeftCaptureThread();
                    break;

                case CaptureSide.Right:
                    rightCaptureThread.Abort();
                    InitRightCaptureThread();
                    break;
            }
        }

        [HandleProcessCorruptedStateExceptions]
        private Image<Bgr, byte> GetFrame(ICapture capture)
        {
            try
            {
                return capture.QueryFrame()?.ToImage<Bgr, byte>();
            }         
            catch
            {
                return null;
            }
        }

        #region IViewCalibrator implementation

        private short leftImageRotationTimes;
        private short rightImageRotationTimes;

        public void RotateImage(CaptureSide captureSide, RotateSide rotateSide)
        {
            short valueToAdd = 0;
            switch (rotateSide)
            {
                case RotateSide.Left:
                    valueToAdd = -1;
                    break;
                case RotateSide.Right:
                    valueToAdd = 1;
                    break;
            }
            switch (captureSide)
            {
                case CaptureSide.Left:
                    leftImageRotationTimes += valueToAdd;
                    break;
                case CaptureSide.Right:
                    rightImageRotationTimes += valueToAdd;
                    break;
            }
        }     
                       
        #endregion

        #region ICaptureManager implementation

        private Dictionary<int, VideoCapture> OpenedCaptures { get; set; }

        public void SetCapture(CaptureSide captureSide, int cameraIndex)
        {
            switch ((int)captureSide)
            {
                case (int)CaptureSide.Left:
                    leftCapture = GetCapture(cameraIndex);
                    leftCaptureIndex = cameraIndex;
                    SetCaptureResolution(leftCapture);
                    break;

                case (int)CaptureSide.Right:
                    rightCapture = GetCapture(cameraIndex);
                    rightCaptureIndex = cameraIndex;
                    SetCaptureResolution(rightCapture);
                    break;
            }
        }

        [HandleProcessCorruptedStateExceptions]
        private void SetCaptureResolution(VideoCapture capture)
        {
            try
            {
                capture.SetCaptureProperty(Emgu.CV.CvEnum.CapProp.FrameWidth, ResolutionWidth);
                capture.SetCaptureProperty(Emgu.CV.CvEnum.CapProp.FrameHeight, ResolutionHeight);
            }
            catch(AccessViolationException ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        private VideoCapture GetCapture(int index)
        {
            VideoCapture capture;
            if (OpenedCaptures.TryGetValue(index, out capture) == true)
            {
                if (leftCapture == capture || rightCapture == capture)
                    return capture;

                OpenedCaptures.Remove(index);
                capture.Dispose();
            }
            capture = new VideoCapture(index);
            OpenedCaptures.Add(index,capture);
            return capture;            
        }

        public CaptureDetails GetCaptureDetails()
        {
            return new CaptureDetails
            {
                LeftChannel = new ChannelDetails
                {
                    CaptureIndex = leftCaptureIndex,
                    RotationAngle = 90 * (leftImageRotationTimes % 4),             
                    FrameWidth = leftCapture.Width,
                    FrameHeight = leftCapture.Height                    
                },
                RightChannel = new ChannelDetails
                {
                    CaptureIndex = rightCaptureIndex,
                    RotationAngle = 90 * (rightImageRotationTimes % 4),
                    FrameWidth = rightCapture.Width,
                    FrameHeight = rightCapture.Height
                }
            };
        }
        #endregion    
    }
}
