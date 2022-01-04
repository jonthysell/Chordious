// Copyright (c) Jon Thysell <http://jonthysell.com>
// Licensed under the MIT License.

namespace Chordious.Core
{
    public class ScaleFinderStyle : FinderStyle
    {
        public ScaleFinderStyle(ConfigFile configFile, ChordiousSettings chordiousSettings = null, DiagramStyle diagramStyle = null) : base(configFile, "scale")
        {
            string level = "ScaleFinderStyle";

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
}
