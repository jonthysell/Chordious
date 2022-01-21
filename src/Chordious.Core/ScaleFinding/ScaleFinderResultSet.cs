// Copyright (c) Jon Thysell <http://jonthysell.com>
// Licensed under the MIT License.

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

        private readonly List<ScaleFinderResult> _results;

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
