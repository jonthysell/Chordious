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
                Diagram.Orientation = (DiagramOrientation)(value);
                RaisePropertyChanged("SelectedOrientationIndex");
                Refresh();
            }
        }

        public ObservableCollection<string> Orientations
        {
            get
            {
                return GetOrientations();
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
                Diagram.LabelLayoutModel = (DiagramLabelLayoutModel)(value);
                RaisePropertyChanged("SelectedLabelLayoutModelIndex");
                Refresh();
            }
        }

        public ObservableCollection<string> LabelLayoutModels
        {
            get
            {
                return GetLabelLayoutModels();
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
                    Diagram.DiagramColor = value;
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }

                RaisePropertyChanged("DiagramColor");
                Refresh();
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
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }

                RaisePropertyChanged("DiagramOpacity");
                Refresh();
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
                    Diagram.DiagramBorderColor = value;
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }

                RaisePropertyChanged("DiagramBorderColor");
                Refresh();
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
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }

                RaisePropertyChanged("DiagramBorderThickness");
                Refresh();
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
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
                
                RaisePropertyChanged("NumFrets");
                Refresh();
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
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }

                RaisePropertyChanged("NumStrings");
                Refresh();
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
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }

                RaisePropertyChanged("GridMargin");
                Refresh();
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
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }

                RaisePropertyChanged("GridMarginLeftOverride");
                RaisePropertyChanged("GridMarginLeft");
                Refresh();
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
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }

                RaisePropertyChanged("GridMarginLeft");
                Refresh();
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
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }

                RaisePropertyChanged("GridMarginRightOverride");
                RaisePropertyChanged("GridMarginRight");
                Refresh();
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
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }

                RaisePropertyChanged("GridMarginRight");
                Refresh();
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
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }

                RaisePropertyChanged("GridMarginTopOverride");
                RaisePropertyChanged("GridMarginTop");
                Refresh();
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
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }

                RaisePropertyChanged("GridMarginTop");
                Refresh();
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
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }

                RaisePropertyChanged("GridMarginBottomOverride");
                RaisePropertyChanged("GridMarginBottom");
                Refresh();
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
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }

                RaisePropertyChanged("GridMarginBottom");
                Refresh();
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
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }

                RaisePropertyChanged("GridFretSpacing");
                Refresh();
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
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }

                RaisePropertyChanged("GridStringSpacing");
                Refresh();
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
                    Diagram.GridColor = value;
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }

                RaisePropertyChanged("GridColor");
                Refresh();
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
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }

                RaisePropertyChanged("GridOpacity");
                Refresh();
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
                    Diagram.GridLineColor = value;
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }

                RaisePropertyChanged("GridLineColor");
                Refresh();
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
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }

                RaisePropertyChanged("GridLineThickness");
                Refresh();
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
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }

                RaisePropertyChanged("GridNutVisible");
                Refresh();
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
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }

                RaisePropertyChanged("GridNutRatio");
                Refresh();
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
                Diagram.Title = value;
                RaisePropertyChanged("Title");
                Refresh();
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
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }

                RaisePropertyChanged("TitleGridPadding");
                Refresh();
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
                Diagram.TitleVisible = value;
                RaisePropertyChanged("TitleVisible");
                Refresh();
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
                Diagram.TitleLabelStyle = (DiagramLabelStyle)(value);
                RaisePropertyChanged("SelectedTitleLabelStyleIndex");
                Refresh();
            }
        }

        public ObservableCollection<string> TitleLabelStyles
        {
            get
            {
                return GetDiagramLabelStyles();
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
                    Diagram.TitleColor = value;
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }

                RaisePropertyChanged("TitleColor");
                Refresh();
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
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }

                RaisePropertyChanged("TitleOpacity");
                Refresh();
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
                        Diagram.NewMark(mp);
                    }
                    catch (Exception ex)
                    {
                        ExceptionUtils.HandleException(ex);
                    }

                    Refresh();
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
                return (null != mp && Diagram.ValidPosition(mp) && !Diagram.HasElementAt(mp));
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
                        Messenger.Default.Send<PromptForTextMessage>(new PromptForTextMessage("Mark text:", dm.Text, (text) =>
                            {
                                dm.Text = text;
                            }, true));
                    }
                    catch (Exception ex)
                    {
                        ExceptionUtils.HandleException(ex);
                    }

                    Refresh();
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
                    }
                    catch (Exception ex)
                    {
                        ExceptionUtils.HandleException(ex);
                    }

                    Refresh();
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
                return (null != mp && Diagram.ValidPosition(mp) && Diagram.HasElementAt(mp));
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
                        Diagram.NewFretLabel(flp, flp.Fret.ToString());
                    }
                    catch (Exception ex)
                    {
                        ExceptionUtils.HandleException(ex);
                    }

                    Refresh();
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
                return (null != flp && Diagram.ValidPosition(flp) && !Diagram.HasElementAt(flp));
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
                        Messenger.Default.Send<PromptForTextMessage>(new PromptForTextMessage("Label text:", dfl.Text, (text) =>
                        {
                            dfl.Text = text;
                        }, true));
                    }
                    catch (Exception ex)
                    {
                        ExceptionUtils.HandleException(ex);
                    }

                    Refresh();
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
                    }
                    catch (Exception ex)
                    {
                        ExceptionUtils.HandleException(ex);
                    }

                    Refresh();
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
                return (null != flp && Diagram.ValidPosition(flp) && Diagram.HasElementAt(flp));
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
                    || (CanAddFretLabel || CanEditFretLabel || CanRemoveFretLabel);
            }
        }

        private void RefreshCursor()
        {
            RaisePropertyChanged("CursorInGrid");
            RaisePropertyChanged("ValidCommandsAtCursor");
            RaisePropertyChanged("AddMark");
            RaisePropertyChanged("CanAddMark");
            RaisePropertyChanged("EditMark");
            RaisePropertyChanged("CanEditMark");
            RaisePropertyChanged("RemoveMark");
            RaisePropertyChanged("CanRemoveMark");
            RaisePropertyChanged("AddFretLabel");
            RaisePropertyChanged("CanAddFretLabel");
            RaisePropertyChanged("EditFretLabel");
            RaisePropertyChanged("CanEditFretLabel");
            RaisePropertyChanged("RemoveFretLabel");
            RaisePropertyChanged("CanRemoveFretLabel");
        }

        #endregion

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

        private static ObservableCollection<string> GetDiagramLabelStyles()
        {
            ObservableCollection<string> collection = new ObservableCollection<string>();

            collection.Add("Regular");
            collection.Add("Chord Name");

            return collection;
        }

        private static ObservableCollection<string> GetOrientations()
        {
            ObservableCollection<string> collection = new ObservableCollection<string>();

            collection.Add("Vertical");
            collection.Add("Horizontal");

            return collection;
        }

        private static ObservableCollection<string> GetLabelLayoutModels()
        {
            ObservableCollection<string> collection = new ObservableCollection<string>();

            collection.Add("Overlap");
            collection.Add("Add Horizontal Padding");
            collection.Add("Add Vertical Padding");
            collection.Add("Add Horizontal & Vertical Padding");

            return collection;
        }
    }
}
