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

        private readonly IViewProvider viewProvider;

        public ObservableCollection<int> CameraIndexes { get; set; }

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
            IoCManager.Initialize();
            viewProvider = IoCManager.Get<IViewProvider>();
            CameraIndexes = new ObservableCollection<int>(viewProvider.AvailableCaptureIndexes);            
        }
        
        internal void StartProcessingFrames(object sender, EventArgs e)
        {
            Task.Run(() =>
            {
                while (true)
                {
                    ProcessNextFrames();
                }
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
