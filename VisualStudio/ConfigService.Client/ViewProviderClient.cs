using System.ServiceModel;

using ViewProvision.Contract;

using ConfigService.Contract;

namespace ConfigService.Client
{
    public class ViewProviderClient : IViewProviderService
    {
        private readonly IViewProviderService channel;

        public ViewProviderClient(string port = "56719")
        {
            //var binding = new NetNamedPipeBinding(NetNamedPipeSecurityMode.None);
            var binding = new BasicHttpBinding();
            binding.MaxReceivedMessageSize = (long)20e6;

            var channelFactory = new ChannelFactory<IViewProviderService>(
                binding,
                new EndpointAddress($"http://localhost:{port}/OculusAR/Config"));

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
