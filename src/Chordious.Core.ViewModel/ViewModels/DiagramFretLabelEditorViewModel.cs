// Copyright (c) Jon Thysell <http://jonthysell.com>
// Licensed under the MIT License.

using System;
using System.ComponentModel;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using Chordious.Core.ViewModel.Resources;

namespace Chordious.Core.ViewModel
{
    public class DiagramFretLabelEditorViewModel : ObservableObject
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
                    OnPropertyChanged(nameof(Text));
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
                return _apply ??= new RelayCommand(() =>
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
        private RelayCommand _apply;

        public RelayCommand Accept
        {
            get
            {
                return _accept ??= new RelayCommand(() =>
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
                });
            }
        }
        private RelayCommand _accept;

        public RelayCommand Cancel
        {
            get
            {
                return _cancel ??= new RelayCommand(() =>
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
                });
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
                OnPropertyChanged(nameof(ApplyChangesOnClose));
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
                OnPropertyChanged(nameof(Dirty));
                OnPropertyChanged(nameof(Title));
                Apply.NotifyCanExecuteChanged();
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
                OnPropertyChanged(nameof(DiagramStyleChanged));
            }
        }
        private bool _diagramStyleChanged = false;

        public ObservableDiagramStyle Style { get; private set; }
        private readonly ObservableDiagramStyle _originalStyle;

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
