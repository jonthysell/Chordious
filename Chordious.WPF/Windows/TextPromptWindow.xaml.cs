// Copyright (c) Jon Thysell <http://jonthysell.com>
// Licensed under the MIT License.

using System.Windows;

namespace Chordious.WPF
{
    /// <summary>
    /// Interaction logic for TextPromptWindow.xaml
    /// </summary>
    public partial class TextPromptWindow : Window
    {
        public TextPromptWindow()
        {
            InitializeComponent();
            IntegrationUtils.EnableAutoSelectOnFirstLoad(InputTextBox);
        }
    }
}
