// 
// BarrePosition.cs
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
    public class BarrePosition : ElementPosition
    {
        public int Fret
        {
            get
            {
                return this._fret;
            }
            private set
            {
                if (value < 1)
                {
                    throw new ArgumentOutOfRangeException();
                }
                this._fret = value;
            }
        }
        private int _fret;

        public int StartString
        {
            get
            {
                return this._startString;
            }
            private set
            {
                if (value < 1)
                {
                    throw new ArgumentOutOfRangeException();
                }
                this._startString = value;
            }
        }
        private int _startString;

        public int EndString
        {
            get
            {
                return this._endString;
            }
            private set
            {
                if (value < 1)
                {
                    throw new ArgumentOutOfRangeException();
                }
                this._endString = value;
            }
        }
        private int _endString;

        public int Width
        {
            get
            {
                return (EndString - StartString) + 1;
            }
        }

        public BarrePosition(int fret, int startString, int endString)
        {
            this.Fret = fret;

            if (startString >= endString)
            {
                throw new BarrePositionInvalidSpanException(startString, endString);
            }

            this.StartString = startString;
            this.EndString = endString;
        }

        public override ElementPosition Clone()
        {
            return new BarrePosition(this.Fret, this.StartString, this.EndString);
        }

        public bool Contains(int fret, int @string)
        {
            if (fret < 1)
            {
                throw new ArgumentOutOfRangeException("fret");
            }

            if (@string < 1)
            {
                throw new ArgumentOutOfRangeException("string");
            }

            if (this.Fret == fret)
            {
                if (this.StartString <= @string && this.EndString >= @string)
                {
                    return true;
                }
            }

            return false;
        }

        public bool Overlaps(BarrePosition position)
        {
            if (null == position)
            {
                throw new ArgumentNullException("position");
            }

            if (this.Equals(position))
            {
                return true;
            }

            if (this.Fret == position.Fret)
            {
                if (this.StartString == position.StartString || this.EndString == position.EndString)
                {
                    return true;
                }
                else if (this.StartString < position.StartString)
                {
                    if (this.EndString >= position.StartString)
                    {
                        return true;
                    }
                }
                else if (this.StartString > position.StartString)
                {
                    if (position.EndString >= this.StartString)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public override bool Equals(ElementPosition obj)
        {
            BarrePosition bp = (obj as BarrePosition);
            return bp != null && bp.Fret == this.Fret && bp.StartString == this.StartString && bp.EndString == this.EndString;
        }

        public override int GetHashCode()
        {
            return string.Format("{0}:{1}:{2}", this.Fret, this.StartString, this.EndString).GetHashCode();
        }
    }

    public class BarrePositionInvalidSpanException : ElementPositionException
    {
        public int AttemptedStartString { get; private set; }
        public int AttemptedEndString { get; private set; }

        public override string Message
        {
            get
            {
                return String.Format(Resources.Strings.BarrePositionInvalidSpanExceptionMessage, AttemptedStartString, AttemptedEndString);
            }
        }

        public BarrePositionInvalidSpanException(int startString, int endString) : base()
        {
            this.AttemptedStartString = startString;
            this.AttemptedEndString = endString;
        }
    }
}
