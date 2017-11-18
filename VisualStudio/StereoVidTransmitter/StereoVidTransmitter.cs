using System;
using UnityEngine;

namespace UnityTransmitter
{
    public class StereoVidTransmitter : IStereoVidTransmitter
    {
        private readonly IEyeBitmapSource _eyeBitmapSource;
        private readonly ITextureConverter _textureConverter;

        public StereoVidTransmitter(IEyeBitmapSource eyeBitmapSource, ITextureConverter textureConverter)
        {
            _eyeBitmapSource = eyeBitmapSource;
            _textureConverter = textureConverter;
        }

        public Texture GetLeftEyeTexture()
        {
            return _textureConverter.FromBitmap(_eyeBitmapSource.GetLeftEyeBitmap());
        }

        public Texture GetRightEyeTexture()
        {
            return _textureConverter.FromBitmap(_eyeBitmapSource.GetRightEyeBitmap());
        }
    }
}