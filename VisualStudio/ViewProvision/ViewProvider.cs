using Emgu.CV;
using Emgu.CV.Structure;

using ViewProvision.Contract;
using System.Collections.Generic;
using System;
using System.Threading;
using System.Runtime.Remoting.Contexts;

namespace ViewProvision
{
    public class ViewProvider : IViewProvider
    {
        /// <summary>
        /// Capture timeout in miliseconds
        /// </summary>
        private static double CaptureTimeout = 1000;

        private VideoCapture leftCapture;
        private VideoCapture rightCapture;

        private ViewDataImage currentFrames;

        private DateTime leftImageUpdateTime;
        private DateTime rightImageUpdateTime;

        private int firstCaptureIndex;

        private Thread leftCaptureThread;
        private Thread rightCaptureThread;
        
        public ViewProvider()
        {
            OpenedCaptures = new Dictionary<int, VideoCapture>();
            //leftCapture = GetCapture(0);
            //rightCapture = GetCapture(1);

            currentFrames = new ViewDataImage(null,null);
            
            InitLeftCaptureThread();
            InitRightCaptureThread();

            //StartCaptureThreads();            
            StartTimestampsChecking();
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

            if (IsCalibrated == false)
                CalibrateCaptures(viewData);

            //if (IsCalibrated == true)
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
        public bool IsCalibrated { get; private set; }

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
        public void ResetCalibration()
        {
            IsCalibrated = false;
        }
     

        private void ApplyCalibrationParameters(ViewDataInternal viewData)
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
                    //leftCapture?.Dispose();
                    leftCapture = GetCapture(cameraIndex);
                    SetCaptureFullHd(leftCapture);
                    if (leftCaptureThread.IsAlive == false)
                        leftCaptureThread.Start();
                    break;

                case (int)CaptureSide.Right:
                    //rightCapture?.Dispose();
                    rightCapture = GetCapture(cameraIndex);
                    SetCaptureFullHd(rightCapture);
                    if (rightCaptureThread.IsAlive == false)
                        rightCaptureThread.Start();
                    break;
            }
        }

        private void SetCaptureFullHd(VideoCapture capture)
        {
            capture.SetCaptureProperty(Emgu.CV.CvEnum.CapProp.FrameHeight, 1080);
            capture.SetCaptureProperty(Emgu.CV.CvEnum.CapProp.FrameWidth, 1920);
        }

        public IEnumerable<int> AvailableCaptureIndexes
        {
            var result = new List<int>();
            for (int i = 0; i < NumberOfCameraIndexes; ++i)
                using (var capture = new VideoCapture(i))
                    if (capture.IsOpened)
                        yield return i;

            yield break;
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
        #endregion
    }
}
