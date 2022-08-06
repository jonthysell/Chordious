// Copyright (c) Jon Thysell <http://jonthysell.com>
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

using Chordious.Core.ViewModel.Resources;

namespace Chordious.Core.ViewModel
{
    public class DiagramLibraryViewModel : ObservableObject
    {
        public static AppViewModel AppVM
        {
            get
            {
                return AppViewModel.Instance;
            }
        }

        public static string Title
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
                return (SelectedNode is not null);
            }
        }

        public ObservableDiagramLibraryNode SelectedNode
        {
            get
            {
                return _selectedNode;
            }
            set
            {
                // Make sure to deselect diagrams when selecting a new library node
                if (SelectedNode is not null)
                {
                    SelectedNode.SelectedDiagrams.Clear();
                }

                _selectedNode = value;
                OnPropertyChanged(nameof(SelectedNode));
                OnPropertyChanged(nameof(NodeIsSelected));
                CreateNode.NotifyCanExecuteChanged();
                OnPropertyChanged(nameof(CreateNodeLabel));
                EditNode.NotifyCanExecuteChanged();
                OnPropertyChanged(nameof(EditNodeLabel));
                DeleteNode.NotifyCanExecuteChanged();
                OnPropertyChanged(nameof(DeleteNodeLabel));
                CloneNode.NotifyCanExecuteChanged();
                OnPropertyChanged(nameof(CloneNodeLabel));
                OnPropertyChanged(nameof(CopyNode));
                OnPropertyChanged(nameof(CopyNodeLabel));
                OnPropertyChanged(nameof(MergeNode));
                OnPropertyChanged(nameof(MergeNodeLabel));
                OnPropertyChanged(nameof(EditNodeStyle));
                OnPropertyChanged(nameof(EditNodeStyleLabel));
            }
        }
        private ObservableDiagramLibraryNode _selectedNode;

        #endregion SelectedNode

        #region Nodes

        public static string NodesLabel
        {
            get
            {
                return Strings.DiagramLibraryNodesLabel;
            }
        }

        public static string NodesToolTip
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
                if (_firstLoad || _nodes is null)
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

        public static string CreateNodeLabel
        {
            get
            {
                return Strings.NewLabel;
            }
        }

        public static string CreateNodeToolTip
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
                return _createNode ??= new RelayCommand(() =>
                {
                    try
                    {
                        StrongReferenceMessenger.Default.Send(new PromptForTextMessage(Strings.DiagramLibraryCreateNodePrompt, Library.GetNewCollectionName(), (name) =>
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
                });
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

        public static string EditNodeToolTip
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
                return _editNode ??= new RelayCommand(() =>
                {
                    try
                    {
                        string path = SelectedNode.Path;
                        string oldName = SelectedNode.Name;
                        StrongReferenceMessenger.Default.Send(new PromptForTextMessage(string.Format(Strings.DiagramLibraryRenameCollectionPromptFormat, oldName), oldName, (newName) =>
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
                });
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

        public static string DeleteNodeToolTip
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
                return _deleteNode ??= new RelayCommand(() =>
                {
                    try
                    {
                        string path = SelectedNode.Path;
                        string name = SelectedNode.Name;
                        StrongReferenceMessenger.Default.Send(new ConfirmationMessage(string.Format(Strings.DiagramLibraryDeleteNodePromptFormat, name), (confirmed) =>
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
                });
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

        public static string CloneNodeToolTip
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
                return _cloneNode ??= new RelayCommand(() =>
                {
                    try
                    {
                        string path = SelectedNode.Path;
                        string oldName = SelectedNode.Name;
                        StrongReferenceMessenger.Default.Send(new PromptForTextMessage(string.Format(Strings.DiagramLibraryCloneNodePromptFormat, oldName), Library.GetNewCollectionName(path, oldName), (newName) =>
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
                });
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

        public static string CopyNodeToolTip
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

                return _copyNode ??= new RelayCommand<string>((s) =>
                {
                    try
                    {
                        StrongReferenceMessenger.Default.Send(new ChordiousMessage(Strings.DiagramLibrarySelectNodeFirstMessage));
                    }
                    catch (Exception ex)
                    {
                        ExceptionUtils.HandleException(ex);
                    }
                }, (s) =>
                {
                    return NodeIsSelected;
                });
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

        public static string MergeNodeToolTip
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

                return _mergeNode ??= new RelayCommand<string>((s) =>
                {
                    try
                    {
                        StrongReferenceMessenger.Default.Send(new ChordiousMessage(Strings.DiagramLibrarySelectNodeFirstMessage));
                    }
                    catch (Exception ex)
                    {
                        ExceptionUtils.HandleException(ex);
                    }
                }, (s) =>
                {
                    return NodeIsSelected;
                });
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

        public static string EditNodeStyleToolTip
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

                return _editNodeStyle ??= new RelayCommand(() =>
                {
                    try
                    {
                        StrongReferenceMessenger.Default.Send(new ChordiousMessage(Strings.DiagramLibrarySelectNodeFirstMessage));
                    }
                    catch (Exception ex)
                    {
                        ExceptionUtils.HandleException(ex);
                    }
                }, () =>
                {
                    return NodeIsSelected;
                });
            }
        }
        private RelayCommand _editNodeStyle;

        #endregion

        public Action RequestClose;

        public RelayCommand Close
        {
            get
            {
                return _close ??= new RelayCommand(() =>
                {
                    try
                    {
                        RequestClose?.Invoke();
                    }
                    catch (Exception ex)
                    {
                        ExceptionUtils.HandleException(ex);
                    }
                });
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
            OnPropertyChanged(nameof(Nodes));
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
