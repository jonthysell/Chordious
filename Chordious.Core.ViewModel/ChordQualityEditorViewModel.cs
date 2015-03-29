// 
// ChordQualityEditorViewModel.cs
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

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;

using com.jonthysell.Chordious.Core;

namespace com.jonthysell.Chordious.Core.ViewModel
{
    public class ChordQualityEditorViewModel : NamedIntervalEditorViewModel
    {
        public override string Title
        {
            get
            {
                string title = (IsNew ? "Add " : "Edit ");

                title += "Chord Quality";

                return title;
            }
        }

        public string Abbreviation
        {
            get
            {
                return _abbreviation;
            }
            set
            {
                _abbreviation = value;
                RaisePropertyChanged("Abbreviation");
                RaisePropertyChanged("Accept");
            }
        }
        private string _abbreviation;

        public override RelayCommand Accept
        {
            get
            {
                return new RelayCommand(() =>
                {
                    try
                    {
                        if (null != RequestClose)
                        {
                            RequestClose();
                        }
                        Callback(Name, Abbreviation, GetIntervalArray());
                    }
                    catch (Exception ex)
                    {
                        ExceptionUtils.HandleException(ex);
                    }
                }, () =>
                {
                    return IsValid();
                });
            }
        }

        public override RelayCommand Cancel
        {
            get
            {
                return new RelayCommand(() =>
                {
                    try
                    {
                        if (null != RequestClose)
                        {
                            RequestClose();
                        }
                    }
                    catch (Exception ex)
                    {
                        ExceptionUtils.HandleException(ex);
                    }
                });
            }
        }

        public event Action RequestClose;

        public Action<string, string, int[]> Callback
        {
            get
            {
                return _callback;
            }
            private set
            {
                if (null == value)
                {
                    throw new ArgumentNullException();
                }

                _callback = value;
            }
        }
        private Action<string, string, int[]> _callback;

        public ChordQualityEditorViewModel(bool isNew, Action<string, string, int[]> callback) : base(isNew)
        {
            Callback = callback;
        }

        public ChordQualityEditorViewModel(bool isNew, string name, string abbreviation, int[] intervals, Action<string, string, int[]> callback) : base(isNew, name, intervals)
        {
            Abbreviation = abbreviation;
            Callback = callback;
        }
    }
}
