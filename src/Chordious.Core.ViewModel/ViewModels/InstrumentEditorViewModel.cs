// Copyright (c) Jon Thysell <http://jonthysell.com>
// Licensed under the MIT License.

using System;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using Chordious.Core.ViewModel.Resources;

namespace Chordious.Core.ViewModel
{
    public class InstrumentEditorViewModel : ObservableObject
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
                return IsNew ? Strings.InstrumentEditorNewTitle : (ReadOnly ? Strings.InstrumentEditorEditReadOnlyTitle : Strings.InstrumentEditorEditTitle);
            }
        }

        public string NameLabel
        {
            get
            {
                return Strings.InstrumentEditorNameLabel;
            }
        }

        public string NameToolTip
        {
            get
            {
                return Strings.InstrumentEditorNameToolTip;
            }
        }

        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
                Accept.NotifyCanExecuteChanged();
            }
        }
        private string _name;

        public string NumStringsLabel
        {
            get
            {
                return Strings.InstrumentEditorNumStringsLabel;
            }
        }

        public string NumStringsToolTip
        {
            get
            {
                return Strings.InstrumentEditorNumStringsToolTip;
            }
        }

        public int NumStrings
        {
            get
            {
                return _numStrings;
            }
            set
            {
                try
                {
                    if (value < 2)
                    {
                        throw new ArgumentOutOfRangeException();
                    }

                    _numStrings = value;
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
                OnPropertyChanged(nameof(NumStrings));
                Accept.NotifyCanExecuteChanged();
            }
        }
        private int _numStrings = 2;

        public bool IsNew
        {
            get
            {
                return _isNew;
            }
            private set
            {
                _isNew = value;
                OnPropertyChanged(nameof(IsNew));
                OnPropertyChanged(nameof(Title));
            }
        }
        private bool _isNew;

        public bool ReadOnly
        {
            get
            {
                return _readOnly;
            }
            private set
            {
                _readOnly = value;
                OnPropertyChanged(nameof(ReadOnly));
                OnPropertyChanged(nameof(Title));
            }
        }
        private bool _readOnly;

        public RelayCommand Accept
        {
            get
            {
                return _accept ?? (_accept = new RelayCommand(() =>
                {
                    try
                    {
                        Callback(Name, NumStrings);
                        RequestClose?.Invoke();
                    }
                    catch (Exception ex)
                    {
                        ExceptionUtils.HandleException(ex);
                    }
                }, () =>
                {
                    return IsValid() && !ReadOnly;
                }));
            }
        }
        private RelayCommand _accept;

        public RelayCommand Cancel
        {
            get
            {
                return _cancel ?? (_cancel = new RelayCommand(() =>
                {
                    try
                    {
                        RequestClose?.Invoke();
                    }
                    catch (Exception ex)
                    {
                        ExceptionUtils.HandleException(ex);
                    }
                }));
            }
        }
        private RelayCommand _cancel;

        public Action RequestClose;

        public Action<string, int> Callback { get; private set; }

        private InstrumentEditorViewModel(bool isNew, bool readOnly, Action<string, int> callback)
        {
            if (isNew && readOnly)
            {
                throw new ArgumentOutOfRangeException(nameof(readOnly));
            }

            _isNew = isNew;
            _readOnly = readOnly;

            Callback = callback ?? throw new ArgumentNullException(nameof(callback));
        }

        public InstrumentEditorViewModel(Action<string, int> callback) : this(true, false, callback) { }

        public InstrumentEditorViewModel(string name, int numStrings, bool readOnly, Action<string, int> callback) : this(false, readOnly, callback)
        {
            _name = name;
            _numStrings = numStrings;
        }

        private bool IsValid()
        {
            return !string.IsNullOrWhiteSpace(Name) && NumStrings > 0;
        }
    }
}
