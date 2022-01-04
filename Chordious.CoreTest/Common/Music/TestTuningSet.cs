// Copyright (c) Jon Thysell <http://jonthysell.com>
// Licensed under the MIT License.

using System;
using System.Collections;
using System.Collections.Generic;

using Chordious.Core;

namespace Chordious.CoreTest
{
    public class TestTuningSet : ITuningSet
    {
        public IInstrument Instrument { get; private set; }

        public int Count
        {
            get
            {
                return _tunings.Count;
            }
        }

        private readonly List<ITuning> _tunings;

        public ITuning Get(string longName)
        {
            foreach (ITuning tuning in _tunings)
            {
                if (tuning.LongName == longName)
                {
                    return tuning;
                }
            }

            return null;
        }

        public bool TryGet(string longName, out ITuning tuning)
        {
            throw new NotImplementedException();
        }

        public ITuning Add(string name, FullNote[] rootNotes)
        {
            TestTuning tuning = new TestTuning(this, name, rootNotes);
            _tunings.Add(tuning);
            return tuning;
        }

        public bool Remove(ITuning tuning)
        {
            throw new NotImplementedException();
        }

        public void CopyFrom(ITuningSet tuningSet)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<ITuning> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public TestTuningSet(IInstrument instrument)
        {
            Instrument = instrument;
            _tunings = new List<ITuning>();
        }
    }
}
