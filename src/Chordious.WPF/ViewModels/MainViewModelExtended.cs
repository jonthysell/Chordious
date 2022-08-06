// Copyright (c) Jon Thysell <http://jonthysell.com>
// Licensed under the MIT License.

using System;
using System.Threading.Tasks;

using CommunityToolkit.Mvvm.Messaging;

using Chordious.Core.ViewModel;

using Chordious.WPF.Resources;

namespace Chordious.WPF
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
                OnPropertyChanged(nameof(IsIdle));
            }
        }
        private bool _isIdle;

        public bool IsFirstRun
        {
            get
            {

                if (bool.TryParse(AppVM.GetSetting("app.firstrun"), out bool result))
                {
                    return result;
                }

                return false;
            }
            set
            {
                AppVM.SetSetting("app.firstrun", value);
                OnPropertyChanged(nameof(IsFirstRun));
            }
        }

        public MainViewModelExtended() : base()
        {
            _isIdle = true;
        }

        public void OnLoaded()
        {
            AppVM.AppView.DoOnUIThread(async () =>
            {

                try
                {
                    IsIdle = false;

                    AppVM.TryHandleFailedUserConfigLoad();

                    if (IsFirstRun)
                    {
                        FirstRun();
                    }

                    if (UpdateUtils.UpdateEnabled && UpdateUtils.CheckUpdateOnStart && UpdateUtils.IsConnectedToInternet)
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

            if (UpdateUtils.UpdateEnabled)
            {
                StrongReferenceMessenger.Default.Send(new ConfirmationMessage(Strings.FirstRunUpdateEnabledPrompt, (enableAutoUpdate) =>
                {
                    try
                    {
                        UpdateUtils.CheckUpdateOnStart = enableAutoUpdate;
                    }
                    catch (Exception ex)
                    {
                        ExceptionUtils.HandleException(ex);
                    }
                }));
            }
            else
            {
                StrongReferenceMessenger.Default.Send(new ChordiousMessage(Strings.FirstRunMessage));
            }
        }
    }
}
