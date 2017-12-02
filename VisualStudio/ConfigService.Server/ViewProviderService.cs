using System;
using System.ServiceModel;
using System.Diagnostics;

using ViewProvision.Contract;
using ConfigService.Contract;

namespace ConfigService.Server
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, UseSynchronizationContext = false)]
    public class ViewProviderService : IViewProviderService
    {
        private static readonly string EndpointName = "OculusAR_Config";

        private readonly IViewProvider viewProvider;

        public static ViewProviderService Create(IViewProvider provider, string port = "56719")
        {
            ServiceHost host = null;

            ViewProviderService serviceInstance = new ViewProviderService(provider);
            try
            {
                Uri uri = new Uri($"net.tcp://localhost:{port}");
                host = new ServiceHost(serviceInstance, uri);

                var binding = new NetTcpBinding();

                host.AddServiceEndpoint(
                    typeof(IViewProviderService),
                    binding,
                    uri + EndpointName);

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
