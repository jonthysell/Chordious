// Copyright (c) Jon Thysell <http://jonthysell.com>
// Licensed under the MIT License.

using System;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Chordious.Core.ViewModel
{
    public abstract class ObservableHeaderObject : ObservableObject
    {
        protected string HeaderName { get; private set; } = null;

        public bool IsHeader
        {
            get
            {
                return !string.IsNullOrWhiteSpace(HeaderName);
            }
        }

        public ObservableHeaderObject() : base() { }

        public ObservableHeaderObject(string headerName)
        {
            if (string.IsNullOrWhiteSpace(headerName))
            {
                throw new ArgumentNullException(nameof(headerName));
            }

            HeaderName = headerName.Trim();
        }

        public override string ToString()
        {
            return HeaderName;
        }
    }
}
