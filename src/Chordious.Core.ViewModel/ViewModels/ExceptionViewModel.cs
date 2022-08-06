// Copyright (c) Jon Thysell <http://jonthysell.com>
// Licensed under the MIT License.

using System;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using Chordious.Core.ViewModel.Resources;

namespace Chordious.Core.ViewModel
{
    public class ExceptionViewModel : ObservableObject
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
                return Strings.ExceptionTitle;
            }
        }

        public string Message
        {
            get
            {
                return Exception.Message;
            }
        }

        public string DetailsLabel
        {
            get
            {
                return Strings.ExceptionDetailsLabel;
            }
        }

        public string DetailsToolTip
        {
            get
            {
                return Strings.ExceptionDetailsToolTip;
            }
        }

        public string Details
        {
            get
            {
                return string.Format(Strings.ExceptionViewModelDetailsFormat, Exception.ToString());
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
                    }
                    catch (Exception ex)
                    {
                        ExceptionUtils.HandleException(ex);
                    }
                }));
            }
        }
        private RelayCommand _accept;

        internal Exception Exception { get; private set; }

        public Action RequestClose;

        public ExceptionViewModel(Exception exception)
        {
            Exception = exception ?? throw new ArgumentNullException(nameof(exception));
        }
    }
}
