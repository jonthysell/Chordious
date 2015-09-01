// 
// ChordFinderOptions.cs
//  
// Author:
//       Jon Thysell <thysell@gmail.com>
// 
// Copyright (c) 2013, 2014, 2015 Jon Thysell <http://jonthysell.com>
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
    public class ChordFinderOptions : FinderOptions
    {
        public ChordQuality ChordQuality
        {
            get
            {
                return GetChordQuality();
            }
        }
        private ChordQuality _cachedChordQuality;

        public string ChordQualityLevel
        {
            get
            {
                return this.Settings[Prefix + "cqlevel"];
            }
            set
            {
                this.Settings[Prefix + "cqlevel"] = value;
            }
        }

        public bool AllowRootlessChords
        {
            get
            {
                return this.Settings.GetBoolean(Prefix + "allowrootlesschords");
            }
            set
            {
                this.Settings.Set(Prefix + "allowrootlesschords", value);
            }
        }

        public ChordFinderOptions(ConfigFile configFile, ChordiousSettings chordiousSettings = null) : base(configFile, "chord")
        {
            if (null != chordiousSettings)
            {
                this.Settings = chordiousSettings;
            }
            else
            {
                string settingsLevel = "ChordFinderOptions";
                this.Settings = new ChordiousSettings(this._configFile.ChordiousSettings, settingsLevel);
            }
            
            this._cachedChordQuality = null;
        }

        public ChordQuality GetChordQuality()
        {
            string name = this.Settings[Prefix + "chordquality"];

            string level = this.ChordQualityLevel;

            if (null != this._cachedChordQuality)
            {
                if (this._cachedChordQuality.Name == name && this._cachedChordQuality.Level == level)
                {
                    return this._cachedChordQuality;
                }
                this._cachedChordQuality = null;
            }

            ChordQualitySet qualities = this._configFile.ChordQualities;
            while (null != qualities)
            {
                if (qualities.Level == level)
                {
                    this._cachedChordQuality = qualities.Get(name);
                    return _cachedChordQuality;
                }
                qualities = qualities.Parent;
            }

            throw new LevelNotFoundException(level);
        }

        public void SetTarget(Note rootNote, ChordQuality chordQuality)
        {
            if (null == chordQuality)
            {
                throw new ArgumentNullException("chordQuality");
            }

            this.Settings[Prefix + "rootnote"] = NoteUtils.ToString(rootNote);
            this.Settings[Prefix + "chordquality"] = chordQuality.Name;
            this.ChordQualityLevel = chordQuality.Level;

            this._cachedChordQuality = chordQuality;
        }

        public ChordFinderOptions Clone()
        {
            ChordFinderOptions cfo = new ChordFinderOptions(this._configFile);
            cfo.Settings.CopyFrom(this.Settings);
            cfo.SetTarget(this.Instrument, this.Tuning);
            cfo.SetTarget(this.RootNote, this.ChordQuality);
            return cfo;
        }
    }
}