// 
// ScaleSet.cs
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
    public class ScaleSet : IReadOnly, IEnumerable<Scale>
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

        public ScaleSet Parent
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
        private ScaleSet _parent;

        private List<Scale> _scales;

        internal ScaleSet(string level)
        {
            Level = level;
            ReadOnly = false;
            _scales = new List<Scale>();
        }

        internal ScaleSet(ScaleSet parent, string level) : this(level)
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
            foreach (Scale s in _scales)
            {
                s.MarkAsReadOnly();
            }
        }

        public IEnumerator<Scale> GetEnumerator()
        {
            foreach (Scale scale in _scales)
            {
                yield return scale;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public Scale Get(string name)
        {
            if (StringUtils.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException("name");
            }

            Scale scale;
            if (TryGet(name, out scale))
            {
                return scale;
            }

            throw new ScaleNotFoundException(this, name);
        }

        public bool TryGet(string name, out Scale scale)
        {
            if (StringUtils.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException("name");
            }

            foreach (Scale s in _scales)
            {
                if (s.Name == name)
                {
                    scale = s;
                    return true;
                }
            }

            scale = null;
            return false;
        }

        public Scale Add(string name, int[] intervals)
        {
            if (this.ReadOnly)
            {
                throw new ObjectIsReadOnlyException(this);
            }

            Scale scale = new Scale(this, name, intervals);
            _scales.Add(scale);
            return scale;
        }

        public void Remove(string name)
        {
            if (this.ReadOnly)
            {
                throw new ObjectIsReadOnlyException(this);
            }

            _scales.Remove(Get(name));
        }

        public void CopyFrom(ScaleSet scaleSet)
        {
            if (null == scaleSet)
            {
                throw new ArgumentNullException("scaleSet");
            }

            foreach (Scale sourceScale in scaleSet)
            {
                bool found = false;
                foreach (Scale scale in _scales)
                {
                    if (sourceScale.Equals(scale))
                    {
                        found = true;
                        break;
                    }
                }

                // Only add if it wasn't found
                if (!found)
                {
                    Add(sourceScale.Name, sourceScale.Intervals);
                }
            }
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
                    if (xmlReader.IsStartElement() && xmlReader.Name == "scale")
                    {
                        Scale scale = new Scale(this, xmlReader.ReadSubtree());
                        _scales.Add(scale);
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

            foreach (Scale scale in _scales)
            {
                scale.Write(xmlWriter);
            }
        }
    }

    public abstract class ScaleSetException : ChordiousException
    {
        public ScaleSet ScaleSet { get; private set; }

        public ScaleSetException(ScaleSet scaleSet) : base()
        {
            this.ScaleSet = scaleSet;
        }
    }

    public abstract class TargetScaleException : ScaleSetException
    {
        public string Name { get; private set; }

        public TargetScaleException(ScaleSet scaleSet, string name) : base(scaleSet)
        {
            this.Name = name;
        }
    }

    public class ScaleNotFoundException : TargetScaleException
    {
        public override string Message
        {
            get
            {
                return String.Format(Resources.Strings.ScaleNotFoundExceptionMessage, Name);
            }
        }

        public ScaleNotFoundException(ScaleSet scaleSet, string name) : base(scaleSet, name) { }
    }

    public class ScaleNameAlreadyExistsException : TargetScaleException
    {
        public override string Message
        {
            get
            {
                return String.Format(Resources.Strings.ScaleNameAlreadyExistsMessage, Name);
            }
        }

        public ScaleNameAlreadyExistsException(ScaleSet scaleSet, string name) : base(scaleSet, name) { }
    }
}
