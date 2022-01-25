// Copyright (c) Jon Thysell <http://jonthysell.com>
// Licensed under the MIT License.

using System.Windows;

using Chordious.Core.ViewModel;

namespace Chordious.WPF
{
    /// <summary>
    /// Interaction logic for InstrumentManagerWindow.xaml
    /// </summary>
    public partial class NamedIntervalManagerWindow : Window
    {
        public NamedIntervalManagerViewModel VM
        {
            get
            {

                return DataContext as NamedIntervalManagerViewModel;
            }
            private set
            {
                DataContext = value;
            }
        }

        public NamedIntervalManagerWindow(NamedIntervalManagerViewModel vm)
        {
            VM = vm;
            InitializeComponent();
        }
    }
}
