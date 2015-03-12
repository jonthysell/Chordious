// 
// ScaleFinderWindow.xaml.cs
//  
// Author:
//       Jon Thysell <thysell@gmail.com>
// 
// Copyright (c) 2015 Jon Thysell <http://jonthysell.com>
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

using com.jonthysell.Chordious.Core.ViewModel;

namespace com.jonthysell.Chordious.WPF
{
    /// <summary>
    /// Interaction logic for ChordFinderWindow.xaml
    /// </summary>
    public partial class ScaleFinderWindow : Window
    {
        public ScaleFinderWindow()
        {
            InitializeComponent();

            // Handle the lack of binding selected listview items in WPF
            ResultsListView.SelectionChanged += ResultsListView_SelectionChanged;
        }

        void ResultsListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ScaleFinderViewModel vm = DataContext as ScaleFinderViewModel;

            if (null != vm)
            {
                foreach (object item in e.AddedItems)
                {
                    ObservableDiagram od = item as ObservableDiagram;
                    vm.SelectedResults.Add(od);
                }

                foreach (object item in e.RemovedItems)
                {
                    ObservableDiagram od = item as ObservableDiagram;
                    vm.SelectedResults.Remove(od);
                }                
            }
        }
    }
}
