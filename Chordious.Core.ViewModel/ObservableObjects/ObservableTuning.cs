// 
// ObservableTuning.cs
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

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

using com.jonthysell.Chordious.Core;

namespace com.jonthysell.Chordious.Core.ViewModel
{
    public class ObservableTuning : ObservableObject
    {
        public string Name
        {
            get
            {
                return Tuning.Name;
            }
        }

        public string LongName
        {
            get
            {
                return Tuning.LongName;
            }
        }

        public string Level
        {
            get
            {
                return Tuning.Level;
            }
        }

        public bool CanEdit
        {
            get
            {
                return !Tuning.ReadOnly;
            }
        }

        public ObservableCollection<ObservableNote> Notes
        {
            get
            {
                return GetNotes();
            }
        }

        internal Tuning Tuning { get; private set; }

        public ObservableTuning(Tuning tuning)
        {
            if (null == tuning)
            {
                throw new ArgumentNullException("tuning");
            }
            Tuning = tuning;
        }

        private ObservableCollection<ObservableNote> GetNotes()
        {
            ObservableCollection<ObservableNote> collection = new ObservableCollection<ObservableNote>();

            foreach(FullNote note in Tuning.RootNotes)
            {
                collection.Add(new ObservableNote(note));
            }

            return collection;
        }

        public override string ToString()
        {
            if (null != Tuning)
            {
                return Tuning.ToString();
            }
            return base.ToString();
        }
    }
}
