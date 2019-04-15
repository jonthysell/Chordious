// 
// DiagramMark.cs
//  
// Author:
//       Jon Thysell <thysell@gmail.com>
// 
// Copyright (c) 2015, 2016, 2017, 2018, 2019 Jon Thysell <http://jonthysell.com>
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

namespace com.jonthysell.Chordious.Core
{
    public class DiagramMark : DiagramElement
    {
        public new MarkPosition Position
        {
            get
            {
                return (MarkPosition)base.Position;
            }
            set
            {
                base.Position = value;
            }
        }

        public DiagramMarkType Type
        {
            get
            {
                return MarkStyle.MarkType;
            }
            set
            {
                MarkStyle.MarkType = value;
            }
        }

        public DiagramMarkStyleWrapper MarkStyle { get; private set; }

        public DiagramMark(Diagram parent, MarkPosition position, string text = "") : base(parent, position, text)
        {
            MarkStyle = new DiagramMarkStyleWrapper(Style, position.Fret > parent.NumFrets ? DiagramMarkType.Bottom : DiagramMarkType.Normal);
        }

        public DiagramMark(Diagram parent, XmlReader xmlReader) : base(parent)
        {
            MarkStyle = new DiagramMarkStyleWrapper(Style);
            Read(xmlReader);
        }

        public override bool IsVisible()
        {
            return MarkStyle.MarkVisible;
        }

        public bool IsAboveTopEdge()
        {
            return (Position.Fret == 0);
        }

        public bool IsBelowBottomEdge()
        {
            return (Position.Fret == Parent.NumFrets + 1);
        }

        public override sealed void Read(XmlReader xmlReader)
        {
            if (null == xmlReader)
            {
                throw new ArgumentNullException(nameof(xmlReader));
            }

            using (xmlReader)
            {
                if (xmlReader.IsStartElement() && xmlReader.Name == "mark")
                {
                    Text = xmlReader.GetAttribute("text");
                    Type = (DiagramMarkType)Enum.Parse(typeof(DiagramMarkType), xmlReader.GetAttribute("type"));

                    int @string = int.Parse(xmlReader.GetAttribute("string"));
                    int fret = int.Parse(xmlReader.GetAttribute("fret"));

                    Position = new MarkPosition(@string, fret);

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

            xmlWriter.WriteStartElement("mark");

            xmlWriter.WriteAttributeString("text", Text);
            xmlWriter.WriteAttributeString("type", Type.ToString());
            xmlWriter.WriteAttributeString("string", Position.String.ToString());
            xmlWriter.WriteAttributeString("fret", Position.Fret.ToString());

            string prefix = DiagramStyle.GetMarkStylePrefix(Type);

            // Only write the styles that apply to the type of this mark
            Style.Write(xmlWriter, prefix + "mark");

            xmlWriter.WriteEndElement();
        }

        public override string ToSvg()
        {
            string svg = "";

            if (IsVisible())
            {
                string prefix = DiagramStyle.GetMarkStylePrefix(Type);

                DiagramMarkShape shape = MarkStyle.MarkShape;
                double radius = MarkStyle.MarkRadiusRatio * 0.5 * Math.Min(Parent.Style.GridStringSpacing, Parent.Style.GridFretSpacing);

                double centerX = Parent.GridLeftEdge() + (Parent.Style.GridStringSpacing * (Position.String - 1));
                double centerY = Parent.GridTopEdge() + (Parent.Style.GridFretSpacing / 2.0) + (Parent.Style.GridFretSpacing * (Position.Fret - 1));

                // Draw shape

                string shapeStyle = Style.GetSvgStyle(DiagramMark._shapeStyleMap, prefix);

                if (MarkStyle.MarkBorderThickness > 0)
                {
                    shapeStyle += Style.GetSvgStyle(DiagramMark._shapeStyleMapBorder, prefix);
                }

                switch (shape)
                {
                    case DiagramMarkShape.Circle:
                        svg += string.Format(CultureInfo.InvariantCulture, SvgConstants.CIRCLE, shapeStyle, radius, centerX, centerY);
                        break;
                    case DiagramMarkShape.Square:
                        svg += string.Format(CultureInfo.InvariantCulture, SvgConstants.RECTANGLE, shapeStyle, radius * 2.0, radius * 2.0, centerX - radius, centerY - radius);
                        break;
                    case DiagramMarkShape.Diamond:
                        string diamondPoints = "";
                        for (int i = 0; i < 4; i++)
                        {
                            double angle = (i * 90.0) * (Math.PI / 180.0);
                            diamondPoints += string.Format(CultureInfo.InvariantCulture, "{0},{1} ", centerX + (radius * Math.Cos(angle)), centerY + (radius * Math.Sin(angle)));
                        }
                        svg += string.Format(CultureInfo.InvariantCulture, SvgConstants.POLYGON, shapeStyle, diamondPoints);
                        break;
                    case DiagramMarkShape.X:
                        string xPoints = "";
                        for (int i = 0; i < 4; i++)
                        {
                            double angle = (45.0 + (i * 90.0));

                            double rad0 = (angle - 45.0) * (Math.PI / 180.0); // Starting close point
                            double len0 = Math.Sqrt(2 * Math.Pow(radius * Math.Sin(XThicknessAngle), 2));
                            xPoints += string.Format(CultureInfo.InvariantCulture, "{0},{1} ", centerX + (len0 * Math.Cos(rad0)), centerY + (len0 * Math.Sin(rad0)));

                            double rad1 = (angle * (Math.PI / 180.0)) - XThicknessAngle; // First far corner
                            xPoints += string.Format(CultureInfo.InvariantCulture, "{0},{1} ", centerX + (radius * Math.Cos(rad1)), centerY + (radius * Math.Sin(rad1)));

                            double rad2 = (angle * (Math.PI / 180.0)) + XThicknessAngle; // Second far corner
                            xPoints += string.Format(CultureInfo.InvariantCulture, "{0},{1} ", centerX + (radius * Math.Cos(rad2)), centerY + (radius * Math.Sin(rad2)));
                        }
                        svg += string.Format(CultureInfo.InvariantCulture, SvgConstants.POLYGON, shapeStyle, xPoints.Trim());
                        break;
                    case DiagramMarkShape.None:
                    default:
                        break;
                }

                // Draw text
                if (!StringUtils.IsNullOrWhiteSpace(Text) && MarkStyle.MarkTextVisible)
                {
                    double textSize = radius * 2.0 * MarkStyle.MarkTextSizeRatio;

                    double textX = centerX;
                    double textY = centerY;

                    switch (MarkStyle.MarkTextAlignment)
                    {
                        case DiagramHorizontalAlignment.Left:
                            textX += (Parent.Style.Orientation == DiagramOrientation.LeftRight) ? -1.0 * textSize * 0.5 : -1.0 * radius; // D <-> U : L <-> R
                            textY += (Parent.Style.Orientation == DiagramOrientation.LeftRight) ? -1.0 * radius : textSize * 0.5; // L <-> R : U <-> D
                            break;
                        case DiagramHorizontalAlignment.Center:
                            textX += (Parent.Style.Orientation == DiagramOrientation.LeftRight) ? -1.0 * textSize * 0.5 : 0; // D <-> U : L <-> R
                            textY += (Parent.Style.Orientation == DiagramOrientation.LeftRight) ? 0 : textSize * 0.5; // L <-> R : U <-> D
                            break;
                        case DiagramHorizontalAlignment.Right:
                            textX += (Parent.Style.Orientation == DiagramOrientation.LeftRight) ? -1.0 * textSize * 0.5 : radius; // D <-> U : L <-> R
                            textY += (Parent.Style.Orientation == DiagramOrientation.LeftRight) ? radius : textSize * 0.5; // L <-> R : U <-> D
                            break;
                    }

                    string textStyle = Style.GetSvgStyle(DiagramMark._textStyleMap, prefix);
                    textStyle += string.Format(CultureInfo.InvariantCulture, "font-size:{0}pt;", textSize);

                    string textFormat = (Parent.Style.Orientation == DiagramOrientation.LeftRight) ? SvgConstants.ROTATED_TEXT : SvgConstants.TEXT;

                    svg += string.Format(CultureInfo.InvariantCulture, textFormat, textStyle, textX, textY, Text);
                }
            }

            return svg;
        }

        public override string ToXaml()
        {
            throw new NotImplementedException();
        }

        private static string[][] _shapeStyleMap =
        {
            new string[] {"mark.color", "fill"},
            new string[] {"mark.opacity", "fill-opacity"},
        };

        private static string[][] _shapeStyleMapBorder =
        {
            new string[] {"mark.bordercolor", "stroke"},
            new string[] {"mark.borderthickness", "stroke-width"},
        };

        private static string[][] _textStyleMap =
        {
            new string[] {"mark.textcolor", "fill"},
            new string[] {"mark.textopacity", "opacity"},
            new string[] {"mark.fontfamily", "font-family"},
            new string[] {"mark.textalignment", "text-anchor"},
            new string[] {"mark.textstyle", ""},
        };

        private const double XThicknessAngle = 5.7 * (Math.PI / 180.0);
    }

    public enum DiagramMarkType
    {
        Normal,
        Muted,
        Root,
        Open,
        OpenRoot,
        Bottom
    }
}
