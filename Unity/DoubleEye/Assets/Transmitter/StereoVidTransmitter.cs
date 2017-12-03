using System;
using System.Threading;
using UnityEngine;
using ViewProvision.Contract;

namespace Assets.Transmitter
{
    public class StereoVidTransmitter : IStereoVidTransmitter
    {
        private readonly ITextureConverter _textureConverter;
        private readonly IViewProvider _viewProvider;

        private ViewDataImage _currentView;

        private Texture _leftTexture, _rightTexture;
        private Thread[] _threads;

        private Action[] _threadActions;
        private ManualResetEvent[] _startEvents;
        private ManualResetEvent[] _finishEvents;

        public StereoVidTransmitter(ITextureConverter textureConverter, IViewProvider viewProvider)
        {
            _textureConverter = textureConverter;
            _viewProvider = viewProvider;

            InitializeThreadsAndEvents();
        }

        public StereoView GetStereoView()
        {
            _currentView = _viewProvider.GetCurrentView();

            ConvertImageToTexturesInSeparateThreads();

            return new StereoView()
            {
                LeftEye = _leftTexture,
                RightEye = _rightTexture
            };
        }

        private void ConvertImageToTexturesInSeparateThreads()
        {
            foreach (var startEvent in _startEvents)
                startEvent.Set();

            WaitHandle.WaitAll(_finishEvents);

            foreach (var finishEvent in _finishEvents)
                finishEvent.Reset();
        }

        private void InitializeThreadsAndEvents()
        {
            _threads[0] = new Thread(() => SetTexture(0));
            _threads[1] = new Thread(() => SetTexture(1));

            _startEvents = new ManualResetEvent[2];
            _finishEvents = new ManualResetEvent[2];

            _threadActions = new Action[]
            {
                () => _leftTexture = _textureConverter.FromImage(_currentView.LeftImage),
                () => _rightTexture = _textureConverter.FromImage(_currentView.RightImage)
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