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

        private byte[] _leftTextureData, _rightTextureData;
        private Thread[] _threads;

        private Action[] _threadActions;
        private ManualResetEvent[] _startEvents;
        private ManualResetEvent[] _finishEvents;

        public StereoVidTransmitter(ITextureConverter textureConverter, IViewProvider viewProvider)
        {
            _textureConverter = textureConverter;
            _viewProvider = viewProvider;

            InitializeThreadsAndEvents();

            StartThreads();
        }

        private void StartThreads()
        {
            foreach (var thread in _threads)
                thread.Start();
        }

        public StereoView GetStereoView()
        {
            _viewProvider.UpdateFrames();
            _currentView = _viewProvider.GetCurrentView();


            return  CreateStereoView();
            //return CreateStereoViewParallel();
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

        private StereoView CreateStereoViewParallel()
        {
            var leftTexture = new Texture2D(_currentView.LeftImage.Width, _currentView.LeftImage.Height, TextureFormat.RGB24,
                false);
            var rightTexture = new Texture2D(_currentView.RightImage.Width, _currentView.RightImage.Height, TextureFormat.RGB24,
                false);

            SetImageDataForTextures();

            leftTexture.LoadRawTextureData(_leftTextureData);
            rightTexture.LoadRawTextureData(_rightTextureData);

            leftTexture.Apply();
            rightTexture.Apply();

            return new StereoView()
            {
                LeftEye = leftTexture,
                RightEye = rightTexture
            };
        }

        private void SetImageDataForTextures()
        {
            foreach (var startEvent in _startEvents)
                startEvent.Set();

            WaitHandle.WaitAll(_finishEvents);

            foreach (var finishEvent in _finishEvents)
                finishEvent.Reset();
        }

        private void InitializeThreadsAndEvents()
        {
            _threads = new[]
            {
                new Thread(() => SetTexture(0)),
                new Thread(() => SetTexture(1))
            };

            _startEvents = new[]
            {
                new ManualResetEvent(false),
                new ManualResetEvent(false)
            };

            _finishEvents = new[]
            {
                new ManualResetEvent(false),
                new ManualResetEvent(false)
            };

            _threadActions = new Action[]
            {
                () =>  _leftTextureData = _textureConverter.DataFromImage(_currentView.LeftImage),
                () =>  _rightTextureData = _textureConverter.DataFromImage(_currentView.RightImage)
            };
        }

        private void SetTexture(int side)
        {
            while (true)
            {
                _startEvents[side].WaitOne();

                _threadActions[side].Invoke();

                _startEvents[side].Reset();
                _finishEvents[side].Set();
            }
        }

    }
}