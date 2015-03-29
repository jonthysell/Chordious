// 
// NamedInterval.cs
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
using System.Xml;

namespace com.jonthysell.Chordious.Core
{
    public abstract class NamedInterval : IReadOnly
    {
        public bool ReadOnly { get; protected set; }

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

                if (ReadOnly)
                {
                    throw new ObjectIsReadOnlyException(this);
                }

                _name = value;
            }
        }
        private string _name;

        public virtual string LongName
        {
            get
            {
                return String.Format("{0} ({1})", Name, GetIntervalString());
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
                if (null == value)
                {
                    throw new ArgumentNullException();
                }

                if (ReadOnly)
                {
                    throw new ObjectIsReadOnlyException(this);
                }

                _intervals = value;
            }
        }
        private int[] _intervals;

        public abstract string Level { get; }

        protected bool ReadBase(XmlReader xmlReader, string localName)
        {
            if (null == xmlReader)
            {
                throw new ArgumentNullException("xmlReader");
            }

            if (StringUtils.IsNullOrWhiteSpace(localName))
            {
                throw new ArgumentNullException("localName");
            }

            if (xmlReader.IsStartElement() && xmlReader.Name == localName)
            {
                this.Name = xmlReader.GetAttribute("name");

                string steps = xmlReader.GetAttribute("steps");

                SetIntervals(steps);

                return true;
            }

            return false;
        }

        protected void WriteBase(XmlWriter xmlWriter)
        {
            if (null == xmlWriter)
            {
                throw new ArgumentNullException("xmlWriter");
            }

            xmlWriter.WriteStartElement("quality");

            xmlWriter.WriteAttributeString("name", this.Name);

            string intervals = GetIntervalString();

            xmlWriter.WriteAttributeString("steps", intervals);
        }

        public InternalNote[] GetNotes(InternalNote root)
        {
            InternalNote[] notes = new InternalNote[Intervals.Length];

            for (int i = 0; i < notes.Length; i++)
            {
                notes[i] = NoteUtils.Shift(root, Intervals[i]);
            }

            return notes;
        }

        public void MarkAsReadOnly()
        {
            this.ReadOnly = true;
        }

        public void SetIntervals(string intervalString)
        {
            this.Intervals = ParseIntervalString(intervalString);
        }

        public static int[] ParseIntervalString(string intervalString)
        {
            if (StringUtils.IsNullOrWhiteSpace(intervalString))
            {
                throw new ArgumentNullException("intervalString");
            }

            intervalString = intervalString.Trim().TrimEnd(_separator);

            string[] s = intervalString.Split(_separator);

            int[] intervals = new int[s.Length];

            for (int i = 0; i < intervals.Length; i++)
            {
                intervals[i] = Int32.Parse(s[i]);
            }

            return intervals;
        }

        protected string GetIntervalString()
        {
            string intervals = "";

            for (int i = 0; i < Intervals.Length; i++)
            {
                intervals += Intervals[i] + _separator.ToString();
            }

            intervals = intervals.TrimEnd(_separator);

            return intervals;
        }

        private const char _separator = ';';
    }
}