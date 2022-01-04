// Copyright (c) Jon Thysell <http://jonthysell.com>
// Licensed under the MIT License.

using System;

namespace Chordious.Core
{
    public class ScaleFinderOptions : FinderOptions2, IScaleFinderOptions
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

        public bool StrictIntervals
        {
            get
            {
                return Settings.GetBoolean(Prefix + "strictintervals");
            }
            set
            {
                Settings.Set(Prefix + "strictintervals", value);
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
