﻿using ConfigService.Contract;
using IoCContainer;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ViewVisualization.ViewModels;

namespace ViewVisualization.Controls
{
    /// <summary>
    /// Interaction logic for ChannelControl.xaml
    /// </summary>
    public partial class ChannelControl : UserControl
    {
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

        public ICommand LeftRotateCommand
        {
            get { return (ICommand)GetValue(LeftRotateCommandProperty); }
            set { SetValue(LeftRotateCommandProperty, value);}
        }

        public ICommand RightRotateCommand
        {
            get { return (ICommand)GetValue(RightRotateCommandProperty); }
            set { SetValue(RightRotateCommandProperty, value); }
        }


        public static readonly DependencyProperty ImageProperty = DependencyProperty.Register("Image",typeof(Bitmap),typeof(ChannelControl));
        public static readonly DependencyProperty CameraIndexProperty = DependencyProperty.Register("CameraIndex",typeof(int),typeof(ChannelControl));
        public static readonly DependencyProperty CameraNamesProperty = DependencyProperty.Register("CameraNames",
            typeof(ObservableCollection<string>), typeof(ChannelControl));

        public static readonly DependencyProperty LeftRotateCommandProperty =
            DependencyProperty.Register(
                "LeftRotateCommand",
                typeof(ICommand),
                typeof(ChannelControl));

        public static readonly DependencyProperty RightRotateCommandProperty =
            DependencyProperty.Register(
                "RightRotateCommand",
                typeof(ICommand),
                typeof(ChannelControl));

        public ChannelControl()
        {
            InitializeComponent();

            ChannelGrid.DataContext = this;

        }

        private void RotateLeftButton_Click(object sender, RoutedEventArgs e)
        {
            ICommand command = (ICommand)GetValue(LeftRotateCommandProperty);
            command.Execute(null);
        }

        private void RotateRightButton_Click(object sender, RoutedEventArgs e)
        {
            ICommand command = (ICommand)GetValue(RightRotateCommandProperty);
            command.Execute(null);
        }
    }
}
