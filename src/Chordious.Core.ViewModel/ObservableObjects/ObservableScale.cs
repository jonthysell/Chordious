// Copyright (c) Jon Thysell <http://jonthysell.com>
// Licensed under the MIT License.

namespace Chordious.Core.ViewModel
{
    public class ObservableScale : ObservableNamedInterval
    {
        internal Scale Scale
        {
            get
            {
                return NamedInterval as Scale;
            }
        }

        public ObservableScale(Scale scale) : base(scale) { }

        public ObservableScale(string headerName) : base(headerName) { }
    }
}
