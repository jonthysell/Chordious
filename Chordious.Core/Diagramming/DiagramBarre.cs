// 
// DiagramBarre.cs
//  
// Author:
//       Jon Thysell <thysell@gmail.com>
// 
// Copyright (c) 2015, 2016, 2017, 2019 Jon Thysell <http://jonthysell.com>
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
using System.Globalization;
using System.Xml;

namespace Chordious.Core
{
    public class DiagramBarre : DiagramElement
    {
        public new BarrePosition Position
        {
            get
            {
                return (BarrePosition)base.Position;
            }
            set
            {
                base.Position = value;
            }
        }

        public DiagramBarre(Diagram parent, BarrePosition position) : base(parent, position) { }

        public DiagramBarre(Diagram parent, XmlReader xmlReader) : base(parent)
        {
            Read(xmlReader);
        }

        public override sealed void Read(XmlReader xmlReader)
        {
            if (null == xmlReader)
            {
                throw new ArgumentNullException(nameof(xmlReader));
            }

            using (xmlReader)
            {
                if (xmlReader.IsStartElement() && xmlReader.Name == "barre")
                {
                    Text = xmlReader.GetAttribute("text");

                    int fret = int.Parse(xmlReader.GetAttribute("fret"));
                    int start = int.Parse(xmlReader.GetAttribute("start"));
                    int end = int.Parse(xmlReader.GetAttribute("end"));

                    Position = new BarrePosition(fret, start, end);

                    while (xmlReader.Read())
                    {
                        if (xmlReader.IsStartElement() && xmlReader.Name == "style")
                        {
                            Style.Read(xmlReader.ReadSubtree());
                        }
                    }
                }
            }
        }

        public override void Write(XmlWriter xmlWriter)
        {
            if (null == xmlWriter)
            {
                throw new ArgumentNullException(nameof(xmlWriter));
            }

            xmlWriter.WriteStartElement("barre");

            xmlWriter.WriteAttributeString("text", Text);
            xmlWriter.WriteAttributeString("fret", Position.Fret.ToString());
            xmlWriter.WriteAttributeString("start", Position.StartString.ToString());
            xmlWriter.WriteAttributeString("end", Position.EndString.ToString());

            Style.Write(xmlWriter, "barre");

            xmlWriter.WriteEndElement();
        }

        public override bool IsVisible()
        {
            return Style.BarreVisible;
        }

        public override string ToSvg()
        {
            string svg = "";

            if (IsVisible())
            {
                double startX = Parent.GridLeftEdge() + (Parent.Style.GridStringSpacing * (Position.StartString - 1));
                double endX = Parent.GridLeftEdge() + (Parent.Style.GridStringSpacing * (Position.EndString - 1));

                double centerY = Parent.GridTopEdge() + (Parent.Style.GridFretSpacing / 2.0) + (Parent.Style.GridFretSpacing * (Position.Fret - 1));

                switch (Style.BarreVerticalAlignment)
                {
                    case DiagramVerticalAlignment.Bottom:
                        centerY += Parent.Style.GridFretSpacing * 0.5;
                        break;
                    case DiagramVerticalAlignment.Top:
                        centerY -= Parent.Style.GridFretSpacing * 0.5;
                        break;
                    case DiagramVerticalAlignment.Middle:
                    default:
                        break;
                }

                string shapeStyle = Style.GetSvgStyle(DiagramBarre._shapeStyleMap);
                shapeStyle += "fill-opacity:0;";

                double radiusX = (Position.EndString - Position.StartString) * Parent.Style.GridStringSpacing;
                double radiusY = Parent.Style.GridFretSpacing * Style.BarreArcRatio;
                svg += string.Format(CultureInfo.InvariantCulture, SvgConstants.ARC, shapeStyle, startX, centerY, radiusX, radiusY, endX, centerY);
            }

            return svg;
        }

        private static string[][] _shapeStyleMap =
        {
            new string[] {"barre.opacity", "opacity"},
            new string[] {"barre.linecolor", "stroke"},
            new string[] {"barre.linethickness", "stroke-width"},
        };

        public override string ToXaml()
        {
            throw new NotImplementedException();
        }
    }
}
