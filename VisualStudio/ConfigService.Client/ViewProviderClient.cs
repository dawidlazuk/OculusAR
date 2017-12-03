using System.ServiceModel;

using ViewProvision.Contract;

using ConfigService.Contract;
using System;

namespace ConfigService.Client
{
    public class ViewProviderClient : IViewProviderService
    {
        private readonly IViewProviderService channel;


        public event UnhandledExceptionEventHandler OnException;

        public ViewProviderClient(string port = "56719")
        {
            //var binding = new NetNamedPipeBinding(NetNamedPipeSecurityMode.None);
            var binding = new BasicHttpBinding();
            binding.MaxReceivedMessageSize = (long)20e6;

            var channelFactory = new ChannelFactory<IViewProviderService>(
                binding,
                new EndpointAddress($"http://localhost:{port}/OculusAR/Config"));

            try
            {
                this.channel = channelFactory.CreateChannel();
            }
            catch(Exception ex)
            {
                OnException?.Invoke(this, new UnhandledExceptionEventArgs(ex, false));
            }
        }
               
        
        public ViewDataBitmap GetCurrentViewAsBitmaps()
        {
            return Call(channel.GetCurrentViewAsBitmaps);
        }

        public ViewDataBitmap GetCurrentViewInternal()
        {
            return Call(channel.GetCurrentViewAsBitmaps);
        }

#if DEBUG
        //TODO delete, only for developement
        public void UpdateFrames()
        {
            Call(channel.UpdateFrames);
        }
#endif

        public void RotateImage(CaptureSide captureSide, RotateSide rotateSide)
        {
            Call(() => channel.RotateImage(captureSide, rotateSide));
        }

        public CaptureDetails GetCaptureDetails()
        {
            return Call(channel.GetCaptureDetails);
        }

        public void SetCapture(CaptureSide captureSide, int cameraIndex)
        {
            Call(() => channel.SetCapture(captureSide, cameraIndex));
        }        

        private T Call<T>(Func<T> function)
        {
            try
            {
                return function();
            }
            catch(Exception ex)
            {
                OnException?.Invoke(this, new UnhandledExceptionEventArgs(ex,false));
                return default(T);
            }
        }

        private void Call(Action function)
        {
            try
            {
                function();
            }
            catch (Exception ex)
            {
                OnException?.Invoke(this, new UnhandledExceptionEventArgs(ex, false));
            }
        }

    }
}
