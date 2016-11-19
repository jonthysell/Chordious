// 
// MarkUtils.cs
//  
// Author:
//       Jon Thysell <thysell@gmail.com>
// 
// Copyright (c) 2013, 2015 Jon Thysell <http://jonthysell.com>
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

        public static bool ValidateChord(int[] chordMarks, ChordFinderOptions chordFinderOptions)
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

        public static bool ValidateScale(IEnumerable<MarkPosition> scaleMarks, ScaleFinderOptions scaleFinderOptions)
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

        public static BarrePosition AutoBarrePosition(int[] marks, bool mirroredResult = false)
        {
            MarkAnalysis ma = ChordAnalysis(marks);

            if (mirroredResult)
            {
                // Flip the barre to the correct side
                int oldStart = ma.AutoBarreStartString;
                int oldEnd = ma.AutoBarreEndString;

                ma.AutoBarreStartString = marks.Length - 1 - oldEnd;
                ma.AutoBarreEndString = marks.Length - 1 - oldStart;
            }

            if (ma.AutoBarreFret > 0)
            {
                return new BarrePosition(ma.AutoBarreFret, ma.AutoBarreStartString + 1, ma.AutoBarreEndString + 1);
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

            int firstMark = -1;
            int lastMark = -1;

            bool onBarre = false;

            for (int i = 0; i < marks.Length; i++)
            {
                if (marks[i] == 0)
                {
                    ma.HasOpenStrings = true;
                    onBarre = false;
                }
                else if (marks[i] < 0)
                {
                    ma.HasMutedStrings = true;
                    onBarre = false;
                }
                else if (marks[i] > 0)
                {
                    ma.MarkCount++;
                    ma.MeanFret += marks[i];
                    ma.MinFret = Math.Min(ma.MinFret, marks[i]);
                    ma.MaxFret = Math.Max(ma.MaxFret, marks[i]);

                    if (!onBarre)
                    {
                        firstMark = i;
                        onBarre = true;
                    }

                    if (onBarre)
                    {
                        if (marks[firstMark] > marks[i])
                        {
                            firstMark = i;
                        }
                        lastMark = i;
                    }
                }
            }

            ma.MeanFret /= ma.MarkCount;

            if (ma.MinFret == Int32.MaxValue || ma.MaxFret == Int32.MinValue)
            {
                ma.MinFret = 0;
                ma.MaxFret = 0;
            }

            ma.Reach = (ma.MaxFret - ma.MinFret) + 1;

            // Calculate auto-barre
            if (firstMark >= 0 && lastMark >= 0 && firstMark != lastMark)
            {
                ma.AutoBarreFret = Math.Min(marks[firstMark], marks[lastMark]);
                ma.AutoBarreStartString = firstMark;
                ma.AutoBarreEndString = lastMark;
            }

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

                if (fret == 0)
                {
                    ma.HasOpenStrings = true;
                }

                if (fret >= 0)
                {
                    hasMarks[str - 1] = true;

                    ma.MarkCount++;
                    ma.MeanFret += fret;
                    ma.MinFret = Math.Min(ma.MinFret, fret);
                    ma.MaxFret = Math.Max(ma.MaxFret,fret);
                }
            }

            ma.MeanFret /= ma.MarkCount;

            if (ma.MinFret == Int32.MaxValue || ma.MaxFret == Int32.MinValue)
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
            public int AutoBarreFret;
            public int AutoBarreStartString;
            public int AutoBarreEndString;
            public bool HasOpenStrings;
            public bool HasMutedStrings;

            public MarkAnalysis()
            {
                MinFret = Int32.MaxValue;
                MeanFret = 0.0;
                MaxFret = Int32.MinValue;
                Reach = 0;
                MarkCount = 0;
                AutoBarreFret = Int32.MinValue;
                AutoBarreStartString = Int32.MinValue;
                AutoBarreEndString = Int32.MinValue;
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
                if ((object)ma == null)
                {
                    throw new ArgumentException();
                }

                // Order by presence of muted strings
                if (this.HasMutedStrings && !ma.HasMutedStrings)
                {
                    return 1;
                }
                else if (ma.HasMutedStrings && !this.HasMutedStrings)
                {
                    return -1;
                }

                // Order by mean fret
                if (this.MeanFret < ma.MeanFret)
                {
                    return -1;
                }
                else if (this.MeanFret > ma.MeanFret)
                {
                    return 1;
                }

                // Order by mark count
                if (this.MarkCount < ma.MarkCount)
                {
                    return -1;
                }
                else if (this.MarkCount > ma.MarkCount)
                {
                    return 1;
                }

                // Order by reach
                if (this.Reach < ma.Reach)
                {
                    return -1;
                }
                else if (this.Reach > ma.Reach)
                {
                    return 1;
                }

                // Order by presence of open strings
                if (this.HasOpenStrings && !ma.HasOpenStrings)
                {
                    return -1;
                }
                else if (ma.HasOpenStrings && !this.HasOpenStrings)
                {
                    return 1;
                }

                return 0;
            }
        }
    }
}