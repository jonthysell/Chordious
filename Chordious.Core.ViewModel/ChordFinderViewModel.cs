// 
// ChordFinderViewModel.cs
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
using System.Collections.Specialized;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;

using com.jonthysell.Chordious.Core;

namespace com.jonthysell.Chordious.Core.ViewModel
{
    public class ChordFinderViewModel : ViewModelBase, IIdle
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
                return "Chord Finder";
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
                RaisePropertyChanged("IsIdle");
                RaisePropertyChanged("SearchAsync");
            }
        }
        private bool _isIdle = true;

        #region Options

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
                    RaisePropertyChanged("SelectedInstrument");
                    RaisePropertyChanged("SearchAsync");
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
                RaisePropertyChanged("Instruments");
            }
        }
        private ObservableCollection<ObservableInstrument> _instruments;

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
                    RaisePropertyChanged("SelectedTuning");
                    RaisePropertyChanged("SearchAsync");
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
                RaisePropertyChanged("Tunings");
            }
        }
        private ObservableCollection<ObservableTuning> _tunings;

        public RelayCommand ShowInstrumentManager
        {
            get
            {
                return new RelayCommand(() =>
                {
                    try
                    {
                        Messenger.Default.Send<ShowInstrumentManagerMessage>(new ShowInstrumentManagerMessage(() =>
                        {
                            RefreshInstruments(SelectedInstrument.Instrument, SelectedTuning.Tuning);
                        }));
                    }
                    catch (Exception ex)
                    {
                        ExceptionUtils.HandleException(ex);
                    }
                });
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
                    RaisePropertyChanged("SelectedRootNote");
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
                    if (null != value)
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
                    RaisePropertyChanged("SelectedChordQuality");
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
                RaisePropertyChanged("ChordQualities");
            }
        }
        private ObservableCollection<ObservableChordQuality> _chordQualities;

        public RelayCommand ShowChordQualityManager
        {
            get
            {
                return new RelayCommand(() =>
                {
                    try
                    {
                        Messenger.Default.Send<ShowChordQualityManagerMessage>(new ShowChordQualityManagerMessage(() =>
                            {
                                RefreshChordQualities(SelectedChordQuality.ChordQuality);
                            }));
                    }
                    catch (Exception ex)
                    {
                        ExceptionUtils.HandleException(ex);
                    }
                });
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
                    RaisePropertyChanged("NumFrets");
                    RaisePropertyChanged("MaxReach");
                }
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
                    RaisePropertyChanged("MaxReach");
                    RaisePropertyChanged("NumFrets");
                }
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
                    RaisePropertyChanged("MaxFret");
                }
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
                RaisePropertyChanged("AllowOpenStrings");
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
                RaisePropertyChanged("AllowMutedStrings");
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
                RaisePropertyChanged("AllowRootlessChords");
            }
        }

        #endregion

        #region Styles

        public bool AddTitle
        {
            get
            {
                return Style.AddTitle;
            }
            set
            {
                Style.AddTitle = value;
                RaisePropertyChanged("AddTitle");
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
                RaisePropertyChanged("MirrorResults");
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
                RaisePropertyChanged("SelectedBarreTypeOptionIndex");
            }
        }

        public ObservableCollection<string> BarreTypeOptions
        {
            get
            {
                return GetBarreTypeOptions();
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
                RaisePropertyChanged("AddRootNotes");
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
                RaisePropertyChanged("SelectedMarkTextOptionIndex");
            }
        }

        public ObservableCollection<string> MarkTextOptions
        {
            get
            {
                return GetMarkTextOptions();
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
                RaisePropertyChanged("AddBottomMarks");
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
                RaisePropertyChanged("SelectedBottomMarkTextOptionIndex");
            }
        }

        public ObservableCollection<string> BottomMarkTextOptions
        {
            get
            {
                return GetMarkTextOptions();
            }
        }

        #endregion

        public RelayCommand SetAsDefaults
        {
            get
            {
                return new RelayCommand(() =>
                    {
                        try
                        {
                            Messenger.Default.Send<ConfirmationMessage>(new ConfirmationMessage("This will set your current search parameters as the new default values. Do you want to continue?", (confirmed) =>
                            {
                                if (confirmed)
                                {
                                    Options.Settings.SetParent();
                                    Style.Settings.SetParent();
                                    RefreshSettings();
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

        public RelayCommand ResetToDefaults
        {
            get
            {
                return new RelayCommand(() =>
                {
                    try
                    {
                        Messenger.Default.Send<ConfirmationMessage>(new ConfirmationMessage("This will reset your current search parameters to the default values. Do you want to continue?", (confirmed) =>
                        {
                            if (confirmed)
                            {
                                Options.Settings.Clear();
                                Style.Settings.Clear();
                                RefreshSettings();
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

        public RelayCommand SearchAsync
        {
            get
            {
                return new RelayCommand(async () =>
                {
                    try
                    {
                        IsIdle = false;
                        ChordFinderResultSet results = await FindChordsAsync();
                        Results.Clear();
                        SelectedResults.Clear();

                        if (null != results)
                        {
                            for (int i = 0; i < results.Count; i++)
                            {
                                Results.Add(await RenderChordAsync(results.ResultAt(i)));
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        ExceptionUtils.HandleException(ex);
                    }
                    finally
                    {
                        IsIdle = true;
                    }
                }, () =>
                {
                    return CanSearch();
                });
            }
        }

        public RelayCommand SaveSelected
        {
            get
            {
                return new RelayCommand(() =>
                {
                    try
                    {
                        Messenger.Default.Send<PromptForTextMessage>(new PromptForTextMessage("Save selected diagrams to:", LastDiagramCollectionName, (name) =>
                        {
                            DiagramLibrary library = AppVM.UserConfig.DiagramLibrary;
                            DiagramCollection targetCollection = null;

                            if (!library.TryGet(name, out targetCollection))
                            {
                                targetCollection = library.Add(name);
                            }

                            foreach (ObservableDiagram od in SelectedResults)
                            {
                                targetCollection.Add(od.Diagram);
                            }

                            LastDiagramCollectionName = name.Trim();
                        }));
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

        private static string LastDiagramCollectionName = "Chord Finder Results";

        public ObservableCollection<ObservableDiagram> SelectedResults
        {
            get
            {
                return _selectedResults;
            }
            private set
            {
                if (null == value)
                {
                    throw new ArgumentNullException();
                }

                _selectedResults = value;
                RaisePropertyChanged("SelectedResults");
            }
        }
        private ObservableCollection<ObservableDiagram> _selectedResults;

        private void SelectedResults_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            RaisePropertyChanged("SaveSelected");
        }

        public ObservableCollection<ObservableDiagram> Results
        {
            get
            {
                return _results;
            }
            private set
            {
                _results = value;
                RaisePropertyChanged("Results");
            }
        }
        public ObservableCollection<ObservableDiagram> _results;

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

        private void RefreshInstruments(Instrument selectedInstrument = null, Tuning selectedTuning = null)
        {
            Instruments = AppVM.GetInstruments();

            foreach (ObservableInstrument oi in Instruments)
            {
                if (oi.Instrument == selectedInstrument)
                {
                    SelectedInstrument = oi;
                    break;
                }
            }

            Tunings = SelectedInstrument.GetTunings();

            foreach (ObservableTuning ot in Tunings)
            {
                if (ot.Tuning == selectedTuning)
                {
                    SelectedTuning = ot;
                    break;
                }
            }
        }

        private void RefreshChordQualities(ChordQuality selectedChordQuality = null)
        {
            ChordQualities = AppVM.GetChordQualities();

            foreach (ObservableChordQuality ocq in ChordQualities)
            {
                if (ocq.ChordQuality == selectedChordQuality)
                {
                    SelectedChordQuality = ocq;
                    break;
                }
            }
        }

        private void RefreshSettings()
        {
            RefreshInstruments(Options.Instrument, Options.Tuning);
            RefreshChordQualities(Options.ChordQuality);

            RaisePropertyChanged("SelectedRootNote");
            RaisePropertyChanged("NumFrets");
            RaisePropertyChanged("MaxReach");
            RaisePropertyChanged("MaxFret");
            RaisePropertyChanged("AllowOpenStrings");
            RaisePropertyChanged("AllowMutedStrings");
            RaisePropertyChanged("AllowRootlessChords");
            RaisePropertyChanged("AddTitle");
            RaisePropertyChanged("MirrorResults");
            RaisePropertyChanged("SelectedBarreTypeOptionIndex");
            RaisePropertyChanged("AddRootNotes");
            RaisePropertyChanged("SelectedMarkTextOptionIndex");
            RaisePropertyChanged("AddBottomMarks");
            RaisePropertyChanged("SelectedBottomMarkTextOptionIndex");
        }

        private Task<ChordFinderResultSet> FindChordsAsync()
        {
            return Task<ChordFinderResultSet>.Factory.StartNew(() =>
            {
                return ChordFinder.FindChords(Options);
            });
        }

        private Task<ObservableDiagram> RenderChordAsync(ChordFinderResult result)
        {            
            return Task<ObservableDiagram>.Factory.StartNew(() =>
            {
                ObservableDiagram od = null;
                AppVM.DoOnUIThread(() =>
                    {
                        od = new ObservableDiagram(result.ToDiagram(Style));
                    });
                return od;
            });
        }

        private bool CanSearch()
        {
            return IsIdle && (null != SelectedInstrument) && (null != SelectedTuning);
        }

        private static ObservableCollection<string> GetBarreTypeOptions()
        {
            ObservableCollection<string> collection = new ObservableCollection<string>();

            collection.Add("None");
            collection.Add("Partial");
            collection.Add("Full");

            return collection;
        }

        private static ObservableCollection<string> GetMarkTextOptions()
        {
            ObservableCollection<string> collection = new ObservableCollection<string>();

            collection.Add("None");
            collection.Add("Show Both");
            collection.Add("Prefer Flats");
            collection.Add("Prefer Sharps");

            return collection;
        }
    }
}
