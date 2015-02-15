// 
// SvgConstants.cs
//  
// Author:
//       Jon Thysell <thysell@gmail.com>
// 
// Copyright (c) 2015 Jon Thysell <http://jonthysell.com>
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

namespace com.jonthysell.Chordious.Core
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

        public const string TEXT_CHORDNAME = @"
<text style=""{0}"" x=""{1}"" y=""{2}"">{3}<tspan style=""{4}"">{5}</tspan></text>";

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