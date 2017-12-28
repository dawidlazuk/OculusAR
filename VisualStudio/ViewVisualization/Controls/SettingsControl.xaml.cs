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

        public SettingsControl()
        {
            InitializeComponent();
            CustomInitialize();
        }

        private void CustomInitialize()
        {
            SettingsGrid.DataContext = this;
        }


    }

}
