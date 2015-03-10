// 
// Tuning.cs
//  
// Author:
//       Jon Thysell <thysell@gmail.com>
// 
// Copyright (c) 2013, 2015 Jon Thysell <http://jonthysell.com>
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
    public class Tuning : IReadOnly
    {
        public bool ReadOnly { get; private set; }

        public TuningSet Parent
        {
            get
            {
                return _parent;
            }
            private set
            {
                if (null == value)
                {
                    throw new ArgumentNullException();
                }
                _parent = value;
            }
        }
        private TuningSet _parent;

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
                if (String.IsNullOrEmpty(value))
                {
                    throw new ArgumentNullException();
                }

                if (this.ReadOnly)
                {
                    throw new ObjectIsReadOnlyException(this);
                }

                Tuning tuning;
                if (Parent.TryGet(value, out tuning) && this != tuning)
                {
                    throw new TuningNameAlreadyExistsException(Parent, value);
                }

                _name = value;
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

                return String.Format("{0} ({1})", Name, notes);
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
                if (null == value)
                {
                    throw new ArgumentNullException();
                }

                if (value.Length != Parent.Instrument.NumStrings)
                {
                    throw new ArgumentOutOfRangeException();
                }

                if (this.ReadOnly)
                {
                    throw new ObjectIsReadOnlyException(this);
                }

                _rootNotes = value;
            }
        }
        private FullNote[] _rootNotes;

        private Tuning(TuningSet parent)
        {
            if (null == parent)
            {
                throw new ArgumentNullException("parent");
            }

            this.ReadOnly = false;
            this.Parent = parent;
        }

        internal Tuning(TuningSet parent, string name, FullNote[] rootNotes) : this(parent)
        {
            this.Name = name;
            this.RootNotes = rootNotes;
        }

        internal Tuning(TuningSet parent, XmlReader xmlReader) : this(parent)
        {
            if (null == xmlReader)
            {
                throw new ArgumentNullException("xmlReader");
            }

            using (xmlReader)
            {
                if (xmlReader.IsStartElement() && xmlReader.Name == "tuning")
                {
                    this.Name = xmlReader.GetAttribute("name");

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
                    this.RootNotes = rootNotes;
                }
            }
        }

        public void MarkAsReadOnly()
        {
            this.ReadOnly = true;
        }

        public InternalNote InternalNoteAt(int str, int fret)
        {
            if (str < 0 || str >= RootNotes.Length)
            {
                throw new ArgumentOutOfRangeException("str");
            }

            if (fret < 0)
            {
                throw new ArgumentOutOfRangeException("fret");
            }

            return NoteUtils.Shift(RootNotes[str].InternalNote, fret);
        }

        public FullNote FullNoteAt(int str, int fret, InternalNoteStringStyle style)
        {
            if (str < 0 || str >= RootNotes.Length)
            {
                throw new ArgumentOutOfRangeException("str");
            }

            if (fret < 0)
            {
                throw new ArgumentOutOfRangeException("fret");
            }

            return RootNotes[str].Shift(fret, style);
        }

        public void Write(XmlWriter xmlWriter)
        {
            if (null == xmlWriter)
            {
                throw new ArgumentNullException("xmlWriter");
            }

            xmlWriter.WriteStartElement("tuning");

            xmlWriter.WriteAttributeString("name", this.Name);

            string rootNotes = "";

            for (int i = 0; i < RootNotes.Length; i++)
            {
                rootNotes += RootNotes[i].ToString() + ";";
            }

            rootNotes = rootNotes.TrimEnd(';');

            xmlWriter.WriteAttributeString("notes", rootNotes);

            xmlWriter.WriteEndElement();
        }
    }
}