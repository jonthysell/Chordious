// Copyright (c) Jon Thysell <http://jonthysell.com>
// Licensed under the MIT License.

using System;

namespace Chordious.Core.ViewModel
{
    public class ObservableNamedInterval : ObservableHeaderObject
    {
        public string Name
        {
            get
            {
                if (IsHeader)
                {
                    return HeaderName;
                }
                return NamedInterval.Name;
            }
        }

        public string LongName
        {
            get
            {
                if (IsHeader)
                {
                    return HeaderName;
                }
                return NamedInterval.LongName;
            }
        }

        public int[] Intervals
        {
            get
            {
                return NamedInterval.Intervals;
            }
        }

        public string Level
        {
            get
            {
                return NamedInterval.Level;
            }
        }

        public bool ReadOnly
        {
            get
            {
                return NamedInterval.ReadOnly;
            }
        }

        internal NamedInterval NamedInterval { get; private set; }

        public ObservableNamedInterval(NamedInterval namedInterval) : base()
        {
            NamedInterval = namedInterval ?? throw new ArgumentNullException(nameof(namedInterval));
        }

        public ObservableNamedInterval(string headerName) : base(headerName) { }

        public override string ToString()
        {
            if (NamedInterval is not null)
            {
                return NamedInterval.ToString();
            }
            return base.ToString();
        }
    }
}
