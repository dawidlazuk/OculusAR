using System.Windows;
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
        }
    }
}
