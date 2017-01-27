// 
// ChordQuality.cs
//  
// Author:
//       Jon Thysell <thysell@gmail.com>
// 
// Copyright (c) 2013, 2014, 2015, 2016, 2017 Jon Thysell <http://jonthysell.com>
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
    public class ChordQuality : NamedInterval, IChordQuality
    {
        public string Abbreviation
        {
            get
            {
                return _abbreviation;
            }
            set
            {
                if (StringUtils.IsNullOrWhiteSpace(value))
                {
                    value = "";
                }

                if (ReadOnly)
                {
                    throw new ObjectIsReadOnlyException(this);
                }

                string oldValue = _abbreviation;
                _abbreviation = value;

                // Resort with parent
                if (UpdateParent)
                {
                    Parent.Resort(this, () =>
                    {
                        _abbreviation = oldValue;
                    });
                }
            }
        }
        private string _abbreviation;

        internal ChordQuality(ChordQualitySet parent, string name, string abbreviation, int[] intervals) : base(parent)
        {
            if (null == parent)
            {
                throw new ArgumentNullException("parent");
            }

            Name = name;
            Abbreviation = abbreviation;
            Intervals = intervals;

            UpdateParent = true;
        }

        internal ChordQuality(ChordQualitySet parent, XmlReader xmlReader) : base(parent)
        {
            if (null == parent)
            {
                throw new ArgumentNullException("parent");
            }

            if (null == xmlReader)
            {
                throw new ArgumentNullException("xmlReader");
            }

            using (xmlReader)
            {
                if (ReadBase(xmlReader, "quality"))
                {
                    Abbreviation = xmlReader.GetAttribute("abbv");
                }
            }

            UpdateParent = true;
        }

        protected override string GetLongName()
        {
            if (!StringUtils.IsNullOrWhiteSpace(Abbreviation))
            {
                return string.Format("{0} \"{1}\" ({2})", Name, Abbreviation, GetIntervalString());
            }

            return base.GetLongName();
        }

        public void Update(string name, string abbreviation, int[] intervals)
        {
            if (StringUtils.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException("name");
            }

            if (null == intervals)
            {
                throw new ArgumentNullException("intervals");
            }

            if (ReadOnly)
            {
                throw new ObjectIsReadOnlyException(this);
            }

            UpdateParent = false;

            string oldName = Name;
            string oldAbbreviation = Abbreviation;
            int[] oldIntervals = Intervals;

            Name = name;
            Abbreviation = abbreviation;
            Intervals = intervals;

            Parent.Resort(this, () =>
            {
                Name = oldName;
                Abbreviation = oldAbbreviation;
                Intervals = oldIntervals;
                UpdateParent = true;
            });
        }

        public void Write(XmlWriter xmlWriter)
        {
            if (null == xmlWriter)
            {
                throw new ArgumentNullException("xmlWriter");
            }

            WriteBase(xmlWriter, "quality");

            xmlWriter.WriteAttributeString("abbv", Abbreviation);

            xmlWriter.WriteEndElement();
        }
    }
}