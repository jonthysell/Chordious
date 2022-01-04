// Copyright (c) Jon Thysell <http://jonthysell.com>
// Licensed under the MIT License.

using System;

using GalaSoft.MvvmLight.Messaging;

namespace Chordious.Core.ViewModel
{
    public class ExceptionUtils
    {
        public static AppViewModel AppVM
        {
            get
            {
                return AppViewModel.Instance;
            }
        }

        public static void HandleException(Exception exception)
        {
            AppVM.AppView.DoOnUIThread(() =>
            {
                Messenger.Default.Send(new ExceptionMessage(exception));
            });
        }
    }
}
