// Copyright (c) Jon Thysell <http://jonthysell.com>
// Licensed under the MIT License.

using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

using Chordious.Core.ViewModel;

namespace Chordious.WPF
{
    /// <summary>
    /// Interaction logic for DiagramLibraryWindow.xaml
    /// </summary>
    public partial class DiagramLibraryWindow : Window
    {
        public DiagramLibraryViewModel VM
        {
            get
            {
                return _vm ??= DataContext as DiagramLibraryViewModel;
            }
        }
        private DiagramLibraryViewModel _vm;

        public DiagramLibraryWindow()
        {
            InitializeComponent();

            // Handle the lack of binding selected listview items in WPF
            DiagramsListView.SelectionChanged += DiagramsListView_SelectionChanged;
        }

        void DiagramsListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ObservableDiagramLibraryNode selectedNode = VM.SelectedNode;

            if (selectedNode is not null)
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

        private void DiagramImage_MouseMove(object sender, MouseEventArgs e)
        {
            if (sender is Image image && image.DataContext is ObservableDiagram && e.LeftButton == MouseButtonState.Pressed)
            {
                IntegrationUtils.DiagramLibraryNodeToDragDrop(DiagramsListView, VM.SelectedNode, true);
            }
        }

        private void DiagramImage_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is Image image && image.DataContext is ObservableDiagram od)
            {
                if (e.ChangedButton == MouseButton.Left && e.ButtonState == MouseButtonState.Pressed && e.ClickCount == 1 && VM.SelectedNode.SelectedDiagrams.Contains(od) && image != _lastClicked)
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

        private void DiagramLibraryNode_MouseMove(object sender, MouseEventArgs e)
        {
            if (sender is FrameworkElement element && element.DataContext is ObservableDiagramLibraryNode sourceNode && e.LeftButton == MouseButtonState.Pressed)
            {
                IntegrationUtils.DiagramLibraryNodeToDragDrop(element, sourceNode, false);
            }
        }

        private void DiagramsListView_Drop(object sender, DragEventArgs e)
        {
            if (sender == DiagramsListView && VM.NodeIsSelected)
            {
                IntegrationUtils.DragDropToDiagramLibraryNode(e.Data, VM.SelectedNode, IntegrationUtils.GetDropAction(e.KeyStates));
                e.Handled = true;
            }
        }

        private void DiagramLibraryNode_Drop(object sender, DragEventArgs e)
        {
            if (sender is FrameworkElement element && element.DataContext is ObservableDiagramLibraryNode destinationNode)
            {
                IntegrationUtils.DragDropToDiagramLibraryNode(e.Data, destinationNode, IntegrationUtils.GetDropAction(e.KeyStates));
                e.Handled = true;
            }
            else if (sender == NodesListView)
            {
                IntegrationUtils.DragDropToDiagramLibraryNode(e.Data, null, IntegrationUtils.GetDropAction(e.KeyStates));
                e.Handled = true;
            }
        }
    }
}
