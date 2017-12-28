using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Forms;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;
using UserControl = System.Windows.Controls.UserControl;

namespace ViewVisualization.Controls
{
    /// <summary>
    /// Interaction logic for SettingsControl.xaml
    /// </summary>
    public partial class SettingsControl : UserControl
    {
        public ObservableCollection<ProcessingLibrary> Libraries => _libraries;

        private readonly ObservableCollection<ProcessingLibrary> _libraries = new ObservableCollection<ProcessingLibrary>();

        public SettingsControl()
        {
            InitializeComponent();
            CustomInitialize();
        }

        private void CustomInitialize()
        {
            SettingsGrid.DataContext = this;
            _libraries.Add(new ProcessingLibrary() { Name = "Gray filter" });
            _libraries.Add(new ProcessingLibrary() { Name = "Sobel filter" });
            _libraries.Add(new ProcessingLibrary() { Name = "Edge detection"});
        }

        private void LoadLibraries_OnClick(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog
            {
                Multiselect = true,
                Filter = "Class Library Files | *.dll"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                var files = openFileDialog.SafeFileNames;
                foreach (var file in files)
                {
                    var methods = new ObservableCollection<ProcessingMethod>() { new ProcessingMethod() { Name = file } };
                    var library = new ProcessingLibrary() { Name = file, Methods = methods };
                    _libraries.Add(library);
                }
            }
        }
    }

    public class ProcessingMethod
    {
        public string Name { get; set; }
    }

    public class ProcessingLibrary
    {
        public string Name { get; set; }
        public bool Active { get; set; }
        public ObservableCollection<ProcessingMethod> Methods { get; set; }
    }
}
