// 
// DiagramFretLabelEditorViewModel.cs
//  
// Author:
//       Jon Thysell <thysell@gmail.com>
// 
// Copyright (c) 2015, 2016, 2017, 2019 Jon Thysell <http://jonthysell.com>
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
using System.ComponentModel;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

using com.jonthysell.Chordious.Core.ViewModel.Resources;

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
                return Strings.DiagramFretLabelEditorTitle + (Dirty ? "*" : "");
            }
        }

        #region Properties

        public string PropertiesGroupLabel
        {
            get
            {
                return Strings.DiagramFretLabelEditorPropertiesGroupLabel;
            }
        }

        public string TextLabel
        {
            get
            {
                return Strings.DiagramFretLabelEditorTextLabel;
            }
        }

        public string TextToolTip
        {
            get
            {
                return Strings.DiagramFretLabelEditorTextToolTip;
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

        #endregion

        public RelayCommand Apply
        {
            get
            {
                return _apply ?? (_apply = new RelayCommand(() =>
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
                }));
            }
        }
        private RelayCommand _apply;

        public RelayCommand Accept
        {
            get
            {
                return _accept ?? (_accept = new RelayCommand(() =>
                {
                    try
                    {
                        ApplyChangesOnClose = true;
                        RequestClose?.Invoke();
                    }
                    catch (Exception ex)
                    {
                        ExceptionUtils.HandleException(ex);
                    }
                }));
            }
        }
        private RelayCommand _accept;

        public RelayCommand Cancel
        {
            get
            {
                return _cancel ?? (_cancel = new RelayCommand(() =>
                {
                    try
                    {
                        ApplyChangesOnClose = false;
                        RequestClose?.Invoke();
                    }
                    catch (Exception ex)
                    {
                        ExceptionUtils.HandleException(ex);
                    }
                }));
            }
        }
        private RelayCommand _cancel;

        public Action RequestClose;

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
                Apply.RaiseCanExecuteChanged();
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

        internal DiagramFretLabel DiagramFretLabel { get; private set; }

        public DiagramFretLabelEditorViewModel(DiagramFretLabel diagramFretLabel, bool isNew)
        {
            DiagramFretLabel = diagramFretLabel ?? throw new ArgumentNullException(nameof(diagramFretLabel));

            // Save properties
            _text = diagramFretLabel.Text;

            // Save original
            _originalStyle = new ObservableDiagramStyle(diagramFretLabel.Style);

            // Create editable clone
            DiagramStyle clone = _originalStyle.Style.Clone();
            if (_originalStyle.Style.ReadOnly)
            {
                clone.MarkAsReadOnly();
            }

            Style = new ObservableDiagramStyle(clone);
            Style.PropertyChanged += ObservableDiagramStyle_PropertyChanged;

            if (isNew)
            {
                _dirty = true;
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
                DiagramFretLabel.Text = Text;
                _originalStyle.Style.Clear();
                _originalStyle.Style.CopyFrom(Style.Style);
            }

            Dirty = false;
            DiagramStyleChanged = true;
        }
    }
}
