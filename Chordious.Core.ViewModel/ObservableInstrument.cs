// 
// ObservableInstrument.cs
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
using System.Collections.ObjectModel;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

using com.jonthysell.Chordious.Core;

namespace com.jonthysell.Chordious.Core.ViewModel
{
    public class ObservableInstrument : ObservableObject
    {
        public string Name
        {
            get
            {
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

        internal Instrument Instrument { get; private set; }

        public ObservableInstrument(Instrument instrument)
        {
            if (null == instrument)
            {
                throw new ArgumentNullException("instrument");
            }
            Instrument = instrument;
        }

        public ObservableCollection<ObservableTuning> GetTunings()
        {
            ObservableCollection<ObservableTuning> collection = new ObservableCollection<ObservableTuning>();
            foreach (Tuning t in Instrument.Tunings)
            {
                collection.Add(new ObservableTuning(t));
            }
            return collection;
        }
    }
}
