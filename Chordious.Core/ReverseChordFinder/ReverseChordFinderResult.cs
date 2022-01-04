// Copyright (c) Jon Thysell <http://jonthysell.com>
// Licensed under the MIT License.

using System;

namespace Chordious.Core
{
    public class ReverseChordFinderResult : IReverseChordFinderResult
    {
        public InternalNote RootNote { get; private set; }

        public IChordQuality ChordQuality { get; private set; }

        public ReverseChordFinderResultSet Parent { get; private set; }

        internal ReverseChordFinderResult(ReverseChordFinderResultSet parent, InternalNote rootNote, IChordQuality chordQuality)
        {
            Parent = parent ?? throw new ArgumentNullException(nameof(parent));
            RootNote = rootNote;
            ChordQuality = chordQuality ?? throw new ArgumentNullException(nameof(chordQuality));
        }

        public int CompareTo(object obj)
        {
            if (null == obj)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            if (!(obj is ReverseChordFinderResult rcfr))
            {
                throw new ArgumentException();
            }

            return RootNote.CompareTo(rcfr.RootNote);
        }
    }
}
