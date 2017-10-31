using System.Collections.ObjectModel;
using System.Drawing;
using System.Windows.Controls;
using ViewVisualization.ViewModels;

namespace ViewVisualization.Controls
{
    /// <summary>
    /// Interaction logic for ChannelControl.xaml
    /// </summary>
    public partial class ChannelControl : UserControl
    {
        private ChannelViewModel viewModel;

        //TODO Make belows as dependency property to set binding on them
        public Bitmap Image { get; set; }

        public int CameraIndex { get; set; }

        public ObservableCollection<int> CameraIndexes { get; set; }


        public ChannelControl()
        {
            InitializeComponent();

//            viewModel = new ChannelViewModel();
//            this.DataContext = viewModel;
        }
    }
}
