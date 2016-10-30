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
                    Style.GridLineThicknessIsLocal = value;
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

        #region Text

        public string TitleTextGroupLabel
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

        public string SelectedMarkTypeLabel
        {
            get
            {
                return Strings.DiagramStyleMarkTypeLabel;
            }
        }

        public string SelectedMarkTypeToolTip
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
                    RaisePropertyChanged("SelectedMarkTypeIndex");
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }
        }

        public ObservableCollection<string> MarkTypes
        {
            get
            {
                return ObservableEnums.GetMarkTypes();
            }
        }

        public string MarksGroupLabel
        {
            get
            {
                return Strings.DiagramStyleDiagramMarksGroupLabel;
            }
        }

        #region Background

        public string MarkBackgroundGroupLabel
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
                    RaisePropertyChanged("MarkShapeIsLocal");
                    RaisePropertyChanged("SelectedMarkShapeIndex");
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }
        }

        public string SelectedMarkShapeLabel
        {
            get
            {
                return Strings.DiagramStyleMarkShapeLabel;
            }
        }

        public string SelectedMarkShapeToolTip
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
                    RaisePropertyChanged("SelectedMarkShapeIndex");
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }
        }

        public ObservableCollection<string> MarkShapes
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
                    RaisePropertyChanged("MarkVisibleIsLocal");
                    RaisePropertyChanged("MarkVisible");
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }
        }

        public string MarkVisibleLabel
        {
            get
            {
                return Strings.DiagramStyleMarkVisibleLabel;
            }
        }

        public string MarkVisibleToolTip
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
                    RaisePropertyChanged("MarkVisible");
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
                    RaisePropertyChanged("MarkColorIsLocal");
                    RaisePropertyChanged("MarkColor");
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }
        }

        public string MarkColorLabel
        {
            get
            {
                return Strings.DiagramStyleMarkColorLabel;
            }
        }

        public string MarkColorToolTip
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
                    if (null != value)
                    {
                        MarkStyle.MarkColor = value;
                        ObservableEnums.SortedInsert(Colors, MarkColor);
                        RaisePropertyChanged("MarkColor");
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
                    RaisePropertyChanged("MarkOpacityIsLocal");
                    RaisePropertyChanged("MarkOpacity");
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }
        }

        public string MarkOpacityLabel
        {
            get
            {
                return Strings.DiagramStyleMarkOpacityLabel;
            }
        }

        public string MarkOpacityToolTip
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
                    RaisePropertyChanged("MarkOpacity");
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
                    RaisePropertyChanged("MarkRadiusRatioIsLocal");
                    RaisePropertyChanged("MarkRadiusRatio");
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }
        }

        public string MarkRadiusRatioLabel
        {
            get
            {
                return Strings.DiagramStyleMarkRadiusRatioLabel;
            }
        }

        public string MarkRadiusRatioToolTip
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
                    RaisePropertyChanged("MarkRadiusRatio");
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }
        }

        #endregion

        #region Border

        public string MarkBorderGroupLabel
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
                    RaisePropertyChanged("MarkBorderColorIsLocal");
                    RaisePropertyChanged("MarkBorderColor");
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }
        }

        public string MarkBorderColorLabel
        {
            get
            {
                return Strings.DiagramStyleMarkBorderColorLabel;
            }
        }

        public string MarkBorderColorToolTip
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
                    RaisePropertyChanged("MarkBorderColor");
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
                    RaisePropertyChanged("MarkBorderThicknessIsLocal");
                    RaisePropertyChanged("MarkBorderThickness");
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }
        }

        public string MarkBorderThicknessLabel
        {
            get
            {
                return Strings.DiagramStyleMarkBorderThicknessLabel;
            }
        }

        public string MarkBorderThicknessToolTip
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
                    RaisePropertyChanged("MarkBorderThickness");
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }
        }

        #endregion

        #region Text

        public string MarkTextGroupLabel
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
                    RaisePropertyChanged("MarkTextAlignmentIsLocal");
                    RaisePropertyChanged("SelectedMarkTextAlignmentIndex");
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }
        }

        public string SelectedMarkTextAlignmentLabel
        {
            get
            {
                return Strings.DiagramStyleMarkTextAlignmentLabel;
            }
        }

        public string SelectedMarkTextAlignmentToolTip
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
                    RaisePropertyChanged("SelectedMarkTextAlignmentIndex");
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }
        }

        public ObservableCollection<string> MarkTextAlignments
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
                    RaisePropertyChanged("MarkTextVisibleIsLocal");
                    RaisePropertyChanged("MarkTextVisible");
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }
        }

        public string MarkTextVisibleLabel
        {
            get
            {
                return Strings.DiagramStyleMarkTextVisibleLabel;
            }
        }

        public string MarkTextVisibleToolTip
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
                    RaisePropertyChanged("MarkTextVisible");
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
                    RaisePropertyChanged("MarkTextColorIsLocal");
                    RaisePropertyChanged("MarkTextColor");
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }
        }

        public string MarkTextColorLabel
        {
            get
            {
                return Strings.DiagramStyleMarkTextColorLabel;
            }
        }

        public string MarkTextColorToolTip
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
                    if (null != value)
                    {
                        MarkStyle.MarkTextColor = value;
                        ObservableEnums.SortedInsert(Colors, MarkTextColor);
                        RaisePropertyChanged("MarkTextColor");
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
                    RaisePropertyChanged("MarkTextOpacityIsLocal");
                    RaisePropertyChanged("MarkTextOpacity");
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }
        }

        public string MarkTextOpacityLabel
        {
            get
            {
                return Strings.DiagramStyleMarkTextOpacityLabel;
            }
        }

        public string MarkTextOpacityToolTip
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
                    RaisePropertyChanged("MarkTextOpacity");
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
                    RaisePropertyChanged("MarkFontFamilyIsLocal");
                    RaisePropertyChanged("MarkFontFamily");
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }
        }

        public string MarkFontFamilyLabel
        {
            get
            {
                return Strings.DiagramStyleMarkFontFamilyLabel;
            }
        }

        public string MarkFontFamilyToolTip
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
                    if (null != value)
                    {
                        MarkStyle.MarkFontFamily = value;
                        ObservableEnums.SortedInsert(FontFamilies, MarkFontFamily);
                        RaisePropertyChanged("MarkFontFamily");
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
                    RaisePropertyChanged("MarkTextStyleIsLocal");
                    RaisePropertyChanged("SelectedMarkTextStyleIndex");
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }
        }

        public string SelectedMarkTextStyleLabel
        {
            get
            {
                return Strings.DiagramStyleMarkTextStyleLabel;
            }
        }

        public string SelectedMarkTextStyleToolTip
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
                    RaisePropertyChanged("SelectedMarkTextStyleIndex");
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }
        }

        public ObservableCollection<string> MarkTextStyles
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
                    RaisePropertyChanged("MarkTextSizeRatioIsLocal");
                    RaisePropertyChanged("MarkTextSizeRatio");
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }
        }

        public string MarkTextSizeRatioLabel
        {
            get
            {
                return Strings.DiagramStyleMarkTextSizeRatioLabel;
            }
        }

        public string MarkTextSizeRatioToolTip
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
                    RaisePropertyChanged("MarkTextSizeRatio");
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

        public string FretLabelsGroupLabel
        {
            get
            {
                return Strings.DiagramStyleDiagramFretLabelsGroupLabel;
            }
        }

        #region Layout

        public string FretLabelLayoutGroupLabel
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
                    RaisePropertyChanged("FretLabelTextAlignmentIsLocal");
                    RaisePropertyChanged("SelectedFretLabelTextAlignmentIndex");
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }
        }

        public string SelectedFretLabelTextAlignmentLabel
        {
            get
            {
                return Strings.DiagramStyleFretLabelTextAlignmentLabel;
            }
        }

        public string SelectedFretLabelTextAlignmentToolTip
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
                    RaisePropertyChanged("SelectedFretLabelTextAlignmentIndex");
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }
        }

        public ObservableCollection<string> FretLabelTextAlignments
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
                    RaisePropertyChanged("FretLabelTextVisibleIsLocal");
                    RaisePropertyChanged("FretLabelTextVisible");
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }
        }

        public string FretLabelTextVisibleLabel
        {
            get
            {
                return Strings.DiagramStyleFretLabelTextVisibleLabel;
            }
        }

        public string FretLabelTextVisibleToolTip
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
                    RaisePropertyChanged("FretLabelTextVisible");
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
                    RaisePropertyChanged("FretLabelGridPaddingIsLocal");
                    RaisePropertyChanged("FretLabelGridPadding");
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }
        }

        public string FretLabelGridPaddingLabel
        {
            get
            {
                return Strings.DiagramStyleFretLabelGridPaddingLabel;
            }
        }

        public string FretLabelGridPaddingToolTip
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
                    RaisePropertyChanged("FretLabelGridPadding");
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }
        }

        #endregion

        #region Text

        public string FretLabelTextGroupLabel
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
                    RaisePropertyChanged("FretLabelTextColorIsLocal");
                    RaisePropertyChanged("FretLabelTextColor");
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }
        }

        public string FretLabelTextColorLabel
        {
            get
            {
                return Strings.DiagramStyleFretLabelTextColorLabel;
            }
        }

        public string FretLabelTextColorToolTip
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
                    if (null != value)
                    {
                        Style.FretLabelTextColor = value;
                        ObservableEnums.SortedInsert(Colors, FretLabelTextColor);
                        RaisePropertyChanged("FretLabelTextColor");
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
                    RaisePropertyChanged("FretLabelTextOpacityIsLocal");
                    RaisePropertyChanged("FretLabelTextOpacity");
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }
        }

        public string FretLabelTextOpacityLabel
        {
            get
            {
                return Strings.DiagramStyleFretLabelTextOpacityLabel;
            }
        }

        public string FretLabelTextOpacityToolTip
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
                    RaisePropertyChanged("FretLabelTextOpacity");
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
                    RaisePropertyChanged("FretLabelFontFamilyIsLocal");
                    RaisePropertyChanged("FretLabelFontFamily");
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }
        }

        public string FretLabelFontFamilyLabel
        {
            get
            {
                return Strings.DiagramStyleFretLabelFontFamilyLabel;
            }
        }

        public string FretLabelFontFamilyToolTip
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
                    if (null != value)
                    {
                        Style.FretLabelFontFamily = value;
                        ObservableEnums.SortedInsert(FontFamilies, FretLabelFontFamily);
                        RaisePropertyChanged("FretLabelFontFamily");
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
                    RaisePropertyChanged("FretLabelTextStyleIsLocal");
                    RaisePropertyChanged("SelectedFretLabelTextStyleIndex");
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }
        }

        public string SelectedFretLabelTextStyleLabel
        {
            get
            {
                return Strings.DiagramStyleFretLabelTextStyleLabel;
            }
        }

        public string SelectedFretLabelTextStyleToolTip
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
                    RaisePropertyChanged("SelectedFretLabelTextStyleIndex");
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }
        }

        public ObservableCollection<string> FretLabelTextStyles
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
                    RaisePropertyChanged("FretLabelTextSizeRatioIsLocal");
                    RaisePropertyChanged("FretLabelTextSizeRatio");
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }
        }

        public string FretLabelTextSizeRatioLabel
        {
            get
            {
                return Strings.DiagramStyleFretLabelTextSizeRatioLabel;
            }
        }

        public string FretLabelTextSizeRatioToolTip
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
                    RaisePropertyChanged("FretLabelTextSizeRatio");
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
                    RaisePropertyChanged("FretLabelTextWidthRatioIsLocal");
                    RaisePropertyChanged("FretLabelTextWidthRatio");
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }
        }

        public string FretLabelTextWidthRatioLabel
        {
            get
            {
                return Strings.DiagramStyleFretLabelTextWidthRatioLabel;
            }
        }

        public string FretLabelTextWidthRatioToolTip
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
                    RaisePropertyChanged("FretLabelTextWidthRatio");
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

        public string BarresGroupLabel
        {
            get
            {
                return Strings.DiagramStyleDiagramBarresGroupLabel;
            }
        }

        #region Layout

        public string BarreLayoutGroupLabel
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
                    RaisePropertyChanged("BarreVisibleIsLocal");
                    RaisePropertyChanged("BarreVisible");
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }
        }

        public string BarreVisibleLabel
        {
            get
            {
                return Strings.DiagramStyleBarreVisibleLabel;
            }
        }

        public string BarreVisibleToolTip
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
                    RaisePropertyChanged("BarreVisible");
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
                    RaisePropertyChanged("BarreVerticalAlignmentIsLocal");
                    RaisePropertyChanged("SelectedBarreVerticalAlignmentIndex");
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }
        }

        public string SelectedBarreVerticalAlignmentLabel
        {
            get
            {
                return Strings.DiagramStyleBarreVerticalAlignmentLabel;
            }
        }

        public string SelectedBarreVerticalAlignmentToolTip
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
                    RaisePropertyChanged("SelectedBarreVerticalAlignmentIndex");
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }
        }

        public ObservableCollection<string> BarreVerticalAlignments
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
                    RaisePropertyChanged("BarreStackIsLocal");
                    RaisePropertyChanged("SelectedBarreStackIndex");
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }
        }

        public string SelectedBarreStackLabel
        {
            get
            {
                return Strings.DiagramStyleBarreStackLabel;
            }
        }

        public string SelectedBarreStackToolTip
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
                    RaisePropertyChanged("SelectedBarreStackIndex");
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }
        }

        public ObservableCollection<string> BarreStacks
        {
            get
            {
                return ObservableEnums.GetBarreStacks();
            }
        }

        #endregion

        #region Style

        public string BarreStyleGroupLabel
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
                    RaisePropertyChanged("BarreArcRatioIsLocal");
                    RaisePropertyChanged("BarreArcRatio");
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }
        }

        public string BarreArcRatioLabel
        {
            get
            {
                return Strings.DiagramStyleBarreArcRatioLabel;
            }
        }

        public string BarreArcRatioToolTip
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
                    RaisePropertyChanged("BarreArcRatio");
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
                    RaisePropertyChanged("BarreOpacityIsLocal");
                    RaisePropertyChanged("BarreOpacity");
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }
        }

        public string BarreOpacityLabel
        {
            get
            {
                return Strings.DiagramStyleBarreOpacityLabel;
            }
        }

        public string BarreOpacityToolTip
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
                    RaisePropertyChanged("BarreOpacity");
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
                    RaisePropertyChanged("BarreLineColorIsLocal");
                    RaisePropertyChanged("BarreLineColor");
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }
        }

        public string BarreLineColorLabel
        {
            get
            {
                return Strings.DiagramStyleBarreLineColorLabel;
            }
        }

        public string BarreLineColorToolTip
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
                    RaisePropertyChanged("BarreLineColor");
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
                    RaisePropertyChanged("BarreLineThicknessIsLocal");
                    RaisePropertyChanged("BarreLineThickness");
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }
        }

        public string BarreLineThicknessLabel
        {
            get
            {
                return Strings.DiagramStyleBarreLineThicknessLabel;
            }
        }

        public string BarreLineThicknessToolTip
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
                    RaisePropertyChanged("BarreLineThickness");
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

        #region Reset

        public string ResetLabel
        {
            get
            {
                return Strings.DiagramStyleResetLabel;
            }
        }

        public string ResetToolTip
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
                return new RelayCommand(() =>
                {
                    try
                    {
                        Messenger.Default.Send<ConfirmationMessage>(new ConfirmationMessage(Strings.DiagramStyleResetPrompt, (confirmed) =>
                        {
                            if (confirmed)
                            {
                                Style.Clear();
                                RaisePropertyChanged("");
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
            private set
            {
                if (null == value)
                {
                    throw new ArgumentNullException();
                }
                _diagramStyle = value;
                MarkStyle = new DiagramMarkStyleWrapper(value);
            }
        }
        private DiagramStyle _diagramStyle;

        internal DiagramMarkStyleWrapper MarkStyle { get; private set; }

        public ObservableDiagramStyle(DiagramStyle diagramStyle, DiagramMarkStyleWrapper diagramMarkStyle = null) : base()
        {
            if (null == diagramStyle)
            {
                throw new ArgumentNullException("diagramStyle");
            }

            Style = diagramStyle;

            if (null != diagramMarkStyle)
            {
                if (diagramMarkStyle.Style != diagramStyle)
                {
                    throw new ArgumentException("diagramMarkStyle");
                }

                MarkStyle = diagramMarkStyle;
            }

            // Pre-seed used fonts
            ObservableEnums.SortedInsert(FontFamilies, Style.TitleFontFamily);
            MarkStyle.ForEachMarkType(() =>
            {
                ObservableEnums.SortedInsert(FontFamilies, MarkStyle.MarkFontFamily);
            });
            ObservableEnums.SortedInsert(FontFamilies, Style.FretLabelFontFamily);

            // Pre-seed used colors
            ObservableEnums.SortedInsert(Colors, Style.DiagramColor);
            ObservableEnums.SortedInsert(Colors, Style.DiagramBorderColor);
            ObservableEnums.SortedInsert(Colors, Style.GridColor);
            ObservableEnums.SortedInsert(Colors, Style.GridLineColor);
            ObservableEnums.SortedInsert(Colors, Style.TitleColor);
            MarkStyle.ForEachMarkType(() =>
            {
                ObservableEnums.SortedInsert(Colors, MarkStyle.MarkColor);
                ObservableEnums.SortedInsert(Colors, MarkStyle.MarkTextColor);
                ObservableEnums.SortedInsert(Colors, MarkStyle.MarkBorderColor);
            });
            ObservableEnums.SortedInsert(Colors, Style.FretLabelTextColor);
            ObservableEnums.SortedInsert(Colors, Style.BarreLineColor);

            PropertyChanged += ObservableDiagramStyle_PropertyChanged;
            MarkStyle.MarkTypeChanged += MarkStyle_MarkTypeChanged;
        }

        private void ObservableDiagramStyle_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName != "Summary")
            {
                RaisePropertyChanged("Summary");
                if (e.PropertyName != "Reset" && e.PropertyName != "LocalCount")
                {
                    RaisePropertyChanged("LocalCount");
                    RaisePropertyChanged("Reset");
                }
            }
        }

        private void MarkStyle_MarkTypeChanged(DiagramMarkType obj)
        {
            RaisePropertyChanged("MarkShapeIsLocal");
            RaisePropertyChanged("SelectedMarkShapeIndex");
            RaisePropertyChanged("MarkVisibleIsLocal");
            RaisePropertyChanged("MarkVisible");
            RaisePropertyChanged("MarkColorIsLocal");
            RaisePropertyChanged("MarkColor");
            RaisePropertyChanged("MarkOpacityIsLocal");
            RaisePropertyChanged("MarkOpacity");
            RaisePropertyChanged("MarkRadiusRatioIsLocal");
            RaisePropertyChanged("MarkRadiusRatio");
            RaisePropertyChanged("MarkBorderColorIsLocal");
            RaisePropertyChanged("MarkBorderColor");
            RaisePropertyChanged("MarkBorderThicknessIsLocal");
            RaisePropertyChanged("MarkBorderThickness");
            RaisePropertyChanged("MarkTextAlignmentIsLocal");
            RaisePropertyChanged("SelectedMarkTextAlignmentIndex");
            RaisePropertyChanged("MarkTextVisibleIsLocal");
            RaisePropertyChanged("MarkTextVisible");
            RaisePropertyChanged("MarkTextColorIsLocal");
            RaisePropertyChanged("MarkTextColor");
            RaisePropertyChanged("MarkTextOpacityIsLocal");
            RaisePropertyChanged("MarkTextOpacity");
            RaisePropertyChanged("MarkFontFamilyIsLocal");
            RaisePropertyChanged("MarkFontFamily");
            RaisePropertyChanged("MarkTextStyleIsLocal");
            RaisePropertyChanged("SelectedMarkTextStyleIndex");
            RaisePropertyChanged("MarkTextSizeRatioIsLocal");
            RaisePropertyChanged("MarkTextSizeRatio");
        }
    }
}
