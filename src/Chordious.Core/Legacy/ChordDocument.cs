// Copyright (c) Jon Thysell <http://jonthysell.com>
// Licensed under the MIT License.

using System;
using System.IO;

namespace Chordious.Core.Legacy
{
    public class ChordDocument
    {
        public static DiagramCollection Load(DiagramStyle parentStyle, Stream inputStream)
        {
            if (parentStyle is null)
            {
                throw new ArgumentNullException(nameof(parentStyle));
            }

            if (inputStream is null)
            {
                throw new ArgumentNullException(nameof(inputStream));
            }

            DiagramCollection collection = new DiagramCollection(parentStyle);

            using (StreamReader sr = new StreamReader(inputStream))
            {
                ChordOptions currentChordOptions = new ChordOptions();

                string line;
                while (null != (line = sr.ReadLine()))
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
                        else if (!string.IsNullOrEmpty(line)) // treat line as a chord
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
            if (chord is null)
            {
                throw new ArgumentNullException(nameof(chord));
            }

            if (chordOptions is null)
            {
                throw new ArgumentNullException(nameof(chordOptions));
            }

            if (parentStyle is null)
            {
                throw new ArgumentNullException(nameof(parentStyle));
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

            DiagramMarkStyleWrapper dmsw = new DiagramMarkStyleWrapper(diagram.Style)
            {
                MarkType = DiagramMarkType.Normal,
                MarkVisible = true,
                MarkRadiusRatio = 0.67,
                MarkShape = DiagramMarkShape.Circle,
                MarkBorderThickness = 2,
                MarkColor = "Black",
                MarkOpacity = 1.0,
                MarkBorderColor = "Black"
            };

            dmsw.MarkType = DiagramMarkType.Muted;
            dmsw.MarkVisible = true;
            dmsw.MarkRadiusRatio = 0.33;
            dmsw.MarkShape = DiagramMarkShape.X;
            dmsw.MarkBorderThickness = 2;
            dmsw.MarkColor = "Black";
            dmsw.MarkOpacity = 1.0;
            dmsw.MarkBorderColor = "Black";

            dmsw.MarkType = DiagramMarkType.Open;
            bool openCircle = (chordOptions.OpenStringType == OpenStringType.Circle);
            dmsw.MarkVisible = openCircle;
            dmsw.MarkRadiusRatio = 0.33;
            dmsw.MarkShape = openCircle ? DiagramMarkShape.Circle : DiagramMarkShape.None;
            dmsw.MarkBorderThickness = 2;
            dmsw.MarkColor = "White";
            dmsw.MarkOpacity = 1.0;
            dmsw.MarkBorderColor = "Black";

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

            diagram.Style.BarreVisible = true;
            diagram.Style.BarreVerticalAlignment = DiagramVerticalAlignment.Middle;
            diagram.Style.BarreArcRatio = 0.5;
            diagram.Style.BarreStack = DiagramBarreStack.UnderMarks;
            diagram.Style.BarreOpacity = 1.0;
            diagram.Style.BarreLineColor = "Black";
            diagram.Style.BarreLineThickness = 2.0;

            // Process barres
            if (chordOptions.BarreType == BarreType.None)
            {
                diagram.Style.BarreVisible = false;
            }

            BarrePosition bp = MarkUtils.AutoBarrePosition(chord.Marks);

            if (chord.Barre == -1)
            {
                if (chordOptions.FullBarres && bp is not null)
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
                if (bp is not null)
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

            if (chordOptions.FullBarres && bp is not null)
            {
                bp = new BarrePosition(bp.Fret, 1, chord.NumStrings);
            }

            if (bp is not null)
            {
                if (chordOptions.BarreType == BarreType.Straight)
                {
                    DiagramBarre straightBarre = diagram.NewBarre(bp);
                    straightBarre.Style.BarreArcRatio = 0;

                }
                else if (chordOptions.BarreType == BarreType.Arc)
                {
                    diagram.NewBarre(bp);
                }
            }

            // Process baseline / fret marker
            diagram.Style.GridNutVisible = false;
            diagram.Style.GridNutRatio = 2.0;

            diagram.Style.FretLabelTextVisible = true;
            diagram.Style.FretLabelTextAlignment = DiagramHorizontalAlignment.Left;
            diagram.Style.FretLabelTextSizeRatio = 0.6;
            diagram.Style.FretLabelTextWidthRatio = 0.5;
            diagram.Style.FretLabelTextStyle = DiagramTextStyle.Regular;

            diagram.Style.FretLabelGridPadding = margin / 4.0;

            diagram.Style.FretLabelTextColor = "Black";
            diagram.Style.FretLabelTextOpacity = 1.0;
            diagram.Style.FretLabelFontFamily = chordOptions.FontFamily;

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

        private static readonly string CommentPrefix = "#";
        private static readonly string OptionsPrefix = "%";
    }
}
