// Copyright (c) Jon Thysell <http://jonthysell.com>
// Licensed under the MIT License.

using System;
using System.Collections.Generic;

namespace Chordious.Core
{
    public class MarkUtils
    {
        public static string ToString(int mark)
        {
            return mark < 0 ? "x" : mark.ToString();
        }

        public static string ToString(int[] marks)
        {
            string s = "";

            for (int i = 0; i < marks.Length; i++)
            {
                s += ToString(marks[i]) + ",";
            }

            s = s.TrimEnd(',');

            return s;
        }

        public static string ToString(IEnumerable<MarkPosition> marks)
        {
            string s = "";

            foreach (MarkPosition mark in marks)
            {
                s += mark.ToString() + ",";
            }

            s = s.TrimEnd(',');

            return s;
        }

        public static bool ValidateChord(int[] chordMarks, IChordFinderOptions chordFinderOptions)
        {
            if (chordMarks is null)
            {
                throw new ArgumentNullException(nameof(chordMarks));
            }

            if (chordFinderOptions is null)
            {
                throw new ArgumentNullException(nameof(chordFinderOptions));
            }

            MarkAnalysis ma = ChordAnalysis(chordMarks);

            bool reachPass = ma.Reach <= chordFinderOptions.MaxReach;
            bool openPass = chordFinderOptions.AllowOpenStrings || !ma.HasOpenStrings;
            bool mutePass = chordFinderOptions.AllowMutedStrings || !ma.HasMutedStrings;

            return reachPass && openPass && mutePass;
        }

        public static bool ValidateScale(IEnumerable<MarkPosition> scaleMarks, IScaleFinderOptions scaleFinderOptions)
        {
            if (scaleMarks is null)
            {
                throw new ArgumentNullException(nameof(scaleMarks));
            }

            if (scaleFinderOptions is null)
            {
                throw new ArgumentNullException(nameof(scaleFinderOptions));
            }

            MarkAnalysis ma = ScaleAnalysis(scaleMarks, scaleFinderOptions.Instrument.NumStrings);

            bool reachPass = ma.Reach <= scaleFinderOptions.MaxReach;
            bool openPass = scaleFinderOptions.AllowOpenStrings || !ma.HasOpenStrings;
            bool mutePass = scaleFinderOptions.AllowMutedStrings || !ma.HasMutedStrings;

            return reachPass && openPass && mutePass;
        }

        public static void MinMaxFrets(int[] marks, out int minFret, out int maxFret)
        {
            MarkAnalysis ma = ChordAnalysis(marks);
            minFret = ma.MinFret;
            maxFret = ma.MaxFret;
        }

        public static BarrePosition AutoBarrePosition(int[] marks, BarreTypeOption barreTypeOption = BarreTypeOption.None, bool leftToRight = false)
        {
            if (barreTypeOption != BarreTypeOption.None)
            {
                MarkAnalysis ma = ChordAnalysis(marks);

                int targetFret = ma.MinFret;
                if (targetFret > 0)
                {
                    int startString;
                    int endString;
                    if (leftToRight)
                    {
                        startString = 0;
                        endString = 0;
                        for (int i = startString; i < marks.Length; i++)
                        {
                            if (marks[i] < targetFret)
                            {
                                break;
                            }

                            if ((barreTypeOption == BarreTypeOption.Full && marks[i] >= targetFret) ||
                                (barreTypeOption == BarreTypeOption.Partial && marks[i] == targetFret))
                            {
                                endString = i;
                            }
                        }
                    }
                    else
                    {
                        startString = marks.Length - 1;
                        endString = marks.Length - 1;
                        for (int i = endString; i >= 0; i--)
                        {
                            if (marks[i] < targetFret)
                            {
                                break;
                            }

                            if ((barreTypeOption == BarreTypeOption.Full && marks[i] >= targetFret) ||
                                (barreTypeOption == BarreTypeOption.Partial && marks[i] == targetFret))
                            {
                                startString = i;
                            }
                        }
                    }

                    if (endString > startString)
                    {
                        return new BarrePosition(targetFret, startString + 1, endString + 1);
                    }
                }
            }

            return null;
        }

        public static int Compare(int[] marksA, int[] marksB)
        {
            if (marksA is null)
            {
                throw new ArgumentNullException(nameof(marksA));
            }

            if (marksB is null)
            {
                throw new ArgumentNullException(nameof(marksB));
            }

            if (marksA.Length != marksB.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(marksB));
            }

            MarkAnalysis analysisA = ChordAnalysis(marksA);
            MarkAnalysis analysisB = ChordAnalysis(marksB);

            return analysisA.CompareTo(analysisB);
        }

        public static int Compare(IEnumerable<MarkPosition> marksA, IEnumerable<MarkPosition> marksB, int numStrings)
        {
            if (marksA is null)
            {
                throw new ArgumentNullException(nameof(marksA));
            }

            if (marksB is null)
            {
                throw new ArgumentNullException(nameof(marksB));
            }

            if (numStrings <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(numStrings));
            }

            MarkAnalysis analysisA = ScaleAnalysis(marksA, numStrings);
            MarkAnalysis analysisB = ScaleAnalysis(marksB, numStrings);

            return analysisA.CompareTo(analysisB);
        }

        public static int[] AbsoluteToRelativeMarks(int[] absoluteMarks, out int baseLine, int numFrets)
        {
            if (absoluteMarks is null)
            {
                throw new ArgumentNullException(nameof(absoluteMarks));
            }

            MarkAnalysis ma = ChordAnalysis(absoluteMarks);

            if (ma.Reach > numFrets)
            {
                throw new ArgumentOutOfRangeException(nameof(numFrets));
            }

            int[] relativeMarks = new int[absoluteMarks.Length];

            baseLine = ma.MaxFret > numFrets ? ma.MinFret : 0;

            for (int i = 0; i < absoluteMarks.Length; i++)
            {
                relativeMarks[i] = absoluteMarks[i];
                if (relativeMarks[i] > 0 && baseLine > 0)
                {
                    relativeMarks[i] -= (baseLine - 1);
                }
            }

            return relativeMarks;
        }

        public static InternalNote?[] GetInternalNotes(int[] marks, ITuning tuning)
        {
            if (marks is null || marks.Length == 0)
            {
                throw new ArgumentNullException(nameof(marks));
            }

            if (tuning is null)
            {
                throw new ArgumentNullException(nameof(tuning));
            }

            InternalNote?[] notes = new InternalNote?[marks.Length];

            for (int i = 0; i < marks.Length; i++)
            {
                notes[i] = marks[i] == -1 ? null : (InternalNote?)NoteUtils.Shift(tuning.RootNotes[i].InternalNote, marks[i]);
            }

            return notes;
        }

        public static IEnumerable<MarkPosition> AbsoluteToRelativeMarks(IEnumerable<MarkPosition> absoluteMarks, out int baseLine, int numFrets, int numStrings)
        {
            if (absoluteMarks is null)
            {
                throw new ArgumentNullException(nameof(absoluteMarks));
            }

            MarkAnalysis ma = ScaleAnalysis(absoluteMarks, numStrings);

            if (ma.Reach > numFrets)
            {
                throw new ArgumentOutOfRangeException(nameof(numFrets));
            }

            List<MarkPosition> relativeMarks = new List<MarkPosition>();

            baseLine = ma.MaxFret > numFrets ? ma.MinFret : 0;

            foreach (MarkPosition absoluteMark in absoluteMarks)
            {
                MarkPosition relativeMark = (MarkPosition)absoluteMark.Clone();

                if (relativeMark.Fret > 0 && baseLine > 0)
                {
                    relativeMark = new MarkPosition(relativeMark.String, relativeMark.Fret - (baseLine - 1));
                }

                relativeMarks.Add(relativeMark);
            }

            return relativeMarks;
        }

        protected static MarkAnalysis ChordAnalysis(int[] marks)
        {
            if (marks is null)
            {
                throw new ArgumentNullException(nameof(marks));
            }

            MarkAnalysis ma = new MarkAnalysis();

            for (int i = 0; i < marks.Length; i++)
            {
                if (marks[i] == 0)
                {
                    ma.HasOpenStrings = true;
                }
                else if (marks[i] < 0)
                {
                    ma.HasMutedStrings = true;
                }
                else if (marks[i] > 0)
                {
                    ma.MarkCount++;
                    ma.MeanFret += marks[i];
                    ma.MinFret = Math.Min(ma.MinFret, marks[i]);
                    ma.MaxFret = Math.Max(ma.MaxFret, marks[i]);
                }
            }

            ma.MeanFret /= ma.MarkCount;

            if (ma.MinFret == int.MaxValue || ma.MaxFret == int.MinValue)
            {
                ma.MinFret = 0;
                ma.MaxFret = 0;
            }

            ma.Reach = (ma.MaxFret - ma.MinFret) + 1;

            return ma;
        }

        protected static MarkAnalysis ScaleAnalysis(IEnumerable<MarkPosition> marks, int numStrings)
        {
            if (marks is null)
            {
                throw new ArgumentNullException(nameof(marks));
            }

            MarkAnalysis ma = new MarkAnalysis();

            // First pass for basic stats
            bool[] hasMarks = new bool[numStrings];

            foreach (MarkPosition mark in marks)
            {
                int str = mark.String;
                int fret = mark.Fret;

                hasMarks[str - 1] = true;

                if (fret == 0)
                {
                    ma.HasOpenStrings = true;
                }

                if (fret > 0)
                {
                    ma.MarkCount++;
                    ma.MeanFret += fret;
                    ma.MinFret = Math.Min(ma.MinFret, fret);
                    ma.MaxFret = Math.Max(ma.MaxFret, fret);
                }
            }

            ma.MeanFret /= ma.MarkCount;

            if (ma.MinFret == int.MaxValue || ma.MaxFret == int.MinValue)
            {
                ma.MinFret = 0;
                ma.MaxFret = 0;
            }

            ma.Reach = (ma.MaxFret - ma.MinFret) + 1;

            // Check for muted strings
            for (int i = 0; i < hasMarks.Length; i++)
            {
                if (!hasMarks[i])
                {
                    ma.HasMutedStrings = true;
                    break;
                }
            }

            return ma;
        }

        protected class MarkAnalysis : IComparable
        {
            public int MinFret;
            public double MeanFret;
            public int MaxFret;
            public int Reach;
            public int MarkCount;
            public bool HasOpenStrings;
            public bool HasMutedStrings;

            public MarkAnalysis()
            {
                MinFret = int.MaxValue;
                MeanFret = 0.0;
                MaxFret = int.MinValue;
                Reach = 0;
                MarkCount = 0;
                HasOpenStrings = false;
                HasMutedStrings = false;
            }

            public int CompareTo(object obj)
            {
                if (obj is null)
                {
                    throw new ArgumentNullException(nameof(obj));
                }

                if (obj is not MarkAnalysis ma)
                {
                    throw new ArgumentOutOfRangeException(nameof(obj));
                }

                // Order by presence of muted strings
                if (HasMutedStrings && !ma.HasMutedStrings)
                {
                    return 1;
                }
                else if (ma.HasMutedStrings && !HasMutedStrings)
                {
                    return -1;
                }

                // Order by mean fret
                if (MeanFret < ma.MeanFret)
                {
                    return -1;
                }
                else if (MeanFret > ma.MeanFret)
                {
                    return 1;
                }

                // Order by mark count
                if (MarkCount < ma.MarkCount)
                {
                    return -1;
                }
                else if (MarkCount > ma.MarkCount)
                {
                    return 1;
                }

                // Order by reach
                if (Reach < ma.Reach)
                {
                    return -1;
                }
                else if (Reach > ma.Reach)
                {
                    return 1;
                }

                // Order by presence of open strings
                if (HasOpenStrings && !ma.HasOpenStrings)
                {
                    return -1;
                }
                else if (ma.HasOpenStrings && !HasOpenStrings)
                {
                    return 1;
                }

                return 0;
            }
        }
    }
}
