// Copyright (c) Jon Thysell <http://jonthysell.com>
// Licensed under the MIT License.

using System;
using System.ComponentModel;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;

using Chordious.Core.ViewModel.Resources;

namespace Chordious.Core.ViewModel
{
    public class ObservableDiagram : ObservableObject
    {
        public static AppViewModel AppVM
        {
            get
            {
                return AppViewModel.Instance;
            }
        }

        public string SvgText
        {
            get
            {
                return Diagram.ToImageMarkup(ImageMarkupType.SVG);
            }
        }

        public object ImageObject
        {
            get
            {
                return _imageObject;
            }
            private set
            {
                _imageObject = value;
                RaisePropertyChanged(nameof(ImageObject));
            }
        }
        private object _imageObject;

        #region Dimensions

        public string TotalWidthLabel
        {
            get
            {
                return Strings.DiagramTotalWidthLabel;
            }
        }

        public string TotalWidthToolTip
        {
            get
            {
                return Strings.DiagramTotalWidthToolTip;
            }
        }

        public int TotalWidth
        {
            get
            {
                return (int)(Diagram.TotalWidth() + 0.5);
            }
        }

        public string TotalHeightLabel
        {
            get
            {
                return Strings.DiagramTotalHeightLabel;
            }
        }

        public string TotalHeightToolTip
        {
            get
            {
                return Strings.DiagramTotalHeightToolTip;
            }
        }

        public int TotalHeight
        {
            get
            {
                return (int)(Diagram.TotalHeight() + 0.5);
            }
        }

        public string NumFretsLabel
        {
            get
            {
                return Strings.DiagramNumFretsLabel;
            }
        }

        public string NumFretsToolTip
        {
            get
            {
                return Strings.DiagramNumFretsToolTip;
            }
        }

        public int NumFrets
        {
            get
            {
                return Diagram.NumFrets;
            }
            set
            {
                try
                {
                    Diagram.NumFrets = value;
                    RaisePropertyChanged(nameof(NumFrets));
                    Refresh();
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }
        }

        public string NumStringsLabel
        {
            get
            {
                return Strings.DiagramNumStringsLabel;
            }
        }

        public string NumStringsToolTip
        {
            get
            {
                return Strings.DiagramNumStringsToolTip;
            }
        }

        public int NumStrings
        {
            get
            {
                return Diagram.NumStrings;
            }
            set
            {
                try
                {
                    Diagram.NumStrings = value;
                    RaisePropertyChanged(nameof(NumStrings));
                    Refresh();
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }
        }

        #endregion

        #region Title

        public string TitleLabel
        {
            get
            {
                return Strings.DiagramTitleTextLabel;
            }
        }

        public string TitleToolTip
        {
            get
            {
                return Strings.DiagramTitleTextToolTip;
            }
        }

        public string Title
        {
            get
            {
                return Diagram.Title;
            }
            set
            {
                try
                {
                    Diagram.Title = value;
                    RaisePropertyChanged(nameof(Title));
                    Refresh();
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }
        }

        #endregion

        #region Elements

        public RelayCommand EditElement
        {
            get
            {
                return _editElement ?? (_editElement = new RelayCommand(() =>
                {
                    try
                    {
                        MarkPosition mp = MarkPosition;
                        BarrePosition bp = BarrePosition;
                        FretLabelPosition flp = FretLabelPosition;

                        bool hasMark = null != mp && Diagram.HasElementAt(mp);
                        bool hasBarre = null != bp && Diagram.HasElementAt(bp);
                        bool hasFretLabel = null != flp && Diagram.HasElementAt(flp);

                        if (hasMark)
                        {
                            DiagramMark dm = (DiagramMark)Diagram.ElementAt(mp);
                            Messenger.Default.Send(new ShowDiagramMarkEditorMessage(dm, false, (changed) =>
                            {
                                try
                                {
                                    if (changed)
                                    {
                                        Refresh();
                                    }
                                }
                                catch (Exception ex)
                                {
                                    ExceptionUtils.HandleException(ex);
                                }
                            }));
                        }
                        else if (hasBarre)
                        {
                            DiagramBarre db = (DiagramBarre)Diagram.ElementAt(bp);
                            Messenger.Default.Send(new ShowDiagramBarreEditorMessage(db, false, (changed) =>
                            {
                                try
                                {
                                    if (changed)
                                    {
                                        Refresh();
                                    }
                                }
                                catch (Exception ex)
                                {
                                    ExceptionUtils.HandleException(ex);
                                }
                            }));
                        }
                        else if (hasFretLabel)
                        {
                            DiagramFretLabel dfl = (DiagramFretLabel)Diagram.ElementAt(flp);
                            Messenger.Default.Send(new ShowDiagramFretLabelEditorMessage(dfl, false, (changed) =>
                            {
                                try
                                {
                                    if (changed)
                                    {
                                        Refresh();
                                    }
                                }
                                catch (Exception ex)
                                {
                                    ExceptionUtils.HandleException(ex);
                                }
                            }));
                        }
                    }
                    catch (Exception ex)
                    {
                        ExceptionUtils.HandleException(ex);
                    }

                }, () =>
                {
                    return (CanEditMark || CanEditFretLabel || CanEditBarre);
                }));
            }
        }
        private RelayCommand _editElement;

        #endregion

        #region Marks

        public string AddMarkLabel
        {
            get
            {
                return Strings.ObservableDiagramAddMarkLabel;
            }
        }

        public string AddMarkToolTip
        {
            get
            {
                return Strings.ObservableDiagramAddMarkToolTip;
            }
        }

        public RelayCommand AddMark
        {
            get
            {
                return _addMark ?? (_addMark = new RelayCommand(() =>
                {
                    try
                    {
                        MarkPosition mp = MarkPosition;
                        DiagramMark dm = Diagram.NewMark(mp);
                        Messenger.Default.Send(new ShowDiagramMarkEditorMessage(dm, true, (changed) =>
                        {
                            try
                            { 
                                if (changed)
                                {
                                    Refresh();
                                }
                                else
                                {
                                    Diagram.RemoveMark(mp);
                                }
                            }
                            catch (Exception ex)
                            {
                                ExceptionUtils.HandleException(ex);
                            }
                        }));
                    }
                    catch (Exception ex)
                    {
                        ExceptionUtils.HandleException(ex);
                    }
                }, () =>
                {
                    return CanAddMark;
                }));
            }
        }
        private RelayCommand _addMark;

        public bool CanAddMark
        {
            get
            {
                MarkPosition mp = MarkPosition;
                return (null != mp && Diagram.CanAddNewMarkAt(mp));
            }
        }

        public string EditMarkLabel
        {
            get
            {
                return Strings.ObservableDiagramEditMarkLabel;
            }
        }

        public string EditMarkToolTip
        {
            get
            {
                return Strings.ObservableDiagramEditMarkToolTip;
            }
        }

        public RelayCommand EditMark
        {
            get
            {
                return _editMark ?? (_editMark = new RelayCommand(() =>
                {
                    try
                    {
                        DiagramMark dm = (DiagramMark)Diagram.ElementAt(MarkPosition);
                        Messenger.Default.Send(new ShowDiagramMarkEditorMessage(dm, false, (changed) =>
                        {
                            try
                            {
                                if (changed)
                                {
                                    Refresh();
                                }
                            }
                            catch (Exception ex)
                            {
                                ExceptionUtils.HandleException(ex);
                            }
                        }));
                    }
                    catch (Exception ex)
                    {
                        ExceptionUtils.HandleException(ex);
                    }
                }, () =>
                {
                    return CanEditMark;
                }));
            }
        }
        private RelayCommand _editMark;

        public bool CanEditMark
        {
            get
            {
                MarkPosition mp = MarkPosition;
                return (null != mp && Diagram.ValidPosition(mp) && Diagram.HasElementAt(mp));
            }
        }

        public string RemoveMarkLabel
        {
            get
            {
                return Strings.ObservableDiagramRemoveMarkLabel;
            }
        }

        public string RemoveMarkToolTip
        {
            get
            {
                return Strings.ObservableDiagramRemoveMarkToolTip;
            }
        }

        public RelayCommand RemoveMark
        {
            get
            {
                return _removeMark ?? (_removeMark = new RelayCommand(() =>
                {
                    try
                    {
                        MarkPosition mp = MarkPosition;
                        Diagram.RemoveMark(mp);
                        Refresh();
                    }
                    catch (Exception ex)
                    {
                        ExceptionUtils.HandleException(ex);
                    }
                }, () =>
                {
                    return CanRemoveMark;
                }));
            }
        }
        private RelayCommand _removeMark;

        public bool CanRemoveMark
        {
            get
            {
                MarkPosition mp = MarkPosition;
                return (null != mp && Diagram.CanRemoveMarkAt(mp));
            }
        }

        internal MarkPosition MarkPosition
        {
            get
            {
                return (MarkPosition)Diagram.GetPosition<MarkPosition>(CursorX, CursorY);
            }
        }

        #endregion

        #region Fret Labels

        public string AddFretLabelLabel
        {
            get
            {
                FretLabelPosition flp = FretLabelPosition;
                return null != flp && flp.Side == FretLabelSide.Left ? Strings.ObservableDiagramAddLeftFretLabelLabel : Strings.ObservableDiagramAddRightFretLabelLabel;
            }
        }

        public string AddFretLabelToolTip
        {
            get
            {
                FretLabelPosition flp = FretLabelPosition;
                return null != flp && flp.Side == FretLabelSide.Left ? Strings.ObservableDiagramAddLeftFretLabelToolTip : Strings.ObservableDiagramAddRightFretLabelToolTip;
            }
        }

        public RelayCommand AddFretLabel
        {
            get
            {
                return _addFretLabel ?? (_addFretLabel = new RelayCommand(() =>
                {
                    try
                    {
                        FretLabelPosition flp = FretLabelPosition;
                        DiagramFretLabel dfl = Diagram.NewFretLabel(flp, "");
                        Messenger.Default.Send(new ShowDiagramFretLabelEditorMessage(dfl, true, (changed) =>
                        {
                            try
                            {
                                if (changed)
                                {
                                    Refresh();
                                }
                                else
                                {
                                    Diagram.RemoveFretLabel(flp);
                                }
                            }
                            catch (Exception ex)
                            {
                                ExceptionUtils.HandleException(ex);
                            }
                        }));
                    }
                    catch (Exception ex)
                    {
                        ExceptionUtils.HandleException(ex);
                    }
                }, () =>
                {
                    return CanAddFretLabel;
                }));
            }
        }
        private RelayCommand _addFretLabel;

        public bool CanAddFretLabel
        {
            get
            {
                FretLabelPosition flp = FretLabelPosition;
                return (null != flp && Diagram.CanAddNewFretLabelAt(flp));
            }
        }

        public string EditFretLabelLabel
        {
            get
            {
                FretLabelPosition flp = FretLabelPosition;
                return null != flp && flp.Side == FretLabelSide.Left ? Strings.ObservableDiagramEditLeftFretLabelLabel : Strings.ObservableDiagramEditRightFretLabelLabel;
            }
        }

        public string EditFretLabelToolTip
        {
            get
            {
                FretLabelPosition flp = FretLabelPosition;
                return null != flp && flp.Side == FretLabelSide.Left ? Strings.ObservableDiagramEditLeftFretLabelToolTip : Strings.ObservableDiagramEditRightFretLabelToolTip;
            }
        }

        public RelayCommand EditFretLabel
        {
            get
            {
                return _editFretLabel ?? (_editFretLabel = new RelayCommand(() =>
                {
                    try
                    {
                        DiagramFretLabel dfl = (DiagramFretLabel)Diagram.ElementAt(FretLabelPosition);
                        Messenger.Default.Send(new ShowDiagramFretLabelEditorMessage(dfl, false, (changed) =>
                        {
                            try
                            {
                                if (changed)
                                {
                                    Refresh();
                                }
                            }
                            catch (Exception ex)
                            {
                                ExceptionUtils.HandleException(ex);
                            }
                        }));
                    }
                    catch (Exception ex)
                    {
                        ExceptionUtils.HandleException(ex);
                    }
                }, () =>
                {
                    return CanEditFretLabel;
                }));
            }
        }
        private RelayCommand _editFretLabel;

        public bool CanEditFretLabel
        {
            get
            {
                FretLabelPosition flp = FretLabelPosition;
                return (null != flp && Diagram.ValidPosition(flp) && Diagram.HasElementAt(flp));
            }
        }

        public string RemoveFretLabelLabel
        {
            get
            {
                FretLabelPosition flp = FretLabelPosition;
                return null != flp && flp.Side == FretLabelSide.Left ? Strings.ObservableDiagramRemoveLeftFretLabelLabel : Strings.ObservableDiagramRemoveRightFretLabelLabel;
            }
        }

        public string RemoveFretLabelToolTip
        {
            get
            {
                FretLabelPosition flp = FretLabelPosition;
                return null != flp && flp.Side == FretLabelSide.Left ? Strings.ObservableDiagramRemoveLeftFretLabelToolTip : Strings.ObservableDiagramRemoveRightFretLabelToolTip;
            }
        }

        public RelayCommand RemoveFretLabel
        {
            get
            {
                return _removeFretLabel ?? (_removeFretLabel = new RelayCommand(() =>
                {
                    try
                    {
                        FretLabelPosition flp = FretLabelPosition;
                        Diagram.RemoveFretLabel(flp);
                        Refresh();
                    }
                    catch (Exception ex)
                    {
                        ExceptionUtils.HandleException(ex);
                    }
                }, () =>
                {
                    return CanRemoveFretLabel;
                }));
            }
        }
        private RelayCommand _removeFretLabel;

        public bool CanRemoveFretLabel
        {
            get
            {
                FretLabelPosition flp = FretLabelPosition;
                return (null != flp && Diagram.CanRemoveFretLabelAt(flp));
            }
        }

        internal FretLabelPosition FretLabelPosition
        {
            get
            {
                return (FretLabelPosition)Diagram.GetPosition<FretLabelPosition>(CursorX, CursorY);
            }
        }

        #endregion

        #region Barres

        public string AddBarreLabel
        {
            get
            {
                return Strings.ObservableDiagramAddBarreLabel;
            }
        }

        public string AddBarreToolTip
        {
            get
            {
                return Strings.ObservableDiagramAddBarreToolTip;
            }
        }

        public RelayCommand AddBarre
        {
            get
            {
                return _addBarre ?? (_addBarre = new RelayCommand(() =>
                {
                    try
                    {
                        BarrePosition bp = BarrePosition;
                        Messenger.Default.Send(new PromptForTextMessage(string.Format(Strings.ObservableDiagramAddBarrePromptFormat, 2, bp.Width), bp.Width.ToString(), (widthText) =>
                        {
                            try
                            {
                                int width = int.Parse(widthText);
                                bp = new BarrePosition(bp.Fret, bp.StartString, bp.StartString + width - 1);

                                DiagramBarre db = Diagram.NewBarre(bp);
                                Messenger.Default.Send(new ShowDiagramBarreEditorMessage(db, true, (changed) =>
                                {
                                    try
                                    {
                                        if (changed)
                                        {
                                            Refresh();
                                        }
                                        else
                                        {
                                            Diagram.RemoveBarre(bp);
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        ExceptionUtils.HandleException(ex);
                                    }
                                }));
                            }
                            catch (Exception ex)
                            {
                                ExceptionUtils.HandleException(ex);
                            }
                        }));
                    }
                    catch (Exception ex)
                    {
                        ExceptionUtils.HandleException(ex);
                    }
                }, () =>
                {
                    return CanAddBarre;
                }));
            }
        }
        private RelayCommand _addBarre;

        public bool CanAddBarre
        {
            get
            {
                BarrePosition bp = BarrePosition;
                return (null != bp && Diagram.CanAddNewBarreAt(bp));
            }
        }

        public string EditBarreLabel
        {
            get
            {
                return Strings.ObservableDiagramEditBarreLabel;
            }
        }

        public string EditBarreToolTip
        {
            get
            {
                return Strings.ObservableDiagramEditBarreToolTip;
            }
        }

        public RelayCommand EditBarre
        {
            get
            {
                return _editBarre ?? (_editBarre = new RelayCommand(() =>
                {
                    try
                    {
                        DiagramBarre db = (DiagramBarre)Diagram.ElementAt(BarrePosition);
                        Messenger.Default.Send(new ShowDiagramBarreEditorMessage(db, false, (changed) =>
                        {
                            try
                            {
                                if (changed)
                                {
                                    Refresh();
                                }
                            }
                            catch (Exception ex)
                            {
                                ExceptionUtils.HandleException(ex);
                            }
                        }));
                    }
                    catch (Exception ex)
                    {
                        ExceptionUtils.HandleException(ex);
                    }
                }, () =>
                {
                    return CanEditBarre;
                }));
            }
        }
        private RelayCommand _editBarre;

        public bool CanEditBarre
        {
            get
            {
                BarrePosition bp = BarrePosition;
                return (null != bp && Diagram.ValidPosition(bp) && Diagram.HasElementAt(bp));
            }
        }

        public string RemoveBarreLabel
        {
            get
            {
                return Strings.ObservableDiagramRemoveBarreLabel;
            }
        }

        public string RemoveBarreToolTip
        {
            get
            {
                return Strings.ObservableDiagramRemoveBarreToolTip;
            }
        }

        public RelayCommand RemoveBarre
        {
            get
            {
                return _removeBarre ?? (_removeBarre = new RelayCommand(() =>
                {
                    try
                    {
                        BarrePosition bp = BarrePosition;
                        Diagram.RemoveBarre(bp);
                        Refresh();
                    }
                    catch (Exception ex)
                    {
                        ExceptionUtils.HandleException(ex);
                    }
                }, () =>
                {
                    return CanRemoveBarre;
                }));
            }
        }
        private RelayCommand _removeBarre;

        public bool CanRemoveBarre
        {
            get
            {
                BarrePosition bp = BarrePosition;
                return (null != bp && Diagram.CanRemoveBarreAt(bp));
            }
        }

        internal BarrePosition BarrePosition
        {
            get
            {
                return (BarrePosition)Diagram.GetPosition<BarrePosition>(CursorX, CursorY);
            }
        }

        #endregion

        #region Cursor Info

        public double CursorX
        {
            get
            {
                return _cursorX;
            }
            set
            {
                _cursorX = value;
                RaisePropertyChanged(nameof(CursorX));
                RefreshCursor();
            }
        }
        private double _cursorX;

        public double CursorY
        {
            get
            {
                return _cursorY;
            }
            set
            {
                _cursorY = value;
                RaisePropertyChanged(nameof(CursorY));
                RefreshCursor();
            }
        }
        private double _cursorY;

        public bool CursorInGrid
        {
            get
            {
                return Diagram.InGrid(CursorX, CursorY);
            }
        }

        public bool ValidCommandsAtCursor
        {
            get
            {
                return (CanAddMark || CanEditMark || CanRemoveMark)
                    || (CanAddFretLabel || CanEditFretLabel || CanRemoveFretLabel)
                    || (CanAddBarre || CanEditBarre || CanRemoveBarre);
            }
        }

        private void RefreshCursor()
        {
            RaisePropertyChanged(nameof(CursorInGrid));
            RaisePropertyChanged(nameof(ValidCommandsAtCursor));

            EditElement.RaiseCanExecuteChanged();

            AddMark.RaiseCanExecuteChanged();
            RaisePropertyChanged(nameof(CanAddMark));

            EditMark.RaiseCanExecuteChanged();
            RaisePropertyChanged(nameof(CanEditMark));

            RemoveMark.RaiseCanExecuteChanged();
            RaisePropertyChanged(nameof(CanRemoveMark));

            AddFretLabel.RaiseCanExecuteChanged();
            RaisePropertyChanged(nameof(CanAddFretLabel));
            RaisePropertyChanged(nameof(AddFretLabelLabel));
            RaisePropertyChanged(nameof(AddFretLabelToolTip));

            EditFretLabel.RaiseCanExecuteChanged();
            RaisePropertyChanged(nameof(CanEditFretLabel));
            RaisePropertyChanged(nameof(EditFretLabelLabel));
            RaisePropertyChanged(nameof(EditFretLabelToolTip));

            RemoveFretLabel.RaiseCanExecuteChanged();
            RaisePropertyChanged(nameof(CanRemoveFretLabel));
            RaisePropertyChanged(nameof(RemoveFretLabelLabel));
            RaisePropertyChanged(nameof(RemoveFretLabelToolTip));

            AddBarre.RaiseCanExecuteChanged();
            RaisePropertyChanged(nameof(CanAddBarre));

            EditBarre.RaiseCanExecuteChanged();
            RaisePropertyChanged(nameof(CanEditBarre));

            RemoveBarre.RaiseCanExecuteChanged();
            RaisePropertyChanged(nameof(CanRemoveBarre));
        }

        public static bool IsCursorProperty(string property)
        {
            if (string.IsNullOrWhiteSpace(property))
            {
                throw new ArgumentNullException(nameof(property));
            }

            if (property == nameof(CursorX) ||
                property == nameof(CursorY))
            {
                return true;
            }

            foreach (string cursorProperty in _cursorProperties)
            {
                if (property == cursorProperty)
                {
                    return true;
                }
            }
            return false;
        }

        private static readonly string[] _cursorProperties = new string[] {
            nameof(CursorInGrid),
            nameof(ValidCommandsAtCursor),
            nameof(EditElement),
            nameof(AddMark),
            nameof(CanAddMark),
            nameof(EditMark),
            nameof(CanEditMark),
            nameof(RemoveMark),
            nameof(CanRemoveMark),
            nameof(AddFretLabel),
            nameof(CanAddFretLabel),
            nameof(EditFretLabel),
            nameof(CanEditFretLabel),
            nameof(RemoveFretLabel),
            nameof(CanRemoveFretLabel),
            nameof(AddBarre),
            nameof(CanAddBarre),
            nameof(EditBarre),
            nameof(CanEditBarre),
            nameof(RemoveBarre),
            nameof(CanRemoveBarre)
        };

        #endregion

        #region Editor

        public RelayCommand ShowEditor
        {
            get
            {
                return _showEditor ?? (_showEditor = new RelayCommand(() =>
                {
                    try
                    {
                        Messenger.Default.Send(new ShowDiagramEditorMessage(this, false, (changed) =>
                        {
                            try
                            {
                                PostEditCallback?.Invoke(changed);
                            }
                            catch (Exception ex)
                            {
                                ExceptionUtils.HandleException(ex);
                            }
                        }));
                    }
                    catch (Exception ex)
                    {
                        ExceptionUtils.HandleException(ex);
                    }
                }));
            }
        }
        private RelayCommand _showEditor;

        public bool IsEditMode
        {
            get
            {
                return _isEditMode;
            }
            set
            {
                _isEditMode = value;
                RaisePropertyChanged(nameof(IsEditMode));
                Refresh();
            }
        }
        private bool _isEditMode = false;

        public Action<bool> PostEditCallback
        {
            get
            {
                return _postEditCallback;
            }
            set
            {
                _postEditCallback = value ?? throw new ArgumentNullException();
                RaisePropertyChanged(nameof(PostEditCallback));
            }
        }
        private Action<bool> _postEditCallback;

        #endregion

        #region SendToClipboard

        public string SendImageToClipboardLabel
        {
            get
            {
                return Strings.ObservableDiagramSendImageToClipboardLabel;
            }
        }

        public string SendImageToClipboardToolTip
        {
            get
            {
                return Strings.ObservableDiagramSendImageToClipboardToolTip;
            }
        }

        public RelayCommand SendImageToClipboard
        {
            get
            {
                return _sendImageToClipboard ?? (_sendImageToClipboard = new RelayCommand(() =>
                {
                    try
                    {
                        AppVM.AppView.DiagramToClipboard(this, 1.0f);
                    }
                    catch (Exception ex)
                    {
                        ExceptionUtils.HandleException(ex);
                    }
                }));
            }
        }
        private RelayCommand _sendImageToClipboard;

        public string SendScaledImageToClipboardLabel
        {
            get
            {
                return Strings.ObservableDiagramSendScaledImageToClipboardLabel;
            }
        }

        public string SendScaledImageToClipboardToolTip
        {
            get
            {
                return Strings.ObservableDiagramSendScaledImageToClipboardToolTip;
            }
        }

        public RelayCommand SendScaledImageToClipboard
        {
            get
            {
                return _sendScaledImageToClipboard ?? (_sendScaledImageToClipboard = new RelayCommand(() =>
                {
                    try
                    {
                        float scaleFactor = AppVM.Settings.GetFloat("observablediagram.sendscaledimagetoclipboard.scalefactor", 1.0f);
                        string defaultValue = ((int)(Math.Round(scaleFactor * 100))).ToString();

                        Messenger.Default.Send(new PromptForTextMessage(Strings.ObservableDiagramSendScaledImageToClipboardScalePercentagePrompt, defaultValue, (percentText) =>
                        {
                            try
                            {
                                percentText = percentText.Trim();

                                if (int.TryParse(percentText, out int result))
                                {
                                    scaleFactor = 0.01f * result;
                                }

                                AppVM.AppView.DiagramToClipboard(this, scaleFactor);
                                AppVM.Settings.Set("observablediagram.sendscaledimagetoclipboard.scalefactor", scaleFactor);
                            }
                            catch (Exception ex)
                            {
                                ExceptionUtils.HandleException(ex);
                            }
                        }));
                    }
                    catch (Exception ex)
                    {
                        ExceptionUtils.HandleException(ex);
                    }
                }));
            }
        }
        private RelayCommand _sendScaledImageToClipboard;

        public string SendTextToClipboardLabel
        {
            get
            {
                return Strings.ObservableDiagramSendTextToClipboardLabel;
            }
        }

        public string SendTextToClipboardToolTip
        {
            get
            {
                return Strings.ObservableDiagramSendTextToClipboardToolTip;
            }
        }

        public RelayCommand SendTextToClipboard
        {
            get
            {
                return _sendTextToClipboard ?? (_sendTextToClipboard = new RelayCommand(() =>
                {
                    try
                    {
                        AppVM.AppView.TextToClipboard(SvgText);
                    }
                    catch (Exception ex)
                    {
                        ExceptionUtils.HandleException(ex);
                    }
                }));
            }
        }
        private RelayCommand _sendTextToClipboard;

        #endregion

        public RelayCommand RenderImage
        {
            get
            {
                return _renderImage ?? (_renderImage = new RelayCommand(() =>
                {
                    try
                    {
                        Render();
                    }
                    catch (Exception ex)
                    {
                        ExceptionUtils.HandleException(ex);
                    }
                }));
            }
        }
        private RelayCommand _renderImage;

        public bool AutoRender
        {
            get
            {
                return _autoRender;
            }
            set
            {
                _autoRender = value;
                RaisePropertyChanged(nameof(AutoRender));
            }
        }
        private bool _autoRender;

        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
                RaisePropertyChanged(nameof(Name));
            }
        }
        private string _name;

        public ObservableDiagramStyle Style { get; private set; }

        internal Diagram Diagram
        {
            get
            {
                return _diagram;
            }
            set
            {
                _diagram = value ?? throw new ArgumentNullException();
                Refresh();
            }
        }
        private Diagram _diagram;

        public ObservableDiagram(Diagram diagram, bool autoRender = true, string name = "") : base()
        {
            _autoRender = autoRender;
            _name = name;
            _diagram = diagram ?? throw new ArgumentNullException(nameof(diagram));

            Style = new ObservableDiagramStyle(diagram.Style);
            Style.PropertyChanged += Style_PropertyChanged;

            if (AutoRender)
            {
                Render();
            }
        }

        private void Style_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Refresh();
        }

        public void ResetStyles()
        {
            Diagram.ClearStyles();
            RaisePropertyChanged(nameof(Style));
            Refresh();
        }

        public void Refresh()
        {
            RaisePropertyChanged(nameof(SvgText));
            RaisePropertyChanged(nameof(TotalHeight));
            RaisePropertyChanged(nameof(TotalWidth));

            if (AutoRender)
            {
                Render();
            }
        }

        protected void Render()
        {
            AppVM.AppView.DoOnUIThread(() =>
            {
                ImageObject = AppVM.AppView.SvgTextToImage(SvgText, TotalWidth, TotalHeight, IsEditMode);
            });
        }

        public override string ToString()
        {
            if (!string.IsNullOrWhiteSpace(Name))
            {
                return Name;
            }

            return Strings.ObservableDiagramName;
        }
    }
}
