// 
// ScaleFinderOptions.cs
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
    public class ScaleFinderOptions : FinderOptions
    {
        public Scale Scale
        {
            get
            {
                return GetScale();
            }
        }
        private Scale _cachedScale;

        public string ScaleLevel
        {
            get
            {
                return this.Settings[Prefix + "slevel"];
            }
            set
            {
                this.Settings[Prefix + "slevel"] = value;
            }
        }

        public ScaleFinderOptions(ConfigFile configFile, ChordiousSettings chordiousSettings = null) : base(configFile, "scale")
        {
            if (null != chordiousSettings)
            {
                this.Settings = chordiousSettings;
            }
            else
            {
                string settingsLevel = "ScaleFinderOptions";
                this.Settings = new ChordiousSettings(this._configFile.ChordiousSettings, settingsLevel);
            }
            this._cachedScale = null;
        }

        public Scale GetScale()
        {
            string name = this.Settings[Prefix + "scale"];

            string level = this.ScaleLevel;

            if (null != this._cachedScale)
            {
                if (this._cachedScale.Name == name && this._cachedInstrument.Level == level)
                {
                    return this._cachedScale;
                }
            }

            ScaleSet scales = this._configFile.Scales;
            while (null != scales)
            {
                if (scales.Level == level)
                {
                    this._cachedScale = scales.Get(name);
                    return _cachedScale;
                }
                scales = scales.Parent;
            }

            throw new LevelNotFoundException(level);
        }

        public void SetTarget(Note rootNote, Scale scale)
        {
            if (null == scale)
            {
                throw new ArgumentNullException("scale");
            }

            this.Settings[Prefix + "rootnote"] = NoteUtils.ToString(rootNote);
            this.Settings[Prefix + "scale"] = scale.Name;
            this.ScaleLevel = scale.Level;

            this._cachedScale = scale;
        }

        public ScaleFinderOptions Clone()
        {
            ScaleFinderOptions sfo = new ScaleFinderOptions(this._configFile);
            sfo.Settings.CopyFrom(this.Settings);
            return sfo;
        }
    }
}
