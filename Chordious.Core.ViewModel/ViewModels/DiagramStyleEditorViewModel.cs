﻿// 
// DiagramStyleEditorViewModel.cs
//  
// Author:
//       Jon Thysell <thysell@gmail.com>
// 
// Copyright (c) 2016, 2017, 2019, 2021 Jon Thysell <http://jonthysell.com>
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
using System.ComponentModel;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

using Chordious.Core.ViewModel.Resources;

namespace Chordious.Core.ViewModel
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
                return Strings.DiagramStyleEditorTitle + (Dirty ? "*" : "");
            }
        }

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
                RaisePropertyChanged(nameof(ApplyChangesOnClose));
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
                RaisePropertyChanged(nameof(Dirty));
                RaisePropertyChanged(nameof(Title));
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
                RaisePropertyChanged(nameof(DiagramStyleChanged));
            }
        }
        private bool _diagramStyleChanged = false;

        public string SelectedStyleLabel
        {
            get
            {
                return Strings.DiagramStyleEditorSelectedStyleLabel;
            }
        }

        public string SelectedStyleToolTip
        {
            get
            {
                return Strings.DiagramStyleEditorSelectedStyleToolTip;
            }
        }

        public ObservableDiagramStyle Style
        {
            get
            {
                return Styles[SelectedStyleIndex];
            }
        }

        public int SelectedStyleIndex
        {
            get
            {
                return _selectedStyleIndex;
            }
            set
            {
                if (value < 0 || value > Styles.Count)
                {
                    throw new ArgumentOutOfRangeException();
                }

                _selectedStyleIndex = value;
                RaisePropertyChanged(nameof(SelectedStyleIndex));
                RaisePropertyChanged(nameof(Style));
            }
        }
        private int _selectedStyleIndex;

        public ObservableCollection<ObservableDiagramStyle> Styles { get; private set; }

        private readonly ObservableCollection<ObservableDiagramStyle> _originalStyles;

        public DiagramStyleEditorViewModel(ObservableDiagramStyle diagramStyle)
        {
            if (null == diagramStyle)
            {
                throw new ArgumentNullException(nameof(diagramStyle));
            }

            // Add original
            _originalStyles = new ObservableCollection<ObservableDiagramStyle>
            {
                diagramStyle
            };

            // Recursively parents up the tree
            DiagramStyle style = diagramStyle.Style.Parent;

            while (null != style)
            {
                ObservableDiagramStyle parentStyle = new ObservableDiagramStyle(style);
                _originalStyles.Insert(0, parentStyle);
                style = style.Parent;
            }

            // Add editable clones
            Styles = new ObservableCollection<ObservableDiagramStyle>();

            foreach (ObservableDiagramStyle originalStyle in _originalStyles)
            {
                DiagramStyle clone = originalStyle.Style.Clone();
                if (originalStyle.Style.ReadOnly)
                {
                    clone.MarkAsReadOnly();
                }

                ObservableDiagramStyle editableStyle = new ObservableDiagramStyle(clone);
                editableStyle.PropertyChanged += ObservableDiagramStyle_PropertyChanged;
                Styles.Add(editableStyle);
            }

            _selectedStyleIndex = Styles.Count - 1;
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
            for (int i = 0; i < _originalStyles.Count; i++)
            {
                if (_originalStyles[i].IsEditable)
                {
                    _originalStyles[i].Style.Clear();
                    _originalStyles[i].Style.CopyFrom(Styles[i].Style);
                }
            }

            Dirty = false;
            DiagramStyleChanged = true;
        }
    }
}
