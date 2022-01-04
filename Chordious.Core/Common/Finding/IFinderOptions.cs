// Copyright (c) Jon Thysell <http://jonthysell.com>
// Licensed under the MIT License.

namespace Chordious.Core
{
    public interface IFinderOptions
    {
        IInstrument Instrument { get; }
        ITuning Tuning { get; }
    }
}
