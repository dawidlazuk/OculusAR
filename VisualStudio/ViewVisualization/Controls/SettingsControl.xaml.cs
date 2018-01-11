using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using ConfigService.Contract;
using ConfigService.Server;
using IoCContainer;
using ViewProvision;
using ViewProvision.Contract;
using ViewProvision.Processors;
using ViewVisualization.Annotations;
using ViewVisualization.Converters;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;
using UserControl = System.Windows.Controls.UserControl;

namespace ViewVisualization.Controls
{
    /// <summary>
    /// Control for the image processing settings interface.
    /// </summary>
    public partial class SettingsControl : UserControl, INotifyPropertyChanged
    {
        /// <summary>
        /// Collection of Available image processors
        /// </summary>
        public ObservableCollection<ProcessorInfo> ImageProcessors
        {
            get { return _imageProcessorInfos; }
            set
            {
                _imageProcessorInfos = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<ProcessorInfo> _imageProcessorInfos;

        private bool _start = true;

        public ICommand ToggleCaptureCommand
        {
            get { return (ICommand) GetValue(ToggleCaptureCommandProperty); }
            set { SetValue(ToggleCaptureCommandProperty, value); }
        }

        public static readonly DependencyProperty ToggleCaptureCommandProperty =
            DependencyProperty.Register(
                "ToggleCaptureCommand",
                typeof(ICommand),
                typeof(SettingsControl));

        private IViewProviderService _viewProvider;

        public SettingsControl()
        {
            InitializeComponent();
            CustomInitialize();
        }

        private void CustomInitialize()
        {
#if DEBUG
            //TODO remove - only for using without Unity to host the service
            ViewProvider provider = new ViewProvision.ViewProvider(true);
            IProcessedViewProvider processedProvider = new ProcessedViewProvider(provider, new List<IImageProcessor>()
            {
                new GrayImageProcessor(),
                new SmoothBilateralProcessor(7,255,34),
                new SobelProcessor()
            });
            ViewProviderService service = ViewProviderService.Create(processedProvider);
#endif

            SettingsGrid.DataContext = this;
            _viewProvider = IoCManager.Get<IViewProviderService>();

            GetProcessorsList();
        }

        private void GetProcessorsList()
        {
            ImageProcessors = new ObservableCollection<ProcessorInfo>(_viewProvider.GetAllImageProcessors()
                .Select(x => new ProcessorInfo()
                {
                    Name = x.Item1,
                    Active = x.Item2
                })
                .AsEnumerable());
        }

        private void ActiveCheckBoxOnClick(object sender, RoutedEventArgs e)
        {
            var checkBox = (CheckBox) sender;
            var index = ImageProcessorsList.Items.IndexOf(checkBox.DataContext);
            if (index < 0) return;

            var selectedProcessor = ImageProcessors[index];

            _viewProvider.SetProcessorState(selectedProcessor.Name, selectedProcessor.Active);
        }

        private void MoveDownButtonOnClick(object sender, RoutedEventArgs e)
        {
            var button = (Button) sender;
            var index = ImageProcessorsList.Items.IndexOf(button.DataContext);
            if (index < 0) return;

            var selectedProcessor = ImageProcessors[index];
            _viewProvider.ChangeProcessorPriority(selectedProcessor.Name, false);

            GetProcessorsList();
        }

        private void MoveUpButtonOnClick(object sender, RoutedEventArgs e)
        {
            var button = (Button) sender;
            var index = ImageProcessorsList.Items.IndexOf(button.DataContext);
            if (index < 0) return;

            var selectedProcessor = ImageProcessors[index];
            _viewProvider.ChangeProcessorPriority(selectedProcessor.Name, true);

            GetProcessorsList();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void StartUnityButton_OnClick(object sender, RoutedEventArgs e)
        {

        }

        private void CaptureVideoButton_OnClick(object sender, RoutedEventArgs e)
        {
            ICommand command = (ICommand) GetValue(ToggleCaptureCommandProperty);
            command.Execute(null);

            CaptureButtonLabel.Content = _start ? "Stop capturing" : "Start capturing";

            CaptureButtonImagePlay.Visibility = _start ? Visibility.Collapsed : Visibility.Visible;
            CaptureButtonImageStop.Visibility = !_start ? Visibility.Collapsed : Visibility.Visible;

            _start = !_start;
        }
    }
}
