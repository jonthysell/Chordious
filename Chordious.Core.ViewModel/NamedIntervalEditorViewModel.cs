// 
// NamedIntervalEditorViewModel.cs
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
                RaisePropertyChanged("Accept");
            }
        }
        private string _name;

        public ObservableCollection<NamedIntervalValue> Intervals
        {
            get
            {
                return _intervals;
            }
            private set
            {
                if (null == value)
                {
                    throw new ArgumentNullException();
                }
                _intervals = value;
                RaisePropertyChanged("Intervals");
                RaisePropertyChanged("Example");
                RaisePropertyChanged("Accept");
            }
        }
        private ObservableCollection<NamedIntervalValue> _intervals;

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

        public RelayCommand AddInterval
        {
            get
            {
                return new RelayCommand(() =>
                {
                    try
                    {
                        Intervals.Add(CreateNamedIntervalValue());
                        RaisePropertyChanged("RemoveInterval");
                        RaisePropertyChanged("Example");
                        RaisePropertyChanged("Accept");
                    }
                    catch (Exception ex)
                    {
                        ExceptionUtils.HandleException(ex);
                    }
                });
            }
        }

        public RelayCommand RemoveInterval
        {
            get
            {
                return new RelayCommand(() =>
                {
                    try
                    {
                        NamedIntervalValue niValue = Intervals[Intervals.Count - 1];
                        niValue.ValueChanged -= IntervalValueChanged;
                        Intervals.Remove(niValue);
                        RaisePropertyChanged("RemoveInterval");
                        RaisePropertyChanged("Example");
                        RaisePropertyChanged("Accept");
                    }
                    catch (Exception ex)
                    {
                        ExceptionUtils.HandleException(ex);
                    }
                }, () =>
                {
                    return Intervals.Count > 0;
                });
            }
        }

        public abstract RelayCommand Accept { get; }

        public abstract RelayCommand Cancel { get; }

        public NamedIntervalEditorViewModel(bool isNew)
        {
            IsNew = isNew;
            Intervals = new ObservableCollection<NamedIntervalValue>();
        }

        public NamedIntervalEditorViewModel(bool isNew, string name, int[] intervals) : this(isNew)
        {
            if (null == intervals)
            {
                throw new ArgumentNullException("intervals");
            }

            Name = name;

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

        protected void IntervalValueChanged()
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
            return !String.IsNullOrWhiteSpace(Name) && (null != Intervals && Intervals.Count > 0);
        }
    }

    public class NamedIntervalValue : ObservableObject
    {
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
                if (null != ValueChanged)
                {
                    ValueChanged();
                }
            }
        }
        private int _value;

        public event Action ValueChanged;

        public NamedIntervalValue(int value = 0)
        {
            Value = value;
            ValueChanged += () =>
            {
                RaisePropertyChanged("Value");
            };
        }
    }
}
