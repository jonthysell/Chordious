// 
// DiagramEditorWindow.xaml.cs
//  
// Author:
//       Jon Thysell <thysell@gmail.com>
// 
// Copyright (c) 2015, 2017, 2019 Jon Thysell <http://jonthysell.com>
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

using com.jonthysell.Chordious.Core.ViewModel;

namespace com.jonthysell.Chordious.WPF
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
