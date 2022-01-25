// Copyright (c) Jon Thysell <http://jonthysell.com>
// Licensed under the MIT License.

using System;
using System.Collections.ObjectModel;
using System.ComponentModel;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

using Chordious.Core.ViewModel.Resources;

namespace Chordious.Core.ViewModel
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
                    RaisePropertyChanged(nameof(Text));
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
                    RaisePropertyChanged(nameof(SelectedMarkTypeIndex));
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

        public ObservableDiagramStyle Style { get; private set; }
        private readonly ObservableDiagramStyle _originalStyle;

        internal DiagramMark DiagramMark { get; private set; }

        public DiagramMarkEditorViewModel(DiagramMark diagramMark, bool isNew)
        {
            DiagramMark = diagramMark ?? throw new ArgumentNullException(nameof(diagramMark));

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
