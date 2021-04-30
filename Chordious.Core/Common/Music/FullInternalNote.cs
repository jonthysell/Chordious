// 
// FullInternalNote.cs
//  
// Author:
//       Jon Thysell <thysell@gmail.com>
// 
// Copyright (c) 2021 Jon Thysell <http://jonthysell.com>
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
