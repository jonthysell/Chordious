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
    public class ChordFinderStyle
    {
        public DiagramStyle Style { get; private set; }
        public ChordiousSettings Settings { get; private set; }

        public bool AddTitle
        {
            get
            {
                return Settings.GetBoolean("chordfinderstyle.addtitle");
            }
            set
            {
                Settings.Set("chordfinderstyle.addtitle", value);
            }
        }

        public BarreTypeOption BarreTypeOption
        {
            get
            {
                return Settings.GetEnum<BarreTypeOption>("chordfinderstyle.barretypeoption");
            }
            set
            {
                Settings.Set("chordfinderstyle.barretypeoption", value);
            }
        }

        public bool AddRootNotes
        {
            get
            {
                return Settings.GetBoolean("chordfinderstyle.addrootnotes");
            }
            set
            {
                Settings.Set("chordfinderstyle.addrootnotes", value);
            }
        }

        public bool AddBottomMarks
        {
            get
            {
                return Settings.GetBoolean("chordfinderstyle.addbottommarks");
            }
            set
            {
                Settings.Set("chordfinderstyle.addbottommarks", value);
            }
        }

        public MarkTextOption MarkTextOption
        {
            get
            {
                return Settings.GetEnum<MarkTextOption>("chordfinderstyle.marktextoption");               
            }
            set
            {
                Settings.Set("chordfinderstyle.marktextoption", value);
            }
        }

        public MarkTextOption BottomMarkTextOption
        {
            get
            {
                return Settings.GetEnum<MarkTextOption>("chordfinderstyle.bottommarktextoption");
            }
            set
            {
                Settings.Set("chordfinderstyle.bottommarktextoption", value);
            }
        }

        public bool MirrorResults
        {
            get
            {
                return this.Settings.GetBoolean("chordfinderstyle.mirrorresults");
            }
            set
            {
                this.Settings.Set("chordfinderstyle.mirrorresults", value);
            }
        }

        private ConfigFile _configFile;

        public ChordFinderStyle(ConfigFile configFile)
        {
            if (null == configFile)
            {
                throw new ArgumentNullException("configFile");
            }

            this._configFile = configFile;
            this.Style = new DiagramStyle(this._configFile.DiagramStyle, "ChordFinderStyle");
            this.Settings = new ChordiousSettings(this._configFile.ChordiousSettings, "ChordFinderStyle");
        }
    }

    public enum MarkTextOption
    {
        None,
        ShowNote_ShowBoth,
        ShowNote_PreferFlats,
        ShowNote_PreferSharps
    }

    public enum BarreTypeOption
    {
        None,
        Partial,
        Full
    }
}
