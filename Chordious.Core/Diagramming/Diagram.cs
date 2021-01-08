// 
// Diagram.cs
//  
// Author:
//       Jon Thysell <thysell@gmail.com>
// 
// Copyright (c) 2015, 2016, 2017, 2019, 2020, 2021 Jon Thysell <http://jonthysell.com>
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
using System.Linq;
using System.Text;
using System.Xml;

using Chordious.Core.Resources;

namespace Chordious.Core
{
    public class Diagram
    {
        #region Elements

        public string Title
        {
            get
            {
                return _title;
            }
            set
            {
                _title = value?.Trim();
            }
        }
        private string _title;

        public IEnumerable<DiagramMark> Marks
        {
            get
            {
                return _marks.AsEnumerable();
            }
        }
        private List<DiagramMark> _marks;

        public IEnumerable<DiagramFretLabel> FretLabels
        {
            get
            {
                return _fretLabels.AsEnumerable();
            }
        }
        private List<DiagramFretLabel> _fretLabels;

        public IEnumerable<DiagramBarre> Barres
        {
            get
            {
                return _barres.AsEnumerable();
            }
        }
        private List<DiagramBarre> _barres;

        #endregion

        #region Properties

        public int NumStrings
        {
            get
            {
                return _numStrings;
            }
            set
            {
                Resize(value, NumFrets);             
            }
        }
        private int _numStrings;

        public int NumFrets
        {
            get
            {
                return _numFrets;
            }
            set
            {
                Resize(NumStrings, value);
            }
        }
        private int _numFrets;

        #endregion

        public DiagramStyle Style { get; private set; }

        public Diagram(DiagramStyle parentStyle)
        {
            if (null == parentStyle)
            {
                throw new ArgumentNullException(nameof(parentStyle));
            }

            _marks = new List<DiagramMark>();
            _fretLabels = new List<DiagramFretLabel>();
            _barres = new List<DiagramBarre>();

            Style = new DiagramStyle(parentStyle, LevelKey);

            _numFrets = Style.NewDiagramNumFrets;
            _numStrings = Style.NewDiagramNumStrings;
        }

        public Diagram(DiagramStyle parentStyle, int numStrings, int numFrets) : this(parentStyle)
        {
            NumStrings = numStrings;
            NumFrets = numFrets;
        }

        public Diagram(DiagramStyle parentStyle, XmlReader xmlReader) : this(parentStyle)
        {
            if (null == xmlReader)
            {
                throw new ArgumentNullException(nameof(xmlReader));
            }

            Read(xmlReader);
        }

        public void Read(XmlReader xmlReader)
        {
            if (null == xmlReader)
            {
                throw new ArgumentNullException(nameof(xmlReader));
            }

            using (xmlReader)
            {
                if (xmlReader.IsStartElement())
                {
                    Title = xmlReader.GetAttribute("title");

                    NumStrings = int.Parse(xmlReader.GetAttribute("strings"));
                    NumFrets = int.Parse(xmlReader.GetAttribute("frets"));

                    while (xmlReader.Read())
                    {
                        if (xmlReader.IsStartElement())
                        {
                            switch(xmlReader.Name)
                            {
                                case "mark":
                                    NewMark(xmlReader.ReadSubtree());
                                    break;
                                case "barre":
                                    NewBarre(xmlReader.ReadSubtree());
                                    break;
                                case "fretlabel":
                                    NewFretLabel(xmlReader.ReadSubtree());
                                    break;
                                case "style":
                                    Style.Read(xmlReader.ReadSubtree());
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
                throw new ArgumentNullException(nameof(xmlWriter));
            }

            xmlWriter.WriteStartElement("diagram");

            xmlWriter.WriteAttributeString("title", Title);
            xmlWriter.WriteAttributeString("strings", NumStrings.ToString());
            xmlWriter.WriteAttributeString("frets", NumFrets.ToString());

            foreach (DiagramMark mark in Marks)
            {
                mark.Write(xmlWriter);
            }

            foreach (DiagramBarre barre in Barres)
            {
                barre.Write(xmlWriter);
            }

            foreach (DiagramFretLabel fretLabel in FretLabels)
            {
                fretLabel.Write(xmlWriter);
            }

            Style.Write(xmlWriter);

            xmlWriter.WriteEndElement();
        }

        public Diagram Clone()
        {
            Diagram clone = new Diagram(Style.Parent, NumStrings, NumFrets)
            {
                Title = Title
            };
            clone.Style.CopyFrom(Style);

            foreach (DiagramMark mark in Marks)
            {
                DiagramMark clonedMark = clone.NewMark(mark.Position, mark.Text);
                clonedMark.Style.CopyFrom(mark.Style);
                clonedMark.Type = mark.Type;
            }

            foreach (DiagramBarre barre in Barres)
            {
                DiagramBarre clonedBarre = clone.NewBarre(barre.Position);
                clonedBarre.Style.CopyFrom(barre.Style);
            }

            foreach (DiagramFretLabel fretLabel in FretLabels)
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
                throw new ArgumentOutOfRangeException(nameof(newNumStrings));
            }

            if (newNumFrets < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(newNumFrets));
            }

            List<DiagramMark> endMarks = new List<DiagramMark>();

            // Check through marks
            foreach (DiagramMark mark in Marks)
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
                else if (mark.Position.Fret > newNumFrets)
                {
                    throw new CantResizeDiagramException();
                }
            }

            // Check through fret labels
            foreach (DiagramFretLabel fretLabel in FretLabels)
            {
                if (fretLabel.Position.Fret > newNumFrets)
                {
                    throw new CantResizeDiagramException();
                }
            }

            // Check through barres
            foreach (DiagramBarre barre in Barres)
            {
                if (barre.Position.Fret > newNumFrets ||
                    barre.Position.StartString > newNumStrings ||
                    barre.Position.EndString > newNumStrings)
                {
                    throw new CantResizeDiagramException();
                }
            }

            _numStrings = newNumStrings;
            _numFrets = newNumFrets;

            // Move end marks to new end
            foreach (DiagramMark endMark in endMarks)
            {
                endMark.Position = new MarkPosition(endMark.Position.String, NumFrets + 1);
            }
        }

        public void ClearStyles()
        {
            Style.Clear();

            foreach (DiagramMark mark in Marks)
            {
                mark.Style.Clear();
            }

            foreach (DiagramFretLabel fretLabel in FretLabels)
            {
                fretLabel.Style.Clear();
            }

            foreach (DiagramBarre barre in Barres)
            {
                barre.Style.Clear();
            }
        }

        public IEnumerable<string> GetUsedColors()
        {
            foreach (var color in Style.GetUsedColors(true))
            {
                yield return color;
            }

            foreach (var color in Marks.SelectMany(mark => mark.Style.GetUsedColors(false)))
            {
                yield return color;
            }

            foreach (var color in FretLabels.SelectMany(fretLabel => fretLabel.Style.GetUsedColors(false)))
            {
                yield return color;
            }

            foreach (var color in Barres.SelectMany(barre => barre.Style.GetUsedColors(false)))
            {
                yield return color;
            }
        }

        #endregion

        #region Element Checks

        public bool ValidPosition(ElementPosition position)
        {
            if (null == position)
            {
                throw new ArgumentNullException(nameof(position));
            }

            if (position.GetType() == typeof(MarkPosition))
            {
                MarkPosition mp = (MarkPosition)position;
                return (mp.Fret <= NumFrets + 1) && (mp.String <= NumStrings);
            }
            else if (position.GetType() == typeof(FretLabelPosition))
            {
                FretLabelPosition flp = (FretLabelPosition)position;
                return (flp.Fret <= NumFrets);
            }
            else if (position.GetType() == typeof(BarrePosition))
            {
                BarrePosition bp = (BarrePosition)position;
                return (bp.Fret <= NumFrets && bp.StartString <= NumStrings && bp.EndString <= NumStrings);
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
                throw new ArgumentNullException(nameof(position));
            }

            if (position.GetType() == typeof(MarkPosition))
            {
                foreach (DiagramMark mark in Marks)
                {
                    if (mark.Position == position)
                    {
                        return mark;
                    }
                }
            }
            else if (position.GetType() == typeof(FretLabelPosition))
            {
                foreach (DiagramFretLabel fretLabel in FretLabels)
                {
                    if (fretLabel.Position == position)
                    {
                        return fretLabel;
                    }
                }
            }
            else if (position.GetType() == typeof(BarrePosition))
            {
                foreach (DiagramBarre barre in Barres)
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
                throw new ArgumentNullException(nameof(element));
            }

            if (null == position)
            {
                throw new ArgumentNullException(nameof(position));
            }

            // Warning: Do not use element.Position as this method can be called during
            // element creation before element.Position is defined

            if (position.GetType() == typeof(MarkPosition))
            {
                foreach (DiagramMark mark in Marks)
                {
                    if (mark.Position == position && element != mark)
                    {
                        return false;
                    }
                }
            }
            else if (position.GetType() == typeof(FretLabelPosition))
            {
                foreach (DiagramFretLabel fretLabel in FretLabels)
                {
                    if (fretLabel.Position == position && element != fretLabel)
                    {
                        return false;
                    }
                }
            }
            else if (position.GetType() == typeof(BarrePosition))
            {
                foreach (DiagramBarre barre in Barres)
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
            return Style.TitleVisible && !StringUtils.IsNullOrWhiteSpace(Title);
        }

        public bool HasVisibleFretLabels(FretLabelSide side)
        {
            foreach (DiagramFretLabel fretLabel in FretLabels)
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
            foreach (DiagramFretLabel fretLabel in FretLabels)
            {
                if (fretLabel.IsVisible() && fretLabel.Position.Side == side)
                {
                    double width = fretLabel.Style.FretLabelGridPadding + fretLabel.GetTextWidth();
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
            foreach (DiagramMark mark in Marks)
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
            foreach (DiagramMark mark in Marks)
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
                throw new ArgumentNullException(nameof(position));
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
            _marks.Add(mark);
            return mark;
        }

        internal DiagramMark NewMark(XmlReader xmlReader)
        {
            DiagramMark mark = new DiagramMark(this, xmlReader);
            _marks.Add(mark);
            return mark;
        }

        public bool CanRemoveMarkAt(MarkPosition position)
        {
            if (null == position)
            {
                throw new ArgumentNullException(nameof(position));
            }

            return HasElementAt(position);
        }

        public void RemoveMark(MarkPosition position)
        {
            _marks.Remove((DiagramMark)ElementAt(position));
        }

        public bool CanAddNewFretLabelAt(FretLabelPosition position)
        {
            if (null == position)
            {
                throw new ArgumentNullException(nameof(position));
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
            _fretLabels.Add(fretLabel);
            return fretLabel;
        }

        internal DiagramFretLabel NewFretLabel(XmlReader xmlReader)
        {
            DiagramFretLabel fretLabel = new DiagramFretLabel(this, xmlReader);
            _fretLabels.Add(fretLabel);
            return fretLabel;
        }

        public bool CanRemoveFretLabelAt(FretLabelPosition position)
        {
            if (null == position)
            {
                throw new ArgumentNullException(nameof(position));
            }

            return HasElementAt(position);
        }

        public void RemoveFretLabel(FretLabelPosition position)
        {
            _fretLabels.Remove((DiagramFretLabel)ElementAt(position));
        }

        public bool CanAddNewBarreAt(BarrePosition position)
        {
            if (null == position)
            {
                throw new ArgumentNullException(nameof(position));
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
            _barres.Add(barre);
            return barre;
        }

        internal DiagramBarre NewBarre(XmlReader xmlReader)
        {
            DiagramBarre barre = new DiagramBarre(this, xmlReader);
            _barres.Add(barre);
            return barre;
        }

        public bool CanRemoveBarreAt(BarrePosition position)
        {
            if (null == position)
            {
                throw new ArgumentNullException(nameof(position));
            }

            return HasElementAt(position);
        }

        public void RemoveBarre(BarrePosition position)
        {
            _barres.Remove((DiagramBarre)ElementAt(position));
        }

        #endregion

        #region Grid Positioning

        public double GridLeftEdge()
        {
            double edge = Style.GridMarginLeft;
            if (Style.LabelLayoutModel == DiagramLabelLayoutModel.AddPaddingBoth
                || Style.LabelLayoutModel == DiagramLabelLayoutModel.AddPaddingHorizontal)
            {
                edge += MaxFretLabelWidth(FretLabelSide.Left);
            }

            return edge;
        }

        public double GridTopEdge()
        {
            double edge = Style.GridMarginTop;

            if (Style.LabelLayoutModel == DiagramLabelLayoutModel.AddPaddingBoth
                || Style.LabelLayoutModel == DiagramLabelLayoutModel.AddPaddingVertical)
            {
                if (HasMarksAboveTopEdge())
                {
                    edge += Style.GridFretSpacing;
                }

                if (HasVisibleTitle())
                {
                    edge += Style.TitleGridPadding;
                    edge += Style.Orientation == DiagramOrientation.LeftRight ? Style.TitleTextSize * Title.Length : Style.TitleTextSize;
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
            return Style.GridFretSpacing * NumFrets;
        }

        public double GridWidth()
        {
            return Style.GridStringSpacing * (NumStrings - 1);
        }

        public bool InGrid(double x, double y)
        {
            return (x > GridLeftEdge() && x < GridRightEdge()) && (y > GridTopEdge() && y < GridBottomEdge());
        }

        public ElementPosition GetPosition<T>(double x, double y) where T : ElementPosition
        {
            GetPositionInternal(x, y, out int @string, out int fret);

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

                    if (@string <= Math.Floor((double)NumStrings / 2))
                    {
                        fls = FretLabelSide.Left;
                    }
                    else if (@string > Math.Ceiling((double)NumStrings / 2))
                    {
                        fls = FretLabelSide.Right;
                    }

                    if (null != fls)
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
            if (Style.Orientation == DiagramOrientation.LeftRight)
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

            double fretSpacing = Style.GridFretSpacing;
            double stringSpacing = Style.GridStringSpacing;

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
                throw new ArgumentOutOfRangeException(nameof(range));
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
            if (Style.Orientation == DiagramOrientation.LeftRight)
            {
                return GetWidth();
            }

            return GetHeight();
        }

        private double GetHeight()
        {
            double height = GridBottomEdge() + Style.GridMarginBottom;

            if (Style.LabelLayoutModel == DiagramLabelLayoutModel.AddPaddingBoth
                 || Style.LabelLayoutModel == DiagramLabelLayoutModel.AddPaddingVertical)
            {
                if (HasMarksBelowBottomEdge())
                {
                    height += Style.GridFretSpacing;
                }
            }

            return height;
        }

        public double TotalWidth()
        {
            if (Style.Orientation == DiagramOrientation.LeftRight)
            {
                return GetHeight();
            }

            return GetWidth();
        }

        private double GetWidth()
        {
            double width = GridRightEdge() + Style.GridMarginRight;

            if (Style.LabelLayoutModel == DiagramLabelLayoutModel.AddPaddingBoth
                 || Style.LabelLayoutModel == DiagramLabelLayoutModel.AddPaddingHorizontal)
            {
                width += MaxFretLabelWidth(FretLabelSide.Right);
            }

            return width;
        }

        public string ToImageMarkup(ImageMarkupType type)
        {
            switch (type)
            {
                case ImageMarkupType.SVG:
                    return ToSvg();
                case ImageMarkupType.XAML:
                    return ToXaml();
            }

            return string.Empty;
        }

        private string ToSvg()
        {
            StringBuilder sb = new StringBuilder();

            double totalWidth = GetWidth();
            double totalHeight = GetHeight();

            // Add background

            string baseStyle = Style.GetSvgStyle(_baseStyleMap);

            if (Style.DiagramBorderThickness > 0)
            {
                baseStyle += Style.GetSvgStyle(_baseStyleMapBorder);
            }

            sb.AppendFormat(CultureInfo.InvariantCulture,
                            SvgConstants.RECTANGLE,
                            baseStyle,
                            totalWidth,
                            totalHeight,
                            0,
                            0);

            // Add grid rectangle
            double rectX = GridLeftEdge();
            double rectY = GridTopEdge();

            double rectWidth = GridWidth();
            double rectHeight = GridHeight();

            string gridRectStyle = Style.GetSvgStyle(_gridBaseStyleMap);

            sb.AppendFormat(CultureInfo.InvariantCulture,
                            SvgConstants.RECTANGLE,
                            gridRectStyle,
                            rectWidth,
                            rectHeight,
                            rectX,
                            rectY);

            string lineStyle = Style.GetSvgStyle(_lineStyleMap);

            // Add vertical Lines
            double vSpacing = Style.GridStringSpacing;
            for (int vLine = 1; vLine < NumStrings - 1; vLine++)
            {
                double x = rectX + (vLine * vSpacing);
                double y1 = rectY;
                double y2 = y1 + rectHeight;
                sb.AppendFormat(CultureInfo.InvariantCulture,
                                SvgConstants.LINE,
                                lineStyle,
                                x,
                                y1,
                                x,
                                y2);
            }

            // Add horizontal Lines
            double hSpacing = Style.GridFretSpacing;
            for (int hLine = 1; hLine < NumFrets; hLine++)
            {
                double x1 = rectX;
                double x2 = rectX + rectWidth;
                double y = rectY + (hLine * hSpacing);

                sb.AppendFormat(CultureInfo.InvariantCulture,
                                SvgConstants.LINE,
                                lineStyle,
                                x1,
                                y,
                                x2,
                                y);
            }

            // Add nut
            if (Style.GridNutVisible)
            {
                double strokeCorrection = Style.GridLineThickness / 2.0;

                double x1 = rectX - strokeCorrection;
                double x2 = rectX + rectWidth + strokeCorrection;
                double y = rectY - strokeCorrection;

                string nutStyle = Style.GetSvgStyle(_nutStyleMap);
                nutStyle += string.Format(CultureInfo.InvariantCulture, "stroke-width:{0};", Style.GridLineThickness * Style.GridNutRatio);

                sb.AppendFormat(CultureInfo.InvariantCulture,
                                SvgConstants.LINE,
                                nutStyle,
                                x1,
                                y,
                                x2,
                                y);
            }

            // Add barres underneath the marks
            foreach (DiagramBarre barre in Barres)
            {
                if (barre.Style.BarreStack == DiagramBarreStack.UnderMarks)
                {
                    sb.Append(barre.ToSvg());
                }
            }

            // Add marks
            foreach (DiagramMark mark in Marks)
            {
                sb.Append(mark.ToSvg());
            }

            // Add barres on top of the marks
            foreach (DiagramBarre barre in Barres)
            {
                if (barre.Style.BarreStack == DiagramBarreStack.OverMarks)
                {
                    sb.Append(barre.ToSvg());
                }
            }

            // Add fret labels
            foreach (DiagramFretLabel fretLabel in FretLabels)
            {
                sb.Append(fretLabel.ToSvg());
            }

            // Add title
            if (HasVisibleTitle())
            {
                double titleX = rectX + (rectWidth / 2.0);
                double titleY = rectY - Style.TitleGridPadding;

                if (Style.Orientation == DiagramOrientation.UpDown)
                {
                    switch (Style.TitleTextAlignment)
                    {
                        case DiagramHorizontalAlignment.Left:
                            titleX = rectX;
                            break;
                        case DiagramHorizontalAlignment.Right:
                            titleX = rectX + rectWidth;
                            break;
                    }
                }

                if (Style.LabelLayoutModel == DiagramLabelLayoutModel.AddPaddingBoth
                || Style.LabelLayoutModel == DiagramLabelLayoutModel.AddPaddingVertical)
                {
                    if (HasMarksAboveTopEdge())
                    {
                        titleY -= Style.GridFretSpacing;
                    }
                }

                string titleStyle = Style.GetSvgStyle(_titleStyleMap);

                if (Style.TitleLabelStyle == DiagramLabelStyle.ChordName && Title.Length > 1)
                {
                    double modifierSize = Style.TitleTextSize * Style.TitleTextSizeModRatio;
                    string modifierStyle = string.Format(CultureInfo.InvariantCulture, "font-size:{0}pt;", modifierSize);
                    string titleChordNameFormat = SvgConstants.TEXT_CHORDNAME;
                    if (Style.Orientation == DiagramOrientation.LeftRight)
                    {
                        titleChordNameFormat = SvgConstants.ROTATED_TEXT_CHORDNAME;
                        titleX -= (Style.TitleTextSize - Style.TitleGridPadding) / 2.0;
                        titleY -= (Style.TitleTextSize + (modifierSize * (Title.Length - 1))) / 2.0;
                    }
                    sb.AppendFormat(CultureInfo.InvariantCulture,
                            titleChordNameFormat,
                            titleStyle,
                            titleX,
                            titleY,
                            Title[0].ToString(),
                            modifierStyle,
                            Title.Substring(1));
                }
                else
                {
                    string titleFormat = SvgConstants.TEXT;
                    if (Style.Orientation == DiagramOrientation.LeftRight)
                    {
                        titleFormat = SvgConstants.ROTATED_TEXT;
                        titleX -= (Style.TitleTextSize - Style.TitleGridPadding) / 2.0;
                        titleY -= (Style.TitleTextSize * Title.Length) / 2.0;
                    }
                    sb.AppendFormat(CultureInfo.InvariantCulture,
                            titleFormat,
                            titleStyle,
                            titleX,
                            titleY,
                            Title);
                }
            }

            string imageText = sb.ToString().Trim();

            // Apply rotation if necessary
            if (Style.Orientation == DiagramOrientation.LeftRight)
            {
                imageText = string.Format(CultureInfo.InvariantCulture,
                    SvgConstants.ROTATE,
                    imageText,
                    -90,
                    totalWidth / 2.0,
                    totalHeight / 2.0);

                // Need to swap here since we used Gets instead of Totals above
                double temp = totalHeight;
                totalHeight = totalWidth;
                totalWidth = temp;
            }

            return string.Format(CultureInfo.InvariantCulture, SvgConstants.BASE, totalWidth, totalHeight, AppInfo.Watermark, imageText);
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

        public const string LevelKey = "Diagram";
    }

    public class CantResizeDiagramException : ChordiousException
    {
        public override string Message
        {
            get
            {
                return Strings.CantResizeDiagramExceptionMessage;
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
