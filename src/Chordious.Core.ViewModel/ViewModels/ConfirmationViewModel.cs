// Copyright (c) Jon Thysell <http://jonthysell.com>
// Licensed under the MIT License.

using System;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using Chordious.Core.ViewModel.Resources;

namespace Chordious.Core.ViewModel
{
    public class ConfirmationViewModel : ObservableObject
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
                return Strings.ConfirmationTitle;
            }
        }

        public string Message { get; private set; }

        public bool DisplayDialog { get; private set; } = true;

        public string YesAndRememberLabel
        {
            get
            {
                return Strings.YesAndRememberLabel;
            }
        }

        public bool ShowAcceptAndRemember
        {
            get
            {
                return !string.IsNullOrWhiteSpace(_rememberAnswerKey);
            }
        }
        private readonly string _rememberAnswerKey;

        public RelayCommand AcceptAndRemember
        {
            get
            {
                return _acceptAndRemember ?? (_acceptAndRemember = new RelayCommand(() =>
                {
                    try
                    {
                        Result = ConfirmationResult.AcceptAndRemember;
                        RequestClose?.Invoke();
                    }
                    catch (Exception ex)
                    {
                        ExceptionUtils.HandleException(ex);
                    }
                }, () =>
                {
                    return ShowAcceptAndRemember;
                }));
            }
        }
        private RelayCommand _acceptAndRemember;

        public RelayCommand Accept
        {
            get
            {
                return _accept ?? (_accept = new RelayCommand(() =>
                {
                    try
                    {
                        Result = ConfirmationResult.Accept;
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

        public RelayCommand Reject
        {
            get
            {
                return _reject ?? (_reject = new RelayCommand(() =>
                {
                    try
                    {
                        Result = ConfirmationResult.Reject;
                        RequestClose?.Invoke();
                    }
                    catch (Exception ex)
                    {
                        ExceptionUtils.HandleException(ex);
                    }
                }));
            }
        }
        private RelayCommand _reject;

        public ConfirmationResult Result { get; private set; }

        public Action RequestClose;

        public Action<bool> Callback { get; private set; }

        public ConfirmationViewModel(string message, Action<bool> callback, string rememberAnswerKey)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                throw new ArgumentNullException(nameof(message));
            }

            Message = message;
            Callback = callback ?? throw new ArgumentNullException(nameof(callback));
            Result = ConfirmationResult.None;

            _rememberAnswerKey = rememberAnswerKey;

            if (ShowAcceptAndRemember)
            {
                if (AppVM.Settings.TryGet(rememberAnswerKey, out bool value))
                {
                    if (value)
                    {
                        DisplayDialog = false;
                        Result = ConfirmationResult.AcceptAndRemember;
                    }
                }
            }
        }

        public void ProcessClose()
        {
            if (ShowAcceptAndRemember)
            {
                AppVM.Settings.Set(_rememberAnswerKey, Result == ConfirmationResult.AcceptAndRemember);
            }
            
            Callback(Result == ConfirmationResult.Accept || Result == ConfirmationResult.AcceptAndRemember);
        }
    }

    public enum ConfirmationResult
    {
        None,
        Accept,
        AcceptAndRemember,
        Reject
    }
}
