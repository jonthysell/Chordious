﻿// 
// DiagramLibraryWindow.xaml.cs
//  
// Author:
//       Jon Thysell <thysell@gmail.com>
// 
// Copyright (c) 2015, 2016, 2017, 2019 Jon Thysell <http://jonthysell.com>
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
    /// Interaction logic for DiagramLibraryWindow.xaml
    /// </summary>
    public partial class DiagramLibraryWindow : Window
    {
        public DiagramLibraryWindow()
        {
            InitializeComponent();

            // Handle the lack of binding selected listview items in WPF
            DiagramsListView.SelectionChanged += DiagramsListView_SelectionChanged;
        }

        void DiagramsListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DiagramLibraryViewModel vm = DataContext as DiagramLibraryViewModel;

            if (null != vm)
            {
                ObservableDiagramLibraryNode selectedNode = vm.SelectedNode;

                if (null != selectedNode)
                {
                    foreach (object item in e.AddedItems)
                    {
                        ObservableDiagram od = item as ObservableDiagram;
                        selectedNode.SelectedDiagrams.Add(od);
                    }

                    foreach (object item in e.RemovedItems)
                    {
                        ObservableDiagram od = item as ObservableDiagram;
                        selectedNode.SelectedDiagrams.Remove(od);
                    }
                }
            }
        }

        private void Image_MouseMove(object sender, MouseEventArgs e)
        {
            if (sender is Image image && image.DataContext is ObservableDiagram od && e.LeftButton == MouseButtonState.Pressed)
            {
                DataObject data = IntegrationUtils.DiagramToDataObject(od, 1.0f);
                DragDrop.DoDragDrop(image, data, DragDropEffects.Copy);
            }
        }
    }
}
