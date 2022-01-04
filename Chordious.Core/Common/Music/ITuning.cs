// Copyright (c) Jon Thysell <http://jonthysell.com>
// Licensed under the MIT License.

using System;

namespace Chordious.Core
{
    public interface ITuning : IReadOnly, IComparable
    {
        ITuningSet Parent { get; }

        string Level { get; }
        string Name { get; }
        string LongName { get; }

        FullNote[] RootNotes { get; }
    }
}
