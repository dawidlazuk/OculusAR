using Emgu.CV;
using Emgu.CV.Structure;

using ViewProvision.Contract;
using System.Collections.Generic;

namespace ViewProvision
{
    public class ViewProvider : IViewProvider
    {
        private ICapture firstCapture;
        private ICapture secondCapture;

        public ViewProvider()
        {
            OpenedCaptures = new Dictionary<int, ICapture>();
            firstCapture = GetCapture(0);
            secondCapture = GetCapture(1);
        }
        
        public ViewData GetCurrentView()
        {
            var firstImage = firstCapture.QueryFrame()?.ToImage<Bgr, byte>();
            var secondImage = secondCapture.QueryFrame()?.ToImage<Bgr, byte>();

            var viewData = new ViewDataInternal(firstImage, secondImage);

            if (IsCalibrated == false)
                CalibrateCaptures(viewData);

            //if (IsCalibrated == true)
                ApplyCalibrationParameters(viewData);

            return viewData.External;
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

        private void CalibrateCaptures(ViewDataInternal viewData)
        {
            //DetectMarkers(viewData);
            //1. Detect ArUco markers on both images
            //   If markers are detected:
            //2.    Calculate rotation of each image (save it to instance field)
            //3.    After rotating the images determine which is right and left [if yes, swap captures]
            //4.    If both results are saved, set flag Calibrated on true;            
        }

        private void ApplyCalibrationParameters(ViewDataInternal viewData)
        {
            viewData.RotateImages(LeftImageRotationTimes, RightImageRotationTimes);
        }


        //private void DetectMarkers(ViewDataInternal viewData)
        //{
        //    var cameraMatrix = new OpenCV.Net.Mat(3, 3, OpenCV.Net.Depth.F32, 1);
        //    var distortion = new OpenCV.Net.Mat(1, 4, OpenCV.Net.Depth.F32, 1);

        //    using (var detector = new MarkerDetector())
        //    {
        //        detector.ThresholdMethod = ThresholdMethod.AdaptiveThreshold;
        //        detector.Param1 = 7.0;
        //        detector.Param2 = 7.0;
        //        detector.MinSize = 0.04f;
        //        detector.MaxSize = 0.5f;
        //        detector.CornerRefinement = CornerRefinementMethod.Lines;

        //        // Detect markers in a sequence of camera images.
        //        var markerSize = 10;
        //        using (var image = OpenCV.Net.Mat.FromArray(viewData.LeftImage.Bytes))
        //        {
        //            var detectedMarkers = detector.Detect(image, cameraMatrix, distortion, markerSize);
        //            Debug.WriteLine($"Left image markers: {detectedMarkers.Count}");
        //        }
        //        using (var image = OpenCV.Net.Mat.FromArray(viewData.RightImage.Bytes))
        //        {
        //            var detectedMarkers = detector.Detect(image, cameraMatrix, distortion, markerSize);
        //            Debug.WriteLine($"Right image markers: {detectedMarkers.Count}");
        //        }
        //    }
        //}
        #endregion

        #region ICaptureManager implementation

        public const uint NumberOfCameraIndexes = 4;

        private Dictionary<int, ICapture> OpenedCaptures { get; set; }


        public void SetCapture(CaptureSide captureSide, int cameraIndex)
        {
            switch ((int)captureSide)
            {
                case (int)CaptureSide.Left:
                    firstCapture = GetCapture(cameraIndex);
                    break;

                case (int)CaptureSide.Right:
                    secondCapture = GetCapture(cameraIndex);
                    break;
            }
        }

        public IEnumerable<int> AvailableCaptureIndexes
        {
            get
            {
                var result = new SortedSet<int>();
                for (int i = 0; i < NumberOfCameraIndexes; ++i)
                    using (var capture = new VideoCapture(i))
                        if (capture.IsOpened)
                            result.Add(i);
                return result;
            }
        }

        private ICapture GetCapture(int index)
        {
            ICapture capture;
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
