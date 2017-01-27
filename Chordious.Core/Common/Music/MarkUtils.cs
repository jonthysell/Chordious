// 
// MarkUtils.cs
//  
// Author:
//       Jon Thysell <thysell@gmail.com>
// 
// Copyright (c) 2013, 2015, 2016, 2017 Jon Thysell <http://jonthysell.com>
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
                s += MarkUtils.ToString(marks[i]) + ",";
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
            if (null == chordMarks)
            {
                throw new ArgumentNullException("chordMarks");
            }

            if (null == chordFinderOptions)
            {
                throw new ArgumentNullException("chordFinderOptions");
            }

            MarkAnalysis ma = ChordAnalysis(chordMarks);

            bool reachPass = ma.Reach <= chordFinderOptions.MaxReach;
            bool openPass = chordFinderOptions.AllowOpenStrings ? true : !ma.HasOpenStrings;
            bool mutePass = chordFinderOptions.AllowMutedStrings ? true : !ma.HasMutedStrings;

            return reachPass && openPass && mutePass;
        }

        public static bool ValidateScale(IEnumerable<MarkPosition> scaleMarks, IScaleFinderOptions scaleFinderOptions)
        {
            if (null == scaleMarks)
            {
                throw new ArgumentNullException("scaleMarks");
            }

            if (null == scaleFinderOptions)
            {
                throw new ArgumentNullException("scaleFinderOptions");
            }

            MarkAnalysis ma = ScaleAnalysis(scaleMarks, scaleFinderOptions.Instrument.NumStrings);

            bool reachPass = ma.Reach <= scaleFinderOptions.MaxReach;
            bool openPass = scaleFinderOptions.AllowOpenStrings ? true : !ma.HasOpenStrings;
            bool mutePass = scaleFinderOptions.AllowMutedStrings ? true : !ma.HasMutedStrings;

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
                int startString = -1;
                int endString = -1;

                if (targetFret > 0)
                {
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

                            if ((barreTypeOption == BarreTypeOption.Full && marks[i] >= targetFret ) ||
                                (barreTypeOption == BarreTypeOption.Partial && marks[i] == targetFret))                            {
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
            if (null == marksA)
            {
                throw new ArgumentNullException("marksA");
            }

            if (null == marksB)
            {
                throw new ArgumentNullException("marksB");
            }

            if (marksA.Length != marksB.Length)
            {
                throw new ArgumentException();
            }

            MarkAnalysis analysisA = ChordAnalysis(marksA);
            MarkAnalysis analysisB = ChordAnalysis(marksB);

            return analysisA.CompareTo(analysisB);
        }

        public static int Compare(IEnumerable<MarkPosition> marksA, IEnumerable<MarkPosition> marksB, int numStrings)
        {
            if (null == marksA)
            {
                throw new ArgumentNullException("marksA");
            }

            if (null == marksB)
            {
                throw new ArgumentNullException("marksB");
            }

            if (numStrings <= 0)
            {
                throw new ArgumentOutOfRangeException("numStrings");
            }

            MarkAnalysis analysisA = ScaleAnalysis(marksA, numStrings);
            MarkAnalysis analysisB = ScaleAnalysis(marksB, numStrings);

            return analysisA.CompareTo(analysisB);
        }

        public static int[] AbsoluteToRelativeMarks(int[] absoluteMarks, out int baseLine, int numFrets)
        {
            if (null == absoluteMarks)
            {
                throw new ArgumentNullException("absoluteMarks");
            }

            MarkAnalysis ma = ChordAnalysis(absoluteMarks);

            if (ma.Reach > numFrets)
            {
                throw new ArgumentOutOfRangeException("numFrets");
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
            if (null == marks || marks.Length == 0)
            {
                throw new ArgumentNullException("marks");
            }

            if (null == tuning)
            {
                throw new ArgumentNullException("tuning");
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
            if (null == absoluteMarks)
            {
                throw new ArgumentNullException("absoluteMarks");
            }

            MarkAnalysis ma = ScaleAnalysis(absoluteMarks, numStrings);

            if (ma.Reach > numFrets)
            {
                throw new ArgumentOutOfRangeException("numFrets");
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
            if (null == marks)
            {
                throw new ArgumentNullException("marks");
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
            if (null == marks)
            {
                throw new ArgumentNullException("marks");
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
                if (null == obj)
                {
                    throw new ArgumentException();
                }

                MarkAnalysis ma = obj as MarkAnalysis;
                if (null == ma)
                {
                    throw new ArgumentException();
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