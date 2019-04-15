// 
// ScaleFinderOptions.cs
//  
// Author:
//       Jon Thysell <thysell@gmail.com>
// 
// Copyright (c) 2015, 2016, 2017, 2019 Jon Thysell <http://jonthysell.com>
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
    public class ScaleFinderOptions : FinderOptions, IScaleFinderOptions
    {
        public IScale Scale
        {
            get
            {
                return GetScale();
            }
        }
        private IScale _cachedScale;

        public string ScaleLevel
        {
            get
            {
                return Settings[Prefix + "slevel"];
            }
            set
            {
                Settings[Prefix + "slevel"] = value;
            }
        }

        public ScaleFinderOptions(ConfigFile configFile, ChordiousSettings chordiousSettings = null) : base(configFile, "scale")
        {
            if (null != chordiousSettings)
            {
                Settings = chordiousSettings;
            }
            else
            {
                string settingsLevel = "ScaleFinderOptions";
                Settings = new ChordiousSettings(_configFile.ChordiousSettings, settingsLevel);
            }
            _cachedScale = null;
        }

        public IScale GetScale()
        {
            string longName = Settings[Prefix + "scale"];

            string level = ScaleLevel;

            if (null != _cachedScale)
            {
                if (_cachedScale.LongName == longName && _cachedScale.Level == level)
                {
                    return _cachedScale;
                }
            }

            ScaleSet scales = _configFile.Scales;
            while (null != scales)
            {
                if (scales.Level == level)
                {
                    if (scales.TryGet(longName, out Scale scale))
                    {
                        _cachedScale = scale;
                        break;
                    }
                }

                scales = scales.Parent;
            }

            return _cachedScale;
        }

        public void SetTarget(Note rootNote, IScale scale)
        {
            if (null == scale)
            {
                throw new ArgumentNullException(nameof(scale));
            }

            Settings[Prefix + "rootnote"] = NoteUtils.ToString(rootNote);
            Settings[Prefix + "scale"] = scale.LongName;
            ScaleLevel = scale.Level;

            _cachedScale = scale;
        }

        public ScaleFinderOptions Clone()
        {
            ScaleFinderOptions sfo = new ScaleFinderOptions(_configFile);
            sfo.Settings.CopyFrom(Settings);
            sfo.SetTarget(Instrument, Tuning);
            sfo.SetTarget(RootNote, Scale);
            return sfo;
        }
    }
}
