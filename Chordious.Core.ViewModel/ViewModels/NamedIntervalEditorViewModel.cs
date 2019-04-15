// 
// NamedIntervalEditorViewModel.cs
//  
// Author:
//       Jon Thysell <thysell@gmail.com>
// 
// Copyright (c) 2015, 2016, 2017, 2019 Jon Thysell <http://jonthysell.com>
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

using com.jonthysell.Chordious.Core.ViewModel.Resources;

namespace com.jonthysell.Chordious.Core.ViewModel
{
    public abstract class NamedIntervalEditorViewModel : ViewModelBase
    {
        public AppViewModel AppVM
        {
            get
            {
                return AppViewModel.Instance;
            }
        }

        public abstract string Title { get; }

        public string NameLabel
        {
            get
            {
                return Strings.NamedIntervalEditorNameLabel;
            }
        }

        public abstract string NameToolTip { get; }

        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
                RaisePropertyChanged("Name");
                Accept.RaiseCanExecuteChanged();
            }
        }
        private string _name;

        public string IntervalsLabel
        {
            get
            {
                return Strings.NamedIntervalEditorIntervalsLabel;
            }
        }

        public abstract string IntervalsToolTip { get; }

        public ObservableCollection<NamedIntervalValue> Intervals { get; private set; } = null;

        public string ExampleLabel
        {
            get
            {
                return Strings.NamedIntervalEditorExampleLabel;
            }
        }

        public abstract string ExampleToolTip { get; }

        public string Example
        {
            get
            {
                string example = "";

                if (Intervals.Count > 0)
                {
                    InternalNote[] notes = NamedInterval.GetNotes(InternalNote.C, GetIntervalArray());

                    for (int i = 0; i < notes.Length; i++)
                    {
                        example += NoteUtils.ToString(notes[i], InternalNoteStringStyle.ShowBoth) + " ";
                    }

                    example = example.TrimEnd();
                }

                return example;
            }
        }

        public bool IsNew
        {
            get
            {
                return _isNew;
            }
            private set
            {
                _isNew = value;
                RaisePropertyChanged("IsNew");
                RaisePropertyChanged("Title");
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
                RaisePropertyChanged("ReadOnly");
                RaisePropertyChanged("Title");
            }
        }
        private bool _readOnly;

        public string AddIntervalLabel
        {
            get
            {
                return Strings.NamedIntervalEditorAddIntervalLabel;
            }
        }

        public string AddIntervalToolTip
        {
            get
            {
                return Strings.NamedIntervalEditorAddIntervalToolTip;
            }
        }

        public RelayCommand AddInterval
        {
            get
            {
                return _addInterval ?? (_addInterval = new RelayCommand(() =>
                {
                    try
                    {
                        Intervals.Add(CreateNamedIntervalValue());
                        RemoveInterval.RaiseCanExecuteChanged();
                        RaisePropertyChanged("Example");
                        Accept.RaiseCanExecuteChanged();
                    }
                    catch (Exception ex)
                    {
                        ExceptionUtils.HandleException(ex);
                    }
                }, () =>
                {
                    return  !ReadOnly;
                }));
            }
        }
        private RelayCommand _addInterval;

        public string RemoveIntervalLabel
        {
            get
            {
                return Strings.NamedIntervalEditorRemoveIntervalLabel;
            }
        }

        public string RemoveIntervalToolTip
        {
            get
            {
                return Strings.NamedIntervalEditorRemoveIntervalToolTip;
            }
        }

        public RelayCommand RemoveInterval
        {
            get
            {
                return _removeInterval ?? (_removeInterval = new RelayCommand(() =>
                {
                    try
                    {
                        NamedIntervalValue niValue = Intervals[Intervals.Count - 1];
                        niValue.ValueChanged -= IntervalValueChanged;
                        Intervals.Remove(niValue);
                        RemoveInterval.RaiseCanExecuteChanged();
                        RaisePropertyChanged("Example");
                        Accept.RaiseCanExecuteChanged();
                    }
                    catch (Exception ex)
                    {
                        ExceptionUtils.HandleException(ex);
                    }
                }, () =>
                {
                    return Intervals.Count > 0 && !ReadOnly;
                }));
            }
        }
        private RelayCommand _removeInterval;

        public RelayCommand Accept
        {
            get
            {
                return _accept ?? (_accept = new RelayCommand(() =>
                {
                    try
                    {
                        OnAccept();
                        OnRequestClose();
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
                        OnRequestClose();
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

        protected NamedIntervalEditorViewModel(bool isNew = true, bool readOnly = false)
        {
            if (isNew && readOnly)
            {
                throw new ArgumentOutOfRangeException(nameof(readOnly));
            }

            _isNew = isNew;
            _readOnly = readOnly;
            Intervals = new ObservableCollection<NamedIntervalValue>();
        }

        public NamedIntervalEditorViewModel(string name, int[] intervals, bool readOnly) : this(false, readOnly)
        {
            if (null == intervals)
            {
                throw new ArgumentNullException(nameof(intervals));
            }

            _name = name;

            for (int i = 0; i < intervals.Length; i++)
            {
                Intervals.Add(CreateNamedIntervalValue(intervals[i]));
            }
        }

        protected NamedIntervalValue CreateNamedIntervalValue(int value = 0)
        {
            NamedIntervalValue niValue = new NamedIntervalValue(value);
            niValue.ValueChanged += IntervalValueChanged;
            return niValue;
        }

        protected void IntervalValueChanged(object sender, EventArgs e)
        {
            RaisePropertyChanged("Example");
        }

        protected int[] GetIntervalArray()
        {
            int[] array = new int[Intervals.Count];

            for (int i = 0; i < Intervals.Count; i++)
            {
                array[i] = Intervals[i].Value;
            }

            return array;
        }

        protected bool IsValid()
        {
            return !string.IsNullOrWhiteSpace(Name) && (null != Intervals && Intervals.Count > 0);
        }

        protected abstract void OnAccept();

        protected void OnRequestClose()
        {
            RequestClose?.Invoke();
        }
    }

    public delegate void ValueChangedEventHandler(object sender, EventArgs e);

    public class NamedIntervalValue : ObservableObject
    {
        public string ValueLabel
        {
            get
            {
                return Strings.NamedIntervalEditorIntervalValueLabel;
            }
        }

        public string ValueToolTip
        {
            get
            {
                return Strings.NamedIntervalEditorIntervalValueToolTip;
            }
        }

        public int Value
        {
            get
            {
                return _value;
            }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException();
                }
                _value = value;
                ValueChanged?.Invoke(this, new EventArgs());
            }
        }
        private int _value;

        public event ValueChangedEventHandler ValueChanged;

        public NamedIntervalValue(int value = 0)
        {
            Value = value;
            ValueChanged += (sender, e) =>
            {
                RaisePropertyChanged("Value");
            };
        }

        public override string ToString()
        {
            return ValueLabel;
        }
    }
}
