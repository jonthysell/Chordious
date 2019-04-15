// 
// DiagramElement.cs
//  
// Author:
//       Jon Thysell <thysell@gmail.com>
// 
// Copyright (c) 2015, 2017, 2019 Jon Thysell <http://jonthysell.com>
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

using com.jonthysell.Chordious.Core.Resources;

namespace com.jonthysell.Chordious.Core
{
    public abstract class DiagramElement
    {
        public Diagram Parent { get; private set; }

        public DiagramStyle Style { get; private set; }

        public ElementPosition Position
        {
            get
            {
                return _position;
            }
            set
            {
                if (!Parent.CanPositionElementAt(this, value))
                {
                    throw new ElementAlreadyExistsAtPositionException(value);
                }

                if (!Parent.ValidPosition(value))
                {
                    throw new ElementPositionOffFretboardException(value);
                }

                _position = value.Clone();
            }
        }
        private ElementPosition _position;

        public string Text
        {
            get
            {
                return _text;
            }
            set
            {
                if (StringUtils.IsNullOrWhiteSpace(value))
                {
                    value = "";
                }

                _text = value.Trim();
            }
        }
        private string _text;

        public DiagramElement(Diagram parent)
        {
            Parent = parent ?? throw new ArgumentNullException("parent");
            Style = new DiagramStyle(parent.Style, "Element");
        }

        public DiagramElement(Diagram parent, ElementPosition position) : this(parent)
        {
            Position = position ?? throw new ArgumentNullException("position");
        }

        public DiagramElement(Diagram parent, ElementPosition position, string text) : this(parent, position)
        {
            Text = text;
        }

        public abstract bool IsVisible();

        public abstract void Read(XmlReader xmlReader);

        public abstract void Write(XmlWriter xmlWriter);

        public string ToImageMarkup(ImageMarkupType type)
        {
            switch (type)
            {
                case ImageMarkupType.SVG:
                    return ToSvg();
                case ImageMarkupType.XAML:
                    return ToXaml();
            }

            return string.Empty;
        }

        public abstract string ToSvg();
        public abstract string ToXaml();
    }

    public class ElementAlreadyExistsAtPositionException : ElementPositionException
    {
        public override string Message
        {
            get
            {
                return Strings.ElementAlreadyExistsAtPositionExceptionMessage;
            }
        }

        public ElementAlreadyExistsAtPositionException(ElementPosition position) : base(position) { }
    }

    public class ElementPositionOffFretboardException : ElementPositionException
    {
        public override string Message
        {
            get
            {
                return Strings.ElementPositionOffFretboardExceptionMessage;
            }
        }

        public ElementPositionOffFretboardException(ElementPosition position) : base(position) { }
    }
}
