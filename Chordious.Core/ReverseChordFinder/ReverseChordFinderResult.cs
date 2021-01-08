﻿// 
// ReverseChordFinderResult.cs
//  
// Author:
//       Jon Thysell <thysell@gmail.com>
// 
// Copyright (c) 2019, 2021 Jon Thysell <http://jonthysell.com>
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
    public class ReverseChordFinderResult : IReverseChordFinderResult
    {
        public InternalNote RootNote { get; private set; }

        public IChordQuality ChordQuality { get; private set; }

        public ReverseChordFinderResultSet Parent { get; private set; }

        internal ReverseChordFinderResult(ReverseChordFinderResultSet parent, InternalNote rootNote, IChordQuality chordQuality)
        {
            Parent = parent ?? throw new ArgumentNullException(nameof(parent));
            RootNote = rootNote;
            ChordQuality = chordQuality ?? throw new ArgumentNullException(nameof(chordQuality));
        }

        public int CompareTo(object obj)
        {
            if (null == obj)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            if (!(obj is ReverseChordFinderResult rcfr))
            {
                throw new ArgumentException();
            }

            return RootNote.CompareTo(rcfr.RootNote);
        }
    }
}
