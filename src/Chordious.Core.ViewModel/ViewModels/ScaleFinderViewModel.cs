﻿// Copyright (c) Jon Thysell <http://jonthysell.com>
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
    public class ScaleFinderViewModel : ObservableObject, IIdle
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
                return Strings.ScaleFinderTitle;
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

                    if (null != value)
                    {
                        Tunings = SelectedInstrument.GetTunings();
                        if (null != Tunings && Tunings.Count > 0)
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
                    if (null != value)
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
                return _showInstrumentManager ?? (_showInstrumentManager = new RelayCommand(() =>
                {
                    try
                    {
                        StrongReferenceMessenger.Default.Send(new ShowInstrumentManagerMessage(() =>
                        {
                            try
                            {
                                RefreshInstruments(SelectedInstrument?.Instrument, SelectedTuning?.Tuning);
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
        private RelayCommand _showInstrumentManager;

        #region RootNode

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

        #region Scale

        public string SelectedScaleLabel
        {
            get
            {
                return Strings.ScaleFinderOptionsSelectedScaleLabel;
            }
        }

        public string SelectedScaleToolTip
        {
            get
            {
                return Strings.ScaleFinderOptionsSelectedScaleToolTip;
            }
        }

        public ObservableScale SelectedScale
        {
            get
            {
                return _scale;
            }
            set
            {
                try
                {
                    _scale = value;
                    if (null != value)
                    {
                        Options.SetTarget(Options.RootNote, SelectedScale.Scale);
                    }
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
                finally
                {
                    OnPropertyChanged(nameof(SelectedScale));
                    SearchAsync.NotifyCanExecuteChanged();
                    SetAsDefaults.NotifyCanExecuteChanged();
                }
            }
        }
        private ObservableScale _scale;

        public ObservableCollection<ObservableScale> Scales
        {
            get
            {
                return _scales;
            }
            private set
            {
                _scales = value;
                OnPropertyChanged(nameof(Scales));
            }
        }
        private ObservableCollection<ObservableScale> _scales;

        public string ShowScaleManagerLabel
        {
            get
            {
                return Strings.ShowScaleManagerLabel;
            }
        }

        public string ShowScaleManagerToolTip
        {
            get
            {
                return Strings.ShowScaleManagerToolTip;
            }
        }

        public RelayCommand ShowScaleManager
        {
            get
            {
                return _showScaleManager ?? (_showScaleManager = new RelayCommand(() =>
                {
                    try
                    {
                        StrongReferenceMessenger.Default.Send(new ShowScaleManagerMessage(() =>
                        {
                            try
                            {
                                RefreshScales(SelectedScale?.Scale);
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
        private RelayCommand _showScaleManager;

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

        public string StrictIntervalsLabel
        {
            get
            {
                return Strings.ScaleFinderOptionsStrictIntervalsLabel;
            }
        }

        public string StrictIntervalsToolTip
        {
            get
            {
                return Strings.ScaleFinderOptionsStrictIntervalsToolTip;
            }
        }

        public bool StrictIntervals
        {
            get
            {
                return Options.StrictIntervals;
            }
            set
            {
                Options.StrictIntervals = value;
                OnPropertyChanged(nameof(StrictIntervals));
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
                return _setAsDefaults ?? (_setAsDefaults = new RelayCommand(() =>
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
                        }, "confirmation.scalefinder.setasdefaults"));
                    }
                    catch (Exception ex)
                    {
                        ExceptionUtils.HandleException(ex);
                    }
                }, () =>
                {
                    return CanSearch();
                }));
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
                return _resetToDefaults ?? (_resetToDefaults = new RelayCommand(() =>
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
                        }, "confirmation.scalefinder.resettodefaults"));
                    }
                    catch (Exception ex)
                    {
                        ExceptionUtils.HandleException(ex);
                    }
                }));
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
                return _searchAsync ?? (_searchAsync = new RelayCommand(async () =>
                {
                    _searchAsyncCancellationTokenSource = new CancellationTokenSource();

                    try
                    {
                        IsIdle = false;
                        Results.Clear();
                        SelectedResults.Clear();

                        ScaleFinderResultSet results = await FindScalesAsync(_searchAsyncCancellationTokenSource.Token);
                        
                        if (null != results)
                        {
                            if (results.Count == 0 && !_searchAsyncCancellationTokenSource.IsCancellationRequested)
                            {
                                StrongReferenceMessenger.Default.Send(new ChordiousMessage(Strings.ScaleFinderNoResultsMessage));
                            }
                            else
                            {
                                for (int i = 0; i < results.Count; i++)
                                {
                                    if (_searchAsyncCancellationTokenSource.IsCancellationRequested)
                                    {
                                        break;
                                    }
                                    Results.Add(await RenderScaleAsync(results.ResultAt(i)));
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
                }));
            }
        }
        private RelayCommand _searchAsync;

        private CancellationTokenSource _searchAsyncCancellationTokenSource;

        public RelayCommand CancelSearch
        {
            get
            {
                return _cancelSearch ?? (_cancelSearch = new RelayCommand(() =>
                {
                    try
                    {
                        _searchAsyncCancellationTokenSource?.Cancel();
                    }
                    catch (Exception ex)
                    {
                        ExceptionUtils.HandleException(ex);
                    }
                }));
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
                return _saveSelected ?? (_saveSelected = new RelayCommand(() =>
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
                }));
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
                return _editSelected ?? (_editSelected = new RelayCommand(() =>
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
                }));
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
                return _sendSelectedToClipboard ?? (_sendSelectedToClipboard = new RelayCommand(() =>
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
                }));
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
                return _sendSelectedToClipboard ?? (_sendSelectedToClipboard = new RelayCommand(() =>
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
                }));
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
                return _sendSelectedToClipboard ?? (_sendSelectedToClipboard = new RelayCommand(() =>
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
                }));
            }
        }

        private RelayCommand _sendSelectedToClipboard;

        #endregion

        public Action RequestClose;

        public RelayCommand CancelOrClose
        {
            get
            {
                return _cancelOrClose ?? (_cancelOrClose = new RelayCommand(() =>
                {
                    try
                    {
                        if (null != _searchAsyncCancellationTokenSource)
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
                }));
            }
        }
        private RelayCommand _cancelOrClose;

        private DateTime? _lastSearchComplete = null;

        private static string LastDiagramCollectionName = "";

        public ObservableCollection<ObservableDiagram> SelectedResults { get; private set; } = null;

        public ObservableCollection<ObservableDiagram> Results { get; private set; } = null;

        internal ScaleFinderOptions Options { get; private set; }
        internal ScaleFinderStyle Style { get; private set; }

        public ScaleFinderViewModel()
        {
            Options = new ScaleFinderOptions(AppVM.UserConfig);
            Style = new ScaleFinderStyle(AppVM.UserConfig);

            RefreshInstruments(Options.Instrument, Options.Tuning);

            RefreshScales(Options.Scale);

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

            if (null != selectedInstrument && null != Instruments)
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

            if (null != SelectedInstrument)
            {
                Tunings = SelectedInstrument.GetTunings();
                SelectedTuning = null;

                if (null != selectedTuning && null != Tunings)
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

        private void RefreshScales(IScale selectedScale = null)
        {
            Scales = AppVM.GetScales();
            SelectedScale = null;

            if (null != selectedScale && null != Scales)
            {
                foreach (ObservableScale os in Scales)
                {
                    if (os.Scale == selectedScale)
                    {
                        SelectedScale = os;
                        break;
                    }
                }
            }
        }

        private void RefreshSettings()
        {
            RefreshInstruments(Options.Instrument, Options.Tuning);
            RefreshScales(Options.Scale);

            OnPropertyChanged(nameof(SelectedRootNote));
            OnPropertyChanged(nameof(NumFrets));
            OnPropertyChanged(nameof(MaxReach));
            OnPropertyChanged(nameof(MaxFret));
            OnPropertyChanged(nameof(AllowOpenStrings));
            OnPropertyChanged(nameof(AllowMutedStrings));
            OnPropertyChanged(nameof(AddTitle));
            OnPropertyChanged(nameof(MirrorResults));
            OnPropertyChanged(nameof(AddRootNotes));
            OnPropertyChanged(nameof(SelectedMarkTextOptionIndex));
            OnPropertyChanged(nameof(SelectedFretLabelSideIndex));
        }

        private Task<ScaleFinderResultSet> FindScalesAsync(CancellationToken cancelToken)
        {
            return Task<ScaleFinderResultSet>.Factory.StartNew(() =>
            {
                ScaleFinderResultSet results = null;

                try
                {
                    Task<ScaleFinderResultSet> task = ScaleFinder.FindScalesAsync(Options, cancelToken);
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

        private Task<ObservableDiagram> RenderScaleAsync(ScaleFinderResult result)
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
            return IsIdle && (null != SelectedInstrument) && (null != SelectedTuning) && (null != SelectedScale);
        }
    }
}
