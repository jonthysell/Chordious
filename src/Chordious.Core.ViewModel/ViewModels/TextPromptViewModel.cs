// Copyright (c) Jon Thysell <http://jonthysell.com>
// Licensed under the MIT License.

using System;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

using Chordious.Core.ViewModel.Resources;

namespace Chordious.Core.ViewModel
{
    public class TextPromptViewModel : ViewModelBase
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
                return Strings.TextPromptTitle;
            }
        }

        public string Prompt { get; private set; }

        public string Text
        {
            get
            {
                return _text;
            }
            set
            {
                _text = value;
                RaisePropertyChanged(nameof(Text));
                Accept.RaiseCanExecuteChanged();
            }
        }
        private string _text;

        public bool AllowBlank
        {
            get
            {
                return _allowBlank;
            }
            set
            {
                _allowBlank = value;
                RaisePropertyChanged(nameof(AllowBlank));
                RaisePropertyChanged(nameof(Accept));
            }
        }
        private bool _allowBlank;

        public bool RequireInteger
        {
            get
            {
                return _requireInteger;
            }
            set
            {
                _requireInteger = value;
                RaisePropertyChanged(nameof(RequireInteger));
                RaisePropertyChanged(nameof(AllowText));
                RaisePropertyChanged(nameof(Accept));
            }
        }
        private bool _requireInteger;

        public bool AllowText
        {
            get
            {
                return !_requireInteger;
            }
        }

        public RelayCommand Accept
        {
            get
            {
                return _accept ?? (_accept = new RelayCommand(() =>
                {
                    try
                    {
                        RequestClose?.Invoke();
                        Callback(Text);
                    }
                    catch (Exception ex)
                    {
                        ExceptionUtils.HandleException(ex);
                    }
                }, () =>
                {
                    return (AllowBlank || !string.IsNullOrWhiteSpace(Text)) && (!RequireInteger || int.TryParse(Text, out _));
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

        public Action<string> Callback { get; private set; }

        public TextPromptViewModel(string prompt, Action<string> callback)
        {
            if (string.IsNullOrWhiteSpace(prompt))
            {
                throw new ArgumentNullException(nameof(prompt));
            }

            Prompt = prompt;
            Callback = callback ?? throw new ArgumentNullException(nameof(callback));
        }
    }
}
