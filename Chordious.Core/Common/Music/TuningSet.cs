// 
// TuningSet.cs
//  
// Author:
//       Jon Thysell <thysell@gmail.com>
// 
// Copyright (c) 2015 Jon Thysell <http://jonthysell.com>
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

namespace com.jonthysell.Chordious.Core
{
    public class TuningSet : IReadOnly, IEnumerable<Tuning>
    {
        public Instrument Instrument { get; private set; }

        public bool ReadOnly { get; private set; }

        public int Count
        {
            get
            {
                return _tunings.Count;
            }
        }

        private List<Tuning> _tunings;

        internal TuningSet(Instrument instrument)
        {
            if (null == instrument)
            {
                throw new ArgumentNullException("instrument");
            }

            this.Instrument = instrument;
            _tunings = new List<Tuning>();
        }

        public void MarkAsReadOnly()
        {
            ReadOnly = true;
            foreach (Tuning t in _tunings)
            {
                t.MarkAsReadOnly();
            }
        }

        public IEnumerator<Tuning> GetEnumerator()
        {
            foreach (Tuning t in _tunings)
            {
                yield return t;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public Tuning Get(string longName)
        {
            if (StringUtils.IsNullOrWhiteSpace(longName))
            {
                throw new ArgumentNullException("longName");
            }

            Tuning tuning;
            if (TryGet(longName, out tuning))
            {
                return tuning;
            }

            throw new TuningNotFoundException(this, longName);
        }

        public bool TryGet(string longName, out Tuning tuning)
        {
            if (StringUtils.IsNullOrWhiteSpace(longName))
            {
                throw new ArgumentNullException("longName");
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

        public Tuning Add(string name, FullNote[] rootNotes)
        {
            if (this.ReadOnly)
            {
                throw new ObjectIsReadOnlyException(this);
            }

            Tuning tuning = new Tuning(this, name, rootNotes);
            Add(tuning);
            return tuning;
        }

        private void Add(Tuning tuning)
        {
            if (null == tuning)
            {
                throw new ArgumentNullException("tuning");
            }

            if (tuning.Parent != this)
            {
                throw new ArgumentOutOfRangeException("tuning");
            }

            if (!ListUtils.SortedInsert<Tuning>(_tunings, tuning))
            {
                throw new TuningAlreadyExistsException(this, tuning.LongName);
            }
        }

        public bool Remove(Tuning tuning)
        {
            if (null == tuning)
            {
                throw new ArgumentNullException("tuning");
            }

            if (this.ReadOnly)
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
                    throw ex;
                }
            }
        }

        public void CopyFrom(TuningSet tuningSet)
        {
            if (null == tuningSet)
            {
                throw new ArgumentNullException("tuningSet");
            }

            if (Instrument.NumStrings != tuningSet.Instrument.NumStrings)
            {
                throw new ArgumentOutOfRangeException("tuningSet");
            }

            foreach (Tuning sourceTuning in tuningSet)
            {
                Tuning tuning = null;

                if (!TryGet(sourceTuning.LongName, out tuning))
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
                throw new ArgumentNullException("xmlReader");
            }

            using (xmlReader)
            {
                do
                {
                    if (xmlReader.IsStartElement() && xmlReader.Name == "tuning")
                    {
                        Tuning tuning = new Tuning(this, xmlReader.ReadSubtree());
                        Add(tuning);
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
            this.TuningSet = tuningSet;
        }
    }

    public abstract class TargetTuningException : TuningSetException
    {
        public string LongName { get; private set; }

        public TargetTuningException(TuningSet tuningSet, string longName) : base(tuningSet)
        {
            this.LongName = longName;
        }
    }

    public class TuningNotFoundException : TargetTuningException
    {
        public override string Message
        {
            get
            {
                return String.Format(Resources.Strings.TuningNotFoundExceptionMessage, LongName);
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
                return String.Format(Resources.Strings.TuningAlreadyExistsMessage, LongName);
            }
        }

        public TuningAlreadyExistsException(TuningSet tuningSet, string longName) : base(tuningSet, longName) { }
    }
}
