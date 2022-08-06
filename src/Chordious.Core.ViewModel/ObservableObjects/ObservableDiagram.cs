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
                OnPropertyChanged(nameof(ImageObject));
            }
        }
        private object _imageObject;

        #region Dimensions

        public static string TotalWidthLabel
        {
            get
            {
                return Strings.DiagramTotalWidthLabel;
            }
        }

        public static string TotalWidthToolTip
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

        public static string TotalHeightLabel
        {
            get
            {
                return Strings.DiagramTotalHeightLabel;
            }
        }

        public static string TotalHeightToolTip
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

        public static string NumFretsLabel
        {
            get
            {
                return Strings.DiagramNumFretsLabel;
            }
        }

        public static string NumFretsToolTip
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
                    OnPropertyChanged(nameof(NumFrets));
                    Refresh();
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }
        }

        public static string NumStringsLabel
        {
            get
            {
                return Strings.DiagramNumStringsLabel;
            }
        }

        public static string NumStringsToolTip
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
                    OnPropertyChanged(nameof(NumStrings));
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

        public static string TitleLabel
        {
            get
            {
                return Strings.DiagramTitleTextLabel;
            }
        }

        public static string TitleToolTip
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
                    OnPropertyChanged(nameof(Title));
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
                return _editElement ??= new RelayCommand(() =>
                {
                    try
                    {
                        MarkPosition mp = MarkPosition;
                        BarrePosition bp = BarrePosition;
                        FretLabelPosition flp = FretLabelPosition;

                        bool hasMark = mp is not null && Diagram.HasElementAt(mp);
                        bool hasBarre = bp is not null && Diagram.HasElementAt(bp);
                        bool hasFretLabel = flp is not null && Diagram.HasElementAt(flp);

                        if (hasMark)
                        {
                            DiagramMark dm = (DiagramMark)Diagram.ElementAt(mp);
                            StrongReferenceMessenger.Default.Send(new ShowDiagramMarkEditorMessage(dm, false, (changed) =>
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
                            StrongReferenceMessenger.Default.Send(new ShowDiagramBarreEditorMessage(db, false, (changed) =>
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
                            StrongReferenceMessenger.Default.Send(new ShowDiagramFretLabelEditorMessage(dfl, false, (changed) =>
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
                });
            }
        }
        private RelayCommand _editElement;

        #endregion

        #region Marks

        public static string AddMarkLabel
        {
            get
            {
                return Strings.ObservableDiagramAddMarkLabel;
            }
        }

        public static string AddMarkToolTip
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
                return _addMark ??= new RelayCommand(() =>
                {
                    try
                    {
                        MarkPosition mp = MarkPosition;
                        DiagramMark dm = Diagram.NewMark(mp);
                        StrongReferenceMessenger.Default.Send(new ShowDiagramMarkEditorMessage(dm, true, (changed) =>
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
                });
            }
        }
        private RelayCommand _addMark;

        public bool CanAddMark
        {
            get
            {
                MarkPosition mp = MarkPosition;
                return (mp is not null && Diagram.CanAddNewMarkAt(mp));
            }
        }

        public static string EditMarkLabel
        {
            get
            {
                return Strings.ObservableDiagramEditMarkLabel;
            }
        }

        public static string EditMarkToolTip
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
                return _editMark ??= new RelayCommand(() =>
                {
                    try
                    {
                        DiagramMark dm = (DiagramMark)Diagram.ElementAt(MarkPosition);
                        StrongReferenceMessenger.Default.Send(new ShowDiagramMarkEditorMessage(dm, false, (changed) =>
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
                });
            }
        }
        private RelayCommand _editMark;

        public bool CanEditMark
        {
            get
            {
                MarkPosition mp = MarkPosition;
                return (mp is not null && Diagram.ValidPosition(mp) && Diagram.HasElementAt(mp));
            }
        }

        public static string RemoveMarkLabel
        {
            get
            {
                return Strings.ObservableDiagramRemoveMarkLabel;
            }
        }

        public static string RemoveMarkToolTip
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
                return _removeMark ??= new RelayCommand(() =>
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
                });
            }
        }
        private RelayCommand _removeMark;

        public bool CanRemoveMark
        {
            get
            {
                MarkPosition mp = MarkPosition;
                return (mp is not null && Diagram.CanRemoveMarkAt(mp));
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
                return flp is not null && flp.Side == FretLabelSide.Left ? Strings.ObservableDiagramAddLeftFretLabelLabel : Strings.ObservableDiagramAddRightFretLabelLabel;
            }
        }

        public string AddFretLabelToolTip
        {
            get
            {
                FretLabelPosition flp = FretLabelPosition;
                return flp is not null && flp.Side == FretLabelSide.Left ? Strings.ObservableDiagramAddLeftFretLabelToolTip : Strings.ObservableDiagramAddRightFretLabelToolTip;
            }
        }

        public RelayCommand AddFretLabel
        {
            get
            {
                return _addFretLabel ??= new RelayCommand(() =>
                {
                    try
                    {
                        FretLabelPosition flp = FretLabelPosition;
                        DiagramFretLabel dfl = Diagram.NewFretLabel(flp, "");
                        StrongReferenceMessenger.Default.Send(new ShowDiagramFretLabelEditorMessage(dfl, true, (changed) =>
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
                });
            }
        }
        private RelayCommand _addFretLabel;

        public bool CanAddFretLabel
        {
            get
            {
                FretLabelPosition flp = FretLabelPosition;
                return (flp is not null && Diagram.CanAddNewFretLabelAt(flp));
            }
        }

        public string EditFretLabelLabel
        {
            get
            {
                FretLabelPosition flp = FretLabelPosition;
                return flp is not null && flp.Side == FretLabelSide.Left ? Strings.ObservableDiagramEditLeftFretLabelLabel : Strings.ObservableDiagramEditRightFretLabelLabel;
            }
        }

        public string EditFretLabelToolTip
        {
            get
            {
                FretLabelPosition flp = FretLabelPosition;
                return flp is not null && flp.Side == FretLabelSide.Left ? Strings.ObservableDiagramEditLeftFretLabelToolTip : Strings.ObservableDiagramEditRightFretLabelToolTip;
            }
        }

        public RelayCommand EditFretLabel
        {
            get
            {
                return _editFretLabel ??= new RelayCommand(() =>
                {
                    try
                    {
                        DiagramFretLabel dfl = (DiagramFretLabel)Diagram.ElementAt(FretLabelPosition);
                        StrongReferenceMessenger.Default.Send(new ShowDiagramFretLabelEditorMessage(dfl, false, (changed) =>
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
                });
            }
        }
        private RelayCommand _editFretLabel;

        public bool CanEditFretLabel
        {
            get
            {
                FretLabelPosition flp = FretLabelPosition;
                return (flp is not null && Diagram.ValidPosition(flp) && Diagram.HasElementAt(flp));
            }
        }

        public string RemoveFretLabelLabel
        {
            get
            {
                FretLabelPosition flp = FretLabelPosition;
                return flp is not null && flp.Side == FretLabelSide.Left ? Strings.ObservableDiagramRemoveLeftFretLabelLabel : Strings.ObservableDiagramRemoveRightFretLabelLabel;
            }
        }

        public string RemoveFretLabelToolTip
        {
            get
            {
                FretLabelPosition flp = FretLabelPosition;
                return flp is not null && flp.Side == FretLabelSide.Left ? Strings.ObservableDiagramRemoveLeftFretLabelToolTip : Strings.ObservableDiagramRemoveRightFretLabelToolTip;
            }
        }

        public RelayCommand RemoveFretLabel
        {
            get
            {
                return _removeFretLabel ??= new RelayCommand(() =>
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
                });
            }
        }
        private RelayCommand _removeFretLabel;

        public bool CanRemoveFretLabel
        {
            get
            {
                FretLabelPosition flp = FretLabelPosition;
                return (flp is not null && Diagram.CanRemoveFretLabelAt(flp));
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

        public static string AddBarreLabel
        {
            get
            {
                return Strings.ObservableDiagramAddBarreLabel;
            }
        }

        public static string AddBarreToolTip
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
                return _addBarre ??= new RelayCommand(() =>
                {
                    try
                    {
                        BarrePosition bp = BarrePosition;
                        StrongReferenceMessenger.Default.Send(new PromptForTextMessage(string.Format(Strings.ObservableDiagramAddBarrePromptFormat, 2, bp.Width), bp.Width.ToString(), (widthText) =>
                        {
                            try
                            {
                                int width = int.Parse(widthText);
                                bp = new BarrePosition(bp.Fret, bp.StartString, bp.StartString + width - 1);

                                DiagramBarre db = Diagram.NewBarre(bp);
                                StrongReferenceMessenger.Default.Send(new ShowDiagramBarreEditorMessage(db, true, (changed) =>
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
                        }, false, true));
                    }
                    catch (Exception ex)
                    {
                        ExceptionUtils.HandleException(ex);
                    }
                }, () =>
                {
                    return CanAddBarre;
                });
            }
        }
        private RelayCommand _addBarre;

        public bool CanAddBarre
        {
            get
            {
                BarrePosition bp = BarrePosition;
                return (bp is not null && Diagram.CanAddNewBarreAt(bp));
            }
        }

        public static string EditBarreLabel
        {
            get
            {
                return Strings.ObservableDiagramEditBarreLabel;
            }
        }

        public static string EditBarreToolTip
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
                return _editBarre ??= new RelayCommand(() =>
                {
                    try
                    {
                        DiagramBarre db = (DiagramBarre)Diagram.ElementAt(BarrePosition);
                        StrongReferenceMessenger.Default.Send(new ShowDiagramBarreEditorMessage(db, false, (changed) =>
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
                });
            }
        }
        private RelayCommand _editBarre;

        public bool CanEditBarre
        {
            get
            {
                BarrePosition bp = BarrePosition;
                return (bp is not null && Diagram.ValidPosition(bp) && Diagram.HasElementAt(bp));
            }
        }

        public static string RemoveBarreLabel
        {
            get
            {
                return Strings.ObservableDiagramRemoveBarreLabel;
            }
        }

        public static string RemoveBarreToolTip
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
                return _removeBarre ??= new RelayCommand(() =>
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
                });
            }
        }
        private RelayCommand _removeBarre;

        public bool CanRemoveBarre
        {
            get
            {
                BarrePosition bp = BarrePosition;
                return (bp is not null && Diagram.CanRemoveBarreAt(bp));
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
                OnPropertyChanged(nameof(CursorX));
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
                OnPropertyChanged(nameof(CursorY));
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
            OnPropertyChanged(nameof(CursorInGrid));
            OnPropertyChanged(nameof(ValidCommandsAtCursor));

            EditElement.NotifyCanExecuteChanged();

            AddMark.NotifyCanExecuteChanged();
            OnPropertyChanged(nameof(CanAddMark));

            EditMark.NotifyCanExecuteChanged();
            OnPropertyChanged(nameof(CanEditMark));

            RemoveMark.NotifyCanExecuteChanged();
            OnPropertyChanged(nameof(CanRemoveMark));

            AddFretLabel.NotifyCanExecuteChanged();
            OnPropertyChanged(nameof(CanAddFretLabel));
            OnPropertyChanged(nameof(AddFretLabelLabel));
            OnPropertyChanged(nameof(AddFretLabelToolTip));

            EditFretLabel.NotifyCanExecuteChanged();
            OnPropertyChanged(nameof(CanEditFretLabel));
            OnPropertyChanged(nameof(EditFretLabelLabel));
            OnPropertyChanged(nameof(EditFretLabelToolTip));

            RemoveFretLabel.NotifyCanExecuteChanged();
            OnPropertyChanged(nameof(CanRemoveFretLabel));
            OnPropertyChanged(nameof(RemoveFretLabelLabel));
            OnPropertyChanged(nameof(RemoveFretLabelToolTip));

            AddBarre.NotifyCanExecuteChanged();
            OnPropertyChanged(nameof(CanAddBarre));

            EditBarre.NotifyCanExecuteChanged();
            OnPropertyChanged(nameof(CanEditBarre));

            RemoveBarre.NotifyCanExecuteChanged();
            OnPropertyChanged(nameof(CanRemoveBarre));
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
                return _showEditor ??= new RelayCommand(() =>
                {
                    try
                    {
                        StrongReferenceMessenger.Default.Send(new ShowDiagramEditorMessage(this, false, (changed) =>
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
                });
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
                OnPropertyChanged(nameof(IsEditMode));
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
                _postEditCallback = value ?? throw new ArgumentNullException(nameof(value));
                OnPropertyChanged(nameof(PostEditCallback));
            }
        }
        private Action<bool> _postEditCallback;

        #endregion

        #region SendToClipboard

        public static string SendImageToClipboardLabel
        {
            get
            {
                return Strings.ObservableDiagramSendImageToClipboardLabel;
            }
        }

        public static string SendImageToClipboardToolTip
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
                return _sendImageToClipboard ??= new RelayCommand(() =>
                {
                    try
                    {
                        AppVM.AppView.DiagramToClipboard(this, 1.0f);
                    }
                    catch (Exception ex)
                    {
                        ExceptionUtils.HandleException(ex);
                    }
                });
            }
        }
        private RelayCommand _sendImageToClipboard;

        public static string SendScaledImageToClipboardLabel
        {
            get
            {
                return Strings.ObservableDiagramSendScaledImageToClipboardLabel;
            }
        }

        public static string SendScaledImageToClipboardToolTip
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
                return _sendScaledImageToClipboard ??= new RelayCommand(() =>
                {
                    try
                    {
                        float scaleFactor = AppVM.Settings.GetFloat("observablediagram.sendscaledimagetoclipboard.scalefactor", 1.0f);
                        string defaultValue = ((int)(Math.Round(scaleFactor * 100))).ToString();

                        StrongReferenceMessenger.Default.Send(new PromptForTextMessage(Strings.ObservableDiagramSendScaledImageToClipboardScalePercentagePrompt, defaultValue, (percentText) =>
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
                        }, false, true));
                    }
                    catch (Exception ex)
                    {
                        ExceptionUtils.HandleException(ex);
                    }
                });
            }
        }
        private RelayCommand _sendScaledImageToClipboard;

        public static string SendTextToClipboardLabel
        {
            get
            {
                return Strings.ObservableDiagramSendTextToClipboardLabel;
            }
        }

        public static string SendTextToClipboardToolTip
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
                return _sendTextToClipboard ??= new RelayCommand(() =>
                {
                    try
                    {
                        AppVM.AppView.TextToClipboard(SvgText);
                    }
                    catch (Exception ex)
                    {
                        ExceptionUtils.HandleException(ex);
                    }
                });
            }
        }
        private RelayCommand _sendTextToClipboard;

        #endregion

        public RelayCommand RenderImage
        {
            get
            {
                return _renderImage ??= new RelayCommand(() =>
                {
                    try
                    {
                        Render();
                    }
                    catch (Exception ex)
                    {
                        ExceptionUtils.HandleException(ex);
                    }
                });
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
                OnPropertyChanged(nameof(AutoRender));
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
                OnPropertyChanged(nameof(Name));
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
                _diagram = value ?? throw new ArgumentNullException(nameof(value));
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
            OnPropertyChanged(nameof(Style));
            Refresh();
        }

        public void Refresh()
        {
            OnPropertyChanged(nameof(SvgText));
            OnPropertyChanged(nameof(TotalHeight));
            OnPropertyChanged(nameof(TotalWidth));

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
