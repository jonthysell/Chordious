// Copyright (c) Jon Thysell <http://jonthysell.com>
// Licensed under the MIT License.

using System.Windows;

using Chordious.Core.ViewModel;

namespace Chordious.WPF
{
    /// <summary>
    /// Interaction logic for ScaleEditorWindow.xaml
    /// </summary>
    public partial class ChordQualityEditorWindow : Window
    {
        public ChordQualityEditorWindow()
        {
            InitializeComponent();
            Loaded += ChordQualityEditorWindow_Loaded;
        }

        private void ChordQualityEditorWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (DataContext is NamedIntervalEditorViewModel vm && vm.IsNew)
            {
                IntegrationUtils.EnableAutoSelectOnFirstLoad(NameTextBox);
            }
        }
    }
}
