// 
// ChordQualitySet.cs
//  
// Author:
//       Jon Thysell <thysell@gmail.com>
// 
// Copyright (c) 2015, 2017 Jon Thysell <http://jonthysell.com>
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
    public class ChordQualitySet : NamedIntervalSet
    {
        public new ChordQualitySet Parent
        {
            get
            {
                return (ChordQualitySet)base.Parent;
            }
            set
            {
                base.Parent = value;
            }
        }

        internal ChordQualitySet(string level) : base(level) { }

        internal ChordQualitySet(ChordQualitySet parent, string level) : base(parent, level) { }

        public new ChordQuality Get(string longName)
        {
            return (ChordQuality)(base.Get(longName));
        }

        public bool TryGet(string longName, out ChordQuality chordQuality)
        {
            NamedInterval namedInterval;
            if (base.TryGet(longName, out namedInterval))
            {
                chordQuality = (ChordQuality)namedInterval;
                return true;
            }

            chordQuality = null;
            return false;
        }
        
        public ChordQuality Add(string name, string abbreviation, int[] intervals)
        {
            if (ReadOnly)
            {
                throw new ObjectIsReadOnlyException(this);
            }

            ChordQuality chordQuality = new ChordQuality(this, name, abbreviation, intervals);
            Add(chordQuality);
            return chordQuality;
        }

        public void CopyFrom(ChordQualitySet chordQualitySet)
        {
            if (null == chordQualitySet)
            {
                throw new ArgumentNullException("chordQualitySet");
            }

            foreach (ChordQuality sourceChordQuality in chordQualitySet)
            {
                bool found = false;
                foreach (NamedInterval ni in _namedIntervals)
                {
                    ChordQuality chordQuality = (ChordQuality)ni;
                    if (sourceChordQuality == chordQuality)
                    {
                        found = true;
                        break;
                    }
                }

                // Only add if it wasn't found
                if (!found)
                {
                    Add(sourceChordQuality.Name, sourceChordQuality.Abbreviation, sourceChordQuality.Intervals);
                }
            }
        }

        public override void Read(XmlReader xmlReader)
        {
            if (null == xmlReader)
            {
                throw new ArgumentNullException("xmlReader");
            }

            using (xmlReader)
            {
                do
                {
                    if (xmlReader.IsStartElement() && xmlReader.Name == "quality")
                    {
                        ChordQuality chordQuality = new ChordQuality(this, xmlReader.ReadSubtree());
                        Add(chordQuality);
                    }
                } while (xmlReader.Read());
            }
        }

        public override void Write(XmlWriter xmlWriter)
        {
            if (null == xmlWriter)
            {
                throw new ArgumentNullException("xmlWriter");
            }

            foreach (NamedInterval namedInterval in _namedIntervals)
            {
                ChordQuality chordQuality = (ChordQuality)namedInterval;
                chordQuality.Write(xmlWriter);
            }
        }
    }
}
