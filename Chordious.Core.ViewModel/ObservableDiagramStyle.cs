// 
// ObservableDiagramStyle.cs
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
using System.Collections.ObjectModel;
using System.ComponentModel;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;

using com.jonthysell.Chordious.Core;

using com.jonthysell.Chordious.Core.ViewModel.Resources;

namespace com.jonthysell.Chordious.Core.ViewModel
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
                return String.Format(Strings.DiagramStyleSummaryLabelFormat, Style.FriendlyLevel);
            }
        }

        public string SummaryToolTip
        {
            get
            {
                return String.Format(Strings.DiagramStyleSummaryToolTipFormat, Style.FriendlyLevel);
            }
        }

        public string Summary
        {
            get
            {
                return Style.Summary;
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

        public string DiagramGroupLabel
        {
            get
            {
                return Strings.DiagramStyleDiagramGroupLabel;
            }
        }

        #region Layout

        public string DiagramLayoutGroupLabel
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
                    RaisePropertyChanged("OrientationIsLocal");
                    RaisePropertyChanged("SelectedOrientationIndex");
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }
        }

        public string SelectedOrientationLabel
        {
            get
            {
                return Strings.DiagramStyleOrientationLabel;
            }
        }

        public string SelectedOrientationToolTip
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
                    RaisePropertyChanged("SelectedOrientationIndex");
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }
        }

        public ObservableCollection<string> Orientations
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
                    RaisePropertyChanged("LabelLayoutModelIsLocal");
                    RaisePropertyChanged("SelectedLabelLayoutModelIndex");
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }
        }

        public string SelectedLabelLayoutModelLabel
        {
            get
            {
                return Strings.DiagramStyleLabelLayoutModelLabel;
            }
        }

        public string SelectedLabelLayoutModelToolTip
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
                    RaisePropertyChanged("SelectedLabelLayoutModelIndex");
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }
        }

        public ObservableCollection<string> LabelLayoutModels
        {
            get
            {
                return ObservableEnums.GetLabelLayoutModels();
            }
        }

        #endregion

        #region Background

        public string DiagramBackgroundGroupLabel
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
                    RaisePropertyChanged("DiagramColorIsLocal");
                    RaisePropertyChanged("DiagramColor");
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }
        }

        public string DiagramColorLabel
        {
            get
            {
                return Strings.DiagramStyleDiagramColorLabel;
            }
        }

        public string DiagramColorToolTip
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
                    if (null != value)
                    {
                        Style.DiagramColor = value;
                        ObservableEnums.SortedInsert(Colors, DiagramColor);
                        RaisePropertyChanged("DiagramColor");
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
                    RaisePropertyChanged("DiagramOpacityIsLocal");
                    RaisePropertyChanged("DiagramOpacity");
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }
        }

        public string DiagramOpacityLabel
        {
            get
            {
                return Strings.DiagramStyleDiagramOpacityLabel;
            }
        }

        public string DiagramOpacityToolTip
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
                    RaisePropertyChanged("DiagramOpacity");
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }
        }

        #endregion

        #region Border

        public string DiagramBorderGroupLabel
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
                    RaisePropertyChanged("DiagramBorderColorIsLocal");
                    RaisePropertyChanged("DiagramBorderColor");
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }
        }

        public string DiagramBorderColorLabel
        {
            get
            {
                return Strings.DiagramStyleDiagramBorderColorLabel;
            }
        }

        public string DiagramBorderColorToolTip
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
                    if (null != value)
                    {
                        Style.DiagramBorderColor = value;
                        ObservableEnums.SortedInsert(Colors, DiagramBorderColor);
                        RaisePropertyChanged("DiagramBorderColor");
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
                    RaisePropertyChanged("DiagramBorderThicknessIsLocal");
                    RaisePropertyChanged("DiagramBorderThickness");
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }
        }

        public string DiagramBorderThicknessLabel
        {
            get
            {
                return Strings.DiagramStyleDiagramBorderThicknessLabel;
            }
        }

        public string DiagramBorderThicknessToolTip
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
                    RaisePropertyChanged("DiagramBorderThickness");
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }
        }

        #endregion

        #region NewDiagram

        public string NewDiagramGroupLabel
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
                    RaisePropertyChanged("NewDiagramNumStringsIsLocal");
                    RaisePropertyChanged("NewDiagramNumStrings");
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }
        }

        public string NewDiagramNumStringsLabel
        {
            get
            {
                return Strings.DiagramStyleNewDiagramNumStringsLabel;
            }
        }

        public string NewDiagramNumStringsToolTip
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
                    RaisePropertyChanged("NewDiagramNumStrings");
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
                    RaisePropertyChanged("NewDiagramNumFretsIsLocal");
                    RaisePropertyChanged("NewDiagramNumFrets");
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }
        }

        public string NewDiagramNumFretsLabel
        {
            get
            {
                return Strings.DiagramStyleNewDiagramNumFretsLabel;
            }
        }

        public string NewDiagramNumFretsToolTip
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
                    RaisePropertyChanged("NewDiagramNumFrets");
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

        public string GridGroupLabel
        {
            get
            {
                return Strings.DiagramStyleGridGroupLabel;
            }
        }

        public string GridMarginGroupLabel
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
                    RaisePropertyChanged("GridMarginIsLocal");
                    RaisePropertyChanged("GridMargin");
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }
        }

        public string GridMarginLabel
        {
            get
            {
                return Strings.DiagramStyleGridMarginLabel;
            }
        }

        public string GridMarginToolTip
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
                    RaisePropertyChanged("GridMargin");
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
                    RaisePropertyChanged("GridMarginLeftIsLocal");
                    RaisePropertyChanged("GridMarginLeft");
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }
        }

        public string GridMarginLeftLabel
        {
            get
            {
                return Strings.DiagramStyleGridMarginLeftLabel;
            }
        }

        public string GridMarginLeftToolTip
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
                    RaisePropertyChanged("GridMarginLeft");
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
                    RaisePropertyChanged("GridMarginRightIsLocal");
                    RaisePropertyChanged("GridMarginRight");
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }
        }

        public string GridMarginRightLabel
        {
            get
            {
                return Strings.DiagramStyleGridMarginRightLabel;
            }
        }

        public string GridMarginRightToolTip
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
                    RaisePropertyChanged("GridMarginRight");
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
                    RaisePropertyChanged("GridMarginTopIsLocal");
                    RaisePropertyChanged("GridMarginTop");
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }
        }

        public string GridMarginTopLabel
        {
            get
            {
                return Strings.DiagramStyleGridMarginTopLabel;
            }
        }

        public string GridMarginTopToolTip
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
                    RaisePropertyChanged("GridMarginTop");
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
                    RaisePropertyChanged("GridMarginBottomIsLocal");
                    RaisePropertyChanged("GridMarginBottom");
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }
        }

        public string GridMarginBottomLabel
        {
            get
            {
                return Strings.DiagramStyleGridMarginBottomLabel;
            }
        }

        public string GridMarginBottomToolTip
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
                    RaisePropertyChanged("GridMarginBottom");
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }
        }

        public string GridSpacingGroupLabel
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
                    RaisePropertyChanged("GridFretSpacingIsLocal");
                    RaisePropertyChanged("GridFretSpacing");
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }
        }

        public string GridFretSpacingLabel
        {
            get
            {
                return Strings.DiagramStyleGridFretSpacingLabel;
            }
        }

        public string GridFretSpacingToolTip
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
                    RaisePropertyChanged("GridFretSpacing");
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
                    RaisePropertyChanged("GridStringSpacingIsLocal");
                    RaisePropertyChanged("GridStringSpacing");
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }
        }

        public string GridStringSpacingLabel
        {
            get
            {
                return Strings.DiagramStyleGridStringSpacingLabel;
            }
        }

        public string GridStringSpacingToolTip
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
                    RaisePropertyChanged("GridStringSpacing");
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }
        }

        public string GridBackgroundGroupLabel
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
                    RaisePropertyChanged("GridColorIsLocal");
                    RaisePropertyChanged("GridColor");
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }
        }

        public string GridColorLabel
        {
            get
            {
                return Strings.DiagramStyleGridColorLabel;
            }
        }

        public string GridColorToolTip
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
                    if (null != value)
                    {
                        Style.GridColor = value;
                        ObservableEnums.SortedInsert(Colors, GridColor);
                        RaisePropertyChanged("GridColor");
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
                    RaisePropertyChanged("GridOpacityIsLocal");
                    RaisePropertyChanged("GridOpacity");
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }
        }

        public string GridOpacityLabel
        {
            get
            {
                return Strings.DiagramStyleGridOpacityLabel;
            }
        }

        public string GridOpacityToolTip
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
                    RaisePropertyChanged("GridOpacity");
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }
        }

        public string GridLineGroupLabel
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
                    RaisePropertyChanged("GridLineColorIsLocal");
                    RaisePropertyChanged("GridLineColor");
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }
        }

        public string GridLineColorLabel
        {
            get
            {
                return Strings.DiagramStyleGridLineColorLabel;
            }
        }

        public string GridLineColorToolTip
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
                    if (null != value)
                    {
                        Style.GridLineColor = value;
                        ObservableEnums.SortedInsert(Colors, GridLineColor);
                        RaisePropertyChanged("GridLineColor");
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
                    Style.GridLineColorIsLocal = value;
                    RaisePropertyChanged("GridLineThicknessIsLocal");
                    RaisePropertyChanged("GridLineThickness");
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }
        }

        public string GridLineThicknessLabel
        {
            get
            {
                return Strings.DiagramStyleGridLineThicknessLabel;
            }
        }

        public string GridLineThicknessToolTip
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
                    RaisePropertyChanged("GridLineThickness");
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }
        }

        public string GridNutGroupLabel
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
                    RaisePropertyChanged("GridNutVisibleIsLocal");
                    RaisePropertyChanged("GridNutVisible");
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }
        }

        public string GridNutVisibleLabel
        {
            get
            {
                return Strings.DiagramStyleGridNutVisibleLabel;
            }
        }

        public string GridNutVisibleToolTip
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
                    RaisePropertyChanged("GridNutVisible");
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
                    RaisePropertyChanged("GridNutRatioIsLocal");
                    RaisePropertyChanged("GridNutRatio");
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }
        }

        public string GridNutRatioLabel
        {
            get
            {
                return Strings.DiagramStyleGridNutRatioLabel;
            }
        }

        public string GridNutRatioToolTip
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
                    RaisePropertyChanged("GridNutRatio");
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }
        }

        #endregion

        #region Title

        public string TitleGroupLabel
        {
            get
            {
                return Strings.DiagramStyleTitleGroupLabel;
            }
        }

        #region Layout

        public string TitleLayoutGroupLabel
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
                    RaisePropertyChanged("TitleVisibleIsLocal");
                    RaisePropertyChanged("TitleVisible");
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }
        }

        public string TitleVisibleLabel
        {
            get
            {
                return Strings.DiagramStyleTitleVisibleLabel;
            }
        }

        public string TitleVisibleToolTip
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
                    RaisePropertyChanged("TitleVisible");
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
                    RaisePropertyChanged("TitleGridPaddingIsLocal");
                    RaisePropertyChanged("TitleGridPadding");
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }
        }

        public string TitleGridPaddingLabel
        {
            get
            {
                return Strings.DiagramStyleTitleGridPaddingLabel;
            }
        }

        public string TitleGridPaddingToolTip
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
                    RaisePropertyChanged("TitleGridPadding");
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
                    RaisePropertyChanged("TitleTextAlignmentIsLocal");
                    RaisePropertyChanged("SelectedTitleTextAlignmentIndex");
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }
        }

        public string SelectedTitleTextAlignmentLabel
        {
            get
            {
                return Strings.DiagramStyleTitleTextAlignmentLabel;
            }
        }

        public string SelectedTitleTextAlignmentToolTip
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
                    RaisePropertyChanged("SelectedTitleTextAlignmentIndex");
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }
        }

        public ObservableCollection<string> TitleTextAlignments
        {
            get
            {
                return ObservableEnums.GetHorizontalAlignments();
            }
        }

        #endregion

        #region Font

        public string TitleFontGroupLabel
        {
            get
            {
                return Strings.DiagramStyleTitleFontGroupLabel;
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
                    RaisePropertyChanged("TitleFontFamilyIsLocal");
                    RaisePropertyChanged("TitleFontFamily");
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }
        }

        public string TitleFontFamilyLabel
        {
            get
            {
                return Strings.DiagramStyleTitleFontFamilyLabel;
            }
        }

        public string TitleFontFamilyToolTip
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
                    if (null != value)
                    {
                        Style.TitleFontFamily = value;
                        ObservableEnums.SortedInsert(FontFamilies, TitleFontFamily);
                        RaisePropertyChanged("TitleFontFamily");
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
                    RaisePropertyChanged("TitleTextSizeIsLocal");
                    RaisePropertyChanged("TitleTextSize");
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }
        }

        public string TitleTextSizeLabel
        {
            get
            {
                return Strings.DiagramStyleTitleTextSizeLabel;
            }
        }

        public string TitleTextSizeToolTip
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
                    RaisePropertyChanged("TitleTextSize");
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
                    RaisePropertyChanged("TitleTextSizeModRatioIsLocal");
                    RaisePropertyChanged("TitleTextSizeModRatio");
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }
        }

        public string TitleTextSizeModRatioLabel
        {
            get
            {
                return Strings.DiagramStyleTitleTextSizeModRatioLabel;
            }
        }

        public string TitleTextSizeModRatioToolTip
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
                    RaisePropertyChanged("TitleTextSizeModRatio");
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
                    RaisePropertyChanged("TitleTextStyleIsLocal");
                    RaisePropertyChanged("SelectedTitleTextStyleIndex");
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }
        }

        public string SelectedTitleTextStyleLabel
        {
            get
            {
                return Strings.DiagramStyleTitleTextStyleLabel;
            }
        }

        public string SelectedTitleTextStyleToolTip
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
                    RaisePropertyChanged("SelectedTitleTextStyleIndex");
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }
        }

        public ObservableCollection<string> TitleTextStyles
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
                    RaisePropertyChanged("TitleLabelStyleIsLocal");
                    RaisePropertyChanged("SelectedTitleLabelStyleIndex");
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }
        }

        public string SelectedTitleLabelStyleLabel
        {
            get
            {
                return Strings.DiagramStyleTitleLabelStyleLabel;
            }
        }

        public string SelectedTitleLabelStyleToolTip
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
                    RaisePropertyChanged("SelectedTitleLabelStyleIndex");
                    RaisePropertyChanged("IsTitleTextSizeModRatioEnabled");
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }
        }

        public ObservableCollection<string> TitleLabelStyles
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
                    RaisePropertyChanged("TitleColorIsLocal");
                    RaisePropertyChanged("TitleColor");
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }
        }

        public string TitleColorLabel
        {
            get
            {
                return Strings.DiagramStyleTitleColorLabel;
            }
        }

        public string TitleColorToolTip
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
                    if (null != value)
                    {
                        Style.TitleColor = value;
                        ObservableEnums.SortedInsert(Colors, TitleColor);
                        RaisePropertyChanged("TitleColor");
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
                    RaisePropertyChanged("TitleOpacityIsLocal");
                    RaisePropertyChanged("TitleOpacity");
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }
        }

        public string TitleOpacityLabel
        {
            get
            {
                return Strings.DiagramStyleTitleOpacityLabel;
            }
        }

        public string TitleOpacityToolTip
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
                    RaisePropertyChanged("TitleOpacity");
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

        public string MarksGroupLabel
        {
            get
            {
                return Strings.DiagramStyleDiagramMarksGroupLabel;
            }
        }

        #endregion

        #region Fret Labels

        public string FretLabelsGroupLabel
        {
            get
            {
                return Strings.DiagramStyleDiagramFretLabelsGroupLabel;
            }
        }

        #endregion

        #region Barres

        public string BarresGroupLabel
        {
            get
            {
                return Strings.DiagramStyleDiagramBarresGroupLabel;
            }
        }

        #endregion

        #region Shared

        public ObservableCollection<string> FontFamilies
        {
            get
            {
                if (null == _fontFamiles)
                {
                    _fontFamiles = new ObservableCollection<string>(ObservableEnums.FontFamilies);
                }
                return _fontFamiles;
            }
        }
        private ObservableCollection<string> _fontFamiles;

        public ObservableCollection<string> Colors
        {
            get
            {
                if (null == _colors)
                {
                    _colors = ObservableEnums.GetColors();
                }
                return _colors;
            }
        }
        private ObservableCollection<string> _colors;

        #endregion

        #region ShowEditor

        public RelayCommand ShowEditor
        {
            get
            {
                return new RelayCommand(() =>
                {
                    try
                    {
                        Messenger.Default.Send<ShowDiagramStyleEditorMessage>(new ShowDiagramStyleEditorMessage(this, (changed) =>
                        {
                            if (null != PostEditCallback)
                            {
                                PostEditCallback(changed);
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

        #endregion

        public Action<bool> PostEditCallback
        {
            get
            {
                return _postEditCallback;
            }
            set
            {
                if (null == value)
                {
                    throw new ArgumentNullException();
                }
                _postEditCallback = value;
                RaisePropertyChanged("PostEditCallback");
            }
        }
        private Action<bool> _postEditCallback;

        internal DiagramStyle Style
        {
            get
            {
                return _diagramStyle;
            }
            set
            {
                if (null == value)
                {
                    throw new ArgumentNullException();
                }
                _diagramStyle = value;
            }
        }
        private DiagramStyle _diagramStyle;

        public ObservableDiagramStyle(DiagramStyle diagramStyle) : base()
        {
            if (null == diagramStyle)
            {
                throw new ArgumentNullException("diagramStyle");
            }

            Style = diagramStyle;

            // Pre-seed used fonts
            ObservableEnums.SortedInsert(FontFamilies, Style.TitleFontFamily);

            // Pre-seed used colors
            ObservableEnums.SortedInsert(Colors, Style.DiagramColor);
            ObservableEnums.SortedInsert(Colors, Style.DiagramBorderColor);
            ObservableEnums.SortedInsert(Colors, Style.GridColor);
            ObservableEnums.SortedInsert(Colors, Style.GridLineColor);
            ObservableEnums.SortedInsert(Colors, Style.TitleColor);

            PropertyChanged += ObservableDiagramStyle_PropertyChanged;
        }

        private void ObservableDiagramStyle_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName != "Summary")
            {
                RaisePropertyChanged("Summary");
            }
        }
    }
}
