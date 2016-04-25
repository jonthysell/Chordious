// 
// ChordiousSettings.cs
//  
// Author:
//       Jon Thysell <thysell@gmail.com>
// 
// Copyright (c) 2015, 2016 Jon Thysell <http://jonthysell.com>
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
    public class ChordiousSettings : InheritableDictionary
    {
        public new ChordiousSettings Parent
        {
            get
            {
                return (ChordiousSettings)base.Parent;
            }
            internal set
            {
                base.Parent = value;
            }
        }

        public ChordiousSettings() : base() { }

        public ChordiousSettings(string level) : base(level) { }

        public ChordiousSettings(ChordiousSettings parentSettings) : base(parentSettings) { }

        public ChordiousSettings(ChordiousSettings parentSettings, string level) : base(parentSettings, level) { }

        public void Read(XmlReader xmlReader)
        {
            base.Read(xmlReader, "setting");
        }

        public void Write(XmlWriter xmlWriter, string filter = "")
        {
            base.Write(xmlWriter, "setting", filter);
        }

        public ChordiousSettings Clone()
        {
            ChordiousSettings cs = new ChordiousSettings(this.Parent, this.Level);
            cs.CopyFrom(this);
            return cs;
        }
    }
}
