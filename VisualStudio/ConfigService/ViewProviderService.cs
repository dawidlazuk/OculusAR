using System.Collections.Generic;
using ViewProvision.Contract;

namespace ConfigService
{

    //TODO set contract
    class ViewProviderService
    {
        private readonly IViewProvider viewProvider;

        public ViewProviderService(IViewProvider viewProvider)
        {
            this.viewProvider = viewProvider;
        }

#region IViewProvider implementation
        
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
