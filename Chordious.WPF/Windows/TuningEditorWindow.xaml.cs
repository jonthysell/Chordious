// Copyright (c) Jon Thysell <http://jonthysell.com>
// Licensed under the MIT License.

using System.Windows;

using Chordious.Core.ViewModel;

namespace Chordious.WPF
{
    /// <summary>
    /// Interaction logic for TuningEditorWindow.xaml
    /// </summary>
    public partial class TuningEditorWindow : Window
    {
        public TuningEditorWindow()
        {
            InitializeComponent();
            Loaded += TuningEditorWindow_Loaded;
        }

        private void TuningEditorWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (DataContext is TuningEditorViewModel vm && vm.IsNew)
            {
                IntegrationUtils.EnableAutoSelectOnFirstLoad(NameTextBox);
            }
        }
    }
}
