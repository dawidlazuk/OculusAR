using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using IoCContainer;
using ViewProvision.Contract;
using ViewVisualization.Annotations;

namespace ViewVisualization.ViewModels
{
    class MainViewModel : INotifyPropertyChanged
    {
        private ViewData viewData;
        public ViewData ViewData { get
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

            Task.Run(() =>
            {
                while (true)
                    ProcessNextFrames();
            });
        }

        int i = 0;
        private void ProcessNextFrames()
        {
            //TODO - remove (only for developement)
            viewProvider.UpdateFrames();

            ViewData = viewProvider.GetCurrentView();
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
