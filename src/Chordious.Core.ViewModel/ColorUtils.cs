// Copyright (c) Jon Thysell <http://jonthysell.com>
// Licensed under the MIT License.

using System.Collections.Generic;

namespace Chordious.Core.ViewModel
{
    public static class ColorUtils
    {
        public static bool TryParseRGB(string s, out byte r, out byte g, out byte b) => Core.ColorUtils.TryParseRGB(s, out r, out g, out b);

        public static readonly IReadOnlyList<string> NamedColors = Core.ColorUtils.NamedColors;
    }
}
