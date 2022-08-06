// Copyright (c) Jon Thysell <http://jonthysell.com>
// Licensed under the MIT License.

namespace Chordious.Core
{
    public class ScaleFinderStyle : FinderStyle
    {
        public ScaleFinderStyle(ConfigFile configFile, ChordiousSettings chordiousSettings = null, DiagramStyle diagramStyle = null) : base(configFile, "scale")
        {
            string level = "ScaleFinderStyle";

            if (chordiousSettings is not null)
            {
                Settings = chordiousSettings;
            }
            else
            {
                Settings = new ChordiousSettings(_configFile.ChordiousSettings, level);
            }

            if (diagramStyle is not null)
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
