// Copyright (c) Jon Thysell <http://jonthysell.com>
// Licensed under the MIT License.

using System;
using System.Collections.ObjectModel;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using Chordious.Core.ViewModel.Resources;

namespace Chordious.Core.ViewModel
{
    public class ObservableNote : ObservableObject
    {
        public AppViewModel AppVM
        {
            get
            {
                return AppViewModel.Instance;
            }
        }

        public string SelectedNoteLabel
        {
            get
            {
                return Strings.ObservableNoteNoteLabel;
            }
        }

        public string SelectedNoteToolTip
        {
            get
            {
                return Strings.ObservableNoteNoteToolTip;
            }
        }

        public int SelectedNoteIndex
        {
            get
            {
                return (int)(FullNote.Note);
            }
            set
            {
                FullNote.Note = (Core.Note)(value);
                OnPropertyChanged(nameof(SelectedNoteIndex));
            }
        }

        public ObservableCollection<string> Notes
        {
            get
            {
                return ObservableEnums.GetNotes();
            }
        }

        public string OctaveLabel
        {
            get
            {
                return Strings.ObservableNoteOctaveLabel;
            }
        }

        public string OctaveToolTip
        {
            get
            {
                return Strings.ObservableNoteOctaveToolTip;
            }
        }

        public int Octave
        {
            get
            {
                return FullNote.Octave;
            }
            set
            {
                FullNote.Octave = value;
                OnPropertyChanged(nameof(Octave));
            }
        }

        internal FullNote FullNote { get; private set; }

        public ObservableNote()
        {
            FullNote = new FullNote();
        }

        internal ObservableNote(FullNote note)
        {
            FullNote = note ?? throw new ArgumentNullException(nameof(note));
        }

        public override string ToString()
        {
            return FullNote.ToString();
        }
    }
}
