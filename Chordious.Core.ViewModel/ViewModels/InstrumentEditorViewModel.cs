// 
// InstrumentEditorViewModel.cs
//  
// Author:
//       Jon Thysell <thysell@gmail.com>
// 
// Copyright (c) 2015, 2016, 2017, 2019, 2020 Jon Thysell <http://jonthysell.com>
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

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

using Chordious.Core.ViewModel.Resources;

namespace Chordious.Core.ViewModel
{
    public class InstrumentEditorViewModel : ViewModelBase
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
                RaisePropertyChanged(nameof(Name));
                Accept.RaiseCanExecuteChanged();
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
                RaisePropertyChanged(nameof(NumStrings));
                Accept.RaiseCanExecuteChanged();
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
                RaisePropertyChanged(nameof(IsNew));
                RaisePropertyChanged(nameof(Title));
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
                RaisePropertyChanged(nameof(ReadOnly));
                RaisePropertyChanged(nameof(Title));
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
