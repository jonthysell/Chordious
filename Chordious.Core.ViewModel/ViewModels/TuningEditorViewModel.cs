// 
// TuningEditorViewModel.cs
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

using System;
using System.Collections.ObjectModel;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

using com.jonthysell.Chordious.Core.ViewModel.Resources;

namespace com.jonthysell.Chordious.Core.ViewModel
{
    public class TuningEditorViewModel : ViewModelBase
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
                return IsNew ? Strings.TuningEditorNewTitle : Strings.TuningEditorEditTitle;
            }
        }

        public string NameLabel
        {
            get
            {
                return Strings.TuningEditorNameLabel;
            }
        }

        public string NameToolTip
        {
            get
            {
                return Strings.TuningEditorNameToolTip;
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
                RaisePropertyChanged("Name");
                RaisePropertyChanged("Accept");
            }
        }
        private string _name;

        public string RootNotesLabel
        {
            get
            {
                return Strings.TuningEditorRootNotesLabel;
            }
        }

        public string RootNotesToolTip
        {
            get
            {
                return Strings.TuningEditorRootNotesToolTip;
            }
        }

        public ObservableCollection<ObservableNote> RootNotes
        {
            get
            {
                return _notes;
            }
            private set
            {
                _notes = value;
                RaisePropertyChanged("RootNotes");
            }
        }
        private ObservableCollection<ObservableNote> _notes;

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

        public RelayCommand Accept
        {
            get
            {
                return new RelayCommand(() =>
                {
                    try
                    {
                        Callback(Name, RootNotes);

                        Accepted = true;
                        RequestClose?.Invoke();
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

        public RelayCommand Cancel
        {
            get
            {
                return new RelayCommand(() =>
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

        public bool Accepted { get; private set; }

        public event Action RequestClose;

        public Action<string, ObservableCollection<ObservableNote>> Callback { get; private set; }

        private TuningEditorViewModel(bool isNew, Action<string, ObservableCollection<ObservableNote>> callback)
        {
            if (null == callback)
            {
                throw new ArgumentNullException("callback");
            }

            IsNew = isNew;
            Callback = callback;

            RootNotes = new ObservableCollection<ObservableNote>();

            Accepted = false;
        }

        public static TuningEditorViewModel AddNewTuning(ObservableInstrument instrument)
        {
            if (null == instrument)
            {
                throw new ArgumentNullException("instrument");
            }

            TuningEditorViewModel tuningEditorVM = new TuningEditorViewModel(true, (name, notes) =>
                {
                    FullNote[] rootNotes = new FullNote[notes.Count];

                    for (int i = 0; i < notes.Count; i++)
                    {
                        rootNotes[i] = notes[i].FullNote;
                    }

                    instrument.Instrument.Tunings.Add(name, rootNotes);
                });

            for (int i = 0; i < instrument.NumStrings; i++)
            {
                tuningEditorVM.RootNotes.Add(new ObservableNote());
            }

            return tuningEditorVM;
        }

        public static TuningEditorViewModel EditExistingTuning(ObservableTuning tuning)
        {
            if (null == tuning)
            {
                throw new ArgumentNullException("tuning");
            }

            TuningEditorViewModel tuningEditorVM = new TuningEditorViewModel(false, (name, notes) =>
            {
                FullNote[] rootNotes = new FullNote[notes.Count];

                for (int i = 0; i < notes.Count; i++)
                {
                    rootNotes[i] = notes[i].FullNote;
                }

                tuning.Tuning.Update(name, rootNotes);
            });

            tuningEditorVM.Name = tuning.Name;

            foreach (ObservableNote note in tuning.Notes)
            {
                tuningEditorVM.RootNotes.Add(new ObservableNote(note.FullNote.Clone()));
            }

            return tuningEditorVM;
        }

        private bool IsValid()
        {
            return !string.IsNullOrWhiteSpace(Name);
        }
    }
}
