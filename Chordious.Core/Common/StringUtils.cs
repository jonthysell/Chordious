// Copyright (c) Jon Thysell <http://jonthysell.com>
// Licensed under the MIT License.

namespace Chordious.Core
{
    public class StringUtils
    {
        public static bool IsNullOrWhiteSpace(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return true;
            }

            s = s.Trim();

            return (s == "");
        }
    }
}
