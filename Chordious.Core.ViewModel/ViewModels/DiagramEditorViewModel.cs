// 
// DiagramEditorViewModel.cs
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

        #region Group Labels

        public string DimensionsGroupLabel
        {
            get
            {
                return Strings.DiagramEditorDimensionsGroupLabel;
            }
        }

        public string DiagramLayoutGroupLabel
        {
            get
            {
                return Strings.DiagramEditorDiagramLayoutGroupLabel;
            }
        }

        public string DiagramBackgroundGroupLabel
        {
            get
            {
                return Strings.DiagramEditorDiagramBackgroundGroupLabel;
            }
        }

        public string DiagramBorderGroupLabel
        {
            get
            {
                return Strings.DiagramEditorDiagramBorderGroupLabel;
            }
        }

        public string GridSpacingGroupLabel
        {
            get
            {
                return Strings.DiagramEditorGridSpacingGroupLabel;
            }
        }

        public string GridMarginGroupLabel
        {
            get
            {
                return Strings.DiagramEditorGridMarginGroupLabel;
            }
        }

        public string GridBackgroundGroupLabel
        {
            get
            {
                return Strings.DiagramEditorGridBackgroundGroupLabel;
            }
        }

        public string GridLineGroupLabel
        {
            get
            {
                return Strings.DiagramEditorGridLineGroupLabel;
            }
        }

        public string GridNutGroupLabel
        {
            get
            {
                return Strings.DiagramEditorGridNutGroupLabel;
            }
        }

        public string TitleGroupLabel
        {
            get
            {
                return Strings.DiagramEditorTitleGroupLabel;
            }
        }

        public string TitleTextGroupLabel
        {
            get
            {
                return Strings.DiagramEditorTitleTextGroupLabel;
            }
        }

        public string TitleLayoutGroupLabel
        {
            get
            {
                return Strings.DiagramEditorTitleLayoutGroupLabel;
            }
        }

        public string SelectedMarkTypeLabel
        {
            get
            {
                return Strings.DiagramEditorSelectedMarkTypeLabel;
            }
        }

        public string MarkBackgroundGroupLabel
        {
            get
            {
                return Strings.DiagramEditorMarkBackgroundGroupLabel;
            }
        }

        public string MarkBorderGroupLabel
        {
            get
            {
                return Strings.DiagramEditorMarkBorderGroupLabel;
            }
        }

        public string MarkTextGroupLabel
        {
            get
            {
                return Strings.DiagramEditorMarkTextGroupLabel;
            }
        }

        public string FretLabelTextGroupLabel
        {
            get
            {
                return Strings.DiagramEditorFretLabelTextGroupLabel;
            }
        }

        public string FretLabelLayoutGroupLabel
        {
            get
            {
                return Strings.DiagramEditorFretLabelLayoutGroupLabel;
            }
        }

        public string BarreStyleGroupLabel
        {
            get
            {
                return Strings.DiagramEditorBarreStyleGroupLabel;
            }
        }

        public string BarreLayoutGroupLabel
        {
            get
            {
                return Strings.DiagramEditorBarreLayoutGroupLabel;
            }
        }

        #endregion

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
                return _resetStyles ?? (_resetStyles = new RelayCommand(() =>
                {
                    Messenger.Default.Send(new ConfirmationMessage(Strings.DiagramEditorResetStylesPrompt, (confirmed) =>
                    {
                        try
                        {
                            if (confirmed)
                            {
                                ObservableDiagram.ResetStyles();
                                ResetStyles.RaiseCanExecuteChanged();
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
                }));
            }
        }
        private RelayCommand _resetStyles;

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

        public ObservableDiagram ObservableDiagram { get; private set; }

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
                ResetStyles.RaiseCanExecuteChanged();
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
