// 
// DiagramLibraryViewModel.cs
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
using System.Collections.ObjectModel;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;

using com.jonthysell.Chordious.Core;

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
                return "Diagram Library";
            }
        }

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
                _selectedNode = value;
                RaisePropertyChanged("SelectedNode");
                RaisePropertyChanged("NodeIsSelected");
                RaisePropertyChanged("CreateNode");
                RaisePropertyChanged("RenameNode");
                RaisePropertyChanged("DeleteNode");
            }
        }
        private ObservableDiagramLibraryNode _selectedNode;

        public ObservableCollection<ObservableDiagramLibraryNode> Nodes
        {
            get
            {
                ObservableCollection<ObservableDiagramLibraryNode> collection = new ObservableCollection<ObservableDiagramLibraryNode>();
                GetNodes(collection, PathUtils.PathRoot);
                return collection;
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
                        Messenger.Default.Send<PromptForTextMessage>(new PromptForTextMessage("Create new collection named:", Library.GetNewCollectionName(), (name) =>
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
                        Messenger.Default.Send<ConfirmationMessage>(new ConfirmationMessage(String.Format("This will delete the collection \"{0}\". Do you want to continue?", name), (confirmed) =>
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

        public RelayCommand RenameNode
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

        internal DiagramLibrary Library { get; private set; }

        public DiagramLibraryViewModel()
        {
            Library = AppVM.UserConfig.DiagramLibrary;
        }

        private void GetNodes(ObservableCollection<ObservableDiagramLibraryNode> collection, string path)
        {
            foreach (string subfolder in Library.GetSubFolders(path))
            {
                if (String.IsNullOrEmpty(subfolder))
                GetNodes(collection, PathUtils.Join(path, subfolder));
            }

            foreach (KeyValuePair<string, DiagramCollection> kvp in Library.GetAll(path))
            {
                collection.Add(new ObservableDiagramLibraryNode(path, kvp.Key, Library));
            }
        }
    }
}
