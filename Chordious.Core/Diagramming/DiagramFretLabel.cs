// 
// DiagramFretLabel.cs
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
                if (xmlReader.IsStartElement() && xmlReader.Name == "fretlabel")
                {
                    this.Text = xmlReader.GetAttribute("text");

                    FretLabelSide side = (FretLabelSide)Enum.Parse(typeof(FretLabelSide), xmlReader.GetAttribute("side"));
                    int fret = Int32.Parse(xmlReader.GetAttribute("fret"));

                    this.Position = new FretLabelPosition(side, fret);

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

            xmlWriter.WriteStartElement("fretlabel");

            xmlWriter.WriteAttributeString("text", this.Text);
            xmlWriter.WriteAttributeString("side", this.Position.Side.ToString());
            xmlWriter.WriteAttributeString("fret", this.Position.Fret.ToString());

            this.Style.Write(xmlWriter, "fretlabel");

            xmlWriter.WriteEndElement();
        }

        public override bool IsVisible()
        {
            return this.Style.FretLabelTextVisible && !String.IsNullOrEmpty(this.Text);
        }

        public double GetTextHeight()
        {
            return this.Style.FretLabelTextSizeRatio * Math.Min(this.Parent.Style.GridStringSpacing, this.Parent.Style.GridFretSpacing);
        }

        public double GetTextWidth()
        {
            return GetTextHeight() * this.Style.FretLabelTextWidthRatio * this.Text.Length;
        }

        public override string ToSvg()
        {
            string svg = "";

            // Draw text
            if (IsVisible())
            {
                double centerY = this.Parent.GridTopEdge() + (this.Parent.Style.GridFretSpacing * 0.5) + (this.Parent.Style.GridFretSpacing * (this.Position.Fret - 1));
                double maxWidth = this.Parent.MaxFretLabelWidth(this.Position.Side);

                double leftGridEdge = this.Parent.GridLeftEdge();
                double rightGridEdge = this.Parent.GridRightEdge();

                double textSize = GetTextHeight();

                double textX = (this.Position.Side == FretLabelSide.Left) ? leftGridEdge : rightGridEdge;
                double textY = centerY;

                if (this.Position.Side == FretLabelSide.Left)
                {
                    switch (this.Style.FretLabelTextAlignment)
                    {
                        case DiagramHorizontalAlignment.Left:
                            textX += (this.Parent.Style.Orientation == DiagramOrientation.LeftRight) ? -1.0 * (this.Style.FretLabelGridPadding + textSize) : -1.0 * (this.Style.FretLabelGridPadding + maxWidth); // D <-> U : L <-> R
                            textY += (this.Parent.Style.Orientation == DiagramOrientation.LeftRight) ? -1.0 * (this.Parent.Style.GridFretSpacing * 0.5) : textSize * 0.5; // L <-> R : U <-> D
                            break;
                        case DiagramHorizontalAlignment.Center:
                            textX += (this.Parent.Style.Orientation == DiagramOrientation.LeftRight) ? -1.0 * (this.Style.FretLabelGridPadding + textSize) : -1.0 * (this.Style.FretLabelGridPadding + (maxWidth * 0.5)); // D <-> U : L <-> R
                            textY += (this.Parent.Style.Orientation == DiagramOrientation.LeftRight) ? 0 : textSize * 0.5; // L <-> R : U <-> D
                            break;
                        case DiagramHorizontalAlignment.Right:
                            textX += (this.Parent.Style.Orientation == DiagramOrientation.LeftRight) ? -1.0 * (this.Style.FretLabelGridPadding + textSize) : -1.0 * this.Style.FretLabelGridPadding; // D <-> U : L <-> R
                            textY += (this.Parent.Style.Orientation == DiagramOrientation.LeftRight) ? this.Parent.Style.GridFretSpacing * 0.5 : textSize * 0.5; // L <-> R : U <-> D
                            break;
                    }
                }
                else if (this.Position.Side == FretLabelSide.Right)
                {
                    switch (this.Style.FretLabelTextAlignment)
                    {
                        case DiagramHorizontalAlignment.Left:
                            textX += (this.Parent.Style.Orientation == DiagramOrientation.LeftRight) ? this.Style.FretLabelGridPadding : this.Style.FretLabelGridPadding; // D <-> U : L <-> R
                            textY += (this.Parent.Style.Orientation == DiagramOrientation.LeftRight) ? -1.0 * (this.Parent.Style.GridFretSpacing * 0.5) : textSize * 0.5; // L <-> R : U <-> D
                            break;
                        case DiagramHorizontalAlignment.Center:
                            textX += (this.Parent.Style.Orientation == DiagramOrientation.LeftRight) ? this.Style.FretLabelGridPadding : this.Style.FretLabelGridPadding + (maxWidth * 0.5); // D <-> U : L <-> R
                            textY += (this.Parent.Style.Orientation == DiagramOrientation.LeftRight) ? 0 : textSize * 0.5; // L <-> R : U <-> D
                            break;
                        case DiagramHorizontalAlignment.Right:
                            textX += (this.Parent.Style.Orientation == DiagramOrientation.LeftRight) ? this.Style.FretLabelGridPadding : this.Style.FretLabelGridPadding + maxWidth; // D <-> U : L <-> R
                            textY += (this.Parent.Style.Orientation == DiagramOrientation.LeftRight) ? this.Parent.Style.GridFretSpacing * 0.5 : textSize * 0.5; // L <-> R : U <-> D
                            break;
                    }
                }

                string textStyle = this.Style.GetSvgStyle(DiagramFretLabel._textStyleMap);
                textStyle += String.Format(CultureInfo.InvariantCulture, "font-size:{0}pt;", textSize);

                string textFormat = (this.Parent.Style.Orientation == DiagramOrientation.LeftRight) ? SvgConstants.ROTATED_TEXT : SvgConstants.TEXT;

                svg += String.Format(CultureInfo.InvariantCulture, textFormat, textStyle, textX, textY, this.Text);
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
