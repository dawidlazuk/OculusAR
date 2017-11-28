using System;
using System.Collections.Generic;
using System.ServiceModel;
using ViewProvision.Contract;

namespace ConfigService.Client
{
    public class ViewProviderClient : IViewProvider
    {
        private IViewProvider channel;

        public ViewProviderClient(string serviceUrl = "net.pipe://oculusar/Config")
        {

            var channelFactory = new ChannelFactory<IViewProvider>(
                new NetNamedPipeBinding(NetNamedPipeSecurityMode.None),
                new EndpointAddress(serviceUrl));

            this.channel = channelFactory.CreateChannel();
        }

        public IEnumerable<int> GetAvailableCaptureIndexes()
        {
            return channel.GetAvailableCaptureIndexes();
        }

        public ViewData GetCurrentView()
        {
            return channel.GetCurrentView();
        }

        public ViewDataInternal GetCurrentViewInternal()
        {
            return channel.GetCurrentViewInternal();
        }

        public void RotateImage(CaptureSide captureSide, RotateSide rotateSide)
        {
            channel.RotateImage(captureSide, rotateSide);
        }

        public void SetCapture(CaptureSide captureSide, int cameraIndex)
        {
            channel.SetCapture(captureSide, cameraIndex);
        }

        public void UpdateFrames()
        {
            channel.UpdateFrames();
        }
    }
}
