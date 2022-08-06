// Copyright (c) Jon Thysell <http://jonthysell.com>
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using Chordious.Core.ViewModel.Resources;

namespace Chordious.Core.ViewModel
{
    public delegate IEnumerable<ObservableNamedInterval> GetNamedIntervals();
    public delegate bool DeleteNamedInterval(NamedInterval namedInterval);

    public abstract class NamedIntervalManagerViewModel : ObservableObject
    {
        public static AppViewModel AppVM
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
                return (SelectedNamedInterval is not null);
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
                OnPropertyChanged(nameof(SelectedNamedInterval));
                OnPropertyChanged(nameof(NamedIntervalIsSelected));
                EditNamedInterval.NotifyCanExecuteChanged();
                OnPropertyChanged(nameof(EditNamedIntervalLabel));
                OnPropertyChanged(nameof(EditNamedIntervalToolTip));
                DeleteNamedInterval.NotifyCanExecuteChanged();
                OnPropertyChanged(nameof(DeleteNamedIntervalLabel));
                OnPropertyChanged(nameof(DeleteNamedIntervalToolTip));
                AddNamedInterval.NotifyCanExecuteChanged();
                OnPropertyChanged(nameof(AddNamedIntervalLabel));
                OnPropertyChanged(nameof(AddNamedIntervalToolTip));
            }
        }
        private ObservableNamedInterval _namedInterval = null;

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
                OnPropertyChanged(nameof(SelectedDefaultNamedIntervalIndex));
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
                OnPropertyChanged(nameof(DefaultNamedIntervals));
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
                OnPropertyChanged(nameof(SelectedUserNamedIntervalIndex));
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
                OnPropertyChanged(nameof(UserNamedIntervals));
            }
        }
        private ObservableCollection<ObservableNamedInterval> _userNamedIntervals;

        public static string AddNamedIntervalLabel
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

        public Action RequestClose;

        public RelayCommand Close
        {
            get
            {
                return _close ??= new RelayCommand(() =>
                {
                    try
                    {
                        RequestClose?.Invoke();
                    }
                    catch (Exception ex)
                    {
                        ExceptionUtils.HandleException(ex);
                    }
                });
            }
        }
        private RelayCommand _close;

        private readonly GetNamedIntervals _getUserNamedIntervals;
        private readonly GetNamedIntervals _getDefaultNamedIntervals;

        protected DeleteNamedInterval _deleteUserNamedInterval;

        public NamedIntervalManagerViewModel(GetNamedIntervals getDefaultNamedIntervals, GetNamedIntervals getUserNamedIntervals, DeleteNamedInterval deleteUserNamedInterval) : base()
        {
            _getDefaultNamedIntervals = getDefaultNamedIntervals;
            _getUserNamedIntervals = getUserNamedIntervals;
            _deleteUserNamedInterval = deleteUserNamedInterval;

            _defaultNamedIntervals = new ObservableCollection<ObservableNamedInterval>(_getDefaultNamedIntervals());
            _userNamedIntervals = new ObservableCollection<ObservableNamedInterval>(_getUserNamedIntervals());
        }

        protected void Refresh(NamedInterval selectedNamedInterval = null)
        {
            DefaultNamedIntervals = new ObservableCollection<ObservableNamedInterval>(_getDefaultNamedIntervals());
            UserNamedIntervals = new ObservableCollection<ObservableNamedInterval>(_getUserNamedIntervals());

            if (selectedNamedInterval is null)
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
