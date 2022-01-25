﻿// Copyright (c) Jon Thysell <http://jonthysell.com>
// Licensed under the MIT License.

using System;

namespace Chordious.Core
{
    public class ChordFinderResult : IChordFinderResult
    {
        public int[] Marks { get; private set; }

        public ChordFinderResultSet Parent { get; private set; }

        internal ChordFinderResult(ChordFinderResultSet parent, int[] marks)
        {
            Parent = parent ?? throw new ArgumentNullException(nameof(parent));
            Marks = marks ?? throw new ArgumentNullException(nameof(marks));
        }

        public Diagram ToDiagram(ChordFinderStyle chordFinderStyle)
        {
            int[] marks = MarkUtils.AbsoluteToRelativeMarks(Marks, out int baseLine, Parent.ChordFinderOptions.NumFrets);

            InternalNote?[] notes = MarkUtils.GetInternalNotes(Marks, Parent.ChordFinderOptions.Tuning);
            InternalNote rootNote = NoteUtils.ToInternalNote(Parent.ChordFinderOptions.RootNote);

            if (chordFinderStyle.MirrorResults)
            {
                Array.Reverse(marks);
                Array.Reverse(notes);
            }

            int numStrings = marks.Length;

            int numFrets = Parent.ChordFinderOptions.NumFrets;

            Diagram d = new Diagram(chordFinderStyle.Style, numStrings, numFrets);

            if (chordFinderStyle.AddTitle)
            {
                d.Title = NoteUtils.ToString(Parent.ChordFinderOptions.RootNote) + Parent.ChordFinderOptions.ChordQuality.Abbreviation;
                d.Style.TitleLabelStyle = DiagramLabelStyle.ChordName;
            }

            // Add marks
            for (int i = 0; i < marks.Length; i++)
            {
                int @string = i + 1;

                int fret = Math.Max(marks[i], 0);
                MarkPosition mp = new MarkPosition(@string, fret);

                DiagramMark dm = d.NewMark(mp);

                // Set mark type
                if (marks[i] == -1) // Muted string
                {
                    dm.Type = DiagramMarkType.Muted;
                }
                else if (marks[i] == 0) // Open string
                {
                    dm.Type = DiagramMarkType.Open;
                }

                // Change to root if necessary
                if (chordFinderStyle.AddRootNotes && notes[i].HasValue && notes[i].Value == rootNote)
                {
                    dm.Type = (dm.Type == DiagramMarkType.Open) ? DiagramMarkType.OpenRoot : DiagramMarkType.Root;
                }

                // Add text
                if (chordFinderStyle.MarkTextOption != MarkTextOption.None)
                {
                    dm.Text = GetText(notes, i, chordFinderStyle.MarkTextOption);
                }

                // Add bottom marks
                if (chordFinderStyle.AddBottomMarks)
                {
                    MarkPosition bottomPosition = new MarkPosition(@string, numFrets + 1);
                    DiagramMark bottomMark = d.NewMark(bottomPosition);
                    bottomMark.Type = DiagramMarkType.Bottom;
                    bottomMark.Text = GetText(notes, i, chordFinderStyle.BottomMarkTextOption);
                }
            }

            // Add nut or fret label
            if (baseLine == 0)
            {
                d.Style.GridNutVisible = true;
            }
            else
            {
                d.Style.GridNutVisible = false;
                FretLabelPosition flp = new FretLabelPosition(chordFinderStyle.FretLabelSide, 1);
                d.NewFretLabel(flp, baseLine.ToString());
            }

            // Add barre
            BarrePosition bp = MarkUtils.AutoBarrePosition(marks, chordFinderStyle.BarreTypeOption, chordFinderStyle.MirrorResults);

            if (null != bp)
            {
                d.NewBarre(bp);
            }

            return d;
        }

        private string GetText(InternalNote?[] notes, int str, MarkTextOption markTextOption)
        {
            if (str < 0 || str >= notes.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(str));
            }

            string text = "";

            if (notes[str].HasValue)
            {
                switch (markTextOption)
                {
                    case MarkTextOption.ShowNote_PreferFlats:
                        text = NoteUtils.ToString(notes[str].Value, InternalNoteStringStyle.PreferFlat);
                        break;
                    case MarkTextOption.ShowNote_PreferSharps:
                        text = NoteUtils.ToString(notes[str].Value, InternalNoteStringStyle.PreferSharp);
                        break;
                    case MarkTextOption.ShowNote_ShowBoth:
                        text = NoteUtils.ToString(notes[str].Value, InternalNoteStringStyle.ShowBoth);
                        break;
                }
            }

            return text;
        }

        public int CompareTo(object obj)
        {
            if (null == obj)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            if (!(obj is ChordFinderResult cfr))
            {
                throw new ArgumentException();
            }

            return MarkUtils.Compare(Marks, cfr.Marks);
        }

        public override string ToString()
        {
            return MarkUtils.ToString(Marks);
        }
    }
}
