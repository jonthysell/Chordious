// 
// ScaleFinderTest.cs
//  
// Author:
//       Jon Thysell <thysell@gmail.com>
// 
// Copyright (c) 2016, 2017, 2019, 2021 Jon Thysell <http://jonthysell.com>
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

using Chordious.Core;

namespace Chordious.CoreTest
{
    [TestClass]
    [DeploymentItem("ScaleFinding\\TestCases")]
    public class ScaleFinderTest
    {
        [TestMethod]
        public void FindScales_NullTest()
        {
            TestUtils.ExpectInnerException<ArgumentNullException>(() =>
            {
                ScaleFinder.FindScales(null);
            });
        }

        [TestMethod]
        public void FindScales_UkuleleTest()
        {
            TestUtils.LoadAndExecuteTestCases<FindScalesTestCase>("ScaleFinderTest_FindScales_UkuleleTest.csv");
        }

        private class FindScalesTestCase : ITestCase
        {
            public TestScaleFinderOptions scaleFinderOptions;
            
            public bool AllowExtras { get; set; }

            public IEnumerable<IScaleFinderResult> ExpectedResult;
            public IEnumerable<IScaleFinderResult> ActualResult;

            public void Execute()
            {
                ActualResult = ScaleFinder.FindScales(scaleFinderOptions).Results;
                TestUtils.AreEqual<IScaleFinderResult>(ExpectedResult, ActualResult, AllowExtras);
            }

            public void Parse(string s)
            {
                if (string.IsNullOrWhiteSpace(s))
                {
                    throw new ArgumentNullException(nameof(s));
                }

                s = s.Trim();

                string[] vals = s.Split('\t');

                scaleFinderOptions = TestScaleFinderOptions.Parse(vals[0]);

                AllowExtras = bool.Parse(vals[1]);

                List<IScaleFinderResult> expectedResult = null;

                if (vals[2] != "null")
                {
                    string[] expectedResults = vals[2].Split(';');

                    expectedResult = new List<IScaleFinderResult>(expectedResults.Length);

                    foreach (string markString in expectedResults)
                    {
                        expectedResult.Add(TestScaleFinderResult.Parse(markString));
                    }
                }

                ExpectedResult = expectedResult;
            }

            public override string ToString()
            {
                return string.Join("\t",
                    scaleFinderOptions,
                    AllowExtras,
                    TestUtils.ToString<IScaleFinderResult>(ExpectedResult, ';'));
            }
        }

        private class TestScaleFinderOptions : IScaleFinderOptions
        {
            public IInstrument Instrument { get; set; }
            public ITuning Tuning { get; set; }

            public IScale Scale { get; set; }

            public Note RootNote { get; set; }

            public int NumFrets { get; set; }
            public int MaxFret { get; set; }
            public int MaxReach { get; set; }
            public bool AllowOpenStrings { get; set; }
            public bool AllowMutedStrings { get; set; }

            public static TestScaleFinderOptions Parse(string s)
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

                IScale scale = null;
                switch (vals[2])
                {
                    case TestScale.MajorScaleName:
                        scale = TestScale.MajorScale;
                        break;
                }
                
                Note rootNote = (Note)Enum.Parse(typeof(Note), vals[3]);

                int numFrets = int.Parse(vals[4]);
                int maxFret = int.Parse(vals[5]);
                int maxReach = int.Parse(vals[6]);
                bool allowOpenStrings = bool.Parse(vals[7]);
                bool allowMutedStrings = bool.Parse(vals[8]);

                return new TestScaleFinderOptions { Instrument = instrument, Tuning = tuning, Scale = scale, RootNote = rootNote, NumFrets = numFrets, MaxFret = maxFret, MaxReach = maxReach, AllowOpenStrings = allowOpenStrings, AllowMutedStrings = allowMutedStrings };
            }

            public override string ToString()
            {
                return string.Join(";",
                    null == Instrument ? "null" : Instrument.Name,
                    null == Tuning ? "null" : Tuning.Name,
                    null == Scale ? "null" : Scale.Name,
                    RootNote,
                    NumFrets,
                    MaxFret,
                    MaxReach,
                    AllowOpenStrings,
                    AllowMutedStrings);
            }
        }

        private class TestScaleFinderResult : IScaleFinderResult
        {
            public IEnumerable<MarkPosition> Marks { get; private set; }

            public TestScaleFinderResult(IEnumerable<MarkPosition> marks)
            {
                Marks = marks;
            }

            public Diagram ToDiagram(ScaleFinderStyle scaleFinderStyle)
            {
                throw new NotImplementedException();
            }

            public static TestScaleFinderResult Parse(string s)
            {
                IEnumerable<MarkPosition> marks = TestUtils.Parse<MarkPosition>(s, MarkPosition.Parse);
                return new TestScaleFinderResult(marks);
            }

            public int CompareTo(object obj)
            {
                if (null == obj)
                {
                    throw new ArgumentNullException(nameof(obj));
                }

                if (!(obj is IScaleFinderResult sfr))
                {
                    throw new ArgumentException();
                }

                return (Marks.Except(sfr.Marks).Count() == 0 && sfr.Marks.Except(Marks).Count() == 0) ? 0 : -1;
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
                return TestUtils.ToString<MarkPosition>(Marks);
            }
        }

        private class TestScale : IScale
        {
            public static TestScale MajorScale
            {
                get
                {
                    return _majorScale ?? (_majorScale = new TestScale(MajorScaleName, new int[] { 0, 2, 4, 5, 7, 9, 11, 12 }));
                }
            }
            private static TestScale _majorScale = null;

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

            public int[] Intervals { get; private set; }

            public TestScale(string name, int[] intervals)
            {
                Name = name;
                Intervals = intervals;
            }

            public const string MajorScaleName = "Major";
        }
    }
}
