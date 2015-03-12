// 
// ScaleFinderViewModel.cs
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
    public class ScaleFinderViewModel : ViewModelBase
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
                return "Scale Finder";
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
                    if (null != value)
                    {
                        _scale = value;
                        Options.SetTarget(Options.RootNote, SelectedScale.Scale);
                        RaisePropertyChanged("SelectedScale");
                    }
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
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
                RaisePropertyChanged("Scales");
            }
        }
        private ObservableCollection<ObservableScale> _scales;

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
                RaisePropertyChanged("MaxReach");
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
                RaisePropertyChanged("NumFrets");
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
                        ScaleFinderResultSet results = await FindScalesAsync();
                        Results.Clear();
                        SelectedResults.Clear();

                        if (null != results)
                        {
                            for (int i = 0; i < results.Count; i++)
                            {
                                Results.Add(await RenderScaleAsync(results.ResultAt(i)));
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

        private static string LastDiagramCollectionName = "Scale Finder Results";

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

        internal ScaleFinderOptions Options { get; private set; }
        internal ScaleFinderStyle Style { get; private set; }

        public ScaleFinderViewModel()
        {
            Options = new ScaleFinderOptions(AppVM.UserConfig);
            Style = new ScaleFinderStyle(AppVM.UserConfig);

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

            Scales = AppVM.GetScales();

            foreach (ObservableScale os in Scales)
            {
                if (os.Scale == Options.Scale)
                {
                    SelectedScale = os;
                    break;
                }
            }

            Results = new ObservableCollection<ObservableDiagram>();
            SelectedResults = new ObservableCollection<ObservableDiagram>();

            SelectedResults.CollectionChanged += SelectedResults_CollectionChanged;
        }

        private Task<ScaleFinderResultSet> FindScalesAsync()
        {
            return Task<ScaleFinderResultSet>.Factory.StartNew(() =>
            {
                return ScaleFinder.FindScales(Options);
            });
        }

        private Task<ObservableDiagram> RenderScaleAsync(ScaleFinderResult result)
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
