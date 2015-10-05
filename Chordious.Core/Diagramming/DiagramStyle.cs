// 
// DiagramStyle.cs
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

        public int NewDiagramNumStringsGet()
        {
            return GetInt32("newdiagram.numstrings", 2);
        }

        public void NewDiagramNumStringsSet(int value)
        {
            if (value < 2)
            {
                throw new ArgumentOutOfRangeException("value");
            }
            Set("newdiagram.numstrings", value);
        }

        public int NewDiagramNumFretsGet()
        {
            return GetInt32("newdiagram.numfrets", 1);
        }

        public void NewDiagramNumFretsSet(int value)
        {
            if (value < 1)
            {
                throw new ArgumentOutOfRangeException("value");
            }
            Set("newdiagram.numfrets", value);
        }

        #endregion

        #region Diagram-specific Styles

        public DiagramOrientation DiagramOrientationGet()
        {
            return GetEnum<DiagramOrientation>("diagram.orientation");
        }

        public void DiagramOrientationSet(DiagramOrientation value)
        {
            Set("diagram.orientation", value);
        }

        public DiagramLabelLayoutModel DiagramLabelLayoutModelGet()
        {
            return GetEnum<DiagramLabelLayoutModel>("diagram.labellayoutmodel");
        }

        public void DiagramLabelLayoutModelSet(DiagramLabelLayoutModel value)
        {
            Set("diagram.labellayoutmodel", value);
        }

        public string DiagramColorGet()
        {
            return GetColor("diagram.color");
        }

        public void DiagramColorSet(string value)
        {
            SetColor("diagram.color", value);
        }

        public double DiagramOpacityGet()
        {
            return GetDouble("diagram.opacity");
        }

        public void DiagramOpacitySet(double value)
        {
            if (value < 0 || value > 1.0)
            {
                throw new ArgumentOutOfRangeException("value");
            }

            Set("diagram.opacity", value);
        }

        public string DiagramBorderColorGet()
        {
            return GetColor("diagram.bordercolor");
        }

        public void DiagramBorderColorSet(string value)
        {
            SetColor("diagram.bordercolor", value);
        }

        public double DiagramBorderThicknessGet()
        {
            return GetDouble("diagram.borderthickness");
        }

        public void DiagramBorderThicknessSet(double value)
        {
            if (value < 0)
            {
                throw new ArgumentOutOfRangeException("value");
            }

            Set("diagram.borderthickness", value);
        }

        #endregion

        #region Grid-specific Styles

        public double GridMarginGet()
        {
            return GetDouble("grid.margin", 0);
        }

        public void GridMarginSet(double value)
        {
            if (value < 0)
            {
                throw new ArgumentOutOfRangeException("value");
            }
            Set("grid.margin", value);
        }

        public bool GridMarginLeftOverrideGet()
        {
            return HasKey("grid.marginleft");
        }

        public void GridMarginLeftOverrideClear()
        {
            Clear("grid.marginleft");
        }

        public double GridMarginLeftGet()
        {
            double margin;
            if (TryGet("grid.marginleft", out margin))
            {
                return margin;
            }
            
            return this.GridMarginGet();
        }

        public void GridMarginLeftSet(double value)
        {
            if (value < 0)
            {
                throw new ArgumentOutOfRangeException("value");
            }
            Set("grid.marginleft", value);
        }

        public bool GridMarginRightOverrideGet()
        {
            return HasKey("grid.marginright");
        }

        public void GridMarginRightOverrideClear()
        {
            Clear("grid.marginright");
        }

        public double GridMarginRightGet()
        {
            double margin;
            if (TryGet("grid.marginright", out margin))
            {
                return margin;
            }

            return this.GridMarginGet();
        }

        public void GridMarginRightSet(double value)
        {
            if (value < 0)
            {
                throw new ArgumentOutOfRangeException("value");
            }
            Set("grid.marginright", value);
        }

        public bool GridMarginTopOverrideGet()
        {
            return HasKey("grid.margintop");
        }

        public void GridMarginTopOverrideClear()
        {
            Clear("grid.margintop");
        }

        public double GridMarginTopGet()
        {
            double margin;
            if (TryGet("grid.margintop", out margin))
            {
                return margin;
            }

            return this.GridMarginGet();
        }

        public void GridMarginTopSet(double value)
        {
            if (value < 0)
            {
                throw new ArgumentOutOfRangeException("value");
            }
            Set("grid.margintop", value);
        }

        public bool GridMarginBottomOverrideGet()
        {
            return HasKey("grid.marginbottom");
        }

        public void GridMarginBottomOverrideClear()
        {
            Clear("grid.marginbottom");
        }

        public double GridMarginBottomGet()
        {
            double margin;
            if (TryGet("grid.marginbottom", out margin))
            {
                return margin;
            }

            return this.GridMarginGet();
        }

        public void GridMarginBottomSet(double value)
        {
            if (value < 0)
            {
                throw new ArgumentOutOfRangeException("value");
            }
            Set("grid.marginbottom", value);
        }

        public double GridFretSpacingGet()
        {
            return GetDouble("grid.fretspacing");
        }

        public void GridFretSpacingSet(double value)
        {
            if (value < 0)
            {
                throw new ArgumentOutOfRangeException("value");
            }
            Set("grid.fretspacing", value);
        }

        public double GridStringSpacingGet()
        {
            return GetDouble("grid.stringspacing");
        }

        public void GridStringSpacingSet(double value)
        {
            if (value < 0)
            {
                throw new ArgumentOutOfRangeException("value");
            }
            Set("grid.stringspacing", value);
        }

        public string GridColorGet()
        {
            return GetColor("grid.color");
        }

        public void GridColorSet(string value)
        {
            SetColor("grid.color", value);
        }

        public double GridOpacityGet()
        {
            return GetDouble("grid.opacity");
        }

        public void GridOpacitySet(double value)
        {
            if (value < 0 || value > 1.0)
            {
                throw new ArgumentOutOfRangeException("value");
            }

            Set("grid.opacity", value);
        }

        public string GridLineColorGet()
        {
            return GetColor("grid.linecolor");
        }

        public void GridLineColorSet(string value)
        {
            SetColor("grid.linecolor", value);
        }

        public double GridLineThicknessGet()
        {
            return GetDouble("grid.linethickness");
        }

        public void GridLineThicknessSet(double value)
        {
            if (value < 0)
            {
                throw new ArgumentOutOfRangeException("value");
            }
            Set("grid.linethickness", value);
        }

        public bool GridNutVisibleGet()
        {
            return GetBoolean("grid.nutvisible");
        }

        public void GridNutVisibleSet(bool value)
        {
            Set("grid.nutvisible", value);
        }

        public double GridNutRatioGet()
        {
            return GetDouble("grid.nutratio", 1.0);
        }

        public void GridNutRatioSet(double value)
        {
            if (value < 0)
            {
                throw new ArgumentOutOfRangeException("value");
            }
            Set("grid.nutratio", value);
        }

        #endregion

        #region DiagramTitle-specific Styles

        public double TitleGridPaddingGet()
        {
            return GetDouble("title.gridpadding", 0);
        }

        public void TitleGridPaddingSet(double value)
        {
            if (value < 0)
            {
                throw new ArgumentOutOfRangeException("value");
            }
            Set("title.gridpadding", value);
        }

        public double TitleTextSizeGet()
        {
            return GetDouble("title.textsize");
        }

        public void TitleTextSizeSet(double value)
        {
            if (value < 0)
            {
                throw new ArgumentOutOfRangeException("value");
            }
            Set("title.textsize", value);
        }

        public double TitleTextSizeModRatioGet()
        {
            return GetDouble("title.textmodratio", 1.0);
        }

        public void TitleTextSizeModRatioSet(double value)
        {
            if (value < 0 || value > 1.0)
            {
                throw new ArgumentOutOfRangeException("value");
            }
            Set("title.textmodratio", value);
        }

        public string TitleFontFamilyGet()
        {
            return Get("title.fontfamily");
        }

        public void TitleFontFamilySet(string value)
        {
            Set("title.fontfamily", value);
        }

        public DiagramHorizontalAlignment TitleTextAlignmentGet()
        {
            return GetEnum<DiagramHorizontalAlignment>("title.textalignment");
        }

        public void TitleTextAlignmentSet(DiagramHorizontalAlignment value)
        {
            Set("title.textalignment", value);
        }

        public DiagramTextStyle TitleTextStyleGet()
        {
            return GetEnum<DiagramTextStyle>("title.textstyle");
        }

        public void TitleTextStyleSet(DiagramTextStyle value)
        {
            Set("title.textstyle", value);
        }

        public bool TitleVisibleGet()
        {
            return GetBoolean("title.textvisible");
        }

        public void TitleVisibleSet(bool value)
        {
            Set("title.textvisible", value);
        }

        public DiagramLabelStyle TitleLabelStyleGet()
        {
                return GetEnum<DiagramLabelStyle>("title.labelstyle");
        }

        public void TitleLabelStyleSet(DiagramLabelStyle value)
        {
            Set("title.labelstyle", value);
        }

        public string TitleColorGet()
        {
            return GetColor("title.textcolor");
        }

        public void TitleColorSet(string value)
        {
            SetColor("title.textcolor", value);
        }

        public double TitleOpacityGet()
        {
            return GetDouble("title.textopacity");
        }

        public void TitleOpacitySet(double value)
        {
            if (value < 0 || value > 1.0)
            {
                throw new ArgumentOutOfRangeException("value");
            }

            Set("title.textopacity", value);
        }

        #endregion

        #region DiagramMark-specific Styles

        public DiagramMarkShape MarkShapeGet(DiagramMarkType type = DiagramMarkType.Normal)
        {
            string prefix = DiagramStyle.MarkStylePrefix(type);
        
            DiagramMarkShape result;
            if (TryGet<DiagramMarkShape>(prefix + "mark.shape", out result))
            {
                return result;
            }

            return GetEnum<DiagramMarkShape>("mark.shape");
        }

        public void MarkShapeSet(DiagramMarkShape value, DiagramMarkType type = DiagramMarkType.Normal)
        {
            string prefix = DiagramStyle.MarkStylePrefix(type);
            Set(prefix + "mark.shape", value);
        }

        public bool MarkVisibleGet(DiagramMarkType type = DiagramMarkType.Normal)
        {
            string prefix = DiagramStyle.MarkStylePrefix(type);

            bool result;
            if (TryGet(prefix + "mark.visible", out result))
            {
                return result;
            }

            return GetBoolean("mark.visible");
        }

        public void MarkVisibleSet(bool value, DiagramMarkType type = DiagramMarkType.Normal)
        {
            string prefix = DiagramStyle.MarkStylePrefix(type);
            Set(prefix + "mark.visible", value);
        }

        public string MarkColorGet(DiagramMarkType type = DiagramMarkType.Normal)
        {
            string prefix = DiagramStyle.MarkStylePrefix(type);

            string result;
            if (TryGetColor(prefix + "mark.color", out result))
            {
                return result;
            }

            return GetColor("mark.color");
        }

        public void MarkColorSet(string value, DiagramMarkType type = DiagramMarkType.Normal)
        {
            string prefix = DiagramStyle.MarkStylePrefix(type);
            SetColor(prefix + "mark.color", value);
        }

        public double MarkOpacityGet(DiagramMarkType type = DiagramMarkType.Normal)
        {
            string prefix = DiagramStyle.MarkStylePrefix(type);

            double result;
            if (TryGet(prefix + "mark.opacity", out result))
            {
                return result;
            }

            return GetDouble("mark.opacity");
        }

        public void MarkOpacitySet(double value, DiagramMarkType type = DiagramMarkType.Normal)
        {
            if (value < 0 || value > 1.0)
            {
                throw new ArgumentOutOfRangeException("value");
            }

            string prefix = DiagramStyle.MarkStylePrefix(type);
            Set(prefix + "mark.opacity", value);
        }

        public DiagramTextStyle MarkTextStyleGet(DiagramMarkType type = DiagramMarkType.Normal)
        {
            string prefix = DiagramStyle.MarkStylePrefix(type);

            DiagramTextStyle result;
            if (TryGet<DiagramTextStyle>(prefix + "mark.textstyle", out result))
            {
                return result;
            }

            return GetEnum<DiagramTextStyle>("mark.textstyle");
            }

        public void MarkTextStyleSet(DiagramTextStyle value, DiagramMarkType type = DiagramMarkType.Normal)
        {
            string prefix = DiagramStyle.MarkStylePrefix(type);
            Set(prefix + "mark.textstyle", value);
        }

        public DiagramHorizontalAlignment MarkTextAlignmentGet(DiagramMarkType type = DiagramMarkType.Normal)
        {
            string prefix = DiagramStyle.MarkStylePrefix(type);

            DiagramHorizontalAlignment result;
            if (TryGet<DiagramHorizontalAlignment>(prefix + "mark.textalignment", out result))
            {
                return result;
            }

            return GetEnum<DiagramHorizontalAlignment>("mark.textalignment");
        }

        public void MarkTextAlignmentSet(DiagramHorizontalAlignment value, DiagramMarkType type = DiagramMarkType.Normal)
        {
            string prefix = DiagramStyle.MarkStylePrefix(type);
            Set(prefix + "mark.textalignment", value);
        }

        public double MarkTextSizeRatioGet(DiagramMarkType type = DiagramMarkType.Normal)
        {
            string prefix = DiagramStyle.MarkStylePrefix(type);

            double result;
            if (TryGet(prefix + "mark.textsizeratio", out result))
            {
                return result;
            }

            return GetDouble("mark.textsizeratio", 1.0);
        }

        public void MarkTextSizeRatioSet(double value, DiagramMarkType type = DiagramMarkType.Normal)
        {
            if (value < 0 || value > 1.0)
            {
                throw new ArgumentOutOfRangeException("value");
            }

            string prefix = DiagramStyle.MarkStylePrefix(type);
            Set(prefix + "mark.textsizeratio", value);
        }

        public bool MarkTextVisibleGet(DiagramMarkType type = DiagramMarkType.Normal)
        {
            string prefix = DiagramStyle.MarkStylePrefix(type);

            bool result;
            if (TryGet(prefix + "mark.textvisible", out result))
            {
                return result;
            }

            return GetBoolean("mark.textvisible");
        }

        public void MarkTextVisibleSet(bool value, DiagramMarkType type = DiagramMarkType.Normal)
        {
            string prefix = DiagramStyle.MarkStylePrefix(type);
            Set(prefix + "mark.textvisible", value);
        }

        public string MarkTextColorGet(DiagramMarkType type = DiagramMarkType.Normal)
        {
            string prefix = DiagramStyle.MarkStylePrefix(type);

            string result;
            if (TryGetColor(prefix + "mark.textcolor", out result))
            {
                return result;
            }

            return GetColor("mark.textcolor");
        }

        public void MarkTextColorSet(string value, DiagramMarkType type = DiagramMarkType.Normal)
        {
            string prefix = DiagramStyle.MarkStylePrefix(type);
            SetColor(prefix + "mark.textcolor", value);
        }

        public double MarkTextOpacityGet(DiagramMarkType type = DiagramMarkType.Normal)
        {
            string prefix = DiagramStyle.MarkStylePrefix(type);

            double result;
            if (TryGet(prefix + "mark.textopacity", out result))
            {
                return result;
            }

            return GetDouble("mark.textopacity");
        }

        public void MarkTextOpacitySet(double value, DiagramMarkType type = DiagramMarkType.Normal)
        {
            if (value < 0 || value > 1.0)
            {
                throw new ArgumentOutOfRangeException("value");
            }

            string prefix = DiagramStyle.MarkStylePrefix(type);
            Set(prefix + "mark.textopacity", value);
        }

        public string MarkFontFamilyGet(DiagramMarkType type = DiagramMarkType.Normal)
        {
            string prefix = DiagramStyle.MarkStylePrefix(type);

            string result;
            if (TryGet(prefix + "mark.fontfamily", out result))
            {
                return result;
            }

            return Get("mark.fontfamily");
        }

        public void MarkFontFamilySet(string value, DiagramMarkType type = DiagramMarkType.Normal)
        {
            string prefix = DiagramStyle.MarkStylePrefix(type);
            Set(prefix + "mark.fontfamily", value);
        }

        public double MarkRadiusRatioGet(DiagramMarkType type = DiagramMarkType.Normal)
        {
            string prefix = DiagramStyle.MarkStylePrefix(type);

            double result;
            if (TryGet(prefix + "mark.radiusratio", out result))
            {
                return result;
            }

            return GetDouble("mark.radiusratio", 1.0);
        }

        public void MarkRadiusRatioSet(double value, DiagramMarkType type = DiagramMarkType.Normal)
        {
            if (value < 0 || value > 1.0)
            {
                throw new ArgumentOutOfRangeException("value");
            }

            string prefix = DiagramStyle.MarkStylePrefix(type);
            Set(prefix + "mark.radiusratio", value);
        }

        public string MarkBorderColorGet(DiagramMarkType type = DiagramMarkType.Normal)
        {
            string prefix = DiagramStyle.MarkStylePrefix(type);

            string result;
            if (TryGetColor(prefix + "mark.bordercolor", out result))
            {
                return result;
            }

            return GetColor("mark.bordercolor");
        }

        public void MarkBorderColorSet(string value, DiagramMarkType type = DiagramMarkType.Normal)
        {
            string prefix = DiagramStyle.MarkStylePrefix(type);
            SetColor(prefix + "mark.bordercolor", value);
        }

        public double MarkBorderThicknessGet(DiagramMarkType type = DiagramMarkType.Normal)
        {
            string prefix = DiagramStyle.MarkStylePrefix(type);

            double result;
            if (TryGet(prefix + "mark.borderthickness", out result))
            {
                return result;
            }

            return GetDouble("mark.borderthickness");
        }

        public void MarkBorderThicknessSet(double value, DiagramMarkType type = DiagramMarkType.Normal)
        {
            if (value < 0)
            {
                throw new ArgumentOutOfRangeException("value");
            }

            string prefix = DiagramStyle.MarkStylePrefix(type);
            Set(prefix + "mark.borderthickness", value);
        }

        public static string MarkStylePrefix(DiagramMarkType type)
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

        public string FretLabelTextColorGet()
        {
            return GetColor("fretlabel.textcolor");
        }

        public void FretLabelTextColorSet(string value)
        {
            SetColor("fretlabel.textcolor", value);
        }

        public double FretLabelTextOpacityGet()
        {
            return GetDouble("fretlabel.textopacity");
        }

        public void FretLabelTextOpacitySet(double value)
        {
            if (value < 0 || value > 1.0)
            {
                throw new ArgumentOutOfRangeException("value");
            }

            Set("fretlabel.textopacity", value);
        }

        public string FretLabelFontFamilyGet()
        {
            return Get("fretlabel.fontfamily");
        }

        public void FretLabelFontFamilySet(string value)
        {
            Set("fretlabel.fontfamily", value);
        }

        public DiagramTextStyle FretLabelTextStyleGet()
        {
            return GetEnum<DiagramTextStyle>("fretlabel.textstyle");
        }

        public void FretLabelTextStyleSet(DiagramTextStyle value)
        {
            Set("fretlabel.textstyle", value);
        }

        public DiagramHorizontalAlignment FretLabelTextAlignmentGet()
        {
            return GetEnum<DiagramHorizontalAlignment>("fretlabel.textalignment");
        }

        public void FretLabelTextAlignmentSet(DiagramHorizontalAlignment value)
        {
            Set("fretlabel.textalignment", value);
        }

        public double FretLabelTextSizeRatioGet()
        {
            return GetDouble("fretlabel.textsizeratio", 1.0);
        }

        public void FretLabelTextSizeRatioSet(double value)
        {
            if (value < 0 || value > 1.0)
            {
                throw new ArgumentOutOfRangeException("value");
            }
            Set("fretlabel.textsizeratio", value);
        }

        public bool FretLabelTextVisibleGet()
        {
            return GetBoolean("fretlabel.textvisible");
        }

        public void FretLabelTextVisibleSet(bool value)
        {
            Set("fretlabel.textvisible", value);
        }

        public double FretLabelGridPaddingGet()
        {
            return GetDouble("fretlabel.gridpadding", 0);
        }

        public void FretLabelGridPaddingSet(double value)
        {
            if (value < 0)
            {
                throw new ArgumentOutOfRangeException("value");
            }
            Set("fretlabel.gridpadding", value);
        }

        public double FretLabelTextWidthRatioGet()
        {
            return GetDouble("fretlabel.textwidthratio", 1.0);
        }

        public void FretLabelTextWidthRatioSet(double value)
        {
            if (value < 0 || value > 1.0)
            {
                throw new ArgumentOutOfRangeException("value");
            }
            Set("fretlabel.textwidthratio", value);
        }

        #endregion

        #region DiagramBarre-specific Styles

        public bool BarreVisibleGet()
        {
            return GetBoolean("barre.visible");
        }

        public void BarreVisibleSet(bool value)
        {
            Set("barre.visible", value);
        }

        public DiagramVerticalAlignment BarreVerticalAlignmentGet()
        {
            return GetEnum<DiagramVerticalAlignment>("barre.verticalalignment");
        }

        public void BarreVerticalAlignmentSet(DiagramVerticalAlignment value)
        {
            Set("barre.verticalalignment", value);
        }

        public DiagramBarreStack BarreStackGet()
        {
            return GetEnum<DiagramBarreStack>("barre.stack");
        }

        public void BarreStackSet(DiagramBarreStack value)
        {
            Set("barre.stack", value);
        }

        public double BarreArcRatioGet()
        {
            return GetDouble("barre.arcratio", 1.0);
        }

        public void BarreArcRatioSet(double value)
        {
            if (value < 0)
            {
                throw new ArgumentOutOfRangeException("value");
            }
            Set("barre.arcratio", value);
        }

        #endregion

        public DiagramStyle() : base() { }

        public DiagramStyle(string level) : base(level) { }

        public DiagramStyle(DiagramStyle parentStyle) : base(parentStyle) { }

        public DiagramStyle(DiagramStyle parentStyle, string level) : base(parentStyle, level) { }

        public void Read(XmlReader xmlReader)
        {
            base.Read(xmlReader, "style");
        }

        public void Write(XmlWriter xmlWriter, string filter = "")
        {
            base.Write(xmlWriter, "style", filter);
        }

        public new DiagramStyle Clone()
        {
            return (DiagramStyle)base.Clone();
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
                throw new ArgumentNullException("key");
            }

            string value;
            if (TryGetColor(key, out value, recursive))
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
            string rawResult;
            if (TryGet(key, out rawResult, recursive))
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
                throw new ArgumentNullException("styleMap");
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
                return String.Format("{0}:{1}pt;", svgStyle, rawValue);
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
                return String.Format("{0}:{1};", svgStyle, value);
            }

            return String.Empty;
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
