// Copyright (c) Jon Thysell <http://jonthysell.com>
// Licensed under the MIT License.

using System;

namespace Chordious.Core
{
    public class FullInternalNote : IComparable<FullInternalNote>, IEquatable<FullInternalNote>
    {
        public InternalNote Note { get; set; }

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

        public FullInternalNote(InternalNote note, int octave)
        {
            Note = note;
            Octave = octave;
        }

        public FullInternalNote() : this(InternalNote.C, 4) { }

        public FullInternalNote Shift(int steps)
        {
            if (steps == 0)
            {
                return new FullInternalNote(Note, Octave);
            }

            int direction = Math.Sign(steps);

            InternalNote note = Note;
            int octave = Octave;

            for (int i = 0; i < Math.Abs(steps); i++)
            {
                int noteIndex = (int)note + direction;
                if (noteIndex < 0)
                {
                    noteIndex += 12;
                }

                note = (InternalNote)(noteIndex % 12);

                if (direction > 0 && note == InternalNote.C)
                {
                    octave++;
                }
                else if (direction < 0 && note == InternalNote.B)
                {
                    octave--;
                }
            }

            return new FullInternalNote(note, octave);
        }

        public int GetShift(FullInternalNote targetNote)
        {
            int compareTo = CompareTo(targetNote);

            if (compareTo == 0)
            {
                return 0;
            }

            FullInternalNote currentNote = Clone();

            int shiftAmount = -1 * Math.Sign(compareTo);

            int totalShift = 0;
            while (!currentNote.Equals(targetNote))
            {
                currentNote = currentNote.Shift(shiftAmount);
                totalShift += shiftAmount;
            }

            return totalShift;
        }

        public int CompareTo(FullInternalNote other)
        {
            if (other is null)
            {
                throw new ArgumentNullException(nameof(other));
            }

            if (Octave == other.Octave)
            {
                return Note.CompareTo(other.Note);
            }

            return Octave.CompareTo(other.Octave);
        }

        public bool Equals(FullInternalNote other)
        {
            if (other is null)
            {
                return false;
            }

            return Note == other.Note && Octave == other.Octave;
        }

        public FullInternalNote Clone()
        {
            return new FullInternalNote(Note, Octave);
        }

        public override string ToString()
        {
            return string.Format("{0}{1}", Note.ToString(), Octave);
        }
    }
}
