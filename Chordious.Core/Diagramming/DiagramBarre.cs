// 
// DiagramBarre.cs
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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace com.jonthysell.Chordious.Core
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

        public bool Visible
        {
            get
            {
                return this.Style.BarreVisibleGet();
            }
            set
            {
                this.Style.BarreVisibleSet(value);
            }
        }

        public DiagramVerticalAlignment VerticalAlignment
        {
            get
            {
                return Style.BarreVerticalAlignmentGet();
            }
            set
            {
                Style.BarreVerticalAlignmentSet(value);
            }
        }

        public DiagramBarreStack Stack
        {
            get
            {
                return Style.BarreStackGet();
            }
            set
            {
                Style.BarreStackSet(value);
            }
        }

        public double ArcRatio
        {
            get
            {
                return Style.BarreArcRatioGet();
            }
            set
            {
                Style.BarreArcRatioSet(value);
            }
        }

        public DiagramBarre(Diagram parent, BarrePosition position) : base(parent, position) { }

        public DiagramBarre(Diagram parent, XmlReader xmlReader) : base(parent)
        {
            this.Read(xmlReader);
        }

        public override sealed void Read(XmlReader xmlReader)
        {
            if (null == xmlReader)
            {
                throw new ArgumentNullException("xmlReader");
            }

            using (xmlReader)
            {
                if (xmlReader.IsStartElement() && xmlReader.Name == "barre")
                {
                    this.Text = xmlReader.GetAttribute("text");

                    int fret = Int32.Parse(xmlReader.GetAttribute("fret"));
                    int start = Int32.Parse(xmlReader.GetAttribute("start"));
                    int end = Int32.Parse(xmlReader.GetAttribute("end"));

                    this.Position = new BarrePosition(fret, start, end);

                    while (xmlReader.Read())
                    {
                        if (xmlReader.IsStartElement() && xmlReader.Name == "style")
                        {
                            this.Style.Read(xmlReader.ReadSubtree());
                        }
                    }
                }
            }
        }

        public override void Write(XmlWriter xmlWriter)
        {
            if (null == xmlWriter)
            {
                throw new ArgumentNullException("xmlWriter");
            }

            xmlWriter.WriteStartElement("barre");

            xmlWriter.WriteAttributeString("text", this.Text);
            xmlWriter.WriteAttributeString("fret", this.Position.Fret.ToString());
            xmlWriter.WriteAttributeString("start", this.Position.StartString.ToString());
            xmlWriter.WriteAttributeString("end", this.Position.EndString.ToString());

            this.Style.Write(xmlWriter, "barre");

            xmlWriter.WriteEndElement();
        }

        public override bool IsVisible()
        {
            return this.Visible;
        }

        public override string ToSvg()
        {
            string svg = "";

            if (this.IsVisible())
            {
                double startX = this.Parent.GridLeftEdge() + (this.Parent.GridStringSpacing * (this.Position.StartString - 1));
                double endX = this.Parent.GridLeftEdge() + (this.Parent.GridStringSpacing * (this.Position.EndString - 1));

                double centerY = this.Parent.GridTopEdge() + (this.Parent.GridFretSpacing / 2.0) + (this.Parent.GridFretSpacing * (this.Position.Fret - 1));

                switch (this.VerticalAlignment)
                {
                    case DiagramVerticalAlignment.Bottom:
                        centerY += this.Parent.GridFretSpacing * 0.5;
                        break;
                    case DiagramVerticalAlignment.Top:
                        centerY -= this.Parent.GridFretSpacing * 0.5;
                        break;
                    case DiagramVerticalAlignment.Middle:
                    default:
                        break;
                }

                string shapeStyle = this.Style.GetSvgStyle(DiagramBarre._shapeStyleMap);
                shapeStyle += "fill-opacity:0;";

                double radiusX = (this.Position.EndString - this.Position.StartString) * this.Parent.GridStringSpacing;
                double radiusY = this.Parent.GridFretSpacing * this.ArcRatio;
                svg += String.Format(SvgConstants.ARC, shapeStyle, startX, centerY, radiusX, radiusY, endX, centerY);
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
