// Copyright (c) Jon Thysell <http://jonthysell.com>
// Licensed under the MIT License.

using System;
using System.Collections.Generic;

namespace Chordious.Core
{
    public class ScaleFinderResult : IScaleFinderResult
    {
        public IEnumerable<MarkPosition> Marks
        {
            get
            {
                return _marks;
            }
        }
        private readonly List<MarkPosition> _marks;

        public ScaleFinderResultSet Parent { get; private set; }

        internal ScaleFinderResult(ScaleFinderResultSet parent, IEnumerable<MarkPosition> marks)
        {
            if (marks is null)
            {
                throw new ArgumentNullException(nameof(marks));
            }

            Parent = parent ?? throw new ArgumentNullException(nameof(parent));
            _marks = new List<MarkPosition>(marks);
        }

        public InternalNote NoteAt(MarkPosition position)
        {
            return NoteUtils.Shift(Parent.ScaleFinderOptions.Tuning.RootNotes[position.String - 1].InternalNote, position.Fret);
        }

        private InternalNote NoteAt(int markPositionIndex)
        {
            return NoteAt(_marks[markPositionIndex]);
        }

        public bool IsRoot(MarkPosition mark)
        {
            if (mark is null)
            {
                throw new ArgumentOutOfRangeException(nameof(mark));
            }

            return NoteAt(mark) == NoteUtils.ToInternalNote(Parent.ScaleFinderOptions.RootNote);
        }

        private bool IsRoot(int markPositionIndex)
        {
            return NoteAt(markPositionIndex) == NoteUtils.ToInternalNote(Parent.ScaleFinderOptions.RootNote);
        }

        public Diagram ToDiagram(ScaleFinderStyle scaleFinderStyle)
        {
            int numStrings = Parent.ScaleFinderOptions.Instrument.NumStrings;
            int numFrets = Parent.ScaleFinderOptions.NumFrets;

            IEnumerable<MarkPosition> marks = MarkUtils.AbsoluteToRelativeMarks(Marks, out int baseLine, numFrets, numStrings);

            Diagram d = new Diagram(scaleFinderStyle.Style, numStrings, numFrets);

            if (scaleFinderStyle.AddTitle)
            {
                d.Title = NoteUtils.ToString(Parent.ScaleFinderOptions.RootNote) + " " + Parent.ScaleFinderOptions.Scale.Name;
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
                FretLabelPosition flp = new FretLabelPosition(scaleFinderStyle.FretLabelSide, 1);
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
                    text = NoteUtils.ToString(NoteAt(mark), InternalNoteStringStyle.PreferFlat);
                    break;
                case MarkTextOption.ShowNote_PreferSharps:
                    text = NoteUtils.ToString(NoteAt(mark), InternalNoteStringStyle.PreferSharp);
                    break;
                case MarkTextOption.ShowNote_ShowBoth:
                    text = NoteUtils.ToString(NoteAt(mark), InternalNoteStringStyle.ShowBoth);
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
            if (obj is null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            if (obj is not ScaleFinderResult sfr)
            {
                throw new ArgumentException();
            }

            return MarkUtils.Compare(Marks, sfr.Marks, Parent.ScaleFinderOptions.Instrument.NumStrings);
        }

        public override string ToString()
        {
            return MarkUtils.ToString(Marks);
        }
    }
}
