// 
// FretLabelPosition.cs
//  
// Author:
//       Jon Thysell <thysell@gmail.com>
// 
// Copyright (c) 2015, 2017, 2019 Jon Thysell <http://jonthysell.com>
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

namespace Chordious.Core
{
    public class FretLabelPosition : ElementPosition
    {
        public FretLabelSide Side { get; private set; }

        public int Fret
        {
            get
            {
                return _fret;
            }
            private set
            {
                if (value < 1)
                {
                    throw new ArgumentOutOfRangeException();
                }
                _fret = value;
            }
        }
        private int _fret;

        public FretLabelPosition(FretLabelSide side, int fret)
        {
            Side = side;
            Fret = fret;
        }

        public override ElementPosition Clone()
        {
            return new FretLabelPosition(Side, Fret);
        }

        public override bool Equals(ElementPosition obj)
        {
            FretLabelPosition flp = (obj as FretLabelPosition);
            return null != flp && flp.Side == Side && flp.Fret == Fret;
        }

        public override int GetHashCode()
        {
            return string.Format("{0}:{1}", Side, Fret).GetHashCode();
        }
    }

    public enum FretLabelSide
    {
        Left,
        Right
    }
}
