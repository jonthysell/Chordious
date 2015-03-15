// 
// DiagramFretLabel.cs
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

        #region Style

        public DiagramTextStyle TextStyle
        {
            get
            {
                return Style.FretLabelTextStyleGet();
            }
            set
            {
                Style.FretLabelTextStyleSet(value);
            }
        }

        public DiagramHorizontalAlignment TextAlignment
        {
            get
            {
                return Style.FretLabelTextAlignmentGet();
            }
            set
            {
                Style.FretLabelTextAlignmentSet(value);
            }
        }

        public double TextSizeRatio
        {
            get
            {
                return Style.FretLabelTextSizeRatioGet();
            }
            set
            {
                Style.FretLabelTextSizeRatioSet(value);
            }
        }

        public bool TextVisible
        {
            get
            {
                return Style.FretLabelTextVisibleGet();
            }
            set
            {
                Style.FretLabelTextVisibleSet(value);
            }
        }

        public double GridPadding
        {
            get
            {
                return Style.FretLabelGridPaddingGet();
            }
            set
            {
                Style.FretLabelGridPaddingSet(value);
            }
        }

        public double TextWidthRatio
        {
            get
            {
                return Style.FretLabelTextWidthRatioGet();
            }
            set
            {
                Style.FretLabelTextWidthRatioSet(value);
            }
        }

        #endregion

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
            return this.TextVisible && !String.IsNullOrEmpty(this.Text);
        }

        public double GetTextHeight()
        {
            return this.Parent.GridFretSpacing * this.TextSizeRatio;
        }

        public double GetTextWidth()
        {
            return GetTextHeight() * this.TextWidthRatio * this.Text.Length;
        }

        public override string ToSvg()
        {
            string svg = "";

            // Draw text
            if (IsVisible())
            {
                double centerY = this.Parent.GridTopEdge() + (this.Parent.GridFretSpacing / 2.0) + (this.Parent.GridFretSpacing * (this.Position.Fret - 1));
                double maxWidth = this.Parent.MaxFretLabelWidth(this.Position.Side);

                double leftGridEdge = this.Parent.GridLeftEdge();
                double rightGridEdge = this.Parent.GridRightEdge();

                double textSize = GetTextHeight();

                double textX = 0.0;
                double textY = centerY + (textSize / 2.0);
                string textStyle = this.Style.GetSvgStyle(DiagramFretLabel._textStyleMap);
                textStyle += String.Format("font-size:{0}pt;", textSize);

                if (this.Position.Side == FretLabelSide.Left)
                {
                    switch (this.TextAlignment)
                    {
                        case DiagramHorizontalAlignment.Left:
                            textX = leftGridEdge - (this.GridPadding + maxWidth);
                            break;
                        case DiagramHorizontalAlignment.Center:
                            textX = leftGridEdge - (this.GridPadding + (maxWidth / 2.0));
                            break;
                        case DiagramHorizontalAlignment.Right:
                            textX = leftGridEdge - this.GridPadding;
                            break;
                    }
                }
                else if (this.Position.Side == FretLabelSide.Right)
                {
                    switch (this.TextAlignment)
                    {
                        case DiagramHorizontalAlignment.Left:
                            textX = rightGridEdge + this.GridPadding;
                            break;
                        case DiagramHorizontalAlignment.Center:
                            textX = rightGridEdge + (this.GridPadding + (maxWidth / 2.0));
                            break;
                        case DiagramHorizontalAlignment.Right:
                            textX = rightGridEdge + (this.GridPadding + maxWidth);
                            break;
                    }
                }

                string textFormat = SvgConstants.TEXT;

                if (Parent.Orientation == DiagramOrientation.LeftRight)
                {
                    textFormat = SvgConstants.ROTATED_TEXT;
                    textX -= textSize / 2.0;
                    textY -= textSize / 2.0;
                }

                svg += String.Format(textFormat, textStyle, textX, textY, this.Text);
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
