// 
// ReverseChordFinderOptions.cs
//  
// Author:
//       Jon Thysell <thysell@gmail.com>
// 
// Copyright (c) 2019 Jon Thysell <http://jonthysell.com>
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
using System.Collections.Generic;

namespace Chordious.Core
{
    public class ReverseChordFinderOptions : FinderOptions, IReverseChordFinderOptions
    {
        public int[] Marks
        {
            get
            {
                return GetMarks();
            }
        }

        private int[] _cachedMarks;

        public IEnumerable<IChordQuality> ChordQualities { get; set; }

        public ReverseChordFinderOptions(ConfigFile configFile, ChordiousSettings chordiousSettings = null) : base(configFile, "reversechord")
        {
            if (null != chordiousSettings)
            {
                Settings = chordiousSettings;
            }
            else
            {
                string settingsLevel = "ReverseChordFinderOptions";
                Settings = new ChordiousSettings(_configFile.ChordiousSettings, settingsLevel);
            }
            _cachedMarks = null;
        }

        public int[] GetMarks()
        {
            int[] marks = new int[Instrument.NumStrings];

            for (int i = 0; i < marks.Length; i++)
            {
                marks[i] = (null != _cachedMarks && i < _cachedMarks.Length) ? _cachedMarks[i] : -1;
            }

            _cachedMarks = marks;

            return _cachedMarks;
        }

        public void SetTarget(int[] marks)
        {
            _cachedMarks = marks ?? throw new ArgumentNullException(nameof(marks));
        }

        public ReverseChordFinderOptions Clone()
        {
            ReverseChordFinderOptions rcfo = new ReverseChordFinderOptions(_configFile);
            rcfo.Settings.CopyFrom(Settings);
            rcfo.SetTarget(Instrument, Tuning);
            return rcfo;
        }
    }
}
