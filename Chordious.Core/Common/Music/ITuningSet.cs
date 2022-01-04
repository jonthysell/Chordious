// Copyright (c) Jon Thysell <http://jonthysell.com>
// Licensed under the MIT License.

using System.Collections.Generic;

namespace Chordious.Core
{
    public interface ITuningSet : IEnumerable<ITuning>
    {
        IInstrument Instrument { get; }

        int Count { get; }

        ITuning Get(string longName);
        bool TryGet(string longName, out ITuning tuning);

        ITuning Add(string name, FullNote[] rootNotes);
        bool Remove(ITuning tuning);

        void CopyFrom(ITuningSet tuningSet);
    }
}
