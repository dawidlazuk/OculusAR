using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using ViewVisualization.ViewModels;

namespace ViewVisualization
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainViewModel viewModel;

        public MainWindow()
        {
            InitializeComponent();

            viewModel = new MainViewModel();
            this.DataContext = viewModel;

            this.Loaded += viewModel.StartProcessingFrames;
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);

            HwndSource source = HwndSource.FromHwnd(new WindowInteropHelper(this).Handle);
            if(source != null)
            {
                source.AddHook(HwndHandler);
                UsbNotification.RegisterUsbDeviceNotification(source.Handle);
            }
        }

        private IntPtr HwndHandler(IntPtr hwnd, int msg, IntPtr wparam, IntPtr lparam, ref bool handled)
        {
            if(msg == UsbNotification.WmDevicechange)
            {
                //Code responsible for USB device insert/remove events
                switch ((int)wparam)
                {
                    case UsbNotification.DbtDevicearrival:
                        var task = Task.Run(() =>
                        {
                            Thread.Sleep(1500);
                            Application.Current.Dispatcher.Invoke(viewModel.RefreshAvailableCameras);
                        });                        
                        break;

                    case UsbNotification.DbtDeviceremovecomplete:
                        viewModel.RefreshAvailableCameras();
                        break;
                }
            }
            handled = false;
            return IntPtr.Zero;
        }
    }
}
