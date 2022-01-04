// Copyright (c) Jon Thysell <http://jonthysell.com>
// Licensed under the MIT License.

namespace Chordious.Core
{
    public interface INamedInterval
    {
        NamedIntervalSet Parent { get; }

        string Level { get; }
        string Name { get; }
        string LongName { get; }

        int[] Intervals { get; }
    }
}
