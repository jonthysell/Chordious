// 
// NamedIntervalManagerViewModel.cs
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
    public class NamedIntervalManagerViewModel<T> : ViewModelBase where T : NamedInterval
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
                if (IsChordQuality())
                {
                    return "Chord Qualities";
                }
                else if (IsScale())
                {
                    return "Scales";
                }

                return "Named Intervals";
            }
        }

        public bool NamedIntervalIsSelected
        {
            get
            {
                return (null != SelectedNamedInterval);
            }
        }

        public ObservableNamedInterval SelectedNamedInterval
        {
            get
            {
                return _namedInterval;
            }
            set
            {
                _namedInterval = value;
                RaisePropertyChanged("SelectedNamedInterval");
                RaisePropertyChanged("NamedIntervalIsSelected");
                RaisePropertyChanged("EditNamedInterval");
                RaisePropertyChanged("DeleteNamedInterval");
                RaisePropertyChanged("AddEditNamedInterval");
            }
        }
        private ObservableNamedInterval _namedInterval;

        public ObservableCollection<ObservableNamedInterval> NamedIntervals
        {
            get
            {
                return _namedIntervals;
            }
            private set
            {
                _namedIntervals = value;
                RaisePropertyChanged("NamedIntervals");
            }
        }
        private ObservableCollection<ObservableNamedInterval> _namedIntervals;

        public RelayCommand AddNamedInterval
        {
            get
            {
                return new RelayCommand(() =>
                {
                    try
                    {
                        Messenger.Default.Send<ShowNamedIntervalEditorMessage<T>>(new ShowNamedIntervalEditorMessage<T>(true, (name, intervals) =>
                        {
                            try
                            {
                                NamedInterval ni = null;

                                if (IsChordQuality())
                                {
                                    ni = AppVM.UserConfig.ChordQualities.Add(name, "", intervals);
                                }
                                else if (IsScale())
                                {
                                    ni = AppVM.UserConfig.Scales.Add(name, intervals);
                                }

                                Refresh(ni);
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

        public RelayCommand EditNamedInterval
        {
            get
            {
                return new RelayCommand(() =>
                {
                    try
                    {
                        Messenger.Default.Send<ShowNamedIntervalEditorMessage<ChordQuality>>(new ShowNamedIntervalEditorMessage<ChordQuality>(false, (name, intervals) =>
                        {
                            try
                            {
                                SelectedNamedInterval.NamedInterval.Name = name;
                                SelectedNamedInterval.NamedInterval.Intervals = intervals;
                                Refresh(SelectedNamedInterval.NamedInterval);
                            }
                            catch (Exception ex)
                            {
                                ExceptionUtils.HandleException(ex);
                            }
                        }, SelectedNamedInterval.Name, SelectedNamedInterval.Intervals));
                    }
                    catch (Exception ex)
                    {
                        ExceptionUtils.HandleException(ex);
                    }
                }, () =>
                {
                    return NamedIntervalIsSelected && SelectedNamedInterval.CanEdit;
                });
            }
        }

        public RelayCommand DeleteNamedInterval
        {
            get
            {
                return new RelayCommand(() =>
                {
                    try
                    {
                        string type = "the named interval";

                        if (IsChordQuality())
                        {
                            type = "the chord quality";
                        }
                        else if (IsScale())
                        {
                            type = "the scale";
                        }

                        Messenger.Default.Send<ConfirmationMessage>(new ConfirmationMessage(String.Format("This will delete {0} \"{1}\". This cannot be undone. Do you want to continue?", type, SelectedNamedInterval.Name), (confirm) =>
                        {
                            try
                            {
                                if (confirm)
                                {
                                    AppVM.UserConfig.ChordQualities.Remove(SelectedNamedInterval.Name);
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
                    return NamedIntervalIsSelected && SelectedNamedInterval.CanEdit;
                });
            }
        }

        public NamedIntervalManagerViewModel() : base()
        {
            Refresh();
        }

        internal void Refresh(NamedInterval selectedNamedInterval = null)
        {
            IEnumerable<ObservableNamedInterval> namedIntervals = null;

            if (IsChordQuality())
            {
                namedIntervals = AppVM.GetChordQualities();
            }
            else if (IsScale())
            {
                namedIntervals = AppVM.GetScales();
            }

            NamedIntervals = new ObservableCollection<ObservableNamedInterval>(namedIntervals);

            if (null == selectedNamedInterval)
            {
                SelectedNamedInterval = null;
            }
            else
            {
                foreach (ObservableNamedInterval oni in NamedIntervals)
                {
                    if (oni.NamedInterval == selectedNamedInterval)
                    {
                        SelectedNamedInterval = oni;
                        break;
                    }
                }
            }
        }

        protected bool IsChordQuality()
        {
            return (typeof(T) == typeof(ChordQuality));
        }

        protected bool IsScale()
        {
            return (typeof(T) == typeof(Scale));
        }
    }
}
