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
using System.Collections.Generic;
using System.IO;
using System.Text;

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
            LoadAndExecuteAutoBarrePositionTestCases("MarkUtilsTest_AutoBarrePosition_NoBarreTest.csv");
        }

        [TestMethod]
        public void AutoBarrePosition_BarreTest()
        {
            LoadAndExecuteAutoBarrePositionTestCases("MarkUtilsTest_AutoBarrePosition_BarreTest.csv");
        }

        private void LoadAndExecuteAutoBarrePositionTestCases(string fileName)
        {
            IEnumerable<AutoBarrePositionTestCase> testCases = LoadAutoBarrePositionTestCases(fileName);
            ExecuteAutoBarrePositionTestCases(testCases);
        }

        private IEnumerable<AutoBarrePositionTestCase> LoadAutoBarrePositionTestCases(string fileName)
        {
            List<AutoBarrePositionTestCase> testCases = new List<AutoBarrePositionTestCase>();

            using (StreamReader sr = new StreamReader(fileName))
            {
                string line;
                while (null != (line = sr.ReadLine()))
                {
                    line = line.Trim();
                    if (!String.IsNullOrWhiteSpace(line) && !line.StartsWith("#"))
                    {
                        testCases.Add(AutoBarrePositionTestCase.Parse(line));
                    }
                }
            }

            return testCases;
        }

        private void ExecuteAutoBarrePositionTestCases(IEnumerable<AutoBarrePositionTestCase> testCases)
        {
            List<AutoBarrePositionTestCase> failedTestCases = new List<AutoBarrePositionTestCase>();

            foreach (AutoBarrePositionTestCase testCase in testCases)
            {
                testCase.ActualResult = MarkUtils.AutoBarrePosition(testCase.marks, testCase.barreTypeOption, testCase.rightToLeft);
                if (testCase.ExpectedResult != testCase.ActualResult)
                {
                    failedTestCases.Add(testCase);
                }
            }

            if (failedTestCases.Count > 0)
            {
                StringBuilder sb = new StringBuilder();

                sb.AppendLine(String.Format("{0} test cases failed:", failedTestCases.Count));

                foreach (AutoBarrePositionTestCase failedTestCase in failedTestCases)
                {
                    sb.AppendLine(String.Format("Expected: <{3}>, Actual: <{4}> for test case \"{0};{1};{2};{3}\"",
                        String.Join(",", failedTestCase.marks),
                        failedTestCase.barreTypeOption,
                        failedTestCase.rightToLeft,
                        failedTestCase.ExpectedResult == null ? "null" : failedTestCase.ExpectedResult.ToString(),
                        failedTestCase.ActualResult == null ? "null" : failedTestCase.ActualResult.ToString()
                        ));
                }

                Assert.Fail(sb.ToString());
            }
        }

        private class AutoBarrePositionTestCase
        {
            public int[] marks;
            public BarreTypeOption barreTypeOption;
            public bool rightToLeft;

            public BarrePosition ExpectedResult;
            public BarrePosition ActualResult;

            public static AutoBarrePositionTestCase Parse(string s)
            {
                if (String.IsNullOrWhiteSpace(s))
                {
                    throw new ArgumentNullException("s");
                }

                s = s.Trim();

                string[] vals = s.Split('\t');

                int[] marks = TestUtils.ParseIntArray(vals[0]);

                BarreTypeOption barreTypeOption = (BarreTypeOption)Enum.Parse(typeof(BarreTypeOption), vals[1]);
                bool rightToLeft = Boolean.Parse(vals[2]);

                BarrePosition ExpectedResult = BarrePosition.Parse(vals[3]);

                return new AutoBarrePositionTestCase { marks = marks, barreTypeOption = barreTypeOption, rightToLeft = rightToLeft, ExpectedResult = ExpectedResult };
            }
        }

        #endregion
    }
}
