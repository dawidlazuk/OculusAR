using System;
using System.Collections.Generic;
using System.Threading;

using Emgu.CV;
using Emgu.CV.Structure;

using ViewProvision.Contract;
using System.Collections.Generic;
using System.ServiceModel;
using System;
using ConfigService;
using System.ServiceModel.Description;
using System.Diagnostics;

namespace ViewProvision
{
    public class ViewProvider : IViewProvider
    {
        const int ResolutionWidth = 1920;
        const int ResolutionHeight = 1080;

        /// <summary>
        /// Capture timeout in miliseconds
        /// </summary>
        private static double CaptureTimeout = 1000;

        private VideoCapture leftCapture;
        private VideoCapture rightCapture;

        private int leftCaptureIndex;
        private int rightCaptureIndex;

        private DateTime leftImageUpdateTime;
        private DateTime rightImageUpdateTime;

        private Thread leftCaptureThread;
        private Thread rightCaptureThread;

        private ViewDataImage currentFrames;
                      
        
        public ViewProvider()
        {
            OpenedCaptures = new Dictionary<int, VideoCapture>();

            currentFrames = new ViewDataImage(null,null);
            
            InitLeftCaptureThread();
            InitRightCaptureThread();

            StartTimestampsChecking();

            SetCapture(CaptureSide.Left, 0);
            SetCapture(CaptureSide.Right, 1);
        }

        public ViewDataBitmap GetCurrentViewAsBitmaps()
        {
            return currentFrames.Bitmaps;
        }

        public ViewDataImage GetCurrentView()
        {          
            return currentFrames;
        }

        [Obsolete]
        public void UpdateFrames()
        {
            throw new InvalidOperationException("Do not use. It's obsolete method.");
            var firstImage = GetFrame(leftCapture);
            var secondImage = GetFrame(rightCapture);

            var viewData = new ViewDataImage(firstImage, secondImage);

            ApplyCalibrationParameters(viewData);

            currentFrames = viewData;        
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
                        currentFrames.LeftImage = image;
                        leftImageUpdateTime = DateTime.Now;
                    }
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
                        currentFrames.RightImage = image;
                        rightImageUpdateTime = DateTime.Now;
                    }
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

        private short LeftImageRotationTimes { get; set; } = 0;
        private short RightImageRotationTimes { get; set; } = 0;


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
                    LeftImageRotationTimes += valueToAdd;
                    break;
                case CaptureSide.Right:
                    RightImageRotationTimes += valueToAdd;
                    break;
            }
        }     

        private void ApplyCalibrationParameters(ViewDataImage viewData)
        {
            //TODO modify to operate on single images;
            viewData.RotateImages(LeftImageRotationTimes, RightImageRotationTimes);
        }
               
        #endregion

        #region ICaptureManager implementation

        public const uint NumberOfCameraIndexes = 4;

        private Dictionary<int, VideoCapture> OpenedCaptures { get; set; }


        public void SetCapture(CaptureSide captureSide, int cameraIndex)
        {
            switch ((int)captureSide)
            {
                case (int)CaptureSide.Left:
                    leftCapture = GetCapture(cameraIndex);
                    leftCaptureIndex = cameraIndex;
                    SetCaptureResolution(leftCapture);
                    if (leftCaptureThread.IsAlive == false)
                        leftCaptureThread.Start();
                    break;

                case (int)CaptureSide.Right:
                    rightCapture = GetCapture(cameraIndex);
                    rightCaptureIndex = cameraIndex;
                    SetCaptureResolution(rightCapture);
                    if (rightCaptureThread.IsAlive == false)
                        rightCaptureThread.Start();
                    break;
            }
        }

        private void SetCaptureResolution(VideoCapture capture)
        {
            capture.SetCaptureProperty(Emgu.CV.CvEnum.CapProp.FrameHeight, ResolutionWidth);
            capture.SetCaptureProperty(Emgu.CV.CvEnum.CapProp.FrameWidth, ResolutionHeight);
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
                LeftIndex = leftCaptureIndex,
                RightIndex = rightCaptureIndex
            };
        }

        #endregion

        private void InitService(string serviceUrl = "net.pipe://OculusAR")
        {
            ViewProviderService.Create(this, serviceUrl);
        }
    }
}
