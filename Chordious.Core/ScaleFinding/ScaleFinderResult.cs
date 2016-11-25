// 
// ScaleFinderResult.cs
//  
// Author:
//       Jon Thysell <thysell@gmail.com>
// 
// Copyright (c) 2015, 2016 Jon Thysell <http://jonthysell.com>
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
using System.Collections.Generic;

namespace com.jonthysell.Chordious.Core
{
    public class ScaleFinderResult : IComparable
    {
        public IEnumerable<MarkPosition> Marks
        {
            get
            {
                return _marks;
            }
        }
        private List<MarkPosition> _marks;

        public ScaleFinderResultSet Parent { get; private set; }

        internal ScaleFinderResult(ScaleFinderResultSet parent, IEnumerable<MarkPosition> marks)
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
            _marks = new List<MarkPosition>(marks);
        }

        public InternalNote NoteAt(MarkPosition position)
        {
            return this.Parent.ScaleFinderOptions.Tuning.InternalNoteAt(position.String - 1, position.Fret);
        }

        private InternalNote NoteAt(int markPositionIndex)
        {
            return NoteAt(_marks[markPositionIndex]);
        }

        public bool IsRoot(MarkPosition mark)
        {
            if (null == mark)
            {
                throw new ArgumentOutOfRangeException("mark");
            }

            return NoteAt(mark) == NoteUtils.ToInternalNote(this.Parent.ScaleFinderOptions.RootNote);
        }

        private bool IsRoot(int markPositionIndex)
        {
            return NoteAt(markPositionIndex) == NoteUtils.ToInternalNote(this.Parent.ScaleFinderOptions.RootNote);
        }

        public Diagram ToDiagram(ScaleFinderStyle scaleFinderStyle)
        {
            int numStrings = this.Parent.ScaleFinderOptions.Instrument.NumStrings;
            int numFrets = this.Parent.ScaleFinderOptions.NumFrets;

            int baseLine;
            IEnumerable<MarkPosition> marks = MarkUtils.AbsoluteToRelativeMarks(this.Marks, out baseLine, numFrets, numStrings);

            Diagram d = new Diagram(scaleFinderStyle.Style, numStrings, numFrets);

            if (scaleFinderStyle.AddTitle)
            {
                d.Title = NoteUtils.ToString(this.Parent.ScaleFinderOptions.RootNote) + " " + this.Parent.ScaleFinderOptions.Scale.Name;
                d.Style.TitleLabelStyle = DiagramLabelStyle.Regular;
            }

            int markPositionIndex = 0;
            // Add existing marks
            foreach (MarkPosition mark in marks)
            {
                int @string = mark.String;

                if (scaleFinderStyle.MirrorResults)
                {
                    @string = numStrings - (@string - 1);
                }

                int fret = mark.Fret;
                MarkPosition mp = new MarkPosition(@string, fret);

                DiagramMark dm = d.NewMark(mp);

                // Set mark type
                if (scaleFinderStyle.AddRootNotes && IsRoot(markPositionIndex))  // Use markPositionIndex, to get the correct roots in mirror mode
                {
                    dm.Type = DiagramMarkType.Root;
                }

                if (fret == 0) // Open string
                {
                    dm.Type = (dm.Type == DiagramMarkType.Root) ? DiagramMarkType.OpenRoot : DiagramMarkType.Open;
                }

                // Add text
                dm.Text = GetText(markPositionIndex, scaleFinderStyle.MarkTextOption);  // Use markPositionIndex, not mp, to get the correct text in mirror mode

                markPositionIndex++;
            }

            // Add nut or fret label
            if (baseLine == 0)
            {
                d.Style.GridNutVisible = true;
            }
            else
            {
                d.Style.GridNutVisible = false;
                FretLabelPosition flp = new FretLabelPosition(FretLabelSide.Right, 1);
                d.NewFretLabel(flp, baseLine.ToString());
            }

            return d;
        }

        public string GetText(MarkPosition mark, MarkTextOption markTextOption)
        {
            string text = "";
            switch (markTextOption)
            {
                case MarkTextOption.ShowNote_PreferFlats:
                    text = NoteUtils.ToString(this.NoteAt(mark), InternalNoteStringStyle.PreferFlat);
                    break;
                case MarkTextOption.ShowNote_PreferSharps:
                    text = NoteUtils.ToString(this.NoteAt(mark), InternalNoteStringStyle.PreferSharp);
                    break;
                case MarkTextOption.ShowNote_ShowBoth:
                    text = NoteUtils.ToString(this.NoteAt(mark), InternalNoteStringStyle.ShowBoth);
                    break;
            }
            return text;
        }

        private string GetText(int markPositionIndex, MarkTextOption markTextOption)
        {
            return GetText(_marks[markPositionIndex], markTextOption);
        }

        public int CompareTo(object obj)
        {
            if (null == obj)
            {
                throw new ArgumentNullException("obj");
            }

            ScaleFinderResult sfr = obj as ScaleFinderResult;
            if (null == sfr)
            {
                throw new ArgumentException();
            }

            return MarkUtils.Compare(this.Marks, sfr.Marks, this.Parent.ScaleFinderOptions.Instrument.NumStrings);
        }

        public override string ToString()
        {
            return MarkUtils.ToString(this.Marks);
        }
    }
}
