// 
// DiagramMarkEditorViewModel.cs
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
using System.Collections.Specialized;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;

using com.jonthysell.Chordious.Core;

namespace com.jonthysell.Chordious.Core.ViewModel
{
    public class DiagramMarkEditorViewModel : ViewModelBase
    {
        public AppViewModel AppVM
        {
            get
            {
                return AppViewModel.Instance;
            }
        }

        public string Title
        {
            get
            {
                return "Diagram Mark Editor" + (Dirty ? "*" : "");
            }
        }

        public string Text
        {
            get
            {
                return _text;
            }
            set
            {
                try
                {
                    _text = value;
                    Dirty = true;
                    RaisePropertyChanged("Text");
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }
        }
        private string _text;

        public int SelectedMarkTypeIndex
        {
            get
            {
                return (int)_diagramMarkType;
            }
            set
            {
                try
                {
                    _diagramMarkType = (DiagramMarkType)value;
                    Dirty = true;
                    RaisePropertyChanged("SelectedMarkTypeIndex");
                    RefreshProperties();
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }
        }
        private DiagramMarkType _diagramMarkType;

        public ObservableCollection<string> MarkTypes
        {
            get
            {
                return ObservableEnums.GetMarkTypes();
            }
        }

        public int SelectedMarkShapeIndex
        {
            get
            {
                return (int)StyleBuffer.MarkShapeGet(_diagramMarkType);
            }
            set
            {
                try
                {
                    StyleBuffer.MarkShapeSet((DiagramMarkShape)value, _diagramMarkType);
                    Dirty = true;
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

        public bool Visible
        {
            get
            {
                return StyleBuffer.MarkVisibleGet(_diagramMarkType);
            }
            set
            {
                try
                {
                    StyleBuffer.MarkVisibleSet(value, _diagramMarkType);
                    Dirty = true;
                    RaisePropertyChanged("Visible");
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }
        }

        public string Color
        {
            get
            {
                return StyleBuffer.MarkColorGet(_diagramMarkType);
            }
            set
            {
                try
                {
                    if (null != value)
                    {
                        StyleBuffer.MarkColorSet(value, _diagramMarkType);
                        ObservableEnums.SortedInsert(Colors, Color);
                        Dirty = true;
                        RaisePropertyChanged("Color");
                    }
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }
        }

        public double Opacity
        {
            get
            {
                return StyleBuffer.MarkOpacityGet(_diagramMarkType);
            }
            set
            {
                try
                {
                    StyleBuffer.MarkOpacitySet(value, _diagramMarkType);
                    Dirty = true;
                    RaisePropertyChanged("Opacity");
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }
        }

        public int SelectedTextStyleIndex
        {
            get
            {
                return (int)StyleBuffer.MarkTextStyleGet(_diagramMarkType);
            }
            set
            {
                try
                {
                    StyleBuffer.MarkTextStyleSet((DiagramTextStyle)value, _diagramMarkType);
                    Dirty = true;
                    RaisePropertyChanged("SelectedTextStyleIndex");
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }
        }

        public ObservableCollection<string> TextStyles
        {
            get
            {
                return ObservableEnums.GetTextStyles();
            }
        }

        public int SelectedTextAlignmentIndex
        {
            get
            {
                return (int)StyleBuffer.MarkTextAlignmentGet(_diagramMarkType);
            }
            set
            {
                try
                {
                    StyleBuffer.MarkTextAlignmentSet((DiagramHorizontalAlignment)value, _diagramMarkType);
                    Dirty = true;
                    RaisePropertyChanged("SelectedTextAlignmentIndex");
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }
        }

        public ObservableCollection<string> TextAlignments
        {
            get
            {
                return ObservableEnums.GetTextAlignments();
            }
        }

        public double TextSizeRatio
        {
            get
            {
                return StyleBuffer.MarkTextSizeRatioGet(_diagramMarkType);
            }
            set
            {
                try
                {
                    StyleBuffer.MarkTextSizeRatioSet(value, _diagramMarkType);
                    Dirty = true;
                    RaisePropertyChanged("TextSizeRatio");
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }
        }

        public bool TextVisible
        {
            get
            {
                return StyleBuffer.MarkTextVisibleGet(_diagramMarkType);
            }
            set
            {
                try
                {
                    StyleBuffer.MarkTextVisibleSet(value, _diagramMarkType);
                    Dirty = true;
                    RaisePropertyChanged("TextVisible");
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }
        }

        public string TextColor
        {
            get
            {
                return StyleBuffer.MarkTextColorGet(_diagramMarkType);
            }
            set
            {
                try
                {
                    if (null != value)
                    {
                        StyleBuffer.MarkTextColorSet(value, _diagramMarkType);
                        ObservableEnums.SortedInsert(Colors, TextColor);
                        Dirty = true;
                        RaisePropertyChanged("TextColor");
                    }
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }
        }

        public double TextOpacity
        {
            get
            {
                return StyleBuffer.MarkTextOpacityGet(_diagramMarkType);
            }
            set
            {
                try
                {
                    StyleBuffer.MarkTextOpacitySet(value, _diagramMarkType);
                    Dirty = true;
                    RaisePropertyChanged("TextOpacity");
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }
        }

        public string TextFontFamily
        {
            get
            {
                return StyleBuffer.MarkFontFamilyGet(_diagramMarkType);
            }
            set
            {
                try
                {
                    if (null != value)
                    {
                        StyleBuffer.MarkFontFamilySet(value, _diagramMarkType);
                        ObservableEnums.SortedInsert(FontFamilies, TextFontFamily);
                        Dirty = true;
                        RaisePropertyChanged("TextFontFamily");
                    }
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }
        }

        public double RadiusRatio
        {
            get
            {
                return StyleBuffer.MarkRadiusRatioGet(_diagramMarkType);
            }
            set
            {
                try
                {
                    StyleBuffer.MarkRadiusRatioSet(value, _diagramMarkType);
                    Dirty = true;
                    RaisePropertyChanged("RadiusRatio");
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }
        }

        public string BorderColor
        {
            get
            {
                return StyleBuffer.MarkBorderColorGet(_diagramMarkType);
            }
            set
            {
                try
                {
                    if (null != value)
                    {
                        StyleBuffer.MarkBorderColorSet(value, _diagramMarkType);
                        ObservableEnums.SortedInsert(Colors, BorderColor);
                        Dirty = true;
                        RaisePropertyChanged("BorderColor");
                    }
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }
        }

        public double BorderThickness
        {
            get
            {
                return StyleBuffer.MarkBorderThicknessGet(_diagramMarkType);
            }
            set
            {
                try
                {
                    StyleBuffer.MarkBorderThicknessSet(value, _diagramMarkType);
                    Dirty = true;
                    RaisePropertyChanged("BorderThickness");
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }
        }

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

        public RelayCommand Apply
        {
            get
            {
                return new RelayCommand(() =>
                {
                    try
                    {
                        ApplyChanges();
                    }
                    catch (Exception ex)
                    {
                        ExceptionUtils.HandleException(ex);
                    }
                }, () =>
                {
                    return Dirty;
                });
            }
        }

        public RelayCommand Accept
        {
            get
            {
                return new RelayCommand(() =>
                {
                    try
                    {
                        ApplyChangesOnClose = true;
                        if (null != RequestClose)
                        {
                            RequestClose();
                        }
                    }
                    catch (Exception ex)
                    {
                        ExceptionUtils.HandleException(ex);
                    }
                });
            }
        }

        public RelayCommand Cancel
        {
            get
            {
                return new RelayCommand(() =>
                {
                    try
                    {
                        ApplyChangesOnClose = false;
                        if (null != RequestClose)
                        {
                            RequestClose();
                        }
                    }
                    catch (Exception ex)
                    {
                        ExceptionUtils.HandleException(ex);
                    }
                });
            }
        }

        public event Action RequestClose;

        public bool ApplyChangesOnClose
        {
            get
            {
                return _applyChangesOnClose;
            }
            private set
            {
                _applyChangesOnClose = value;
                RaisePropertyChanged("ApplyChangesOnClose");
            }
        }
        private bool _applyChangesOnClose = false;

        public bool Dirty
        {
            get
            {
                return _dirty;
            }
            private set
            {
                _dirty = value;
                RaisePropertyChanged("Dirty");
                RaisePropertyChanged("Title");
                RaisePropertyChanged("Apply");
            }
        }
        private bool _dirty = false;

        public bool DiagramMarkChanged
        {
            get
            {
                return _diagramMarkChanged;
            }
            private set
            {
                _diagramMarkChanged = value;
                RaisePropertyChanged("DiagramMarkChanged");
            }
        }
        private bool _diagramMarkChanged = false;

        internal DiagramMark DiagramMark { get; private set; }
        internal DiagramStyle StyleBuffer { get; private set; }

        public DiagramMarkEditorViewModel(DiagramMark diagramMark, bool isNew)
        {
            if (null == diagramMark)
            {
                throw new ArgumentNullException("diagramMark");
            }

            DiagramMark = diagramMark;

            // Buffer values
            _text = DiagramMark.Text;
            _diagramMarkType = DiagramMark.Type;

            StyleBuffer = new DiagramStyle(DiagramMark.Style, "DiagramMarkEditor");

            // Pre-seed used fonts
            ObservableEnums.SortedInsert(FontFamilies, TextFontFamily);

            // Pre-seed used colors
            ObservableEnums.SortedInsert(Colors, Color);
            ObservableEnums.SortedInsert(Colors, TextColor);
            ObservableEnums.SortedInsert(Colors, BorderColor);

            if (isNew)
            {
                Dirty = true;
            }
        }

        public bool ProcessClose()
        {
            if (ApplyChangesOnClose)
            {
                ApplyChanges();
            }
            return DiagramMarkChanged;
        }

        private void ApplyChanges()
        {
            DiagramMark.Text = _text;
            DiagramMark.Type = _diagramMarkType;

            StyleBuffer.SetParent();
            StyleBuffer.Clear();

            Dirty = false;
            DiagramMarkChanged = true;
        }

        private void RefreshProperties()
        {
            RaisePropertyChanged("SelectedMarkShapeIndex");
            RaisePropertyChanged("Visible");
            RaisePropertyChanged("Color");
            RaisePropertyChanged("Opacity");
            RaisePropertyChanged("SelectedTextStyleIndex");
            RaisePropertyChanged("SelectedTextAlignmentIndex");
            RaisePropertyChanged("TextSizeRatio");
            RaisePropertyChanged("TextVisible");
            RaisePropertyChanged("TextColor");
            RaisePropertyChanged("TextOpacity");
            RaisePropertyChanged("SelectedFontFamily");
            RaisePropertyChanged("RadiusRatio");
            RaisePropertyChanged("BorderColor");
            RaisePropertyChanged("BorderThickness");
        }
    }
}
