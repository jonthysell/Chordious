// Copyright (c) Jon Thysell <http://jonthysell.com>
// Licensed under the MIT License.

using System;
using System.Collections.ObjectModel;

namespace Chordious.Core.ViewModel
{
    public class ObservableInstrument : ObservableHeaderObject
    {
        public string Name
        {
            get
            {
                if (IsHeader)
                {
                    return HeaderName;
                }
                return Instrument.Name;
            }
        }

        public int NumStrings
        {
            get
            {
                return Instrument.NumStrings;
            }
        }

        public string Level
        {
            get
            {
                return Instrument.Level;
            }
        }

        public bool ReadOnly
        {
            get
            {
                return Instrument.ReadOnly;
            }
        }

        internal Instrument Instrument { get; private set; }

        public ObservableInstrument(Instrument instrument) : base()
        {
            Instrument = instrument ?? throw new ArgumentNullException(nameof(instrument));
        }

        public ObservableInstrument(string headerName) : base(headerName) { }

        public ObservableCollection<ObservableTuning> GetTunings()
        {
            ObservableCollection<ObservableTuning> collection = new ObservableCollection<ObservableTuning>();
            foreach (Tuning t in Instrument.Tunings)
            {
                collection.Add(new ObservableTuning(t));
            }
            return collection;
        }

        public override string ToString()
        {
            if (Instrument is not null)
            {
                return Instrument.ToString();
            }
            return base.ToString();
        }
    }
}
