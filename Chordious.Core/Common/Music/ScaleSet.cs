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
    public class ScaleSet : NamedIntervalSet
    {
        public new ScaleSet Parent
        {
            get
            {
                return (ScaleSet)base.Parent;
            }
            set
            {
                base.Parent = value;
            }
        }

        internal ScaleSet(string level) : base(level) { }

        internal ScaleSet(ScaleSet parent, string level) : base(parent, level) { }

        public new Scale Get(string longName)
        {
            return (Scale)(base.Get(longName));
        }

        public bool TryGet(string longName, out Scale scale)
        {
            NamedInterval namedInterval;
            if (base.TryGet(longName, out namedInterval))
            {
                scale = (Scale)namedInterval;
                return true;
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
            Add(scale);
            return scale;
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
                foreach (NamedInterval namedInterval in _namedIntervals)
                {
                    Scale scale = (Scale)namedInterval;
                    if (sourceScale == scale)
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
                    if (xmlReader.IsStartElement() && xmlReader.Name == "scale")
                    {
                        Scale scale = new Scale(this, xmlReader.ReadSubtree());
                        Add(scale);
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
                Scale scale = (Scale)namedInterval;
                scale.Write(xmlWriter);
            }
        }
    }
}
