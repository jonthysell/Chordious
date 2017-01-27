// 
// MarkPosition.cs
//  
// Author:
//       Jon Thysell <thysell@gmail.com>
// 
// Copyright (c) 2015, 2016, 2017 Jon Thysell <http://jonthysell.com>
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
    public class MarkPosition : ElementPosition
    {
        public int String
        {
            get
            {
                return _string;
            }
            private set
            {
                if (value < 1)
                {
                    throw new ArgumentOutOfRangeException();
                }
                _string = value;
            }
        }
        private int _string;

        public int Fret
        {
            get
            {
                return _fret;
            }
            private set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException();
                }
                _fret = value;
            }
        }
        private int _fret;

        public MarkPosition(int @string, int fret)
        {
            String = @string;
            Fret = fret;
        }

        public override ElementPosition Clone()
        {
            return new MarkPosition(String, Fret);
        }

        public static MarkPosition Parse(string s)
        {
            if (StringUtils.IsNullOrWhiteSpace(s))
            {
                throw new ArgumentNullException(s);
            }

            s = s.Trim();

            if (s.Equals("null", StringComparison.CurrentCultureIgnoreCase))
            {
                return null;
            }

            string[] vals = s.Split(':');

            return new MarkPosition(int.Parse(vals[0]), int.Parse(vals[1]));
        }

        public override bool Equals(ElementPosition obj)
        {
            MarkPosition mp = (obj as MarkPosition);
            return null != mp && mp.String == String && mp.Fret == Fret;
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

        public override string ToString()
        {
            return string.Format("{0}:{1}", String, Fret);
        }
    }
}
