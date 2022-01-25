// Copyright (c) Jon Thysell <http://jonthysell.com>
// Licensed under the MIT License.

using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

using Chordious.Core.ViewModel;

namespace Chordious.WPF
{
    /// <summary>
    /// Interaction logic for DiagramEditorWindow.xaml
    /// </summary>
    public partial class DiagramEditorWindow : Window
    {
        public DiagramEditorViewModel VM
        {
            get
            {
                return _vm ?? (_vm = DataContext as DiagramEditorViewModel);
            }
        }
        private DiagramEditorViewModel _vm;

        public DiagramEditorWindow()
        {
            InitializeComponent();
        }

        private void ImageContextMenu_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            UpdateCursorPosition();
        }

        private void DiagramImage_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            UpdateCursorPosition();
        }

        private bool UpdateCursorPosition()
        {
            ObservableDiagram od = VM.ObservableDiagram;
            if (null != od)
            {
                Point p = MouseUtils.CorrectGetPosition(DiagramImage);
                od.CursorX = p.X;
                od.CursorY = p.Y;
                return od.ValidCommandsAtCursor;
            }
            return false;
        }

        private void DiagramImage_MouseMove(object sender, MouseEventArgs e)
        {
            if (sender is Image image && e.LeftButton == MouseButtonState.Pressed)
            {
                ObservableDiagram od = VM.ObservableDiagram;
                IntegrationUtils.DiagramToDragDrop(image, od);
            }
        }
    }
}
