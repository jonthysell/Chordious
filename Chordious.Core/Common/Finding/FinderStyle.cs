// 
// FinderStyle.cs
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
    public abstract class FinderStyle
    {
        public DiagramStyle Style { get; private set; }
        public ChordiousSettings Settings { get; private set; }

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

        public bool MirrorResults
        {
            get
            {
                return this.Settings.GetBoolean(Prefix + "mirrorresults");
            }
            set
            {
                this.Settings.Set(Prefix + "mirrorresults", value);
            }
        }

        private ConfigFile _configFile;

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
                    throw new ArgumentNullException();
                }

                _prefix = value.Trim() + "finderstyle.";
            }
        }
        private string _prefix;

        protected FinderStyle(ConfigFile configFile, string prefix)
        {
            if (null == configFile)
            {
                throw new ArgumentNullException("configFile");
            }

            this._configFile = configFile;
            this.Prefix = prefix;

            string level = Prefix.Substring(0, 1).ToUpper() + Prefix.Substring(1) + "FinderStyle";

            this.Style = new DiagramStyle(this._configFile.DiagramStyle, level);
            this.Settings = new ChordiousSettings(this._configFile.ChordiousSettings, level);
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
