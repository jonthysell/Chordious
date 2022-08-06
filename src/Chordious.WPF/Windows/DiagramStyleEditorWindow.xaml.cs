// Copyright (c) Jon Thysell <http://jonthysell.com>
// Licensed under the MIT License.

using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Chordious.WPF
{
    /// <summary>
    /// Interaction logic for DiagramStyleEditorWindow.xaml
    /// </summary>
    public partial class DiagramStyleEditorWindow : Window
    {
        public DiagramStyleEditorWindow()
        {
            InitializeComponent();
        }

        private void FocusOn_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is UIElement uiElement)
            {
                if (e.Source is not ComboBox)
                {
                    uiElement.Focus();
                }
            }
        }
    }
}
