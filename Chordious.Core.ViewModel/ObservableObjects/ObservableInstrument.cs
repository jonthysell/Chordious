// 
// ObservableInstrument.cs
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
using System.Collections.ObjectModel;

namespace com.jonthysell.Chordious.Core.ViewModel
{
    public class ObservableInstrument : ObservableHeaderObject
    {
        public string Name
        {
            get
            {
                if (IsHeader)
                {
                    return HeaderName;
                }
                return Instrument.Name;
            }
        }

        public int NumStrings
        {
            get
            {
                return Instrument.NumStrings;
            }
        }

        public string Level
        {
            get
            {
                return Instrument.Level;
            }
        }

        public bool ReadOnly
        {
            get
            {
                return Instrument.ReadOnly;
            }
        }

        internal Instrument Instrument { get; private set; }

        public ObservableInstrument(Instrument instrument) : base()
        {
            if (null == instrument)
            {
                throw new ArgumentNullException("instrument");
            }
            Instrument = instrument;
        }

        public ObservableInstrument(string headerName) : base(headerName) { }

        public ObservableCollection<ObservableTuning> GetTunings()
        {
            ObservableCollection<ObservableTuning> collection = new ObservableCollection<ObservableTuning>();
            foreach (Tuning t in Instrument.Tunings)
            {
                collection.Add(new ObservableTuning(t));
            }
            return collection;
        }

        public override string ToString()
        {
            if (null != Instrument)
            {
                return Instrument.ToString();
            }
            return base.ToString();
        }
    }
}
