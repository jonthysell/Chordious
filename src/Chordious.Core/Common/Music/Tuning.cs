// Copyright (c) Jon Thysell <http://jonthysell.com>
// Licensed under the MIT License.

using System;
using System.Xml;

namespace Chordious.Core
{
    public class Tuning : ITuning
    {
        public bool ReadOnly { get; private set; }

        public ITuningSet Parent
        {
            get
            {
                return _parent;
            }
        }
        private readonly TuningSet _parent;

        public string Level
        {
            get
            {
                return Parent.Instrument.Level;
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
                if (StringUtils.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentNullException();
                }

                if (ReadOnly)
                {
                    throw new ObjectIsReadOnlyException(this);
                }

                value = value.Trim();

                string oldValue = _name;
                _name = value;

                // Resort with parent
                if (UpdateParent)
                {
                    _parent.Resort(this, () =>
                    {
                        _name = oldValue;
                    });
                }
            }
        }
        private string _name;

        public string LongName
        {
            get
            {
                string notes = "";

                foreach (FullNote n in RootNotes)
                {
                    notes += n.ToString() + " ";
                }

                notes = notes.TrimEnd();

                return string.Format("{0} ({1})", Name, notes);
            }
        }

        public FullNote[] RootNotes
        {
            get
            {
                return _rootNotes;
            }
            set
            {
                if (value is null)
                {
                    throw new ArgumentNullException();
                }

                if (value.Length != Parent.Instrument.NumStrings)
                {
                    throw new ArgumentOutOfRangeException();
                }

                if (ReadOnly)
                {
                    throw new ObjectIsReadOnlyException(this);
                }

                FullNote[] oldValue = _rootNotes;
                _rootNotes = value;

                // Resort with parent
                if (UpdateParent)
                {
                    _parent.Resort(this, () =>
                    {
                        _rootNotes = oldValue;
                    });
                }
            }
        }
        private FullNote[] _rootNotes;

        private bool UpdateParent = false;

        private Tuning(TuningSet parent)
        {
            ReadOnly = false;
            _parent = parent ?? throw new ArgumentNullException(nameof(parent));
        }

        internal Tuning(TuningSet parent, string name, FullNote[] rootNotes) : this(parent)
        {
            Name = name;
            RootNotes = rootNotes;

            UpdateParent = true;
        }

        internal Tuning(TuningSet parent, XmlReader xmlReader) : this(parent)
        {
            if (xmlReader is null)
            {
                throw new ArgumentNullException(nameof(xmlReader));
            }

            using (xmlReader)
            {
                if (xmlReader.IsStartElement() && xmlReader.Name == "tuning")
                {
                    Name = xmlReader.GetAttribute("name");

                    string notes = xmlReader.GetAttribute("notes");

                    string[] s = notes.Split(';');

                    FullNote[] rootNotes = new FullNote[s.Length];

                    if (rootNotes.Length != Parent.Instrument.NumStrings)
                    {
                        throw new ArgumentOutOfRangeException();
                    }

                    for (int i = 0; i < rootNotes.Length; i++)
                    {
                        rootNotes[i] = FullNote.Parse(s[i]);
                    }
                    RootNotes = rootNotes;
                }
            }

            UpdateParent = true;
        }

        public void Update(string name, FullNote[] rootNotes)
        {
            if (StringUtils.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            if (rootNotes is null)
            {
                throw new ArgumentNullException();
            }

            if (rootNotes.Length != Parent.Instrument.NumStrings)
            {
                throw new ArgumentOutOfRangeException();
            }

            if (ReadOnly)
            {
                throw new ObjectIsReadOnlyException(this);
            }

            UpdateParent = false;

            string oldName = Name;
            FullNote[] oldRootNotes = RootNotes;

            Name = name;
            RootNotes = rootNotes;

            _parent.Resort(this, () =>
            {
                Name = oldName;
                RootNotes = oldRootNotes;
                UpdateParent = true;
            });
        }

        public void MarkAsReadOnly()
        {
            ReadOnly = true;
        }

        public InternalNote InternalNoteAt(int str, int fret)
        {
            if (str < 0 || str >= RootNotes.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(str));
            }

            if (fret < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(fret));
            }

            return NoteUtils.Shift(RootNotes[str].InternalNote, fret);
        }

        public FullNote FullNoteAt(int str, int fret, InternalNoteStringStyle style)
        {
            if (str < 0 || str >= RootNotes.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(str));
            }

            if (fret < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(fret));
            }

            return RootNotes[str].Shift(fret, style);
        }

        public void Write(XmlWriter xmlWriter)
        {
            if (xmlWriter is null)
            {
                throw new ArgumentNullException(nameof(xmlWriter));
            }

            xmlWriter.WriteStartElement("tuning");

            xmlWriter.WriteAttributeString("name", Name);

            string rootNotes = "";

            for (int i = 0; i < RootNotes.Length; i++)
            {
                rootNotes += RootNotes[i].ToString() + ";";
            }

            rootNotes = rootNotes.TrimEnd(';');

            xmlWriter.WriteAttributeString("notes", rootNotes);

            xmlWriter.WriteEndElement();
        }

        public int CompareTo(object obj)
        {
            if (obj is null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            if (obj is not Tuning tuning)
            {
                throw new ArgumentException();
            }

            return LongName.CompareTo(tuning.LongName);
        }

        public override string ToString()
        {
            return LongName;
        }
    }
}
