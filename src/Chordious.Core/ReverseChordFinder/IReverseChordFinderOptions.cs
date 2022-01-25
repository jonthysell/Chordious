// Copyright (c) Jon Thysell <http://jonthysell.com>
// Licensed under the MIT License.

using System.Collections.Generic;

namespace Chordious.Core
{
    public interface IReverseChordFinderOptions : IFinderOptions
    {
        int[] Marks { get; }

        IEnumerable<IChordQuality> ChordQualities { get; }
    }
}
