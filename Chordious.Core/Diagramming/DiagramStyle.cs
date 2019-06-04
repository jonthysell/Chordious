// 
// DiagramStyle.cs
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

using Chordious.Core.Resources;

namespace Chordious.Core
{
    public class DiagramStyle : InheritableDictionary
    {
        public new DiagramStyle Parent
        {
            get
            {
                return (DiagramStyle)base.Parent;
            }
            internal set
            {
                base.Parent = value;
            }
        }

        #region New Diagram-specific Styles

        public bool NewDiagramNumStringsIsLocal
        {
            get
            {
                return IsLocalGet("newdiagram.numstrings");
            }
            set
            {
                IsLocalSet("newdiagram.numstrings", value);
            }
        }

        public int NewDiagramNumStrings
        {
            get
            {
                return GetInt32("newdiagram.numstrings", 2);
            }
            set
            {
                if (value < 2)
                {
                    throw new ArgumentOutOfRangeException();
                }

                Set("newdiagram.numstrings", value);
            }
        }

        public bool NewDiagramNumFretsIsLocal
        {
            get
            {
                return IsLocalGet("newdiagram.numfrets");
            }
            set
            {
                IsLocalSet("newdiagram.numfrets", value);
            }
        }

        public int NewDiagramNumFrets
        {
            get
            {
                return GetInt32("newdiagram.numfrets", 1);
            }
            set
            {
                if (value < 1)
                {
                    throw new ArgumentOutOfRangeException();
                }

                Set("newdiagram.numfrets", value);
            }
        }

        #endregion

        #region Diagram-specific Styles

        public bool OrientationIsLocal
        {
            get
            {
                return IsLocalGet("diagram.orientation");
            }
            set
            {
                IsLocalSet("diagram.orientation", value);
            }
        }

        public DiagramOrientation Orientation
        {
            get
            {
                return GetEnum<DiagramOrientation>("diagram.orientation");
            }
            set
            {
                Set("diagram.orientation", value);
            }
        }

        public bool LabelLayoutModelIsLocal
        {
            get
            {
                return IsLocalGet("diagram.labellayoutmodel");
            }
            set
            {
                IsLocalSet("diagram.labellayoutmodel", value);
            }
        }

        public DiagramLabelLayoutModel LabelLayoutModel
        {
            get
            {
                return GetEnum<DiagramLabelLayoutModel>("diagram.labellayoutmodel");
            }
            set
            {
                Set("diagram.labellayoutmodel", value);
            }
        }

        public bool DiagramColorIsLocal
        {
            get
            {
                return IsLocalGet("diagram.color");
            }
            set
            {
                IsLocalSet("diagram.color", value);
            }
        }

        public string DiagramColor
        {
            get
            {
                return GetColor("diagram.color");
            }
            set
            {
                SetColor("diagram.color", value);
            }
        }

        public bool DiagramOpacityIsLocal
        {
            get
            {
                return IsLocalGet("diagram.opacity");
            }
            set
            {
                IsLocalSet("diagram.opacity", value);
            }
        }

        public double DiagramOpacity
        {
            get
            {
                return GetDouble("diagram.opacity");
            }
            set
            {
                if (value < 0 || value > 1.0)
                {
                    throw new ArgumentOutOfRangeException();
                }

                Set("diagram.opacity", value);
            }
        }

        public bool DiagramBorderColorIsLocal
        {
            get
            {
                return IsLocalGet("diagram.bordercolor");
            }
            set
            {
                IsLocalSet("diagram.bordercolor", value);
            }
        }

        public string DiagramBorderColor
        {
            get
            {
                return GetColor("diagram.bordercolor");
            }
            set
            {
                SetColor("diagram.bordercolor", value);
            }
        }

        public bool DiagramBorderThicknessIsLocal
        {
            get
            {
                return IsLocalGet("diagram.borderthickness");
            }
            set
            {
                IsLocalSet("diagram.borderthickness", value);
            }
        }

        public double DiagramBorderThickness
        {
            get
            {
                return GetDouble("diagram.borderthickness");
            }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException();
                }

                Set("diagram.borderthickness", value);
            }
        }

        #endregion

        #region Grid-specific Styles

        public bool GridMarginIsLocal
        {
            get
            {
                return IsLocalGet("grid.margin");
            }
            set
            {
                IsLocalSet("grid.margin", value);
            }
        }

        public double GridMargin
        {
            get
            {
                return GetDouble("grid.margin", 0);
            }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException();
                }

                Set("grid.margin", value);
            }
        }

        public bool GridMarginLeftOverride
        {
            get
            {
                return HasKey("grid.marginleft");
            }
            set
            {
                if (value && !GridMarginLeftOverride)
                {
                    GridMarginLeft = GridMargin;
                }
                else if (!value)
                {
                    Clear("grid.marginleft");
                }
            }
        }

        public bool GridMarginLeftIsLocal
        {
            get
            {
                return IsLocalGet("grid.marginleft");
            }
            set
            {
                IsLocalSet("grid.marginleft", value, GridMargin);
            }
        }

        public double GridMarginLeft
        {
            get
            {
                if (TryGet("grid.marginleft", out double margin))
                {
                    return margin;
                }

                return GridMargin;
            }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException();
                }

                Set("grid.marginleft", value);
            }
        }

        public bool GridMarginRightOverride
        {
            get
            {
                return HasKey("grid.marginright");
            }
            set
            {
                if (value && !GridMarginRightOverride)
                {
                    GridMarginRight = GridMargin;
                }
                else if (!value)
                {
                    Clear("grid.marginright");
                }
            }
        }

        public bool GridMarginRightIsLocal
        {
            get
            {
                return IsLocalGet("grid.marginright");
            }
            set
            {
                IsLocalSet("grid.marginright", value, GridMargin);
            }
        }

        public double GridMarginRight
        {
            get
            {
                if (TryGet("grid.marginright", out double margin))
                {
                    return margin;
                }

                return GridMargin;
            }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException();
                }

                Set("grid.marginright", value);
            }
        }

        public bool GridMarginTopOverride
        {
            get
            {
                return HasKey("grid.margintop");
            }
            set
            {
                if (value && !GridMarginTopOverride)
                {
                    GridMarginTop = GridMargin;
                }
                else if (!value)
                {
                    Clear("grid.margintop");
                }
            }
        }

        public bool GridMarginTopIsLocal
        {
            get
            {
                return IsLocalGet("grid.margintop");
            }
            set
            {
                IsLocalSet("grid.margintop", value, GridMargin);
            }
        }

        public double GridMarginTop
        {
            get
            {
                if (TryGet("grid.margintop", out double margin))
                {
                    return margin;
                }

                return GridMargin;
            }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException();
                }

                Set("grid.margintop", value);
            }
        }

        public bool GridMarginBottomOverride
        {
            get
            {
                return HasKey("grid.marginbottom");
            }
            set
            {
                if (value && !GridMarginBottomOverride)
                {
                    GridMarginBottom = GridMargin;
                }
                else if (!value)
                {
                    Clear("grid.marginbottom");
                }
            }
        }

        public bool GridMarginBottomIsLocal
        {
            get
            {
                return IsLocalGet("grid.marginbottom");
            }
            set
            {
                IsLocalSet("grid.marginbottom", value, GridMargin);
            }
        }

        public double GridMarginBottom
        {
            get
            {
                if (TryGet("grid.marginbottom", out double margin))
                {
                    return margin;
                }

                return GridMargin;
            }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException();
                }

                Set("grid.marginbottom", value);
            }
        }

        public bool GridFretSpacingIsLocal
        {
            get
            {
                return IsLocalGet("grid.fretspacing");
            }
            set
            {
                IsLocalSet("grid.fretspacing", value);
            }
        }

        public double GridFretSpacing
        {
            get
            {
                return GetDouble("grid.fretspacing");
            }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException();
                }

                Set("grid.fretspacing", value);
            }
        }

        public bool GridStringSpacingIsLocal
        {
            get
            {
                return IsLocalGet("grid.stringspacing");
            }
            set
            {
                IsLocalSet("grid.stringspacing", value);
            }
        }

        public double GridStringSpacing
        {
            get
            {
                return GetDouble("grid.stringspacing");
            }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException();
                }

                Set("grid.stringspacing", value);
            }
        }

        public bool GridColorIsLocal
        {
            get
            {
                return IsLocalGet("grid.color");
            }
            set
            {
                IsLocalSet("grid.color", value);
            }
        }

        public string GridColor
        {
            get
            {
                return GetColor("grid.color");
            }
            set
            {
                SetColor("grid.color", value);
            }
        }

        public bool GridOpacityIsLocal
        {
            get
            {
                return IsLocalGet("grid.opacity");
            }
            set
            {
                IsLocalSet("grid.opacity", value);
            }
        }

        public double GridOpacity
        {
            get
            {
                return GetDouble("grid.opacity");
            }
            set
            {
                if (value < 0 || value > 1.0)
                {
                    throw new ArgumentOutOfRangeException();
                }

                Set("grid.opacity", value);
            }
        }

        public bool GridLineColorIsLocal
        {
            get
            {
                return IsLocalGet("grid.linecolor");
            }
            set
            {
                IsLocalSet("grid.linecolor", value);
            }
        }

        public string GridLineColor
        {
            get
            {
                return GetColor("grid.linecolor");
            }
            set
            {
                SetColor("grid.linecolor", value);
            }
        }

        public bool GridLineThicknessIsLocal
        {
            get
            {
                return IsLocalGet("grid.linethickness");
            }
            set
            {
                IsLocalSet("grid.linethickness", value);
            }
        }

        public double GridLineThickness
        {
            get
            {
                return GetDouble("grid.linethickness");
            }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException();
                }

                Set("grid.linethickness", value);
            }
        }

        public bool GridNutVisibleIsLocal
        {
            get
            {
                return IsLocalGet("grid.nutvisible");
            }
            set
            {
                IsLocalSet("grid.nutvisible", value);
            }
        }

        public bool GridNutVisible
        {
            get
            {
                return GetBoolean("grid.nutvisible");
            }
            set
            {
                Set("grid.nutvisible", value);
            }
        }

        public bool GridNutRatioIsLocal
        {
            get
            {
                return IsLocalGet("grid.nutratio");
            }
            set
            {
                IsLocalSet("grid.nutratio", value);
            }
        }

        public double GridNutRatio
        {
            get
            {
                return GetDouble("grid.nutratio", 1.0);
            }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException();
                }

                Set("grid.nutratio", value);
            }
        }

        #endregion

        #region Title-specific Styles

        public bool TitleGridPaddingIsLocal
        {
            get
            {
                return IsLocalGet("title.gridpadding");
            }
            set
            {
                IsLocalSet("title.gridpadding", value);
            }
        }

        public double TitleGridPadding
        {
            get
            {
                return GetDouble("title.gridpadding", 0);
            }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException();
                }

                Set("title.gridpadding", value);
            }
        }

        public bool TitleTextSizeIsLocal
        {
            get
            {
                return IsLocalGet("title.textsize");
            }
            set
            {
                IsLocalSet("title.textsize", value);
            }
        }

        public double TitleTextSize
        {
            get
            {
                return GetDouble("title.textsize");
            }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException();
                }

                Set("title.textsize", value);
            }
        }

        public bool TitleTextSizeModRatioIsLocal
        {
            get
            {
                return IsLocalGet("title.textmodratio");
            }
            set
            {
                IsLocalSet("title.textmodratio", value);
            }
        }

        public double TitleTextSizeModRatio
        {
            get
            {
                return GetDouble("title.textmodratio", 1.0);
            }
            set
            {
                if (value < 0 || value > 1.0)
                {
                    throw new ArgumentOutOfRangeException();
                }

                Set("title.textmodratio", value);
            }
        }

        public bool TitleFontFamilyIsLocal
        {
            get
            {
                return IsLocalGet("title.fontfamily");
            }
            set
            {
                IsLocalSet("title.fontfamily", value);
            }
        }

        public string TitleFontFamily
        {
            get
            {
                return Get("title.fontfamily");
            }
            set
            {
                Set("title.fontfamily", value);
            }
        }

        public bool TitleTextAlignmentIsLocal
        {
            get
            {
                return IsLocalGet("title.textalignment");
            }
            set
            {
                IsLocalSet("title.textalignment", value);
            }
        }

        public DiagramHorizontalAlignment TitleTextAlignment
        {
            get
            {
                return GetEnum<DiagramHorizontalAlignment>("title.textalignment");
            }
            set
            {
                Set("title.textalignment", value);
            }
        }

        public bool TitleTextStyleIsLocal
        {
            get
            {
                return IsLocalGet("title.textstyle");
            }
            set
            {
                IsLocalSet("title.textstyle", value);
            }
        }

        public DiagramTextStyle TitleTextStyle
        {
            get
            {
                return GetEnum<DiagramTextStyle>("title.textstyle");
            }
            set
            {
                Set("title.textstyle", value);
            }
        }

        public bool TitleVisibleIsLocal
        {
            get
            {
                return IsLocalGet("title.textvisible");
            }
            set
            {
                IsLocalSet("title.textvisible", value);
            }
        }

        public bool TitleVisible
        {
            get
            {
                return GetBoolean("title.textvisible");
            }
            set
            {
                Set("title.textvisible", value);
            }
        }

        public bool TitleLabelStyleIsLocal
        {
            get
            {
                return IsLocalGet("title.labelstyle");
            }
            set
            {
                IsLocalSet("title.labelstyle", value);
            }
        }

        public DiagramLabelStyle TitleLabelStyle
        {
            get
            {
                return GetEnum<DiagramLabelStyle>("title.labelstyle");
            }
            set
            {
                Set("title.labelstyle", value);
            }
        }

        public bool TitleColorIsLocal
        {
            get
            {
                return IsLocalGet("title.textcolor");
            }
            set
            {
                IsLocalSet("title.textcolor", value);
            }
        }

        public string TitleColor
        {
            get
            {
                return GetColor("title.textcolor");
            }
            set
            {
                SetColor("title.textcolor", value);
            }
        }

        public bool TitleOpacityIsLocal
        {
            get
            {
                return IsLocalGet("title.textopacity");
            }
            set
            {
                IsLocalSet("title.textopacity", value);
            }
        }

        public double TitleOpacity
        {
            get
            {
                return GetDouble("title.textopacity");
            }
            set
            {
                if (value < 0 || value > 1.0)
                {
                    throw new ArgumentOutOfRangeException();
                }

                Set("title.textopacity", value);
            }
        }

        #endregion

        #region DiagramMark-specific Styles

        public bool MarkStyleIsLocalGet(string key, DiagramMarkType type = DiagramMarkType.Normal)
        {
            string prefix = GetMarkStylePrefix(type);

            return IsLocalGet(prefix + key);
        }

        public void MarkStyleIsLocalSet(string key, bool value, DiagramMarkType type = DiagramMarkType.Normal)
        {
            string prefix = GetMarkStylePrefix(type);

            IsLocalSet(prefix + key, value, Get(key));
        }

        public string MarkStyleGet(string key, DiagramMarkType type = DiagramMarkType.Normal)
        {
            string prefix = GetMarkStylePrefix(type);

            if (TryGet(prefix + key, out string result))
            {
                return result;
            }

            return Get(key);
        }

        public string MarkStyleGetColor(string key, DiagramMarkType type = DiagramMarkType.Normal)
        {
            string prefix = GetMarkStylePrefix(type);

            if (TryGetColor(prefix + key, out string result))
            {
                return result;
            }

            return GetColor(key);
        }

        public bool MarkStyleGetBoolean(string key, DiagramMarkType type = DiagramMarkType.Normal)
        {
            string prefix = GetMarkStylePrefix(type);

            if (TryGet(prefix + key, out bool result))
            {
                return result;
            }

            return GetBoolean(key);
        }

        public double MarkStyleGetDouble(string key, DiagramMarkType type = DiagramMarkType.Normal)
        {
            string prefix = GetMarkStylePrefix(type);

            if (TryGet(prefix + key, out double result))
            {
                return result;
            }

            return GetDouble(key);
        }

        public TEnum MarkStyleGetEnum<TEnum>(string key, DiagramMarkType type = DiagramMarkType.Normal) where TEnum : struct
        {
            string prefix = GetMarkStylePrefix(type);

            if (TryGet<TEnum>(prefix + key, out TEnum result))
            {
                return result;
            }

            return GetEnum<TEnum>(key);
        }

        public void MarkStyleSet(string key, double value, DiagramMarkType type = DiagramMarkType.Normal)
        {
            string prefix = GetMarkStylePrefix(type);

            Set(prefix + key, value);
        }

        public void MarkStyleSet(string key, object value, DiagramMarkType type = DiagramMarkType.Normal)
        {
            string prefix = GetMarkStylePrefix(type);

            Set(prefix + key, value);
        }

        public void MarkStyleSetColor(string key, string value, DiagramMarkType type = DiagramMarkType.Normal)
        {
            string prefix = GetMarkStylePrefix(type);

            SetColor(prefix + key, value);
        }

        public static string GetMarkStylePrefix(DiagramMarkType type)
        {
            string prefix = "";
            switch (type)
            {
                case DiagramMarkType.Muted:
                    prefix = "muted";
                    break;
                case DiagramMarkType.Root:
                    prefix = "root";
                    break;
                case DiagramMarkType.Open:
                    prefix = "open";
                    break;
                case DiagramMarkType.OpenRoot:
                    prefix = "openroot";
                    break;
                case DiagramMarkType.Bottom:
                    prefix = "bottom";
                    break;
            }
            return prefix;
        }

        #endregion

        #region DiagramFretLabel-specific Styles

        public bool FretLabelTextColorIsLocal
        {
            get
            {
                return IsLocalGet("fretlabel.textcolor");
            }
            set
            {
                IsLocalSet("fretlabel.textcolor", value);
            }
        }

        public string FretLabelTextColor
        {
            get
            {
                return GetColor("fretlabel.textcolor");
            }
            set
            {
                SetColor("fretlabel.textcolor", value);
            }
        }

        public bool FretLabelTextOpacityIsLocal
        {
            get
            {
                return IsLocalGet("fretlabel.textopacity");
            }
            set
            {
                IsLocalSet("fretlabel.textopacity", value);
            }
        }

        public double FretLabelTextOpacity
        {
            get
            {
                return GetDouble("fretlabel.textopacity");
            }
            set
            {
                if (value < 0 || value > 1.0)
                {
                    throw new ArgumentOutOfRangeException(nameof(value));
                }

                Set("fretlabel.textopacity", value);
            }
        }

        public bool FretLabelFontFamilyIsLocal
        {
            get
            {
                return IsLocalGet("fretlabel.fontfamily");
            }
            set
            {
                IsLocalSet("fretlabel.fontfamily", value);
            }
        }

        public string FretLabelFontFamily
        {
            get
            {
                return Get("fretlabel.fontfamily");
            }
            set
            {
                Set("fretlabel.fontfamily", value);
            }
        }

        public bool FretLabelTextStyleIsLocal
        {
            get
            {
                return IsLocalGet("fretlabel.textstyle");
            }
            set
            {
                IsLocalSet("fretlabel.textstyle", value);
            }
        }

        public DiagramTextStyle FretLabelTextStyle
        {
            get
            {
                return GetEnum<DiagramTextStyle>("fretlabel.textstyle");
            }
            set
            {
                Set("fretlabel.textstyle", value);
            }
        }

        public bool FretLabelTextAlignmentIsLocal
        {
            get
            {
                return IsLocalGet("fretlabel.textalignment");
            }
            set
            {
                IsLocalSet("fretlabel.textalignment", value);
            }
        }

        public DiagramHorizontalAlignment FretLabelTextAlignment
        {
            get
            {
                return GetEnum<DiagramHorizontalAlignment>("fretlabel.textalignment");
            }
            set
            {
                Set("fretlabel.textalignment", value);
            }
        }

        public bool FretLabelTextSizeRatioIsLocal
        {
            get
            {
                return IsLocalGet("fretlabel.textsizeratio");
            }
            set
            {
                IsLocalSet("fretlabel.textsizeratio", value);
            }
        }

        public double FretLabelTextSizeRatio
        {
            get
            {
                return GetDouble("fretlabel.textsizeratio", 1.0);
            }
            set
            {
                if (value < 0 || value > 1.0)
                {
                    throw new ArgumentOutOfRangeException(nameof(value));
                }

                Set("fretlabel.textsizeratio", value);
            }
        }

        public bool FretLabelTextVisibleIsLocal
        {
            get
            {
                return IsLocalGet("fretlabel.textvisible");
            }
            set
            {
                IsLocalSet("fretlabel.textvisible", value);
            }
        }

        public bool FretLabelTextVisible
        {
            get
            {
                return GetBoolean("fretlabel.textvisible");
            }
            set
            {
                Set("fretlabel.textvisible", value);
            }
        }

        public bool FretLabelGridPaddingIsLocal
        {
            get
            {
                return IsLocalGet("fretlabel.gridpadding");
            }
            set
            {
                IsLocalSet("fretlabel.gridpadding", value);
            }
        }

        public double FretLabelGridPadding
        {
            get
            {
                return GetDouble("fretlabel.gridpadding", 0);
            }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(value));
                }

                Set("fretlabel.gridpadding", value);
            }
        }

        public bool FretLabelTextWidthRatioIsLocal
        {
            get
            {
                return IsLocalGet("fretlabel.textwidthratio");
            }
            set
            {
                IsLocalSet("fretlabel.textwidthratio", value);
            }
        }

        public double FretLabelTextWidthRatio
        {
            get
            {
                return GetDouble("fretlabel.textwidthratio", 1.0);
            }
            set
            {
                if (value < 0 || value > 1.0)
                {
                    throw new ArgumentOutOfRangeException(nameof(value));
                }

                Set("fretlabel.textwidthratio", value);
            }
        }

        #endregion

        #region DiagramBarre-specific Styles

        public bool BarreVisibleIsLocal
        {
            get
            {
                return IsLocalGet("barre.visible");
            }
            set
            {
                IsLocalSet("barre.visible", value);
            }
        }

        public bool BarreVisible
        {
            get
            {
                return GetBoolean("barre.visible");
            }
            set
            {
                Set("barre.visible", value);
            }
        }

        public bool BarreVerticalAlignmentIsLocal
        {
            get
            {
                return IsLocalGet("barre.verticalalignment");
            }
            set
            {
                IsLocalSet("barre.verticalalignment", value);
            }
        }

        public DiagramVerticalAlignment BarreVerticalAlignment
        {
            get
            {
                return GetEnum<DiagramVerticalAlignment>("barre.verticalalignment");
            }
            set
            {
                Set("barre.verticalalignment", value);
            }
        }

        public bool BarreStackIsLocal
        {
            get
            {
                return IsLocalGet("barre.stack");
            }
            set
            {
                IsLocalSet("barre.stack", value);
            }
        }

        public DiagramBarreStack BarreStack
        {
            get
            {
                return GetEnum<DiagramBarreStack>("barre.stack");
            }
            set
            {
                Set("barre.stack", value);
            }
        }

        public bool BarreArcRatioIsLocal
        {
            get
            {
                return IsLocalGet("barre.arcratio");
            }
            set
            {
                IsLocalSet("barre.arcratio", value);
            }
        }

        public double BarreArcRatio
        {
            get
            {
                return GetDouble("barre.arcratio", 1.0);
            }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException();
                }

                Set("barre.arcratio", value);
            }
        }

        public bool BarreOpacityIsLocal
        {
            get
            {
                return IsLocalGet("barre.opacity");
            }
            set
            {
                IsLocalSet("barre.opacity", value);
            }
        }

        public double BarreOpacity
        {
            get
            {
                return GetDouble("barre.opacity");
            }
            set
            {
                if (value < 0 || value > 1.0)
                {
                    throw new ArgumentOutOfRangeException();
                }

                Set("barre.opacity", value);
            }
        }

        public bool BarreLineColorIsLocal
        {
            get
            {
                return IsLocalGet("barre.linecolor");
            }
            set
            {
                IsLocalSet("barre.linecolor", value);
            }
        }

        public string BarreLineColor
        {
            get
            {
                return GetColor("barre.linecolor");
            }
            set
            {
                SetColor("barre.linecolor", value);
            }
        }

        public bool BarreLineThicknessIsLocal
        {
            get
            {
                return IsLocalGet("barre.linethickness");
            }
            set
            {
                IsLocalSet("barre.linethickness", value);
            }
        }

        public double BarreLineThickness
        {
            get
            {
                return GetDouble("barre.linethickness");
            }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException();
                }

                Set("barre.linethickness", value);
            }
        }

        #endregion

        public DiagramStyle() : base() { }

        public DiagramStyle(string level) : base(level) { }

        public DiagramStyle(DiagramStyle parentStyle) : base(parentStyle) { }

        public DiagramStyle(DiagramStyle parentStyle, string level) : base(parentStyle, level) { }

        public void Read(XmlReader xmlReader)
        {
            Read(xmlReader, "style");
        }

        public void Write(XmlWriter xmlWriter, string filter = "")
        {
            Write(xmlWriter, "style", filter);
        }

        public DiagramStyle Clone()
        {
            DiagramStyle ds = new DiagramStyle(Level);

            if (null != Parent)
            {
                ds.Parent = Parent;
            }

            ds.CopyFrom(this);

            return ds;
        }

        public void SetColor(string key, string value)
        {
            string cleanColor = ColorUtils.ParseColor(value);
            Set(key, cleanColor);
        }

        public string GetColor(string key, bool recursive = true)
        {
            if (StringUtils.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentNullException(nameof(key));
            }

            if (TryGetColor(key, out string value, recursive))
            {
                return value;
            }

            throw new InheritableDictionaryKeyNotFoundException(this, key);
        }

        public string GetColor(string key, string defaultValue, bool recursive = true)
        {
            try
            {
                return GetColor(key, recursive);
            }
            catch (InheritableDictionaryKeyNotFoundException) { }

            return ColorUtils.ParseColor(defaultValue);
        }

        public bool TryGetColor(string key, out string result, bool recursive = true)
        {
            if (TryGet(key, out string rawResult, recursive))
            {
                return ColorUtils.TryParseColor(rawResult, out result);
            }

            result = default(string);
            return false;
        }

        public string GetSvgStyle(string[][] styleMap, string prefix = "")
        {
            if (null == styleMap)
            {
                throw new ArgumentNullException(nameof(styleMap));
            }

            string style = "";

            foreach (string[] map in styleMap)
            {
                string key = map[0];
                string svgStyle = map[1];

                string rawValue = "";
                bool found = false;

                // Add prefix if available

                prefix = CleanPrefix(prefix);

                if (!StringUtils.IsNullOrWhiteSpace(prefix))
                {
                    found = TryGet(prefix + key, out rawValue);
                }

                if (!found)
                {
                    found = TryGet(key, out rawValue);
                }

                if (found)
                {
                    style += ToSvgStyle(key, svgStyle, rawValue);
                }
            }

            return style;
        }

        private string ToSvgStyle(string key, string svgStyle, string rawValue)
        {
            string value = rawValue;

            // Check if rawValue needs to be converted into a valid SVG value
            if (key.EndsWith("textstyle") && StringUtils.IsNullOrWhiteSpace(svgStyle))
            {
                DiagramTextStyle dts = (DiagramTextStyle)Enum.Parse(typeof(DiagramTextStyle), rawValue);
                string textStyle = "";

                if (dts == DiagramTextStyle.Bold ||
                    dts == DiagramTextStyle.BoldItalic)
                {
                    textStyle += "font-weight:bold;";
                }

                if (dts == DiagramTextStyle.Italic ||
                    dts == DiagramTextStyle.BoldItalic)
                {
                    textStyle += "font-style:italic;";
                }

                return textStyle;
            }
            else if (svgStyle == "font-size")
            {
                return string.Format(CultureInfo.InvariantCulture, "{0}:{1}pt;", svgStyle, rawValue);
            }
            else if (svgStyle == "text-anchor")
            {
                DiagramHorizontalAlignment dta = (DiagramHorizontalAlignment)Enum.Parse(typeof(DiagramHorizontalAlignment), rawValue);

                switch (dta)
                {
                    case DiagramHorizontalAlignment.Left:
                        value = "start";
                        break;
                    case DiagramHorizontalAlignment.Center:
                        value = "middle";
                        break;
                    case DiagramHorizontalAlignment.Right:
                        value = "end";
                        break;
                }
            }
            else if (svgStyle == "fill" || svgStyle == "stroke")
            {
                value = value.ToLower();
            }

            if (!StringUtils.IsNullOrWhiteSpace(svgStyle))
            {
                return string.Format("{0}:{1};", svgStyle, value);
            }

            return string.Empty;
        }

        public override string GetFriendlyKeyName(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException(nameof(key));
            }

            key = CleanKey(key);

            switch (key)
            {
                case "newdiagram.numstrings":
                    return Strings.NewDiagramNumStringsFriendlyKeyName;
                case "newdiagram.numfrets":
                    return Strings.NewDiagramNumFretsFriendlyKeyName;
                case "diagram.orientation":
                    return Strings.DiagramOrientationFriendlyKeyName;
                case "diagram.labellayoutmodel":
                    return Strings.DiagramLabelLayoutModelFriendlyKeyName;
                case "diagram.color":
                    return Strings.DiagramColorFriendlyKeyName;
                case "diagram.opacity":
                    return Strings.DiagramOpacityFriendlyKeyName;
                case "diagram.bordercolor":
                    return Strings.DiagramBorderColorFriendlyKeyName;
                case "diagram.borderthickness":
                    return Strings.DiagramBorderThicknessFriendlyKeyName;
                case "grid.margin":
                    return Strings.GridMarginFriendlyKeyName;
                case "grid.marginleft":
                    return Strings.GridMarginLeftFriendlyKeyName;
                case "grid.marginright":
                    return Strings.GridMarginRightFriendlyKeyName;
                case "grid.margintop":
                    return Strings.GridMarginTopFriendlyKeyName;
                case "grid.marginbottom":
                    return Strings.GridMarginBottomFriendlyKeyName;
                case "grid.fretspacing":
                    return Strings.GridFretSpacingFriendlyKeyName;
                case "grid.stringspacing":
                    return Strings.GridStringSpacingFriendlyKeyName;
                case "grid.color":
                    return Strings.GridColorFriendlyKeyName;
                case "grid.opacity":
                    return Strings.GridOpacityFriendlyKeyName;
                case "grid.linecolor":
                    return Strings.GridLineColorFriendlyKeyName;
                case "grid.linethickness":
                    return Strings.GridLineThicknessFriendlyKeyName;
                case "grid.nutvisible":
                    return Strings.GridNutVisibleFriendlyKeyName;
                case "grid.nutratio":
                    return Strings.GridNutRatioFriendlyKeyName;
                case "title.gridpadding":
                    return Strings.TitleGridPaddingFriendlyKeyName;
                case "title.textsize":
                    return Strings.TitleTextSizeFriendlyKeyName;
                case "title.textmodratio":
                    return Strings.TitleTextSizeModRatioFriendlyKeyName;
                case "title.fontfamily":
                    return Strings.TitleFontFamilyFriendlyKeyName;
                case "title.textalignment":
                    return Strings.TitleTextAlignmentFriendlyKeyName;
                case "title.textstyle":
                    return Strings.TitleTextStyleFriendlyKeyName;
                case "title.textvisible":
                    return Strings.TitleVisibleFriendlyKeyName;
                case "title.labelstyle":
                    return Strings.TitleLabelStyleFriendlyKeyName;
                case "title.textcolor":
                    return Strings.TitleColorFriendlyKeyName;
                case "title.textopacity":
                    return Strings.TitleOpacityFriendlyKeyName;
                case "mark.shape":
                    return Strings.MarkShapeFriendlyKeyName;
                case "mark.visible":
                    return Strings.MarkVisibleFriendlyKeyName;
                case "mark.radiusratio":
                    return Strings.MarkRadiusRatioFriendlyKeyName;
                case "mark.color":
                    return Strings.MarkColorFriendlyKeyName;
                case "mark.opacity":
                    return Strings.MarkOpacityFriendlyKeyName;
                case "mark.textalignment":
                    return Strings.MarkTextAlignmentFriendlyKeyName;
                case "mark.textstyle":
                    return Strings.MarkTextStyleFriendlyKeyName;
                case "mark.textvisible":
                    return Strings.MarkTextVisibleFriendlyKeyName;
                case "mark.textcolor":
                    return Strings.MarkTextColorFriendlyKeyName;
                case "mark.textopacity":
                    return Strings.MarkTextOpacityFriendlyKeyName;
                case "mark.textsizeratio":
                    return Strings.MarkTextSizeRatioFriendlyKeyName;
                case "mark.fontfamily":
                    return Strings.MarkFontFamilyFriendlyKeyName;
                case "mark.bordercolor":
                    return Strings.MarkBorderColorFriendlyKeyName;
                case "mark.borderthickness":
                    return Strings.MarkBorderThicknessFriendlyKeyName;
                case "mutedmark.shape":
                    return Strings.MutedMarkShapeFriendlyKeyName;
                case "mutedmark.visible":
                    return Strings.MutedMarkVisibleFriendlyKeyName;
                case "mutedmark.radiusratio":
                    return Strings.MutedMarkRadiusRatioFriendlyKeyName;
                case "mutedmark.color":
                    return Strings.MutedMarkColorFriendlyKeyName;
                case "mutedmark.opacity":
                    return Strings.MutedMarkOpacityFriendlyKeyName;
                case "mutedmark.textalignment":
                    return Strings.MutedMarkTextAlignmentFriendlyKeyName;
                case "mutedmark.textstyle":
                    return Strings.MutedMarkTextStyleFriendlyKeyName;
                case "mutedmark.textvisible":
                    return Strings.MutedMarkTextVisibleFriendlyKeyName;
                case "mutedmark.textcolor":
                    return Strings.MutedMarkTextColorFriendlyKeyName;
                case "mutedmark.textopacity":
                    return Strings.MutedMarkTextOpacityFriendlyKeyName;
                case "mutedmark.textsizeratio":
                    return Strings.MutedMarkTextSizeRatioFriendlyKeyName;
                case "mutedmark.fontfamily":
                    return Strings.MutedMarkFontFamilyFriendlyKeyName;
                case "mutedmark.bordercolor":
                    return Strings.MutedMarkBorderColorFriendlyKeyName;
                case "mutedmark.borderthickness":
                    return Strings.MutedMarkBorderThicknessFriendlyKeyName;
                case "rootmark.shape":
                    return Strings.RootMarkShapeFriendlyKeyName;
                case "rootmark.visible":
                    return Strings.RootMarkVisibleFriendlyKeyName;
                case "rootmark.radiusratio":
                    return Strings.RootMarkRadiusRatioFriendlyKeyName;
                case "rootmark.color":
                    return Strings.RootMarkColorFriendlyKeyName;
                case "rootmark.opacity":
                    return Strings.RootMarkOpacityFriendlyKeyName;
                case "rootmark.textalignment":
                    return Strings.RootMarkTextAlignmentFriendlyKeyName;
                case "rootmark.textstyle":
                    return Strings.RootMarkTextStyleFriendlyKeyName;
                case "rootmark.textvisible":
                    return Strings.RootMarkTextVisibleFriendlyKeyName;
                case "rootmark.textcolor":
                    return Strings.RootMarkTextColorFriendlyKeyName;
                case "rootmark.textopacity":
                    return Strings.RootMarkTextOpacityFriendlyKeyName;
                case "rootmark.textsizeratio":
                    return Strings.RootMarkTextSizeRatioFriendlyKeyName;
                case "rootmark.fontfamily":
                    return Strings.RootMarkFontFamilyFriendlyKeyName;
                case "rootmark.bordercolor":
                    return Strings.RootMarkBorderColorFriendlyKeyName;
                case "rootmark.borderthickness":
                    return Strings.RootMarkBorderThicknessFriendlyKeyName;
                case "openmark.shape":
                    return Strings.OpenMarkShapeFriendlyKeyName;
                case "openmark.visible":
                    return Strings.OpenMarkVisibleFriendlyKeyName;
                case "openmark.radiusratio":
                    return Strings.OpenMarkRadiusRatioFriendlyKeyName;
                case "openmark.color":
                    return Strings.OpenMarkColorFriendlyKeyName;
                case "openmark.opacity":
                    return Strings.OpenMarkOpacityFriendlyKeyName;
                case "openmark.textalignment":
                    return Strings.OpenMarkTextAlignmentFriendlyKeyName;
                case "openmark.textstyle":
                    return Strings.OpenMarkTextStyleFriendlyKeyName;
                case "openmark.textvisible":
                    return Strings.OpenMarkTextVisibleFriendlyKeyName;
                case "openmark.textcolor":
                    return Strings.OpenMarkTextColorFriendlyKeyName;
                case "openmark.textopacity":
                    return Strings.OpenMarkTextOpacityFriendlyKeyName;
                case "openmark.textsizeratio":
                    return Strings.OpenMarkTextSizeRatioFriendlyKeyName;
                case "openmark.fontfamily":
                    return Strings.OpenMarkFontFamilyFriendlyKeyName;
                case "openmark.bordercolor":
                    return Strings.OpenMarkBorderColorFriendlyKeyName;
                case "openmark.borderthickness":
                    return Strings.OpenMarkBorderThicknessFriendlyKeyName;
                case "openrootmark.shape":
                    return Strings.OpenRootMarkShapeFriendlyKeyName;
                case "openrootmark.visible":
                    return Strings.OpenRootMarkVisibleFriendlyKeyName;
                case "openrootmark.radiusratio":
                    return Strings.OpenRootMarkRadiusRatioFriendlyKeyName;
                case "openrootmark.color":
                    return Strings.OpenRootMarkColorFriendlyKeyName;
                case "openrootmark.opacity":
                    return Strings.OpenRootMarkOpacityFriendlyKeyName;
                case "openrootmark.textalignment":
                    return Strings.OpenRootMarkTextAlignmentFriendlyKeyName;
                case "openrootmark.textstyle":
                    return Strings.OpenRootMarkTextStyleFriendlyKeyName;
                case "openrootmark.textvisible":
                    return Strings.OpenRootMarkTextVisibleFriendlyKeyName;
                case "openrootmark.textcolor":
                    return Strings.OpenRootMarkTextColorFriendlyKeyName;
                case "openrootmark.textopacity":
                    return Strings.OpenRootMarkTextOpacityFriendlyKeyName;
                case "openrootmark.textsizeratio":
                    return Strings.OpenRootMarkTextSizeRatioFriendlyKeyName;
                case "openrootmark.fontfamily":
                    return Strings.OpenRootMarkFontFamilyFriendlyKeyName;
                case "openrootmark.bordercolor":
                    return Strings.OpenRootMarkBorderColorFriendlyKeyName;
                case "openrootmark.borderthickness":
                    return Strings.OpenRootMarkBorderThicknessFriendlyKeyName;
                case "bottommark.shape":
                    return Strings.BottomMarkShapeFriendlyKeyName;
                case "bottommark.visible":
                    return Strings.BottomMarkVisibleFriendlyKeyName;
                case "bottommark.radiusratio":
                    return Strings.BottomMarkRadiusRatioFriendlyKeyName;
                case "bottommark.color":
                    return Strings.BottomMarkColorFriendlyKeyName;
                case "bottommark.opacity":
                    return Strings.BottomMarkOpacityFriendlyKeyName;
                case "bottommark.textalignment":
                    return Strings.BottomMarkTextAlignmentFriendlyKeyName;
                case "bottommark.textstyle":
                    return Strings.BottomMarkTextStyleFriendlyKeyName;
                case "bottommark.textvisible":
                    return Strings.BottomMarkTextVisibleFriendlyKeyName;
                case "bottommark.textcolor":
                    return Strings.BottomMarkTextColorFriendlyKeyName;
                case "bottommark.textopacity":
                    return Strings.BottomMarkTextOpacityFriendlyKeyName;
                case "bottommark.textsizeratio":
                    return Strings.BottomMarkTextSizeRatioFriendlyKeyName;
                case "bottommark.fontfamily":
                    return Strings.BottomMarkFontFamilyFriendlyKeyName;
                case "bottommark.bordercolor":
                    return Strings.BottomMarkBorderColorFriendlyKeyName;
                case "bottommark.borderthickness":
                    return Strings.BottomMarkBorderThicknessFriendlyKeyName;
                case "fretlabel.gridpadding":
                    return Strings.FretLabelGridPaddingFriendlyKeyName;
                case "fretlabel.textcolor":
                    return Strings.FretLabelTextColorFriendlyKeyName;
                case "fretlabel.textopacity":
                    return Strings.FretLabelTextOpacityFriendlyKeyName;
                case "fretlabel.textsizeratio":
                    return Strings.FretLabelTextSizeRatioFriendlyKeyName;
                case "fretlabel.textwidthratio":
                    return Strings.FretLabelTextWidthRatioFriendlyKeyName;
                case "fretlabel.fontfamily":
                    return Strings.FretLabelFontFamilyFriendlyKeyName;
                case "fretlabel.textalignment":
                    return Strings.FretLabelTextAlignmentFriendlyKeyName;
                case "fretlabel.textstyle":
                    return Strings.FretLabelTextStyleFriendlyKeyName;
                case "fretlabel.textvisible":
                    return Strings.FretLabelTextVisibleFriendlyKeyName;
                case "barre.visible":
                    return Strings.BarreVisibleFriendlyKeyName;
                case "barre.verticalalignment":
                    return Strings.BarreVerticalAlignmentFriendlyKeyName;
                case "barre.stack":
                    return Strings.BarreStackFriendlyKeyName;
                case "barre.arcratio":
                    return Strings.BarreArcRatioFriendlyKeyName;
                case "barre.opacity":
                    return Strings.BarreOpacityFriendlyKeyName;
                case "barre.linecolor":
                    return Strings.BarreLineColorFriendlyKeyName;
                case "barre.linethickness":
                    return Strings.BarreLineThicknessFriendlyKeyName;
                default:
                    return base.GetFriendlyKeyName(key);
            }
        }

        public override string GetFriendlyValue(string key, bool recursive = true)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException(nameof(key));
            }

            key = CleanKey(key);

            switch (key)
            {
                case "diagram.orientation":
                    return EnumUtils.GetFriendlyValue(GetEnum<DiagramOrientation>(key, recursive));
                case "diagram.labellayoutmodel":
                    return EnumUtils.GetFriendlyValue(GetEnum<DiagramLabelLayoutModel>(key, recursive));
                case "title.textalignment":
                case "mark.textalignment":
                case "mutedmark.textalignment":
                case "rootmark.textalignment":
                case "openmark.textalignment":
                case "openrootmark.textalignment":
                case "bottommark.textalignment":
                case "fretlabel.textalignment":
                    return EnumUtils.GetFriendlyValue(GetEnum<DiagramHorizontalAlignment>(key, recursive));
                case "title.textstyle":
                case "mark.textstyle":
                case "mutedmark.textstyle":
                case "rootmark.textstyle":
                case "openmark.textstyle":
                case "openrootmark.textstyle":
                case "bottommark.textstyle":
                case "fretlabel.textstyle":
                    return EnumUtils.GetFriendlyValue(GetEnum<DiagramTextStyle>(key, recursive));
                case "title.labelstyle":
                    return EnumUtils.GetFriendlyValue(GetEnum<DiagramLabelStyle>(key, recursive));
                case "title.textsize":
                    return string.Format("{0} pt", GetFriendlyDoubleValue(key, recursive));
                case "mark.shape":
                case "mutedmark.shape":
                case "rootmark.shape":
                case "openmark.shape":
                case "openrootmark.shape":
                case "bottommark.shape":
                    return EnumUtils.GetFriendlyValue(GetEnum<DiagramMarkShape>(key, recursive));
                case "barre.stack":
                    return EnumUtils.GetFriendlyValue(GetEnum<DiagramBarreStack>(key, recursive));
                case "barre.verticalalignment":
                    return EnumUtils.GetFriendlyValue(GetEnum<DiagramVerticalAlignment>(key, recursive));
                case "diagram.borderthickness":
                case "grid.margin":
                case "grid.marginleft":
                case "grid.marginright":
                case "grid.margintop":
                case "grid.marginbottom":
                case "grid.fretspacing":
                case "grid.stringspacing":
                case "grid.linethickness":
                case "title.gridpadding":
                case "mark.borderthickness":
                case "mutedmark.borderthickness":
                case "rootmark.borderthickness":
                case "openmark.borderthickness":
                case "openrootmark.borderthickness":
                case "bottommark.borderthickness":
                case "fretlabel.gridpadding":
                case "barre.linethickness":
                    return string.Format("{0} px", GetFriendlyDoubleValue(key, recursive));
                case "diagram.opacity":
                case "title.textopacity":
                case "title.textmodratio":
                case "grid.nutratio":
                case "grid.opacity":
                case "mark.radiusratio":
                case "mark.opacity":
                case "mark.textopacity":
                case "mark.textsizeratio":
                case "mutedmark.radiusratio":
                case "mutedmark.opacity":
                case "mutedmark.textopacity":
                case "mutedmark.textsizeratio":
                case "rootmark.radiusratio":
                case "rootmark.opacity":
                case "rootmark.textopacity":
                case "rootmark.textsizeratio":
                case "openmark.radiusratio":
                case "openmark.opacity":
                case "openmark.textopacity":
                case "openmark.textsizeratio":
                case "openrootmark.radiusratio":
                case "openrootmark.opacity":
                case "openrootmark.textopacity":
                case "openrootmark.textsizeratio":
                case "bottommark.radiusratio":
                case "bottommark.opacity":
                case "bottommark.textopacity":
                case "bottommark.textsizeratio":
                case "fretlabel.textopacity":
                case "fretlabel.textsizeratio":
                case "fretlabel.textwidthratio":
                case "barre.opacity":
                case "barre.arcratio":
                    return GetFriendlyDoubleValue(key, "P0", recursive);
                case "title.textvisible":
                case "grid.nutvisible":
                case "mark.visible":
                case "mutedmark.visible":
                case "rootmark.visible":
                case "openmark.visible":
                case "openrootmark.visible":
                case "bottommark.visible":
                case "mark.textvisible":
                case "mutedmark.textvisible":
                case "rootmark.textvisible":
                case "openmark.textvisible":
                case "openrootmark.textvisible":
                case "bottommark.textvisible":
                case "fretlabel.textvisible":
                case "barre.visible":
                    return GetBoolean(key, recursive) ? Strings.BooleanTrueFriendlyValue : Strings.BooleanFalseFriendlyValue;
                default:
                    return base.GetFriendlyValue(key, recursive);
            }
        }

        protected override string GetFriendlyLevel()
        {
            return string.Format(Strings.DiagramStyleFriendlyLevelFormat, base.GetFriendlyLevel());
        }
    }

    public enum DiagramOrientation
    {
        UpDown,
        LeftRight
    }

    public enum DiagramLabelLayoutModel
    {
        Overlap,
        AddPaddingHorizontal,
        AddPaddingVertical,
        AddPaddingBoth
    }

    public enum DiagramMarkShape
    {
        None,
        Circle,
        Square,
        Diamond,
        X
    }

    public enum DiagramTextStyle
    {
        Regular,
        Bold,
        Italic,
        BoldItalic
    }

    public enum DiagramHorizontalAlignment
    {
        Left,
        Center,
        Right
    }

    public enum DiagramVerticalAlignment
    {
        Top,
        Middle,
        Bottom
    }

    public enum DiagramLabelStyle
    {
        Regular,
        ChordName
    }

    public enum DiagramBarreStack
    {
        UnderMarks,
        OverMarks
    }
}
