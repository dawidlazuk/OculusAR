using System;
using System.Diagnostics;
using System.ServiceModel;

using ViewProvision.Contract;

using ConfigService.Contract;

namespace ConfigService
{
    public class ViewProviderService : IViewProviderService
    {
        private readonly IViewProvider viewProvider;

        public static ViewProviderService Create(IViewProvider provider, string serviceUrl)
        {
            ServiceHost host = null;

            ViewProviderService serviceInstance = new ViewProviderService(provider);
            try
            {
                var uri = new Uri(serviceUrl);
                host = new ServiceHost(serviceInstance, uri);

                var binding = new NetNamedPipeBinding(NetNamedPipeSecurityMode.None);

                host.AddServiceEndpoint(
                    typeof(IViewProvider),
                    binding,
                    uri + "Config");

                host.Open();
                return serviceInstance;
            }
            catch (Exception ex)
            {
                host = null;
                Debug.WriteLine("Error hosting config service: " + ex.Message);
                return null;
            }
        }


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
