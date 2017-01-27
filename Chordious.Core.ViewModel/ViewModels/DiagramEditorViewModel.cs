// 
// DiagramEditorViewModel.cs
//  
// Author:
//       Jon Thysell <thysell@gmail.com>
// 
// Copyright (c) 2015, 2016, 2017 Jon Thysell <http://jonthysell.com>
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
using GalaSoft.MvvmLight.Messaging;

using com.jonthysell.Chordious.Core.ViewModel.Resources;

namespace com.jonthysell.Chordious.Core.ViewModel
{
    public class DiagramEditorViewModel : ViewModelBase
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
                return Strings.DiagramEditorTitle + (Dirty ? "*" : "");
            }
        }

        public string ResetStylesLabel
        {
            get
            {
                return Strings.DiagramEditorResetStylesLabel;
            }
        }

        public string ResetStylesToolTip
        {
            get
            {
                return Strings.DiagramEditorResetStylesToolTip;
            }
        }

        public RelayCommand ResetStyles
        {
            get
            {
                return new RelayCommand(() =>
                {
                    Messenger.Default.Send<ConfirmationMessage>(new ConfirmationMessage(Strings.DiagramEditorResetStylesPrompt, (confirmed) =>
                    {
                        if (confirmed)
                        {
                            ObservableDiagram.ResetStyles();
                            RaisePropertyChanged("ResetStyles");
                        }
                    }, "confirmation.diagrameditor.resetstyles"));
                },
                () =>
                {
                    return Style.LocalCount > 0;
                });
            }
        }

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

        public bool DiagramChanged
        {
            get
            {
                return _diagramChanged;
            }
            private set
            {
                _diagramChanged = value;
                RaisePropertyChanged("DiagramChanged");
            }
        }
        private bool _diagramChanged = false;

        public ObservableDiagram ObservableDiagram
        {
            get
            {
                return _observableDiagram;
            }
            private set
            {
                if (null == value)
                {
                    throw new ArgumentNullException();
                }
                _observableDiagram = value;
                RaisePropertyChanged("ObservableDiagram");
            }
        }
        private ObservableDiagram _observableDiagram;

        private ObservableDiagram OriginalObservableDiagram;

        public ObservableDiagramStyle Style
        {
            get
            {
                return ObservableDiagram.Style;
            }
        }

        public DiagramEditorViewModel(ObservableDiagram diagram, bool isNew)
        {
            if (null == diagram)
            {
                throw new ArgumentNullException("diagram");
            }

            OriginalObservableDiagram = diagram;

            ObservableDiagram = new ObservableDiagram(diagram.Diagram.Clone());
            ObservableDiagram.IsEditMode = true;

            if (isNew)
            {
                Dirty = true;
            }

            ObservableDiagram.PropertyChanged += ObservableDiagram_PropertyChanged;
            ObservableDiagram.Style.PropertyChanged += Style_PropertyChanged;
        }

        void ObservableDiagram_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (!ObservableDiagram.IsCursorProperty(e.PropertyName))
            {
                Dirty = true;
                if (e.PropertyName == "Style")
                {
                    RaisePropertyChanged("Style");
                }
            }
        }

        void Style_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "LocalCount")
            {
                Dirty = true;
                RaisePropertyChanged("ResetStyles");
            }
        }

        public bool ProcessClose()
        {
            if (ApplyChangesOnClose)
            {
                ApplyChanges();
            }
            return DiagramChanged;
        }

        private void ApplyChanges()
        {
            OriginalObservableDiagram.Diagram = ObservableDiagram.Diagram.Clone();
            Dirty = false;
            DiagramChanged = true;
        }
    }
}
