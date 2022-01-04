// Copyright (c) Jon Thysell <http://jonthysell.com>
// Licensed under the MIT License.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

using Chordious.Core.Resources;

namespace Chordious.Core
{
    public class TuningSet : ITuningSet, IReadOnly
    {
        public IInstrument Instrument
        {
            get
            {
                return _instrument;
            }
        }
        private readonly Instrument _instrument;

        public bool ReadOnly { get; private set; }

        public int Count
        {
            get
            {
                return _tunings.Count;
            }
        }

        private readonly List<ITuning> _tunings;

        internal TuningSet(Instrument instrument)
        {
            _instrument = instrument ?? throw new ArgumentNullException(nameof(instrument));
            _tunings = new List<ITuning>();
        }

        public void MarkAsReadOnly()
        {
            ReadOnly = true;
            foreach (Tuning t in _tunings)
            {
                t.MarkAsReadOnly();
            }
        }

        public IEnumerator<ITuning> GetEnumerator()
        {
            foreach (Tuning t in _tunings)
            {
                yield return t;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public ITuning Get(string longName)
        {
            if (StringUtils.IsNullOrWhiteSpace(longName))
            {
                throw new ArgumentNullException(nameof(longName));
            }

            if (TryGet(longName, out ITuning tuning))
            {
                return tuning;
            }

            throw new TuningNotFoundException(this, longName);
        }

        public bool TryGet(string longName, out ITuning tuning)
        {
            if (StringUtils.IsNullOrWhiteSpace(longName))
            {
                throw new ArgumentNullException(nameof(longName));
            }

            foreach (Tuning t in _tunings)
            {
                if (t.LongName == longName)
                {
                    tuning = t;
                    return true;
                }
            }

            tuning = null;
            return false;
        }

        public ITuning Add(string name, FullNote[] rootNotes)
        {
            if (ReadOnly)
            {
                throw new ObjectIsReadOnlyException(this);
            }

            Tuning tuning = new Tuning(this, name, rootNotes);
            Add(tuning);
            return tuning;
        }

        private void Add(ITuning tuning)
        {
            if (null == tuning)
            {
                throw new ArgumentNullException(nameof(tuning));
            }

            if (tuning.Parent != this)
            {
                throw new ArgumentOutOfRangeException(nameof(tuning));
            }

            if (!ListUtils.SortedInsert<ITuning>(_tunings, tuning))
            {
                throw new TuningAlreadyExistsException(this, tuning.LongName);
            }
        }

        public bool Remove(ITuning tuning)
        {
            if (null == tuning)
            {
                throw new ArgumentNullException(nameof(tuning));
            }

            if (ReadOnly)
            {
                throw new ObjectIsReadOnlyException(this);
            }

            return _tunings.Remove(tuning);
        }

        internal void Resort(Tuning tuning, Action rollback)
        {
            if (Remove(tuning))
            {
                try
                {
                    Add(tuning);
                }
                catch (Exception ex)
                {
                    if (null != rollback)
                    {
                        rollback();

                        // Re-add since remove succeeded but re-add failed
                        if (null != (ex as TuningAlreadyExistsException))
                        {
                            Add(tuning);
                        }
                    }
                    throw;
                }
            }
        }

        public string GetNewTuningName()
        {
            return GetNewTuningName(Strings.TuningSetDefaultNewTuningName);
        }

        public string GetNewTuningName(string baseName)
        {
            if (StringUtils.IsNullOrWhiteSpace(baseName))
            {
                throw new ArgumentNullException(nameof(baseName));
            }

            string name = baseName;

            bool valid = false;

            int count = 1;
            while (!valid)
            {
                if (!_tunings.Exists(tuning => tuning.Name == name))
                {
                    valid = true; // Found an unused name
                }
                else
                {
                    name = string.Format("{0} ({1})", baseName, count);
                    count++;
                }
            }

            return name;
        }

        public void CopyFrom(ITuningSet tuningSet)
        {
            if (null == tuningSet)
            {
                throw new ArgumentNullException(nameof(tuningSet));
            }

            if (Instrument.NumStrings != tuningSet.Instrument.NumStrings)
            {
                throw new ArgumentOutOfRangeException(nameof(tuningSet));
            }

            foreach (ITuning sourceTuning in tuningSet)
            {
                if (!TryGet(sourceTuning.LongName, out _))
                {
                    FullNote[] rootNotes = new FullNote[sourceTuning.RootNotes.Length];
                    sourceTuning.RootNotes.CopyTo(rootNotes, 0);

                    Add(sourceTuning.Name, rootNotes);
                }
            }
        }

        public void Read(XmlReader xmlReader)
        {
            if (null == xmlReader)
            {
                throw new ArgumentNullException(nameof(xmlReader));
            }

            using (xmlReader)
            {
                do
                {
                    if (xmlReader.IsStartElement() && xmlReader.Name == "tuning")
                    {
                        try
                        {
                            Tuning tuning = new Tuning(this, xmlReader.ReadSubtree());
                            Add(tuning);
                        }
                        catch (ArgumentOutOfRangeException) { }
                    }
                } while (xmlReader.Read());
            }
        }
    }

    public abstract class TuningSetException : ChordiousException
    {
        public TuningSet TuningSet { get; private set; }

        public TuningSetException(TuningSet tuningSet) : base()
        {
            TuningSet = tuningSet;
        }
    }

    public abstract class TargetTuningException : TuningSetException
    {
        public string LongName { get; private set; }

        public TargetTuningException(TuningSet tuningSet, string longName) : base(tuningSet)
        {
            LongName = longName;
        }
    }

    public class TuningNotFoundException : TargetTuningException
    {
        public override string Message
        {
            get
            {
                return string.Format(Strings.TuningNotFoundExceptionMessage, LongName);
            }
        }

        public TuningNotFoundException(TuningSet tuningSet, string longName) : base(tuningSet, longName) { }
    }

    public class TuningAlreadyExistsException : TargetTuningException
    {
        public override string Message
        {
            get
            {
                return string.Format(Strings.TuningAlreadyExistsMessage, LongName);
            }
        }

        public TuningAlreadyExistsException(TuningSet tuningSet, string longName) : base(tuningSet, longName) { }
    }
}
