// Copyright (c) Jon Thysell <http://jonthysell.com>
// Licensed under the MIT License.

using System.Collections.Generic;

namespace Chordious.Core
{
    public class ReverseChordFinderResultSet
    {
        public IReverseChordFinderOptions ReverseChordFinderOptions { get; private set; }

        public int Count
        {
            get
            {
                return _results.Count;
            }
        }

        public IEnumerable<IReverseChordFinderResult> Results
        {
            get
            {
                foreach (ReverseChordFinderResult rcfr in _results)
                {
                    yield return rcfr;
                }
            }
        }

        private readonly List<IReverseChordFinderResult> _results;

        internal ReverseChordFinderResultSet(IReverseChordFinderOptions reverseChordFinderOptions)
        {
            ReverseChordFinderOptions = reverseChordFinderOptions;
            _results = new List<IReverseChordFinderResult>();
        }

        public void AddResult(InternalNote rootNote, IChordQuality chordQuality)
        {
            _results.Add(new ReverseChordFinderResult(this, rootNote, chordQuality));
            _results.Sort();
        }
    }
}
