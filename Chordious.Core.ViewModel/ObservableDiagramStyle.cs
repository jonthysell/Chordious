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

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;

using com.jonthysell.Chordious.Core;

using com.jonthysell.Chordious.Core.ViewModel.Resources;

namespace com.jonthysell.Chordious.Core.ViewModel
{
    public class ObservableDiagramStyle : ObservableObject
    {
        #region Layout

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

        #region Grid

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

        public bool GridMarginLeftOverride
        {
            get
            {
                return Style.GridMarginLeftOverride;
            }
            set
            {
                try
                {
                    Style.GridMarginLeftOverride = value;
                    RaisePropertyChanged("GridMarginLeftOverride");
                    RaisePropertyChanged("GridMarginLeft");
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
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

        public bool GridMarginRightOverride
        {
            get
            {
                return Style.GridMarginRightOverride;
            }
            set
            {
                try
                {
                    Style.GridMarginRightOverride = value;
                    RaisePropertyChanged("GridMarginRightOverride");
                    RaisePropertyChanged("GridMarginRight");
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
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

        public bool GridMarginTopOverride
        {
            get
            {
                return Style.GridMarginTopOverride;
            }
            set
            {
                try
                {
                    Style.GridMarginTopOverride = value;
                    RaisePropertyChanged("GridMarginTopOverride");
                    RaisePropertyChanged("GridMarginTop");
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
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

        public bool GridMarginBottomOverride
        {
            get
            {
                return Style.GridMarginBottomOverride;
            }
            set
            {
                try
                {
                    Style.GridMarginBottomOverride = value;
                    RaisePropertyChanged("GridMarginBottomOverride");
                    RaisePropertyChanged("GridMarginBottom");
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
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

        public bool IsTitleTextSizeModRatioEnabled
        {
            get
            {
                return (DiagramLabelStyle)SelectedTitleLabelStyleIndex == DiagramLabelStyle.ChordName;
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

        public string TitleTextFontFamily
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
                        ObservableEnums.SortedInsert(FontFamilies, TitleTextFontFamily);
                        RaisePropertyChanged("TitleTextFontFamily");
                    }
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
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
        }
    }
}
