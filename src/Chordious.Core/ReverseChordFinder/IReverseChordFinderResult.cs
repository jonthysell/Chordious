// Copyright (c) Jon Thysell <http://jonthysell.com>
// Licensed under the MIT License.

using System;

namespace Chordious.Core
{
    public interface IReverseChordFinderResult : IComparable
    {
        InternalNote RootNote { get; }

        IChordQuality ChordQuality { get; }
    }
}
