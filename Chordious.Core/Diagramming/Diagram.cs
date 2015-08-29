// 
// Diagram.cs
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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace com.jonthysell.Chordious.Core
{
    public class Diagram
    {
        #region Elements

        public string Title { get; set; }

        public IEnumerable<DiagramMark> Marks
        {
            get
            {
                return this._marks.AsEnumerable();
            }
        }
        private List<DiagramMark> _marks;

        public IEnumerable<DiagramFretLabel> FretLabels
        {
            get
            {
                return this._fretLabels.AsEnumerable();
            }
        }
        private List<DiagramFretLabel> _fretLabels;

        public IEnumerable<DiagramBarre> Barres
        {
            get
            {
                return this._barres.AsEnumerable();
            }
        }
        private List<DiagramBarre> _barres;

        #endregion

        #region Properties

        public int NumStrings
        {
            get
            {
                return this._numStrings;
            }
            set
            {
                this.Resize(value, this.NumFrets);             
            }
        }
        private int _numStrings;

        public int NumFrets
        {
            get
            {
                return this._numFrets;
            }
            set
            {
                this.Resize(this.NumStrings, value);
            }
        }
        private int _numFrets;

        #endregion

        #region Style

        public DiagramStyle Style { get; private set; }

        public DiagramOrientation Orientation
        {
            get
            {
                return Style.DiagramOrientationGet();
            }
            set
            {
                Style.DiagramOrientationSet(value);
            }
        }

        public DiagramLabelLayoutModel LabelLayoutModel
        {
            get
            {
                return Style.DiagramLabelLayoutModelGet();
            }
            set
            {
                Style.DiagramLabelLayoutModelSet(value);
            }
        }

        public string DiagramColor
        {
            get
            {
                return Style.DiagramColorGet();
            }
            set
            {
                Style.DiagramColorSet(value);
            }
        }

        public double DiagramOpacity
        {
            get
            {
                return Style.DiagramOpacityGet();
            }
            set
            {
                Style.DiagramOpacitySet(value);
            }
        }

        public string DiagramBorderColor
        {
            get
            {
                return Style.DiagramBorderColorGet();
            }
            set
            {
                Style.DiagramBorderColorSet(value);
            }
        }

        public double DiagramBorderThickness
        {
            get
            {
                return Style.DiagramBorderThicknessGet();
            }
            set
            {
                Style.DiagramBorderThicknessSet(value);
            }
        }

        #endregion

        #region Title Style

        public double TitleGridPadding
        {
            get
            {
                return Style.TitleGridPaddingGet();
            }
            set
            {
                Style.TitleGridPaddingSet(value);
            }
        }

        public double TitleTextSize
        {
            get
            {
                return Style.TitleTextSizeGet();
            }
            set
            {
                Style.TitleTextSizeSet(value);
            }
        }

        public double TitleTextSizeModRatio
        {
            get
            {
                return Style.TitleTextSizeModRatioGet();
            }
            set
            {
                Style.TitleTextSizeModRatioSet(value);
            }
        }

        public string TitleFontFamily
        {
            get
            {
                return Style.TitleFontFamilyGet();
            }
            set
            {
                Style.TitleFontFamilySet(value);
            }
        }

        public DiagramHorizontalAlignment TitleTextAlignment
        {
            get
            {
                return Style.TitleTextAlignmentGet();
            }
            set
            {
                Style.TitleTextAlignmentSet(value);
            }
        }

        public DiagramTextStyle TitleTextStyle
        {
            get
            {
                return Style.TitleTextStyleGet();
            }
            set
            {
                Style.TitleTextStyleSet(value);
            }
        }

        public bool TitleVisible
        {
            get
            {
                return Style.TitleVisibleGet();
            }
            set
            {
                Style.TitleVisibleSet(value);
            }
        }

        public DiagramLabelStyle TitleLabelStyle
        {
            get
            {
                return Style.TitleLabelStyleGet();
            }
            set
            {
                Style.TitleLabelStyleSet(value);
            }
        }

        public string TitleColor
        {
            get
            {
                return Style.TitleColorGet();
            }
            set
            {
                Style.TitleColorSet(value);
            }
        }

        public double TitleOpacity
        {
            get
            {
                return Style.TitleOpacityGet();
            }
            set
            {
                Style.TitleOpacitySet(value);
            }
        }

        #endregion

        #region Grid Style

        public double GridMargin
        {
            get
            {
                return Style.GridMarginGet();
            }
            set
            {
                Style.GridMarginSet(value);
            }
        }

        public bool GridMarginLeftOverride
        {
            get
            {
                return Style.GridMarginLeftOverrideGet();
            }
            set
            {
                if (value && !Style.GridMarginLeftOverrideGet())
                {
                    Style.GridMarginLeftSet(GridMargin);
                }
                else if (!value)
                {
                    Style.GridMarginLeftOverrideClear();
                }
            }
        }

        public double GridMarginLeft
        {
            get
            {
                return Style.GridMarginLeftGet();
            }
            set
            {
                Style.GridMarginLeftSet(value);
            }
        }

        public bool GridMarginRightOverride
        {
            get
            {
                return Style.GridMarginRightOverrideGet();
            }
            set
            {
                if (value && !Style.GridMarginRightOverrideGet())
                {
                    Style.GridMarginRightSet(GridMargin);
                }
                else if (!value)
                {
                    Style.GridMarginRightOverrideClear();
                }
            }
        }

        public double GridMarginRight
        {
            get
            {
                return Style.GridMarginRightGet();
            }
            set
            {
                Style.GridMarginRightSet(value);
            }
        }

        public bool GridMarginTopOverride
        {
            get
            {
                return Style.GridMarginTopOverrideGet();
            }
            set
            {
                if (value && !Style.GridMarginTopOverrideGet())
                {
                    Style.GridMarginTopSet(GridMargin);
                }
                else if (!value)
                {
                    Style.GridMarginTopOverrideClear();
                }
            }
        }

        public double GridMarginTop
        {
            get
            {
                return Style.GridMarginTopGet();
            }
            set
            {
                Style.GridMarginTopSet(value);
            }
        }

        public bool GridMarginBottomOverride
        {
            get
            {
                return Style.GridMarginBottomOverrideGet();
            }
            set
            {
                if (value && !Style.GridMarginBottomOverrideGet())
                {
                    Style.GridMarginBottomSet(GridMargin);
                }
                else if (!value)
                {
                    Style.GridMarginBottomOverrideClear();
                }
            }
        }

        public double GridMarginBottom
        {
            get
            {
                return Style.GridMarginBottomGet();
            }
            set
            {
                Style.GridMarginBottomSet(value);
            }
        }

        public double GridFretSpacing
        {
            get
            {
                return Style.GridFretSpacingGet();
            }
            set
            {
                Style.GridFretSpacingSet(value);
            }
        }

        public double GridStringSpacing
        {
            get
            {
                return Style.GridStringSpacingGet();
            }
            set
            {
                Style.GridStringSpacingSet(value);
            }
        }

        public string GridColor
        {
            get
            {
                return Style.GridColorGet();
            }
            set
            {
                Style.GridColorSet(value);
            }
        }

        public double GridOpacity
        {
            get
            {
                return Style.GridOpacityGet();
            }
            set
            {
                Style.GridOpacitySet(value);
            }
        }

        public string GridLineColor
        {
            get
            {
                return Style.GridLineColorGet();
            }
            set
            {
                Style.GridLineColorSet(value);
            }
        }

        public double GridLineThickness
        {
            get
            {
                return Style.GridLineThicknessGet();
            }
            set
            {
                Style.GridLineThicknessSet(value);
            }
        }

        public bool GridNutVisible
        {
            get
            {
                return Style.GridNutVisibleGet();
            }
            set
            {
                Style.GridNutVisibleSet(value);
            }
        }

        public double GridNutRatio
        {
            get
            {
                return Style.GridNutRatioGet();
            }
            set
            {
                Style.GridNutRatioSet(value);
            }
        }

        #endregion

        public Diagram(DiagramStyle parentStyle)
        {
            if (null == parentStyle)
            {
                throw new ArgumentNullException("parentStyle");
            }

            this._marks = new List<DiagramMark>();
            this._fretLabels = new List<DiagramFretLabel>();
            this._barres = new List<DiagramBarre>();

            this.Style = new DiagramStyle(parentStyle, "Diagram");

            this._numFrets = Style.NewDiagramNumFretsGet();
            this._numStrings = Style.NewDiagramNumStringsGet();
        }

        public Diagram(DiagramStyle parentStyle, int numStrings, int numFrets) : this(parentStyle)
        {
            this.NumStrings = numStrings;
            this.NumFrets = numFrets;
        }

        public Diagram(DiagramStyle parentStyle, XmlReader xmlReader) : this(parentStyle)
        {
            if (null == xmlReader)
            {
                throw new ArgumentNullException("xmlReader");
            }

            this.Read(xmlReader);
        }

        public void Read(XmlReader xmlReader)
        {
            if (null == xmlReader)
            {
                throw new ArgumentNullException("xmlReader");
            }

            using (xmlReader)
            {
                if (xmlReader.IsStartElement())
                {
                    this.Title = xmlReader.GetAttribute("title");

                    this.NumStrings = Int32.Parse(xmlReader.GetAttribute("strings"));
                    this.NumFrets = Int32.Parse(xmlReader.GetAttribute("frets"));

                    while (xmlReader.Read())
                    {
                        if (xmlReader.IsStartElement())
                        {
                            switch(xmlReader.Name)
                            {
                                case "mark":
                                    this.NewMark(xmlReader.ReadSubtree());
                                    break;
                                case "barre":
                                    this.NewBarre(xmlReader.ReadSubtree());
                                    break;
                                case "fretlabel":
                                    this.NewFretLabel(xmlReader.ReadSubtree());
                                    break;
                                case "style":
                                    this.Style.Read(xmlReader.ReadSubtree());
                                    break;
                            }
                        }
                    }
                }
            }
        }

        public void Write(XmlWriter xmlWriter)
        {
            if (null == xmlWriter)
            {
                throw new ArgumentNullException("xmlWriter");
            }

            xmlWriter.WriteStartElement("diagram");

            xmlWriter.WriteAttributeString("title", this.Title);
            xmlWriter.WriteAttributeString("strings", this.NumStrings.ToString());
            xmlWriter.WriteAttributeString("frets", this.NumFrets.ToString());

            foreach (DiagramMark mark in this.Marks)
            {
                mark.Write(xmlWriter);
            }

            foreach (DiagramBarre barre in this.Barres)
            {
                barre.Write(xmlWriter);
            }

            foreach (DiagramFretLabel fretLabel in this.FretLabels)
            {
                fretLabel.Write(xmlWriter);
            }

            this.Style.Write(xmlWriter);

            xmlWriter.WriteEndElement();
        }

        public Diagram Clone()
        {
            Diagram clone = new Diagram(Style.Parent, NumStrings, NumFrets);

            clone.Title = this.Title;
            clone.Style.CopyFrom(this.Style);

            foreach (DiagramMark mark in this.Marks)
            {
                DiagramMark clonedMark = clone.NewMark(mark.Position, mark.Text);
                clonedMark.Style.CopyFrom(mark.Style);
                clonedMark.Type = mark.Type;
            }

            foreach (DiagramBarre barre in this.Barres)
            {
                DiagramBarre clonedBarre = clone.NewBarre(barre.Position);
                clonedBarre.Style.CopyFrom(barre.Style);
            }

            foreach (DiagramFretLabel fretLabel in this.FretLabels)
            {
                DiagramFretLabel clonedFretLabel = clone.NewFretLabel(fretLabel.Position, fretLabel.Text);
                clonedFretLabel.Style.CopyFrom(fretLabel.Style);
            }

            return clone;
        }

        #region Operations

        public void Resize(int newNumStrings, int newNumFrets)
        {
            if (newNumStrings < 2)
            {
                throw new ArgumentOutOfRangeException("newNumStrings");
            }

            if (newNumFrets < 1)
            {
                throw new ArgumentOutOfRangeException("newNumFrets");
            }

            List<DiagramMark> endMarks = new List<DiagramMark>();

            // Check through marks
            foreach (DiagramMark mark in this.Marks)
            {
                if (mark.Position.String > newNumStrings)
                {
                    throw new CantResizeDiagramException();
                }

                // Save end marks to be pushed after resize
                if (mark.IsBelowBottomEdge())
                {
                    endMarks.Add(mark);
                }
                else if (mark.Position.Fret > newNumFrets + 1)
                {
                    throw new CantResizeDiagramException();
                }
            }

            // Check through fret labels
            foreach (DiagramFretLabel fretLabel in this.FretLabels)
            {
                if (fretLabel.Position.Fret > newNumFrets)
                {
                    throw new CantResizeDiagramException();
                }
            }

            this._numStrings = newNumStrings;
            this._numFrets = newNumFrets;

            // Move end marks to new end
            foreach (DiagramMark endMark in endMarks)
            {
                endMark.Position = new MarkPosition(endMark.Position.String, this.NumFrets + 1);
            }
        }

        #endregion

        #region Element Checks

        public bool ValidPosition(ElementPosition position)
        {
            if (null == position)
            {
                throw new ArgumentNullException("position");
            }

            if (position.GetType() == typeof(MarkPosition))
            {
                MarkPosition mp = (MarkPosition)position;
                return (mp.Fret <= this.NumFrets + 1) && (mp.String <= this.NumStrings);
            }
            else if (position.GetType() == typeof(FretLabelPosition))
            {
                FretLabelPosition flp = (FretLabelPosition)position;
                return (flp.Fret <= this.NumFrets);
            }
            else if (position.GetType() == typeof(BarrePosition))
            {
                BarrePosition bp = (BarrePosition)position;
                return (bp.EndString <= this.NumStrings);
            }

            return false;
        }

        public bool HasElementAt(ElementPosition position)
        {
            return null != ElementAt(position);
        }

        public DiagramElement ElementAt(ElementPosition position)
        {
            if (null == position)
            {
                throw new ArgumentNullException("position");
            }

            if (position.GetType() == typeof(MarkPosition))
            {
                foreach (DiagramMark mark in this._marks)
                {
                    if (mark.Position.Equals(position))
                    {
                        return mark;
                    }
                }
            }
            else if (position.GetType() == typeof(FretLabelPosition))
            {
                foreach (DiagramFretLabel fretLabel in this.FretLabels)
                {
                    if (fretLabel.Position.Equals(position))
                    {
                        return fretLabel;
                    }
                }
            }
            else if (position.GetType() == typeof(BarrePosition))
            {
                foreach (DiagramBarre barre in this.Barres)
                {
                    if (barre.Position.Overlaps((BarrePosition)position))
                    {
                        return barre;
                    }
                }
            }

            return null;
        }

        public bool HasVisibleTitle()
        {
            return !String.IsNullOrEmpty(this.Title) && this.TitleVisible;
        }

        public bool HasVisibleFretLabels(FretLabelSide side)
        {
            foreach (DiagramFretLabel fretLabel in this.FretLabels)
            {
                if (fretLabel.IsVisible() && fretLabel.Position.Side == side)
                {
                    return true;
                }
            }

            return false;
        }

        public double MaxFretLabelWidth(FretLabelSide side)
        {
            double maxWidth = 0;
            foreach (DiagramFretLabel fretLabel in this.FretLabels)
            {
                if (fretLabel.IsVisible() && fretLabel.Position.Side == side)
                {
                    double width = fretLabel.GridPadding + fretLabel.GetTextWidth();
                    if (width > maxWidth)
                    {
                        maxWidth = width;
                    }
                }
            }

            return maxWidth;
        }

        public bool HasMarksAboveTopEdge()
        {
            foreach (DiagramMark mark in this.Marks)
            {
                if (mark.IsVisible() && mark.IsAboveTopEdge())
                {
                    return true;
                }
            }

            return false;
        }

        public bool HasMarksBelowBottomEdge()
        {
            foreach (DiagramMark mark in this.Marks)
            {
                if (mark.IsVisible() && mark.IsBelowBottomEdge())
                {
                    return true;
                }
            }

            return false;
        }

        #endregion

        #region New Elements

        public DiagramMark NewMark(MarkPosition position, string text = "")
        {
            DiagramMark mark = new DiagramMark(this, position, text);
            this._marks.Add(mark);
            return mark;
        }

        internal DiagramMark NewMark(XmlReader xmlReader)
        {
            DiagramMark mark = new DiagramMark(this, xmlReader);
            this._marks.Add(mark);
            return mark;
        }

        public void RemoveMark(MarkPosition position)
        {
            this._marks.Remove((DiagramMark)ElementAt(position));
        }

        public DiagramFretLabel NewFretLabel(FretLabelPosition position, string text)
        {
            DiagramFretLabel fretLabel = new DiagramFretLabel(this, position, text);
            this._fretLabels.Add(fretLabel);
            return fretLabel;
        }

        internal DiagramFretLabel NewFretLabel(XmlReader xmlReader)
        {
            DiagramFretLabel fretLabel = new DiagramFretLabel(this, xmlReader);
            this._fretLabels.Add(fretLabel);
            return fretLabel;
        }

        public void RemoveFretLabel(FretLabelPosition position)
        {
            this._fretLabels.Remove((DiagramFretLabel)ElementAt(position));
        }

        public DiagramBarre NewBarre(BarrePosition position)
        {
            DiagramBarre barre = new DiagramBarre(this, position);
            this._barres.Add(barre);
            return barre;
        }

        internal DiagramBarre NewBarre(XmlReader xmlReader)
        {
            DiagramBarre barre = new DiagramBarre(this, xmlReader);
            this._barres.Add(barre);
            return barre;
        }

        public void RemoveBarre(BarrePosition position)
        {
            this._barres.Remove((DiagramBarre)ElementAt(position));
        }

        #endregion

        #region Grid Positioning

        public double GridLeftEdge()
        {
            double edge = this.GridMarginLeft;
            if (this.LabelLayoutModel == DiagramLabelLayoutModel.AddPaddingBoth
                || this.LabelLayoutModel == DiagramLabelLayoutModel.AddPaddingHorizontal)
            {
                edge += this.MaxFretLabelWidth(FretLabelSide.Left);
            }

            return edge;
        }

        public double GridTopEdge()
        {
            double edge = this.GridMarginTop;

            if (this.LabelLayoutModel == DiagramLabelLayoutModel.AddPaddingBoth
                || this.LabelLayoutModel == DiagramLabelLayoutModel.AddPaddingVertical)
            {
                if (this.HasMarksAboveTopEdge())
                {
                    edge += this.GridFretSpacing;
                }

                if (this.HasVisibleTitle())
                {
                    edge += this.TitleGridPadding;
                    edge += Orientation == DiagramOrientation.LeftRight ? this.TitleTextSize * this.Title.Length : this.TitleTextSize;
                }
            }

            return edge;
        }

        public double GridRightEdge()
        {
            return GridLeftEdge() + GridWidth();
        }

        public double GridBottomEdge()
        {
            return GridTopEdge() + GridHeight();
        }

        public double GridHeight()
        {
            return this.GridFretSpacing * this.NumFrets;
        }

        public double GridWidth()
        {
            return this.GridStringSpacing * (this.NumStrings - 1);
        }

        public bool InGrid(double x, double y)
        {
            return (x > GridLeftEdge() && x < GridRightEdge()) && (y > GridTopEdge() && y < GridBottomEdge());
        }

        public ElementPosition GetPosition<T>(double x, double y) where T : ElementPosition
        {
            int @string;
            int fret;
            GetPositionInternal(x, y, out @string, out fret);

            try
            {
                if (typeof(T) == typeof(MarkPosition))
                {
                    MarkPosition mp = new MarkPosition(@string, fret);
                    if (ValidPosition(mp))
                    {
                        return mp;
                    }
                }
                else if (typeof(T) == typeof(FretLabelPosition))
                {
                    FretLabelSide? fls = null;

                    if (@string == 0 || @string == 1)
                    {
                        fls = FretLabelSide.Left;
                    }
                    else if (@string == NumStrings || @string == NumStrings + 1)
                    {
                        fls = FretLabelSide.Right;
                    }

                    if (fls.HasValue)
                    {
                        FretLabelPosition flp = new FretLabelPosition(fls.Value, fret);
                        if (ValidPosition(flp))
                        {
                            return flp;
                        }
                    }
                }
                else if (typeof(T) == typeof(BarrePosition))
                {
                    BarrePosition bp = new BarrePosition(fret, @string, NumStrings);
                    if (ValidPosition(bp))
                    {
                        return bp;
                    }
                }
            }
            catch (Exception) { }

            return null;
        }

        private void GetPositionInternal(double x, double y, out int @string, out int fret)
        {
            int[] position = new int[] { -1, -1 };

            // Fix for left/right orientation
            if (Orientation == DiagramOrientation.LeftRight)
            {
                double oldY = y;
                y = x;
                x = GetWidth() - oldY;
            }

            double topEdge = GridTopEdge();
            double bottomEdge = GridBottomEdge();
            double leftEdge = GridLeftEdge();
            double rightEdge = GridRightEdge();

            double fretSpacing = this.GridFretSpacing;
            double stringSpacing = this.GridStringSpacing;

            // String position
            double xMin = leftEdge - (stringSpacing * 1.5);
            double xMax = rightEdge + (stringSpacing * 1.5);
            if (x >= xMin && x <= xMax)
            {
                position[0] = (int)(Remap(x, xMin, xMax, 0, NumStrings + 1) + 0.5);
            }

            // Fret position
            double yMin = topEdge - fretSpacing;
            double yMax = bottomEdge + fretSpacing;
            if (y >= yMin && y <= yMax)
            {
                position[1] = (int)(Remap(y, yMin, yMax, 0, NumFrets + 2));
            }

            @string = position[0];
            fret = position[1];
        }

        private double Remap(double value, double sourceMin, double sourceMax, double destMin, double destMax)
        {
            if (value <= sourceMin)
            {
                return destMin;
            }
            else if (value >= sourceMax)
            {
                return destMax;
            }

            double relativeValue = (value - sourceMin) / (sourceMax - sourceMin);

            return destMin + (relativeValue * (destMax - destMin));
        }

        #endregion

        #region Drawing

        public double TotalHeight()
        {
            if (Orientation == DiagramOrientation.LeftRight)
            {
                return GetWidth();
            }

            return GetHeight();
        }

        private double GetHeight()
        {
            double height = GridBottomEdge() + this.GridMarginBottom;
            
            if (this.LabelLayoutModel == DiagramLabelLayoutModel.AddPaddingBoth
                 || this.LabelLayoutModel == DiagramLabelLayoutModel.AddPaddingVertical)
            {
                if (this.HasMarksBelowBottomEdge())
                {
                    height += this.GridFretSpacing;
                }
            }

            return height;
        }

        public double TotalWidth()
        {
            if (Orientation == DiagramOrientation.LeftRight)
            {
                return GetHeight();
            }

            return GetWidth();
        }

        private double GetWidth()
        {
            double width = GridRightEdge() + this.GridMarginRight;

            if (this.LabelLayoutModel == DiagramLabelLayoutModel.AddPaddingBoth
                 || this.LabelLayoutModel == DiagramLabelLayoutModel.AddPaddingHorizontal)
            {
                width += this.MaxFretLabelWidth(FretLabelSide.Right);
            }

            return width;
        }

        public string ToImageMarkup(ImageMarkupType type)
        {
            switch (type)
            {
                case ImageMarkupType.SVG:
                    return this.ToSvg();
                case ImageMarkupType.XAML:
                    return this.ToXaml();
            }

            return String.Empty;
        }

        private string ToSvg()
        {
            StringBuilder sb = new StringBuilder();

            double totalWidth = this.GetWidth();
            double totalHeight = this.GetHeight();

            // Add background

            string baseStyle = this.Style.GetSvgStyle(Diagram._baseStyleMap);

            if (this.DiagramBorderThickness > 0)
            {
                baseStyle += this.Style.GetSvgStyle(Diagram._baseStyleMapBorder);
            }

            sb.AppendFormat(SvgConstants.RECTANGLE,
                            baseStyle,
                            totalWidth,
                            totalHeight,
                            0,
                            0);

            // Add grid rectangle
            double rectX = this.GridLeftEdge();
            double rectY = this.GridTopEdge();

            double rectWidth = this.GridWidth();
            double rectHeight = this.GridHeight();

            string gridRectStyle = this.Style.GetSvgStyle(Diagram._gridBaseStyleMap);

            sb.AppendFormat(SvgConstants.RECTANGLE,
                            gridRectStyle,
                            rectWidth,
                            rectHeight,
                            rectX,
                            rectY);

            string lineStyle = this.Style.GetSvgStyle(Diagram._lineStyleMap);

            // Add vertical Lines
            double vSpacing = this.GridStringSpacing;
            for (int vLine = 1; vLine < NumStrings - 1; vLine++)
            {
                double x = rectX + (vLine * vSpacing);
                double y1 = rectY;
                double y2 = y1 + rectHeight;
                sb.AppendFormat(SvgConstants.LINE,
                                lineStyle,
                                x,
                                y1,
                                x,
                                y2);
            }

            // Add horizontal Lines
            double hSpacing = this.GridFretSpacing;
            for (int hLine = 1; hLine < NumFrets; hLine++)
            {
                double x1 = rectX;
                double x2 = rectX + rectWidth;
                double y = rectY + (hLine * hSpacing);

                sb.AppendFormat(SvgConstants.LINE,
                                lineStyle,
                                x1,
                                y,
                                x2,
                                y);
            }

            // Add nut
            if (this.GridNutVisible)
            {
                double strokeCorrection = this.GridLineThickness / this.GridNutRatio;

                double x1 = rectX - strokeCorrection;
                double x2 = rectX + rectWidth + strokeCorrection;
                double y = rectY - strokeCorrection;

                string nutStyle = this.Style.GetSvgStyle(Diagram._nutStyleMap);
                nutStyle += String.Format("stroke-width:{0};", this.GridLineThickness * this.GridNutRatio);

                sb.AppendFormat(SvgConstants.LINE,
                                nutStyle,
                                x1,
                                y,
                                x2,
                                y);
            }

            // Add barres underneath the marks
            foreach (DiagramBarre barre in this.Barres)
            {
                if (barre.Stack == DiagramBarreStack.UnderMarks)
                {
                    sb.Append(barre.ToSvg());
                }
            }

            // Add marks
            foreach (DiagramMark mark in this.Marks)
            {
                sb.Append(mark.ToSvg());
            }

            // Add barres on top of the marks
            foreach (DiagramBarre barre in this.Barres)
            {
                if (barre.Stack == DiagramBarreStack.OverMarks)
                {
                    sb.Append(barre.ToSvg());
                }
            }

            // Add fret labels
            foreach (DiagramFretLabel fretLabel in this.FretLabels)
            {
                sb.Append(fretLabel.ToSvg());
            }

            // Add title
            if (HasVisibleTitle())
            {
                double titleX = rectX + (rectWidth / 2.0);
                double titleY = rectY - this.TitleGridPadding;

                if (Orientation == DiagramOrientation.UpDown)
                {
                    switch (this.TitleTextAlignment)
                    {
                        case DiagramHorizontalAlignment.Left:
                            titleX = rectX;
                            break;
                        case DiagramHorizontalAlignment.Right:
                            titleX = rectX + rectWidth;
                            break;
                    }
                }

                if (this.LabelLayoutModel == DiagramLabelLayoutModel.AddPaddingBoth
                || this.LabelLayoutModel == DiagramLabelLayoutModel.AddPaddingVertical)
                {
                    if (this.HasMarksAboveTopEdge())
                    {
                        titleY -= this.GridFretSpacing;
                    }
                }

                string titleStyle = this.Style.GetSvgStyle(Diagram._titleStyleMap);

                switch (this.TitleLabelStyle)
                {
                    case DiagramLabelStyle.ChordName:
                        double modifierSize = this.TitleTextSize * this.TitleTextSizeModRatio;
                        string modifierStyle = String.Format("font-size:{0}pt;", modifierSize);
                        string titleChordNameFormat = SvgConstants.TEXT_CHORDNAME;
                        if (Orientation == DiagramOrientation.LeftRight)
                        {
                            titleChordNameFormat = SvgConstants.ROTATED_TEXT_CHORDNAME;
                            titleX -= (TitleTextSize - this.TitleGridPadding) / 2.0;
                            titleY -= (TitleTextSize + (modifierSize * (Title.Length - 1))) / 2.0;
                        }
                        sb.AppendFormat(titleChordNameFormat,
                                titleStyle,
                                titleX,
                                titleY,
                                this.Title[0].ToString(),
                                modifierStyle,
                                this.Title.Substring(1));
                        break;
                    case DiagramLabelStyle.Regular:
                    default:
                        string titleFormat = SvgConstants.TEXT;
                        if (Orientation == DiagramOrientation.LeftRight)
                        {
                            titleFormat = SvgConstants.ROTATED_TEXT;
                            titleX -= (TitleTextSize - this.TitleGridPadding) / 2.0;
                            titleY -= (TitleTextSize * Title.Length) / 2.0;
                        }
                        sb.AppendFormat(titleFormat,
                                titleStyle,
                                titleX,
                                titleY,
                                this.Title);
                        break;
                }
            }

            string imageText = sb.ToString().Trim();

            // Apply rotation if necessary
            if (this.Orientation == DiagramOrientation.LeftRight)
            {
                imageText = String.Format(SvgConstants.ROTATE,
                    imageText,
                    -90,
                    totalWidth / 2.0,
                    totalHeight / 2.0);

                // Need to swap here since we used Gets instead of Totals above
                double temp = totalHeight;
                totalHeight = totalWidth;
                totalWidth = temp;
            }

            // Add watermark
            string watermark = String.Format("Created with {0}.", AppInfo.ProgramTitle);

            return String.Format(SvgConstants.BASE, totalWidth, totalHeight, watermark, imageText);
        }

        private static string[][] _baseStyleMap =
        {
            new string[] {"diagram.color", "fill"},
            new string[] {"diagram.opacity", "fill-opacity"},
        };

        private static string[][] _baseStyleMapBorder =
        {
            new string[] {"diagram.bordercolor", "stroke"},
            new string[] {"diagram.borderthickness", "stroke-width"},
        };

        private static string[][] _gridBaseStyleMap =
        {
            new string[] {"grid.color", "fill"},
            new string[] {"grid.opacity", "fill-opacity"},
            new string[] {"grid.linecolor", "stroke"},
            new string[] {"grid.linethickness", "stroke-width"},
        };

        private static string[][] _lineStyleMap =
        {
            new string[] {"grid.linecolor", "stroke"},
            new string[] {"grid.linethickness", "stroke-width"},
        };

        private static string[][] _nutStyleMap =
        {
            new string[] {"grid.linecolor", "stroke"},
        };

        private static string[][] _titleStyleMap =
        {
            new string[] {"title.textcolor", "fill"},
            new string[] {"title.textopacity", "opacity"},
            new string[] {"title.textsize", "font-size"},
            new string[] {"title.fontfamily", "font-family"},
            new string[] {"title.textalignment", "text-anchor"},
            new string[] {"title.textstyle", ""},
        };

        private string ToXaml()
        {
            throw new NotImplementedException();
        }

        #endregion
    }

    public class CantResizeDiagramException : ChordiousException
    {
        public CantResizeDiagramException() : base() { }
    }

    public enum ImageMarkupType
    {
        SVG,
        XAML
    }
}
