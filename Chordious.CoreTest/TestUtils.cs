// 
// TestUtils.cs
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
using System.IO;
using System.Collections.Generic;
using System.Text;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Chordious.CoreTest
{
    public class TestUtils
    {
        public static void ExpectInnerException<T>(Action action) where T : Exception
        {
            try
            {
                action();
                Assert.Fail("Exception of type {0} not thrown.", typeof(T));
            }
            catch (AggregateException ex)
            {
                Assert.IsInstanceOfType(ex.InnerException, typeof(T));
            }
        }

        public static void AreEqual<T>(IEnumerable<T> expected, IEnumerable<T> actual, bool allowExtras = false) where T : class
        {
            if ((null == expected) != (null == actual))
            {
                Assert.AreEqual(expected, actual);
            }

            if (null != expected && null != actual)
            {
                Queue<T> expectedList = new Queue<T>(expected);
                List<T> actualList = new List<T>(actual);

                List<T> missingItems = new List<T>();

                while (expectedList.Count > 0)
                {
                    T expectedItem = expectedList.Dequeue();

                    T foundActualItem = null;

                    foreach (T actualItem in actualList)
                    {
                        if (expectedItem.Equals(actualItem))
                        {
                            foundActualItem = actualItem;
                            break;
                        }
                    }

                    if (null != foundActualItem)
                    {
                        actualList.Remove(foundActualItem);
                    }
                    else
                    {
                        missingItems.Add(expectedItem);
                    }
                }

                if (missingItems.Count > 0)
                {
                    StringBuilder sb = new StringBuilder();

                    sb.AppendLine(string.Format("{0} items missing from expected:", missingItems.Count));

                    foreach (T item in missingItems)
                    {
                        sb.AppendLine(item.ToString());
                    }

                    Assert.Fail(sb.ToString());
                }

                if (actualList.Count > 0 && !allowExtras)
                {
                    StringBuilder sb = new StringBuilder();

                    sb.AppendLine(string.Format("{0} items extra in actual:", actualList.Count));

                    foreach (T item in actualList)
                    {
                        sb.AppendLine(item.ToString());
                    }

                    Assert.Fail(sb.ToString());
                }
            }
        }

        public static int[] ParseIntArray(string s, char delimiter = ',')
        {
            if (string.IsNullOrWhiteSpace(s))
            {
                throw new ArgumentNullException(nameof(s));
            }

            s = s.Trim();

            string[] arrayVals = s.Split(delimiter);

            int[] array = new int[arrayVals.Length];
            for (int i = 0; i < arrayVals.Length; i++)
            {
                array[i] = int.Parse(arrayVals[i]);
            }

            return array;
        }

        public static IEnumerable<T> Parse<T>(string s, Func<string,T> parser, char delimiter = ',')
        {
            if (string.IsNullOrWhiteSpace(s))
            {
                throw new ArgumentNullException(nameof(s));
            }

            s = s.Trim();

            string[] arrayVals = s.Split(delimiter);

            T[] array = new T[arrayVals.Length];
            for (int i = 0; i < arrayVals.Length; i++)
            {
                array[i] = parser(arrayVals[i]);
            }

            return array;
        }

        public static string ToString<T>(IEnumerable<T> items, char delimiter = ',')
        {
            if (null == items)
            {
                return "null";
            }

            StringBuilder sb = new StringBuilder();
            foreach (T item in items)
            {
                sb.Append(item.ToString());
                sb.Append(delimiter);
            }

            return sb.ToString().TrimEnd(delimiter);
        }

        public static void LoadAndExecuteTestCases<T>(string fileName) where T : ITestCase, new()
        {
            IEnumerable<T> testCases = LoadTestCases<T>(fileName);
            ExecuteTestCases<T>(testCases);
        }

        public static IEnumerable<T> LoadTestCases<T>(string fileName) where T : ITestCase, new()
        {
            List<T> testCases = new List<T>();

            using (StreamReader sr = new StreamReader(fileName))
            {
                string line;
                while (null != (line = sr.ReadLine()))
                {
                    line = line.Trim();
                    if (!string.IsNullOrWhiteSpace(line) && !line.StartsWith("#"))
                    {
                        T testCase = new T();
                        testCase.Parse(line);
                        testCases.Add(testCase);
                    }
                }
            }

            return testCases;
        }

        public static void ExecuteTestCases<T>(IEnumerable<T> testCases) where T : ITestCase, new()
        {
            List<T> failedTestCases = new List<T>();
            StringBuilder failMessages = new StringBuilder();

            foreach (T testCase in testCases)
            {
                try
                {
                    testCase.Execute();
                }
                catch (AssertFailedException ex)
                {
                    failedTestCases.Add(testCase);
                    failMessages.AppendLine(string.Format("Test case \"{0}\" failed:", testCase));
                    failMessages.AppendLine(ex.Message);
                }
            }

            if (failedTestCases.Count > 0)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine(string.Format("{0} test cases failed:", failedTestCases.Count));

                sb.Append(failMessages.ToString());

                Assert.Fail(sb.ToString());
            }
        }
    }

    public interface ITestCase
    {
        void Execute();
        void Parse(string s);
    }
}
