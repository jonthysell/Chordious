// Copyright (c) Jon Thysell <http://jonthysell.com>
// Licensed under the MIT License.

namespace Chordious.Core
{
    public interface IChordFinderOptions : IFinderOptions2
    {
        IChordQuality ChordQuality { get; }
        bool AllowRootlessChords { get; }
        bool AllowPartialChords { get; }
    }
}
