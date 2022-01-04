// Copyright (c) Jon Thysell <http://jonthysell.com>
// Licensed under the MIT License.

namespace Chordious.Core
{
    public class ChordFinderStyle : FinderStyle
    {
        public BarreTypeOption BarreTypeOption
        {
            get
            {
                return Settings.GetEnum<BarreTypeOption>(Prefix + "barretypeoption");
            }
            set
            {
                Settings.Set(Prefix + "barretypeoption", value);
            }
        }

        public bool AddBottomMarks
        {
            get
            {
                return Settings.GetBoolean(Prefix + "addbottommarks");
            }
            set
            {
                Settings.Set(Prefix + "addbottommarks", value);
            }
        }

        public MarkTextOption BottomMarkTextOption
        {
            get
            {
                return Settings.GetEnum<MarkTextOption>(Prefix + "bottommarktextoption");
            }
            set
            {
                Settings.Set(Prefix + "bottommarktextoption", value);
            }
        }

        public ChordFinderStyle(ConfigFile configFile, ChordiousSettings chordiousSettings = null, DiagramStyle diagramStyle = null) : base(configFile, "chord")
        {
            string level = "ChordFinderStyle";

            if (null != chordiousSettings)
            {
                Settings = chordiousSettings;
            }
            else
            {
                Settings = new ChordiousSettings(_configFile.ChordiousSettings, level);
            }

            if (null != diagramStyle)
            {
                Style = diagramStyle;
            }
            else
            {
                Style = new DiagramStyle(_configFile.DiagramStyle, level);
            }
        }
    }

    public enum BarreTypeOption
    {
        None,
        Partial,
        Full
    }
}
