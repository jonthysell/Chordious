// 
// ConfirmationViewModel.cs
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

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;

using com.jonthysell.Chordious.Core.ViewModel.Resources;

namespace com.jonthysell.Chordious.Core.ViewModel
{
    public class ConfirmationViewModel : ViewModelBase
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

        public string Message
        {
            get
            {
                return _message;
            }
            private set
            {
                if (String.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentNullException();
                }
                _message = value;
            }
        }
        private string _message;

        public bool DisplayDialog { get; private set; }

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
                return !String.IsNullOrWhiteSpace(_rememberAnswerKey);
            }
        }
        private string _rememberAnswerKey;

        public RelayCommand AcceptAndRemember
        {
            get
            {
                return new RelayCommand(() =>
                {
                    try
                    {
                        Result = ConfirmationResult.AcceptAndRemember;
                        if (null != RequestClose)
                        {
                            RequestClose();
                        }
                    }
                    catch (Exception ex)
                    {
                        ExceptionUtils.HandleException(ex);
                    }
                }, () =>
                {
                    return ShowAcceptAndRemember;
                });
            }
        }

        public RelayCommand Accept
        {
            get
            {
                return new RelayCommand(() =>
                {
                    try
                    {
                        Result = ConfirmationResult.Accept;
                        if (null != RequestClose)
                        {
                            RequestClose();
                        }
                    }
                    catch (Exception ex)
                    {
                        ExceptionUtils.HandleException(ex);
                    }
                });
            }
        }

        public RelayCommand Reject
        {
            get
            {
                return new RelayCommand(() =>
                {
                    try
                    {
                        Result = ConfirmationResult.Reject;
                        if (null != RequestClose)
                        {
                            RequestClose();
                        }
                    }
                    catch (Exception ex)
                    {
                        ExceptionUtils.HandleException(ex);
                    }
                });
            }
        }

        public ConfirmationResult Result { get; private set; }

        public event Action RequestClose;

        public Action<bool> Callback
        {
            get
            {
                return _callback;
            }
            private set
            {
                if (null == value)
                {
                    throw new ArgumentNullException();
                }
                _callback = value;
            }
        }
        private Action<bool> _callback;

        public ConfirmationViewModel(string message, Action<bool> callback, string rememberAnswerKey)
        {
            Message = message;
            Callback = callback;
            DisplayDialog = true;
            Result = ConfirmationResult.None;

            _rememberAnswerKey = rememberAnswerKey;

            if (ShowAcceptAndRemember)
            {
                bool value;
                if (AppVM.Settings.TryGet(rememberAnswerKey, out value))
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
