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

        public ViewDataImage GetCurrentView()
        {
            return viewProvider.GetCurrentView();
        }

        public ViewDataBitmap GetCurrentViewAsBitmaps()
        {
            return viewProvider.GetCurrentViewAsBitmaps();
        }

        public void RotateImage(CaptureSide captureSide, RotateSide rotateSide)
        {
            viewProvider.RotateImage(captureSide, rotateSide);
        }

        public void SetCapture(CaptureSide captureSide, int cameraIndex)
        {
            viewProvider.SetCapture(captureSide, cameraIndex);
        }        
#endregion
    }
}
