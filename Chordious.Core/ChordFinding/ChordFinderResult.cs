// 
// ChordFinderResult.cs
//  
// Author:
//       Jon Thysell <thysell@gmail.com>
// 
// Copyright (c) 2013, 2014, 2015 Jon Thysell <http://jonthysell.com>
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

namespace com.jonthysell.Chordious.Core
{
    public class ChordFinderResult : IComparable
    {
        public int[] Marks { get; private set; }

        public ChordFinderResultSet Parent { get; private set; }

        internal ChordFinderResult(ChordFinderResultSet parent, int[] marks)
        {
            if (null == parent)
            {
                throw new ArgumentNullException("parent");
            }

            if (null == marks)
            {
                throw new ArgumentNullException("marks");
            }

            this.Parent = parent;
            this.Marks = marks;
        }

        public InternalNote NoteAt(int str)
        {
            if (str < 0 || str > this.Marks.Length)
            {
                throw new ArgumentOutOfRangeException("str");
            }

            return this.Parent.ChordFinderOptions.Tuning.NoteAt(str, Marks[str]);
        }

        public bool IsRoot(int str)
        {
            if (str < 0 || str > this.Marks.Length)
            {
                throw new ArgumentOutOfRangeException("str");
            }

            return NoteAt(str) == NoteUtils.ToInternalNote(this.Parent.ChordFinderOptions.RootNote);
        }

        public Diagram ToDiagram(ChordFinderStyle chordFinderStyle)
        {
            int baseLine;
            int[] marks = MarkUtils.AbsoluteToRelativeMarks(this.Marks, out baseLine, this.Parent.ChordFinderOptions.NumFrets);

            int numStrings = marks.Length;

            int numFrets = this.Parent.ChordFinderOptions.NumFrets;

            Diagram d = new Diagram(chordFinderStyle.Style, numStrings, numFrets);

            if (chordFinderStyle.AddTitle)
            {
                d.Title = NoteUtils.ToString(this.Parent.ChordFinderOptions.RootNote) + this.Parent.ChordFinderOptions.ChordQuality.Abbreviation;
            }

            // Add marks
            for (int i = 0; i < marks.Length; i++)
            {
                int @string = i + 1;

                if (chordFinderStyle.MirrorResults)
                {
                    @string = marks.Length - i;
                }

                int fret = Math.Max(marks[i], 0);
                MarkPosition mp = new MarkPosition(@string, fret);

                DiagramMark dm = d.NewMark(mp);

                // Set mark type
                if (chordFinderStyle.AddRootNotes && IsRoot(i))
                {
                    dm.Type = DiagramMarkType.Root;
                }

                if (marks[i] == -1) // Muted string
                {
                    dm.Type = DiagramMarkType.Muted;
                }
                else if (marks[i] == 0) // Open string
                {
                    dm.Type = (dm.Type == DiagramMarkType.Root) ? DiagramMarkType.OpenRoot : DiagramMarkType.Open;
                }

                // Add text
                dm.Text = GetText(i, chordFinderStyle.MarkTextOption);

                // Add bottom marks
                if (chordFinderStyle.AddBottomMarks)
                {
                    MarkPosition bottomPosition = new MarkPosition(@string, numFrets + 1);
                    DiagramMark bottomMark = d.NewMark(bottomPosition);
                    bottomMark.Type = DiagramMarkType.Bottom;
                    bottomMark.Text = GetText(i, chordFinderStyle.BottomMarkTextOption);
                }
            }

            // Add nut or fret label
            if (baseLine == 0)
            {
                d.GridNutVisible = true;
            }
            else
            {
                d.GridNutVisible = false;
                FretLabelPosition flp = new FretLabelPosition(FretLabelSide.Right, 1);
                d.NewFretLabel(flp, baseLine.ToString());
            }

            // Add barre
            BarrePosition bp = null;

            if (chordFinderStyle.BarreTypeOption != BarreTypeOption.None)
            {
                bp = MarkUtils.AutoBarrePosition(marks);
            }

            if (null != bp)
            {
                if (chordFinderStyle.BarreTypeOption == BarreTypeOption.Full)
                {
                    bp = new BarrePosition(bp.Fret, 1, numStrings);
                }
                d.NewBarre(bp);
            }

            return d;
        }

        private string GetText(int str, MarkTextOption markTextOption)
        {
            if (str < 0 || str > this.Marks.Length)
            {
                throw new ArgumentOutOfRangeException("str");
            }

            string text = "";
            if (this.Marks[str] >= 0)
            {
                switch (markTextOption)
                {
                    case MarkTextOption.ShowNote_PreferFlats:
                        text = NoteUtils.ToString(this.NoteAt(str), InternalNoteStringStyle.PreferFlat);
                        break;
                    case MarkTextOption.ShowNote_PreferSharps:
                        text = NoteUtils.ToString(this.NoteAt(str), InternalNoteStringStyle.PreferSharp);
                        break;
                    case MarkTextOption.ShowNote_ShowBoth:
                        text = NoteUtils.ToString(this.NoteAt(str), InternalNoteStringStyle.ShowBoth);
                        break;
                }
            }
            return text;
        }

        public int CompareTo(object obj)
        {
            if (null == obj)
            {
                throw new ArgumentException();
            }

            ChordFinderResult cfr = obj as ChordFinderResult;
            if ((object)cfr == null)
            {
                throw new ArgumentException();
            }

            return MarkUtils.Compare(this.Marks, cfr.Marks);
        }

        public override string ToString()
        {
            return MarkUtils.ToString(this.Marks);
        }
    }
}
