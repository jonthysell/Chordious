// 
// InstrumentSet.cs
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
    public class InstrumentSet : IReadOnly, IEnumerable<Instrument>
    {
        public bool ReadOnly { get; private set; }

        public string Level
        {
            get
            {
                return this._level;
            }
            set
            {
                if (StringUtils.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentNullException();
                }

                if (this.ReadOnly)
                {
                    throw new ObjectIsReadOnlyException(this);
                }

                this._level = value;
            }
        }
        private string _level;

        public InstrumentSet Parent
        {
            get
            {
                return this._parent;
            }
            set
            {
                if (this.ReadOnly)
                {
                    throw new ObjectIsReadOnlyException(this);
                }

                this._parent = value;
            }
        }
        private InstrumentSet _parent;

        private List<Instrument> _instruments;

        internal InstrumentSet(string level)
        {
            Level = level;
            ReadOnly = false;
            _instruments = new List<Instrument>();
        }

        internal InstrumentSet(InstrumentSet parent, string level) : this(level)
        {
            if (null == parent)
            {
                throw new ArgumentNullException("parent");
            }

            Parent = parent;
        }

        public void MarkAsReadOnly()
        {
            ReadOnly = true;
            foreach (Instrument i in _instruments)
            {
                i.MarkAsReadOnly();
            }
        }

        public IEnumerator<Instrument> GetEnumerator()
        {
            foreach (Instrument instrument in _instruments)
            {
                yield return instrument;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public Instrument Get(string name)
        {
            if (StringUtils.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException("name");
            }

            Instrument instrument;
            if (TryGet(name, out instrument))
            {
                return instrument;
            }

            throw new InstrumentNotFoundException(this, name);
        }

        public bool TryGet(string name, out Instrument instrument)
        {
            if (StringUtils.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException("name");
            }

            foreach (Instrument i in _instruments)
            {
                if (i.Name == name)
                {
                    instrument = i;
                    return true;
                }
            }

            instrument = null;
            return false;
        }
        
        public Instrument Add(string name, int numStrings)
        {
            if (this.ReadOnly)
            {
                throw new ObjectIsReadOnlyException(this);
            }

            Instrument instrument = new Instrument(this, name, numStrings);
            Add(instrument);
            return instrument;
        }

        private void Add(Instrument instrument)
        {
            if (null == instrument)
            {
                throw new ArgumentNullException("instrument");
            }

            if (instrument.Parent != this)
            {
                throw new ArgumentOutOfRangeException("instrument");
            }

            ListUtils.SortedInsert<Instrument>(_instruments, instrument);
        }

        public void Remove(string name)
        {
            if (this.ReadOnly)
            {
                throw new ObjectIsReadOnlyException(this);
            }

            _instruments.Remove(Get(name));
        }

        internal void Resort(Instrument instrument)
        {
            if (null == instrument)
            {
                throw new ArgumentNullException("instrument");
            }

            if (this.ReadOnly)
            {
                throw new ObjectIsReadOnlyException(this);
            }

            if (_instruments.Remove(instrument))
            {
                Add(instrument);
            }
        }

        public void CopyFrom(InstrumentSet instrumentSet)
        {
            if (null == instrumentSet)
            {
                throw new ArgumentNullException("instrumentSet");
            }

            foreach (Instrument sourceInstrument in instrumentSet)
            {
                Instrument instrument = null;

                if (!TryGet(sourceInstrument.Name, out instrument))
                {
                    instrument = Add(sourceInstrument.Name, sourceInstrument.NumStrings);
                }

                if (sourceInstrument.NumStrings == instrument.NumStrings)
                {
                    instrument.Tunings.CopyFrom(sourceInstrument.Tunings);
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
                    if (xmlReader.IsStartElement() && xmlReader.Name == "instrument")
                    {
                        Instrument instrument = new Instrument(this, xmlReader.ReadSubtree());
                        Add(instrument);
                    }
                } while (xmlReader.Read());
            }
        }

        public void Write(XmlWriter xmlWriter)
        {
            if (null == xmlWriter)
            {
                throw new ArgumentNullException("xmlWriter");
            }

            foreach (Instrument i in _instruments)
            {
                i.Write(xmlWriter);
            }
        }
    }

    public abstract class InstrumentSetException : ChordiousException
    {
        public InstrumentSet InstrumentSet { get; private set; }

        public InstrumentSetException(InstrumentSet instrumentSet) : base()
        {
            this.InstrumentSet = instrumentSet;
        }
    }

    public abstract class TargetInstrumentException : InstrumentSetException
    {
        public string Name { get; private set; }

        public TargetInstrumentException(InstrumentSet instrumentSet, string name) : base(instrumentSet)
        {
            this.Name = name;
        }
    }

    public class InstrumentNotFoundException : TargetInstrumentException
    {
        public override string Message
        {
            get
            {
                return String.Format(Resources.Strings.InstrumentNotFoundExceptionMessage, Name);
            }
        }

        public InstrumentNotFoundException(InstrumentSet instrumentSet, string name) : base(instrumentSet, name) { }
    }

    public class InstrumentNameAlreadyExistsException : TargetInstrumentException
    {
        public override string Message
        {
            get
            {
                return String.Format(Resources.Strings.InstrumentNameAlreadyExistsMessage, Name);
            }
        }

        public InstrumentNameAlreadyExistsException(InstrumentSet instrumentSet, string name) : base(instrumentSet, name) { }
    }
}