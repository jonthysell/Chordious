// Copyright (c) Jon Thysell <http://jonthysell.com>
// Licensed under the MIT License.

using System;

using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;

using Chordious.Core.ViewModel.Resources;

namespace Chordious.Core.ViewModel
{
    public class ChordQualityManagerViewModel : NamedIntervalManagerViewModel
    {
        public override string Title
        {
            get
            {
                return Strings.ChordQualityManagerTitle;
            }
        }

        public override string DefaultNamedIntervalGroupLabel
        {
            get
            {
                return Strings.ChordQualityManagerDefaultNamedIntervalGroupLabel;
            }
        }

        public override string DefaultNamedIntervalGroupToolTip
        {
            get
            {
                return Strings.ChordQualityManagerDefaultNamedIntervalGroupToolTip;
            }
        }

        public override string UserNamedIntervalGroupLabel
        {
            get
            {
                return Strings.ChordQualityManagerUserNamedIntervalGroupLabel;
            }
        }

        public override string UserNamedIntervalGroupToolTip
        {
            get
            {
                return Strings.ChordQualityManagerUserNamedIntervalGroupToolTip;
            }
        }

        public override string AddNamedIntervalToolTip
        {
            get
            {
                return Strings.ChordQualityManagerAddNamedIntervalToolTip;
            }
        }

        public override RelayCommand AddNamedInterval
        {
            get
            {
                return _addNamedInterval ?? (_addNamedInterval = new RelayCommand(() =>
                {
                    try
                    {
                        Messenger.Default.Send(new ShowChordQualityEditorMessage((name, abbreviation, intervals) =>
                        {
                            try
                            {
                                ChordQuality addedChordQuality = AppVM.UserConfig.ChordQualities.Add(name, abbreviation, intervals);
                                Refresh(addedChordQuality);
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
        private RelayCommand _addNamedInterval;

        public override string EditNamedIntervalToolTip
        {
            get
            {
                return Strings.ChordQualityManagerEditNamedIntervalToolTip;
            }
        }

        public override RelayCommand EditNamedInterval
        {
            get
            {
                return _editNamedInterval ?? (_editNamedInterval = new RelayCommand(() =>
                {
                    try
                    {
                        Messenger.Default.Send(new ShowChordQualityEditorMessage(SelectedNamedInterval.Name, ((ChordQuality)(SelectedNamedInterval.NamedInterval)).Abbreviation, SelectedNamedInterval.Intervals, SelectedNamedInterval.ReadOnly, (name, abbreviation, intervals) =>
                        {
                            try
                            {
                                ChordQuality cq = (ChordQuality)(SelectedNamedInterval.NamedInterval);
                                cq.Update(name, abbreviation, intervals);
                                Refresh(SelectedNamedInterval.NamedInterval);
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
                    return NamedIntervalIsSelected;
                }));
            }
        }
        private RelayCommand _editNamedInterval;

        public override string DeleteNamedIntervalToolTip
        {
            get
            {
                return Strings.ChordQualityManagerDeleteNamedIntervalToolTip;
            }
        }

        public override RelayCommand DeleteNamedInterval
        {
            get
            {
                return _deleteNamedInterval ?? (_deleteNamedInterval = new RelayCommand(() =>
                {
                    try
                    {
                        Messenger.Default.Send(new ConfirmationMessage(string.Format(Strings.ChordQualityManagerDeleteNamedIntervalPromptFormat, SelectedNamedInterval.LongName), (confirm) =>
                        {
                            try
                            {
                                if (confirm)
                                {
                                    _deleteUserNamedInterval(SelectedNamedInterval.NamedInterval);
                                    Refresh();
                                }
                            }
                            catch (Exception ex)
                            {
                                ExceptionUtils.HandleException(ex);
                            }
                        }, "confirmation.chordqualitymanager.deletenamedinterval"));
                    }
                    catch (Exception ex)
                    {
                        ExceptionUtils.HandleException(ex);
                    }
                }, () =>
                {
                    return NamedIntervalIsSelected && !SelectedNamedInterval.ReadOnly;
                }));
            }
        }
        private RelayCommand _deleteNamedInterval;

        public ChordQualityManagerViewModel() : base(AppViewModel.Instance.GetDefaultChordQualities, AppViewModel.Instance.GetUserChordQualities, AppViewModel.Instance.UserConfig.ChordQualities.Remove) { }
    }
}
