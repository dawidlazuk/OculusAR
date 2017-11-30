using System.ServiceModel;

using ViewProvision.Contract;

using ConfigService.Contract;

namespace ConfigService.Client
{
    public class ViewProviderClient : IViewProviderService
    {
        private IViewProviderService channel;

        public ViewProviderClient(string serviceUrl = "net.pipe://oculusar/Config")
        {

            var channelFactory = new ChannelFactory<IViewProviderService>(
                new NetNamedPipeBinding(NetNamedPipeSecurityMode.None),
                new EndpointAddress(serviceUrl));

            this.channel = channelFactory.CreateChannel();
        }
               
        
        public ViewDataBitmap GetCurrentViewAsBitmaps()
        {
            throw new System.NotImplementedException();
        }

        public ViewDataBitmap GetCurrentViewInternal()
        {
            return channel.GetCurrentViewAsBitmaps();
        }

        public void RotateImage(CaptureSide captureSide, RotateSide rotateSide)
        {
            channel.RotateImage(captureSide, rotateSide);
        }

        public CaptureDetails GetCaptureDetails()
        {
            throw new System.NotImplementedException();
        }

        public void SetCapture(CaptureSide captureSide, int cameraIndex)
        {
            channel.SetCapture(captureSide, cameraIndex);
        }        
    }
}
