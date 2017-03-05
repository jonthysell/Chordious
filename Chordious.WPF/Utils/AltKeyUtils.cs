// 
// AltKeyUtils.cs
//  
// Author:
//       Jon Thysell <thysell@gmail.com>
// 
// Copyright (c) 2017 Jon Thysell <http://jonthysell.com>
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

namespace com.jonthysell.Chordious.WPF
{
    public class AltKeyUtils
    {
        private static Dictionary<string, AltKeyEntry> _labelMap = new Dictionary<string, AltKeyEntry>();

        public static bool TryAddLabel(string labelKey, string labelValue)
        {
            try
            {
                AddLabel(labelKey, labelValue);
                return true;
            }
            catch (Exception) { }

            return false;
        }

        public static void AddLabel(string labelKey, string labelValue)
        {
            labelKey = CleanLabelKey(labelKey);

            char? altKey = FindAltKey(labelKey, labelValue);

            AltKeyEntry ake = new AltKeyEntry(labelKey, labelValue, altKey);
            _labelMap.Add(ake.LabelKey, ake);
        }

        public static bool HasLabel(string labelKey)
        {
            labelKey = CleanLabelKey(labelKey);

            return _labelMap.ContainsKey(labelKey);
        }

        public static string GetLabel(string labelKey)
        {
            return Get(labelKey).LabelValue;
        }

        public static bool TryRemove(string labelKey, bool recursive = false)
        {
            try
            {
                Remove(labelKey, recursive);
                return true;
            }
            catch (Exception) { }

            return false;
        }

        public static void Remove(string labelKey, bool recursive = false)
        {
            labelKey = CleanLabelKey(labelKey);

            if (!recursive)
            {
                _labelMap.Remove(labelKey);
            }
            else
            {
                List<string> keys = new List<string>();
                foreach(KeyValuePair<string, AltKeyEntry> kvp in _labelMap)
                {
                    if (kvp.Key.StartsWith(labelKey))
                    {
                        keys.Add(kvp.Key);
                    }
                }

                foreach (string key in keys)
                {
                    _labelMap.Remove(key);
                }
            }
        }

        private static AltKeyEntry Get(string labelKey)
        {
            labelKey = CleanLabelKey(labelKey);

            return _labelMap[labelKey];
        }

        private static char? FindAltKey(string labelKey, string labelValue)
        {
            foreach (char altKey in GetAltKeyCandidates(labelValue))
            {
                if (IsAltKeyAvailable(labelKey, altKey))
                {
                    return altKey;
                }
            }

            return null;
        }

        private static IEnumerable<char> GetAltKeyCandidates(string labelValue)
        {
            if (string.IsNullOrWhiteSpace(labelValue))
            {
                yield break;
            }

            labelValue = labelValue.Trim();

            // Look at first letters of each word
            string[] words = labelValue.Split(WordSeparators, StringSplitOptions.RemoveEmptyEntries);

            foreach (string word in words)
            {
                string cleanWord = CleanWord(word);
                if (!string.IsNullOrEmpty(cleanWord) && IsValidAltKeyCandidate(cleanWord[0]))
                {
                    yield return cleanWord[0];
                }
            }

            // Look at remaining characters in each word
            foreach (string word in words)
            {
                string cleanWord = CleanWord(word);
                if (!string.IsNullOrEmpty(cleanWord))
                {
                    for (int i = 1; i < cleanWord.Length; i++)
                    {
                        if (IsValidAltKeyCandidate(cleanWord[i]))
                        {
                            yield return cleanWord[i];
                        }
                    }
                }
            }
        }

        private static bool IsValidAltKeyCandidate(char altKey)
        {
            return AcceptableAltKeys.Contains(altKey.ToString());
        }

        private static bool IsAltKeyAvailable(string labelKey, char altKey)
        {
            if (!labelKey.Contains(KeyPartsDelimiter.ToString()))
            {
                throw new ArgumentOutOfRangeException("labelKey");
            }

            int targetScope = labelKey.Split(KeyPartsDelimiter).Length;

            int delimIndex = labelKey.LastIndexOf(KeyPartsDelimiter);
            while (delimIndex > 0)
            {
                string lookupKey = labelKey.Substring(0, delimIndex + 1);

                foreach (string key in _labelMap.Keys)
                {
                    int keyScope = key.Split(KeyPartsDelimiter).Length;
                    if (key.StartsWith(lookupKey) && targetScope == keyScope)
                    {
                        if (_labelMap[key].AltKey.HasValue &&
                            _labelMap[key].AltKey.ToString().Equals(altKey.ToString(), StringComparison.InvariantCultureIgnoreCase))
                        {
                            return false;
                        }
                    }
                }

                delimIndex = labelKey.LastIndexOf(KeyPartsDelimiter, delimIndex - 1);
                targetScope--;
            }

            return true;
        }

        private static string CleanLabelKey(string labelKey)
        {
            if (string.IsNullOrWhiteSpace(labelKey))
            {
                throw new ArgumentNullException("labelKey");
            }

            return labelKey.Trim();
        }

        private static string CleanWord(string word)
        {
            string cleanWord = "";
            foreach(char c in word)
            {
                if (Char.IsLetter(c))
                {
                    cleanWord += c;
                }
            }

            return cleanWord;
        }

        public static char AltKeyPrefix = '_';
        public static char KeyPartsDelimiter = '.';

        private static char[] WordSeparators = new char[] { ' ', '\t' };

        private static string AcceptableAltKeys = "abcdefhkmnorstuvwxzABCDEFGHIJKLMNOPQRSTUBWXYZ";

        private class AltKeyEntry
        {
            public string LabelKey { get; private set; }

            public string LabelValue { get; private set; }

            public string OriginalLabelValue { get; private set; }

            public char? AltKey { get; private set; }

            public AltKeyEntry(string labelKey, string labelValue, char? altKey)
            {
                LabelKey = AltKeyUtils.CleanLabelKey(labelKey);
                OriginalLabelValue = labelValue;
                AltKey = altKey;

                if (!altKey.HasValue)
                {
                    LabelValue = labelValue;
                }
                else
                {
                    int indexOfAlt = labelValue.IndexOf(altKey.Value);
                    LabelValue = labelValue.Insert(indexOfAlt, AltKeyUtils.AltKeyPrefix.ToString());
                }
            }
        }

    }
}
