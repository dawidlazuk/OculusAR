using ConfigService.Contract;
using IoCContainer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ViewVisualization.ViewModels
{
    class ChannelViewModel
    {
        public Bitmap Image { get; set; }

        public int CameraIndex { get; set; }

        public ObservableCollection<int> CameraIndexes { get; set; }

        public ICommand ButtonCommand { get; private set; }

    }
}
