// 
// ObservableDiagramLibraryNode.cs
//  
// Author:
//       Jon Thysell <thysell@gmail.com>
// 
// Copyright (c) 2015, 2016, 2017 Jon Thysell <http://jonthysell.com>
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
using System.Collections.Specialized;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;

using com.jonthysell.Chordious.Core.ViewModel.Resources;

namespace com.jonthysell.Chordious.Core.ViewModel
{
    public class ObservableDiagramLibraryNode : ObservableObject
    {
        public AppViewModel AppVM
        {
            get
            {
                return AppViewModel.Instance;
            }
        }

        public string Path
        {
            get
            {
                return _path;
            }
            private set
            {
                _path = value;
                RaisePropertyChanged("Path");
            }
        }
        private string _path;

        public string Name
        {
            get
            {
                return _name;
            }
            private set
            {
                _name = value;
                RaisePropertyChanged("Name");
            }
        }
        private string _name;

        public ObservableCollection<ObservableDiagram> Diagrams
        {
            get
            {
                if (_firstLoad)
                {
                    foreach (Diagram diagram in Library.Get(Path, Name))
                    {
                        _diagrams.Add(CreateObservableDiagram(diagram));
                    }
                    _firstLoad = false;
                }
                return _diagrams;
            }
            private set
            {
                _diagrams = value;
                RaisePropertyChanged("Diagrams");
            }
        }
        private ObservableCollection<ObservableDiagram> _diagrams;

        public ObservableCollection<ObservableDiagram> SelectedDiagrams
        {
            get
            {
                return _selectedDiagrams;
            }
            private set
            {
                if (null == value)
                {
                    throw new ArgumentNullException();
                }

                _selectedDiagrams = value;
                RaisePropertyChanged("SelectedDiagrams");
                UpdateCommands();
            }
        }
        private ObservableCollection<ObservableDiagram> _selectedDiagrams;

        #region CreateDiagram

        public string CreateDiagramLabel
        {
            get
            {
                return Strings.NewLabel;
            }
        }

        public string CreateDiagramToolTip
        {
            get
            {
                return Strings.ObservableDiagramLibraryNodeCreateDiagramToolTip;
            }
        }

        public RelayCommand CreateDiagram
        {
            get
            {
                return new RelayCommand(() =>
                {
                    try
                    {
                        ObservableDiagram od = CreateObservableDiagram();

                        Messenger.Default.Send<ShowDiagramEditorMessage>(new ShowDiagramEditorMessage(od, true, od.PostEditCallback));
                    }
                    catch (Exception ex)
                    {
                        ExceptionUtils.HandleException(ex);
                    }
                });
            }
        }

        #endregion

        #region EditSelected

        public string EditSelectedLabel
        {
            get
            {
                return Strings.EditLabel;
            }
        }

        public string EditSelectedToolTip
        {
            get
            {
                return Strings.ObservableDiagramLibraryNodeEditSelectedToolTip;
            }
        }

        public RelayCommand EditSelected
        {
            get
            {
                // Use the built-in ObservableDiagram's RelayCommand if it's available
                if (SelectedDiagrams.Count == 1)
                {
                    return SelectedDiagrams[0].ShowEditor;
                }

                // If a single diagram isn't selected, throw an error
                return new RelayCommand(() =>
                {
                    try
                    {
                        Messenger.Default.Send<ChordiousMessage>(new ChordiousMessage(Strings.DiagramLibraryOnlyOneDiagramCanBeEditedMessage));
                    }
                    catch (Exception ex)
                    {
                        ExceptionUtils.HandleException(ex);
                    }
                }, () =>
                {
                    return SelectedDiagrams.Count > 0;
                });
            }
        }

        #endregion

        #region ResetStylesSelected

        public string ResetStylesSelectedLabel
        {
            get
            {
                return Strings.ResetStylesLabel;
            }
        }

        public string ResetStylesSelectedToolTip
        {
            get
            {
                int count = SelectedDiagrams.Count;
                return string.Format(count == 1 ? Strings.ObservableDiagramLibraryNodeResetStylesSelectedToolTipSingleFormat : Strings.ObservableDiagramLibraryNodeResetStylesSelectedToolTipPluralFormat, count);
            }
        }

        public RelayCommand ResetStylesSelected
        {
            get
            {
                return new RelayCommand(() =>
                {
                    try
                    {
                        int count = SelectedDiagrams.Count;
                        string message = string.Format(count < 2 ? Strings.ObservableDiagramLibraryNodeResetStylesSelectedPromptSingleFormat : Strings.ObservableDiagramLibraryNodeResetStylesSelectedPromptPluralFormat, count);

                        Messenger.Default.Send<ConfirmationMessage>(new ConfirmationMessage(message, (confirmed) =>
                        {
                            if (confirmed)
                            {
                                foreach (ObservableDiagram od in SelectedDiagrams)
                                {
                                    od.ResetStyles();
                                }
                            }
                        }, "confirmation.diagramlibrarynode.resetstyles"));
                    }
                    catch (Exception ex)
                    {
                        ExceptionUtils.HandleException(ex);
                    }
                }, () =>
                {
                    return SelectedDiagrams.Count > 0;
                });
            }
        }

        #endregion

        #region CloneSelected

        public string CloneSelectedLabel
        {
            get
            {
                return Strings.CloneLabel;
            }
        }

        public string CloneSelectedToolTip
        {
            get
            {
                int count = SelectedDiagrams.Count;
                return string.Format(count == 1 ? Strings.ObservableDiagramLibraryNodeCloneSelectedToolTipSingleFormat : Strings.ObservableDiagramLibraryNodeCloneSelectedToolTipPluralFormat, count);
            }
        }

        public RelayCommand CloneSelected
        {
            get
            {
                return new RelayCommand(() =>
                {
                    try
                    {
                        List<ObservableDiagram> itemsToClone = new List<ObservableDiagram>(SelectedDiagrams);

                        DiagramCollection collection = Library.Get(Path, Name);
                        foreach (ObservableDiagram od in itemsToClone)
                        {
                            Diagram clone = od.Diagram.Clone();
                            collection.Add(clone);
                        }
                        Redraw();
                    }
                    catch (Exception ex)
                    {
                        ExceptionUtils.HandleException(ex);
                    }
                }, () =>
                {
                    return SelectedDiagrams.Count > 0;
                });
            }
        }

        #endregion

        #region CopySelected

        public string CopySelectedLabel
        {
            get
            {
                return Strings.CopyLabel;
            }
        }

        public string CopySelectedToolTip
        {
            get
            {
                int count = SelectedDiagrams.Count;
                return string.Format(count == 1 ? Strings.ObservableDiagramLibraryNodeCopySelectedToolTipSingleFormat : Strings.ObservableDiagramLibraryNodeCopySelectedToolTipPluralFormat, count);
            }
        }

        public RelayCommand CopySelected
        {
            get
            {
                return new RelayCommand(() =>
                {
                    try
                    {
                        List<ObservableDiagram> itemsToCopy = new List<ObservableDiagram>(SelectedDiagrams);

                        Messenger.Default.Send<ShowDiagramCollectionSelectorMessage>(new ShowDiagramCollectionSelectorMessage((name, newCollection) =>
                        {
                            try
                            {
                                DiagramLibrary library = AppVM.UserConfig.DiagramLibrary;
                                DiagramCollection targetCollection = library.Get(name);

                                foreach (ObservableDiagram od in itemsToCopy)
                                {
                                    Diagram clone = od.Diagram.Clone();
                                    targetCollection.Add(clone);
                                }

                                Redraw(newCollection);
                            }
                            catch (Exception ex)
                            {
                                ExceptionUtils.HandleException(ex);
                            }
                        }
                        , Name));
                    }
                    catch (Exception ex)
                    {
                        ExceptionUtils.HandleException(ex);
                    }
                }, () =>
                {
                    return SelectedDiagrams.Count > 0;
                });
            }
        }

        #endregion

        #region MoveSelected

        public string MoveSelectedLabel
        {
            get
            {
                return Strings.MoveLabel;
            }
        }

        public string MoveSelectedToolTip
        {
            get
            {
                int count = SelectedDiagrams.Count;
                return string.Format(count == 1 ? Strings.ObservableDiagramLibraryNodeMoveSelectedToolTipSingleFormat : Strings.ObservableDiagramLibraryNodeMoveSelectedToolTipPluralFormat, count);
            }
        }

        public RelayCommand MoveSelected
        {
            get
            {
                return new RelayCommand(() =>
                {
                    try
                    {
                        List<ObservableDiagram> itemsToMove = new List<ObservableDiagram>(SelectedDiagrams);

                        Messenger.Default.Send<ShowDiagramCollectionSelectorMessage>(new ShowDiagramCollectionSelectorMessage((name, newCollection) =>
                        {
                            try
                            {
                                DiagramLibrary library = AppVM.UserConfig.DiagramLibrary;
                                DiagramCollection targetCollection = library.Get(name);

                                foreach (ObservableDiagram od in itemsToMove)
                                {
                                    Collection.Remove(od.Diagram);
                                    targetCollection.Add(od.Diagram);
                                }

                                Redraw(newCollection);
                            }
                            catch (Exception ex)
                            {
                                ExceptionUtils.HandleException(ex);
                            }
                        }
                        , Name));
                    }
                    catch (Exception ex)
                    {
                        ExceptionUtils.HandleException(ex);
                    }
                }, () =>
                {
                    return SelectedDiagrams.Count > 0;
                });
            }
        }

        #endregion

        #region ExportSelected

        public string ExportSelectedLabel
        {
            get
            {
                return Strings.ExportLabel;
            }
        }

        public string ExportSelectedToolTip
        {
            get
            {
                int count = SelectedDiagrams.Count;
                return string.Format(count == 1 ? Strings.ObservableDiagramLibraryNodeExportSelectedToolTipSingleFormat : Strings.ObservableDiagramLibraryNodeExportSelectedToolTipPluralFormat, count);
            }
        }

        public RelayCommand ExportSelected
        {
            get
            {
                return new RelayCommand(() =>
                {
                    try
                    {
                        Messenger.Default.Send<ShowDiagramExportMessage>(new ShowDiagramExportMessage(SelectedDiagrams, Name));
                    }
                    catch (Exception ex)
                    {
                        ExceptionUtils.HandleException(ex);
                    }
                }, () =>
                {
                    return SelectedDiagrams.Count > 0;
                });
            }
        }

        #endregion

        #region DeleteSelected

        public string DeleteSelectedLabel
        {
            get
            {
                return Strings.DeleteLabel;
            }
        }

        public string DeleteSelectedToolTip
        {
            get
            {
                int count = SelectedDiagrams.Count;
                return string.Format(count == 1 ? Strings.ObservableDiagramLibraryNodeDeleteSelectedToolTipSingleFormat : Strings.ObservableDiagramLibraryNodeDeleteSelectedToolTipPluralFormat, count);
            }
        }

        public RelayCommand DeleteSelected
        {
            get
            {
                return new RelayCommand(() =>
                {
                    try
                    {
                        int count = SelectedDiagrams.Count;
                        string message = string.Format(count < 2 ? Strings.DiagramLibraryDeleteSelectedDiagramsPromptSingleFormat : Strings.DiagramLibraryDeleteSelectedDiagramsPromptPluralFormat, count);

                        Messenger.Default.Send<ConfirmationMessage>(new ConfirmationMessage(message, (confirmed) =>
                        {
                            if (confirmed)
                            {
                                List<ObservableDiagram> itemsToDelete = new List<ObservableDiagram>(SelectedDiagrams);

                                DiagramCollection collection = Library.Get(Path, Name);
                                foreach (ObservableDiagram od in itemsToDelete)
                                {
                                    collection.Remove(od.Diagram);
                                    Diagrams.Remove(od);
                                }
                            }
                        }, "confirmation.diagramlibrarynode.deletediagram"));
                    }
                    catch (Exception ex)
                    {
                        ExceptionUtils.HandleException(ex);
                    }
                }, () =>
                {
                    return SelectedDiagrams.Count > 0;
                });
            }
        }

        #endregion

        #region EditCollectionStyle

        public string EditCollectionStyleLabel
        {
            get
            {
                return Strings.EditStyleLabel;
            }
        }

        public string EditCollectionStyleToolTip
        {
            get
            {
                return string.Format(Strings.ObservableDiagramLibraryNodeEditCollectionStyleToolTipFormat, Name);
            }
        }

        public RelayCommand EditCollectionStyle
        {
            get
            {
                return CollectionStyle.ShowEditor;
            }
        }

        #endregion

        internal DiagramLibrary Library { get; private set; }

        internal DiagramCollection Collection
        {
            get
            {
                return _collection ?? (_collection = Library.Get(Path, Name));
            }
        }
        private DiagramCollection _collection;

        public ObservableDiagramStyle CollectionStyle
        {
            get
            {
                if (null == _collectionStyle)
                {
                    _collectionStyle = new ObservableDiagramStyle(Collection.Style);
                    _collectionStyle.PostEditCallback = (changed) =>
                    {
                        if (changed)
                        {
                            Redraw();
                        }                        
                    };
                }
                return _collectionStyle;
            }
        }
        private ObservableDiagramStyle _collectionStyle;

        private bool _firstLoad = true;

        private Action<bool> _redrawCallback;

        public ObservableDiagramLibraryNode(string path, string name, DiagramLibrary library, Action<bool> redrawCallback = null) : base()
        {
            if (null == library)
            {
                throw new ArgumentNullException("library");
            }

            Path = path;
            Name = name;

            Library = library;

            _redrawCallback = redrawCallback;

            Diagrams = new ObservableCollection<ObservableDiagram>();

            SelectedDiagrams = new ObservableCollection<ObservableDiagram>();
            SelectedDiagrams.CollectionChanged += SelectedDiagrams_CollectionChanged;
        }

        private void SelectedDiagrams_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            UpdateCommands();
        }

        private void Redraw(bool reload = false)
        {
            _redrawCallback?.Invoke(reload);
        }

        internal void RedrawDiagrams()
        {
            if (!_firstLoad)
            {
                Diagrams.Clear();
                _firstLoad = true;
                RaisePropertyChanged("Diagrams");
            }
        }

        private ObservableDiagram CreateObservableDiagram()
        {
            return CreateObservableDiagram(new Diagram(Collection.Style));
        }

        private ObservableDiagram CreateObservableDiagram(Diagram diagram)
        {
            ObservableDiagram od = new ObservableDiagram(diagram);
            od.PostEditCallback = GetDiagramPostEditCallback(od);
            return od;
        }

        private Action<bool> GetDiagramPostEditCallback(ObservableDiagram od)
        {
            ObservableDiagram originalObservableDiagram = od;
            Diagram originalDiagram = od.Diagram;

            return (changed) =>
            {
                if (changed)
                {
                    if (Collection.Contains(originalDiagram))
                    {
                        // We edited an existing diagram, so we need to replace the pre-edited Diagram still in the library
                        // with the newly minted Diagram from the editor
                        Collection.Replace(originalDiagram, originalObservableDiagram.Diagram);
                    }
                    else
                    {
                        // This was a new diagram, so we need to add it to the library collection
                        Collection.Add(originalObservableDiagram.Diagram);
                        // Then add it to the list of visible diagrams
                        Diagrams.Add(originalObservableDiagram);
                    }

                    // Now we need to refresh the individual image
                    originalObservableDiagram.Refresh();

                    // Now that the internal Diagram has changed, we need to rebuild the callback so that the new Diagram is
                    // cached correctly for any future calls to edit this od
                    originalObservableDiagram.PostEditCallback = GetDiagramPostEditCallback(originalObservableDiagram);
                }
            };
        }

        private void UpdateCommands()
        {
            RaisePropertyChanged("EditSelected");
            RaisePropertyChanged("CloneSelected");
            RaisePropertyChanged("CloneSelectedToolTip");
            RaisePropertyChanged("CopySelected");
            RaisePropertyChanged("CopySelectedToolTip");
            RaisePropertyChanged("MoveSelected");
            RaisePropertyChanged("MoveSelectedToolTip");
            RaisePropertyChanged("ExportSelected");
            RaisePropertyChanged("ExportSelectedToolTip");
            RaisePropertyChanged("DeleteSelected");
            RaisePropertyChanged("DeleteSelectedToolTip");
            RaisePropertyChanged("ResetStylesSelected");
            RaisePropertyChanged("ResetStylesSelectedToolTip");
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
