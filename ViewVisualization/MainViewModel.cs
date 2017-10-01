using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

using IoCContainer;

using ViewProvision.Contract;
using ViewVisualization.Annotations;

namespace ViewVisualization
{
    class MainViewModel : INotifyPropertyChanged
    {
        private ViewData viewData;
        public ViewData ViewData { get
            {return viewData;}
            set
            {
                viewData = value;
                OnPropertyChanged();
            } }

        private readonly IViewProvider viewProvider;

        public MainViewModel()
        {
            IoCManager.Initialize();
            viewProvider = IoCManager.Get<IViewProvider>();

            Task.Run(() =>
            {
                while (true)
                    ProcessNextFrames();
            });
        }

        int i = 0;
        private void ProcessNextFrames()
        {            
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
