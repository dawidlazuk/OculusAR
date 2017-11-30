using System.ServiceModel;

using ViewProvision.Contract;

using ConfigService.Contract;

namespace ConfigService.Client
{
    public class ViewProviderClient : IViewProviderService
    {
        private readonly IViewProviderService channel;

        public ViewProviderClient(string serviceUrl = "net.pipe://OculusAR/Config")
        {
            var binding = new NetNamedPipeBinding(NetNamedPipeSecurityMode.None);
            binding.MaxReceivedMessageSize = (long)20e6;

            var channelFactory = new ChannelFactory<IViewProviderService>(
                binding,
                new EndpointAddress(serviceUrl));

            this.channel = channelFactory.CreateChannel();
        }
               
        
        public ViewDataBitmap GetCurrentViewAsBitmaps()
        {
            return channel.GetCurrentViewAsBitmaps();
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
            return channel.GetCaptureDetails();
        }

        public void SetCapture(CaptureSide captureSide, int cameraIndex)
        {
            channel.SetCapture(captureSide, cameraIndex);
        }        
    }
}
