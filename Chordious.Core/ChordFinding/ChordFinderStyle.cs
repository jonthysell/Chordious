// 
// ChordFinderStyle.cs
//  
// Author:
//       Jon Thysell <thysell@gmail.com>
// 
// Copyright (c) 2015 Jon Thysell <http://jonthysell.com>
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System;

namespace com.jonthysell.Chordious.Core
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
                this.Settings = chordiousSettings;
            }
            else
            {
                this.Settings = new ChordiousSettings(this._configFile.ChordiousSettings, level);
            }

            if (null != diagramStyle)
            {
                this.Style = diagramStyle;
            }
            else
            {
                this.Style = new DiagramStyle(this._configFile.DiagramStyle, level);
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
