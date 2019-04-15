// 
// DiagramLibraryViewModel.cs
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

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;

using com.jonthysell.Chordious.Core.ViewModel.Resources;

namespace com.jonthysell.Chordious.Core.ViewModel
{
    public class DiagramLibraryViewModel : ViewModelBase
    {
        public AppViewModel AppVM
        {
            get
            {
                return AppViewModel.Instance;
            }
        }

        public string Title
        {
            get
            {
                return Strings.DiagramLibraryTitle;
            }
        }

        #region SelectedNode

        public bool NodeIsSelected
        {
            get
            {
                return (null != SelectedNode);
            }
        }

        public ObservableDiagramLibraryNode SelectedNode
        {
            get
            {
                return _selectedNode;
            }
            private set
            {
                // Make sure to deselect diagrams when selecting a new library node
                if (null != SelectedNode)
                {
                    SelectedNode.SelectedDiagrams.Clear();
                }

                _selectedNode = value;
                RaisePropertyChanged(nameof(SelectedNode));
                RaisePropertyChanged(nameof(NodeIsSelected));
                CreateNode.RaiseCanExecuteChanged();
                RaisePropertyChanged(nameof(CreateNodeLabel));
                EditNode.RaiseCanExecuteChanged();
                RaisePropertyChanged(nameof(EditNodeLabel));
                DeleteNode.RaiseCanExecuteChanged();
                RaisePropertyChanged(nameof(DeleteNodeLabel));
                CloneNode.RaiseCanExecuteChanged();
                RaisePropertyChanged(nameof(CloneNodeLabel));
                RaisePropertyChanged(nameof(CopyNode));
                RaisePropertyChanged(nameof(CopyNodeLabel));
                RaisePropertyChanged(nameof(MergeNode));
                RaisePropertyChanged(nameof(MergeNodeLabel));
                RaisePropertyChanged(nameof(EditNodeStyle));
                RaisePropertyChanged(nameof(EditNodeStyleLabel));
            }
        }
        private ObservableDiagramLibraryNode _selectedNode;

        #endregion SelectedNode

        #region Nodes

        public string NodesLabel
        {
            get
            {
                return Strings.DiagramLibraryNodesLabel;
            }
        }

        public string NodesToolTip
        {
            get
            {
                return Strings.DiagramLibraryNodesToolTip;
            }
        }

        public ObservableCollection<ObservableDiagramLibraryNode> Nodes
        {
            get
            {
                if (_firstLoad || null == _nodes)
                {
                    ObservableCollection<ObservableDiagramLibraryNode> collection = new ObservableCollection<ObservableDiagramLibraryNode>();
                    GetNodes(collection, PathUtils.PathRoot);
                    _nodes = collection;
                    _firstLoad = false;
                }

                return _nodes;
            }
        }
        private ObservableCollection<ObservableDiagramLibraryNode> _nodes;

        private bool _firstLoad = true;

        #endregion

        #region CreateNode

        public string CreateNodeLabel
        {
            get
            {
                return Strings.NewLabel;
            }
        }

        public string CreateNodeToolTip
        {
            get
            {
                return Strings.DiagramLibraryCreateNodeToolTip;
            }
        }

        public RelayCommand CreateNode
        {
            get
            {
                return _createNode ?? (_createNode = new RelayCommand(() =>
                {
                    try
                    {
                        Messenger.Default.Send(new PromptForTextMessage(Strings.DiagramLibraryCreateNodePrompt, Library.GetNewCollectionName(), (name) =>
                        {
                            try
                            {
                                Library.Add(name);
                                ReloadNodes();
                            }
                            catch (Exception ex)
                            {
                                ExceptionUtils.HandleException(ex);
                            }
                        }));
                    }
                    catch (Exception ex)
                    {
                        ExceptionUtils.HandleException(ex);
                    }
                }));
            }
        }
        private RelayCommand _createNode;

        #endregion

        #region EditNode

        public string EditNodeLabel
        {
            get
            {
                if (NodeIsSelected)
                {
                    return string.Format(Strings.DiagramLibraryEditNodeLabelFormat, SelectedNode.Name);
                }

                return Strings.EditLabel;
            }
        }

        public string EditNodeToolTip
        {
            get
            {
                return Strings.DiagramLibraryEditNodeToolTip;
            }
        }

        public RelayCommand EditNode
        {
            get
            {
                return _editNode ?? (_editNode = new RelayCommand(() =>
                {
                    try
                    {
                        string path = SelectedNode.Path;
                        string oldName = SelectedNode.Name;
                        Messenger.Default.Send(new PromptForTextMessage(string.Format(Strings.DiagramLibraryRenameCollectionPromptFormat, oldName), oldName, (newName) =>
                        {
                            try
                            {
                                Library.Rename(path, oldName, newName);
                                ReloadNodes();
                            }
                            catch (Exception ex)
                            {
                                ExceptionUtils.HandleException(ex);
                            }
                        }));
                    }
                    catch (Exception ex)
                    {
                        ExceptionUtils.HandleException(ex);
                    }
                }, () =>
                {
                    return NodeIsSelected;
                }));
            }
        }
        private RelayCommand _editNode;

        #endregion

        #region DeleteNode

        public string DeleteNodeLabel
        {
            get
            {
                if (NodeIsSelected)
                {
                    return string.Format(Strings.DiagramLibraryDeleteNodeLabelFormat, SelectedNode.Name);
                }

                return Strings.DeleteLabel;
            }
        }

        public string DeleteNodeToolTip
        {
            get
            {
                return Strings.DiagramLibraryDeleteNodeToolTip;
            }
        }

        public RelayCommand DeleteNode
        {
            get
            {
                return _deleteNode ?? (_deleteNode = new RelayCommand(() =>
                {
                    try
                    {
                        string path = SelectedNode.Path;
                        string name = SelectedNode.Name;
                        Messenger.Default.Send(new ConfirmationMessage(string.Format(Strings.DiagramLibraryDeleteNodePromptFormat, name), (confirmed) =>
                        {
                            try
                            {
                                if (confirmed)
                                {
                                    Library.Remove(path, name);
                                    ReloadNodes();
                                }
                            }
                            catch (Exception ex)
                            {
                                ExceptionUtils.HandleException(ex);
                            }
                        }, "confirmation.diagramlibrary.deletenode"));
                    }
                    catch (Exception ex)
                    {
                        ExceptionUtils.HandleException(ex);
                    }
                }, () =>
                {
                    return NodeIsSelected;
                }));
            }
        }
        private RelayCommand _deleteNode;

        #endregion

        #region CloneNode

        public string CloneNodeLabel
        {
            get
            {
                if (NodeIsSelected)
                {
                    return string.Format(Strings.DiagramLibraryCloneNodeLabelFormat, SelectedNode.Name);
                }

                return Strings.CloneLabel;
            }
        }

        public string CloneNodeToolTip
        {
            get
            {
                return Strings.DiagramLibraryCloneNodeToolTip;
            }
        }

        public RelayCommand CloneNode
        {
            get
            {
                return _cloneNode ?? (_cloneNode = new RelayCommand(() =>
                {
                    try
                    {
                        string path = SelectedNode.Path;
                        string oldName = SelectedNode.Name;
                        Messenger.Default.Send(new PromptForTextMessage(string.Format(Strings.DiagramLibraryCloneNodePromptFormat, oldName), Library.GetNewCollectionName(path, oldName), (newName) =>
                        {
                            try
                            {
                                Library.Clone(path, oldName, newName);
                                ReloadNodes();
                            }
                            catch (Exception ex)
                            {
                                ExceptionUtils.HandleException(ex);
                            }
                        }));
                    }
                    catch (Exception ex)
                    {
                        ExceptionUtils.HandleException(ex);
                    }
                }, () =>
                {
                    return NodeIsSelected;
                }));
            }
        }
        private RelayCommand _cloneNode;

        #endregion

        #region CopyNode

        public string CopyNodeLabel
        {
            get
            {
                if (NodeIsSelected)
                {
                    return string.Format(Strings.DiagramLibraryCopyNodeLabelFormat, SelectedNode.Name);
                }

                return Strings.CopyLabel;
            }
        }

        public string CopyNodeToolTip
        {
            get
            {
                return Strings.DiagramLibraryCopyNodeToolTip;
            }
        }

        public RelayCommand<string> CopyNode
        {
            get
            {
                if (NodeIsSelected)
                {
                    return SelectedNode.CopyNode;
                }

                return _copyNode ?? (_copyNode = new RelayCommand<string>((s) =>
                {
                    try
                    {
                        Messenger.Default.Send(new ChordiousMessage(Strings.DiagramLibrarySelectNodeFirstMessage));
                    }
                    catch (Exception ex)
                    {
                        ExceptionUtils.HandleException(ex);
                    }
                }, (s) =>
                {
                    return NodeIsSelected;
                }));
            }
        }
        private RelayCommand<string> _copyNode;

        #endregion

        #region MergeNode

        public string MergeNodeLabel
        {
            get
            {
                if (NodeIsSelected)
                {
                    return string.Format(Strings.DiagramLibraryMergeNodeLabelFormat, SelectedNode.Name);
                }

                return Strings.MergeLabel;
            }
        }

        public string MergeNodeToolTip
        {
            get
            {
                return Strings.DiagramLibraryMergeNodeToolTip;
            }
        }

        public RelayCommand<string> MergeNode
        {
            get
            {
                if (NodeIsSelected)
                {
                    return SelectedNode.MergeNode;
                }

                return _mergeNode ?? (_mergeNode = new RelayCommand<string>((s) =>
                {
                    try
                    {
                        Messenger.Default.Send(new ChordiousMessage(Strings.DiagramLibrarySelectNodeFirstMessage));
                    }
                    catch (Exception ex)
                    {
                        ExceptionUtils.HandleException(ex);
                    }
                }, (s) =>
                {
                    return NodeIsSelected;
                }));
            }
        }
        private RelayCommand<string> _mergeNode;

        #endregion

        #region EditNodeStyle

        public string EditNodeStyleLabel
        {
            get
            {
                if (NodeIsSelected)
                {
                    return string.Format(Strings.DiagramLibraryEditNodeStyleLabelFormat, SelectedNode.Name);
                }

                return Strings.EditStyleLabel;
            }
        }

        public string EditNodeStyleToolTip
        {
            get
            {
                return Strings.DiagramLibraryEditNodeStyleToolTip;
            }
        }

        public RelayCommand EditNodeStyle
        {
            get
            {
                if (NodeIsSelected)
                {
                    return SelectedNode.EditCollectionStyle;
                }

                return _editNodeStyle ?? (_editNodeStyle = new RelayCommand(() =>
                {
                    try
                    {
                        Messenger.Default.Send(new ChordiousMessage(Strings.DiagramLibrarySelectNodeFirstMessage));
                    }
                    catch (Exception ex)
                    {
                        ExceptionUtils.HandleException(ex);
                    }
                }, () =>
                {
                    return NodeIsSelected;
                }));
            }
        }
        private RelayCommand _editNodeStyle;

        #endregion

        public Action RequestClose;

        public RelayCommand Close
        {
            get
            {
                return _close ?? (_close = new RelayCommand(() =>
                {
                    try
                    {
                        RequestClose?.Invoke();
                    }
                    catch (Exception ex)
                    {
                        ExceptionUtils.HandleException(ex);
                    }
                }));
            }
        }
        private RelayCommand _close;

        internal DiagramLibrary Library { get; private set; }

        public DiagramLibraryViewModel()
        {
            Library = AppVM.UserConfig.DiagramLibrary;
        }

        private void GetNodes(ObservableCollection<ObservableDiagramLibraryNode> collection, string path)
        {
            foreach (string subfolder in Library.GetSubFolders(path))
            {
                GetNodes(collection, PathUtils.Join(path, subfolder));
            }

            foreach (KeyValuePair<string, DiagramCollection> kvp in Library.GetAll(path))
            {
                collection.Add(new ObservableDiagramLibraryNode(path, kvp.Key, Library, RedrawNodes));
            }
        }

        private void ReloadNodes()
        {
            _firstLoad = true;
            RaisePropertyChanged(nameof(Nodes));
        }

        private void RedrawNodes(bool reloadFirst)
        {
            if (reloadFirst)
            {
                ReloadNodes();
            }

            foreach (ObservableDiagramLibraryNode node in Nodes)
            {
                node.RedrawDiagrams();
            }
        }
    }
}
