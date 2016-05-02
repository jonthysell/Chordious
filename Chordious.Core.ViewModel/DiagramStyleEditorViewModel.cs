// 
// DiagramStyleEditorViewModel.cs
//  
// Author:
//       Jon Thysell <thysell@gmail.com>
// 
// Copyright (c) 2016 Jon Thysell <http://jonthysell.com>
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
    public class DiagramStyleEditorViewModel : ViewModelBase
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
                return String.Format(Strings.DiagramStyleEditorTitleFormat, ObservableDiagramStyle.Level) + (Dirty ? "*" : "");
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

        public ObservableDiagramStyle ObservableDiagramStyle
        {
            get
            {
                return _observableDiagramStyle;
            }
            private set
            {
                if (null == value)
                {
                    throw new ArgumentNullException();
                }
                _observableDiagramStyle = value;
                RaisePropertyChanged("ObservableDiagramStyle");
            }
        }
        private ObservableDiagramStyle _observableDiagramStyle;

        private ObservableDiagramStyle OriginalObservableDiagramStyle;

        public DiagramStyleEditorViewModel(ObservableDiagramStyle diagramStyle)
        {
            if (null == diagramStyle)
            {
                throw new ArgumentNullException("diagramStyle");
            }

            OriginalObservableDiagramStyle = diagramStyle;

            ObservableDiagramStyle = new ObservableDiagramStyle(diagramStyle.Style.Clone());

            ObservableDiagramStyle.PropertyChanged += ObservableDiagramStyle_PropertyChanged;
        }

        void ObservableDiagramStyle_PropertyChanged(object sender, PropertyChangedEventArgs e)
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
            OriginalObservableDiagramStyle.Style.Clear();
            OriginalObservableDiagramStyle.Style.CopyFrom(ObservableDiagramStyle.Style);
            Dirty = false;
            DiagramStyleChanged = true;
        }
    }
}
