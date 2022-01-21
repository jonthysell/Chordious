// Copyright (c) Jon Thysell <http://jonthysell.com>
// Licensed under the MIT License.

using System;
using System.Xml;

using Chordious.Core.Resources;

namespace Chordious.Core
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
                _text = value?.Trim();
            }
        }
        private string _text;

        public DiagramElement(Diagram parent)
        {
            Parent = parent ?? throw new ArgumentNullException(nameof(parent));
            Style = new DiagramStyle(parent.Style, "Element");
        }

        public DiagramElement(Diagram parent, ElementPosition position) : this(parent)
        {
            Position = position ?? throw new ArgumentNullException(nameof(position));
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
