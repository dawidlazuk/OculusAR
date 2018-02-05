using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using IoCContainer;
using ViewProvision.Contract;
using ViewVisualization.Annotations;
using System;
using System.Threading;
using System.Windows;
using System.Collections.Generic;
using DirectShowLib;
using System.Linq;
using System.Windows.Input;
using ConfigService.Contract;
using ConfigService.Client;
using ViewProvision;
using ConfigService.Server;
using Prism.Commands;
using ViewProvision.Processors;
using ViewVisualization.Controls;

namespace ViewVisualization.ViewModels
{
    /// <summary>
    /// Viewmodel of whole configuration application
    /// </summary>
    class MainViewModel : INotifyPropertyChanged
    {
        private ViewDataBitmap viewData;

        /// <summary>
        /// Images currently shown by application
        /// </summary>
        public ViewDataBitmap ViewData { get
            {return viewData;}
            set
            {
                var old = viewData;
                viewData = value;
                OnPropertyChanged();
                if (old != null)
                {
                    old.LeftImage?.Dispose();
                    old.RightImage?.Dispose();
                }
            }
        }

        private readonly IViewProviderService viewProvider;

        private ObservableCollection<string> _systemCameras;

        /// <summary>
        /// Cameras available in the system
        /// </summary>
        public ObservableCollection<string> SystemCameras
        {
            get { return _systemCameras; }
            set
            {
                _systemCameras = value;
                OnPropertyChanged();
            }
        }


        private int leftCameraIndex;

        /// <summary>
        /// Index of the camera assigned to the left channel
        /// </summary>
        public int LeftCameraIndex
        {
            get { return leftCameraIndex;}
            set
            {
                leftCameraIndex = value;
                if(value != -1)
                    viewProvider.SetCapture(CaptureSide.Left, leftCameraIndex);
            }
        }

        private int rightCameraIndex;

        /// <summary>
        /// Index of the camera assigned to the right channel
        /// </summary>
        public int RightCameraIndex
        {
            get { return rightCameraIndex; }
            set
            {
                rightCameraIndex = value;
                if(value != -1)
                    viewProvider.SetCapture(CaptureSide.Right, rightCameraIndex);
            }
        }
        
        public ICommand LeftRotateLeftCommand { get; set; }
        public ICommand LeftRotateRightCommand { get; set; }
        public ICommand RightRotateLeftCommand { get; set; }
        public ICommand RightRotateRightCommand { get; set; }
        public ICommand StartCaptureCommand { get; set; }

        public MainViewModel()
        {
#if DEBUG
            //Only for using without Unity to host the service
            ViewProvider provider = new ViewProvision.ViewProvider(true);
            IProcessedViewProvider processedProvider = new ProcessedViewProvider(provider, new List<IImageProcessor>()
            {
                new GrayImageProcessor(),
                new SmoothBilateralProcessor(7,255,34),
                new SobelProcessor(),
                new ColorProcessor()
            });
            ViewProviderService service = ViewProviderService.Create(processedProvider);
#endif

            viewProvider = IoCManager.Get<IViewProviderService>();
            (viewProvider as ViewProviderClient).OnException += (sender, e) => MessageBox.Show($"Exception:\n{(e.ExceptionObject as Exception)?.Message}");

            RefreshAvailableCameras();
            var captureDetails = viewProvider.GetCaptureDetails();

            leftCameraIndex = captureDetails?.LeftChannel.CaptureIndex ?? 0;
            rightCameraIndex = captureDetails?.RightChannel.CaptureIndex ?? 0;

            LeftRotateLeftCommand = new DelegateCommand(() => viewProvider.RotateImage(CaptureSide.Left,  RotateSide.Left));
            LeftRotateRightCommand = new DelegateCommand(() => viewProvider.RotateImage(CaptureSide.Left, RotateSide.Right));
            RightRotateLeftCommand = new DelegateCommand(() => viewProvider.RotateImage(CaptureSide.Right, RotateSide.Left));
            RightRotateRightCommand = new DelegateCommand(() => viewProvider.RotateImage(CaptureSide.Right, RotateSide.Right));

            StartCaptureCommand = new DelegateCommand(StartCapture);
        }

        private CancellationTokenSource _cancelSource;
        private bool _activeProcessing = false;
        private void StartCapture()
        {
            if (!_activeProcessing)
            {
                _cancelSource = new CancellationTokenSource();
                StartProcessingFrames();
            }
            else
            {
                _cancelSource.Cancel();
            }

            _activeProcessing = !_activeProcessing;
        }

        private IEnumerable<string> GetAvailableCaptureIndexes()
        {
            DsDevice[] systemCameras = DsDevice.GetDevicesOfCat(FilterCategory.VideoInputDevice);                        
            return systemCameras.Select(cam => cam.Name);
        }

        /// <summary>
        /// Update info about cameras available in the system.
        /// </summary>
        internal void RefreshAvailableCameras()
        {
            SystemCameras = new ObservableCollection<string>(GetAvailableCaptureIndexes());
        }
        
        internal void StartProcessingFrames()
        {
            Task.Run(() =>
            {
                while (true)
                {
                    if (_cancelSource.Token.IsCancellationRequested)
                        return;

                    ProcessNextFrames();
                }
            });
        }
        
        private void ProcessNextFrames()
        {
#if DEBUG
            //only for developement & testing
            viewProvider.UpdateFrames();
#endif

            var frames = viewProvider.GetCurrentViewAsBitmaps();

            Application.Current?.Dispatcher?.Invoke(() => 
            {
                ViewData = frames;
            });
        }


        #region INotifyPropertyChanged implementation
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
