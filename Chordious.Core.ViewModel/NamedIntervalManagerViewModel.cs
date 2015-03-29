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
    public delegate IEnumerable<ObservableNamedInterval> GetNamedIntervals();
    public delegate void DeleteNamedInterval(string name);

    public abstract class NamedIntervalManagerViewModel : ViewModelBase
    {
        public AppViewModel AppVM
        {
            get
            {
                return AppViewModel.Instance;
            }
        }

        public abstract string Title { get; }

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
                RaisePropertyChanged("AddNamedInterval");
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

        public abstract RelayCommand AddNamedInterval { get; }

        public abstract RelayCommand EditNamedInterval { get; }

        public RelayCommand DeleteNamedInterval
        {
            get
            {
                return new RelayCommand(() =>
                {
                    try
                    {
                        Messenger.Default.Send<ConfirmationMessage>(new ConfirmationMessage(String.Format("This will delete \"{0}\". This cannot be undone. Do you want to continue?", SelectedNamedInterval.Name), (confirm) =>
                        {
                            try
                            {
                                if (confirm)
                                {
                                    _deleteNamedInterval(SelectedNamedInterval.Name);
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

        private GetNamedIntervals _getNamedIntervals;

        private DeleteNamedInterval _deleteNamedInterval;

        public NamedIntervalManagerViewModel(GetNamedIntervals getNamedIntervals, DeleteNamedInterval deleteNamedInterval) : base()
        {
            _getNamedIntervals = getNamedIntervals;
            _deleteNamedInterval = deleteNamedInterval;
            Refresh();
        }

        internal void Refresh(NamedInterval selectedNamedInterval = null)
        {
            NamedIntervals = new ObservableCollection<ObservableNamedInterval>(_getNamedIntervals());

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
    }
}
