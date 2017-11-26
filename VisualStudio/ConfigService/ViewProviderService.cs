using System.Collections.Generic;
using ViewProvision.Contract;

namespace ConfigService
{
    class ViewProviderService : IViewProvider
    {
        private IViewProvider viewProvider;

        public ViewProviderService(IViewProvider viewProvider)
        {
            this.viewProvider = viewProvider;
        }

#region IViewProvider implementation

        public IEnumerable<int> GetAvailableCaptureIndexes()
        {
            return viewProvider.GetAvailableCaptureIndexes();
        }

        public ViewData GetCurrentView()
        {
            return viewProvider.GetCurrentView();
        }

        public ViewDataInternal GetCurrentViewInternal()
        {
            return viewProvider.GetCurrentViewInternal();
        }

        public void RotateImage(CaptureSide captureSide, RotateSide rotateSide)
        {
            viewProvider.RotateImage(captureSide, rotateSide);
        }

        public void SetCapture(CaptureSide captureSide, int cameraIndex)
        {
            viewProvider.SetCapture(captureSide, cameraIndex);
        }

        public void UpdateFrames()
        {
            viewProvider.UpdateFrames();
        }

#endregion
    }
}
