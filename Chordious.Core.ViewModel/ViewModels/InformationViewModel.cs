// Copyright (c) Jon Thysell <http://jonthysell.com>
// Licensed under the MIT License.

using System;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace Chordious.Core.ViewModel
{
    public class InformationViewModel : ViewModelBase
    {
        public AppViewModel AppVM
        {
            get
            {
                return AppViewModel.Instance;
            }
        }

        public string Title { get; private set; }

        public string Message { get; private set; }

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

        public Action RequestClose;

        public Action Callback { get; private set; }

        public InformationViewModel(string message, string title, Action callback = null)
        {

            if (string.IsNullOrWhiteSpace(message))
            {
                throw new ArgumentNullException(nameof(message));
            }

            if (string.IsNullOrWhiteSpace(title))
            {
                throw new ArgumentNullException(nameof(title));
            }

            Title = title;
            Message = message;
            Callback = callback;
        }

        public void ProcessClose()
        {
            Callback?.Invoke();
        }
    }
}
