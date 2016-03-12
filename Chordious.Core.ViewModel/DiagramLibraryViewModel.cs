// 
// DiagramLibraryViewModel.cs
//  
// Author:
//       Jon Thysell <thysell@gmail.com>
// 
// Copyright (c) 2015, 2016 Jon Thysell <http://jonthysell.com>
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

using com.jonthysell.Chordious.Core;
using com.jonthysell.Chordious.Core.Legacy;

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
                RaisePropertyChanged("SelectedNode");
                RaisePropertyChanged("NodeIsSelected");
                RaisePropertyChanged("CreateNode");
                RaisePropertyChanged("CreateNodeLabel");
                RaisePropertyChanged("EditNode");
                RaisePropertyChanged("EditNodeLabel");
                RaisePropertyChanged("CloneNode");
                RaisePropertyChanged("CloneNodeLabel");
                RaisePropertyChanged("DeleteNode");
                RaisePropertyChanged("DeleteNodeLabel");
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
                ObservableCollection<ObservableDiagramLibraryNode> collection = new ObservableCollection<ObservableDiagramLibraryNode>();
                GetNodes(collection, PathUtils.PathRoot);
                return collection;
            }
        }

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
                return new RelayCommand(() =>
                {
                    try
                    {
                        Messenger.Default.Send<PromptForTextMessage>(new PromptForTextMessage(Strings.DiagramLibraryCreateNodePrompt, Library.GetNewCollectionName(), (name) =>
                        {
                            Library.Add(name);
                            RaisePropertyChanged("Nodes");
                        }));
                    }
                    catch (Exception ex)
                    {
                        ExceptionUtils.HandleException(ex);
                    }
                });
            }
        }

        #endregion

        #region EditNode

        public string EditNodeLabel
        {
            get
            {
                if (NodeIsSelected)
                {
                    return String.Format(Strings.DiagramLibraryEditNodeLabelFormat, SelectedNode.Name);
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
                return new RelayCommand(() =>
                {
                    try
                    {
                        string path = SelectedNode.Path;
                        string oldName = SelectedNode.Name;
                        Messenger.Default.Send<PromptForTextMessage>(new PromptForTextMessage(String.Format("Rename collection \"{0}\" to:", oldName), oldName, (newName) =>
                        {
                            Library.Rename(path, oldName, newName);
                            RaisePropertyChanged("Nodes");
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

        #endregion

        #region CloneNode

        public string CloneNodeLabel
        {
            get
            {
                if (NodeIsSelected)
                {
                    return String.Format(Strings.DiagramLibraryCloneNodeLabelFormat, SelectedNode.Name);
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
                return new RelayCommand(() =>
                {
                    try
                    {
                        string path = SelectedNode.Path;
                        string oldName = SelectedNode.Name;
                        Messenger.Default.Send<PromptForTextMessage>(new PromptForTextMessage(String.Format(Strings.DiagramLibraryCloneNodePromptFormat, oldName), Library.GetNewCollectionName(path, oldName), (newName) =>
                        {
                            Library.Clone(path, oldName, newName);
                            RaisePropertyChanged("Nodes");
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

        #endregion

        #region DeleteNode

        public string DeleteNodeLabel
        {
            get
            {
                if (NodeIsSelected)
                {
                    return String.Format(Strings.DiagramLibraryDeleteNodeLabelFormat, SelectedNode.Name);
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
                return new RelayCommand(() =>
                {
                    try
                    {
                        string path = SelectedNode.Path;
                        string name = SelectedNode.Name;
                        Messenger.Default.Send<ConfirmationMessage>(new ConfirmationMessage(String.Format(Strings.DiagramLibraryDeleteNodePromptFormat, name), (confirmed) =>
                        {
                            if (confirmed)
                            {
                                Library.Remove(path, name);
                                RaisePropertyChanged("Nodes");
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

        #endregion

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
                collection.Add(new ObservableDiagramLibraryNode(path, kvp.Key, Library));
            }
        }
    }
}
