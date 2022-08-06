﻿// Copyright (c) Jon Thysell <http://jonthysell.com>
// Licensed under the MIT License.

using System;
using System.IO;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Chordious.CoreTest
{
    public class TestUtils
    {
        public static Assembly TestAssembly => _testAssembly ??= typeof(TestUtils).GetTypeInfo().Assembly;
        private static Assembly _testAssembly;

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
            if ((expected is null) != (actual is null))
            {
                Assert.AreEqual(expected, actual);
            }

            if (expected is not null && actual is not null)
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

                    if (foundActualItem is not null)
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

        public static IEnumerable<T> Parse<T>(string s, Func<string, T> parser, char delimiter = ',')
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
            if (items is null)
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

            using (StreamReader sr = new StreamReader(GetEmbeddedResource(fileName)))
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

        private static Stream GetEmbeddedResource(string fileName)
        {
            foreach (var name in TestAssembly.GetManifestResourceNames())
            {
                if (name.EndsWith(fileName))
                {
                    return TestAssembly.GetManifestResourceStream(name);
                }
            }

            throw new FileNotFoundException();
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
