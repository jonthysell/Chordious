// Copyright (c) Jon Thysell <http://jonthysell.com>
// Licensed under the MIT License.

using System.Windows;

using Chordious.Core.ViewModel;

namespace Chordious.WPF
{
    /// <summary>
    /// Interaction logic for InstrumentEditorWindow.xaml
    /// </summary>
    public partial class InstrumentEditorWindow : Window
    {
        public InstrumentEditorWindow()
        {
            InitializeComponent();
            Loaded += InstrumentEditorWindow_Loaded;
        }

        private void InstrumentEditorWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (DataContext is InstrumentEditorViewModel vm && vm.IsNew)
            {
                IntegrationUtils.EnableAutoSelectOnFirstLoad(NameTextBox);
            }
        }
    }
}
