// Copyright (c) Jon Thysell <http://jonthysell.com>
// Licensed under the MIT License.

using System;

namespace Chordious.Core
{
    public abstract class FinderStyle
    {
        public DiagramStyle Style { get; protected set; }
        public ChordiousSettings Settings { get; protected set; }

        public bool AddTitle
        {
            get
            {
                return Settings.GetBoolean(Prefix + "addtitle");
            }
            set
            {
                Settings.Set(Prefix + "addtitle", value);
            }
        }

        public bool AddRootNotes
        {
            get
            {
                return Settings.GetBoolean(Prefix + "addrootnotes");
            }
            set
            {
                Settings.Set(Prefix + "addrootnotes", value);
            }
        }

        public MarkTextOption MarkTextOption
        {
            get
            {
                return Settings.GetEnum<MarkTextOption>(Prefix + "marktextoption");
            }
            set
            {
                Settings.Set(Prefix + "marktextoption", value);
            }
        }

        public FretLabelSide FretLabelSide
        {
            get
            {
                return Settings.GetEnum<FretLabelSide>(Prefix + "fretlabelside");
            }
            set
            {
                Settings.Set(Prefix + "fretlabelside", value);
            }
        }

        public bool MirrorResults
        {
            get
            {
                return Settings.GetBoolean(Prefix + "mirrorresults");
            }
            set
            {
                Settings.Set(Prefix + "mirrorresults", value);
            }
        }

        protected ConfigFile _configFile;

        public string Prefix
        {
            get
            {
                return _prefix;
            }
            private set
            {
                if (StringUtils.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentNullException(nameof(value));
                }

                _prefix = value.Trim() + "finderstyle.";
            }
        }
        private string _prefix;

        protected FinderStyle(ConfigFile configFile, string prefix)
        {
            if (StringUtils.IsNullOrWhiteSpace(prefix))
            {
                throw new ArgumentNullException(nameof(prefix));
            }

            _configFile = configFile ?? throw new ArgumentNullException(nameof(configFile));
            Prefix = prefix;
        }
    }

    public enum MarkTextOption
    {
        None,
        ShowNote_ShowBoth,
        ShowNote_PreferFlats,
        ShowNote_PreferSharps
    }
}
