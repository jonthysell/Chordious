// Copyright (c) Jon Thysell <http://jonthysell.com>
// Licensed under the MIT License.

using System;
using System.Collections.Generic;

namespace Chordious.Core
{
    public interface IScaleFinderResult : IComparable
    {
        IEnumerable<MarkPosition> Marks { get; }

        Diagram ToDiagram(ScaleFinderStyle scaleFinderStyle);
    }
}
