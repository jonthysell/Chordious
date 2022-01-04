// Copyright (c) Jon Thysell <http://jonthysell.com>
// Licensed under the MIT License.

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

                string shapeStyle = Style.GetSvgStyle(_shapeStyleMap);
                shapeStyle += "fill-opacity:0;";

                double radiusX = Math.Max((Position.EndString - Position.StartString) * Parent.Style.GridStringSpacing, ArcRadiusMin);
                double radiusY = Math.Max(Parent.Style.GridFretSpacing * Style.BarreArcRatio, ArcRadiusMin);
                svg += string.Format(CultureInfo.InvariantCulture, SvgConstants.ARC, shapeStyle, startX, centerY, radiusX, radiusY, endX, centerY);
            }

            return svg;
        }

        private static readonly string[][] _shapeStyleMap =
        {
            new string[] {"barre.opacity", "opacity"},
            new string[] {"barre.linecolor", "stroke"},
            new string[] {"barre.linethickness", "stroke-width"},
        };

        private const double ArcRadiusMin = 0.01;

        public override string ToXaml()
        {
            throw new NotImplementedException();
        }
    }
}
