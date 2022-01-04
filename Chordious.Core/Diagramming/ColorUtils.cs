// Copyright (c) Jon Thysell <http://jonthysell.com>
// Licensed under the MIT License.

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

            result = default;
            return false;
        }

        public static bool IsColor(string s)
        {
            if (!StringUtils.IsNullOrWhiteSpace(s))
            {
                s = s.Trim();
                if (TryParseRGB(s, out _, out _, out _))
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
