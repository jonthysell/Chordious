// Copyright (c) Jon Thysell <http://jonthysell.com>
// Licensed under the MIT License.

using System;
using System.Xml;

namespace Chordious.Core
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
                if (value < 2)
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
        private readonly TuningSet _tunings;

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

            if (!(obj is Instrument instrument))
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
