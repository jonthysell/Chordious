// 
// DiagramMarkStyleWrapper.cs
//  
// Author:
//       Jon Thysell <thysell@gmail.com>
// 
// Copyright (c) 2016 Jon Thysell <http://jonthysell.com>
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

namespace com.jonthysell.Chordious.Core
{
    public class DiagramMarkStyleWrapper
    {
        public DiagramMarkType MarkType { get; set; }

        public DiagramMarkShape MarkShape
        {
            get
            {
                return Style.MarkStyleGetEnum<DiagramMarkShape>("mark.shape", this.MarkType);
            }
            set
            {
                Style.MarkStyleSet("mark.shape", value, this.MarkType);
            }
        }

        public bool MarkVisible
        {
            get
            {
                return Style.MarkStyleGetBoolean("mark.visible", this.MarkType);
            }
            set
            {
                Style.MarkStyleSet("mark.visible", value, this.MarkType);
            }
        }

        public string MarkColor
        {
            get
            {
                return Style.MarkStyleGetColor("mark.color", this.MarkType);
            }
            set
            {
                Style.MarkStyleSetColor("mark.color", value, this.MarkType);
            }
        }

        public double MarkOpacity
        {
            get
            {
                return Style.MarkStyleGetDouble("mark.opacity", this.MarkType);
            }
            set
            {
                if (value < 0 || value > 1.0)
                {
                    throw new ArgumentOutOfRangeException();
                }

                Style.MarkStyleSet("mark.opacity", value, this.MarkType);
            }
        }

        public DiagramTextStyle MarkTextStyle
        {
            get
            {
                return Style.MarkStyleGetEnum<DiagramTextStyle>("mark.textstyle", this.MarkType);
            }
            set
            {
                Style.MarkStyleSet("mark.textstyle", value, this.MarkType);
            }
        }

        public DiagramHorizontalAlignment MarkTextAlignment
        {
            get
            {
                return Style.MarkStyleGetEnum<DiagramHorizontalAlignment>("mark.textalignment", this.MarkType);
            }
            set
            {
                Style.MarkStyleSet("mark.textalignment", value, this.MarkType);
            }
        }

        public double MarkTextSizeRatio
        {
            get
            {
                return Style.MarkStyleGetDouble("mark.textsizeratio", this.MarkType);
            }
            set
            {
                if (value < 0 || value > 1.0)
                {
                    throw new ArgumentOutOfRangeException();
                }

                Style.MarkStyleSet("mark.textsizeratio", value, this.MarkType);
            }
        }

        public bool MarkTextVisible
        {
            get
            {
                return Style.MarkStyleGetBoolean("mark.textvisible", this.MarkType);
            }
            set
            {
                Style.MarkStyleSet("mark.textvisible", value, this.MarkType);
            }
        }

        public string MarkTextColor
        {
            get
            {
                return Style.MarkStyleGetColor("mark.textcolor", this.MarkType);
            }
            set
            {
                Style.MarkStyleSetColor("mark.textcolor", value, this.MarkType);
            }
        }

        public double MarkTextOpacity
        {
            get
            {
                return Style.MarkStyleGetDouble("mark.textopacity", this.MarkType);
            }
            set
            {
                if (value < 0 || value > 1.0)
                {
                    throw new ArgumentOutOfRangeException();
                }

                Style.MarkStyleSet("mark.textopacity", value, this.MarkType);
            }
        }

        public string MarkFontFamily
        {
            get
            {
                return Style.MarkStyleGet("mark.fontfamily", this.MarkType);
            }
            set
            {
                Style.MarkStyleSet("mark.fontfamily", value, this.MarkType);
            }
        }

        public double MarkRadiusRatio
        {
            get
            {
                return Style.MarkStyleGetDouble("mark.radiusratio", this.MarkType);
            }
            set
            {
                if (value < 0 || value > 1.0)
                {
                    throw new ArgumentOutOfRangeException();
                }

                Style.MarkStyleSet("mark.radiusratio", value, this.MarkType);
            }
        }

        public string MarkBorderColor
        {
            get
            {
                return Style.MarkStyleGetColor("mark.bordercolor", this.MarkType);
            }
            set
            {
                Style.MarkStyleSetColor("mark.bordercolor", value, this.MarkType);
            }
        }

        public double MarkBorderThickness
        {
            get
            {
                return Style.MarkStyleGetDouble("mark.borderthickness", this.MarkType);
            }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException();
                }

                Style.MarkStyleSet("mark.borderthickness", value, this.MarkType);
            }
        }

        public DiagramStyle Style { get; private set; }

        public DiagramMarkStyleWrapper(DiagramStyle style, DiagramMarkType markType = DiagramMarkType.Normal)
        {
            if (null == style)
            {
                throw new ArgumentNullException("style");
            }

            Style = style;
            MarkType = markType;
        }
    }
}
