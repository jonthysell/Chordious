// Copyright (c) Jon Thysell <http://jonthysell.com>
// Licensed under the MIT License.

using System;
using System.Collections.Specialized;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

using Chordious.Core.ViewModel.Resources;

namespace Chordious.Core.ViewModel
{
    public class ChordFinderViewModel : ObservableObject, IIdle
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
                return Strings.ChordFinderTitle;
            }
        }

        public bool IsIdle
        {
            get
            {
                return _isIdle;
            }
            private set
            {
                _isIdle = value;
                OnPropertyChanged(nameof(IsIdle));
                SearchAsync.NotifyCanExecuteChanged();
                SetAsDefaults.NotifyCanExecuteChanged();
            }
        }
        private bool _isIdle = true;

        #region Options

        #region Instruments

        public string SelectedInstrumentLabel
        {
            get
            {
                return Strings.FinderSelectedInstrumentLabel;
            }
        }

        public string SelectedInstrumentToolTip
        {
            get
            {
                return Strings.FinderSelectedInstrumentToolTip;
            }
        }

        public ObservableInstrument SelectedInstrument
        {
            get
            {
                return _instrument;
            }
            set
            {
                try
                {
                    _instrument = value;
                    SelectedTuning = null;
                    Tunings = null;

                    if (value is not null)
                    {
                        Tunings = SelectedInstrument.GetTunings();
                        if (Tunings is not null && Tunings.Count > 0)
                        {
                            SelectedTuning = Tunings[0];
                            Options.SetTarget(SelectedInstrument.Instrument, SelectedTuning.Tuning);
                        }
                    }
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
                finally
                {
                    OnPropertyChanged(nameof(SelectedInstrument));
                    SearchAsync.NotifyCanExecuteChanged();
                    SetAsDefaults.NotifyCanExecuteChanged();
                }
            }
        }
        private ObservableInstrument _instrument;

        public ObservableCollection<ObservableInstrument> Instruments
        {
            get
            {
                return _instruments;
            }
            private set
            {
                _instruments = value;
                OnPropertyChanged(nameof(Instruments));
            }
        }
        private ObservableCollection<ObservableInstrument> _instruments;

        #endregion

        #region Tunings

        public string SelectedTuningLabel
        {
            get
            {
                return Strings.FinderSelectedTuningLabel;
            }
        }

        public string SelectedTuningToolTip
        {
            get
            {
                return Strings.FinderSelectedTuningToolTip;
            }
        }

        public ObservableTuning SelectedTuning
        {
            get
            {
                return _tuning;
            }
            set
            {
                try
                {
                    _tuning = value;
                    if (value is not null)
                    {
                        Options.SetTarget(SelectedInstrument.Instrument, SelectedTuning.Tuning);
                    }
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
                finally
                {
                    OnPropertyChanged(nameof(SelectedTuning));
                    SearchAsync.NotifyCanExecuteChanged();
                    SetAsDefaults.NotifyCanExecuteChanged();
                }
            }
        }
        private ObservableTuning _tuning;

        public ObservableCollection<ObservableTuning> Tunings
        {
            get
            {
                return _tunings;
            }
            private set
            {
                _tunings = value;
                OnPropertyChanged(nameof(Tunings));
            }
        }
        private ObservableCollection<ObservableTuning> _tunings;

        #endregion

        public string ShowInstrumentManagerLabel
        {
            get
            {
                return Strings.ShowInstrumentManagerLabel;
            }
        }

        public string ShowInstrumentManagerToolTip
        {
            get
            {
                return Strings.ShowInstrumentManagerToolTip;
            }
        }

        public RelayCommand ShowInstrumentManager
        {
            get
            {
                return _showInstrumentManager ??= new RelayCommand(() =>
                {
                    try
                    {
                        StrongReferenceMessenger.Default.Send(new ShowInstrumentManagerMessage(() =>
                        {
                            RefreshInstruments(SelectedInstrument?.Instrument, SelectedTuning?.Tuning);
                        }));
                    }
                    catch (Exception ex)
                    {
                        ExceptionUtils.HandleException(ex);
                    }
                });
            }
        }
        private RelayCommand _showInstrumentManager;

        #region RootNote

        public string SelectedRootNoteLabel
        {
            get
            {
                return Strings.FinderSelectedRootNoteLabel;
            }
        }

        public string SelectedRootNoteToolTip
        {
            get
            {
                return Strings.FinderSelectedRootNoteToolTip;
            }
        }

        public string SelectedRootNote
        {
            get
            {
                return NoteUtils.ToString(Options.RootNote);
            }
            set
            {
                try
                {
                    Options.RootNote = NoteUtils.ParseNote(value);
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
                finally
                {
                    OnPropertyChanged(nameof(SelectedRootNote));
                }
            }
        }

        public ObservableCollection<string> RootNotes
        {
            get
            {
                return ObservableEnums.GetNotes();
            }
        }

        #endregion

        #region ChordQuality

        public string SelectedChordQualityLabel
        {
            get
            {
                return Strings.ChordFinderOptionsSelectedChordQualityLabel;
            }
        }

        public string SelectedChordQualityToolTip
        {
            get
            {
                return Strings.ChordFinderOptionsSelectedChordQualityToolTip;
            }
        }

        public ObservableChordQuality SelectedChordQuality
        {
            get
            {
                return _chordQuality;
            }
            set
            {
                try
                {
                    _chordQuality = value;
                    if (value is not null)
                    {
                        Options.SetTarget(Options.RootNote, SelectedChordQuality.ChordQuality);
                    }
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
                finally
                {
                    OnPropertyChanged(nameof(SelectedChordQuality));
                    SearchAsync.NotifyCanExecuteChanged();
                    SetAsDefaults.NotifyCanExecuteChanged();
                }
            }
        }
        private ObservableChordQuality _chordQuality;

        public ObservableCollection<ObservableChordQuality> ChordQualities
        {
            get
            {
                return _chordQualities;
            }
            private set
            {
                _chordQualities = value;
                OnPropertyChanged(nameof(ChordQualities));
            }
        }
        private ObservableCollection<ObservableChordQuality> _chordQualities;

        public string ShowChordQualityManagerLabel
        {
            get
            {
                return Strings.ShowChordQualityManagerLabel;
            }
        }

        public string ShowChordQualityManagerToolTip
        {
            get
            {
                return Strings.ShowChordQualityManagerToolTip;
            }
        }

        public RelayCommand ShowChordQualityManager
        {
            get
            {
                return _showChordQualityManager ??= new RelayCommand(() =>
                {
                    try
                    {
                        StrongReferenceMessenger.Default.Send(new ShowChordQualityManagerMessage(() =>
                        {
                            try
                            {
                                RefreshChordQualities(SelectedChordQuality?.ChordQuality);
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
        private RelayCommand _showChordQualityManager;

        #endregion

        public string NumFretsLabel
        {
            get
            {
                return Strings.FinderOptionsNumFretsLabel;
            }
        }

        public string NumFretsToolTip
        {
            get
            {
                return Strings.FinderOptionsNumFretsToolTip;
            }
        }

        public int NumFrets
        {
            get
            {
                return Options.NumFrets;
            }
            set
            {
                try
                {
                    Options.NumFrets = value;
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
                finally
                {
                    OnPropertyChanged(nameof(NumFrets));
                    OnPropertyChanged(nameof(MaxReach));
                }
            }
        }

        public string MaxReachLabel
        {
            get
            {
                return Strings.FinderOptionsMaxReachLabel;
            }
        }

        public string MaxReachToolTip
        {
            get
            {
                return Strings.FinderOptionsMaxReachToolTip;
            }
        }

        public int MaxReach
        {
            get
            {
                return Options.MaxReach;
            }
            set
            {
                try
                {
                    Options.MaxReach = value;
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
                finally
                {
                    OnPropertyChanged(nameof(MaxReach));
                    OnPropertyChanged(nameof(NumFrets));
                }
            }
        }

        public string MaxFretLabel
        {
            get
            {
                return Strings.FinderOptionsMaxFretLabel;
            }
        }

        public string MaxFretToolTip
        {
            get
            {
                return Strings.FinderOptionsMaxFretToolTip;
            }
        }

        public int MaxFret
        {
            get
            {
                return Options.MaxFret;
            }
            set
            {
                try
                {
                    Options.MaxFret = value;
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
                finally
                {
                    OnPropertyChanged(nameof(MaxFret));
                }
            }
        }

        public string AllowOpenStringsLabel
        {
            get
            {
                return Strings.FinderOptionsAllowOpenStringsLabel;
            }
        }

        public string AllowOpenStringsToolTip
        {
            get
            {
                return Strings.FinderOptionsAllowOpenStringsToolTip;
            }
        }

        public bool AllowOpenStrings
        {
            get
            {
                return Options.AllowOpenStrings;
            }
            set
            {
                Options.AllowOpenStrings = value;
                OnPropertyChanged(nameof(AllowOpenStrings));
            }
        }

        public string AllowMutedStringsLabel
        {
            get
            {
                return Strings.FinderOptionsAllowMutedStringsLabel;
            }
        }

        public string AllowMutedStringsToolTip
        {
            get
            {
                return Strings.FinderOptionsAllowMutedStringsToolTip;
            }
        }

        public bool AllowMutedStrings
        {
            get
            {
                return Options.AllowMutedStrings;
            }
            set
            {
                Options.AllowMutedStrings = value;
                OnPropertyChanged(nameof(AllowMutedStrings));
            }
        }

        public string AllowRootlessChordsLabel
        {
            get
            {
                return Strings.ChordFinderOptionsAllowRootlessChordsLabel;
            }
        }

        public string AllowRootlessChordsToolTip
        {
            get
            {
                return Strings.ChordFinderOptionsAllowRootlessChordsToolTip;
            }
        }

        public bool AllowRootlessChords
        {
            get
            {
                return Options.AllowRootlessChords;
            }
            set
            {
                Options.AllowRootlessChords = value;
                OnPropertyChanged(nameof(AllowRootlessChords));
            }
        }

        public string AllowPartialChordsLabel
        {
            get
            {
                return Strings.ChordFinderOptionsAllowPartialChordsLabel;
            }
        }

        public string AllowPartialChordsToolTip
        {
            get
            {
                return Strings.ChordFinderOptionsAllowPartialChordsToolTip;
            }
        }

        public bool AllowPartialChords
        {
            get
            {
                return Options.AllowPartialChords;
            }
            set
            {
                Options.AllowPartialChords = value;
                OnPropertyChanged(nameof(AllowPartialChords));
            }
        }

        #endregion

        #region Styles

        public string AddTitleLabel
        {
            get
            {
                return Strings.FinderOptionsAddTitleLabel;
            }
        }

        public string AddTitleToolTip
        {
            get
            {
                return Strings.FinderOptionsAddTitleToolTip;
            }
        }

        public bool AddTitle
        {
            get
            {
                return Style.AddTitle;
            }
            set
            {
                Style.AddTitle = value;
                OnPropertyChanged(nameof(AddTitle));
            }
        }

        public string MirrorResultsLabel
        {
            get
            {
                return Strings.FinderOptionsMirrorResultsLabel;
            }
        }

        public string MirrorResultsToolTip
        {
            get
            {
                return Strings.FinderOptionsMirrorResultsToolTip;
            }
        }

        public bool MirrorResults
        {
            get
            {
                return Style.MirrorResults;
            }
            set
            {
                Style.MirrorResults = value;
                OnPropertyChanged(nameof(MirrorResults));
            }
        }

        public string SelectedBarreTypeOptionLabel
        {
            get
            {
                return Strings.ChordFinderOptionsBarreTypeLabel;
            }
        }

        public string SelectedBarreTypeOptionToolTip
        {
            get
            {
                return Strings.ChordFinderOptionsBarreTypeToolTip;
            }
        }

        public int SelectedBarreTypeOptionIndex
        {
            get
            {
                return (int)Style.BarreTypeOption;
            }
            set
            {
                Style.BarreTypeOption = (BarreTypeOption)(value);
                OnPropertyChanged(nameof(SelectedBarreTypeOptionIndex));
            }
        }

        public ObservableCollection<string> BarreTypeOptions
        {
            get
            {
                return ObservableEnums.GetBarreTypeOptions();
            }
        }

        public string AddRootNotesLabel
        {
            get
            {
                return Strings.FinderOptionsAddRootNotesLabel;
            }
        }

        public string AddRootNotesToolTip
        {
            get
            {
                return Strings.FinderOptionsAddRootNotesToolTip;
            }
        }

        public bool AddRootNotes
        {
            get
            {
                return Style.AddRootNotes;
            }
            set
            {
                Style.AddRootNotes = value;
                OnPropertyChanged(nameof(AddRootNotes));
            }
        }

        public string SelectedMarkTextOptionLabel
        {
            get
            {
                return Strings.FinderOptionsMarkTextLabel;
            }
        }

        public string SelectedMarkTextOptionToolTip
        {
            get
            {
                return Strings.FinderOptionsMarkTextToolTip;
            }
        }

        public int SelectedMarkTextOptionIndex
        {
            get
            {
                return (int)Style.MarkTextOption;
            }
            set
            {
                Style.MarkTextOption = (MarkTextOption)(value);
                OnPropertyChanged(nameof(SelectedMarkTextOptionIndex));
            }
        }

        public ObservableCollection<string> MarkTextOptions
        {
            get
            {
                return ObservableEnums.GetMarkTextOptions();
            }
        }

        public string AddBottomMarksLabel
        {
            get
            {
                return Strings.ChordFinderOptionsAddBottomMarksLabel;
            }
        }

        public string AddBottomMarksToolTip
        {
            get
            {
                return Strings.ChordFinderOptionsAddBottomMarksToolTip;
            }
        }

        public bool AddBottomMarks
        {
            get
            {
                return Style.AddBottomMarks;
            }
            set
            {
                Style.AddBottomMarks = value;
                OnPropertyChanged(nameof(AddBottomMarks));
            }
        }

        public int SelectedBottomMarkTextOptionIndex
        {
            get
            {
                return (int)Style.BottomMarkTextOption;
            }
            set
            {
                Style.BottomMarkTextOption = (MarkTextOption)(value);
                OnPropertyChanged(nameof(SelectedBottomMarkTextOptionIndex));
            }
        }

        public string SelectedBottomMarkTextOptionLabel
        {
            get
            {
                return Strings.ChordFinderOptionsBottomMarkTextLabel;
            }
        }

        public string SelectedBottomMarkTextOptionToolTip
        {
            get
            {
                return Strings.ChordFinderOptionsBottomMarkTextToolTip;
            }
        }

        public ObservableCollection<string> BottomMarkTextOptions
        {
            get
            {
                return ObservableEnums.GetMarkTextOptions();
            }
        }

        public string SelectedFretLabelSideLabel
        {
            get
            {
                return Strings.FinderOptionsFretLabelSideLabel;
            }
        }

        public string SelectedFretLabelSideToolTip
        {
            get
            {
                return Strings.FinderOptionsFretLabelSideToolTip;
            }
        }

        public int SelectedFretLabelSideIndex
        {
            get
            {
                return (int)Style.FretLabelSide;
            }
            set
            {
                Style.FretLabelSide = (FretLabelSide)(value);
                OnPropertyChanged(nameof(SelectedFretLabelSideIndex));
            }
        }

        public ObservableCollection<string> FretLabelSides
        {
            get
            {
                return ObservableEnums.GetFretLabelSides();
            }
        }

        #endregion

        public string SetAsDefaultsLabel
        {
            get
            {
                return Strings.FinderOptionsSetAsDefaultsLabel;
            }
        }

        public string SetAsDefaultsToolTip
        {
            get
            {
                return Strings.FinderOptionsSetAsDefaultsToolTip;
            }
        }

        public RelayCommand SetAsDefaults
        {
            get
            {
                return _setAsDefaults ??= new RelayCommand(() =>
                {
                    try
                    {
                        StrongReferenceMessenger.Default.Send(new ConfirmationMessage(Strings.FinderOptionsSetAsDefaultsPromptMessage, (confirmed) =>
                        {
                            try
                            {
                                if (confirmed)
                                {
                                    Options.Settings.SetParent();
                                    Style.Settings.SetParent();
                                    RefreshSettings();
                                }
                            }
                            catch (Exception ex)
                            {
                                ExceptionUtils.HandleException(ex);
                            }
                        }, "confirmation.chordfinder.setasdefaults"));
                    }
                    catch (Exception ex)
                    {
                        ExceptionUtils.HandleException(ex);
                    }
                }, () =>
                {
                    return CanSearch();
                });
            }
        }
        private RelayCommand _setAsDefaults;

        public string ResetToDefaultsLabel
        {
            get
            {
                return Strings.FinderOptionsResetToDefaultsLabel;
            }
        }

        public string ResetToDefaultsToolTip
        {
            get
            {
                return Strings.FinderOptionsResetToDefaultsToolTip;
            }
        }

        public RelayCommand ResetToDefaults
        {
            get
            {
                return _resetToDefaults ??= new RelayCommand(() =>
                {
                    try
                    {
                        StrongReferenceMessenger.Default.Send(new ConfirmationMessage(Strings.FinderOptionsResetToDefaultsPromptMessage, (confirmed) =>
                        {
                            try
                            {
                                if (confirmed)
                                {
                                    Options.Settings.Clear();
                                    Style.Settings.Clear();
                                    RefreshSettings();
                                }
                            }
                            catch (Exception ex)
                            {
                                ExceptionUtils.HandleException(ex);
                            }
                        }, "confirmation.chordfinder.resettodefaults"));
                    }
                    catch (Exception ex)
                    {
                        ExceptionUtils.HandleException(ex);
                    }
                });
            }
        }
        private RelayCommand _resetToDefaults;

        public string SearchAsyncLabel
        {
            get
            {
                return Strings.FinderSearchLabel;
            }
        }

        public string SearchAsyncToolTip
        {
            get
            {
                return Strings.FinderSearchToolTip;
            }
        }

        public RelayCommand SearchAsync
        {
            get
            {
                return _searchAsync ??= new RelayCommand(async () =>
                {
                    _searchAsyncCancellationTokenSource = new CancellationTokenSource();

                    try
                    {
                        IsIdle = false;

                        int numStrings = SelectedInstrument.NumStrings;
                        int numUniqueNotes = SelectedChordQuality.ChordQuality.GetUniqueNotes(NoteUtils.ToInternalNote(Options.RootNote)).Length;

                        if (numUniqueNotes > numStrings)
                        {
                            int numUniqueNotesNoRoot = SelectedChordQuality.ChordQuality.GetUniqueNotes(NoteUtils.ToInternalNote(Options.RootNote), false).Length;
                            if (numUniqueNotesNoRoot <= numStrings && !Options.AllowRootlessChords)
                            {
                                StrongReferenceMessenger.Default.Send(new ChordiousMessage(Strings.ChordFinderNotEnoughStringsTryRootlessMessage));
                                return;
                            }
                            else if (numUniqueNotesNoRoot > numStrings && !Options.AllowPartialChords)
                            {
                                StrongReferenceMessenger.Default.Send(new ChordiousMessage(Strings.ChordFinderNotEnoughStringsTryPartialMessage));
                                return;
                            }
                        }

                        Results.Clear();
                        SelectedResults.Clear();

                        ChordFinderResultSet results = await FindChordsAsync(_searchAsyncCancellationTokenSource.Token);

                        if (results is not null)
                        {
                            if (results.Count == 0 && !_searchAsyncCancellationTokenSource.IsCancellationRequested)
                            {
                                StrongReferenceMessenger.Default.Send(new ChordiousMessage(Strings.ChordFinderNoResultsMessage));
                            }
                            else
                            {
                                for (int i = 0; i < results.Count; i++)
                                {
                                    if (_searchAsyncCancellationTokenSource.IsCancellationRequested)
                                    {
                                        break;
                                    }
                                    Results.Add(await RenderChordAsync(results.ResultAt(i)));
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        ExceptionUtils.HandleException(ex);
                    }
                    finally
                    {
                        _lastSearchComplete = DateTime.Now;
                        _searchAsyncCancellationTokenSource = null;
                        IsIdle = true;
                    }
                }, () =>
                {
                    return CanSearch();
                });
            }
        }
        private RelayCommand _searchAsync;

        private CancellationTokenSource _searchAsyncCancellationTokenSource;

        public RelayCommand CancelSearch
        {
            get
            {
                return _cancelSearch ??= new RelayCommand(() =>
                {
                    try
                    {
                        _searchAsyncCancellationTokenSource?.Cancel();
                    }
                    catch (Exception ex)
                    {
                        ExceptionUtils.HandleException(ex);
                    }
                });
            }
        }
        private RelayCommand _cancelSearch;

        public string SaveSelectedLabel
        {
            get
            {
                return Strings.FinderSaveSelectedLabel;
            }
        }

        public string SaveSelectedToolTip
        {
            get
            {
                return Strings.FinderSaveSelectedToolTip;
            }
        }

        public RelayCommand SaveSelected
        {
            get
            {
                return _saveSelected ??= new RelayCommand(() =>
                {
                    try
                    {
                        StrongReferenceMessenger.Default.Send(new ShowDiagramCollectionSelectorMessage((name, newCollection) =>
                        {
                            try
                            {
                                DiagramLibrary library = AppVM.UserConfig.DiagramLibrary;
                                DiagramCollection targetCollection = library.Get(name);

                                foreach (ObservableDiagram od in SelectedResults)
                                {
                                    targetCollection.Add(od.Diagram);
                                }

                                LastDiagramCollectionName = name.Trim();
                            }
                            catch (Exception ex)
                            {
                                ExceptionUtils.HandleException(ex);
                            }
                        }, LastDiagramCollectionName));
                    }
                    catch (Exception ex)
                    {
                        ExceptionUtils.HandleException(ex);
                    }
                }, () =>
                {
                    return SelectedResults.Count > 0;
                });
            }
        }
        private RelayCommand _saveSelected;

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
                return Strings.FinderEditSelectedToolTip;
            }
        }

        public RelayCommand EditSelected
        {
            get
            {
                // Use the built-in ObservableDiagram's RelayCommand if it's available
                if (SelectedResults.Count == 1)
                {
                    return SelectedResults[0].ShowEditor;
                }

                // If a single result isn't selected, throw an error
                return _editSelected ??= new RelayCommand(() =>
                {
                    try
                    {
                        StrongReferenceMessenger.Default.Send(new ChordiousMessage(Strings.FinderOnlyOneResultCanBeEditedMessage));
                    }
                    catch (Exception ex)
                    {
                        ExceptionUtils.HandleException(ex);
                    }
                }, () =>
                {
                    return SelectedResults.Count > 0;
                });
            }
        }
        private RelayCommand _editSelected;

        #region SendToClipboard

        public string SendSelectedImageToClipboardLabel
        {
            get
            {
                return Strings.FinderSendSelectedImageToClipboardLabel;
            }
        }

        public string SendSelectedImageToClipboardToolTip
        {
            get
            {
                return Strings.FinderSendSelectedImageToClipboardToolTip;
            }
        }

        public RelayCommand SendSelectedImageToClipboard
        {
            get
            {
                // Use the built-in ObservableDiagram's RelayCommand if it's available
                if (SelectedResults.Count == 1)
                {
                    return SelectedResults[0].SendImageToClipboard;
                }

                // If a single result isn't selected, throw an error
                return _sendSelectedToClipboard ??= new RelayCommand(() =>
                {
                    try
                    {
                        StrongReferenceMessenger.Default.Send(new ChordiousMessage(Strings.FinderOnlyOneResultCanBeCopiedToClipboardMessage));
                    }
                    catch (Exception ex)
                    {
                        ExceptionUtils.HandleException(ex);
                    }
                }, () =>
                {
                    return SelectedResults.Count > 0;
                });
            }
        }

        public string SendSelectedScaledImageToClipboardLabel
        {
            get
            {
                return Strings.FinderSendSelectedScaledImageToClipboardLabel;
            }
        }

        public string SendSelectedScaledImageToClipboardToolTip
        {
            get
            {
                return Strings.FinderSendSelectedScaledImageToClipboardToolTip;
            }
        }

        public RelayCommand SendSelectedScaledImageToClipboard
        {
            get
            {
                // Use the built-in ObservableDiagram's RelayCommand if it's available
                if (SelectedResults.Count == 1)
                {
                    return SelectedResults[0].SendScaledImageToClipboard;
                }

                // If a single diagram isn't selected, throw an error
                return _sendSelectedToClipboard ??= new RelayCommand(() =>
                {
                    try
                    {
                        StrongReferenceMessenger.Default.Send(new ChordiousMessage(Strings.FinderOnlyOneResultCanBeCopiedToClipboardMessage));
                    }
                    catch (Exception ex)
                    {
                        ExceptionUtils.HandleException(ex);
                    }
                }, () =>
                {
                    return SelectedResults.Count > 0;
                });
            }
        }

        public string SendSelectedTextToClipboardLabel
        {
            get
            {
                return Strings.FinderSendSelectedTextToClipboardLabel;
            }
        }

        public string SendSelectedTextToClipboardToolTip
        {
            get
            {
                return Strings.FinderSendSelectedTextToClipboardToolTip;
            }
        }

        public RelayCommand SendSelectedTextToClipboard
        {
            get
            {
                // Use the built-in ObservableDiagram's RelayCommand if it's available
                if (SelectedResults.Count == 1)
                {
                    return SelectedResults[0].SendTextToClipboard;
                }

                // If a single result isn't selected, throw an error
                return _sendSelectedToClipboard ??= new RelayCommand(() =>
                {
                    try
                    {
                        StrongReferenceMessenger.Default.Send(new ChordiousMessage(Strings.FinderOnlyOneResultCanBeCopiedToClipboardMessage));
                    }
                    catch (Exception ex)
                    {
                        ExceptionUtils.HandleException(ex);
                    }
                }, () =>
                {
                    return SelectedResults.Count > 0;
                });
            }
        }

        private RelayCommand _sendSelectedToClipboard;

        #endregion

        public Action RequestClose;

        public RelayCommand CancelOrClose
        {
            get
            {
                return _cancelOrClose ??= new RelayCommand(() =>
                {
                    try
                    {
                        if (_searchAsyncCancellationTokenSource is not null)
                        {
                            _searchAsyncCancellationTokenSource.Cancel();
                        }
                        else if (!_lastSearchComplete.HasValue || (DateTime.Now - _lastSearchComplete.Value) > TimeSpan.FromMilliseconds(500))
                        {
                            RequestClose?.Invoke();
                        }
                    }
                    catch (Exception ex)
                    {
                        ExceptionUtils.HandleException(ex);
                    }
                });
            }
        }
        private RelayCommand _cancelOrClose;

        private DateTime? _lastSearchComplete = null;

        private static string LastDiagramCollectionName = "";

        public ObservableCollection<ObservableDiagram> SelectedResults { get; private set; } = null;

        public ObservableCollection<ObservableDiagram> Results { get; private set; } = null;

        internal ChordFinderOptions Options { get; private set; }
        internal ChordFinderStyle Style { get; private set; }

        public ChordFinderViewModel()
        {
            Options = new ChordFinderOptions(AppVM.UserConfig);
            Style = new ChordFinderStyle(AppVM.UserConfig);

            RefreshInstruments(Options.Instrument, Options.Tuning);

            RefreshChordQualities(Options.ChordQuality);

            Results = new ObservableCollection<ObservableDiagram>();
            SelectedResults = new ObservableCollection<ObservableDiagram>();

            SelectedResults.CollectionChanged += SelectedResults_CollectionChanged;
        }

        private void SelectedResults_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            SaveSelected.NotifyCanExecuteChanged();
            OnPropertyChanged(nameof(EditSelected));
            _editSelected?.NotifyCanExecuteChanged();
            OnPropertyChanged(nameof(SendSelectedImageToClipboard));
            OnPropertyChanged(nameof(SendSelectedScaledImageToClipboard));
            OnPropertyChanged(nameof(SendSelectedTextToClipboard));
            _sendSelectedToClipboard?.NotifyCanExecuteChanged();
        }

        private void RefreshInstruments(IInstrument selectedInstrument = null, ITuning selectedTuning = null)
        {
            Instruments = AppVM.GetInstruments();
            SelectedInstrument = null;

            if (selectedInstrument is not null && Instruments is not null)
            {
                foreach (ObservableInstrument oi in Instruments)
                {
                    if (oi.Instrument == selectedInstrument)
                    {
                        SelectedInstrument = oi;
                        break;
                    }
                }
            }

            if (SelectedInstrument is not null)
            {
                Tunings = SelectedInstrument.GetTunings();
                SelectedTuning = null;

                if (selectedTuning is not null && Tunings is not null)
                {
                    foreach (ObservableTuning ot in Tunings)
                    {
                        if (ot.Tuning == selectedTuning)
                        {
                            SelectedTuning = ot;
                            break;
                        }
                    }
                }
            }
        }

        private void RefreshChordQualities(IChordQuality selectedChordQuality = null)
        {
            ChordQualities = AppVM.GetChordQualities();
            SelectedChordQuality = null;

            if (selectedChordQuality is not null && ChordQualities is not null)
            {
                foreach (ObservableChordQuality ocq in ChordQualities)
                {
                    if (ocq.ChordQuality == selectedChordQuality)
                    {
                        SelectedChordQuality = ocq;
                        break;
                    }
                }
            }
        }

        private void RefreshSettings()
        {
            RefreshInstruments(Options.Instrument, Options.Tuning);
            RefreshChordQualities(Options.ChordQuality);

            OnPropertyChanged(nameof(SelectedRootNote));
            OnPropertyChanged(nameof(NumFrets));
            OnPropertyChanged(nameof(MaxReach));
            OnPropertyChanged(nameof(MaxFret));
            OnPropertyChanged(nameof(AllowOpenStrings));
            OnPropertyChanged(nameof(AllowMutedStrings));
            OnPropertyChanged(nameof(AllowRootlessChords));
            OnPropertyChanged(nameof(AddTitle));
            OnPropertyChanged(nameof(MirrorResults));
            OnPropertyChanged(nameof(SelectedBarreTypeOptionIndex));
            OnPropertyChanged(nameof(AddRootNotes));
            OnPropertyChanged(nameof(SelectedMarkTextOptionIndex));
            OnPropertyChanged(nameof(AddBottomMarks));
            OnPropertyChanged(nameof(SelectedBottomMarkTextOptionIndex));
            OnPropertyChanged(nameof(SelectedFretLabelSideIndex));
        }

        private Task<ChordFinderResultSet> FindChordsAsync(CancellationToken cancelToken)
        {
            return Task<ChordFinderResultSet>.Factory.StartNew(() =>
            {
                ChordFinderResultSet results = null;

                try
                {
                    Task<ChordFinderResultSet> task = ChordFinder.FindChordsAsync(Options, cancelToken);
                    task.Wait();

                    results = task.Result;
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }

                return results;
            });
        }

        private Task<ObservableDiagram> RenderChordAsync(IChordFinderResult result)
        {
            return Task<ObservableDiagram>.Factory.StartNew(() =>
            {
                ObservableDiagram od = null;
                AppVM.AppView.DoOnUIThread(() =>
                    {
                        try
                        {
                            od = new ObservableDiagram(result.ToDiagram(Style), name: Strings.FinderResultDiagramName);
                            od.PostEditCallback = (changed) =>
                            {
                                if (changed)
                                {
                                    od.Refresh();
                                }
                            };
                        }
                        catch (Exception ex)
                        {
                            ExceptionUtils.HandleException(ex);
                        }
                    });
                return od;
            });
        }

        private bool CanSearch()
        {
            return IsIdle && (SelectedInstrument is not null) && (SelectedTuning is not null) && (SelectedChordQuality is not null);
        }
    }
}
