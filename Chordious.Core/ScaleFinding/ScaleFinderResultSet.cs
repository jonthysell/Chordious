// 
// ScaleFinderResultSet.cs
//  
// Author:
//       Jon Thysell <thysell@gmail.com>
// 
// Copyright (c) 2015, 2016, 2017, 2019 Jon Thysell <http://jonthysell.com>
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

using System.Collections.Generic;

namespace Chordious.Core
{
    public class ScaleFinderResultSet
    {
        public IScaleFinderOptions ScaleFinderOptions { get; private set; }

        public int Count
        {
            get
            {
                return _results.Count;
            }
        }

        public IEnumerable<ScaleFinderResult> Results
        {
            get
            {
                foreach (ScaleFinderResult sfr in _results)
                {
                    yield return sfr;
                }
            }
        }

        private List<ScaleFinderResult> _results;

        internal ScaleFinderResultSet(IScaleFinderOptions scaleFinderOptions)
        {
            ScaleFinderOptions = scaleFinderOptions;
            _results = new List<ScaleFinderResult>();
        }

        public void AddResult(IEnumerable<MarkPosition> marks)
        {
            if (IsValid(marks))
            {
                _results.Add(new ScaleFinderResult(this, marks));
                _results.Sort();
            }
        }

        public ScaleFinderResult ResultAt(int index)
        {
            return _results[index];
        }

        private bool IsValid(IEnumerable<MarkPosition> marks)
        {
            return MarkUtils.ValidateScale(marks, ScaleFinderOptions);
        }
    }
}