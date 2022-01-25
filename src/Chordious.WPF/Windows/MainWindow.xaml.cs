// Copyright (c) Jon Thysell <http://jonthysell.com>
// Licensed under the MIT License.

using System;
using System.Windows;

using Chordious.Core.ViewModel;

namespace Chordious.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            DataContext = new MainViewModelExtended();

            Loaded += MainWindow_Loaded;
        }

        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                // Fix annoying Windows 8.1 tablet forced maximization
                WindowState = WindowState.Normal;

                ((MainViewModelExtended)DataContext).OnLoaded();
            }
            catch (Exception ex)
            {
                ExceptionUtils.HandleException(ex);
            }
        }
    }
}
