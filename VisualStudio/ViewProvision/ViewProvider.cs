using System;
using System.Collections.Generic;
using System.Threading;
using Emgu.CV;
using Emgu.CV.Structure;
using ViewProvision.Contract;


namespace ViewProvision
{
    public class ViewProvider : IViewProvider
    {
        const int ResolutionWidth = 1440;
        const int ResolutionHeight = 1080;

        /// <summary>
        /// Capture timeout in miliseconds
        /// </summary>
        private static double CaptureTimeout = 10000;

        private VideoCapture leftCapture;
        private VideoCapture rightCapture;

        private int leftCaptureIndex;
        private int rightCaptureIndex;

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

            //if(initService)
            //    InitService();
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

        private void InitLeftCaptureThread()
        {
            leftCaptureThread = new Thread(new ThreadStart(() =>
            {
                while (true)
                {
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
                    leftWaitEvent.WaitOne();
                }
            }));            
        }

        private void InitRightCaptureThread()
        {
            rightCaptureThread = new Thread(new ThreadStart(() =>
            {
                while (true)
                {
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
                    rightWaitEvent.WaitOne();
                }
            }));
        }

        private void StartCaptureThreads()
        {        
            leftCaptureThread.Start();
            rightCaptureThread.Start();
        }

        private void StartTimestampsChecking()
        {
            Thread timestampCheckThread = new Thread(new ThreadStart(() =>
            {                
                while (true)
                {
                    if (leftCaptureThread.IsAlive)
                        if ((DateTime.Now - leftImageUpdateTime).TotalMilliseconds > CaptureTimeout)
                        {
                            leftCapture = null;
                            KillAndCreateCaptureThread(CaptureSide.Left);
                        }

                    if (rightCaptureThread.IsAlive)
                        if ((DateTime.Now - rightImageUpdateTime).TotalMilliseconds > CaptureTimeout)
                        {
                            rightCapture = null;
                            KillAndCreateCaptureThread(CaptureSide.Right);
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
                    //TODO uncomment
                    SetCaptureResolution(leftCapture);
                    if (leftCaptureThread.IsAlive == false)
                        leftCaptureThread.Start();
                    break;

                case (int)CaptureSide.Right:
                    rightCapture = GetCapture(cameraIndex);
                    rightCaptureIndex = cameraIndex;
                    //TODO uncomment
                    SetCaptureResolution(rightCapture);
                    if (rightCaptureThread.IsAlive == false)
                        rightCaptureThread.Start();
                    break;
            }
        }

        private void SetCaptureResolution(VideoCapture capture)
        {
            capture.SetCaptureProperty(Emgu.CV.CvEnum.CapProp.FrameWidth, ResolutionWidth);
            capture.SetCaptureProperty(Emgu.CV.CvEnum.CapProp.FrameHeight, ResolutionHeight);
        }

        private VideoCapture GetCapture(int index)
        {
            VideoCapture capture;
            if (OpenedCaptures.TryGetValue(index, out capture) == false)
            {
                capture = new VideoCapture(index);
                OpenedCaptures.Add(index, capture);
            }           
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

        //[Obsolete]
        //private void InitService(string port = "56719")
        //{
        //    ViewProviderService.Create(
        //        new ProcessedViewProvider(
        //            this,
        //            new List<IImageProcessor>
        //            {
        //                new GrayImageProcessor()
        //            }),
        //        port);
        //}
    }
}
