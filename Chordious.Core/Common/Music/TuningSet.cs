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

        public Tuning Get(string name)
        {
            if (StringUtils.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException("name");
            }

            Tuning tuning;
            if (TryGet(name, out tuning))
            {
                return tuning;
            }

            throw new TuningNotFoundException(this, name);
        }

        public bool TryGet(string name, out Tuning tuning)
        {
            if (StringUtils.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException("name");
            }

            foreach (Tuning t in _tunings)
            {
                if (t.Name == name)
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
            _tunings.Add(tuning);
            return tuning;
        }

        public void Remove(string name)
        {
            if (this.ReadOnly)
            {
                throw new ObjectIsReadOnlyException(this);
            }

            _tunings.Remove(Get(name));
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

                if (!TryGet(sourceTuning.Name, out tuning))
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
                        _tunings.Add(tuning);
                    }
                } while (xmlReader.Read());
            }
        }
    }

    public class TuningSetException : ChordiousException
    {
        public TuningSet TuningSet { get; private set; }

        public TuningSetException(TuningSet tuningSet) : base()
        {
            this.TuningSet = tuningSet;
        }
    }

    public abstract class TargetTuningException : TuningSetException
    {
        public string Name { get; private set; }

        public TargetTuningException(TuningSet tuningSet, string name): base(tuningSet)
        {
            this.Name = name;
        }
    }

    public class TuningNotFoundException : TargetTuningException
    {
        public TuningNotFoundException(TuningSet tuningSet, string name) : base(tuningSet, name) { }
    }

    public class TuningNameAlreadyExistsException : TargetTuningException
    {
        public TuningNameAlreadyExistsException(TuningSet tuningSet, string name) : base(tuningSet, name) { }
    }
}
