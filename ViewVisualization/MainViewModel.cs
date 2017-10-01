using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using ViewProvision;
using ViewVisualization.Annotations;

namespace ViewVisualization
{
    class MainViewModel : INotifyPropertyChanged
    {
        private ViewData viewData;
        public ViewData ViewData { get
            {return viewData;;}
            set
            {
                viewData = value;
                OnPropertyChanged();
            } }

        private readonly ViewProvider viewProvider;

        public MainViewModel()
        {
            viewProvider = new ViewProvider();

            Task.Run(() =>
            {
                while(true)
                    ProcessNextFrames();
            });
        }

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
