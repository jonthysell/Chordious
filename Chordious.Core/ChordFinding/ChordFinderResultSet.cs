// Copyright (c) Jon Thysell <http://jonthysell.com>
// Licensed under the MIT License.

using System.Collections.Generic;

namespace Chordious.Core
{
    public class ChordFinderResultSet
    {
        public IChordFinderOptions ChordFinderOptions { get; private set; }

        public int Count
        {
            get
            {
                return _results.Count;
            }
        }

        public IEnumerable<IChordFinderResult> Results
        {
            get
            {
                foreach (ChordFinderResult cfr in _results)
                {
                    yield return cfr;
                }
            }
        }

        private readonly List<IChordFinderResult> _results;

        internal ChordFinderResultSet(IChordFinderOptions chordFinderOptions)
        {
            ChordFinderOptions = chordFinderOptions;
            _results = new List<IChordFinderResult>();
        }

        public void AddResult(int[] marks)
        {
            if (IsValid(marks))
            {
                _results.Add(new ChordFinderResult(this, marks));
                _results.Sort();
            }
        }

        public IChordFinderResult ResultAt(int index)
        {
            return _results[index];
        }

        public Diagram DiagramAt(int index, ChordFinderStyle chordFinderStyle)
        {
            IChordFinderResult result = ResultAt(index);
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
            return MarkUtils.ValidateChord(marks, ChordFinderOptions);
        }
    }
}
