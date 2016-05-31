// 
// MainViewModelExtended.cs
//  
// Author:
//       Jon Thysell <thysell@gmail.com>
// 
// Copyright (c) 2015, 2016 Jon Thysell <http://jonthysell.com>
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
using System.Threading.Tasks;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;

using com.jonthysell.Chordious.Core.ViewModel;

using com.jonthysell.Chordious.WPF.Resources;

namespace com.jonthysell.Chordious.WPF
{
    public class MainViewModelExtended : MainViewModel, IIdle
    {
        public bool IsIdle
        {
            get
            {
                return _isIdle;
            }
            private set
            {
                _isIdle = value;
                RaisePropertyChanged("IsIdle");
            }
        }
        private bool _isIdle;

        public bool IsFirstRun
        {
            get
            {
                bool result;

                if (Boolean.TryParse(AppVM.GetSetting("app.firstrun"), out result))
                {
                    return result;
                }

                return false;
            }
            set
            {
                AppVM.SetSetting("app.firstrun", value);
                RaisePropertyChanged("IsFirstRun");
            }
        }

        public MainViewModelExtended() : base()
        {
            IsIdle = true;
        }

        public void OnLoaded()
        {
            Task.Factory.StartNew(async () =>
            {
                try
                {
                    IsIdle = false;

                    AppVM.TryHandleFailedUserConfigLoad();

                    if (IsFirstRun)
                    {
                        FirstRun();
                    }

                    if (UpdateUtils.GetCheckUpdateOnStart())
                    {
                        await UpdateUtils.UpdateCheckAsync(true, false);
                    }
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
                finally
                {
                    IsIdle = true;
                }
            });
        }

        private void FirstRun()
        {
            // Turn off first-run so it doesn't run next time
            IsFirstRun = false;

            AppVM.DoOnUIThread(() =>
            {
                Messenger.Default.Send<ConfirmationMessage>(new ConfirmationMessage(Strings.FirstRunMessage, (enableAutoUpdate) =>
                {
                    UpdateUtils.SetCheckUpdateOnStart(enableAutoUpdate);
                }));
            });
        }
    }
}
