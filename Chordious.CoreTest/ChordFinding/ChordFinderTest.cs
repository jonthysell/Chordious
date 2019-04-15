// 
// ChordFinderTest.cs
//  
// Author:
//       Jon Thysell <thysell@gmail.com>
// 
// Copyright (c) 2016, 2017, 2019 Jon Thysell <http://jonthysell.com>
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
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using com.jonthysell.Chordious.Core;

namespace com.jonthysell.Chordious.CoreTest
{
    [TestClass]
    [DeploymentItem("ChordFinding\\TestCases")]
    public class ChordFinderTest
    {
        [TestMethod]
        public void FindChords_NullTest()
        {
            TestUtils.ExpectInnerException<ArgumentNullException>(() =>
            {
                ChordFinder.FindChords(null);
            });
        }

        [TestMethod]
        public void FindChords_UkuleleTest()
        {
            TestUtils.LoadAndExecuteTestCases<FindChordsTestCase>("ChordFinderTest_FindChords_UkuleleTest.csv");
        }

        private class FindChordsTestCase : ITestCase
        {
            public TestChordFinderOptions chordFinderOptions;
            
            public bool allowExtras { get; set; }

            public IEnumerable<IChordFinderResult> ExpectedResult;
            public IEnumerable<IChordFinderResult> ActualResult;

            public void Execute()
            {
                ActualResult = ChordFinder.FindChords(chordFinderOptions).Results;
                TestUtils.AreEqual<IChordFinderResult>(ExpectedResult, ActualResult, allowExtras);
            }

            public void Parse(string s)
            {
                if (string.IsNullOrWhiteSpace(s))
                {
                    throw new ArgumentNullException(nameof(s));
                }

                s = s.Trim();

                string[] vals = s.Split('\t');

                chordFinderOptions = TestChordFinderOptions.Parse(vals[0]);

                allowExtras = bool.Parse(vals[1]);

                List<IChordFinderResult> expectedResult = null;

                if (vals[2] != "null")
                {
                    string[] expectedResults = vals[2].Split(';');

                    expectedResult = new List<IChordFinderResult>(expectedResults.Length);

                    foreach (string markString in expectedResults)
                    {
                        expectedResult.Add(TestChordFinderResult.Parse(markString));
                    }
                }

                ExpectedResult = expectedResult;
            }

            public override string ToString()
            {
                return string.Join("\t",
                    chordFinderOptions,
                    allowExtras,
                    TestUtils.ToString<IChordFinderResult>(ExpectedResult, ';'));
            }
        }

        private class TestChordFinderOptions : IChordFinderOptions
        {
            public IInstrument Instrument { get; set; }
            public ITuning Tuning { get; set; }

            public IChordQuality ChordQuality { get; set; }

            public Note RootNote { get; set; }

            public int NumFrets { get; set; }
            public int MaxFret { get; set; }
            public int MaxReach { get; set; }
            public bool AllowOpenStrings { get; set; }
            public bool AllowMutedStrings { get; set; }
            public bool AllowRootlessChords { get; set; }

            public static TestChordFinderOptions Parse(string s)
            {
                if (string.IsNullOrWhiteSpace(s))
                {
                    throw new ArgumentNullException(nameof(s));
                }

                s = s.Trim();

                string[] vals = s.Split(';');

                IInstrument instrument = null;
                switch (vals[0])
                {
                    case TestInstrument.UkuleleInstrumentName:
                        instrument = TestInstrument.Ukulele;
                        break;
                }

                ITuning tuning = null;
                switch (vals[1])
                {
                    case TestInstrument.UkuleleStandardTuningName:
                        tuning = TestInstrument.Ukulele.Tunings.Get(TestInstrument.UkuleleStandardTuningName);
                        break;
                }

                IChordQuality chordQuality = null;
                switch (vals[2])
                {
                    case TestChordQuality.MajorChordQualityName:
                        chordQuality = TestChordQuality.MajorChord;
                        break;
                    case TestChordQuality.Dominant7thChordQualityName:
                        chordQuality = TestChordQuality.DominantSeventhChord;
                        break;
                    case TestChordQuality.Dominant9thChordQualityName:
                        chordQuality = TestChordQuality.DominantNinthChord;
                        break;
                }
                
                Note rootNote = (Note)Enum.Parse(typeof(Note), vals[3]);

                int numFrets = int.Parse(vals[4]);
                int maxFret = int.Parse(vals[5]);
                int maxReach = int.Parse(vals[6]);
                bool allowOpenStrings = bool.Parse(vals[7]);
                bool allowMutedStrings = bool.Parse(vals[8]);
                bool allowRootlessChords = bool.Parse(vals[9]);

                return new TestChordFinderOptions { Instrument = instrument, Tuning = tuning, ChordQuality = chordQuality, RootNote = rootNote, NumFrets = numFrets, MaxFret = maxFret, MaxReach = maxReach, AllowOpenStrings = allowOpenStrings, AllowMutedStrings = allowMutedStrings, AllowRootlessChords = allowRootlessChords };
            }

            public override string ToString()
            {
                return string.Join(";",
                    null == Instrument ? "null" : Instrument.Name,
                    null == Tuning ? "null" : Tuning.Name,
                    null == ChordQuality ? "null" : ChordQuality.Name,
                    RootNote,
                    NumFrets,
                    MaxFret,
                    MaxReach,
                    AllowOpenStrings,
                    AllowMutedStrings,
                    AllowRootlessChords);
            }
        }

        private class TestChordFinderResult : IChordFinderResult
        {
            public int[] Marks { get; private set; }

            public TestChordFinderResult(int[] marks)
            {
                Marks = marks;
            }

            public Diagram ToDiagram(ChordFinderStyle chordFinderStyle)
            {
                throw new NotImplementedException();
            }

            public static TestChordFinderResult Parse(string s)
            {
                int[] marks = TestUtils.ParseIntArray(s);
                return new TestChordFinderResult(marks);
            }

            public int CompareTo(object obj)
            {
                if (null == obj)
                {
                    throw new ArgumentNullException(nameof(obj));
                }

                IChordFinderResult cfr = obj as IChordFinderResult;
                if (null == cfr)
                {
                    throw new ArgumentException();
                }

                return Marks.SequenceEqual(cfr.Marks) ? 0 : -1;
            }

            public override bool Equals(object obj)
            {
                return CompareTo(obj) == 0;
            }

            public override int GetHashCode()
            {
                return base.GetHashCode();
            }

            public override string ToString()
            {
                return TestUtils.ToString<int>(Marks);
            }
        }

        private class TestChordQuality : IChordQuality
        {
            public static TestChordQuality MajorChord
            {
                get
                {
                    return _majorChord ?? (_majorChord = new TestChordQuality(MajorChordQualityName, "", new int[] { 0, 4, 7 }));
                }
            }
            private static TestChordQuality _majorChord = null;

            public static TestChordQuality DominantSeventhChord
            {
                get
                {
                    return _dominantSeventhChord ?? (_dominantSeventhChord = new TestChordQuality(Dominant7thChordQualityName, "7", new int[] { 0, 4, 7, 10 }));
                }
            }
            private static TestChordQuality _dominantSeventhChord = null;

            public static TestChordQuality DominantNinthChord
            {
                get
                {
                    return _dominantNinthChord ?? (_dominantNinthChord = new TestChordQuality(Dominant9thChordQualityName, "9", new int[] { 0, 4, 7, 10, 14 }));
                }
            }
            private static TestChordQuality _dominantNinthChord = null;

            public NamedIntervalSet Parent
            {
                get
                {
                    throw new NotImplementedException();
                }
            }

            public string Level
            {
                get
                {
                    return "Test";
                }
            }

            public string Name { get; private set; }

            public string LongName
            {
                get
                {
                    return Name;
                }
            }

            public string Abbreviation { get; private set; }

            public int[] Intervals { get; private set; }

            public TestChordQuality(string name, string abbreviation, int[] intervals)
            {
                Name = name;
                Abbreviation = abbreviation;
                Intervals = intervals;
            }

            public const string MajorChordQualityName = "Major";
            public const string Dominant7thChordQualityName = "Dominant 7th";
            public const string Dominant9thChordQualityName = "Dominant 9th";
        }
    }
}
