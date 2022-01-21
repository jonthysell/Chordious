// Copyright (c) Jon Thysell <http://jonthysell.com>
// Licensed under the MIT License.

using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

using Chordious.Core.ViewModel;

namespace Chordious.WPF
{
    /// <summary>
    /// Interaction logic for ChordFinderWindow.xaml
    /// </summary>
    public partial class ChordFinderWindow : Window
    {
        public ChordFinderViewModel VM
        {
            get
            {
                return DataContext as ChordFinderViewModel;
            }
            private set
            {
                DataContext = value;
            }
        }

        public ChordFinderWindow(ChordFinderViewModel vm)
        {
            VM = vm;

            InitializeComponent();

            // Handle the lack of binding selected listview items in WPF
            ResultsListView.SelectionChanged += ResultsListView_SelectionChanged;

            Closing += ChordFinderWindow_Closing;
        }

        private void ChordFinderWindow_Closing(object sender, CancelEventArgs e)
        {
            VM.CancelSearch.Execute(null);
            e.Cancel = false;
        }

        void ResultsListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            foreach (object item in e.AddedItems)
            {
                ObservableDiagram od = item as ObservableDiagram;
                VM.SelectedResults.Add(od);
            }

            foreach (object item in e.RemovedItems)
            {
                ObservableDiagram od = item as ObservableDiagram;
                VM.SelectedResults.Remove(od);
            }
        }

        private void DiagramImage_MouseMove(object sender, MouseEventArgs e)
        {
            if (sender is Image image && image.DataContext is ObservableDiagram od && e.LeftButton == MouseButtonState.Pressed)
            {
                IntegrationUtils.DiagramToDragDrop(image, od);
            }
        }

        private void DiagramImage_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is Image image && image.DataContext is ObservableDiagram od)
            {
                if (e.ChangedButton == MouseButton.Left && e.ButtonState == MouseButtonState.Pressed && e.ClickCount == 1 && VM.SelectedResults.Contains(od) && image != _lastClicked)
                {
                    _lastClicked = image;
                    e.Handled = true;
                }
                else
                {
                    _lastClicked = null;
                }
            }
        }

        private Image _lastClicked = null;
    }
}
