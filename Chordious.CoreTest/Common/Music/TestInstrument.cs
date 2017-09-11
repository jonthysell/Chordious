// 
// TestInstrument.cs
//  
// Author:
//       Jon Thysell <thysell@gmail.com>
// 
// Copyright (c) 2016, 2017 Jon Thysell <http://jonthysell.com>
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

using com.jonthysell.Chordious.Core;

namespace com.jonthysell.Chordious.CoreTest
{
    public class TestInstrument : IInstrument
    {
        public static TestInstrument Ukulele
        {
            get
            {
                if (null == _ukulele)
                {
                    _ukulele = new TestInstrument(UkuleleInstrumentName, 4);
                    _ukulele.Tunings.Add(UkuleleStandardTuningName,
                        new FullNote[]
                        {
                            new FullNote(Note.G, 4),
                            new FullNote(Note.C, 4),
                            new FullNote(Note.E, 4),
                            new FullNote(Note.A, 4),
                        });
                }
                return _ukulele;
            }
        }
        private static TestInstrument _ukulele = null;

        public bool ReadOnly
        {
            get
            {
                return false;
            }
        }

        public InstrumentSet Parent
        {
            get
            {
                return null;
            }
        }

        public string Level
        {
            get
            {
                return "Test";
            }
        }

        public string Name { get; private set; }
        public int NumStrings { get; private set; }

        public ITuningSet Tunings { get; private set; }

        public TestInstrument(string name, int numStrings)
        {
            Name = name;
            NumStrings = numStrings;

            Tunings = new TestTuningSet(this);
        }

        public void MarkAsReadOnly()
        {
            throw new NotImplementedException();
        }

        public int CompareTo(object obj)
        {
            throw new NotImplementedException();
        }

        public const string UkuleleInstrumentName = "Ukulele";
        public const string UkuleleStandardTuningName = "UkuleleStandard";
    }
}
