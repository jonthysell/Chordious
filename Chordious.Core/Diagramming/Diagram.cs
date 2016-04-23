// 
// Diagram.cs
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

        public DiagramStyle Style { get; private set; }

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

            // Check through barres
            foreach (DiagramBarre barre in this.Barres)
            {
                if (barre.Position.Fret > newNumFrets ||
                    barre.Position.StartString > newNumStrings ||
                    barre.Position.EndString > newNumStrings)
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
                return (bp.Fret <= this.NumFrets && bp.StartString <= this.NumStrings && bp.EndString <= this.NumStrings);
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
                    if (mark.Position == position)
                    {
                        return mark;
                    }
                }
            }
            else if (position.GetType() == typeof(FretLabelPosition))
            {
                foreach (DiagramFretLabel fretLabel in this.FretLabels)
                {
                    if (fretLabel.Position == position)
                    {
                        return fretLabel;
                    }
                }
            }
            else if (position.GetType() == typeof(BarrePosition))
            {
                foreach (DiagramBarre barre in this.Barres)
                {
                    if (barre.Position == position)
                    {
                        return barre;
                    }
                }
            }

            return null;
        }

        public bool CanPositionElementAt(DiagramElement element, ElementPosition position)
        {
            if (null == element)
            {
                throw new ArgumentNullException("element");
            }

            if (null == position)
            {
                throw new ArgumentNullException("position");
            }

            // Warning: Do not use element.Position as this method can be called during
            // element creation before element.Position is defined

            if (position.GetType() == typeof(MarkPosition))
            {
                foreach (DiagramMark mark in this._marks)
                {
                    if (mark.Position == position && element != mark)
                    {
                        return false;
                    }
                }
            }
            else if (position.GetType() == typeof(FretLabelPosition))
            {
                foreach (DiagramFretLabel fretLabel in this.FretLabels)
                {
                    if (fretLabel.Position == position && element != fretLabel)
                    {
                        return false;
                    }
                }
            }
            else if (position.GetType() == typeof(BarrePosition))
            {
                foreach (DiagramBarre barre in this.Barres)
                {
                    if (barre.Position.Overlaps((BarrePosition)position) && element != barre)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public bool HasVisibleTitle()
        {
            return !String.IsNullOrEmpty(this.Title) && this.Style.TitleVisible;
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

        public bool CanAddNewMarkAt(MarkPosition position)
        {
            if (null == position)
            {
                throw new ArgumentNullException("position");
            }

            if (ValidPosition(position) && !HasElementAt(position))
            {
                return true;
            }

            return false;
        }

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

        public bool CanRemoveMarkAt(MarkPosition position)
        {
            if (null == position)
            {
                throw new ArgumentNullException("position");
            }

            return HasElementAt(position);
        }

        public void RemoveMark(MarkPosition position)
        {
            this._marks.Remove((DiagramMark)ElementAt(position));
        }

        public bool CanAddNewFretLabelAt(FretLabelPosition position)
        {
            if (null == position)
            {
                throw new ArgumentNullException("position");
            }

            if (ValidPosition(position) && !HasElementAt(position))
            {
                return true;
            }

            return false;
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

        public bool CanRemoveFretLabelAt(FretLabelPosition position)
        {
            if (null == position)
            {
                throw new ArgumentNullException("position");
            }

            return HasElementAt(position);
        }

        public void RemoveFretLabel(FretLabelPosition position)
        {
            this._fretLabels.Remove((DiagramFretLabel)ElementAt(position));
        }

        public bool CanAddNewBarreAt(BarrePosition position)
        {
            if (null == position)
            {
                throw new ArgumentNullException("position");
            }

            if (ValidPosition(position) && !HasElementAt(position))
            {
                foreach (DiagramBarre barre in Barres)
                {
                    if (barre.Position.Overlaps(position))
                    {
                        return false;
                    }
                }
                return true;
            }

            return false;
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

        public bool CanRemoveBarreAt(BarrePosition position)
        {
            if (null == position)
            {
                throw new ArgumentNullException("position");
            }

            return HasElementAt(position);
        }

        public void RemoveBarre(BarrePosition position)
        {
            this._barres.Remove((DiagramBarre)ElementAt(position));
        }

        #endregion

        #region Grid Positioning

        public double GridLeftEdge()
        {
            double edge = this.Style.GridMarginLeft;
            if (this.Style.LabelLayoutModel == DiagramLabelLayoutModel.AddPaddingBoth
                || this.Style.LabelLayoutModel == DiagramLabelLayoutModel.AddPaddingHorizontal)
            {
                edge += this.MaxFretLabelWidth(FretLabelSide.Left);
            }

            return edge;
        }

        public double GridTopEdge()
        {
            double edge = this.Style.GridMarginTop;

            if (this.Style.LabelLayoutModel == DiagramLabelLayoutModel.AddPaddingBoth
                || this.Style.LabelLayoutModel == DiagramLabelLayoutModel.AddPaddingVertical)
            {
                if (this.HasMarksAboveTopEdge())
                {
                    edge += this.Style.GridFretSpacing;
                }

                if (this.HasVisibleTitle())
                {
                    edge += this.Style.TitleGridPadding;
                    edge += this.Style.Orientation == DiagramOrientation.LeftRight ? this.Style.TitleTextSize * this.Title.Length : this.Style.TitleTextSize;
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
            return this.Style.GridFretSpacing * this.NumFrets;
        }

        public double GridWidth()
        {
            return this.Style.GridStringSpacing * (this.NumStrings - 1);
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

                    if (@string == 0)
                    {
                        fls = FretLabelSide.Left;
                    }
                    else if (@string == NumStrings + 1)
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
                    // Try to find existing barre covering @string
                    foreach (DiagramBarre barre in Barres)
                    {
                        if (barre.Position.Contains(fret, @string))
                        {
                            return barre.Position.Clone();
                        }
                    }

                    // Find the largest clear range
                    int startString = @string;
                    for (int endString = NumStrings; endString > startString; endString--)
                    {
                        BarrePosition bp = new BarrePosition(fret, startString, endString);
                        if (CanAddNewBarreAt(bp))
                        {
                            return bp;
                        }
                    }
                }
            }
            catch (Exception) { }

            return null;
        }

        private void GetPositionInternal(double x, double y, out int @string, out int fret)
        {
            @string = -1;
            fret = -1;

            // Fix for left/right orientation
            if (this.Style.Orientation == DiagramOrientation.LeftRight)
            {
                double oldY = y;
                y = x;
                x = GetWidth() - oldY;
            }

            double topEdge = GridTopEdge();
            double bottomEdge = GridBottomEdge();
            double leftEdge = GridLeftEdge();
            double rightEdge = GridRightEdge();

            double leftMargin = leftEdge;
            double rightMargin = GetWidth() - rightEdge;

            double fretSpacing = this.Style.GridFretSpacing;
            double stringSpacing = this.Style.GridStringSpacing;

            double stringRange = stringSpacing / 2.0;

            // Determine buffered grid edges
            double xGridMin = leftEdge - Math.Min(stringRange, leftMargin / 3.0);
            double xGridMax = rightEdge + Math.Min(stringRange, rightMargin / 3.0);

            // Find string position
            if (x < xGridMin) // left of grid
            {
                @string = 0;
            }
            else if (x > xGridMax) // right of grid
            {
                @string = NumStrings + 1;
            }
            else // in grid
            {
                for (int str = 0; str < NumStrings; str++)
                {
                    double stringX = leftEdge + (stringSpacing * str);
                    if (Within(x, stringX, stringRange))
                    {
                        @string = str + 1;
                        break;
                    }
                }
            }

            // Find fret position
            double yMin = Math.Max(0, topEdge - fretSpacing);
            double yMax = Math.Min(GetHeight(), bottomEdge + fretSpacing);
            if (y >= yMin && y <= yMax)
            {
                fret = (int)(Remap(y, yMin, yMax, 0, NumFrets + 2));
            }
        }

        private bool Within(double value, double center, double range)
        {
            if (range < 0)
            {
                throw new ArgumentOutOfRangeException("range");
            }

            return (value >= (center - range)) && (value <= (center + range));
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
            if (this.Style.Orientation == DiagramOrientation.LeftRight)
            {
                return GetWidth();
            }

            return GetHeight();
        }

        private double GetHeight()
        {
            double height = GridBottomEdge() + this.Style.GridMarginBottom;

            if (this.Style.LabelLayoutModel == DiagramLabelLayoutModel.AddPaddingBoth
                 || this.Style.LabelLayoutModel == DiagramLabelLayoutModel.AddPaddingVertical)
            {
                if (this.HasMarksBelowBottomEdge())
                {
                    height += this.Style.GridFretSpacing;
                }
            }

            return height;
        }

        public double TotalWidth()
        {
            if (this.Style.Orientation == DiagramOrientation.LeftRight)
            {
                return GetHeight();
            }

            return GetWidth();
        }

        private double GetWidth()
        {
            double width = GridRightEdge() + this.Style.GridMarginRight;

            if (this.Style.LabelLayoutModel == DiagramLabelLayoutModel.AddPaddingBoth
                 || this.Style.LabelLayoutModel == DiagramLabelLayoutModel.AddPaddingHorizontal)
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

            if (this.Style.DiagramBorderThickness > 0)
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
            double vSpacing = this.Style.GridStringSpacing;
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
            double hSpacing = this.Style.GridFretSpacing;
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
            if (this.Style.GridNutVisible)
            {
                double strokeCorrection = this.Style.GridLineThickness / this.Style.GridNutRatio;

                double x1 = rectX - strokeCorrection;
                double x2 = rectX + rectWidth + strokeCorrection;
                double y = rectY - strokeCorrection;

                string nutStyle = this.Style.GetSvgStyle(Diagram._nutStyleMap);
                nutStyle += String.Format("stroke-width:{0};", this.Style.GridLineThickness * this.Style.GridNutRatio);

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
                double titleY = rectY - this.Style.TitleGridPadding;

                if (this.Style.Orientation == DiagramOrientation.UpDown)
                {
                    switch (this.Style.TitleTextAlignment)
                    {
                        case DiagramHorizontalAlignment.Left:
                            titleX = rectX;
                            break;
                        case DiagramHorizontalAlignment.Right:
                            titleX = rectX + rectWidth;
                            break;
                    }
                }

                if (this.Style.LabelLayoutModel == DiagramLabelLayoutModel.AddPaddingBoth
                || this.Style.LabelLayoutModel == DiagramLabelLayoutModel.AddPaddingVertical)
                {
                    if (this.HasMarksAboveTopEdge())
                    {
                        titleY -= this.Style.GridFretSpacing;
                    }
                }

                string titleStyle = this.Style.GetSvgStyle(Diagram._titleStyleMap);

                if (this.Style.TitleLabelStyle == DiagramLabelStyle.ChordName && this.Title.Length > 1)
                {
                    double modifierSize = this.Style.TitleTextSize * this.Style.TitleTextSizeModRatio;
                    string modifierStyle = String.Format("font-size:{0}pt;", modifierSize);
                    string titleChordNameFormat = SvgConstants.TEXT_CHORDNAME;
                    if (this.Style.Orientation == DiagramOrientation.LeftRight)
                    {
                        titleChordNameFormat = SvgConstants.ROTATED_TEXT_CHORDNAME;
                        titleX -= (this.Style.TitleTextSize - this.Style.TitleGridPadding) / 2.0;
                        titleY -= (this.Style.TitleTextSize + (modifierSize * (this.Title.Length - 1))) / 2.0;
                    }
                    sb.AppendFormat(titleChordNameFormat,
                            titleStyle,
                            titleX,
                            titleY,
                            this.Title[0].ToString(),
                            modifierStyle,
                            this.Title.Substring(1));
                }
                else
                {
                    string titleFormat = SvgConstants.TEXT;
                    if (this.Style.Orientation == DiagramOrientation.LeftRight)
                    {
                        titleFormat = SvgConstants.ROTATED_TEXT;
                        titleX -= (this.Style.TitleTextSize - this.Style.TitleGridPadding) / 2.0;
                        titleY -= (this.Style.TitleTextSize * Title.Length) / 2.0;
                    }
                    sb.AppendFormat(titleFormat,
                            titleStyle,
                            titleX,
                            titleY,
                            this.Title);
                }
            }

            string imageText = sb.ToString().Trim();

            // Apply rotation if necessary
            if (this.Style.Orientation == DiagramOrientation.LeftRight)
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

            return String.Format(SvgConstants.BASE, totalWidth, totalHeight, AppInfo.Watermark, imageText);
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
        public override string Message
        {
            get
            {
                return Resources.Strings.CantResizeDiagramExceptionMessage;
            }
        }

        public CantResizeDiagramException() : base() { }
    }

    public enum ImageMarkupType
    {
        SVG,
        XAML
    }
}
