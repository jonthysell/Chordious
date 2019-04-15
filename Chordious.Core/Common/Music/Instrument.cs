// 
// Instrument.cs
//  
// Author:
//       Jon Thysell <thysell@gmail.com>
// 
// Copyright (c) 2013, 2015, 2016, 2017, 2019 Jon Thysell <http://jonthysell.com>
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
using System.Xml;

namespace com.jonthysell.Chordious.Core
{
    public class Instrument : IInstrument
    {
        public bool ReadOnly { get; private set; }

        public InstrumentSet Parent
        {
            get
            {
                return _parent;
            }
            private set
            {
                _parent = value ?? throw new ArgumentNullException();
            }
        }
        private InstrumentSet _parent;

        public string Level
        {
            get
            {
                return Parent.Level;
            }
        }

        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentNullException();
                }

                if (ReadOnly)
                {
                    throw new ObjectIsReadOnlyException(this);
                }

                value = value.Trim();

                if (Parent.TryGet(value, out Instrument instrument) && this != instrument)
                {
                    throw new InstrumentNameAlreadyExistsException(Parent, value);
                }

                _name = value;
                Parent.Resort(this);
            }
        }
        private string _name;

        public int NumStrings
        {
            get
            {
                return _numStrings;
            }
            private set
            {
                if (value <= 0)
                {
                    throw new ArgumentOutOfRangeException();
                }

                if (ReadOnly)
                {
                    throw new ObjectIsReadOnlyException(this);
                }

                _numStrings = value;
            }
        }
        private int _numStrings;

        public ITuningSet Tunings
        {
            get
            {
                return _tunings;
            }
        }
        private TuningSet _tunings;

        private Instrument(InstrumentSet parent)
        {
            ReadOnly = false;
            _tunings = new TuningSet(this);
            Parent = parent;
        }

        internal Instrument(InstrumentSet parent, string name, int numStrings) : this(parent)
        {
            Name = name;
            NumStrings = numStrings;
        }

        internal Instrument(InstrumentSet parent, XmlReader xmlReader) : this(parent)
        {
            Read(xmlReader);
        }

        public void MarkAsReadOnly()
        {
            ReadOnly = true;
            foreach (Tuning t in Tunings)
            {
                t.MarkAsReadOnly();
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
                if (xmlReader.IsStartElement() && xmlReader.Name == "instrument")
                {
                    Name = xmlReader.GetAttribute("name");
                    NumStrings = int.Parse(xmlReader.GetAttribute("strings"));

                    while (xmlReader.Read())
                    {
                        if (xmlReader.IsStartElement() && xmlReader.Name == "tuning")
                        {
                            _tunings.Read(xmlReader.ReadSubtree());
                        }
                    }
                }
            }
        }

        public void Write(XmlWriter xmlWriter)
        {
            if (null == xmlWriter)
            {
                throw new ArgumentNullException(nameof(xmlWriter));
            }

            xmlWriter.WriteStartElement("instrument");

            xmlWriter.WriteAttributeString("name", Name);
            xmlWriter.WriteAttributeString("strings", NumStrings.ToString());

            foreach (Tuning t in _tunings)
            {
                t.Write(xmlWriter);
            }

            xmlWriter.WriteEndElement();
        }

        public int CompareTo(object obj)
        {
            if (null == obj)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            Instrument instrument = obj as Instrument;
            if (null == instrument)
            {
                throw new ArgumentException();
            }

            return Name.CompareTo(instrument.Name);
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
