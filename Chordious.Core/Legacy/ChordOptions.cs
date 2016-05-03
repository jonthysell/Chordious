// 
// ChordOptions.cs
//  
// Author:
//       Jon Thysell <thysell@gmail.com>
// 
// Copyright (c) 2013, 2015, 2016 Jon Thysell <http://jonthysell.com>
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

namespace com.jonthysell.Chordious.Core.Legacy
{
    internal class ChordOptions
    {
        public double Width
        {
            get
            {
                return this._width;
            }
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentOutOfRangeException();
                }
                this._width = value;
            }
        }
        private double _width;

        public double Height
        {
            get
            {
                return this._height;
            }
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentOutOfRangeException();
                }
                this._height = value;
            }
        }
        private double _height;

        public double StrokeWidth
        {
            get
            {
                return this._strokeWidth;
            }
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentOutOfRangeException();
                }
                this._strokeWidth = value;
            }
        }
        private double _strokeWidth;

        public double Margin
        {
            get
            {
                return this._margin;
            }
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentOutOfRangeException();
                }
                this._margin = value;
            }
        }
        private double _margin;

        public double FontSize
        {
            get
            {
                return this._fontSize;
            }
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentOutOfRangeException();
                }
                this._fontSize = value;
            }
        }
        private double _fontSize;

        public string FontFamily
        {
            get
            {
                return this._fontFamily;
            }
            set
            {
                if (String.IsNullOrEmpty(value))
                {
                    throw new ArgumentNullException();
                }
                this._fontFamily = value;
            }
        }
        private string _fontFamily;

        public FontStyle FontStyle { get; set; }

        public BarreType BarreType { get; set; }

        public OpenStringType OpenStringType { get; set; }

        public bool FullBarres { get; set; }

        public ChordOptions() : this(DefaultWidth, DefaultHeight, DefaultStrokeWidth, DefaultMargin, DefaultFontSize, DefaultFontFamily, DefaultFontStyle, DefaultBarreType, DefaultOpenStringType, DefaultFullBarres) {}

        public ChordOptions(double width, double height, double strokeWidth, double margin, double fontSize, string fontFamily, FontStyle fontStyle, BarreType barreType, OpenStringType openStringType, bool fullBarres)
        {
            this.Width = width;
            this.Height = height;
            this.StrokeWidth = strokeWidth;
            this.Margin = margin;
            this.FontSize = fontSize;
            this.FontFamily = fontFamily;
            this.FontStyle = fontStyle;
            this.BarreType = barreType;
            this.OpenStringType = openStringType;
            this.FullBarres = fullBarres;
        }

        public ChordOptions(string optionLine) : this()
        {
            if (String.IsNullOrEmpty(optionLine))
            {
                throw new ArgumentNullException("optionLine");
            }

            string[] s = optionLine.TrimStart(OptionsPrefix[0]).Trim().Split(';');

            try
            {
                this.Width = Double.Parse(s[0]);
                this.Height = Double.Parse(s[1]);
                this.StrokeWidth = Double.Parse(s[2]);
                this.Margin = Double.Parse(s[3]);
                this.FontSize = Double.Parse(s[4]);
                this.FontFamily = s[5];
                this.FontStyle = (FontStyle)Enum.Parse(typeof(FontStyle), s[6]);
                this.BarreType = (BarreType)Enum.Parse(typeof(BarreType), s[7]);
                this.OpenStringType = (OpenStringType)Enum.Parse(typeof(OpenStringType), s[8]);
                this.FullBarres = Boolean.Parse(s[9]);
            }
            catch (IndexOutOfRangeException) { }
        }

        public void CopyTo(ChordOptions co)
        {
            if (null == co)
            {
                throw new ArgumentNullException("co");
            }

            co.Width = this.Width;
            co.Height = this.Height;
            co.StrokeWidth = this.StrokeWidth;
            co.Margin = this.Margin;
            co.FontSize = this.FontSize;
            co.FontFamily = this.FontFamily;
            co.FontStyle = this.FontStyle;
            co.BarreType = this.BarreType;
            co.OpenStringType = this.OpenStringType;
            co.FullBarres = this.FullBarres;
        }

        public override string ToString()
        {
            string s = "";

            s += OptionsPrefix;
            s += String.Format("{0};{1};{2};{3};{4};{5};{6};{7};{8};{9}",
                               Width, Height, StrokeWidth, Margin, FontSize, FontFamily, FontStyle, BarreType, OpenStringType, FullBarres);

            return s;
        }

        public const string OptionsPrefix = "%";
        public const int DefaultWidth = 171;
        public const int DefaultHeight = 258;
        public const int DefaultStrokeWidth = 2;
        public const int DefaultMargin = 30;
        public const int DefaultFontSize = 48;
        public const string DefaultFontFamily = "serif";
        public const FontStyle DefaultFontStyle = FontStyle.Regular;
        public const BarreType DefaultBarreType = BarreType.None;
        public const OpenStringType DefaultOpenStringType = OpenStringType.None;
        public const bool DefaultFullBarres = false;
    }

    internal enum FontStyle
    {
        Regular,
        Bold,
        Italic,
        BoldItalic
    }

    internal enum BarreType
    {
        None,
        Arc,
        Straight
    }

    internal enum OpenStringType
    {
        None,
        Circle
    }
}