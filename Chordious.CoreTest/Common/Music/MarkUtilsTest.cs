// 
// MarkUtilsTest.cs
//  
// Author:
//       Jon Thysell <thysell@gmail.com>
// 
// Copyright (c) 2016 Jon Thysell <http://jonthysell.com>
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

using Microsoft.VisualStudio.TestTools.UnitTesting;

using com.jonthysell.Chordious.Core;

namespace com.jonthysell.Chordious.CoreTest
{
    [TestClass]
    [DeploymentItem("Common\\Music\\TestCases")]
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
                    throw new ArgumentNullException("s");
                }

                s = s.Trim();

                string[] vals = s.Split('\t');

                marks = TestUtils.ParseIntArray(vals[0]);

                barreTypeOption = (BarreTypeOption)Enum.Parse(typeof(BarreTypeOption), vals[1]);
                rightToLeft = Boolean.Parse(vals[2]);

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
