// 
// InstrumentManagerViewModel.cs
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
using System.Collections.ObjectModel;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;

using com.jonthysell.Chordious.Core;

namespace com.jonthysell.Chordious.Core.ViewModel
{
    public class InstrumentManagerViewModel : ViewModelBase
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
                return "Instruments";
            }
        }

        public bool InstrumentIsSelected
        {
            get
            {
                return (null != SelectedInstrument);
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

                    if (null == SelectedInstrument)
                    {
                        Tunings = null;
                    }
                    else
                    {
                        Tunings = SelectedInstrument.GetTunings();
                        if (Tunings.Count > 0)
                        {
                            SelectedTuning = Tunings[0];
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
                    RaisePropertyChanged("InstrumentIsSelected");
                    RaisePropertyChanged("EditInstrument");
                    RaisePropertyChanged("DeleteInstrument");
                    RaisePropertyChanged("AddTuning");
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

        public bool TuningIsSelected
        {
            get
            {
                return (null != SelectedTuning);
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
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
                finally
                {
                    RaisePropertyChanged("SelectedTuning");
                    RaisePropertyChanged("TuningIsSelected");
                    RaisePropertyChanged("EditTuning");
                    RaisePropertyChanged("DeleteTuning");
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

        public RelayCommand AddInstrument
        {
            get
            {
                return new RelayCommand(() =>
                {
                    try
                    {
                        Messenger.Default.Send<ShowInstrumentEditorMessage>(new ShowInstrumentEditorMessage(true, (name, numStrings) =>
                        {
                            try
                            {
                                Instrument addedInstrument = AppVM.UserConfig.Instruments.Add(name, numStrings);
                                Refresh(addedInstrument);
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

        public RelayCommand EditInstrument
        {
            get
            {
                return new RelayCommand(() =>
                {
                    try
                    {
                        Messenger.Default.Send<ShowInstrumentEditorMessage>(new ShowInstrumentEditorMessage(false, (name, numStrings) =>
                        {
                            try
                            {
                                SelectedInstrument.Instrument.Name = name;
                                Refresh(SelectedInstrument.Instrument);
                            }
                            catch (Exception ex)
                            {
                                ExceptionUtils.HandleException(ex);
                            }
                        }, SelectedInstrument.Name, SelectedInstrument.NumStrings));
                    }
                    catch (Exception ex)
                    {
                        ExceptionUtils.HandleException(ex);
                    }
                }, () =>
                {
                    return InstrumentIsSelected && SelectedInstrument.CanEdit;
                });
            }
        }

        public RelayCommand DeleteInstrument
        {
            get
            {
                return new RelayCommand(() =>
                {
                    try
                    {
                        Messenger.Default.Send<ConfirmationMessage>(new ConfirmationMessage(String.Format("This will delete the instrument \"{0}\". This cannot be undone. Do you want to continue?", SelectedInstrument.Name), (confirm) =>
                        {
                            try
                            {
                                if (confirm)
                                {
                                    AppVM.UserConfig.Instruments.Remove(SelectedInstrument.Name);
                                    Refresh();
                                }
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
                    return InstrumentIsSelected && SelectedInstrument.CanEdit;
                });
            }
        }

        public RelayCommand AddTuning
        {
            get
            {
                return new RelayCommand(() =>
                {
                    try
                    {
                        Messenger.Default.Send<ShowTuningEditorMessage>(new ShowTuningEditorMessage(SelectedInstrument, (accepted) =>
                        {
                            if (accepted)
                            {
                                Refresh(SelectedInstrument.Instrument);
                            }
                        }));
                    }
                    catch (Exception ex)
                    {
                        ExceptionUtils.HandleException(ex);
                    }
                }, () =>
                {
                    return InstrumentIsSelected && SelectedInstrument.CanEdit;
                });
            }
        }

        public RelayCommand EditTuning
        {
            get
            {
                return new RelayCommand(() =>
                {
                    try
                    {
                        Messenger.Default.Send<ShowTuningEditorMessage>(new ShowTuningEditorMessage(SelectedTuning, (accepted) =>
                        {
                            if (accepted)
                            {
                                Refresh(SelectedInstrument.Instrument, SelectedTuning.Tuning);
                            }
                        }));
                    }
                    catch (Exception ex)
                    {
                        ExceptionUtils.HandleException(ex);
                    }
                }, () =>
                {
                    return InstrumentIsSelected && SelectedInstrument.CanEdit && TuningIsSelected && SelectedTuning.CanEdit;
                });
            }
        }

        public RelayCommand DeleteTuning
        {
            get
            {
                return new RelayCommand(() =>
                {
                    try
                    {
                        Messenger.Default.Send<ConfirmationMessage>(new ConfirmationMessage(String.Format("This will delete the tuning \"{0}\". This cannot be undone. Do you want to continue?", SelectedTuning.Name), (confirm) =>
                        {
                            try
                            {
                                if (confirm)
                                {
                                    SelectedInstrument.Instrument.Tunings.Remove(SelectedTuning.Name);
                                    Refresh(SelectedInstrument.Instrument);
                                }
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
                    return InstrumentIsSelected && SelectedInstrument.CanEdit && TuningIsSelected && SelectedTuning.CanEdit;
                });
            }
        }

        public InstrumentManagerViewModel()
        {
            Refresh();
        }

        internal void Refresh(Instrument selectedInstrument = null, Tuning selectedTuning = null)
        {
            Instruments = AppVM.GetInstruments();

            if (null == selectedInstrument)
            {
                SelectedInstrument = null;
            }
            else
            {
                foreach (ObservableInstrument oi in Instruments)
                {
                    if (oi.Instrument == selectedInstrument)
                    {
                        SelectedInstrument = oi;
                        break;
                    }
                }

                if (null != SelectedInstrument && null != selectedTuning)
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
    }
}
