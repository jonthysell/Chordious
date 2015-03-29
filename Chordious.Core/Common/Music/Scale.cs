// 
// Scale.cs
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
    public class Scale : NamedInterval
    {
        public ScaleSet Parent
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
        private ScaleSet _parent;

        public override string Level
        {
            get
            {
                return Parent.Level;
            }
        }

        private Scale(ScaleSet parent)
        {
            this.Parent = parent;
        }

        internal Scale(ScaleSet parent, string name, int[] intervals) : this(parent)
        {
            this.Name = name;            
            this.Intervals = intervals;
        }

        internal Scale(ScaleSet parent, XmlReader xmlReader) : this(parent)
        {
            if (null == xmlReader)
            {
                throw new ArgumentNullException("xmlReader");
            }

            using (xmlReader)
            {
                ReadBase(xmlReader, "scale");
            }
        }

        public void Write(XmlWriter xmlWriter)
        {
            if (null == xmlWriter)
            {
                throw new ArgumentNullException("xmlWriter");
            }

            WriteBase(xmlWriter);

            xmlWriter.WriteEndElement();
        }
    }
}