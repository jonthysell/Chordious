// Copyright (c) Jon Thysell <http://jonthysell.com>
// Licensed under the MIT License.

using System.Windows;

using Chordious.Core.ViewModel;

namespace Chordious.WPF
{
    /// <summary>
    /// Interaction logic for ScaleEditorWindow.xaml
    /// </summary>
    public partial class ScaleEditorWindow : Window
    {
        public ScaleEditorWindow()
        {
            InitializeComponent();
            Loaded += ScaleEditorWindow_Loaded;
        }

        private void ScaleEditorWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (DataContext is NamedIntervalEditorViewModel vm && vm.IsNew)
            {
                IntegrationUtils.EnableAutoSelectOnFirstLoad(NameTextBox);
            }
        }
    }
}
