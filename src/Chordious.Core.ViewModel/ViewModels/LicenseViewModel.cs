// Copyright (c) Jon Thysell <http://jonthysell.com>
// Licensed under the MIT License.

using System;
using System.Collections.ObjectModel;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

using Chordious.Core.ViewModel.Resources;

namespace Chordious.Core.ViewModel
{
    public class LicensesViewModel : ViewModelBase
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
            return new ObservableLicense("MVVM Light", "Copyright © 2009-2018 Laurent Bugnion", AppInfo.MitLicenseName, AppInfo.MitLicenseBody);
        }

        public void ProcessClose()
        {
            Callback?.Invoke();
        }
    }
}
