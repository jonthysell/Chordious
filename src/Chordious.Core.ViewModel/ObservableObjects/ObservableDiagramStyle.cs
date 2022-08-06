// Copyright (c) Jon Thysell <http://jonthysell.com>
// Licensed under the MIT License.

using System;
using System.Collections.ObjectModel;
using System.ComponentModel;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

using Chordious.Core.ViewModel.Resources;

namespace Chordious.Core.ViewModel
{
    public class ObservableDiagramStyle : ObservableObject
    {
        public string Level
        {
            get
            {
                return Style.FriendlyLevel;
            }
        }

        public string SummaryLabel
        {
            get
            {
                return string.Format(Strings.DiagramStyleSummaryLabelFormat, Style.FriendlyLevel);
            }
        }

        public string SummaryToolTip
        {
            get
            {
                return string.Format(Strings.DiagramStyleSummaryToolTipFormat, Style.FriendlyLevel);
            }
        }

        public string Summary
        {
            get
            {
                return Style.Summary;
            }
        }

        public int LocalCount
        {
            get
            {
                return Style.LocalCount;
            }
        }

        public bool IsEditable
        {
            get
            {
                return !Style.ReadOnly;
            }
        }

        #region Diagram

        public static string DiagramGroupLabel
        {
            get
            {
                return Strings.DiagramStyleDiagramGroupLabel;
            }
        }

        #region Layout

        public static string DiagramLayoutGroupLabel
        {
            get
            {
                return Strings.DiagramStyleDiagramLayoutGroupLabel;
            }
        }

        public bool OrientationIsLocal
        {
            get
            {
                return Style.OrientationIsLocal;
            }
            set
            {
                try
                {
                    Style.OrientationIsLocal = value;
                    OnPropertyChanged(nameof(OrientationIsLocal));
                    OnPropertyChanged(nameof(SelectedOrientationIndex));
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }
        }

        public static string SelectedOrientationLabel
        {
            get
            {
                return Strings.DiagramStyleOrientationLabel;
            }
        }

        public static string SelectedOrientationToolTip
        {
            get
            {
                return Strings.DiagramStyleOrientationToolTip;
            }
        }

        public int SelectedOrientationIndex
        {
            get
            {
                return (int)Style.Orientation;
            }
            set
            {
                try
                {
                    Style.Orientation = (DiagramOrientation)(value);
                    OnPropertyChanged(nameof(SelectedOrientationIndex));
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }
        }

        public static ObservableCollection<string> Orientations
        {
            get
            {
                return ObservableEnums.GetOrientations();
            }
        }

        public bool LabelLayoutModelIsLocal
        {
            get
            {
                return Style.LabelLayoutModelIsLocal;
            }
            set
            {
                try
                {
                    Style.LabelLayoutModelIsLocal = value;
                    OnPropertyChanged(nameof(LabelLayoutModelIsLocal));
                    OnPropertyChanged(nameof(SelectedLabelLayoutModelIndex));
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }
        }

        public static string SelectedLabelLayoutModelLabel
        {
            get
            {
                return Strings.DiagramStyleLabelLayoutModelLabel;
            }
        }

        public static string SelectedLabelLayoutModelToolTip
        {
            get
            {
                return Strings.DiagramStyleLabelLayoutModelToolTip;
            }
        }

        public int SelectedLabelLayoutModelIndex
        {
            get
            {
                return (int)Style.LabelLayoutModel;
            }
            set
            {
                try
                {
                    Style.LabelLayoutModel = (DiagramLabelLayoutModel)(value);
                    OnPropertyChanged(nameof(SelectedLabelLayoutModelIndex));
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }
        }

        public static ObservableCollection<string> LabelLayoutModels
        {
            get
            {
                return ObservableEnums.GetLabelLayoutModels();
            }
        }

        #endregion

        #region Background

        public static string DiagramBackgroundGroupLabel
        {
            get
            {
                return Strings.DiagramStyleDiagramBackgroundGroupLabel;
            }
        }

        public bool DiagramColorIsLocal
        {
            get
            {
                return Style.DiagramColorIsLocal;
            }
            set
            {
                try
                {
                    Style.DiagramColorIsLocal = value;
                    OnPropertyChanged(nameof(DiagramColorIsLocal));
                    OnPropertyChanged(nameof(DiagramColor));
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }
        }

        public static string DiagramColorLabel
        {
            get
            {
                return Strings.DiagramStyleDiagramColorLabel;
            }
        }

        public static string DiagramColorToolTip
        {
            get
            {
                return Strings.DiagramStyleDiagramColorToolTip;
            }
        }

        public string DiagramColor
        {
            get
            {
                return Style.DiagramColor;
            }
            set
            {
                try
                {
                    if (value is not null)
                    {
                        Style.DiagramColor = value;
                        OnPropertyChanged(nameof(DiagramColor));
                    }
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }
        }

        public bool DiagramOpacityIsLocal
        {
            get
            {
                return Style.DiagramOpacityIsLocal;
            }
            set
            {
                try
                {
                    Style.DiagramOpacityIsLocal = value;
                    OnPropertyChanged(nameof(DiagramOpacityIsLocal));
                    OnPropertyChanged(nameof(DiagramOpacity));
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }
        }

        public static string DiagramOpacityLabel
        {
            get
            {
                return Strings.DiagramStyleDiagramOpacityLabel;
            }
        }

        public static string DiagramOpacityToolTip
        {
            get
            {
                return Strings.DiagramStyleDiagramOpacityToolTip;
            }
        }

        public double DiagramOpacity
        {
            get
            {
                return Style.DiagramOpacity;
            }
            set
            {
                try
                {
                    Style.DiagramOpacity = value;
                    OnPropertyChanged(nameof(DiagramOpacity));
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }
        }

        #endregion

        #region Border

        public static string DiagramBorderGroupLabel
        {
            get
            {
                return Strings.DiagramStyleDiagramBorderGroupLabel;
            }
        }

        public bool DiagramBorderColorIsLocal
        {
            get
            {
                return Style.DiagramBorderColorIsLocal;
            }
            set
            {
                try
                {
                    Style.DiagramBorderColorIsLocal = value;
                    OnPropertyChanged(nameof(DiagramBorderColorIsLocal));
                    OnPropertyChanged(nameof(DiagramBorderColor));
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }
        }

        public static string DiagramBorderColorLabel
        {
            get
            {
                return Strings.DiagramStyleDiagramBorderColorLabel;
            }
        }

        public static string DiagramBorderColorToolTip
        {
            get
            {
                return Strings.DiagramStyleDiagramBorderColorToolTip;
            }
        }

        public string DiagramBorderColor
        {
            get
            {
                return Style.DiagramBorderColor;
            }
            set
            {
                try
                {
                    if (value is not null)
                    {
                        Style.DiagramBorderColor = value;
                        OnPropertyChanged(nameof(DiagramBorderColor));
                    }
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }
        }

        public bool DiagramBorderThicknessIsLocal
        {
            get
            {
                return Style.DiagramBorderThicknessIsLocal;
            }
            set
            {
                try
                {
                    Style.DiagramBorderThicknessIsLocal = value;
                    OnPropertyChanged(nameof(DiagramBorderThicknessIsLocal));
                    OnPropertyChanged(nameof(DiagramBorderThickness));
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }
        }

        public static string DiagramBorderThicknessLabel
        {
            get
            {
                return Strings.DiagramStyleDiagramBorderThicknessLabel;
            }
        }

        public static string DiagramBorderThicknessToolTip
        {
            get
            {
                return Strings.DiagramStyleDiagramBorderThicknessToolTip;
            }
        }

        public double DiagramBorderThickness
        {
            get
            {
                return Style.DiagramBorderThickness;
            }
            set
            {
                try
                {
                    Style.DiagramBorderThickness = value;
                    OnPropertyChanged(nameof(DiagramBorderThickness));
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }
        }

        #endregion

        #region NewDiagram

        public static string NewDiagramGroupLabel
        {
            get
            {
                return Strings.DiagramStyleNewDiagramGroupLabel;
            }
        }

        public bool NewDiagramNumStringsIsLocal
        {
            get
            {
                return Style.NewDiagramNumStringsIsLocal;
            }
            set
            {
                try
                {
                    Style.NewDiagramNumStringsIsLocal = value;
                    OnPropertyChanged(nameof(NewDiagramNumStringsIsLocal));
                    OnPropertyChanged(nameof(NewDiagramNumStrings));
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }
        }

        public static string NewDiagramNumStringsLabel
        {
            get
            {
                return Strings.DiagramStyleNewDiagramNumStringsLabel;
            }
        }

        public static string NewDiagramNumStringsToolTip
        {
            get
            {
                return Strings.DiagramStyleNewDiagramNumStringsToolTip;
            }
        }

        public int NewDiagramNumStrings
        {
            get
            {
                return Style.NewDiagramNumStrings;
            }
            set
            {
                try
                {
                    Style.NewDiagramNumStrings = value;
                    OnPropertyChanged(nameof(NewDiagramNumStrings));
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }
        }

        public bool NewDiagramNumFretsIsLocal
        {
            get
            {
                return Style.NewDiagramNumFretsIsLocal;
            }
            set
            {
                try
                {
                    Style.NewDiagramNumFretsIsLocal = value;
                    OnPropertyChanged(nameof(NewDiagramNumFretsIsLocal));
                    OnPropertyChanged(nameof(NewDiagramNumFrets));
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }
        }

        public static string NewDiagramNumFretsLabel
        {
            get
            {
                return Strings.DiagramStyleNewDiagramNumFretsLabel;
            }
        }

        public static string NewDiagramNumFretsToolTip
        {
            get
            {
                return Strings.DiagramStyleNewDiagramNumFretsToolTip;
            }
        }

        public int NewDiagramNumFrets
        {
            get
            {
                return Style.NewDiagramNumFrets;
            }
            set
            {
                try
                {
                    Style.NewDiagramNumFrets = value;
                    OnPropertyChanged(nameof(NewDiagramNumFrets));
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }
        }

        #endregion

        #endregion

        #region Grid

        public static string GridGroupLabel
        {
            get
            {
                return Strings.DiagramStyleGridGroupLabel;
            }
        }

        public static string GridMarginGroupLabel
        {
            get
            {
                return Strings.DiagramStyleGridMarginGroupLabel;
            }
        }

        public bool GridMarginIsLocal
        {
            get
            {
                return Style.GridMarginIsLocal;
            }
            set
            {
                try
                {
                    Style.GridMarginIsLocal = value;
                    OnPropertyChanged(nameof(GridMarginIsLocal));
                    OnPropertyChanged(nameof(GridMargin));
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }
        }

        public static string GridMarginLabel
        {
            get
            {
                return Strings.DiagramStyleGridMarginLabel;
            }
        }

        public static string GridMarginToolTip
        {
            get
            {
                return Strings.DiagramStyleGridMarginToolTip;
            }
        }

        public double GridMargin
        {
            get
            {
                return Style.GridMargin;
            }
            set
            {
                try
                {
                    Style.GridMargin = value;
                    OnPropertyChanged(nameof(GridMargin));
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }
        }

        public bool GridMarginLeftIsLocal
        {
            get
            {
                return Style.GridMarginLeftIsLocal;
            }
            set
            {
                try
                {
                    Style.GridMarginLeftIsLocal = value;
                    OnPropertyChanged(nameof(GridMarginLeftIsLocal));
                    OnPropertyChanged(nameof(GridMarginLeft));
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }
        }

        public static string GridMarginLeftLabel
        {
            get
            {
                return Strings.DiagramStyleGridMarginLeftLabel;
            }
        }

        public static string GridMarginLeftToolTip
        {
            get
            {
                return Strings.DiagramStyleGridMarginLeftToolTip;
            }
        }

        public double GridMarginLeft
        {
            get
            {
                return Style.GridMarginLeft;
            }
            set
            {
                try
                {
                    Style.GridMarginLeft = value;
                    OnPropertyChanged(nameof(GridMarginLeft));
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }
        }

        public bool GridMarginRightIsLocal
        {
            get
            {
                return Style.GridMarginRightIsLocal;
            }
            set
            {
                try
                {
                    Style.GridMarginRightIsLocal = value;
                    OnPropertyChanged(nameof(GridMarginRightIsLocal));
                    OnPropertyChanged(nameof(GridMarginRight));
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }
        }

        public static string GridMarginRightLabel
        {
            get
            {
                return Strings.DiagramStyleGridMarginRightLabel;
            }
        }

        public static string GridMarginRightToolTip
        {
            get
            {
                return Strings.DiagramStyleGridMarginRightToolTip;
            }
        }

        public double GridMarginRight
        {
            get
            {
                return Style.GridMarginRight;
            }
            set
            {
                try
                {
                    Style.GridMarginRight = value;
                    OnPropertyChanged(nameof(GridMarginRight));
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }
        }

        public bool GridMarginTopIsLocal
        {
            get
            {
                return Style.GridMarginTopIsLocal;
            }
            set
            {
                try
                {
                    Style.GridMarginTopIsLocal = value;
                    OnPropertyChanged(nameof(GridMarginTopIsLocal));
                    OnPropertyChanged(nameof(GridMarginTop));
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }
        }

        public static string GridMarginTopLabel
        {
            get
            {
                return Strings.DiagramStyleGridMarginTopLabel;
            }
        }

        public static string GridMarginTopToolTip
        {
            get
            {
                return Strings.DiagramStyleGridMarginTopToolTip;
            }
        }

        public double GridMarginTop
        {
            get
            {
                return Style.GridMarginTop;
            }
            set
            {
                try
                {
                    Style.GridMarginTop = value;
                    OnPropertyChanged(nameof(GridMarginTop));
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }
        }

        public bool GridMarginBottomIsLocal
        {
            get
            {
                return Style.GridMarginBottomIsLocal;
            }
            set
            {
                try
                {
                    Style.GridMarginBottomIsLocal = value;
                    OnPropertyChanged(nameof(GridMarginBottomIsLocal));
                    OnPropertyChanged(nameof(GridMarginBottom));
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }
        }

        public static string GridMarginBottomLabel
        {
            get
            {
                return Strings.DiagramStyleGridMarginBottomLabel;
            }
        }

        public static string GridMarginBottomToolTip
        {
            get
            {
                return Strings.DiagramStyleGridMarginBottomToolTip;
            }
        }

        public double GridMarginBottom
        {
            get
            {
                return Style.GridMarginBottom;
            }
            set
            {
                try
                {
                    Style.GridMarginBottom = value;
                    OnPropertyChanged(nameof(GridMarginBottom));
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }
        }

        public static string GridSpacingGroupLabel
        {
            get
            {
                return Strings.DiagramStyleGridSpacingGroupLabel;
            }
        }

        public bool GridFretSpacingIsLocal
        {
            get
            {
                return Style.GridFretSpacingIsLocal;
            }
            set
            {
                try
                {
                    Style.GridFretSpacingIsLocal = value;
                    OnPropertyChanged(nameof(GridFretSpacingIsLocal));
                    OnPropertyChanged(nameof(GridFretSpacing));
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }
        }

        public static string GridFretSpacingLabel
        {
            get
            {
                return Strings.DiagramStyleGridFretSpacingLabel;
            }
        }

        public static string GridFretSpacingToolTip
        {
            get
            {
                return Strings.DiagramStyleGridFretSpacingToolTip;
            }
        }

        public double GridFretSpacing
        {
            get
            {
                return Style.GridFretSpacing;
            }
            set
            {
                try
                {
                    Style.GridFretSpacing = value;
                    OnPropertyChanged(nameof(GridFretSpacing));
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }
        }

        public bool GridStringSpacingIsLocal
        {
            get
            {
                return Style.GridStringSpacingIsLocal;
            }
            set
            {
                try
                {
                    Style.GridStringSpacingIsLocal = value;
                    OnPropertyChanged(nameof(GridStringSpacingIsLocal));
                    OnPropertyChanged(nameof(GridStringSpacing));
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }
        }

        public static string GridStringSpacingLabel
        {
            get
            {
                return Strings.DiagramStyleGridStringSpacingLabel;
            }
        }

        public static string GridStringSpacingToolTip
        {
            get
            {
                return Strings.DiagramStyleGridStringSpacingToolTip;
            }
        }

        public double GridStringSpacing
        {
            get
            {
                return Style.GridStringSpacing;
            }
            set
            {
                try
                {
                    Style.GridStringSpacing = value;
                    OnPropertyChanged(nameof(GridStringSpacing));
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }
        }

        public static string GridBackgroundGroupLabel
        {
            get
            {
                return Strings.DiagramStyleGridBackgroundGroupLabel;
            }
        }

        public bool GridColorIsLocal
        {
            get
            {
                return Style.GridColorIsLocal;
            }
            set
            {
                try
                {
                    Style.GridColorIsLocal = value;
                    OnPropertyChanged(nameof(GridColorIsLocal));
                    OnPropertyChanged(nameof(GridColor));
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }
        }

        public static string GridColorLabel
        {
            get
            {
                return Strings.DiagramStyleGridColorLabel;
            }
        }

        public static string GridColorToolTip
        {
            get
            {
                return Strings.DiagramStyleGridColorToolTip;
            }
        }

        public string GridColor
        {
            get
            {
                return Style.GridColor;
            }
            set
            {
                try
                {
                    if (value is not null)
                    {
                        Style.GridColor = value;
                        OnPropertyChanged(nameof(GridColor));
                    }
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }
        }

        public bool GridOpacityIsLocal
        {
            get
            {
                return Style.GridOpacityIsLocal;
            }
            set
            {
                try
                {
                    Style.GridOpacityIsLocal = value;
                    OnPropertyChanged(nameof(GridOpacityIsLocal));
                    OnPropertyChanged(nameof(GridOpacity));
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }
        }

        public static string GridOpacityLabel
        {
            get
            {
                return Strings.DiagramStyleGridOpacityLabel;
            }
        }

        public static string GridOpacityToolTip
        {
            get
            {
                return Strings.DiagramStyleGridOpacityToolTip;
            }
        }

        public double GridOpacity
        {
            get
            {
                return Style.GridOpacity;
            }
            set
            {
                try
                {
                    Style.GridOpacity = value;
                    OnPropertyChanged(nameof(GridOpacity));
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }
        }

        public static string GridLineGroupLabel
        {
            get
            {
                return Strings.DiagramStyleGridLineGroupLabel;
            }
        }

        public bool GridLineColorIsLocal
        {
            get
            {
                return Style.GridLineColorIsLocal;
            }
            set
            {
                try
                {
                    Style.GridLineColorIsLocal = value;
                    OnPropertyChanged(nameof(GridLineColorIsLocal));
                    OnPropertyChanged(nameof(GridLineColor));
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }
        }

        public static string GridLineColorLabel
        {
            get
            {
                return Strings.DiagramStyleGridLineColorLabel;
            }
        }

        public static string GridLineColorToolTip
        {
            get
            {
                return Strings.DiagramStyleGridLineColorToolTip;
            }
        }

        public string GridLineColor
        {
            get
            {
                return Style.GridLineColor;
            }
            set
            {
                try
                {
                    if (value is not null)
                    {
                        Style.GridLineColor = value;
                        OnPropertyChanged(nameof(GridLineColor));
                    }
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }
        }

        public bool GridLineThicknessIsLocal
        {
            get
            {
                return Style.GridLineThicknessIsLocal;
            }
            set
            {
                try
                {
                    Style.GridLineThicknessIsLocal = value;
                    OnPropertyChanged(nameof(GridLineThicknessIsLocal));
                    OnPropertyChanged(nameof(GridLineThickness));
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }
        }

        public static string GridLineThicknessLabel
        {
            get
            {
                return Strings.DiagramStyleGridLineThicknessLabel;
            }
        }

        public static string GridLineThicknessToolTip
        {
            get
            {
                return Strings.DiagramStyleGridLineThicknessToolTip;
            }
        }

        public double GridLineThickness
        {
            get
            {
                return Style.GridLineThickness;
            }
            set
            {
                try
                {
                    Style.GridLineThickness = value;
                    OnPropertyChanged(nameof(GridLineThickness));
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }
        }

        public static string GridNutGroupLabel
        {
            get
            {
                return Strings.DiagramStyleGridNutGroupLabel;
            }
        }

        public bool GridNutVisibleIsLocal
        {
            get
            {
                return Style.GridNutVisibleIsLocal;
            }
            set
            {
                try
                {
                    Style.GridNutVisibleIsLocal = value;
                    OnPropertyChanged(nameof(GridNutVisibleIsLocal));
                    OnPropertyChanged(nameof(GridNutVisible));
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }
        }

        public static string GridNutVisibleLabel
        {
            get
            {
                return Strings.DiagramStyleGridNutVisibleLabel;
            }
        }

        public static string GridNutVisibleToolTip
        {
            get
            {
                return Strings.DiagramStyleGridNutVisibleToolTip;
            }
        }

        public bool GridNutVisible
        {
            get
            {
                return Style.GridNutVisible;
            }
            set
            {
                try
                {
                    Style.GridNutVisible = value;
                    OnPropertyChanged(nameof(GridNutVisible));
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }
        }

        public bool GridNutRatioIsLocal
        {
            get
            {
                return Style.GridNutRatioIsLocal;
            }
            set
            {
                try
                {
                    Style.GridNutRatioIsLocal = value;
                    OnPropertyChanged(nameof(GridNutRatioIsLocal));
                    OnPropertyChanged(nameof(GridNutRatio));
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }
        }

        public static string GridNutRatioLabel
        {
            get
            {
                return Strings.DiagramStyleGridNutRatioLabel;
            }
        }

        public static string GridNutRatioToolTip
        {
            get
            {
                return Strings.DiagramStyleGridNutRatioToolTip;
            }
        }

        public double GridNutRatio
        {
            get
            {
                return Style.GridNutRatio;
            }
            set
            {
                try
                {
                    Style.GridNutRatio = value;
                    OnPropertyChanged(nameof(GridNutRatio));
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }
        }

        #endregion

        #region Title

        public static string TitleGroupLabel
        {
            get
            {
                return Strings.DiagramStyleTitleGroupLabel;
            }
        }

        #region Layout

        public static string TitleLayoutGroupLabel
        {
            get
            {
                return Strings.DiagramStyleTitleLayoutGroupLabel;
            }
        }

        public bool TitleVisibleIsLocal
        {
            get
            {
                return Style.TitleVisibleIsLocal;
            }
            set
            {
                try
                {
                    Style.TitleVisibleIsLocal = value;
                    OnPropertyChanged(nameof(TitleVisibleIsLocal));
                    OnPropertyChanged(nameof(TitleVisible));
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }
        }

        public static string TitleVisibleLabel
        {
            get
            {
                return Strings.DiagramStyleTitleVisibleLabel;
            }
        }

        public static string TitleVisibleToolTip
        {
            get
            {
                return Strings.DiagramStyleTitleVisibleToolTip;
            }
        }

        public bool TitleVisible
        {
            get
            {
                return Style.TitleVisible;
            }
            set
            {
                try
                {
                    Style.TitleVisible = value;
                    OnPropertyChanged(nameof(TitleVisible));
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }
        }

        public bool TitleGridPaddingIsLocal
        {
            get
            {
                return Style.TitleGridPaddingIsLocal;
            }
            set
            {
                try
                {
                    Style.TitleGridPaddingIsLocal = value;
                    OnPropertyChanged(nameof(TitleGridPaddingIsLocal));
                    OnPropertyChanged(nameof(TitleGridPadding));
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }
        }

        public static string TitleGridPaddingLabel
        {
            get
            {
                return Strings.DiagramStyleTitleGridPaddingLabel;
            }
        }

        public static string TitleGridPaddingToolTip
        {
            get
            {
                return Strings.DiagramStyleTitleGridPaddingToolTip;
            }
        }

        public double TitleGridPadding
        {
            get
            {
                return Style.TitleGridPadding;
            }
            set
            {
                try
                {
                    Style.TitleGridPadding = value;
                    OnPropertyChanged(nameof(TitleGridPadding));
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }
        }

        public bool TitleTextAlignmentIsLocal
        {
            get
            {
                return Style.TitleTextAlignmentIsLocal;
            }
            set
            {
                try
                {
                    Style.TitleTextAlignmentIsLocal = value;
                    OnPropertyChanged(nameof(TitleTextAlignmentIsLocal));
                    OnPropertyChanged(nameof(SelectedTitleTextAlignmentIndex));
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }
        }

        public static string SelectedTitleTextAlignmentLabel
        {
            get
            {
                return Strings.DiagramStyleTitleTextAlignmentLabel;
            }
        }

        public static string SelectedTitleTextAlignmentToolTip
        {
            get
            {
                return Strings.DiagramStyleTitleTextAlignmentToolTip;
            }
        }

        public int SelectedTitleTextAlignmentIndex
        {
            get
            {
                return (int)Style.TitleTextAlignment;
            }
            set
            {
                try
                {
                    Style.TitleTextAlignment = (DiagramHorizontalAlignment)(value);
                    OnPropertyChanged(nameof(SelectedTitleTextAlignmentIndex));
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }
        }

        public static ObservableCollection<string> TitleTextAlignments
        {
            get
            {
                return ObservableEnums.GetHorizontalAlignments();
            }
        }

        #endregion

        #region Text

        public static string TitleTextGroupLabel
        {
            get
            {
                return Strings.DiagramStyleTitleTextGroupLabel;
            }
        }

        public bool TitleFontFamilyIsLocal
        {
            get
            {
                return Style.TitleFontFamilyIsLocal;
            }
            set
            {
                try
                {
                    Style.TitleFontFamilyIsLocal = value;
                    OnPropertyChanged(nameof(TitleFontFamilyIsLocal));
                    OnPropertyChanged(nameof(TitleFontFamily));
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }
        }

        public static string TitleFontFamilyLabel
        {
            get
            {
                return Strings.DiagramStyleTitleFontFamilyLabel;
            }
        }

        public static string TitleFontFamilyToolTip
        {
            get
            {
                return Strings.DiagramStyleTitleFontFamilyToolTip;
            }
        }

        public string TitleFontFamily
        {
            get
            {
                return Style.TitleFontFamily;
            }
            set
            {
                try
                {
                    if (value is not null)
                    {
                        Style.TitleFontFamily = value;
                        FontFamilies.SortedInsert(TitleFontFamily);
                        OnPropertyChanged(nameof(TitleFontFamily));
                    }
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }
        }

        public bool TitleTextSizeIsLocal
        {
            get
            {
                return Style.TitleTextSizeIsLocal;
            }
            set
            {
                try
                {
                    Style.TitleTextSizeIsLocal = value;
                    OnPropertyChanged(nameof(TitleTextSizeIsLocal));
                    OnPropertyChanged(nameof(TitleTextSize));
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }
        }

        public static string TitleTextSizeLabel
        {
            get
            {
                return Strings.DiagramStyleTitleTextSizeLabel;
            }
        }

        public static string TitleTextSizeToolTip
        {
            get
            {
                return Strings.DiagramStyleTitleTextSizeToolTip;
            }
        }

        public double TitleTextSize
        {
            get
            {
                return Style.TitleTextSize;
            }
            set
            {
                try
                {
                    Style.TitleTextSize = value;
                    OnPropertyChanged(nameof(TitleTextSize));
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }
        }

        public bool TitleTextSizeModRatioIsLocal
        {
            get
            {
                return Style.TitleTextSizeModRatioIsLocal;
            }
            set
            {
                try
                {
                    Style.TitleTextSizeModRatioIsLocal = value;
                    OnPropertyChanged(nameof(TitleTextSizeModRatioIsLocal));
                    OnPropertyChanged(nameof(TitleTextSizeModRatio));
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }
        }

        public static string TitleTextSizeModRatioLabel
        {
            get
            {
                return Strings.DiagramStyleTitleTextSizeModRatioLabel;
            }
        }

        public static string TitleTextSizeModRatioToolTip
        {
            get
            {
                return Strings.DiagramStyleTitleTextSizeModRatioToolTip;
            }
        }

        public double TitleTextSizeModRatio
        {
            get
            {
                return Style.TitleTextSizeModRatio;
            }
            set
            {
                try
                {
                    Style.TitleTextSizeModRatio = value;
                    OnPropertyChanged(nameof(TitleTextSizeModRatio));
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }
        }

        public bool TitleTextStyleIsLocal
        {
            get
            {
                return Style.TitleTextStyleIsLocal;
            }
            set
            {
                try
                {
                    Style.TitleTextStyleIsLocal = value;
                    OnPropertyChanged(nameof(TitleTextStyleIsLocal));
                    OnPropertyChanged(nameof(SelectedTitleTextStyleIndex));
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }
        }

        public static string SelectedTitleTextStyleLabel
        {
            get
            {
                return Strings.DiagramStyleTitleTextStyleLabel;
            }
        }

        public static string SelectedTitleTextStyleToolTip
        {
            get
            {
                return Strings.DiagramStyleTitleTextStyleToolTip;
            }
        }

        public int SelectedTitleTextStyleIndex
        {
            get
            {
                return (int)Style.TitleTextStyle;
            }
            set
            {
                try
                {
                    Style.TitleTextStyle = (DiagramTextStyle)(value);
                    OnPropertyChanged(nameof(SelectedTitleTextStyleIndex));
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }
        }

        public static ObservableCollection<string> TitleTextStyles
        {
            get
            {
                return ObservableEnums.GetTextStyles();
            }
        }

        public bool TitleLabelStyleIsLocal
        {
            get
            {
                return Style.TitleLabelStyleIsLocal;
            }
            set
            {
                try
                {
                    Style.TitleLabelStyleIsLocal = value;
                    OnPropertyChanged(nameof(TitleLabelStyleIsLocal));
                    OnPropertyChanged(nameof(SelectedTitleLabelStyleIndex));
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }
        }

        public static string SelectedTitleLabelStyleLabel
        {
            get
            {
                return Strings.DiagramStyleTitleLabelStyleLabel;
            }
        }

        public static string SelectedTitleLabelStyleToolTip
        {
            get
            {
                return Strings.DiagramStyleTitleLabelStyleToolTip;
            }
        }

        public int SelectedTitleLabelStyleIndex
        {
            get
            {
                return (int)Style.TitleLabelStyle;
            }
            set
            {
                try
                {
                    Style.TitleLabelStyle = (DiagramLabelStyle)(value);
                    OnPropertyChanged(nameof(SelectedTitleLabelStyleIndex));
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }
        }

        public static ObservableCollection<string> TitleLabelStyles
        {
            get
            {
                return ObservableEnums.GetDiagramLabelStyles();
            }
        }

        public bool TitleColorIsLocal
        {
            get
            {
                return Style.TitleColorIsLocal;
            }
            set
            {
                try
                {
                    Style.TitleColorIsLocal = value;
                    OnPropertyChanged(nameof(TitleColorIsLocal));
                    OnPropertyChanged(nameof(TitleColor));
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }
        }

        public static string TitleColorLabel
        {
            get
            {
                return Strings.DiagramStyleTitleColorLabel;
            }
        }

        public static string TitleColorToolTip
        {
            get
            {
                return Strings.DiagramStyleTitleColorToolTip;
            }
        }

        public string TitleColor
        {
            get
            {
                return Style.TitleColor;
            }
            set
            {
                try
                {
                    if (value is not null)
                    {
                        Style.TitleColor = value;
                        OnPropertyChanged(nameof(TitleColor));
                    }
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }
        }

        public bool TitleOpacityIsLocal
        {
            get
            {
                return Style.TitleOpacityIsLocal;
            }
            set
            {
                try
                {
                    Style.TitleOpacityIsLocal = value;
                    OnPropertyChanged(nameof(TitleOpacityIsLocal));
                    OnPropertyChanged(nameof(TitleOpacity));
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }
        }

        public static string TitleOpacityLabel
        {
            get
            {
                return Strings.DiagramStyleTitleOpacityLabel;
            }
        }

        public static string TitleOpacityToolTip
        {
            get
            {
                return Strings.DiagramStyleTitleOpacityToolTip;
            }
        }

        public double TitleOpacity
        {
            get
            {
                return Style.TitleOpacity;
            }
            set
            {
                try
                {
                    Style.TitleOpacity = value;
                    OnPropertyChanged(nameof(TitleOpacity));
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }
        }

        #endregion

        #endregion

        #region Marks

        public static string SelectedMarkTypeLabel
        {
            get
            {
                return Strings.DiagramStyleMarkTypeLabel;
            }
        }

        public static string SelectedMarkTypeToolTip
        {
            get
            {
                return Strings.DiagramStyleMarkTypeToolTip;
            }
        }

        public int SelectedMarkTypeIndex
        {
            get
            {
                return (int)MarkStyle.MarkType;
            }
            set
            {
                try
                {
                    MarkStyle.MarkType = (DiagramMarkType)(value);
                    OnPropertyChanged(nameof(SelectedMarkTypeIndex));
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }
        }

        public static ObservableCollection<string> MarkTypes
        {
            get
            {
                return ObservableEnums.GetMarkTypes();
            }
        }

        public static string MarksGroupLabel
        {
            get
            {
                return Strings.DiagramStyleDiagramMarksGroupLabel;
            }
        }

        #region Background

        public static string MarkBackgroundGroupLabel
        {
            get
            {
                return Strings.DiagramStyleMarkBackgroundGroupLabel;
            }
        }

        public bool MarkShapeIsLocal
        {
            get
            {
                return MarkStyle.MarkShapeIsLocal;
            }
            set
            {
                try
                {
                    MarkStyle.MarkShapeIsLocal = value;
                    OnPropertyChanged(nameof(MarkShapeIsLocal));
                    OnPropertyChanged(nameof(SelectedMarkShapeIndex));
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }
        }

        public static string SelectedMarkShapeLabel
        {
            get
            {
                return Strings.DiagramStyleMarkShapeLabel;
            }
        }

        public static string SelectedMarkShapeToolTip
        {
            get
            {
                return Strings.DiagramStyleMarkShapeToolTip;
            }
        }

        public int SelectedMarkShapeIndex
        {
            get
            {
                return (int)MarkStyle.MarkShape;
            }
            set
            {
                try
                {
                    MarkStyle.MarkShape = (DiagramMarkShape)(value);
                    OnPropertyChanged(nameof(SelectedMarkShapeIndex));
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }
        }

        public static ObservableCollection<string> MarkShapes
        {
            get
            {
                return ObservableEnums.GetMarkShapes();
            }
        }

        public bool MarkVisibleIsLocal
        {
            get
            {
                return MarkStyle.MarkVisibleIsLocal;
            }
            set
            {
                try
                {
                    MarkStyle.MarkVisibleIsLocal = value;
                    OnPropertyChanged(nameof(MarkVisibleIsLocal));
                    OnPropertyChanged(nameof(MarkVisible));
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }
        }

        public static string MarkVisibleLabel
        {
            get
            {
                return Strings.DiagramStyleMarkVisibleLabel;
            }
        }

        public static string MarkVisibleToolTip
        {
            get
            {
                return Strings.DiagramStyleMarkVisibleToolTip;
            }
        }

        public bool MarkVisible
        {
            get
            {
                return MarkStyle.MarkVisible;
            }
            set
            {
                try
                {
                    MarkStyle.MarkVisible = value;
                    OnPropertyChanged(nameof(MarkVisible));
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }
        }

        public bool MarkColorIsLocal
        {
            get
            {
                return MarkStyle.MarkColorIsLocal;
            }
            set
            {
                try
                {
                    MarkStyle.MarkColorIsLocal = value;
                    OnPropertyChanged(nameof(MarkColorIsLocal));
                    OnPropertyChanged(nameof(MarkColor));
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }
        }

        public static string MarkColorLabel
        {
            get
            {
                return Strings.DiagramStyleMarkColorLabel;
            }
        }

        public static string MarkColorToolTip
        {
            get
            {
                return Strings.DiagramStyleMarkColorToolTip;
            }
        }

        public string MarkColor
        {
            get
            {
                return MarkStyle.MarkColor;
            }
            set
            {
                try
                {
                    if (value is not null)
                    {
                        MarkStyle.MarkColor = value;
                        OnPropertyChanged(nameof(MarkColor));
                    }
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }
        }

        public bool MarkOpacityIsLocal
        {
            get
            {
                return MarkStyle.MarkOpacityIsLocal;
            }
            set
            {
                try
                {
                    MarkStyle.MarkOpacityIsLocal = value;
                    OnPropertyChanged(nameof(MarkOpacityIsLocal));
                    OnPropertyChanged(nameof(MarkOpacity));
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }
        }

        public static string MarkOpacityLabel
        {
            get
            {
                return Strings.DiagramStyleMarkOpacityLabel;
            }
        }

        public static string MarkOpacityToolTip
        {
            get
            {
                return Strings.DiagramStyleMarkOpacityToolTip;
            }
        }

        public double MarkOpacity
        {
            get
            {
                return MarkStyle.MarkOpacity;
            }
            set
            {
                try
                {
                    MarkStyle.MarkOpacity = value;
                    OnPropertyChanged(nameof(MarkOpacity));
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }
        }

        public bool MarkRadiusRatioIsLocal
        {
            get
            {
                return MarkStyle.MarkRadiusRatioIsLocal;
            }
            set
            {
                try
                {
                    MarkStyle.MarkRadiusRatioIsLocal = value;
                    OnPropertyChanged(nameof(MarkRadiusRatioIsLocal));
                    OnPropertyChanged(nameof(MarkRadiusRatio));
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }
        }

        public static string MarkRadiusRatioLabel
        {
            get
            {
                return Strings.DiagramStyleMarkRadiusRatioLabel;
            }
        }

        public static string MarkRadiusRatioToolTip
        {
            get
            {
                return Strings.DiagramStyleMarkRadiusRatioToolTip;
            }
        }

        public double MarkRadiusRatio
        {
            get
            {
                return MarkStyle.MarkRadiusRatio;
            }
            set
            {
                try
                {
                    MarkStyle.MarkRadiusRatio = value;
                    OnPropertyChanged(nameof(MarkRadiusRatio));
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }
        }

        #endregion

        #region Border

        public static string MarkBorderGroupLabel
        {
            get
            {
                return Strings.DiagramStyleMarkBorderGroupLabel;
            }
        }

        public bool MarkBorderColorIsLocal
        {
            get
            {
                return MarkStyle.MarkBorderColorIsLocal;
            }
            set
            {
                try
                {
                    MarkStyle.MarkBorderColorIsLocal = value;
                    OnPropertyChanged(nameof(MarkBorderColorIsLocal));
                    OnPropertyChanged(nameof(MarkBorderColor));
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }
        }

        public static string MarkBorderColorLabel
        {
            get
            {
                return Strings.DiagramStyleMarkBorderColorLabel;
            }
        }

        public static string MarkBorderColorToolTip
        {
            get
            {
                return Strings.DiagramStyleMarkBorderColorToolTip;
            }
        }

        public string MarkBorderColor
        {
            get
            {
                return MarkStyle.MarkBorderColor;
            }
            set
            {
                try
                {
                    MarkStyle.MarkBorderColor = value;
                    OnPropertyChanged(nameof(MarkBorderColor));
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }
        }

        public bool MarkBorderThicknessIsLocal
        {
            get
            {
                return MarkStyle.MarkBorderThicknessIsLocal;
            }
            set
            {
                try
                {
                    MarkStyle.MarkBorderThicknessIsLocal = value;
                    OnPropertyChanged(nameof(MarkBorderThicknessIsLocal));
                    OnPropertyChanged(nameof(MarkBorderThickness));
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }
        }

        public static string MarkBorderThicknessLabel
        {
            get
            {
                return Strings.DiagramStyleMarkBorderThicknessLabel;
            }
        }

        public static string MarkBorderThicknessToolTip
        {
            get
            {
                return Strings.DiagramStyleMarkBorderThicknessToolTip;
            }
        }

        public double MarkBorderThickness
        {
            get
            {
                return MarkStyle.MarkBorderThickness;
            }
            set
            {
                try
                {
                    MarkStyle.MarkBorderThickness = value;
                    OnPropertyChanged(nameof(MarkBorderThickness));
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }
        }

        #endregion

        #region Text

        public static string MarkTextGroupLabel
        {
            get
            {
                return Strings.DiagramStyleMarkTextGroupLabel;
            }
        }

        public bool MarkTextAlignmentIsLocal
        {
            get
            {
                return MarkStyle.MarkTextAlignmentIsLocal;
            }
            set
            {
                try
                {
                    MarkStyle.MarkTextAlignmentIsLocal = value;
                    OnPropertyChanged(nameof(MarkTextAlignmentIsLocal));
                    OnPropertyChanged(nameof(SelectedMarkTextAlignmentIndex));
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }
        }

        public static string SelectedMarkTextAlignmentLabel
        {
            get
            {
                return Strings.DiagramStyleMarkTextAlignmentLabel;
            }
        }

        public static string SelectedMarkTextAlignmentToolTip
        {
            get
            {
                return Strings.DiagramStyleMarkTextAlignmentToolTip;
            }
        }

        public int SelectedMarkTextAlignmentIndex
        {
            get
            {
                return (int)MarkStyle.MarkTextAlignment;
            }
            set
            {
                try
                {
                    MarkStyle.MarkTextAlignment = (DiagramHorizontalAlignment)(value);
                    OnPropertyChanged(nameof(SelectedMarkTextAlignmentIndex));
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }
        }

        public static ObservableCollection<string> MarkTextAlignments
        {
            get
            {
                return ObservableEnums.GetHorizontalAlignments();
            }
        }

        public bool MarkTextVisibleIsLocal
        {
            get
            {
                return MarkStyle.MarkTextVisibleIsLocal;
            }
            set
            {
                try
                {
                    MarkStyle.MarkTextVisibleIsLocal = value;
                    OnPropertyChanged(nameof(MarkTextVisibleIsLocal));
                    OnPropertyChanged(nameof(MarkTextVisible));
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }
        }

        public static string MarkTextVisibleLabel
        {
            get
            {
                return Strings.DiagramStyleMarkTextVisibleLabel;
            }
        }

        public static string MarkTextVisibleToolTip
        {
            get
            {
                return Strings.DiagramStyleMarkTextVisibleToolTip;
            }
        }

        public bool MarkTextVisible
        {
            get
            {
                return MarkStyle.MarkTextVisible;
            }
            set
            {
                try
                {
                    MarkStyle.MarkTextVisible = value;
                    OnPropertyChanged(nameof(MarkTextVisible));
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }
        }

        public bool MarkTextColorIsLocal
        {
            get
            {
                return MarkStyle.MarkTextColorIsLocal;
            }
            set
            {
                try
                {
                    MarkStyle.MarkTextColorIsLocal = value;
                    OnPropertyChanged(nameof(MarkTextColorIsLocal));
                    OnPropertyChanged(nameof(MarkTextColor));
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }
        }

        public static string MarkTextColorLabel
        {
            get
            {
                return Strings.DiagramStyleMarkTextColorLabel;
            }
        }

        public static string MarkTextColorToolTip
        {
            get
            {
                return Strings.DiagramStyleMarkTextColorToolTip;
            }
        }

        public string MarkTextColor
        {
            get
            {
                return MarkStyle.MarkTextColor;
            }
            set
            {
                try
                {
                    if (value is not null)
                    {
                        MarkStyle.MarkTextColor = value;
                        OnPropertyChanged(nameof(MarkTextColor));
                    }
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }
        }

        public bool MarkTextOpacityIsLocal
        {
            get
            {
                return MarkStyle.MarkTextOpacityIsLocal;
            }
            set
            {
                try
                {
                    MarkStyle.MarkTextOpacityIsLocal = value;
                    OnPropertyChanged(nameof(MarkTextOpacityIsLocal));
                    OnPropertyChanged(nameof(MarkTextOpacity));
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }
        }

        public static string MarkTextOpacityLabel
        {
            get
            {
                return Strings.DiagramStyleMarkTextOpacityLabel;
            }
        }

        public static string MarkTextOpacityToolTip
        {
            get
            {
                return Strings.DiagramStyleMarkTextOpacityToolTip;
            }
        }

        public double MarkTextOpacity
        {
            get
            {
                return MarkStyle.MarkTextOpacity;
            }
            set
            {
                try
                {
                    MarkStyle.MarkTextOpacity = value;
                    OnPropertyChanged(nameof(MarkTextOpacity));
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }
        }

        public bool MarkFontFamilyIsLocal
        {
            get
            {
                return MarkStyle.MarkFontFamilyIsLocal;
            }
            set
            {
                try
                {
                    MarkStyle.MarkFontFamilyIsLocal = value;
                    OnPropertyChanged(nameof(MarkFontFamilyIsLocal));
                    OnPropertyChanged(nameof(MarkFontFamily));
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }
        }

        public static string MarkFontFamilyLabel
        {
            get
            {
                return Strings.DiagramStyleMarkFontFamilyLabel;
            }
        }

        public static string MarkFontFamilyToolTip
        {
            get
            {
                return Strings.DiagramStyleMarkFontFamilyToolTip;
            }
        }

        public string MarkFontFamily
        {
            get
            {
                return MarkStyle.MarkFontFamily;
            }
            set
            {
                try
                {
                    if (value is not null)
                    {
                        MarkStyle.MarkFontFamily = value;
                        FontFamilies.SortedInsert(MarkFontFamily);
                        OnPropertyChanged(nameof(MarkFontFamily));
                    }
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }
        }

        public bool MarkTextStyleIsLocal
        {
            get
            {
                return MarkStyle.MarkTextStyleIsLocal;
            }
            set
            {
                try
                {
                    MarkStyle.MarkTextStyleIsLocal = value;
                    OnPropertyChanged(nameof(MarkTextStyleIsLocal));
                    OnPropertyChanged(nameof(SelectedMarkTextStyleIndex));
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }
        }

        public static string SelectedMarkTextStyleLabel
        {
            get
            {
                return Strings.DiagramStyleMarkTextStyleLabel;
            }
        }

        public static string SelectedMarkTextStyleToolTip
        {
            get
            {
                return Strings.DiagramStyleMarkTextStyleToolTip;
            }
        }

        public int SelectedMarkTextStyleIndex
        {
            get
            {
                return (int)MarkStyle.MarkTextStyle;
            }
            set
            {
                try
                {
                    MarkStyle.MarkTextStyle = (DiagramTextStyle)(value);
                    OnPropertyChanged(nameof(SelectedMarkTextStyleIndex));
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }
        }

        public static ObservableCollection<string> MarkTextStyles
        {
            get
            {
                return ObservableEnums.GetTextStyles();
            }
        }

        public bool MarkTextSizeRatioIsLocal
        {
            get
            {
                return MarkStyle.MarkTextSizeRatioIsLocal;
            }
            set
            {
                try
                {
                    MarkStyle.MarkTextSizeRatioIsLocal = value;
                    OnPropertyChanged(nameof(MarkTextSizeRatioIsLocal));
                    OnPropertyChanged(nameof(MarkTextSizeRatio));
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }
        }

        public static string MarkTextSizeRatioLabel
        {
            get
            {
                return Strings.DiagramStyleMarkTextSizeRatioLabel;
            }
        }

        public static string MarkTextSizeRatioToolTip
        {
            get
            {
                return Strings.DiagramStyleMarkTextSizeRatioToolTip;
            }
        }

        public double MarkTextSizeRatio
        {
            get
            {
                return MarkStyle.MarkTextSizeRatio;
            }
            set
            {
                try
                {
                    MarkStyle.MarkTextSizeRatio = value;
                    OnPropertyChanged(nameof(MarkTextSizeRatio));
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }
        }

        #endregion

        #endregion

        #region Fret Labels

        public static string FretLabelsGroupLabel
        {
            get
            {
                return Strings.DiagramStyleDiagramFretLabelsGroupLabel;
            }
        }

        #region Layout

        public static string FretLabelLayoutGroupLabel
        {
            get
            {
                return Strings.DiagramStyleFretLabelLayoutGroupLabel;
            }
        }

        public bool FretLabelTextAlignmentIsLocal
        {
            get
            {
                return Style.FretLabelTextAlignmentIsLocal;
            }
            set
            {
                try
                {
                    Style.FretLabelTextAlignmentIsLocal = value;
                    OnPropertyChanged(nameof(FretLabelTextAlignmentIsLocal));
                    OnPropertyChanged(nameof(SelectedFretLabelTextAlignmentIndex));
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }
        }

        public static string SelectedFretLabelTextAlignmentLabel
        {
            get
            {
                return Strings.DiagramStyleFretLabelTextAlignmentLabel;
            }
        }

        public static string SelectedFretLabelTextAlignmentToolTip
        {
            get
            {
                return Strings.DiagramStyleFretLabelTextAlignmentToolTip;
            }
        }

        public int SelectedFretLabelTextAlignmentIndex
        {
            get
            {
                return (int)Style.FretLabelTextAlignment;
            }
            set
            {
                try
                {
                    Style.FretLabelTextAlignment = (DiagramHorizontalAlignment)(value);
                    OnPropertyChanged(nameof(SelectedFretLabelTextAlignmentIndex));
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }
        }

        public static ObservableCollection<string> FretLabelTextAlignments
        {
            get
            {
                return ObservableEnums.GetHorizontalAlignments();
            }
        }

        public bool FretLabelTextVisibleIsLocal
        {
            get
            {
                return Style.FretLabelTextVisibleIsLocal;
            }
            set
            {
                try
                {
                    Style.FretLabelTextVisibleIsLocal = value;
                    OnPropertyChanged(nameof(FretLabelTextVisibleIsLocal));
                    OnPropertyChanged(nameof(FretLabelTextVisible));
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }
        }

        public static string FretLabelTextVisibleLabel
        {
            get
            {
                return Strings.DiagramStyleFretLabelTextVisibleLabel;
            }
        }

        public static string FretLabelTextVisibleToolTip
        {
            get
            {
                return Strings.DiagramStyleFretLabelTextVisibleToolTip;
            }
        }

        public bool FretLabelTextVisible
        {
            get
            {
                return Style.FretLabelTextVisible;
            }
            set
            {
                try
                {
                    Style.FretLabelTextVisible = value;
                    OnPropertyChanged(nameof(FretLabelTextVisible));
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }
        }

        public bool FretLabelGridPaddingIsLocal
        {
            get
            {
                return Style.FretLabelGridPaddingIsLocal;
            }
            set
            {
                try
                {
                    Style.FretLabelGridPaddingIsLocal = value;
                    OnPropertyChanged(nameof(FretLabelGridPaddingIsLocal));
                    OnPropertyChanged(nameof(FretLabelGridPadding));
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }
        }

        public static string FretLabelGridPaddingLabel
        {
            get
            {
                return Strings.DiagramStyleFretLabelGridPaddingLabel;
            }
        }

        public static string FretLabelGridPaddingToolTip
        {
            get
            {
                return Strings.DiagramStyleFretLabelGridPaddingToolTip;
            }
        }

        public double FretLabelGridPadding
        {
            get
            {
                return Style.FretLabelGridPadding;
            }
            set
            {
                try
                {
                    Style.FretLabelGridPadding = value;
                    OnPropertyChanged(nameof(FretLabelGridPadding));
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }
        }

        #endregion

        #region Text

        public static string FretLabelTextGroupLabel
        {
            get
            {
                return Strings.DiagramStyleFretLabelTextGroupLabel;
            }
        }

        public bool FretLabelTextColorIsLocal
        {
            get
            {
                return Style.FretLabelTextColorIsLocal;
            }
            set
            {
                try
                {
                    Style.FretLabelTextColorIsLocal = value;
                    OnPropertyChanged(nameof(FretLabelTextColorIsLocal));
                    OnPropertyChanged(nameof(FretLabelTextColor));
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }
        }

        public static string FretLabelTextColorLabel
        {
            get
            {
                return Strings.DiagramStyleFretLabelTextColorLabel;
            }
        }

        public static string FretLabelTextColorToolTip
        {
            get
            {
                return Strings.DiagramStyleFretLabelTextColorToolTip;
            }
        }

        public string FretLabelTextColor
        {
            get
            {
                return Style.FretLabelTextColor;
            }
            set
            {
                try
                {
                    if (value is not null)
                    {
                        Style.FretLabelTextColor = value;
                        OnPropertyChanged(nameof(FretLabelTextColor));
                    }
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }
        }

        public bool FretLabelTextOpacityIsLocal
        {
            get
            {
                return Style.FretLabelTextOpacityIsLocal;
            }
            set
            {
                try
                {
                    Style.FretLabelTextOpacityIsLocal = value;
                    OnPropertyChanged(nameof(FretLabelTextOpacityIsLocal));
                    OnPropertyChanged(nameof(FretLabelTextOpacity));
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }
        }

        public static string FretLabelTextOpacityLabel
        {
            get
            {
                return Strings.DiagramStyleFretLabelTextOpacityLabel;
            }
        }

        public static string FretLabelTextOpacityToolTip
        {
            get
            {
                return Strings.DiagramStyleFretLabelTextOpacityToolTip;
            }
        }

        public double FretLabelTextOpacity
        {
            get
            {
                return Style.FretLabelTextOpacity;
            }
            set
            {
                try
                {
                    Style.FretLabelTextOpacity = value;
                    OnPropertyChanged(nameof(FretLabelTextOpacity));
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }
        }

        public bool FretLabelFontFamilyIsLocal
        {
            get
            {
                return Style.FretLabelFontFamilyIsLocal;
            }
            set
            {
                try
                {
                    Style.FretLabelFontFamilyIsLocal = value;
                    OnPropertyChanged(nameof(FretLabelFontFamilyIsLocal));
                    OnPropertyChanged(nameof(FretLabelFontFamily));
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }
        }

        public static string FretLabelFontFamilyLabel
        {
            get
            {
                return Strings.DiagramStyleFretLabelFontFamilyLabel;
            }
        }

        public static string FretLabelFontFamilyToolTip
        {
            get
            {
                return Strings.DiagramStyleFretLabelFontFamilyToolTip;
            }
        }

        public string FretLabelFontFamily
        {
            get
            {
                return Style.FretLabelFontFamily;
            }
            set
            {
                try
                {
                    if (value is not null)
                    {
                        Style.FretLabelFontFamily = value;
                        FontFamilies.SortedInsert(FretLabelFontFamily);
                        OnPropertyChanged(nameof(FretLabelFontFamily));
                    }
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }
        }

        public bool FretLabelTextStyleIsLocal
        {
            get
            {
                return Style.FretLabelTextStyleIsLocal;
            }
            set
            {
                try
                {
                    Style.FretLabelTextStyleIsLocal = value;
                    OnPropertyChanged(nameof(FretLabelTextStyleIsLocal));
                    OnPropertyChanged(nameof(SelectedFretLabelTextStyleIndex));
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }
        }

        public static string SelectedFretLabelTextStyleLabel
        {
            get
            {
                return Strings.DiagramStyleFretLabelTextStyleLabel;
            }
        }

        public static string SelectedFretLabelTextStyleToolTip
        {
            get
            {
                return Strings.DiagramStyleFretLabelTextStyleToolTip;
            }
        }

        public int SelectedFretLabelTextStyleIndex
        {
            get
            {
                return (int)Style.FretLabelTextStyle;
            }
            set
            {
                try
                {
                    Style.FretLabelTextStyle = (DiagramTextStyle)(value);
                    OnPropertyChanged(nameof(SelectedFretLabelTextStyleIndex));
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }
        }

        public static ObservableCollection<string> FretLabelTextStyles
        {
            get
            {
                return ObservableEnums.GetTextStyles();
            }
        }

        public bool FretLabelTextSizeRatioIsLocal
        {
            get
            {
                return Style.FretLabelTextSizeRatioIsLocal;
            }
            set
            {
                try
                {
                    Style.FretLabelTextSizeRatioIsLocal = value;
                    OnPropertyChanged(nameof(FretLabelTextSizeRatioIsLocal));
                    OnPropertyChanged(nameof(FretLabelTextSizeRatio));
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }
        }

        public static string FretLabelTextSizeRatioLabel
        {
            get
            {
                return Strings.DiagramStyleFretLabelTextSizeRatioLabel;
            }
        }

        public static string FretLabelTextSizeRatioToolTip
        {
            get
            {
                return Strings.DiagramStyleFretLabelTextSizeRatioToolTip;
            }
        }

        public double FretLabelTextSizeRatio
        {
            get
            {
                return Style.FretLabelTextSizeRatio;
            }
            set
            {
                try
                {
                    Style.FretLabelTextSizeRatio = value;
                    OnPropertyChanged(nameof(FretLabelTextSizeRatio));
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }
        }

        public bool FretLabelTextWidthRatioIsLocal
        {
            get
            {
                return Style.FretLabelTextWidthRatioIsLocal;
            }
            set
            {
                try
                {
                    Style.FretLabelTextWidthRatioIsLocal = value;
                    OnPropertyChanged(nameof(FretLabelTextWidthRatioIsLocal));
                    OnPropertyChanged(nameof(FretLabelTextWidthRatio));
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }
        }

        public static string FretLabelTextWidthRatioLabel
        {
            get
            {
                return Strings.DiagramStyleFretLabelTextWidthRatioLabel;
            }
        }

        public static string FretLabelTextWidthRatioToolTip
        {
            get
            {
                return Strings.DiagramStyleFretLabelTextWidthRatioToolTip;
            }
        }

        public double FretLabelTextWidthRatio
        {
            get
            {
                return Style.FretLabelTextWidthRatio;
            }
            set
            {
                try
                {
                    Style.FretLabelTextWidthRatio = value;
                    OnPropertyChanged(nameof(FretLabelTextWidthRatio));
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }
        }

        #endregion

        #endregion

        #region Barres

        public static string BarresGroupLabel
        {
            get
            {
                return Strings.DiagramStyleDiagramBarresGroupLabel;
            }
        }

        #region Layout

        public static string BarreLayoutGroupLabel
        {
            get
            {
                return Strings.DiagramStyleBarreLayoutGroupLabel;
            }
        }

        public bool BarreVisibleIsLocal
        {
            get
            {
                return Style.BarreVisibleIsLocal;
            }
            set
            {
                try
                {
                    Style.BarreVisibleIsLocal = value;
                    OnPropertyChanged(nameof(BarreVisibleIsLocal));
                    OnPropertyChanged(nameof(BarreVisible));
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }
        }

        public static string BarreVisibleLabel
        {
            get
            {
                return Strings.DiagramStyleBarreVisibleLabel;
            }
        }

        public static string BarreVisibleToolTip
        {
            get
            {
                return Strings.DiagramStyleBarreVisibleToolTip;
            }
        }

        public bool BarreVisible
        {
            get
            {
                return Style.BarreVisible;
            }
            set
            {
                try
                {
                    Style.BarreVisible = value;
                    OnPropertyChanged(nameof(BarreVisible));
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }
        }

        public bool BarreVerticalAlignmentIsLocal
        {
            get
            {
                return Style.BarreVerticalAlignmentIsLocal;
            }
            set
            {
                try
                {
                    Style.BarreVerticalAlignmentIsLocal = value;
                    OnPropertyChanged(nameof(BarreVerticalAlignmentIsLocal));
                    OnPropertyChanged(nameof(SelectedBarreVerticalAlignmentIndex));
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }
        }

        public static string SelectedBarreVerticalAlignmentLabel
        {
            get
            {
                return Strings.DiagramStyleBarreVerticalAlignmentLabel;
            }
        }

        public static string SelectedBarreVerticalAlignmentToolTip
        {
            get
            {
                return Strings.DiagramStyleBarreVerticalAlignmentToolTip;
            }
        }

        public int SelectedBarreVerticalAlignmentIndex
        {
            get
            {
                return (int)Style.BarreVerticalAlignment;
            }
            set
            {
                try
                {
                    Style.BarreVerticalAlignment = (DiagramVerticalAlignment)(value);
                    OnPropertyChanged(nameof(SelectedBarreVerticalAlignmentIndex));
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }
        }

        public static ObservableCollection<string> BarreVerticalAlignments
        {
            get
            {
                return ObservableEnums.GetVerticalAlignments();
            }
        }

        public bool BarreStackIsLocal
        {
            get
            {
                return Style.BarreStackIsLocal;
            }
            set
            {
                try
                {
                    Style.BarreStackIsLocal = value;
                    OnPropertyChanged(nameof(BarreStackIsLocal));
                    OnPropertyChanged(nameof(SelectedBarreStackIndex));
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }
        }

        public static string SelectedBarreStackLabel
        {
            get
            {
                return Strings.DiagramStyleBarreStackLabel;
            }
        }

        public static string SelectedBarreStackToolTip
        {
            get
            {
                return Strings.DiagramStyleBarreStackToolTip;
            }
        }

        public int SelectedBarreStackIndex
        {
            get
            {
                return (int)Style.BarreStack;
            }
            set
            {
                try
                {
                    Style.BarreStack = (DiagramBarreStack)(value);
                    OnPropertyChanged(nameof(SelectedBarreStackIndex));
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }
        }

        public static ObservableCollection<string> BarreStacks
        {
            get
            {
                return ObservableEnums.GetBarreStacks();
            }
        }

        #endregion

        #region Style

        public static string BarreStyleGroupLabel
        {
            get
            {
                return Strings.DiagramStyleBarreStyleGroupLabel;
            }
        }

        public bool BarreArcRatioIsLocal
        {
            get
            {
                return Style.BarreArcRatioIsLocal;
            }
            set
            {
                try
                {
                    Style.BarreArcRatioIsLocal = value;
                    OnPropertyChanged(nameof(BarreArcRatioIsLocal));
                    OnPropertyChanged(nameof(BarreArcRatio));
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }
        }

        public static string BarreArcRatioLabel
        {
            get
            {
                return Strings.DiagramStyleBarreArcRatioLabel;
            }
        }

        public static string BarreArcRatioToolTip
        {
            get
            {
                return Strings.DiagramStyleBarreArcRatioToolTip;
            }
        }

        public double BarreArcRatio
        {
            get
            {
                return Style.BarreArcRatio;
            }
            set
            {
                try
                {
                    Style.BarreArcRatio = value;
                    OnPropertyChanged(nameof(BarreArcRatio));
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }
        }

        public bool BarreOpacityIsLocal
        {
            get
            {
                return Style.BarreOpacityIsLocal;
            }
            set
            {
                try
                {
                    Style.BarreOpacityIsLocal = value;
                    OnPropertyChanged(nameof(BarreOpacityIsLocal));
                    OnPropertyChanged(nameof(BarreOpacity));
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }
        }

        public static string BarreOpacityLabel
        {
            get
            {
                return Strings.DiagramStyleBarreOpacityLabel;
            }
        }

        public static string BarreOpacityToolTip
        {
            get
            {
                return Strings.DiagramStyleBarreOpacityToolTip;
            }
        }

        public double BarreOpacity
        {
            get
            {
                return Style.BarreOpacity;
            }
            set
            {
                try
                {
                    Style.BarreOpacity = value;
                    OnPropertyChanged(nameof(BarreOpacity));
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }
        }

        public bool BarreLineColorIsLocal
        {
            get
            {
                return Style.BarreLineColorIsLocal;
            }
            set
            {
                try
                {
                    Style.BarreLineColorIsLocal = value;
                    OnPropertyChanged(nameof(BarreLineColorIsLocal));
                    OnPropertyChanged(nameof(BarreLineColor));
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }
        }

        public static string BarreLineColorLabel
        {
            get
            {
                return Strings.DiagramStyleBarreLineColorLabel;
            }
        }

        public static string BarreLineColorToolTip
        {
            get
            {
                return Strings.DiagramStyleBarreLineColorToolTip;
            }
        }

        public string BarreLineColor
        {
            get
            {
                return Style.BarreLineColor;
            }
            set
            {
                try
                {
                    Style.BarreLineColor = value;
                    OnPropertyChanged(nameof(BarreLineColor));
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }
        }

        public bool BarreLineThicknessIsLocal
        {
            get
            {
                return Style.BarreLineThicknessIsLocal;
            }
            set
            {
                try
                {
                    Style.BarreLineThicknessIsLocal = value;
                    OnPropertyChanged(nameof(BarreLineThicknessIsLocal));
                    OnPropertyChanged(nameof(BarreLineThickness));
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }
        }

        public static string BarreLineThicknessLabel
        {
            get
            {
                return Strings.DiagramStyleBarreLineThicknessLabel;
            }
        }

        public static string BarreLineThicknessToolTip
        {
            get
            {
                return Strings.DiagramStyleBarreLineThicknessToolTip;
            }
        }

        public double BarreLineThickness
        {
            get
            {
                return Style.BarreLineThickness;
            }
            set
            {
                try
                {
                    Style.BarreLineThickness = value;
                    OnPropertyChanged(nameof(BarreLineThickness));
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }
        }

        #endregion

        #endregion

        #region Shared

        public ObservableCollection<string> FontFamilies
        {
            get
            {
                return _fontFamiles ??= new ObservableCollection<string>(ObservableEnums.FontFamilies);
            }
        }
        private ObservableCollection<string> _fontFamiles;

        #endregion

        #region ShowEditor

        public RelayCommand ShowEditor
        {
            get
            {
                return _showEditor ??= new RelayCommand(() =>
                {
                    try
                    {
                        StrongReferenceMessenger.Default.Send(new ShowDiagramStyleEditorMessage(this, (changed) =>
                        {
                            try
                            {
                                PostEditCallback?.Invoke(changed);
                            }
                            catch (Exception ex)
                            {
                                ExceptionUtils.HandleException(ex);
                            }
                        }));
                    }
                    catch (Exception ex)
                    {
                        ExceptionUtils.HandleException(ex);
                    }
                });
            }
        }
        private RelayCommand _showEditor;

        #endregion

        #region Reset

        public static string ResetLabel
        {
            get
            {
                return Strings.DiagramStyleResetLabel;
            }
        }

        public static string ResetToolTip
        {
            get
            {
                return Strings.DiagramStyleResetToolTip;
            }
        }

        public RelayCommand Reset
        {
            get
            {
                return _reset ??= new RelayCommand(() =>
                {
                    try
                    {
                        StrongReferenceMessenger.Default.Send(new ConfirmationMessage(Strings.DiagramStyleResetPrompt, (confirmed) =>
                        {
                            try
                            {
                                if (confirmed)
                                {
                                    Style.Clear();
                                    this.OnAllPropertiesChanged();
                                }
                            }
                            catch (Exception ex)
                            {
                                ExceptionUtils.HandleException(ex);
                            }
                        }, "confirmation.diagramstyle.reset"));
                    }
                    catch (Exception ex)
                    {
                        ExceptionUtils.HandleException(ex);
                    }
                }, () =>
                {
                    return LocalCount > 0;
                });
            }
        }
        private RelayCommand _reset;

        #endregion

        public Action<bool> PostEditCallback
        {
            get
            {
                return _postEditCallback;
            }
            set
            {
                _postEditCallback = value ?? throw new ArgumentNullException(nameof(value));
                OnPropertyChanged(nameof(PostEditCallback));
            }
        }
        private Action<bool> _postEditCallback;

        internal DiagramStyle Style
        {
            get
            {
                return _diagramStyle;
            }
            private set
            {
                _diagramStyle = value ?? throw new ArgumentNullException(nameof(value));
                MarkStyle = new DiagramMarkStyleWrapper(value);
            }
        }
        private DiagramStyle _diagramStyle;

        internal DiagramMarkStyleWrapper MarkStyle { get; private set; }

        public ObservableDiagramStyle(DiagramStyle diagramStyle, DiagramMarkStyleWrapper diagramMarkStyle = null) : base()
        {
            Style = diagramStyle ?? throw new ArgumentNullException(nameof(diagramStyle));

            if (diagramMarkStyle is not null)
            {
                if (diagramMarkStyle.Style != diagramStyle)
                {
                    throw new ArgumentException("Unmatched style.", nameof(diagramMarkStyle));
                }

                MarkStyle = diagramMarkStyle;
            }

            // Pre-seed used fonts
            FontFamilies.SortedInsert(Style.TitleFontFamily);
            MarkStyle.ForEachMarkType(() =>
            {
                FontFamilies.SortedInsert(MarkStyle.MarkFontFamily);
            });
            FontFamilies.SortedInsert(Style.FretLabelFontFamily);

            PropertyChanged += ObservableDiagramStyle_PropertyChanged;
            MarkStyle.MarkTypeChanged += MarkStyle_MarkTypeChanged;
        }

        private void ObservableDiagramStyle_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName != nameof(Summary))
            {
                OnPropertyChanged(nameof(Summary));
                if (e.PropertyName != nameof(Reset) && e.PropertyName != nameof(LocalCount))
                {
                    OnPropertyChanged(nameof(LocalCount));
                    Reset.NotifyCanExecuteChanged();
                }
            }
        }

        private void MarkStyle_MarkTypeChanged(object sender, EventArgs e)
        {
            OnPropertyChanged(nameof(MarkShapeIsLocal));
            OnPropertyChanged(nameof(SelectedMarkShapeIndex));
            OnPropertyChanged(nameof(MarkVisibleIsLocal));
            OnPropertyChanged(nameof(MarkVisible));
            OnPropertyChanged(nameof(MarkColorIsLocal));
            OnPropertyChanged(nameof(MarkColor));
            OnPropertyChanged(nameof(MarkOpacityIsLocal));
            OnPropertyChanged(nameof(MarkOpacity));
            OnPropertyChanged(nameof(MarkRadiusRatioIsLocal));
            OnPropertyChanged(nameof(MarkRadiusRatio));
            OnPropertyChanged(nameof(MarkBorderColorIsLocal));
            OnPropertyChanged(nameof(MarkBorderColor));
            OnPropertyChanged(nameof(MarkBorderThicknessIsLocal));
            OnPropertyChanged(nameof(MarkBorderThickness));
            OnPropertyChanged(nameof(MarkTextAlignmentIsLocal));
            OnPropertyChanged(nameof(SelectedMarkTextAlignmentIndex));
            OnPropertyChanged(nameof(MarkTextVisibleIsLocal));
            OnPropertyChanged(nameof(MarkTextVisible));
            OnPropertyChanged(nameof(MarkTextColorIsLocal));
            OnPropertyChanged(nameof(MarkTextColor));
            OnPropertyChanged(nameof(MarkTextOpacityIsLocal));
            OnPropertyChanged(nameof(MarkTextOpacity));
            OnPropertyChanged(nameof(MarkFontFamilyIsLocal));
            OnPropertyChanged(nameof(MarkFontFamily));
            OnPropertyChanged(nameof(MarkTextStyleIsLocal));
            OnPropertyChanged(nameof(SelectedMarkTextStyleIndex));
            OnPropertyChanged(nameof(MarkTextSizeRatioIsLocal));
            OnPropertyChanged(nameof(MarkTextSizeRatio));
        }

        public override string ToString()
        {
            return Level;
        }
    }
}
