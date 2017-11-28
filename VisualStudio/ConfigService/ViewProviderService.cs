using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.ServiceModel;
using ViewProvision.Contract;

namespace ConfigService
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple, UseSynchronizationContext = false)]
    public class ViewProviderService : IViewProvider
    {
        private IViewProvider viewProvider;

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
