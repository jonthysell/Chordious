// 
// DiagramBarreEditorViewModel.cs
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
    public class DiagramBarreEditorViewModel : ViewModelBase
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
                return "Diagram Barre Editor" + (Dirty ? "*" : "");
            }
        }

        public bool Visible
        {
            get
            {
                return StyleBuffer.BarreVisible;
            }
            set
            {
                try
                {
                    StyleBuffer.BarreVisible = value;
                    Dirty = true;
                    RaisePropertyChanged("Visible");
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }
        }

        public int SelectedVerticalAlignmentIndex
        {
            get
            {
                return (int)StyleBuffer.BarreVerticalAlignment;
            }
            set
            {
                try
                {
                    StyleBuffer.BarreVerticalAlignment = (DiagramVerticalAlignment)value;
                    Dirty = true;
                    RaisePropertyChanged("SelectedVerticalAlignmentIndex");
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }
        }

        public ObservableCollection<string> VerticalAlignments
        {
            get
            {
                return ObservableEnums.GetVerticalAlignments();
            }
        }

        public int SelectedBarreStackIndex
        {
            get
            {
                return (int)StyleBuffer.BarreStack;
            }
            set
            {
                try
                {
                    StyleBuffer.BarreStack = (DiagramBarreStack)value;
                    Dirty = true;
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

        public double ArcRatio
        {
            get
            {
                return StyleBuffer.BarreArcRatio;
            }
            set
            {
                try
                {
                    StyleBuffer.BarreArcRatio = value;
                    Dirty = true;
                    RaisePropertyChanged("ArcRatio");
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
                return StyleBuffer.BarreOpacity;
            }
            set
            {
                try
                {
                    StyleBuffer.BarreOpacity = value;
                    Dirty = true;
                    RaisePropertyChanged("Opacity");
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }
        }

        public string LineColor
        {
            get
            {
                return StyleBuffer.BarreLineColor;
            }
            set
            {
                try
                {
                    if (null != value)
                    {
                        StyleBuffer.BarreLineColor = value;
                        ObservableEnums.SortedInsert(Colors, LineColor);
                        Dirty = true;
                        RaisePropertyChanged("LineColor");
                    }
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }
        }

        public double LineThickness
        {
            get
            {
                return StyleBuffer.BarreLineThickness;
            }
            set
            {
                try
                {
                    StyleBuffer.BarreLineThickness = value;
                    Dirty = true;
                    RaisePropertyChanged("LineThickness");
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }
        }
        
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

        internal DiagramBarre DiagramBarre { get; private set; }
        internal DiagramStyle StyleBuffer { get; private set; }

        public DiagramBarreEditorViewModel(DiagramBarre diagramBarre, bool isNew)
        {
            if (null == diagramBarre)
            {
                throw new ArgumentNullException("diagramBarre");
            }

            DiagramBarre = diagramBarre;

            StyleBuffer = new DiagramStyle(DiagramBarre.Style, "DiagramBarreEditor");

            // Pre-seed used colors
            ObservableEnums.SortedInsert(Colors, LineColor);

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
            StyleBuffer.SetParent();
            StyleBuffer.Clear();

            Dirty = false;
            DiagramFretLabelChanged = true;
        }

        private void RefreshProperties()
        {
            RaisePropertyChanged("Visible");
            RaisePropertyChanged("SelectedVerticalAlignmentIndex");
            RaisePropertyChanged("SelectedBarreStackIndex");
            RaisePropertyChanged("ArcRatio");
            RaisePropertyChanged("Opacity");
            RaisePropertyChanged("LineColor");
            RaisePropertyChanged("LineThickness");
        }
    }
}
