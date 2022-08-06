// Copyright (c) Jon Thysell <http://jonthysell.com>
// Licensed under the MIT License.

using System;
using System.Collections.ObjectModel;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using Chordious.Core.ViewModel.Resources;

namespace Chordious.Core.ViewModel
{
    public class LicensesViewModel : ObservableObject
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
                return Strings.LicensesTitle;
            }
        }

        public ObservableCollection<ObservableLicense> Licenses { get; private set; }

        public RelayCommand Accept
        {
            get
            {
                return _accept ??= new RelayCommand(() =>
                {
                    try
                    {
                        RequestClose?.Invoke();
                    }
                    catch (Exception ex)
                    {
                        ExceptionUtils.HandleException(ex);
                    }
                });
            }
        }
        private RelayCommand _accept;

        public Action RequestClose;

        public Action Callback { get; private set; }

        public LicensesViewModel(Action callback = null)
        {
            Callback = callback;

            Licenses = new ObservableCollection<ObservableLicense>
            {
                GetChordiousLicense(),
                GetMvvmLightLicense()
            };
        }

        private ObservableLicense GetChordiousLicense()
        {
            return new ObservableLicense(AppInfo.Product, AppInfo.Copyright, AppInfo.MitLicenseName, AppInfo.MitLicenseBody);
        }

        private ObservableLicense GetMvvmLightLicense()
        {
            return new ObservableLicense("MVVM Toolkit", "Copyright © .NET Foundation and Contributors", AppInfo.MitLicenseName, AppInfo.MitLicenseBody);
        }

        public void ProcessClose()
        {
            Callback?.Invoke();
        }
    }
}
