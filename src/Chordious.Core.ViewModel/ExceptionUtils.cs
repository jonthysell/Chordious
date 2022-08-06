// Copyright (c) Jon Thysell <http://jonthysell.com>
// Licensed under the MIT License.

using System;

using CommunityToolkit.Mvvm.Messaging;

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
                StrongReferenceMessenger.Default.Send(new ExceptionMessage(exception));
            });
        }
    }
}
