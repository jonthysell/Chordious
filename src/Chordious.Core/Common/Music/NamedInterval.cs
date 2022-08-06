// Copyright (c) Jon Thysell <http://jonthysell.com>
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Linq;
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
                _parent = value ?? throw new ArgumentNullException(nameof(value));
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
                    throw new ArgumentNullException(nameof(value));
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
                _intervals = value ?? throw new ArgumentNullException(nameof(value));

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
            if (xmlReader is null)
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
            if (xmlWriter is null)
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
            return GetNotes(root, Intervals);
        }

        public InternalNote[] GetUniqueNotes(InternalNote root, bool includeRoot = true)
        {
            HashSet<InternalNote> uniqueNotes = new HashSet<InternalNote>();

            foreach (InternalNote note in GetNotes(root))
            {
                if (note != root || includeRoot)
                {
                    uniqueNotes.Add(note);
                }
            }

            return uniqueNotes.ToArray();
        }

        public static InternalNote[] GetNotes(InternalNote root, int[] intervals)
        {
            if (intervals is null || intervals.Length == 0)
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

            if (Intervals is not null)
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
            if (obj is null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            if (obj is not NamedInterval namedInterval)
            {
                throw new ArgumentOutOfRangeException(nameof(obj));
            }

            return LongName.CompareTo(namedInterval.LongName);
        }

        public override string ToString()
        {
            return LongName;
        }
    }
}
