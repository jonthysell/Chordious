// 
// ChordDocument.cs
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
using System.IO;

using com.jonthysell.Chordious.Core;

namespace com.jonthysell.Chordious.Core.Legacy
{
    public class ChordDocument
    {
        public static DiagramCollection Load(DiagramStyle parentStyle, Stream inputStream)
        {
            if (null == parentStyle)
            {
                throw new ArgumentNullException("parentStyle");
            }

            if (null == inputStream)
            {
                throw new ArgumentNullException("inputStream");
            }

            DiagramCollection collection = new DiagramCollection(parentStyle);

            using (StreamReader sr = new StreamReader(inputStream))
            {
                ChordOptions currentChordOptions = new ChordOptions();

                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    line = line.Trim();
                    try
                    {
                        if (line.StartsWith(CommentPrefix)) // line is a comment
                        {
                            // ignore
                        }
                        else if (line.StartsWith(OptionsPrefix)) // line modifies the options
                        {
                            currentChordOptions = new ChordOptions(line);
                        }
                        else if (!String.IsNullOrEmpty(line)) // treat line as a chord
                        {
                            Chord chord = new Chord(line);

                            Diagram diagram = GetDiagram(chord, currentChordOptions, collection.Style);
                            collection.Add(diagram);
                        }
                    }
                    catch (Exception) { }
                }
            }

            return collection;
        }

        internal static Diagram GetDiagram(Chord chord, ChordOptions chordOptions, DiagramStyle parentStyle)
        {
            if (null == chord)
            {
                throw new ArgumentNullException("chord");
            }

            if (null == chordOptions)
            {
                throw new ArgumentNullException("chordOptions");
            }

            if (null == parentStyle)
            {
                throw new ArgumentNullException("parentStyle");
            }

            Diagram diagram = new Diagram(parentStyle, chord.NumStrings, chord.NumFrets);

            // Setup base
            diagram.Style.Orientation = DiagramOrientation.UpDown;
            diagram.Style.LabelLayoutModel = DiagramLabelLayoutModel.AddPaddingVertical;
            diagram.Style.DiagramColor = "White";
            diagram.Style.DiagramOpacity = 0;
            diagram.Style.DiagramBorderColor = "Black";
            diagram.Style.DiagramBorderThickness = 0;

            // Process title
            diagram.Title = chord.Title;
            diagram.Style.TitleLabelStyle = DiagramLabelStyle.ChordName;
            diagram.Style.TitleTextSize = chordOptions.FontSize * (19.0 / 24.0);
            diagram.Style.TitleVisible = true;
            diagram.Style.TitleTextSizeModRatio = 0.75;
            diagram.Style.TitleGridPadding = diagram.Style.TitleTextSize / 2.0;
            diagram.Style.TitleFontFamily = chordOptions.FontFamily;
            diagram.Style.TitleColor = "Black";
            diagram.Style.TitleOpacity = 1;
            diagram.Style.TitleTextAlignment = DiagramHorizontalAlignment.Center;

            // Process grid
            double margin = chordOptions.Margin;
            diagram.Style.GridMargin = margin;
            diagram.Style.GridMarginLeft = margin;
            diagram.Style.GridMarginRight = margin;
            diagram.Style.GridMarginTop = margin;
            diagram.Style.GridMarginBottom = margin;

            double titleHeight = diagram.Style.TitleTextSize;

            double rectWidth = chordOptions.Width - (2.0 * chordOptions.Margin);
            double rectHeight = chordOptions.Height - titleHeight - (2.0 * chordOptions.Margin);

            diagram.Style.GridLineThickness = chordOptions.StrokeWidth;
            diagram.Style.GridStringSpacing = rectWidth / (chord.NumStrings - 1);
            diagram.Style.GridFretSpacing = rectHeight / chord.NumFrets;

            diagram.Style.GridLineColor = "White";
            diagram.Style.GridOpacity = 0;
            diagram.Style.GridLineColor = "Black";

            DiagramTextStyle titleTextStyle = DiagramTextStyle.Regular;
            switch (chordOptions.FontStyle)
            {
                case FontStyle.Bold:
                    titleTextStyle = DiagramTextStyle.Bold;
                    break;
                case FontStyle.Italic:
                    titleTextStyle = DiagramTextStyle.Italic;
                    break;
                case FontStyle.BoldItalic:
                    titleTextStyle = DiagramTextStyle.BoldItalic;
                    break;
            }
            diagram.Style.TitleTextStyle = titleTextStyle;

            // Process marks

            diagram.Style.MarkVisibleSet(true);
            diagram.Style.MarkRadiusRatioSet(0.67);
            diagram.Style.MarkShapeSet(DiagramMarkShape.Circle);
            diagram.Style.MarkBorderThicknessSet(2);
            diagram.Style.MarkColorSet("Black");
            diagram.Style.MarkOpacitySet(1.0);
            diagram.Style.MarkBorderColorSet("Black");

            diagram.Style.MarkVisibleSet(true, DiagramMarkType.Muted);
            diagram.Style.MarkRadiusRatioSet(0.33, DiagramMarkType.Muted);
            diagram.Style.MarkShapeSet(DiagramMarkShape.X, DiagramMarkType.Muted);
            diagram.Style.MarkBorderThicknessSet(2, DiagramMarkType.Muted);
            diagram.Style.MarkColorSet("Black", DiagramMarkType.Muted);
            diagram.Style.MarkOpacitySet(1.0, DiagramMarkType.Muted);
            diagram.Style.MarkBorderColorSet("Black", DiagramMarkType.Muted);

            bool openCircle = (chordOptions.OpenStringType == OpenStringType.Circle);
            diagram.Style.MarkVisibleSet(openCircle, DiagramMarkType.Open);
            diagram.Style.MarkRadiusRatioSet(0.33, DiagramMarkType.Open);
            diagram.Style.MarkShapeSet(openCircle ? DiagramMarkShape.Circle : DiagramMarkShape.None, DiagramMarkType.Open);
            diagram.Style.MarkBorderThicknessSet(2, DiagramMarkType.Open);
            diagram.Style.MarkColorSet("White", DiagramMarkType.Open);
            diagram.Style.MarkOpacitySet(1.0, DiagramMarkType.Open);
            diagram.Style.MarkBorderColorSet("Black", DiagramMarkType.Open);

            for (int str = 0; str < chord.Marks.Length; str++)
            {
                int fret = chord.Marks[str];

                MarkPosition mp = new MarkPosition(str + 1, Math.Max(fret, 0));

                if (fret == -1)
                {
                    DiagramMark mutedMark = diagram.NewMark(mp);
                    mutedMark.Type = DiagramMarkType.Muted;
                }
                else if (fret == 0 && openCircle)
                {
                    DiagramMark openMark = diagram.NewMark(mp);
                    openMark.Type = DiagramMarkType.Open;
                }
                else if (fret > 0)
                {
                    diagram.NewMark(mp);
                }
            }

            diagram.Style.BarreVisibleSet(true);
            diagram.Style.BarreVerticalAlignmentSet(DiagramVerticalAlignment.Middle);
            diagram.Style.BarreArcRatioSet(0.5);
            diagram.Style.BarreStackSet(DiagramBarreStack.UnderMarks);
            diagram.Style.BarreOpacitySet(1.0);
            diagram.Style.BarreLineColorSet("Black");
            diagram.Style.BarreLineThicknessSet(2.0);

            // Process barres
            if (chordOptions.BarreType == BarreType.None)
            {
                diagram.Style.BarreVisibleSet(false);
            }

            BarrePosition bp = MarkUtils.AutoBarrePosition(chord.Marks); ;

            if (chord.Barre == -1)
            {
                if (chordOptions.FullBarres && null != bp)
                {
                    bp = new BarrePosition(bp.Fret, 1, chord.NumStrings);
                }
            }
            else if (chord.Barre == 0)
            {
                bp = null;
            }
            else if (chord.Barre > 0)
            {
                if (null != bp)
                {
                    if (chordOptions.FullBarres)
                    {
                        bp = new BarrePosition(bp.Fret, 1, chord.NumStrings);
                    }
                    else
                    {
                        bp = new BarrePosition(chord.Barre, bp.StartString, bp.EndString);
                    }
                }
            }

            if (chordOptions.FullBarres && null != bp)
            {
                bp = new BarrePosition(bp.Fret, 1, chord.NumStrings);
            }

            if (null != bp)
            {
                if (chordOptions.BarreType == BarreType.Straight)
                {
                    DiagramBarre straightBarre = diagram.NewBarre(bp);
                    straightBarre.ArcRatio = 0;
                
                }
                else if (chordOptions.BarreType == BarreType.Arc)
                {
                    diagram.NewBarre(bp);
                }
            }

            // Process baseline / fret marker
            diagram.Style.GridNutVisible = false;
            diagram.Style.GridNutRatio = 2.0;

            diagram.Style.FretLabelTextVisibleSet(true);
            diagram.Style.FretLabelTextAlignmentSet(DiagramHorizontalAlignment.Left);
            diagram.Style.FretLabelTextSizeRatioSet(0.6);
            diagram.Style.FretLabelTextWidthRatioSet(0.5);
            diagram.Style.FretLabelTextStyleSet(DiagramTextStyle.Regular);

            double padding = margin / 4.0;
            diagram.Style.FretLabelGridPaddingSet(padding);

            diagram.Style.FretLabelTextColorSet("Black");
            diagram.Style.FretLabelTextOpacitySet(1.0);
            diagram.Style.FretLabelFontFamilySet(chordOptions.FontFamily);

            int baseLine = chord.BaseLine;

            if (baseLine == 0)
            {
                diagram.Style.GridNutVisible = true;
            }
            else if (chord.BaseLine > 1)
            {
                FretLabelPosition flp = new FretLabelPosition(FretLabelSide.Right, 1);
                diagram.NewFretLabel(flp, baseLine.ToString());
            }

            return diagram;
        }

        private static string CommentPrefix = "#";
        private static string OptionsPrefix = "%";
    }
}
