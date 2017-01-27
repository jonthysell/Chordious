// 
// NamedIntervalManagerViewModel.cs
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

using System.Collections.Generic;
using System.Collections.ObjectModel;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

using com.jonthysell.Chordious.Core.ViewModel.Resources;

namespace com.jonthysell.Chordious.Core.ViewModel
{
    public delegate IEnumerable<ObservableNamedInterval> GetNamedIntervals();
    public delegate bool DeleteNamedInterval(NamedInterval namedInterval);

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

        public abstract string DefaultNamedIntervalGroupLabel { get; }
        public abstract string DefaultNamedIntervalGroupToolTip { get; }

        public abstract string UserNamedIntervalGroupLabel { get; }
        public abstract string UserNamedIntervalGroupToolTip { get; }

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
                RaisePropertyChanged("EditNamedIntervalLabel");
                RaisePropertyChanged("EditNamedIntervalToolTip");
                RaisePropertyChanged("DeleteNamedInterval");
                RaisePropertyChanged("DeleteNamedIntervalLabel");
                RaisePropertyChanged("DeleteNamedIntervalToolTip");
                RaisePropertyChanged("AddNamedInterval");
                RaisePropertyChanged("AddNamedIntervalLabel");
                RaisePropertyChanged("AddNamedIntervalToolTip");
            }
        }
        private ObservableNamedInterval _namedInterval;

        public int SelectedDefaultNamedIntervalIndex
        {
            get
            {
                return _selectedDefaultNamedIntervalIndex;
            }
            set
            {
                if (value < 0 || value >= DefaultNamedIntervals.Count)
                {
                    _selectedDefaultNamedIntervalIndex = -1;
                }
                else
                {
                    _selectedDefaultNamedIntervalIndex = value;
                    SelectedNamedInterval = DefaultNamedIntervals[_selectedDefaultNamedIntervalIndex];
                    SelectedUserNamedIntervalIndex = -1; // Unselect user named interval
                }
                RaisePropertyChanged("SelectedDefaultNamedIntervalIndex");
            }
        }
        private int _selectedDefaultNamedIntervalIndex = -1;

        public ObservableCollection<ObservableNamedInterval> DefaultNamedIntervals
        {
            get
            {
                return _defaultNamedIntervals;
            }
            private set
            {
                _defaultNamedIntervals = value;
                RaisePropertyChanged("DefaultNamedIntervals");
            }
        }
        private ObservableCollection<ObservableNamedInterval> _defaultNamedIntervals;

        public int SelectedUserNamedIntervalIndex
        {
            get
            {
                return _selectedUserNamedIntervalIndex;
            }
            set
            {
                if (value < 0 || value >= UserNamedIntervals.Count)
                {
                    _selectedUserNamedIntervalIndex = -1;
                }
                else
                {
                    _selectedUserNamedIntervalIndex = value;
                    SelectedNamedInterval = UserNamedIntervals[_selectedUserNamedIntervalIndex];
                    SelectedDefaultNamedIntervalIndex = -1; // Unselect default named interval
                }
                RaisePropertyChanged("SelectedUserNamedIntervalIndex");
            }
        }
        private int _selectedUserNamedIntervalIndex = -1;

        public ObservableCollection<ObservableNamedInterval> UserNamedIntervals
        {
            get
            {
                return _userNamedIntervals;
            }
            private set
            {
                _userNamedIntervals = value;
                RaisePropertyChanged("UserNamedIntervals");
            }
        }
        private ObservableCollection<ObservableNamedInterval> _userNamedIntervals;

        public string AddNamedIntervalLabel
        {
            get
            {
                return Strings.NewLabel;
            }
        }

        public abstract string AddNamedIntervalToolTip { get; }

        public abstract RelayCommand AddNamedInterval { get; }

        public string EditNamedIntervalLabel
        {
            get
            {
                if (NamedIntervalIsSelected)
                {
                    return string.Format(Strings.NamedIntervalManagerEditNamedIntervalLabelFormat, SelectedNamedInterval.Name);
                }

                return Strings.EditLabel;
            }
        }

        public abstract string EditNamedIntervalToolTip { get; }

        public abstract RelayCommand EditNamedInterval { get; }

        public string DeleteNamedIntervalLabel
        {
            get
            {
                if (NamedIntervalIsSelected)
                {
                    return string.Format(Strings.NamedIntervalManagerDeleteNamedIntervalLabelFormat, SelectedNamedInterval.Name);
                }

                return Strings.DeleteLabel;
            }
        }

        public abstract string DeleteNamedIntervalToolTip { get; }

        public abstract RelayCommand DeleteNamedInterval { get; }

        private GetNamedIntervals _getUserNamedIntervals;
        private GetNamedIntervals _getDefaultNamedIntervals;

        protected DeleteNamedInterval _deleteUserNamedInterval;

        public NamedIntervalManagerViewModel(GetNamedIntervals getDefaultNamedIntervals, GetNamedIntervals getUserNamedIntervals, DeleteNamedInterval deleteUserNamedInterval): base()
        {
            _getDefaultNamedIntervals = getDefaultNamedIntervals;
            _getUserNamedIntervals = getUserNamedIntervals;
            _deleteUserNamedInterval = deleteUserNamedInterval;
            Refresh();
        }

        protected void Refresh(NamedInterval selectedNamedInterval = null)
        {
            DefaultNamedIntervals = new ObservableCollection<ObservableNamedInterval>(_getDefaultNamedIntervals());
            UserNamedIntervals = new ObservableCollection<ObservableNamedInterval>(_getUserNamedIntervals());

            if (null == selectedNamedInterval)
            {
                SelectedNamedInterval = null;
            }
            else
            {
                foreach (ObservableNamedInterval oni in UserNamedIntervals)
                {
                    if (oni.NamedInterval == selectedNamedInterval)
                    {
                        SelectedNamedInterval = oni;
                        break;
                    }
                }

                foreach (ObservableNamedInterval oni in DefaultNamedIntervals)
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
