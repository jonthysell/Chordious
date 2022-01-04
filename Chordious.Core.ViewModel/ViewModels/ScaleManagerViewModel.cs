// Copyright (c) Jon Thysell <http://jonthysell.com>
// Licensed under the MIT License.

using System;

using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;

using Chordious.Core.ViewModel.Resources;

namespace Chordious.Core.ViewModel
{
    public class ScaleManagerViewModel : NamedIntervalManagerViewModel
    {
        public override string Title
        {
            get
            {
                return Strings.ScaleManagerTitle;
            }
        }

        public override string DefaultNamedIntervalGroupLabel
        {
            get
            {
                return Strings.ScaleManagerDefaultNamedIntervalGroupLabel;
            }
        }

        public override string DefaultNamedIntervalGroupToolTip
        {
            get
            {
                return Strings.ScaleManagerDefaultNamedIntervalGroupToolTip;
            }
        }

        public override string UserNamedIntervalGroupLabel
        {
            get
            {
                return Strings.ScaleManagerUserNamedIntervalGroupLabel;
            }
        }

        public override string UserNamedIntervalGroupToolTip
        {
            get
            {
                return Strings.ScaleManagerUserNamedIntervalGroupToolTip;
            }
        }

        public override string AddNamedIntervalToolTip
        {
            get
            {
                return Strings.ScaleManagerAddNamedIntervalToolTip;
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
                        Messenger.Default.Send(new ShowScaleEditorMessage((name, intervals) =>
                        {
                            try
                            {
                                Scale addedScale = AppVM.UserConfig.Scales.Add(name, intervals);
                                Refresh(addedScale);
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
                return Strings.ScaleManagerEditNamedIntervalToolTip;
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
                        Messenger.Default.Send(new ShowScaleEditorMessage(SelectedNamedInterval.Name, SelectedNamedInterval.Intervals, SelectedNamedInterval.ReadOnly, (name, intervals) =>
                        {
                            try
                            {
                                SelectedNamedInterval.NamedInterval.Update(name, intervals);
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
                return Strings.ScaleManagerDeleteNamedIntervalToolTip;
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
                        Messenger.Default.Send(new ConfirmationMessage(string.Format(Strings.ScaleManagerDeleteNamedIntervalPromptFormat, SelectedNamedInterval.LongName), (confirm) =>
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
                        }, "confirmation.scalemanager.deletenamedinterval"));
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

        public ScaleManagerViewModel() : base(AppViewModel.Instance.GetDefaultScales, AppViewModel.Instance.GetUserScales, AppViewModel.Instance.UserConfig.Scales.Remove) { }
    }
}
