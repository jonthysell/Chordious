// Copyright (c) Jon Thysell <http://jonthysell.com>
// Licensed under the MIT License.

using System;

namespace Chordious.Core
{
    public interface IChordFinderResult : IComparable
    {
        int[] Marks { get; }

        Diagram ToDiagram(ChordFinderStyle chordFinderStyle);
    }
}
