// 
// ColorUtils.cs
//  
// Author:
//       Jon Thysell <thysell@gmail.com>
// 
// Copyright (c) 2015, 2017, 2019, 2020 Jon Thysell <http://jonthysell.com>
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

using Chordious.Core.Resources;

namespace Chordious.Core
{
    public static class ColorUtils
    {
        public static string ParseColor(string s, HexadecimalColorCase hexadecimalColorCase = DefaultHexadecimalColorCase)
        {
            if (StringUtils.IsNullOrWhiteSpace(s))
            {
                throw new ArgumentNullException(nameof(s));
            }

            if (!IsColor(s))
            {
                throw new ArgumentException(Strings.InvalidColorArgumentExceptionMessage);
            }

            s = s.Trim();

            if (s.StartsWith("#"))
            {
                switch (hexadecimalColorCase)
                {
                    case HexadecimalColorCase.Lower:
                        return s.ToLower();
                    case HexadecimalColorCase.Upper:
                        return s.ToUpper();
                    case HexadecimalColorCase.None:
                    default:
                        return s;
                }
            }
            else
            {
                foreach (string colorName in NamedColors)
                {
                    if (string.Equals(colorName, s, StringComparison.CurrentCultureIgnoreCase))
                    {
                        return colorName;
                    }
                }
            }

            // This should never fire since we passed the IsColor check above
            throw new ArgumentException(Strings.InvalidColorArgumentExceptionMessage);
        }

        public static bool TryParseColor(string s, out string result)
        {
            try
            {
                result = ParseColor(s);
                return true;
            }
            catch (Exception) { }

            result = default(string);
            return false;
        }

        public static bool IsColor(string s)
        {
            if (!StringUtils.IsNullOrWhiteSpace(s))
            {
                s = s.Trim();

                if (TryParseRGB(s, out byte r, out byte g, out byte b))
                {
                    return true;
                }
                else
                {
                    foreach (string colorName in NamedColors)
                    {
                        if (string.Equals(colorName, s, StringComparison.CurrentCultureIgnoreCase))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public static bool TryParseRGB(string s, out byte r, out byte g, out byte b)
        {
            try
            {
                s = s?.Trim() ?? "";
                if (s.StartsWith("#"))
                {
                    s = s.TrimStart('#');

                    string rawRed = null;
                    string rawGreen = null;
                    string rawBlue = null;

                    if (s.Length == 3)
                    {
                        rawRed = s[0].ToString() + s[0].ToString();
                        rawGreen = s[1].ToString() + s[1].ToString();
                        rawBlue = s[2].ToString() + s[2].ToString();
                    }
                    else if (s.Length == 6)
                    {
                        rawRed = s.Substring(0, 2);
                        rawGreen = s.Substring(2, 2);
                        rawBlue = s.Substring(4, 2);
                    }

                    r = Convert.ToByte(rawRed, 16);
                    g = Convert.ToByte(rawGreen, 16);
                    b = Convert.ToByte(rawBlue, 16);
                    return true;
                }
            }
            catch { }

            r = default;
            g = default;
            b = default;
            return false;
        }

        public static string[] NamedColors = new string[]
        {
            "Aqua",
            "Black",
            "Blue",
            "Fuchsia",
            "Gray",
            "Green",
            "Lime",
            "Maroon",
            "Navy",
            "Olive",
            "Purple",
            "Red",
            "Silver",
            "Teal",
            "White",
            "Yellow"
        };

        private const HexadecimalColorCase DefaultHexadecimalColorCase = HexadecimalColorCase.Upper;
    }

    public enum HexadecimalColorCase
    {
        None,
        Lower,
        Upper
    }
}