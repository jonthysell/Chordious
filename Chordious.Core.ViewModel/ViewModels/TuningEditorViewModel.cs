// Copyright (c) Jon Thysell <http://jonthysell.com>
// Licensed under the MIT License.

using System;
using System.Collections.ObjectModel;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

using Chordious.Core.ViewModel.Resources;

namespace Chordious.Core.ViewModel
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
                return IsNew ? Strings.TuningEditorNewTitle : (ReadOnly ? Strings.TuningEditorEditReadOnlyTitle : Strings.TuningEditorEditTitle);
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
                RaisePropertyChanged(nameof(Name));
                Accept.RaiseCanExecuteChanged();
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

        public ObservableCollection<ObservableNote> RootNotes { get; private set; }

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
                    return IsValid() & !ReadOnly;
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

        public bool Accepted { get; private set; } = false;

        public Action RequestClose;

        public Action<string, ObservableCollection<ObservableNote>> Callback { get; private set; }

        private TuningEditorViewModel(bool isNew, bool readOnly, Action<string, ObservableCollection<ObservableNote>> callback)
        {
            if (isNew && readOnly)
            {
                throw new ArgumentOutOfRangeException(nameof(readOnly));
            }

            _isNew = isNew;
            _readOnly = readOnly;
            Callback = callback ?? throw new ArgumentNullException(nameof(callback));

            RootNotes = new ObservableCollection<ObservableNote>();
        }

        public static TuningEditorViewModel AddNewTuning(ObservableInstrument instrument)
        {
            if (null == instrument)
            {
                throw new ArgumentNullException(nameof(instrument));
            }

            TuningEditorViewModel tuningEditorVM = new TuningEditorViewModel(true, false, (name, notes) =>
            {
                FullNote[] rootNotes = new FullNote[notes.Count];

                for (int i = 0; i < notes.Count; i++)
                {
                    rootNotes[i] = notes[i].FullNote;
                }

                instrument.Instrument.Tunings.Add(name, rootNotes);
            })
            {
                Name = (instrument.Instrument.Tunings as TuningSet)?.GetNewTuningName() ?? ""
            };

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
                throw new ArgumentNullException(nameof(tuning));
            }

            TuningEditorViewModel tuningEditorVM = new TuningEditorViewModel(false, tuning.ReadOnly, (name, notes) =>
            {
                FullNote[] rootNotes = new FullNote[notes.Count];

                for (int i = 0; i < notes.Count; i++)
                {
                    rootNotes[i] = notes[i].FullNote;
                }

                tuning.Tuning.Update(name, rootNotes);
            })
            {
                Name = tuning.Name
            };

            foreach (ObservableNote note in tuning.Notes)
            {
                tuningEditorVM.RootNotes.Add(new ObservableNote(note.FullNote.Clone()));
            }

            return tuningEditorVM;
        }

        public static TuningEditorViewModel CopyExistingTuning(ObservableTuning tuning, ObservableInstrument targetInstrument)
        {
            if (null == tuning)
            {
                throw new ArgumentNullException(nameof(tuning));
            }

            if (null == targetInstrument)
            {
                throw new ArgumentNullException(nameof(targetInstrument));
            }

            return CopyExistingTuning(tuning.Tuning, targetInstrument.Instrument);
        }


        private static TuningEditorViewModel CopyExistingTuning(Tuning tuning, Instrument targetInstrument)
        {
            if (null == tuning)
            {
                throw new ArgumentNullException(nameof(tuning));
            }

            if (null == targetInstrument)
            {
                throw new ArgumentNullException(nameof(targetInstrument));
            }

            TuningEditorViewModel tuningEditorVM = new TuningEditorViewModel(true, false, (name, notes) =>
            {
                FullNote[] rootNotes = new FullNote[notes.Count];

                for (int i = 0; i < notes.Count; i++)
                {
                    rootNotes[i] = notes[i].FullNote;
                }

                targetInstrument.Tunings.Add(name, rootNotes);
            })
            {
                Name = tuning.Name
            };

            foreach (FullNote note in tuning.RootNotes)
            {
                tuningEditorVM.RootNotes.Add(new ObservableNote(note.Clone()));
            }

            return tuningEditorVM;
        }

        private bool IsValid()
        {
            return !string.IsNullOrWhiteSpace(Name);
        }
    }
}
