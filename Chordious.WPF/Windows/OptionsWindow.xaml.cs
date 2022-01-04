// Copyright (c) Jon Thysell <http://jonthysell.com>
// Licensed under the MIT License.

using System.Windows;

namespace Chordious.WPF
{
    /// <summary>
    /// Interaction logic for OptionsWindow.xaml
    /// </summary>
    public partial class OptionsWindow : Window
    {
        public OptionsViewModelExtended VM
        {
            get
            {
                return DataContext as OptionsViewModelExtended;
            }
            private set
            {
                DataContext = value;
            }
        }

        public OptionsWindow()
        {
            VM = new OptionsViewModelExtended();
            InitializeComponent();
        }
    }
}
