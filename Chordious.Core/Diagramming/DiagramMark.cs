// 
// DiagramMark.cs
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
using System.Collections.Generic;
using System.Globalization;
using System.Text;
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
                return this.MarkStyle.MarkType;
            }
            set
            {
                this.MarkStyle.MarkType = value;
            }
        }

        public DiagramMarkStyleWrapper MarkStyle { get; private set; }

        public DiagramMark(Diagram parent, MarkPosition position, string text = "") : base(parent, position, text)
        {
            this.MarkStyle = new DiagramMarkStyleWrapper(this.Style);
        }

        public DiagramMark(Diagram parent, XmlReader xmlReader) : base(parent)
        {
            this.MarkStyle = new DiagramMarkStyleWrapper(this.Style);
            this.Read(xmlReader);
        }

        public override bool IsVisible()
        {
            return this.MarkStyle.MarkVisible;
        }

        public bool IsAboveTopEdge()
        {
            return (this.Position.Fret == 0);
        }

        public bool IsBelowBottomEdge()
        {
            return (this.Position.Fret == this.Parent.NumFrets + 1);
        }

        public override sealed void Read(XmlReader xmlReader)
        {
            if (null == xmlReader)
            {
                throw new ArgumentNullException("xmlReader");
            }

            using (xmlReader)
            {
                if (xmlReader.IsStartElement() && xmlReader.Name == "mark")
                {
                    this.Text = xmlReader.GetAttribute("text");
                    this.Type = (DiagramMarkType)Enum.Parse(typeof(DiagramMarkType), xmlReader.GetAttribute("type"));

                    int @string = Int32.Parse(xmlReader.GetAttribute("string"));
                    int fret = Int32.Parse(xmlReader.GetAttribute("fret"));

                    this.Position = new MarkPosition(@string, fret);

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

            xmlWriter.WriteStartElement("mark");

            xmlWriter.WriteAttributeString("text", this.Text);
            xmlWriter.WriteAttributeString("type", this.Type.ToString());
            xmlWriter.WriteAttributeString("string", this.Position.String.ToString());
            xmlWriter.WriteAttributeString("fret", this.Position.Fret.ToString());

            string prefix = DiagramStyle.GetMarkStylePrefix(this.Type);

            // Only write the styles that apply to the type of this mark
            this.Style.Write(xmlWriter, prefix + "mark");

            xmlWriter.WriteEndElement();
        }

        public override string ToSvg()
        {
            string svg = "";

            if (this.IsVisible())
            {
                string prefix = DiagramStyle.GetMarkStylePrefix(this.Type);

                DiagramMarkShape shape = this.MarkStyle.MarkShape;
                double radius = this.MarkStyle.MarkRadiusRatio * 0.5 * Math.Min(this.Parent.Style.GridStringSpacing, this.Parent.Style.GridFretSpacing);

                double centerX = this.Parent.GridLeftEdge() + (this.Parent.Style.GridStringSpacing * (this.Position.String - 1));
                double centerY = this.Parent.GridTopEdge() + (this.Parent.Style.GridFretSpacing / 2.0) + (this.Parent.Style.GridFretSpacing * (this.Position.Fret - 1));

                // Draw shape

                string shapeStyle = this.Style.GetSvgStyle(DiagramMark._shapeStyleMap, prefix);

                if (this.MarkStyle.MarkBorderThickness > 0)
                {
                    shapeStyle += this.Style.GetSvgStyle(DiagramMark._shapeStyleMapBorder, prefix);
                }

                switch (shape)
                {
                    case DiagramMarkShape.Circle:
                        svg += String.Format(CultureInfo.InvariantCulture, SvgConstants.CIRCLE, shapeStyle, radius, centerX, centerY);
                        break;
                    case DiagramMarkShape.Square:
                        svg += String.Format(CultureInfo.InvariantCulture, SvgConstants.RECTANGLE, shapeStyle, radius * 2.0, radius * 2.0, centerX - radius, centerY - radius);
                        break;
                    case DiagramMarkShape.Diamond:
                        string diamondPoints = "";
                        diamondPoints += String.Format(CultureInfo.InvariantCulture, "{0},{1} ", centerX, centerY - radius);
                        diamondPoints += String.Format(CultureInfo.InvariantCulture, "{0},{1} ", centerX + radius, centerY);
                        diamondPoints += String.Format(CultureInfo.InvariantCulture, "{0},{1} ", centerX, centerY + radius);
                        diamondPoints += String.Format(CultureInfo.InvariantCulture, "{0},{1}", centerX - radius, centerY);
                        svg += String.Format(CultureInfo.InvariantCulture, SvgConstants.POLYGON, shapeStyle, diamondPoints);
                        break;
                    case DiagramMarkShape.X:
                        double xDelta = Math.Sqrt((radius * radius / 2.0));
                        svg += String.Format(CultureInfo.InvariantCulture, SvgConstants.LINE, shapeStyle, centerX - xDelta, centerY - xDelta, centerX + xDelta, centerY + xDelta);
                        svg += String.Format(CultureInfo.InvariantCulture, SvgConstants.LINE, shapeStyle, centerX - xDelta, centerY + xDelta, centerX + xDelta, centerY - xDelta);
                        break;
                    case DiagramMarkShape.None:
                    default:
                        break;
                }

                // Draw text
                if (!StringUtils.IsNullOrWhiteSpace(this.Text) && this.MarkStyle.MarkTextVisible)
                {
                    double textSize = radius * 2.0 * this.MarkStyle.MarkTextSizeRatio;

                    double textX = centerX;
                    double textY = centerY;

                    switch (this.MarkStyle.MarkTextAlignment)
                    {
                        case DiagramHorizontalAlignment.Left:
                            textX += (this.Parent.Style.Orientation == DiagramOrientation.LeftRight) ? -1.0 * textSize * 0.5 : -1.0 * radius; // D <-> U : L <-> R
                            textY += (this.Parent.Style.Orientation == DiagramOrientation.LeftRight) ? -1.0 * radius : textSize * 0.5; // L <-> R : U <-> D
                            break;
                        case DiagramHorizontalAlignment.Center:
                            textX += (this.Parent.Style.Orientation == DiagramOrientation.LeftRight) ? -1.0 * textSize * 0.5 : 0; // D <-> U : L <-> R
                            textY += (this.Parent.Style.Orientation == DiagramOrientation.LeftRight) ? 0 : textSize * 0.5; // L <-> R : U <-> D
                            break;
                        case DiagramHorizontalAlignment.Right:
                            textX += (this.Parent.Style.Orientation == DiagramOrientation.LeftRight) ? -1.0 * textSize * 0.5 : radius; // D <-> U : L <-> R
                            textY += (this.Parent.Style.Orientation == DiagramOrientation.LeftRight) ? radius : textSize * 0.5; // L <-> R : U <-> D
                            break;
                    }

                    string textStyle = this.Style.GetSvgStyle(DiagramMark._textStyleMap, prefix);
                    textStyle += String.Format(CultureInfo.InvariantCulture, "font-size:{0}pt;", textSize);

                    string textFormat = (this.Parent.Style.Orientation == DiagramOrientation.LeftRight) ? SvgConstants.ROTATED_TEXT : SvgConstants.TEXT;

                    svg += String.Format(CultureInfo.InvariantCulture, textFormat, textStyle, textX, textY, this.Text);
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
