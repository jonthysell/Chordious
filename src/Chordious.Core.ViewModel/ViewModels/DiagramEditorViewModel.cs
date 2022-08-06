// Copyright (c) Jon Thysell <http://jonthysell.com>
// Licensed under the MIT License.

using System;
using System.ComponentModel;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

using Chordious.Core.ViewModel.Resources;

namespace Chordious.Core.ViewModel
{
    public class DiagramEditorViewModel : ObservableObject
    {
        public static AppViewModel AppVM
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

        #region Group Labels

        public static string DimensionsGroupLabel
        {
            get
            {
                return Strings.DiagramEditorDimensionsGroupLabel;
            }
        }

        public static string DiagramLayoutGroupLabel
        {
            get
            {
                return Strings.DiagramEditorDiagramLayoutGroupLabel;
            }
        }

        public static string DiagramBackgroundGroupLabel
        {
            get
            {
                return Strings.DiagramEditorDiagramBackgroundGroupLabel;
            }
        }

        public static string DiagramBorderGroupLabel
        {
            get
            {
                return Strings.DiagramEditorDiagramBorderGroupLabel;
            }
        }

        public static string GridSpacingGroupLabel
        {
            get
            {
                return Strings.DiagramEditorGridSpacingGroupLabel;
            }
        }

        public static string GridMarginGroupLabel
        {
            get
            {
                return Strings.DiagramEditorGridMarginGroupLabel;
            }
        }

        public static string GridBackgroundGroupLabel
        {
            get
            {
                return Strings.DiagramEditorGridBackgroundGroupLabel;
            }
        }

        public static string GridLineGroupLabel
        {
            get
            {
                return Strings.DiagramEditorGridLineGroupLabel;
            }
        }

        public static string GridNutGroupLabel
        {
            get
            {
                return Strings.DiagramEditorGridNutGroupLabel;
            }
        }

        public static string TitleGroupLabel
        {
            get
            {
                return Strings.DiagramEditorTitleGroupLabel;
            }
        }

        public static string TitleTextGroupLabel
        {
            get
            {
                return Strings.DiagramEditorTitleTextGroupLabel;
            }
        }

        public static string TitleLayoutGroupLabel
        {
            get
            {
                return Strings.DiagramEditorTitleLayoutGroupLabel;
            }
        }

        public static string SelectedMarkTypeLabel
        {
            get
            {
                return Strings.DiagramEditorSelectedMarkTypeLabel;
            }
        }

        public static string MarkBackgroundGroupLabel
        {
            get
            {
                return Strings.DiagramEditorMarkBackgroundGroupLabel;
            }
        }

        public static string MarkBorderGroupLabel
        {
            get
            {
                return Strings.DiagramEditorMarkBorderGroupLabel;
            }
        }

        public static string MarkTextGroupLabel
        {
            get
            {
                return Strings.DiagramEditorMarkTextGroupLabel;
            }
        }

        public static string FretLabelTextGroupLabel
        {
            get
            {
                return Strings.DiagramEditorFretLabelTextGroupLabel;
            }
        }

        public static string FretLabelLayoutGroupLabel
        {
            get
            {
                return Strings.DiagramEditorFretLabelLayoutGroupLabel;
            }
        }

        public static string BarreStyleGroupLabel
        {
            get
            {
                return Strings.DiagramEditorBarreStyleGroupLabel;
            }
        }

        public static string BarreLayoutGroupLabel
        {
            get
            {
                return Strings.DiagramEditorBarreLayoutGroupLabel;
            }
        }

        #endregion

        public static string ResetStylesLabel
        {
            get
            {
                return Strings.DiagramEditorResetStylesLabel;
            }
        }

        public static string ResetStylesToolTip
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
                return _resetStyles ??= new RelayCommand(() =>
                {
                    StrongReferenceMessenger.Default.Send(new ConfirmationMessage(Strings.DiagramEditorResetStylesPrompt, (confirmed) =>
                    {
                        try
                        {
                            if (confirmed)
                            {
                                ObservableDiagram.ResetStyles();
                                ResetStyles.NotifyCanExecuteChanged();
                            }
                        }
                        catch (Exception ex)
                        {
                            ExceptionUtils.HandleException(ex);
                        }
                    }, "confirmation.diagrameditor.resetstyles"));
                },
                () =>
                {
                    return Style.LocalCount > 0;
                });
            }
        }
        private RelayCommand _resetStyles;

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

        public bool DiagramChanged
        {
            get
            {
                return _diagramChanged;
            }
            private set
            {
                _diagramChanged = value;
                OnPropertyChanged(nameof(DiagramChanged));
            }
        }
        private bool _diagramChanged = false;

        public ObservableDiagram ObservableDiagram { get; private set; }

        private readonly ObservableDiagram OriginalObservableDiagram;

        public ObservableDiagramStyle Style
        {
            get
            {
                return ObservableDiagram.Style;
            }
        }

        public DiagramEditorViewModel(ObservableDiagram diagram, bool isNew)
        {
            OriginalObservableDiagram = diagram ?? throw new ArgumentNullException(nameof(diagram));

            ObservableDiagram = new ObservableDiagram(diagram.Diagram.Clone())
            {
                IsEditMode = true
            };

            if (isNew)
            {
                _dirty = true;
            }

            ObservableDiagram.PropertyChanged += ObservableDiagram_PropertyChanged;
            ObservableDiagram.Style.PropertyChanged += Style_PropertyChanged;
        }

        void ObservableDiagram_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (!ObservableDiagram.IsCursorProperty(e.PropertyName))
            {
                Dirty = true;
                if (e.PropertyName == nameof(Style))
                {
                    OnPropertyChanged(nameof(Style));
                }
            }
        }

        void Style_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "LocalCount")
            {
                Dirty = true;
                ResetStyles.NotifyCanExecuteChanged();
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
