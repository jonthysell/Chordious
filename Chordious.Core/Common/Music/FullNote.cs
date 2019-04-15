// 
// FullNote.cs
//  
// Author:
//       Jon Thysell <thysell@gmail.com>
// 
// Copyright (c) 2015, 2017, 2019 Jon Thysell <http://jonthysell.com>
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

using com.jonthysell.Chordious.Core.Resources;

namespace com.jonthysell.Chordious.Core
{
    public class FullNote : IComparable
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
            if (steps == 0)
            {
                return new FullNote(NoteUtils.ToNote(InternalNote, style), Octave);
            }

            int direction = Math.Sign(steps);

            InternalNote note = InternalNote;
            int octave = Octave;

            for (int i = 0; i < steps; i++)
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

            return new FullNote(NoteUtils.ToNote(note, style), octave);
        }

        public int CompareTo(object obj)
        {
            if (null == obj)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            FullNote fullNote = obj as FullNote;
            if (null == fullNote)
            {
                throw new ArgumentException();
            }

            if (Octave == fullNote.Octave)
            {
                return InternalNote.CompareTo(fullNote.InternalNote);
            }

            return Octave.CompareTo(fullNote.Octave);
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

        private static readonly char[] digits = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
    }
}
