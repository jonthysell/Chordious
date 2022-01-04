// Copyright (c) Jon Thysell <http://jonthysell.com>
// Licensed under the MIT License.

using System;

namespace Chordious.Core
{
    public class ChordFinderOptions : FinderOptions2, IChordFinderOptions
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

        public bool AllowPartialChords
        {
            get
            {
                return Settings.GetBoolean(Prefix + "allowpartialchords");
            }
            set
            {
                Settings.Set(Prefix + "allowpartialchords", value);
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
                    if (qualities.TryGet(longName, out ChordQuality cq))
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
                throw new ArgumentNullException(nameof(chordQuality));
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
