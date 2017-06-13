// 
// ObservableNamedInterval.cs
//  
// Author:
//       Jon Thysell <thysell@gmail.com>
// 
// Copyright (c) 2015, 2017 Jon Thysell <http://jonthysell.com>
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

namespace com.jonthysell.Chordious.Core.ViewModel
{
    public class ObservableNamedInterval : ObservableHeaderObject
    {
        public string Name
        {
            get
            {
                if (IsHeader)
                {
                    return HeaderName;
                }
                return NamedInterval.Name;
            }
        }

        public string LongName
        {
            get
            {
                if (IsHeader)
                {
                    return HeaderName;
                }
                return NamedInterval.LongName;
            }
        }

        public int[] Intervals
        {
            get
            {
                return NamedInterval.Intervals;
            }
        }

        public string Level
        {
            get
            {
                return NamedInterval.Level;
            }
        }

        public bool ReadOnly
        {
            get
            {
                return NamedInterval.ReadOnly;
            }
        }

        internal NamedInterval NamedInterval { get; private set; }

        public ObservableNamedInterval(NamedInterval namedInterval) : base()
        {
            if (null == namedInterval)
            {
                throw new ArgumentNullException("namedInterval");
            }
            NamedInterval = namedInterval;
        }

        public ObservableNamedInterval(string headerName) : base(headerName) { }

        public override string ToString()
        {
            if (null != NamedInterval)
            {
                return NamedInterval.ToString();
            }
            return base.ToString();
        }
    }
}
