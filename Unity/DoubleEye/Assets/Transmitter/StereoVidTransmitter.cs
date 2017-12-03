using ViewProvision.Contract;

namespace Assets.Transmitter
{
    public class StereoVidTransmitter : IStereoVidTransmitter
    {
        private readonly ITextureConverter _textureConverter;
        private readonly IViewProvider _viewProvider;

        public StereoVidTransmitter(ITextureConverter textureConverter, IViewProvider viewProvider)
        {
            _textureConverter = textureConverter;
            _viewProvider = viewProvider;
        }

        public StereoView GetStereoView()
        {
            _viewProvider.UpdateFrames();
            var currentView = _viewProvider.GetCurrentView();

            return new StereoView()
            {
                LeftEye = _textureConverter.FromImage(currentView.LeftImage),
                RightEye = _textureConverter.FromImage(currentView.RightImage)
            };
        }
    }
}