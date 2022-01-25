// Copyright (c) Jon Thysell <http://jonthysell.com>
// Licensed under the MIT License.

namespace Chordious.Core
{
    public interface IChordQuality : INamedInterval
    {
        string Abbreviation { get; }
    }
}
