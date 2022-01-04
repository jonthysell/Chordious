// Copyright (c) Jon Thysell <http://jonthysell.com>
// Licensed under the MIT License.

using System;

using Chordious.Core.Resources;

namespace Chordious.Core
{
    public class FullNote : IComparable<FullNote>, IEquatable<FullNote>
    {
        public Note Note { get; set; }

        public InternalNote InternalNote
        {
            get
            {
                return NoteUtils.ToInternalNote(Note);
            }
        }

        public int Octave
        {
            get
            {
                return _octave;
            }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException();
                }
                _octave = value;
            }
        }
        private int _octave;

        public FullNote(Note note, int octave)
        {
            Note = note;
            Octave = octave;
        }

        public FullNote() : this(Note.C, 4) { }

        public FullNote Shift(int steps, InternalNoteStringStyle style)
        {
            FullInternalNote shiftedNote = ToFullInternalNote().Shift(steps);
            return new FullNote(NoteUtils.ToNote(shiftedNote.Note, style), shiftedNote.Octave);
        }

        public FullInternalNote ToFullInternalNote()
        {
            return new FullInternalNote(InternalNote, Octave);
        }

        public int CompareTo(FullNote other)
        {
            if (other is null)
            {
                throw new ArgumentNullException(nameof(other));
            }

            if (Octave == other.Octave)
            {
                return InternalNote.CompareTo(other.InternalNote);
            }

            return Octave.CompareTo(other.Octave);
        }

        public FullNote Clone()
        {
            return new FullNote(Note, Octave);
        }

        public override string ToString()
        {
            return string.Format("{0}{1}", NoteUtils.ToString(Note), Octave);
        }

        public static FullNote Parse(string s)
        {
            if (StringUtils.IsNullOrWhiteSpace(s))
            {
                throw new ArgumentNullException(nameof(s));
            }

            s = s.Trim();

            int splitIndex = s.IndexOfAny(digits);

            if (splitIndex <= 0)
            {
                throw new ArgumentException(Strings.InvalidNoteArgumentExceptionMessage);
            }

            string notePortion = s.Substring(0, splitIndex);
            string octavePortion = s.Substring(splitIndex);

            Note note = NoteUtils.ParseNote(notePortion);
            int octave = int.Parse(octavePortion);

            return new FullNote(note, octave);
        }

        public bool Equals(FullNote other)
        {
            if (other is null)
            {
                return false;
            }

            return Note == other.Note && Octave == other.Octave;
        }

        private static readonly char[] digits = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
    }
}
