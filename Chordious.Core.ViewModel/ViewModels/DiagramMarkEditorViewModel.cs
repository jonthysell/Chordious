// 
// DiagramMarkEditorViewModel.cs
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
using System.Collections.Specialized;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;

using com.jonthysell.Chordious.Core;

using com.jonthysell.Chordious.Core.ViewModel.Resources;

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
                return Strings.DiagramMarkEditorTitle + (Dirty ? "*" : "");
            }
        }

        #region Properties

        public string PropertiesGroupLabel
        {
            get
            {
                return Strings.DiagramMarkEditorPropertiesGroupLabel;
            }
        }

        public string TextLabel
        {
            get
            {
                return Strings.DiagramMarkEditorTextLabel;
            }
        }

        public string TextToolTip
        {
            get
            {
                return Strings.DiagramMarkEditorTextToolTip;
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

        public string SelectedMarkTypeLabel
        {
            get
            {
                return Strings.DiagramMarkEditorMarkTypeLabel;
            }
        }

        public string SelectedMarkTypeToolTip
        {
            get
            {
                return Strings.DiagramMarkEditorMarkTypeToolTip;
            }
        }

        public int SelectedMarkTypeIndex
        {
            get
            {
                return (int)Style.MarkStyle.MarkType;
            }
            set
            {
                try
                {
                    Style.MarkStyle.MarkType = (DiagramMarkType)(value);
                    Dirty = true;
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
                return Style.MarkTypes;
            }
        }

        #endregion

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

        public bool DiagramStyleChanged
        {
            get
            {
                return _diagramStyleChanged;
            }
            private set
            {
                _diagramStyleChanged = value;
                RaisePropertyChanged("DiagramStyleChanged");
            }
        }
        private bool _diagramStyleChanged = false;

        public ObservableDiagramStyle Style { get; private set; }
        private ObservableDiagramStyle _originalStyle;

        internal DiagramMark DiagramMark { get; private set; }

        public DiagramMarkEditorViewModel(DiagramMark diagramMark, bool isNew)
        {
            if (null == diagramMark)
            {
                throw new ArgumentNullException("diagramMark");
            }

            DiagramMark = diagramMark;

            // Save properties
            _text = diagramMark.Text;

            // Save original
            _originalStyle = new ObservableDiagramStyle(diagramMark.Style, diagramMark.MarkStyle);

            // Create editable clone
            DiagramStyle clone = _originalStyle.Style.Clone();
            if (_originalStyle.Style.ReadOnly)
            {
                clone.MarkAsReadOnly();
            }

            Style = new ObservableDiagramStyle(clone);
            Style.MarkStyle.MarkType = DiagramMark.Type;
            Style.PropertyChanged += ObservableDiagramStyle_PropertyChanged;

            if (isNew)
            {
                Dirty = true;
            }
        }

        private void ObservableDiagramStyle_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Dirty = true;
        }

        public bool ProcessClose()
        {
            if (ApplyChangesOnClose)
            {
                ApplyChanges();
            }
            return DiagramStyleChanged;
        }

        private void ApplyChanges()
        {
            if (_originalStyle.IsEditable)
            {
                DiagramMark.Text = Text;
                DiagramMark.Type = Style.MarkStyle.MarkType;
                _originalStyle.Style.Clear();
                _originalStyle.Style.CopyFrom(Style.Style);
            }

            Dirty = false;
            DiagramStyleChanged = true;
        }
    }
}
