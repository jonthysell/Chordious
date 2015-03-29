// 
// ChordQuality.cs
//  
// Author:
//       Jon Thysell <thysell@gmail.com>
// 
// Copyright (c) 2013, 2014, 2015 Jon Thysell <http://jonthysell.com>
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
    public class ChordQuality : NamedInterval
    {
        public ChordQualitySet Parent
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
        private ChordQualitySet _parent;

        public override string Level
        {
            get
            {
                return Parent.Level;
            }
        }

        public string Abbreviation
        {
            get
            {
                return _abbreviation;
            }
            set
            {
                if (null == value)
                {
                    value = "";
                }

                if (ReadOnly)
                {
                    throw new ObjectIsReadOnlyException(this);
                }

                _abbreviation = value;
            }
        }
        private string _abbreviation;

        public override string LongName
        {
            get
            {
                return String.Format("{0} ({1}) ({2})", Name, Abbreviation, GetIntervalString());
            }
        }

        private ChordQuality(ChordQualitySet parent)
        {
            this.Parent = parent;
        }

        internal ChordQuality(ChordQualitySet parent, string name, string abbreviation, int[] intervals) : this(parent)
        {
            this.Name = name;
            this.Abbreviation = abbreviation;
            this.Intervals = intervals;
        }

        internal ChordQuality(ChordQualitySet parent, XmlReader xmlReader) : this(parent)
        {
            if (null == xmlReader)
            {
                throw new ArgumentNullException("xmlReader");
            }

            using (xmlReader)
            {
                if (ReadBase(xmlReader, "quality"))
                {
                    this.Abbreviation = xmlReader.GetAttribute("abbv");
                }
            }
        }

        public void Write(XmlWriter xmlWriter)
        {
            if (null == xmlWriter)
            {
                throw new ArgumentNullException("xmlWriter");
            }

            WriteBase(xmlWriter);

            xmlWriter.WriteAttributeString("abbv", this.Abbreviation);

            xmlWriter.WriteEndElement();
        }
    }
}