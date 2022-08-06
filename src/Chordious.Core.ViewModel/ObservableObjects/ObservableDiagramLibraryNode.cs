﻿// Copyright (c) Jon Thysell <http://jonthysell.com>
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

using Chordious.Core.ViewModel.Resources;

namespace Chordious.Core.ViewModel
{
    public class ObservableDiagramLibraryNode : ObservableObject
    {
        public static AppViewModel AppVM
        {
            get
            {
                return AppViewModel.Instance;
            }
        }

        public string Path { get; private set; } = null;

        public string Name { get; private set; } = null;

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
        }
        private readonly ObservableCollection<ObservableDiagram> _diagrams = null;

        public ObservableCollection<ObservableDiagram> SelectedDiagrams { get; private set; } = null;

        #region CreateDiagram

        public static string CreateDiagramLabel
        {
            get
            {
                return Strings.NewLabel;
            }
        }

        public static string CreateDiagramToolTip
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
                return _createDiagram ??= new RelayCommand(() =>
                {
                    try
                    {
                        ObservableDiagram od = CreateObservableDiagram();

                        StrongReferenceMessenger.Default.Send(new ShowDiagramEditorMessage(od, true, od.PostEditCallback));
                    }
                    catch (Exception ex)
                    {
                        ExceptionUtils.HandleException(ex);
                    }
                });
            }
        }
        private RelayCommand _createDiagram;

        #endregion

        #region EditSelected

        public static string EditSelectedLabel
        {
            get
            {
                return Strings.EditLabel;
            }
        }

        public static string EditSelectedToolTip
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
                return _editSelected ??= new RelayCommand(() =>
                {
                    try
                    {
                        StrongReferenceMessenger.Default.Send(new ChordiousMessage(Strings.DiagramLibraryOnlyOneDiagramCanBeEditedMessage));
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
        private RelayCommand _editSelected;

        #endregion

        #region SendSelectedToClipboard

        public static string SendSelectedImageToClipboardLabel
        {
            get
            {
                return Strings.ObservableDiagramLibraryNodeSendSelectedImageToClipboardLabel;
            }
        }

        public static string SendSelectedImageToClipboardToolTip
        {
            get
            {
                return Strings.ObservableDiagramLibraryNodeSendSelectedImageToClipboardToolTip;
            }
        }

        public RelayCommand SendSelectedImageToClipboard
        {
            get
            {
                // Use the built-in ObservableDiagram's RelayCommand if it's available
                if (SelectedDiagrams.Count == 1)
                {
                    return SelectedDiagrams[0].SendImageToClipboard;
                }

                // If a single diagram isn't selected, throw an error
                return _sendSelectedToClipboard ??= new RelayCommand(() =>
                {
                    try
                    {
                        StrongReferenceMessenger.Default.Send(new ChordiousMessage(Strings.DiagramLibraryOnlyOneDiagramCanBeCopiedToClipboardMessage));
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

        public static string SendSelectedScaledImageToClipboardLabel
        {
            get
            {
                return Strings.ObservableDiagramLibraryNodeSendSelectedScaledImageToClipboardLabel;
            }
        }

        public static string SendSelectedScaledImageToClipboardToolTip
        {
            get
            {
                return Strings.ObservableDiagramLibraryNodeSendSelectedScaledImageToClipboardToolTip;
            }
        }

        public RelayCommand SendSelectedScaledImageToClipboard
        {
            get
            {
                // Use the built-in ObservableDiagram's RelayCommand if it's available
                if (SelectedDiagrams.Count == 1)
                {
                    return SelectedDiagrams[0].SendScaledImageToClipboard;
                }

                // If a single diagram isn't selected, throw an error
                return _sendSelectedToClipboard ??= new RelayCommand(() =>
                {
                    try
                    {
                        StrongReferenceMessenger.Default.Send(new ChordiousMessage(Strings.DiagramLibraryOnlyOneDiagramCanBeCopiedToClipboardMessage));
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

        public static string SendSelectedTextToClipboardLabel
        {
            get
            {
                return Strings.ObservableDiagramLibraryNodeSendSelectedTextToClipboardLabel;
            }
        }

        public static string SendSelectedTextToClipboardToolTip
        {
            get
            {
                return Strings.ObservableDiagramLibraryNodeSendSelectedTextToClipboardToolTip;
            }
        }

        public RelayCommand SendSelectedTextToClipboard
        {
            get
            {
                // Use the built-in ObservableDiagram's RelayCommand if it's available
                if (SelectedDiagrams.Count == 1)
                {
                    return SelectedDiagrams[0].SendTextToClipboard;
                }

                // If a single diagram isn't selected, throw an error
                return _sendSelectedToClipboard ??= new RelayCommand(() =>
                {
                    try
                    {
                        StrongReferenceMessenger.Default.Send(new ChordiousMessage(Strings.DiagramLibraryOnlyOneDiagramCanBeCopiedToClipboardMessage));
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

        private RelayCommand _sendSelectedToClipboard;

        #endregion

        #region ResetStylesSelected

        public static string ResetStylesSelectedLabel
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
                return _resetStylesSelected ??= new RelayCommand(() =>
                {
                    try
                    {
                        int count = SelectedDiagrams.Count;
                        string message = string.Format(count < 2 ? Strings.ObservableDiagramLibraryNodeResetStylesSelectedPromptSingleFormat : Strings.ObservableDiagramLibraryNodeResetStylesSelectedPromptPluralFormat, count);

                        StrongReferenceMessenger.Default.Send(new ConfirmationMessage(message, (confirmed) =>
                        {
                            try
                            {
                                if (confirmed)
                                {
                                    foreach (ObservableDiagram od in SelectedDiagrams)
                                    {
                                        od.ResetStyles();
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                ExceptionUtils.HandleException(ex);
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
        private RelayCommand _resetStylesSelected;

        #endregion

        #region CloneSelected

        public static string CloneSelectedLabel
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
                return _cloneSelected ??= new RelayCommand(() =>
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
        private RelayCommand _cloneSelected;

        #endregion

        #region CopySelected

        public static string CopySelectedLabel
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

        public RelayCommand<string> CopySelected
        {
            get
            {
                return _copySelected ??= new RelayCommand<string>((defaultCollectionName) =>
                {
                    try
                    {
                        CopyDiagrams(new List<ObservableDiagram>(SelectedDiagrams), defaultCollectionName);
                    }
                    catch (Exception ex)
                    {
                        ExceptionUtils.HandleException(ex);
                    }
                }, (defaultCollectionName) =>
                {
                    return SelectedDiagrams.Count > 0;
                });
            }
        }
        private RelayCommand<string> _copySelected;

        #endregion

        #region MoveSelected

        public static string MoveSelectedLabel
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

        public RelayCommand<string> MoveSelected
        {
            get
            {
                return _moveSelected ??= new RelayCommand<string>((defaultCollectionName) =>
                {
                    try
                    {
                        MoveDiagrams(new List<ObservableDiagram>(SelectedDiagrams), defaultCollectionName);
                    }
                    catch (Exception ex)
                    {
                        ExceptionUtils.HandleException(ex);
                    }
                }, (defaultCollectionName) =>
                {
                    return SelectedDiagrams.Count > 0;
                });
            }
        }
        private RelayCommand<string> _moveSelected;

        #endregion

        #region ExportSelected

        public static string ExportSelectedLabel
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
                return _exportSelected ??= new RelayCommand(() =>
                {
                    try
                    {
                        StrongReferenceMessenger.Default.Send(new ShowDiagramExportMessage(SelectedDiagrams, Name));
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
        private RelayCommand _exportSelected;

        #endregion

        #region DeleteSelected

        public static string DeleteSelectedLabel
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
                return _deleteSelected ??= new RelayCommand(() =>
                {
                    try
                    {
                        int count = SelectedDiagrams.Count;
                        string message = string.Format(count < 2 ? Strings.DiagramLibraryDeleteSelectedDiagramsPromptSingleFormat : Strings.DiagramLibraryDeleteSelectedDiagramsPromptPluralFormat, count);

                        StrongReferenceMessenger.Default.Send(new ConfirmationMessage(message, (confirmed) =>
                        {
                            try
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
                            }
                            catch (Exception ex)
                            {
                                ExceptionUtils.HandleException(ex);
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
        private RelayCommand _deleteSelected;

        #endregion

        #region CopyNode

        public static string CopyNodeLabel
        {
            get
            {
                return Strings.CopyLabel;
            }
        }

        public static string CopyNodeToolTip
        {
            get
            {
                return Strings.ObservableDiagramLibraryNodeCopyNodeToolTip;
            }
        }

        public RelayCommand<string> CopyNode
        {
            get
            {
                return _copyNode ??= new RelayCommand<string>((defaultCollectionName) =>
                {
                    try
                    {
                        CopyDiagrams(new List<ObservableDiagram>(Diagrams), defaultCollectionName);
                    }
                    catch (Exception ex)
                    {
                        ExceptionUtils.HandleException(ex);
                    }
                });
            }
        }
        private RelayCommand<string> _copyNode;

        #endregion

        #region MergeNode

        public static string MergeNodeLabel
        {
            get
            {
                return Strings.MergeLabel;
            }
        }

        public static string MergeNodeToolTip
        {
            get
            {
                return Strings.ObservableDiagramLibraryNodeMergeNodeToolTip;
            }
        }

        public RelayCommand<string> MergeNode
        {
            get
            {
                return _mergeNode ??= new RelayCommand<string>((defaultCollectionName) =>
                {
                    try
                    {
                        MoveDiagrams(new List<ObservableDiagram>(Diagrams), defaultCollectionName);
                    }
                    catch (Exception ex)
                    {
                        ExceptionUtils.HandleException(ex);
                    }
                });
            }
        }
        private RelayCommand<string> _mergeNode;

        #endregion

        #region EditCollectionStyle

        public static string EditCollectionStyleLabel
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

        internal DiagramLibrary Library { get; private set; } = null;

        internal DiagramCollection Collection
        {
            get
            {
                return _collection ??= Library.Get(Path, Name);
            }
        }
        private DiagramCollection _collection;

        public ObservableDiagramStyle CollectionStyle
        {
            get
            {
                if (_collectionStyle is null)
                {
                    _collectionStyle = new ObservableDiagramStyle(Collection.Style)
                    {
                        PostEditCallback = (changed) =>
                        {
                            if (changed)
                            {
                                Redraw();
                            }
                        }
                    };
                }
                return _collectionStyle;
            }
        }
        private ObservableDiagramStyle _collectionStyle;

        private bool _firstLoad = true;

        private readonly Action<bool> _redrawCallback;

        public ObservableDiagramLibraryNode(string path, string name, DiagramLibrary library, Action<bool> redrawCallback = null) : base()
        {
            Path = path;
            Name = name;

            Library = library ?? throw new ArgumentNullException(nameof(library));

            _redrawCallback = redrawCallback;

            _diagrams = new ObservableCollection<ObservableDiagram>();

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
                OnPropertyChanged(nameof(Diagrams));
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

        private void CopyDiagrams(IEnumerable<ObservableDiagram> itemsToCopy, string destinationName)
        {
            void performCopy(string name, bool newCollection)
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

            if (!string.IsNullOrWhiteSpace(destinationName))
            {
                // Target was specified
                performCopy(destinationName.Trim(), false);
            }
            else
            {
                // Prompt for a target
                StrongReferenceMessenger.Default.Send(new ShowDiagramCollectionSelectorMessage(performCopy));
            }
        }

        private void MoveDiagrams(IEnumerable<ObservableDiagram> itemsToMove, string destinationName)
        {
            void performMove(string name, bool newCollection)
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

                    PromptToDeleteIfEmpty();
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }

            if (!string.IsNullOrWhiteSpace(destinationName))
            {
                // Target was specified
                performMove(destinationName.Trim(), false);
            }
            else
            {
                // Prompt for a target
                StrongReferenceMessenger.Default.Send(new ShowDiagramCollectionSelectorMessage(performMove));
            }
        }

        private void PromptToDeleteIfEmpty()
        {
            if (Collection.Count == 0)
            {
                StrongReferenceMessenger.Default.Send(new ConfirmationMessage(string.Format(Strings.ObservableDiagramLibraryNodeDeleteEmptyPromptFormat, Name), (confirmed) =>
                {
                    try
                    {
                        if (confirmed)
                        {
                            AppVM.UserConfig.DiagramLibrary.Remove(Path, Name);
                            Redraw(true);
                        }
                    }
                    catch (Exception ex)
                    {
                        ExceptionUtils.HandleException(ex);
                    }

                }));
            }
        }

        private void UpdateCommands()
        {
            OnPropertyChanged(nameof(EditSelected));
            _editSelected?.NotifyCanExecuteChanged();
            OnPropertyChanged(nameof(SendSelectedImageToClipboard));
            OnPropertyChanged(nameof(SendSelectedScaledImageToClipboard));
            OnPropertyChanged(nameof(SendSelectedTextToClipboard));
            _sendSelectedToClipboard?.NotifyCanExecuteChanged();
            CloneSelected.NotifyCanExecuteChanged();
            OnPropertyChanged(nameof(CloneSelectedToolTip));
            CopySelected.NotifyCanExecuteChanged();
            OnPropertyChanged(nameof(CopySelectedToolTip));
            MoveSelected.NotifyCanExecuteChanged();
            OnPropertyChanged(nameof(MoveSelectedToolTip));
            ExportSelected.NotifyCanExecuteChanged();
            OnPropertyChanged(nameof(ExportSelectedToolTip));
            DeleteSelected.NotifyCanExecuteChanged();
            OnPropertyChanged(nameof(DeleteSelectedToolTip));
            ResetStylesSelected.NotifyCanExecuteChanged();
            OnPropertyChanged(nameof(ResetStylesSelectedToolTip));
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
