using ViewProvision.Contract;

using ConfigService.Contract;

namespace ConfigService
{
    class ViewProviderService : IViewProviderService
    {
        private readonly IViewProvider viewProvider;

        public ViewProviderService(IViewProvider viewProvider)
        {
            this.viewProvider = viewProvider;
        }


        #region IViewProviderService implementation

        public ViewDataBitmap GetCurrentViewAsBitmaps()
        {
            return viewProvider.GetCurrentViewAsBitmaps();
        }

        #region IViewCalibrator implementation

        public void RotateImage(CaptureSide captureSide, RotateSide rotateSide)
        {
            viewProvider.RotateImage(captureSide, rotateSide);
        }

        #endregion

        #region ICaptureManagerService implementation

        public void SetCapture(CaptureSide captureSide, int cameraIndex)
        {
            viewProvider.SetCapture(captureSide, cameraIndex);
        }

        public CaptureDetails GetCaptureDetails()
        {
            return viewProvider.GetCaptureDetails();
        }

        #endregion

        #endregion
    }
}
