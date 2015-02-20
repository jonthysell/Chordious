// 
// ChordQualitySet.cs
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
using System.Collections;
using System.Collections.Generic;
using System.Xml;

namespace com.jonthysell.Chordious.Core
{
    public class ChordQualitySet : IReadOnly, IEnumerable<ChordQuality>
    {
        public bool ReadOnly { get; private set; }

        public string Level
        {
            get
            {
                return this._level;
            }
            set
            {
                if (StringUtils.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentNullException();
                }

                if (this.ReadOnly)
                {
                    throw new ObjectIsReadOnlyException(this);
                }

                this._level = value;
            }
        }
        private string _level;

        public ChordQualitySet Parent
        {
            get
            {
                return this._parent;
            }
            set
            {
                if (this.ReadOnly)
                {
                    throw new ObjectIsReadOnlyException(this);
                }

                this._parent = value;
            }
        }
        private ChordQualitySet _parent;

        private List<ChordQuality> _chordQualities;

        internal ChordQualitySet(string level)
        {
            Level = level;
            ReadOnly = false;
            _chordQualities = new List<ChordQuality>();
        }

        internal ChordQualitySet(ChordQualitySet parent, string level) : this(level)
        {
            if (null == parent)
            {
                throw new ArgumentNullException("parent");
            }

            Parent = parent;
        }

        public void MarkAsReadOnly()
        {
            ReadOnly = true;
            foreach (ChordQuality cq in _chordQualities)
            {
                cq.MarkAsReadOnly();
            }
        }

        public IEnumerator<ChordQuality> GetEnumerator()
        {
            foreach (ChordQuality chordQuality in _chordQualities)
            {
                yield return chordQuality;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public ChordQuality Get(string name)
        {
            if (StringUtils.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException("name");
            }

            ChordQuality chordQuality;
            if (TryGet(name, out chordQuality))
            {
                return chordQuality;
            }

            throw new ChordQualityNotFoundException(this, name);
        }

        public bool TryGet(string name, out ChordQuality chordQuality)
        {
            if (StringUtils.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException("name");
            }

            foreach (ChordQuality cq in _chordQualities)
            {
                if (cq.Name == name)
                {
                    chordQuality = cq;
                    return true;
                }
            }

            chordQuality = null;
            return false;
        }
        
        public ChordQuality Add(string name, string abbreviation, int[] intervals)
        {
            if (this.ReadOnly)
            {
                throw new ObjectIsReadOnlyException(this);
            }

            ChordQuality chordQuality = new ChordQuality(this, name, abbreviation, intervals);
            _chordQualities.Add(chordQuality);
            return chordQuality;
        }

        public void Remove(string name)
        {
            if (this.ReadOnly)
            {
                throw new ObjectIsReadOnlyException(this);
            }

            _chordQualities.Remove(Get(name));
        }

        public void Read(XmlReader xmlReader)
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
                        _chordQualities.Add(chordQuality);
                    }
                } while (xmlReader.Read());
            }
        }

        public void Write(XmlWriter xmlWriter)
        {
            if (null == xmlWriter)
            {
                throw new ArgumentNullException("xmlWriter");
            }

            foreach (ChordQuality cq in _chordQualities)
            {
                cq.Write(xmlWriter);
            }
        }
    }

    public class ChordQualitySetException : ChordiousException
    {
        public ChordQualitySet ChordQualitySet { get; private set; }

        public ChordQualitySetException(ChordQualitySet chordQualitySet) : base()
        {
            this.ChordQualitySet = chordQualitySet;
        }
    }

    public abstract class TargetChordQualityException : ChordQualitySetException
    {
        public string Name { get; private set; }

        public TargetChordQualityException(ChordQualitySet chordQualitySet, string name) : base(chordQualitySet)
        {
            this.Name = name;
        }
    }

    public class ChordQualityNotFoundException : TargetChordQualityException
    {
        public ChordQualityNotFoundException(ChordQualitySet chordQualitySet, string name) : base(chordQualitySet, name) { }
    }

    public class ChordQualityNameAlreadyExistsException : TargetChordQualityException
    {
        public ChordQualityNameAlreadyExistsException(ChordQualitySet chordQualitySet, string name) : base(chordQualitySet, name) { }
    }
}
