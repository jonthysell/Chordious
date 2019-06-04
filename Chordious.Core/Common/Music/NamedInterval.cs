// 
// NamedInterval.cs
//  
// Author:
//       Jon Thysell <thysell@gmail.com>
// 
// Copyright (c) 2015, 2016, 2017, 2019 Jon Thysell <http://jonthysell.com>
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

namespace Chordious.Core
{
    public abstract class NamedInterval : INamedInterval, IReadOnly, IComparable
    {
        public bool ReadOnly { get; protected set; }

        public NamedIntervalSet Parent
        {
            get
            {
                return _parent;
            }
            protected set
            {
                _parent = value ?? throw new ArgumentNullException();
            }
        }
        private NamedIntervalSet _parent;

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
                    Parent.Resort(this, () =>
                    {
                        _name = oldValue;
                    });
                }
            }
        }
        private string _name = "";

        public string LongName
        {
            get
            {
                return GetLongName();
            }
        }

        public int[] Intervals
        {
            get
            {
                return _intervals;
            }
            set
            {
                if (ReadOnly)
                {
                    throw new ObjectIsReadOnlyException(this);
                }

                int[] oldValue = _intervals;
                _intervals = value ?? throw new ArgumentNullException();

                // Resort with parent
                if (UpdateParent)
                {
                    Parent.Resort(this, () =>
                    {
                        _intervals = oldValue;
                    });
                }
            }
        }
        private int[] _intervals;

        protected bool UpdateParent = false;

        protected NamedInterval(NamedIntervalSet parent)
        {
            ReadOnly = false;
            Parent = parent;
        }

        protected virtual string GetLongName()
        {
            return string.Format("{0} ({1})", Name, GetIntervalString());
        }

        protected bool ReadBase(XmlReader xmlReader, string localName)
        {
            if (null == xmlReader)
            {
                throw new ArgumentNullException(nameof(xmlReader));
            }

            if (StringUtils.IsNullOrWhiteSpace(localName))
            {
                throw new ArgumentNullException(nameof(localName));
            }

            if (xmlReader.IsStartElement() && xmlReader.Name == localName)
            {
                Name = xmlReader.GetAttribute("name");
                string steps = xmlReader.GetAttribute("steps");

                SetIntervals(steps);

                return true;
            }
            
            return false;
        }

        protected void WriteBase(XmlWriter xmlWriter, string localName)
        {
            if (null == xmlWriter)
            {
                throw new ArgumentNullException(nameof(xmlWriter));
            }

            if (StringUtils.IsNullOrWhiteSpace(localName))
            {
                throw new ArgumentNullException(nameof(localName));
            }

            xmlWriter.WriteStartElement(localName);

            xmlWriter.WriteAttributeString("name", Name);

            string intervals = GetIntervalString();

            xmlWriter.WriteAttributeString("steps", intervals);
        }

        public void Update(string name, int[] intervals)
        {
            if (StringUtils.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            if (ReadOnly)
            {
                throw new ObjectIsReadOnlyException(this);
            }

            UpdateParent = false;

            string oldName = Name;
            int[] oldIntervals = Intervals;

            Name = name;
            Intervals = intervals ?? throw new ArgumentNullException(nameof(intervals));

            Parent.Resort(this, () =>
            {
                Name = oldName;
                Intervals = oldIntervals;
                UpdateParent = true;
            });
        }

        public InternalNote[] GetNotes(InternalNote root)
        {
            return NamedInterval.GetNotes(root, Intervals);
        }

        public static InternalNote[] GetNotes(InternalNote root, int[] intervals)
        {
            if (null == intervals || intervals.Length == 0)
            {
                throw new ArgumentNullException(nameof(intervals));
            }

            InternalNote[] notes = new InternalNote[intervals.Length];

            for (int i = 0; i < notes.Length; i++)
            {
                notes[i] = NoteUtils.Shift(root, intervals[i]);
            }

            return notes;
        }

        public void MarkAsReadOnly()
        {
            ReadOnly = true;
        }

        public void SetIntervals(string intervalString)
        {
            Intervals = ParseIntervalString(intervalString);
        }

        public static int[] ParseIntervalString(string intervalString)
        {
            if (StringUtils.IsNullOrWhiteSpace(intervalString))
            {
                throw new ArgumentNullException(nameof(intervalString));
            }

            intervalString = intervalString.Trim().TrimEnd(_separator);

            string[] s = intervalString.Split(_separator);

            int[] intervals = new int[s.Length];

            for (int i = 0; i < intervals.Length; i++)
            {
                intervals[i] = int.Parse(s[i]);
            }

            return intervals;
        }

        protected string GetIntervalString()
        {
            string intervals = "";

            if (null != Intervals)
            {
                for (int i = 0; i < Intervals.Length; i++)
                {
                    intervals += Intervals[i] + _separator.ToString();
                }
            }

            intervals = intervals.TrimEnd(_separator);

            return intervals;
        }

        private const char _separator = ';';

        public int CompareTo(object obj)
        {
            if (null == obj)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            NamedInterval namedInterval = obj as NamedInterval;
            if (null == namedInterval)
            {
                throw new ArgumentException();
            }

            return LongName.CompareTo(namedInterval.LongName);
        }

        public override string ToString()
        {
            return LongName;
        }
    }
}
