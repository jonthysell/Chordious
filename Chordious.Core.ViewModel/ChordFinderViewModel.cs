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

using com.jonthysell.Chordious.Core;

namespace com.jonthysell.Chordious.Core.ViewModel
{
    public class ChordFinderViewModel : ViewModelBase
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
                    if (null != value)
                    {
                        _instrument = value;
                        Tunings = SelectedInstrument.GetTunings();
                        SelectedTuning = Tunings[0];
                        Options.SetTarget(SelectedInstrument.Instrument, SelectedTuning.Tuning);
                        RaisePropertyChanged("SelectedInstrument");
                    }
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
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
                    if (null != value)
                    {
                        _tuning = value;
                        Options.SetTarget(SelectedInstrument.Instrument, SelectedTuning.Tuning);
                        RaisePropertyChanged("SelectedTuning");
                    }
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
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
                RaisePropertyChanged("SelectedRootNote");
            }
        }

        public ObservableCollection<string> RootNotes
        {
            get
            {
                return AppVM.GetNotes();
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
                    if (null != value)
                    {
                        _chordQuality = value;
                        Options.SetTarget(Options.RootNote, SelectedChordQuality.ChordQuality);
                        RaisePropertyChanged("SelectedChordQuality");
                    }
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
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
                catch(Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
                RaisePropertyChanged("NumFrets");
                if (NumFrets < MaxReach)
                {
                    MaxReach = NumFrets;
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
                RaisePropertyChanged("MaxReach");
                if (MaxReach > NumFrets)
                {
                    NumFrets = MaxReach;
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
                RaisePropertyChanged("MaxFret");
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
                    return IsIdle;
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
                        throw new NotImplementedException();
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

            Instruments = AppVM.GetInstruments();

            foreach (ObservableInstrument oi in Instruments)
            {
                if (oi.Instrument == Options.Instrument)
                {
                    SelectedInstrument = oi;
                    break;
                }
            }

            Tunings = SelectedInstrument.GetTunings();

            foreach (ObservableTuning ot in Tunings)
            {
                if (ot.Tuning == Options.Tuning)
                {
                    SelectedTuning = ot;
                    break;
                }
            }

            ChordQualities = AppVM.GetChordQualities();

            foreach (ObservableChordQuality ocq in ChordQualities)
            {
                if (ocq.ChordQuality == Options.ChordQuality)
                {
                    SelectedChordQuality = ocq;
                    break;
                }
            }

            Results = new ObservableCollection<ObservableDiagram>();
            SelectedResults = new ObservableCollection<ObservableDiagram>();

            SelectedResults.CollectionChanged += SelectedResults_CollectionChanged;
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

        private ObservableCollection<string> GetBarreTypeOptions()
        {
            ObservableCollection<string> collection = new ObservableCollection<string>();

            collection.Add("None");
            collection.Add("Partial");
            collection.Add("Full");

            return collection;
        }

        private ObservableCollection<string> GetMarkTextOptions()
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
