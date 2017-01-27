// 
// ChordFinderOptions.cs
//  
// Author:
//       Jon Thysell <thysell@gmail.com>
// 
// Copyright (c) 2013, 2014, 2015, 2016, 2017 Jon Thysell <http://jonthysell.com>
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
    public class ChordFinderOptions : FinderOptions, IChordFinderOptions
    {
        public IChordQuality ChordQuality
        {
            get
            {
                return GetChordQuality();
            }
        }
        private IChordQuality _cachedChordQuality;

        public string ChordQualityLevel
        {
            get
            {
                return Settings[Prefix + "cqlevel"];
            }
            set
            {
                Settings[Prefix + "cqlevel"] = value;
            }
        }

        public bool AllowRootlessChords
        {
            get
            {
                return Settings.GetBoolean(Prefix + "allowrootlesschords");
            }
            set
            {
                Settings.Set(Prefix + "allowrootlesschords", value);
            }
        }

        public ChordFinderOptions(ConfigFile configFile, ChordiousSettings chordiousSettings = null) : base(configFile, "chord")
        {
            if (null != chordiousSettings)
            {
                Settings = chordiousSettings;
            }
            else
            {
                string settingsLevel = "ChordFinderOptions";
                Settings = new ChordiousSettings(_configFile.ChordiousSettings, settingsLevel);
            }

            _cachedChordQuality = null;
        }

        public IChordQuality GetChordQuality()
        {
            string longName = Settings[Prefix + "chordquality"];

            string level = ChordQualityLevel;

            if (null != _cachedChordQuality)
            {
                if (_cachedChordQuality.LongName == longName && _cachedChordQuality.Level == level)
                {
                    return _cachedChordQuality;
                }
                _cachedChordQuality = null;
            }

            ChordQualitySet qualities = _configFile.ChordQualities;
            while (null != qualities)
            {
                if (qualities.Level == level)
                {
                    ChordQuality cq;
                    if (qualities.TryGet(longName, out cq))
                    {
                        _cachedChordQuality = cq;
                        break;
                    }
                }

                qualities = qualities.Parent;
            }

            return _cachedChordQuality;
        }

        public void SetTarget(Note rootNote, IChordQuality chordQuality)
        {
            if (null == chordQuality)
            {
                throw new ArgumentNullException("chordQuality");
            }

            Settings[Prefix + "rootnote"] = NoteUtils.ToString(rootNote);
            Settings[Prefix + "chordquality"] = chordQuality.LongName;
            ChordQualityLevel = chordQuality.Level;

            _cachedChordQuality = chordQuality;
        }

        public ChordFinderOptions Clone()
        {
            ChordFinderOptions cfo = new ChordFinderOptions(_configFile);
            cfo.Settings.CopyFrom(Settings);
            cfo.SetTarget(Instrument, Tuning);
            cfo.SetTarget(RootNote, ChordQuality);
            return cfo;
        }
    }
}