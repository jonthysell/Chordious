// Copyright (c) Jon Thysell <http://jonthysell.com>
// Licensed under the MIT License.

namespace Chordious.Core
{
    public interface IScaleFinderOptions : IFinderOptions2
    {
        IScale Scale { get; }

        bool StrictIntervals { get; }
    }
}
