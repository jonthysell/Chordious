// Copyright (c) Jon Thysell <http://jonthysell.com>
// Licensed under the MIT License.

using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Chordious.Core;

namespace Chordious.CoreTest
{
    [TestClass]
    public class MarkUtilsTest
    {
        #region AutoBarrePosition

        [TestMethod]
        public void AutoBarrePosition_NullTest()
        {
            Assert.IsNull(MarkUtils.AutoBarrePosition(null));
        }

        [TestMethod]
        public void AutoBarrePosition_NoBarreTest()
        {
            TestUtils.LoadAndExecuteTestCases<AutoBarrePositionTestCase>("MarkUtilsTest_AutoBarrePosition_NoBarreTest.csv");
        }

        [TestMethod]
        public void AutoBarrePosition_BarreTest()
        {
            TestUtils.LoadAndExecuteTestCases<AutoBarrePositionTestCase>("MarkUtilsTest_AutoBarrePosition_BarreTest.csv");
        }

        private class AutoBarrePositionTestCase : ITestCase
        {
            public int[] marks;
            public BarreTypeOption barreTypeOption;
            public bool rightToLeft;

            public BarrePosition ExpectedResult;
            public BarrePosition ActualResult;

            public void Execute()
            {
                ActualResult = MarkUtils.AutoBarrePosition(marks, barreTypeOption, rightToLeft);
                Assert.AreEqual(ExpectedResult, ActualResult);
            }

            public void Parse(string s)
            {
                if (string.IsNullOrWhiteSpace(s))
                {
                    throw new ArgumentNullException(nameof(s));
                }

                s = s.Trim();

                string[] vals = s.Split('\t');

                marks = TestUtils.ParseIntArray(vals[0]);

                barreTypeOption = (BarreTypeOption)Enum.Parse(typeof(BarreTypeOption), vals[1]);
                rightToLeft = bool.Parse(vals[2]);

                ExpectedResult = BarrePosition.Parse(vals[3]);
            }

            public override string ToString()
            {
                return string.Join("\t",
                    string.Join(",", marks),
                    barreTypeOption,
                    rightToLeft,
                    null == ExpectedResult ? "null" : ExpectedResult.ToString());
            }
        }

        #endregion
    }
}
