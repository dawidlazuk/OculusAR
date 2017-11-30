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
using ConfigService.Contract;

namespace ViewVisualization.ViewModels
{
    class MainViewModel : INotifyPropertyChanged
    {
        private ViewDataBitmap viewData;
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

        public ObservableCollection<string> SystemCameras { get; set; }

        private int leftCameraIndex;
        public int LeftCameraIndex
        {
            get { return leftCameraIndex;}
            set
            {
                leftCameraIndex = value;
                viewProvider.SetCapture(CaptureSide.Left, leftCameraIndex);
            }
        }

        private int rightCameraIndex;
        public int RightCameraIndex
        {
            get { return rightCameraIndex; }
            set
            {
                rightCameraIndex = value;
                viewProvider.SetCapture(CaptureSide.Right, rightCameraIndex);
            }
        }

        public MainViewModel()
        {
            ViewProvision.ViewProvider provider = new ViewProvision.ViewProvider();

            IoCManager.Initialize();
            viewProvider = IoCManager.Get<IViewProviderService>();
            SystemCameras = new ObservableCollection<string>(GetAvailableCaptureIndexes());

            var captureDetails = viewProvider.GetCaptureDetails();

            leftCameraIndex = captureDetails.LeftIndex;
            rightCameraIndex = captureDetails.RightIndex;       
        }

        private IEnumerable<string> GetAvailableCaptureIndexes()
        {
            DsDevice[] systemCameras = DsDevice.GetDevicesOfCat(FilterCategory.VideoInputDevice);                        
            return systemCameras.Select(cam => cam.Name);
        }
        
        internal void StartProcessingFrames(object sender, EventArgs e)
        {
            Task.Run(() =>
            {
                while (true)
                    ProcessNextFrames();
            });
        }

        private void ProcessNextFrames()
        {
            var frames = viewProvider.GetCurrentViewAsBitmaps();

            Application.Current.Dispatcher.Invoke(() => 
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
