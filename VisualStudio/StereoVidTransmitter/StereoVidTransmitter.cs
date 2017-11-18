using System;
using UnityEngine;
using ViewProvision.Contract;

namespace UnityTransmitter
{
    public class StereoVidTransmitter : IStereoVidTransmitter
    {
        private readonly IViewProvider _viewProvider;
        private readonly ITextureConverter _textureConverter;

        public StereoVidTransmitter(IViewProvider viewProvider, ITextureConverter textureConverter)
        {
            _viewProvider = viewProvider;
            _textureConverter = textureConverter;
        }

        public Texture GetLeftEyeTexture()
        {
            return _textureConverter.FromBitmap(_viewProvider.GetCurrentView().LeftImage);
        }

        public Texture GetRightEyeTexture()
        {
            return _textureConverter.FromBitmap(_viewProvider.GetCurrentView().RightImage);
        }
    }
}