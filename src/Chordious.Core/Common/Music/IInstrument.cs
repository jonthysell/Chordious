// Copyright (c) Jon Thysell <http://jonthysell.com>
// Licensed under the MIT License.

using System;

namespace Chordious.Core
{
    public interface IInstrument : IReadOnly, IComparable
    {
        InstrumentSet Parent { get; }

        string Level { get; }
        string Name { get; }
        int NumStrings { get; }

        ITuningSet Tunings { get; }
    }
}
