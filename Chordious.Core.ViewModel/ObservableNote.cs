// 
// ObservableNote.cs
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
    public class ObservableNote : ObservableObject
    {
        public AppViewModel AppVM
        {
            get
            {
                return AppViewModel.Instance;
            }
        }

        public int SelectedNoteIndex
        {
            get
            {
                return (int)(FullNote.Note);
            }
            set
            {
                FullNote.Note = (Core.Note)(value);
                RaisePropertyChanged("SelectedNoteIndex");
            }
        }

        public ObservableCollection<string> Notes
        {
            get
            {
                return ObservableEnums.GetNotes();
            }
        }

        public int Octave
        {
            get
            {
                return FullNote.Octave;
            }
            set
            {
                FullNote.Octave = value;
                RaisePropertyChanged("Octave");
            }
        }

        internal FullNote FullNote { get; private set; }

        public ObservableNote()
        {
            FullNote = new FullNote();
        }

        internal ObservableNote(FullNote note)
        {
            if (null == note)
            {
                throw new ArgumentNullException("note");
            }

            FullNote = note;
        }
    }
}
