// 
// DiagramFretLabelEditorViewModel.cs
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
    public class DiagramFretLabelEditorViewModel : ViewModelBase
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
                return "Diagram Fret Label Editor" + (Dirty ? "*" : "");
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

        public string TextColor
        {
            get
            {
                return StyleBuffer.FretLabelTextColorGet();
            }
            set
            {
                try
                {
                    if (null != value)
                    {
                        StyleBuffer.FretLabelTextColorSet(value);
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
                return StyleBuffer.FretLabelTextOpacityGet();
            }
            set
            {
                try
                {
                    StyleBuffer.FretLabelTextOpacitySet(value);
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
                return StyleBuffer.FretLabelFontFamilyGet();
            }
            set
            {
                try
                {
                    if (null != value)
                    {
                        StyleBuffer.FretLabelFontFamilySet(value);
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

        public int SelectedTextStyleIndex
        {
            get
            {
                return (int)StyleBuffer.FretLabelTextStyleGet();
            }
            set
            {
                try
                {
                    StyleBuffer.FretLabelTextStyleSet((DiagramTextStyle)value);
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
                return (int)StyleBuffer.FretLabelTextAlignmentGet();
            }
            set
            {
                try
                {
                    StyleBuffer.FretLabelTextAlignmentSet((DiagramHorizontalAlignment)value);
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
                return StyleBuffer.FretLabelTextSizeRatioGet();
            }
            set
            {
                try
                {
                    StyleBuffer.FretLabelTextSizeRatioSet(value);
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
                return StyleBuffer.FretLabelTextVisibleGet();
            }
            set
            {
                try
                {
                    StyleBuffer.FretLabelTextVisibleSet(value);
                    Dirty = true;
                    RaisePropertyChanged("TextVisible");
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }
        }

        public double GridPadding
        {
            get
            {
                return StyleBuffer.FretLabelGridPaddingGet();
            }
            set
            {
                try
                {
                    StyleBuffer.FretLabelGridPaddingSet(value);
                    Dirty = true;
                    RaisePropertyChanged("GridPadding");
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }
        }

        public double TextWidthRatio
        {
            get
            {
                return StyleBuffer.FretLabelTextWidthRatioGet();
            }
            set
            {
                try
                {
                    StyleBuffer.FretLabelTextWidthRatioSet(value);
                    Dirty = true;
                    RaisePropertyChanged("TextWidthRatio");
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

        public bool DiagramFretLabelChanged
        {
            get
            {
                return _diagramFretLabelChanged;
            }
            private set
            {
                _diagramFretLabelChanged = value;
                RaisePropertyChanged("DiagramFretLabelChanged");
            }
        }
        private bool _diagramFretLabelChanged = false;

        internal DiagramFretLabel DiagramFretLabel { get; private set; }
        internal DiagramStyle StyleBuffer { get; private set; }

        public DiagramFretLabelEditorViewModel(DiagramFretLabel diagrmFretLabel, bool isNew)
        {
            if (null == diagrmFretLabel)
            {
                throw new ArgumentNullException("diagrmFretLabel");
            }

            DiagramFretLabel = diagrmFretLabel;

            // Buffer values
            _text = DiagramFretLabel.Text;

            StyleBuffer = new DiagramStyle(DiagramFretLabel.Style, "DiagramFretLabelEditor");

            // Pre-seed used fonts
            ObservableEnums.SortedInsert(FontFamilies, TextFontFamily);

            // Pre-seed used colors
            ObservableEnums.SortedInsert(Colors, TextColor);

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
            return DiagramFretLabelChanged;
        }

        private void ApplyChanges()
        {
            DiagramFretLabel.Text = _text;

            StyleBuffer.SetParent();
            StyleBuffer.Clear();

            Dirty = false;
            DiagramFretLabelChanged = true;
        }

        private void RefreshProperties()
        {
            RaisePropertyChanged("TextColor");
            RaisePropertyChanged("TextOpacity");
            RaisePropertyChanged("SelectedFontFamily");
            RaisePropertyChanged("SelectedTextStyleIndex");
            RaisePropertyChanged("SelectedTextAlignmentIndex");
            RaisePropertyChanged("TextSizeRatio");
            RaisePropertyChanged("TextVisible");
            RaisePropertyChanged("GridPadding");
            RaisePropertyChanged("TextWidthRatio");
        }
    }
}
