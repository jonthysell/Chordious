// 
// ScaleManagerViewModel.cs
//  
// Author:
//       Jon Thysell <thysell@gmail.com>
// 
// Copyright (c) 2015, 2016, 2017 Jon Thysell <http://jonthysell.com>
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

using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;

using com.jonthysell.Chordious.Core.ViewModel.Resources;

namespace com.jonthysell.Chordious.Core.ViewModel
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
                return new RelayCommand(() =>
                {
                    try
                    {
                        Messenger.Default.Send<ShowScaleEditorMessage>(new ShowScaleEditorMessage(true, (name, intervals) =>
                        {
                            Scale addedScale = AppVM.UserConfig.Scales.Add(name, intervals);
                            Refresh(addedScale);
                        }));
                    }
                    catch (Exception ex)
                    {
                        ExceptionUtils.HandleException(ex);
                    }
                });
            }
        }

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
                return new RelayCommand(() =>
                {
                    try
                    {
                        Messenger.Default.Send<ShowScaleEditorMessage>(new ShowScaleEditorMessage(false, (name, intervals) =>
                        {
                            SelectedNamedInterval.NamedInterval.Update(name, intervals);
                            Refresh(SelectedNamedInterval.NamedInterval);
                        }, SelectedNamedInterval.Name, SelectedNamedInterval.Intervals));
                    }
                    catch (Exception ex)
                    {
                        ExceptionUtils.HandleException(ex);
                    }
                }, () =>
                {
                    return NamedIntervalIsSelected && SelectedNamedInterval.CanEdit;
                });
            }
        }

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
                return new RelayCommand(() =>
                {
                    try
                    {
                        Messenger.Default.Send<ConfirmationMessage>(new ConfirmationMessage(String.Format(Strings.ScaleManagerDeleteNamedIntervalPromptFormat, SelectedNamedInterval.LongName), (confirm) =>
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
                    return NamedIntervalIsSelected && SelectedNamedInterval.CanEdit;
                });
            }
        }

        public ScaleManagerViewModel() : base(AppViewModel.Instance.GetDefaultScales, AppViewModel.Instance.GetUserScales, AppViewModel.Instance.UserConfig.Scales.Remove) { }
    }
}
