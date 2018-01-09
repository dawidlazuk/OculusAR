using System;
using System.Threading;
using UnityEngine;
using UnityEngine.Networking;
using ViewProvision.Contract;

namespace Assets.Transmitter
{
    public class StereoVidTransmitter : IStereoVidTransmitter
    {
        private readonly ITextureConverter _textureConverter;
        private readonly IViewProvider _viewProvider;

        private ViewDataImage _currentView;

        private Texture _leftTexture, _rightTexture;        

        public StereoVidTransmitter(ITextureConverter textureConverter, IViewProvider viewProvider)
        {
            _textureConverter = textureConverter;
            _viewProvider = viewProvider;            
        }

        public StereoView GetStereoView()
        {
            _viewProvider.UpdateFrames();
            _currentView = _viewProvider.GetCurrentView();
            
            return  CreateStereoView();
        }

        private StereoView CreateStereoView()
        {
            SetTextures();

            return new StereoView()
            {
                LeftEye = _leftTexture,
                RightEye = _rightTexture
            };
        }

        private void SetTextures()
        {
            if (_leftTexture == null)
                _leftTexture = _textureConverter.FromImage(_currentView.LeftImage);
            else
                _textureConverter.LoadFromImage(_currentView.LeftImage, _leftTexture);

            if (_rightTexture == null)
                _rightTexture = _textureConverter.FromImage(_currentView.RightImage);
            else
                _textureConverter.LoadFromImage(_currentView.RightImage, _rightTexture);
        }                          
    }
}