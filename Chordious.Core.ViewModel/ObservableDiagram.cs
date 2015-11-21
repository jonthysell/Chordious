// 
// ObservableDiagram.cs
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
using System.Collections.ObjectModel;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;

using com.jonthysell.Chordious.Core;

namespace com.jonthysell.Chordious.Core.ViewModel
{
    public class ObservableDiagram : ObservableObject
    {
        public string SvgText
        {
            get
            {
                return Diagram.ToImageMarkup(ImageMarkupType.SVG);
            }
        }

        public object ImageObject
        {
            get
            {
                return _imageObject;
            }
            private set
            {
                _imageObject = value;
                RaisePropertyChanged("ImageObject");
            }
        }
        private object _imageObject;

        #region Dimensions

        public int TotalWidth
        {
            get
            {
                return (int)(Diagram.TotalWidth() + 0.5);
            }
        }

        public int TotalHeight
        {
            get
            {
                return (int)(Diagram.TotalHeight() + 0.5);
            }
        }

        #endregion

        #region Layout

        public int SelectedOrientationIndex
        {
            get
            {
                return (int)Diagram.Orientation;
            }
            set
            {
                try
                {
                    Diagram.Orientation = (DiagramOrientation)(value);
                    RaisePropertyChanged("SelectedOrientationIndex");
                    Refresh();
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
                return (int)Diagram.LabelLayoutModel;
            }
            set
            {
                try
                {
                    Diagram.LabelLayoutModel = (DiagramLabelLayoutModel)(value);
                    RaisePropertyChanged("SelectedLabelLayoutModelIndex");
                    Refresh();
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
                return Diagram.DiagramColor;
            }
            set
            {
                try
                {
                    if (null != value)
                    {
                        Diagram.DiagramColor = value;
                        ObservableEnums.SortedInsert(Colors, DiagramColor);
                        RaisePropertyChanged("DiagramColor");
                        Refresh();
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
                return Diagram.DiagramOpacity;
            }
            set
            {
                try
                {
                    Diagram.DiagramOpacity = value;
                    RaisePropertyChanged("DiagramOpacity");
                    Refresh();
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
                return Diagram.DiagramBorderColor;
            }
            set
            {
                try
                {
                    if (null != value)
                    {
                        Diagram.DiagramBorderColor = value;
                        ObservableEnums.SortedInsert(Colors, DiagramBorderColor);
                        RaisePropertyChanged("DiagramBorderColor");
                        Refresh();
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
                return Diagram.DiagramBorderThickness;
            }
            set
            {
                try
                {
                    Diagram.DiagramBorderThickness = value;
                    RaisePropertyChanged("DiagramBorderThickness");
                    Refresh();
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }
        }

        #endregion

        #region Grid

        public int NumFrets
        {
            get
            {
                return Diagram.NumFrets;
            }
            set
            {
                try
                {
                    Diagram.NumFrets = value;
                    RaisePropertyChanged("NumFrets");
                    Refresh();
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }
        }

        public int NumStrings
        {
            get
            {
                return Diagram.NumStrings;
            }
            set
            {
                try
                {
                    Diagram.NumStrings = value;
                    RaisePropertyChanged("NumStrings");
                    Refresh();
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }
        }

        public double GridMargin
        {
            get
            {
                return Diagram.GridMargin;
            }
            set
            {
                try
                {
                    Diagram.GridMargin = value;
                    RaisePropertyChanged("GridMargin");
                    Refresh();
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
                return Diagram.GridMarginLeftOverride;
            }
            set
            {
                try
                {
                    Diagram.GridMarginLeftOverride = value;
                    RaisePropertyChanged("GridMarginLeftOverride");
                    RaisePropertyChanged("GridMarginLeft");
                    Refresh();
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
                return Diagram.GridMarginLeft;
            }
            set
            {
                try
                {
                    Diagram.GridMarginLeft = value;
                    RaisePropertyChanged("GridMarginLeft");
                    Refresh();
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
                return Diagram.GridMarginRightOverride;
            }
            set
            {
                try
                {
                    Diagram.GridMarginRightOverride = value;
                    RaisePropertyChanged("GridMarginRightOverride");
                    RaisePropertyChanged("GridMarginRight");
                    Refresh();
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
                return Diagram.GridMarginRight;
            }
            set
            {
                try
                {
                    Diagram.GridMarginRight = value;
                    RaisePropertyChanged("GridMarginRight");
                    Refresh();
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
                return Diagram.GridMarginTopOverride;
            }
            set
            {
                try
                {
                    Diagram.GridMarginTopOverride = value;
                    RaisePropertyChanged("GridMarginTopOverride");
                    RaisePropertyChanged("GridMarginTop");
                    Refresh();
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
                return Diagram.GridMarginTop;
            }
            set
            {
                try
                {
                    Diagram.GridMarginTop = value;
                    RaisePropertyChanged("GridMarginTop");
                    Refresh();
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
                return Diagram.GridMarginBottomOverride;
            }
            set
            {
                try
                {
                    Diagram.GridMarginBottomOverride = value;
                    RaisePropertyChanged("GridMarginBottomOverride");
                    RaisePropertyChanged("GridMarginBottom");
                    Refresh();
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
                return Diagram.GridMarginBottom;
            }
            set
            {
                try
                {
                    Diagram.GridMarginBottom = value;
                    RaisePropertyChanged("GridMarginBottom");
                    Refresh();
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
                return Diagram.GridFretSpacing;
            }
            set
            {
                try
                {
                    Diagram.GridFretSpacing = value;
                    RaisePropertyChanged("GridFretSpacing");
                    Refresh();
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
                return Diagram.GridStringSpacing;
            }
            set
            {
                try
                {
                    Diagram.GridStringSpacing = value;
                    RaisePropertyChanged("GridStringSpacing");
                    Refresh();
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
                return Diagram.GridColor;
            }
            set
            {
                try
                {
                    if (null != value)
                    {
                        Diagram.GridColor = value;
                        ObservableEnums.SortedInsert(Colors, GridColor);
                        RaisePropertyChanged("GridColor");
                        Refresh();
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
                return Diagram.GridOpacity;
            }
            set
            {
                try
                {
                    Diagram.GridOpacity = value;
                    RaisePropertyChanged("GridOpacity");
                    Refresh();
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
                return Diagram.GridLineColor;
            }
            set
            {
                try
                {
                    if (null != value)
                    {
                        Diagram.GridLineColor = value;
                        ObservableEnums.SortedInsert(Colors, GridLineColor);
                        RaisePropertyChanged("GridLineColor");
                        Refresh();
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
                return Diagram.GridLineThickness;
            }
            set
            {
                try
                {
                    Diagram.GridLineThickness = value;
                    RaisePropertyChanged("GridLineThickness");
                    Refresh();
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
                return Diagram.GridNutVisible;
            }
            set
            {
                try
                {
                    Diagram.GridNutVisible = value;
                    RaisePropertyChanged("GridNutVisible");
                    Refresh();
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
                return Diagram.GridNutRatio;
            }
            set
            {
                try
                {
                    Diagram.GridNutRatio = value;
                    RaisePropertyChanged("GridNutRatio");
                    Refresh();
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }
        }

        #endregion

        #region Title

        public string Title
        {
            get
            {
                return Diagram.Title;
            }
            set
            {
                try
                {
                    Diagram.Title = value;
                    RaisePropertyChanged("Title");
                    Refresh();
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }
        }

        public double TitleTextSize
        {
            get
            {
                return Diagram.TitleTextSize;
            }
            set
            {
                try
                {
                    Diagram.TitleTextSize = value;
                    RaisePropertyChanged("TitleTextSize");
                    Refresh();
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
                return Diagram.TitleTextSizeModRatio;
            }
            set
            {
                try
                {
                    Diagram.TitleTextSizeModRatio = value;
                    RaisePropertyChanged("TitleTextSizeModRatio");
                    Refresh();
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
                return Diagram.TitleFontFamily;
            }
            set
            {
                try
                {
                    if (null != value)
                    {
                        Diagram.TitleFontFamily = value;
                        ObservableEnums.SortedInsert(FontFamilies, TitleTextFontFamily);
                        RaisePropertyChanged("TitleTextFontFamily");
                        Refresh();
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
                return (int)Diagram.TitleTextAlignment;
            }
            set
            {
                try
                {
                    Diagram.TitleTextAlignment = (DiagramHorizontalAlignment)(value);
                    RaisePropertyChanged("SelectedTitleTextAlignmentIndex");
                    Refresh();
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
                return (int)Diagram.TitleTextStyle;
            }
            set
            {
                try
                {
                    Diagram.TitleTextStyle = (DiagramTextStyle)(value);
                    RaisePropertyChanged("SelectedTitleTextStyleIndex");
                    Refresh();
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
                return Diagram.TitleGridPadding;
            }
            set
            {
                try
                {
                    Diagram.TitleGridPadding = value;
                    RaisePropertyChanged("TitleGridPadding");
                    Refresh();
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
                return Diagram.TitleVisible;
            }
            set
            {
                try
                {
                    Diagram.TitleVisible = value;
                    RaisePropertyChanged("TitleVisible");
                    Refresh();
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
                return (int)Diagram.TitleLabelStyle;
            }
            set
            {
                try
                {
                    Diagram.TitleLabelStyle = (DiagramLabelStyle)(value);
                    RaisePropertyChanged("SelectedTitleLabelStyleIndex");
                    RaisePropertyChanged("IsTitleTextSizeModRatioEnabled");
                    Refresh();
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
                return Diagram.TitleColor;
            }
            set
            {
                try
                {
                    if (null != value)
                    {
                        Diagram.TitleColor = value;
                        ObservableEnums.SortedInsert(Colors, TitleColor);
                        RaisePropertyChanged("TitleColor");
                        Refresh();
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
                return Diagram.TitleOpacity;
            }
            set
            {
                try
                {
                    Diagram.TitleOpacity = value;
                    RaisePropertyChanged("TitleOpacity");
                    Refresh();
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

        public RelayCommand EditElement
        {
            get
            {
                return new RelayCommand(() =>
                {
                    try
                    {
                        MarkPosition mp = this.MarkPosition;
                        FretLabelPosition flp = this.FretLabelPosition;
                        BarrePosition bp = this.BarrePosition;

                        if (null != mp && Diagram.HasElementAt(mp))
                        {
                            DiagramMark dm = (DiagramMark)Diagram.ElementAt(mp);
                            Messenger.Default.Send<ShowDiagramMarkEditorMessage>(new ShowDiagramMarkEditorMessage(dm, false, (changed) =>
                            {
                                if (changed)
                                {
                                    Refresh();
                                }
                            }));
                        }
                        else if (null != flp && Diagram.HasElementAt(flp))
                        {
                            DiagramFretLabel dfl = (DiagramFretLabel)Diagram.ElementAt(flp);
                            Messenger.Default.Send<ShowDiagramFretLabelEditorMessage>(new ShowDiagramFretLabelEditorMessage(dfl, false, (changed) =>
                            {
                                if (changed)
                                {
                                    Refresh();
                                }
                            }));
                        }
                        else if (null != bp && Diagram.HasElementAt(bp))
                        {
                            DiagramBarre db = (DiagramBarre)Diagram.ElementAt(bp);
                            Messenger.Default.Send<ShowDiagramBarreEditorMessage>(new ShowDiagramBarreEditorMessage(db, false, (changed) =>
                            {
                                if (changed)
                                {
                                    Refresh();
                                }
                            }));
                        }

                    }
                    catch (Exception ex)
                    {
                        ExceptionUtils.HandleException(ex);
                    }

                }, () =>
                {
                    return (CanEditMark || CanEditFretLabel || CanEditBarre);
                });
            }
        }


        #endregion

        #region Marks

        public RelayCommand AddMark
        {
            get
            {
                return new RelayCommand(() =>
                {
                    try
                    {
                        MarkPosition mp = this.MarkPosition;
                        DiagramMark dm = Diagram.NewMark(mp);
                        Messenger.Default.Send<ShowDiagramMarkEditorMessage>(new ShowDiagramMarkEditorMessage(dm, true, (changed) =>
                        {
                            if (changed)
                            {
                                Refresh();
                            }
                            else
                            {
                                Diagram.RemoveMark(mp);
                            }
                        }));
                    }
                    catch (Exception ex)
                    {
                        ExceptionUtils.HandleException(ex);
                    }
                }, () =>
                {
                    return CanAddMark;
                });
            }
        }

        public bool CanAddMark
        {
            get
            {
                MarkPosition mp = this.MarkPosition;
                return (null != mp && Diagram.CanAddNewMarkAt(mp));
            }
        }

        public RelayCommand EditMark
        {
            get
            {
                return new RelayCommand(() =>
                {
                    try
                    {
                        DiagramMark dm = (DiagramMark)Diagram.ElementAt(this.MarkPosition);
                        Messenger.Default.Send<ShowDiagramMarkEditorMessage>(new ShowDiagramMarkEditorMessage(dm, false, (changed) =>
                            {
                                if (changed)
                                {
                                    Refresh();
                                }
                            }));
                    }
                    catch (Exception ex)
                    {
                        ExceptionUtils.HandleException(ex);
                    }
                }, () =>
                {
                    return CanEditMark;
                });
            }
        }

        public bool CanEditMark
        {
            get
            {
                MarkPosition mp = this.MarkPosition;
                return (null != mp && Diagram.ValidPosition(mp) && Diagram.HasElementAt(mp));
            }
        }

        public RelayCommand RemoveMark
        {
            get
            {
                return new RelayCommand(() =>
                {
                    try
                    {
                        MarkPosition mp = this.MarkPosition;
                        Diagram.RemoveMark(mp);
                        Refresh();
                    }
                    catch (Exception ex)
                    {
                        ExceptionUtils.HandleException(ex);
                    }
                }, () =>
                {
                    return CanRemoveMark;
                });
            }
        }

        public bool CanRemoveMark
        {
            get
            {
                MarkPosition mp = this.MarkPosition;
                return (null != mp && Diagram.CanRemoveMarkAt(mp));
            }
        }

        internal MarkPosition MarkPosition
        {
            get
            {
                return (MarkPosition)Diagram.GetPosition<MarkPosition>(CursorX, CursorY);
            }
        }

        #endregion

        #region Fret Labels

        public RelayCommand AddFretLabel
        {
            get
            {
                return new RelayCommand(() =>
                {
                    try
                    {
                        FretLabelPosition flp = this.FretLabelPosition;
                        DiagramFretLabel dfl = Diagram.NewFretLabel(flp, "");
                        Messenger.Default.Send<ShowDiagramFretLabelEditorMessage>(new ShowDiagramFretLabelEditorMessage(dfl, true, (changed) =>
                        {
                            if (changed)
                            {
                                Refresh();
                            }
                            else
                            {
                                Diagram.RemoveFretLabel(flp);
                            }
                        }));
                    }
                    catch (Exception ex)
                    {
                        ExceptionUtils.HandleException(ex);
                    }
                }, () =>
                {
                    return CanAddFretLabel;
                });
            }
        }

        public bool CanAddFretLabel
        {
            get
            {
                FretLabelPosition flp = this.FretLabelPosition;
                return (null != flp && Diagram.CanAddNewFretLabelAt(flp));
            }
        }

        public RelayCommand EditFretLabel
        {
            get
            {
                return new RelayCommand(() =>
                {
                    try
                    {
                        DiagramFretLabel dfl = (DiagramFretLabel)Diagram.ElementAt(this.FretLabelPosition);
                        Messenger.Default.Send<ShowDiagramFretLabelEditorMessage>(new ShowDiagramFretLabelEditorMessage(dfl, false, (changed) =>
                        {
                            if (changed)
                            {
                                Refresh();
                            }
                        }));
                    }
                    catch (Exception ex)
                    {
                        ExceptionUtils.HandleException(ex);
                    }
                }, () =>
                {
                    return CanEditFretLabel;
                });
            }
        }

        public bool CanEditFretLabel
        {
            get
            {
                FretLabelPosition flp = this.FretLabelPosition;
                return (null != flp && Diagram.ValidPosition(flp) && Diagram.HasElementAt(flp));
            }
        }

        public RelayCommand RemoveFretLabel
        {
            get
            {
                return new RelayCommand(() =>
                {
                    try
                    {
                        FretLabelPosition flp = this.FretLabelPosition;
                        Diagram.RemoveFretLabel(flp);
                        Refresh();
                    }
                    catch (Exception ex)
                    {
                        ExceptionUtils.HandleException(ex);
                    }
                }, () =>
                {
                    return CanRemoveFretLabel;
                });
            }
        }

        public bool CanRemoveFretLabel
        {
            get
            {
                FretLabelPosition flp = this.FretLabelPosition;
                return (null != flp && Diagram.CanRemoveFretLabelAt(flp));
            }
        }

        internal FretLabelPosition FretLabelPosition
        {
            get
            {
                return (FretLabelPosition)Diagram.GetPosition<FretLabelPosition>(CursorX, CursorY);
            }
        }

        #endregion

        #region Barres

        public RelayCommand AddBarre
        {
            get
            {
                return new RelayCommand(() =>
                {
                    try
                    {
                        BarrePosition bp = this.BarrePosition;
                        Messenger.Default.Send<PromptForTextMessage>(new PromptForTextMessage(String.Format("Barre across how many strings? ({0}-{1})", 2, bp.Width), bp.Width.ToString(), (widthText) =>
                        {
                            int width = Int32.Parse(widthText);
                            bp = new BarrePosition(bp.Fret, bp.StartString, bp.StartString + width - 1);

                            DiagramBarre db = Diagram.NewBarre(bp);
                            Messenger.Default.Send<ShowDiagramBarreEditorMessage>(new ShowDiagramBarreEditorMessage(db, true, (changed) =>
                            {
                                if (changed)
                                {
                                    Refresh();
                                }
                                else
                                {
                                    Diagram.RemoveBarre(bp);
                                }
                            }));
                        }));
                    }
                    catch (Exception ex)
                    {
                        ExceptionUtils.HandleException(ex);
                    }
                }, () =>
                {
                    return CanAddBarre;
                });
            }
        }

        public bool CanAddBarre
        {
            get
            {
                BarrePosition bp = this.BarrePosition;
                return (null != bp && Diagram.CanAddNewBarreAt(bp));
            }
        }

        public RelayCommand EditBarre
        {
            get
            {
                return new RelayCommand(() =>
                {
                    try
                    {
                        DiagramBarre db = (DiagramBarre)Diagram.ElementAt(this.BarrePosition);
                        Messenger.Default.Send<ShowDiagramBarreEditorMessage>(new ShowDiagramBarreEditorMessage(db, false, (changed) =>
                        {
                            if (changed)
                            {
                                Refresh();
                            }
                        }));
                    }
                    catch (Exception ex)
                    {
                        ExceptionUtils.HandleException(ex);
                    }
                }, () =>
                {
                    return CanEditBarre;
                });
            }
        }

        public bool CanEditBarre
        {
            get
            {
                BarrePosition bp = this.BarrePosition;
                return (null != bp && Diagram.ValidPosition(bp) && Diagram.HasElementAt(bp));
            }
        }

        public RelayCommand RemoveBarre
        {
            get
            {
                return new RelayCommand(() =>
                {
                    try
                    {
                        BarrePosition bp = this.BarrePosition;
                        Diagram.RemoveBarre(bp);
                        Refresh();
                    }
                    catch (Exception ex)
                    {
                        ExceptionUtils.HandleException(ex);
                    }
                }, () =>
                {
                    return CanRemoveBarre;
                });
            }
        }

        public bool CanRemoveBarre
        {
            get
            {
                BarrePosition bp = this.BarrePosition;
                return (null != bp && Diagram.CanRemoveBarreAt(bp));
            }
        }

        internal BarrePosition BarrePosition
        {
            get
            {
                return (BarrePosition)Diagram.GetPosition<BarrePosition>(CursorX, CursorY);
            }
        }

        #endregion

        #region Cursor Info

        public double CursorX
        {
            get
            {
                return _cursorX;
            }
            set
            {
                _cursorX = value;
                RaisePropertyChanged("CursorX");
                RefreshCursor();
            }
        }
        private double _cursorX;

        public double CursorY
        {
            get
            {
                return _cursorY;
            }
            set
            {
                _cursorY = value;
                RaisePropertyChanged("CursorY");
                RefreshCursor();
            }
        }
        private double _cursorY;

        public bool CursorInGrid
        {
            get
            {
                return Diagram.InGrid(CursorX, CursorY);
            }
        }

        public bool ValidCommandsAtCursor
        {
            get
            {
                return (CanAddMark || CanEditMark || CanRemoveMark)
                    || (CanAddFretLabel || CanEditFretLabel || CanRemoveFretLabel)
                    || (CanAddBarre || CanEditBarre || CanRemoveBarre);
            }
        }

        private void RefreshCursor()
        {
            foreach (string property in _cursorProperties)
            {
                RaisePropertyChanged(property);
            }
        }

        public static bool IsCursorProperty(string property)
        {
            if (String.IsNullOrWhiteSpace(property))
            {
                throw new ArgumentNullException("property");
            }

            if (property == "CursorX" || property == "CursorY")
            {
                return true;
            }

            foreach (string cursorProperty in _cursorProperties)
            {
                if (property == cursorProperty)
                {
                    return true;
                }
            }
            return false;
        }

        private static string[] _cursorProperties = new string[] {
            "CursorInGrid",
            "ValidCommandsAtCursor",
            "AddMark",
            "CanAddMark",
            "EditMark",
            "CanEditMark",
            "RemoveMark",
            "CanRemoveMark",
            "AddFretLabel",
            "CanAddFretLabel",
            "EditFretLabel",
            "CanEditFretLabel",
            "RemoveFretLabel",
            "CanRemoveFretLabel",
            "AddBarre",
            "CanAddBarre",
            "EditBarre",
            "CanEditBarre",
            "RemoveBarre",
            "CanRemoveBarre"
        };

        #endregion

        public RelayCommand ShowEditor
        {
            get
            {
                return new RelayCommand(() =>
                {
                    try
                    {
                        Messenger.Default.Send<ShowDiagramEditorMessage>(new ShowDiagramEditorMessage(this, false, (changed) =>
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

        public bool IsEditMode
        {
            get
            {
                return _isEditMode;
            }
            set
            {
                _isEditMode = value;
                RaisePropertyChanged("IsEditMode");
                Refresh();
            }
        }
        private bool _isEditMode = false;

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
        public Action<bool> _postEditCallback;

        public RelayCommand RenderImage
        {
            get
            {
                return new RelayCommand(() =>
                {
                    Render();
                });
            }
        }

        public bool AutoRender
        {
            get
            {
                return _autorender;
            }
            set
            {
                _autorender = value;
                RaisePropertyChanged("AutoRender");
            }
        }
        private bool _autorender;

        internal Diagram Diagram
        {
            get
            {
                return _diagram;
            }
            set
            {
                if (null == value)
                {
                    throw new ArgumentNullException();
                }
                _diagram = value;
                Refresh();
            }
        }
        private Diagram _diagram;

        public ObservableDiagram(Diagram diagram, bool autoRender = true) : base()
        {
            if (null == diagram)
            {
                throw new ArgumentNullException("diagram");
            }

            AutoRender = autoRender;
            Diagram = diagram;
        }

        public void Refresh()
        {
            RaisePropertyChanged("SvgText");
            RaisePropertyChanged("TotalHeight");
            RaisePropertyChanged("TotalWidth");

            if (AutoRender)
            {
                Render();
            }
        }

        protected void Render()
        {
            AppViewModel.Instance.DoOnUIThread(() =>
            {
                ImageObject = AppViewModel.Instance.SvgTextToImage(SvgText, TotalWidth, TotalHeight, IsEditMode);
            });
        }
    }
}
