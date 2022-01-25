// Copyright (c) Jon Thysell <http://jonthysell.com>
// Licensed under the MIT License.

using System.Windows;

namespace Chordious.WPF
{
    /// <summary>
    /// Interaction logic for LicensesWindow.xaml
    /// </summary>
    public partial class LicensesWindow : Window
    {
        public LicensesWindow()
        {
            InitializeComponent();
            LicensesTab.SelectedIndex = 0; // Fixes focus bug when check for updates is disabled
        }
    }
}
