using System.Collections.ObjectModel;
using System.Drawing;
using System.Windows;
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
        public Bitmap Image
        {
            get { return (Bitmap) GetValue(ImageProperty); }
            set { SetValue(ImageProperty, value); }
        }

        public int CameraIndex
        {
            get { return (int) GetValue(CameraIndexProperty); }
            set { SetValue(CameraIndexProperty,value);}
        }

        public ObservableCollection<string> CameraNames
        {
            get { return (ObservableCollection<string>) GetValue(CameraNamesProperty); }
            set { SetValue(CameraNamesProperty, value);}
        }

        public static readonly DependencyProperty ImageProperty = DependencyProperty.Register("Image",typeof(Bitmap),typeof(ChannelControl));
        public static readonly DependencyProperty CameraIndexProperty = DependencyProperty.Register("CameraIndex",typeof(int),typeof(ChannelControl));
        public static readonly DependencyProperty CameraNamesProperty = DependencyProperty.Register("CameraNames",
            typeof(ObservableCollection<string>), typeof(ChannelControl));


        public ChannelControl()
        {
            InitializeComponent();

            ChannelGrid.DataContext = this;
        }
    }
}
