// 
// DiagramFretLabel.cs
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

namespace com.jonthysell.Chordious.Core
{
    public class DiagramFretLabel : DiagramElement
    {
        public new FretLabelPosition Position
        {
            get
            {
                return (FretLabelPosition)base.Position;
            }
            set
            {
                base.Position = value;
            }
        }

        public DiagramFretLabel(Diagram parent, FretLabelPosition position, string text) : base(parent, position, text) { }

        public DiagramFretLabel(Diagram parent, XmlReader xmlReader) : base(parent)
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
                if (xmlReader.IsStartElement() && xmlReader.Name == "fretlabel")
                {
                    Text = xmlReader.GetAttribute("text");

                    FretLabelSide side = (FretLabelSide)Enum.Parse(typeof(FretLabelSide), xmlReader.GetAttribute("side"));
                    int fret = int.Parse(xmlReader.GetAttribute("fret"));

                    Position = new FretLabelPosition(side, fret);

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

            xmlWriter.WriteStartElement("fretlabel");

            xmlWriter.WriteAttributeString("text", Text);
            xmlWriter.WriteAttributeString("side", Position.Side.ToString());
            xmlWriter.WriteAttributeString("fret", Position.Fret.ToString());

            Style.Write(xmlWriter, "fretlabel");

            xmlWriter.WriteEndElement();
        }

        public override bool IsVisible()
        {
            return Style.FretLabelTextVisible && !string.IsNullOrEmpty(Text);
        }

        public double GetTextHeight()
        {
            return Style.FretLabelTextSizeRatio * Math.Min(Parent.Style.GridStringSpacing, Parent.Style.GridFretSpacing);
        }

        public double GetTextWidth()
        {
            return GetTextHeight() * Style.FretLabelTextWidthRatio * Text.Length;
        }

        public override string ToSvg()
        {
            string svg = "";

            // Draw text
            if (IsVisible())
            {
                double centerY = Parent.GridTopEdge() + (Parent.Style.GridFretSpacing * 0.5) + (Parent.Style.GridFretSpacing * (Position.Fret - 1));
                double maxWidth = Parent.MaxFretLabelWidth(Position.Side);

                double leftGridEdge = Parent.GridLeftEdge();
                double rightGridEdge = Parent.GridRightEdge();

                double textSize = GetTextHeight();

                double textX = (Position.Side == FretLabelSide.Left) ? leftGridEdge : rightGridEdge;
                double textY = centerY;

                if (Position.Side == FretLabelSide.Left)
                {
                    switch (Style.FretLabelTextAlignment)
                    {
                        case DiagramHorizontalAlignment.Left:
                            textX += (Parent.Style.Orientation == DiagramOrientation.LeftRight) ? -1.0 * (Style.FretLabelGridPadding + textSize) : -1.0 * (Style.FretLabelGridPadding + maxWidth); // D <-> U : L <-> R
                            textY += (Parent.Style.Orientation == DiagramOrientation.LeftRight) ? -1.0 * (Parent.Style.GridFretSpacing * 0.5) : textSize * 0.5; // L <-> R : U <-> D
                            break;
                        case DiagramHorizontalAlignment.Center:
                            textX += (Parent.Style.Orientation == DiagramOrientation.LeftRight) ? -1.0 * (Style.FretLabelGridPadding + textSize) : -1.0 * (Style.FretLabelGridPadding + (maxWidth * 0.5)); // D <-> U : L <-> R
                            textY += (Parent.Style.Orientation == DiagramOrientation.LeftRight) ? 0 : textSize * 0.5; // L <-> R : U <-> D
                            break;
                        case DiagramHorizontalAlignment.Right:
                            textX += (Parent.Style.Orientation == DiagramOrientation.LeftRight) ? -1.0 * (Style.FretLabelGridPadding + textSize) : -1.0 * Style.FretLabelGridPadding; // D <-> U : L <-> R
                            textY += (Parent.Style.Orientation == DiagramOrientation.LeftRight) ? Parent.Style.GridFretSpacing * 0.5 : textSize * 0.5; // L <-> R : U <-> D
                            break;
                    }
                }
                else if (Position.Side == FretLabelSide.Right)
                {
                    switch (Style.FretLabelTextAlignment)
                    {
                        case DiagramHorizontalAlignment.Left:
                            textX += (Parent.Style.Orientation == DiagramOrientation.LeftRight) ? Style.FretLabelGridPadding : Style.FretLabelGridPadding; // D <-> U : L <-> R
                            textY += (Parent.Style.Orientation == DiagramOrientation.LeftRight) ? -1.0 * (Parent.Style.GridFretSpacing * 0.5) : textSize * 0.5; // L <-> R : U <-> D
                            break;
                        case DiagramHorizontalAlignment.Center:
                            textX += (Parent.Style.Orientation == DiagramOrientation.LeftRight) ? Style.FretLabelGridPadding : Style.FretLabelGridPadding + (maxWidth * 0.5); // D <-> U : L <-> R
                            textY += (Parent.Style.Orientation == DiagramOrientation.LeftRight) ? 0 : textSize * 0.5; // L <-> R : U <-> D
                            break;
                        case DiagramHorizontalAlignment.Right:
                            textX += (Parent.Style.Orientation == DiagramOrientation.LeftRight) ? Style.FretLabelGridPadding : Style.FretLabelGridPadding + maxWidth; // D <-> U : L <-> R
                            textY += (Parent.Style.Orientation == DiagramOrientation.LeftRight) ? Parent.Style.GridFretSpacing * 0.5 : textSize * 0.5; // L <-> R : U <-> D
                            break;
                    }
                }

                string textStyle = Style.GetSvgStyle(DiagramFretLabel._textStyleMap);
                textStyle += string.Format(CultureInfo.InvariantCulture, "font-size:{0}pt;", textSize);

                string textFormat = (Parent.Style.Orientation == DiagramOrientation.LeftRight) ? SvgConstants.ROTATED_TEXT : SvgConstants.TEXT;

                svg += string.Format(CultureInfo.InvariantCulture, textFormat, textStyle, textX, textY, Text);
            }

            return svg;
        }

        private static string[][] _textStyleMap =
        {
            new string[] {"fretlabel.textcolor", "fill"},
            new string[] {"fretlabel.textopacity", "opacity"},
            new string[] {"fretlabel.fontfamily", "font-family"},
            new string[] {"fretlabel.textalignment", "text-anchor"},
            new string[] {"fretlabel.textstyle", ""},
        };

        public override string ToXaml()
        {
            throw new NotImplementedException();
        }
    }
}
