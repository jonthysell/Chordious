// Copyright (c) Jon Thysell <http://jonthysell.com>
// Licensed under the MIT License.

using System;

using GalaSoft.MvvmLight;

namespace Chordious.Core.ViewModel
{
    public class ObservableLicense : ObservableObject
    {
        public string Header
        {
            get
            {
                return ProductName;
            }
        }

        public string Body
        {
            get
            {
                return string.Join(Environment.NewLine + Environment.NewLine, LicenseName, Copyright, LicenseBody);
            }
        }

        public string ProductName { get; private set;}

        public string Copyright { get; private set; }

        public string LicenseName { get; private set; }

        public string LicenseBody { get; private set; }

        public ObservableLicense(string productName, string copyright, string licenseName, string licenseBody)
        {
            ProductName = string.IsNullOrWhiteSpace(productName) ? throw new ArgumentNullException(nameof(productName)) : productName.Trim();
            Copyright = string.IsNullOrWhiteSpace(copyright) ? throw new ArgumentNullException(nameof(copyright)) : copyright.Trim();
            LicenseName = string.IsNullOrWhiteSpace(licenseName) ? throw new ArgumentNullException(nameof(licenseName)) : licenseName.Trim();
            LicenseBody = string.IsNullOrWhiteSpace(licenseBody) ? throw new ArgumentNullException(nameof(licenseBody)) : licenseBody.Trim();
        }
    }
}
