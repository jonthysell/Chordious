// 
// ChordFinderResultSet.cs
//  
// Author:
//       Jon Thysell <thysell@gmail.com>
// 
// Copyright (c) 2013, 2014, 2015 Jon Thysell <http://jonthysell.com>
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

namespace com.jonthysell.Chordious.Core
{
    public class ChordFinderResultSet
    {
        public ChordFinderOptions ChordFinderOptions { get; private set; }

        public int Count
        {
            get
            {
                return _results.Count;
            }
        }

        public IEnumerable<ChordFinderResult> Results
        {
            get
            {
                foreach (ChordFinderResult cfr in _results)
                {
                    yield return cfr;
                }
            }
        }

        private List<ChordFinderResult> _results;

        internal ChordFinderResultSet(ChordFinderOptions chordFinderOptions)
        {
            this.ChordFinderOptions = chordFinderOptions;
            this._results = new List<ChordFinderResult>();
        }

        public void AddResult(int[] marks)
        {
            if (IsValid(marks))
            {
                if (ChordFinderOptions.MirrorResults)
                {
                    Array.Reverse(marks);
                }
                
                _results.Add(new ChordFinderResult(this, marks));
                _results.Sort();
            }
        }

        public ChordFinderResult ResultAt(int index)
        {
            return _results[index];
        }

        public Diagram DiagramAt(int index, ChordFinderStyle chordFinderStyle)
        {
            ChordFinderResult result = ResultAt(index);
            return result.ToDiagram(chordFinderStyle);
        }

        public DiagramCollection ToDiagramCollection(ChordFinderStyle chordFinderStyle)
        {
            DiagramCollection collection = new DiagramCollection(chordFinderStyle.Style);

            foreach (ChordFinderResult result in _results)
            {
                collection.Add(result.ToDiagram(chordFinderStyle));
            }

            return collection;
        }

        private bool IsValid(int[] marks)
        {
            bool reachPass = MarkUtils.Reach(marks) <= ChordFinderOptions.MaxReach;
            bool openPass = ChordFinderOptions.AllowOpenStrings ? true : !MarkUtils.HasOpenStrings(marks);
            bool mutePass = ChordFinderOptions.AllowMutedStrings ? true : !MarkUtils.HasMutedStrings(marks);

            return reachPass && openPass && mutePass;
        }
    }
}