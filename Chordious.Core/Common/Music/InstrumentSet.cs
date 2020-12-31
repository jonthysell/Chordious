// 
// InstrumentSet.cs
//  
// Author:
//       Jon Thysell <thysell@gmail.com>
// 
// Copyright (c) 2015, 2017, 2019, 2020 Jon Thysell <http://jonthysell.com>
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

using Chordious.Core.Resources;

namespace Chordious.Core
{
    public class InstrumentSet : IReadOnly, IEnumerable<Instrument>
    {
        public bool ReadOnly { get; private set; }

        public string Level
        {
            get
            {
                return _level;
            }
            set
            {
                if (StringUtils.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentNullException();
                }

                if (ReadOnly)
                {
                    throw new ObjectIsReadOnlyException(this);
                }

                _level = value;
            }
        }
        private string _level;

        public InstrumentSet Parent
        {
            get
            {
                return _parent;
            }
            set
            {
                if (ReadOnly)
                {
                    throw new ObjectIsReadOnlyException(this);
                }

                _parent = value;
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
            Parent = parent ?? throw new ArgumentNullException(nameof(parent));
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
            return GetEnumerator();
        }

        public Instrument Get(string name)
        {
            if (StringUtils.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            if (TryGet(name, out Instrument instrument))
            {
                return instrument;
            }

            throw new InstrumentNotFoundException(this, name);
        }

        public bool TryGet(string name, out Instrument instrument)
        {
            if (StringUtils.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
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
            if (ReadOnly)
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
                throw new ArgumentNullException(nameof(instrument));
            }

            if (instrument.Parent != this)
            {
                throw new ArgumentOutOfRangeException(nameof(instrument));
            }

            ListUtils.SortedInsert<Instrument>(_instruments, instrument);
        }

        public void Remove(string name)
        {
            if (ReadOnly)
            {
                throw new ObjectIsReadOnlyException(this);
            }

            _instruments.Remove(Get(name));
        }

        internal void Resort(Instrument instrument)
        {
            if (null == instrument)
            {
                throw new ArgumentNullException(nameof(instrument));
            }

            if (ReadOnly)
            {
                throw new ObjectIsReadOnlyException(this);
            }

            if (_instruments.Remove(instrument))
            {
                Add(instrument);
            }
        }

        public string GetNewInstrumentName()
        {
            return GetNewInstrumentName(Strings.InstrumentSetDefaultNewInstrumentName);
        }

        public string GetNewInstrumentName(string baseName)
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
                if (!TryGet(name, out Instrument instrument))
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

        public void CopyFrom(InstrumentSet instrumentSet)
        {
            if (null == instrumentSet)
            {
                throw new ArgumentNullException(nameof(instrumentSet));
            }

            foreach (Instrument sourceInstrument in instrumentSet)
            {

                if (!TryGet(sourceInstrument.Name, out Instrument instrument))
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
                throw new ArgumentNullException(nameof(xmlReader));
            }

            using (xmlReader)
            {
                do
                {
                    if (xmlReader.IsStartElement() && xmlReader.Name == "instrument")
                    {
                        try
                        {
                            Instrument instrument = new Instrument(this, xmlReader.ReadSubtree());
                            Add(instrument);
                        }
                        catch (ArgumentOutOfRangeException) { }
                    }
                } while (xmlReader.Read());
            }
        }

        public void Write(XmlWriter xmlWriter)
        {
            if (null == xmlWriter)
            {
                throw new ArgumentNullException(nameof(xmlWriter));
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
            InstrumentSet = instrumentSet;
        }
    }

    public abstract class TargetInstrumentException : InstrumentSetException
    {
        public string Name { get; private set; }

        public TargetInstrumentException(InstrumentSet instrumentSet, string name) : base(instrumentSet)
        {
            Name = name;
        }
    }

    public class InstrumentNotFoundException : TargetInstrumentException
    {
        public override string Message
        {
            get
            {
                return string.Format(Strings.InstrumentNotFoundExceptionMessage, Name);
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
                return string.Format(Strings.InstrumentNameAlreadyExistsMessage, Name);
            }
        }

        public InstrumentNameAlreadyExistsException(InstrumentSet instrumentSet, string name) : base(instrumentSet, name) { }
    }
}
