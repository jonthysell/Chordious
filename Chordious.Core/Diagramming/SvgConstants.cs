// Copyright (c) Jon Thysell <http://jonthysell.com>
// Licensed under the MIT License.

namespace Chordious.Core
{
    public class SvgConstants
    {
        public const string BASE = @"<?xml version=""1.0"" encoding=""UTF-8"" standalone=""no""?>
<svg xmlns=""http://www.w3.org/2000/svg"" version=""1.1"" width=""{0}"" height=""{1}"">
<!-- {2} -->
{3}
</svg>";

        public const string ROTATE = @"
<g transform=""translate({3} {2}) rotate({1}) translate(-{2} -{3})"">{0}
</g>";

        public const string CIRCLE = @"
<circle style=""{0}"" r=""{1}"" cx=""{2}"" cy=""{3}"" />";

        public const string TEXT = @"
<text style=""{0}"" x=""{1}"" y=""{2}"">{3}</text>";

        public const string ROTATED_TEXT = @"
<text style=""{0}"" x=""{1}"" y=""{2}"" transform=""rotate(90 {1} {2})"">{3}</text>";

        public const string TEXT_CHORDNAME = @"
<text style=""{0}"" x=""{1}"" y=""{2}"">{3}<tspan style=""{4}"">{5}</tspan></text>";

        public const string ROTATED_TEXT_CHORDNAME = @"
<g transform=""rotate(90 {1} {2})""><text style=""{0}"" x=""{1}"" y=""{2}"">{3}<tspan style=""{4}"">{5}</tspan></text></g>";

        public const string RECTANGLE = @"
<rect style=""{0}"" width=""{1}"" height=""{2}"" x=""{3}"" y=""{4}"" />";

        public const string POLYGON = @"
<polygon style=""{0}"" points=""{1}"" />";

        public const string LINE = @"
<line style=""{0}"" x1=""{1}"" y1=""{2}"" x2=""{3}"" y2=""{4}"" />";

        public const string ARC = @"
<path style=""{0}"" d=""M {1} {2} A {3} {4} 0 0 1 {5} {6}"" />";

    }
}