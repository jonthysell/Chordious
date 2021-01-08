﻿// 
// ElementPosition.cs
//  
// Author:
//       Jon Thysell <thysell@gmail.com>
// 
// Copyright (c) 2015, 2017, 2019, 2021 Jon Thysell <http://jonthysell.com>
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
    public abstract class ElementPosition : IEquatable<ElementPosition>
    {
        public abstract ElementPosition Clone();

        public override bool Equals(object obj)
        {
            return Equals(obj as ElementPosition);
        }

        public static bool operator ==(ElementPosition a, ElementPosition b)
        {
            if (a is null)
            {
                return b is null;
            }

            return a.Equals(b);
        }
        public static bool operator !=(ElementPosition a, ElementPosition b)
        {
            return !(a == b);
        }

        public abstract bool Equals(ElementPosition obj);
        public override abstract int GetHashCode();
    }

    public abstract class ElementPositionException : ChordiousException
    {
        public ElementPosition Position { get; protected set; }

        public ElementPositionException() : base()
        {
            Position = null;
        }

        public ElementPositionException(ElementPosition position) : base()
        {
            Position = position.Clone();
        }
    }
}
