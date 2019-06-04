// 
// FinderOptions.cs
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

using Chordious.Core.Resources;

namespace Chordious.Core
{
    public abstract class FinderOptions : IFinderOptions
    {
        public ChordiousSettings Settings { get; protected set; }

        public IInstrument Instrument
        {
            get
            {
                return GetInstrument();
            }
        }
        protected IInstrument _cachedInstrument;

        public ITuning Tuning
        {
            get
            {
                return GetTuning();
            }
        }
        protected ITuning _cachedTuning;

        public string InstrumentTuningLevel
        {
            get
            {
                return Settings[Prefix + "itlevel"];
            }
            set
            {
                Settings[Prefix + "itlevel"] = value;
            }
        }

        public Note RootNote
        {
            get
            {
                return Settings.GetNote(Prefix + "rootnote");
            }
            set
            {
                Settings.Set(Prefix + "rootnote", value);
            }
        }

        public int NumFrets
        {
            get
            {
                return Settings.GetInt32(Prefix + "numfrets");
            }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException();
                }

                Settings.Set(Prefix + "numfrets", value);

                if (NumFrets < MaxReach)
                {
                    MaxReach = NumFrets;
                }
            }
        }

        public int MaxFret
        {
            get
            {
                return Settings.GetInt32(Prefix + "maxfret");
            }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException();
                }

                Settings.Set(Prefix + "maxfret", value);
            }
        }

        public int MaxReach
        {
            get
            {
                return Settings.GetInt32(Prefix + "maxreach");
            }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException();
                }

                Settings.Set(Prefix + "maxreach", value);

                if (MaxReach > NumFrets)
                {
                    NumFrets = MaxReach;
                }
            }
        }

        public bool AllowOpenStrings
        {
            get
            {
                return Settings.GetBoolean(Prefix + "allowopenstrings");
            }
            set
            {
                Settings.Set(Prefix + "allowopenstrings", value);
            }
        }

        public bool AllowMutedStrings
        {
            get
            {
                return Settings.GetBoolean(Prefix + "allowmutedstrings");
            }
            set
            {
                Settings.Set(Prefix + "allowmutedstrings", value);
            }
        }

        public bool UsingDefaultTarget
        {
            get
            {
                return Settings.MatchesParentValue(Prefix + "instrument")
                    && Settings.MatchesParentValue(Prefix + "tuning")
                    && Settings.MatchesParentValue(Prefix + "itlevel");
            }
        }

        protected ConfigFile _configFile;

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

                _prefix = value.Trim() + "finderoptions.";
            }
        }
        private string _prefix;

        protected FinderOptions(ConfigFile configFile, string prefix)
        {
            if (StringUtils.IsNullOrWhiteSpace(prefix))
            {
                throw new ArgumentNullException(nameof(prefix));
            }

            _configFile = configFile ?? throw new ArgumentNullException(nameof(configFile));
            Prefix = prefix;

            _cachedInstrument = null;
            _cachedTuning = null;
        }

        public IInstrument GetInstrument()
        {
            string name = Settings[Prefix + "instrument"];

            string level = InstrumentTuningLevel;

            if (null != _cachedInstrument)
            {
                if (_cachedInstrument.Name == name && _cachedInstrument.Level == level)
                {
                    return _cachedInstrument;
                }
                _cachedInstrument = null;
                _cachedTuning = null;
            }

            InstrumentSet instruments = _configFile.Instruments;
            while (null != instruments)
            {
                if (instruments.Level == level)
                {
                    if (instruments.TryGet(name, out Instrument i))
                    {
                        _cachedInstrument = i;
                        break;
                    }
                }

                instruments = instruments.Parent;
            }

            return _cachedInstrument;
        }

        public ITuning GetTuning()
        {
            string longName = Settings[Prefix + "tuning"];

            if (null != _cachedInstrument && null != _cachedTuning)
            {
                string level = InstrumentTuningLevel;
                if (_cachedInstrument.Level == level && _cachedTuning.LongName == longName && _cachedTuning.Level == level)
                {
                    return _cachedTuning;
                }
                _cachedTuning = null;
            }

            if (null != Instrument && Instrument.Tunings.TryGet(longName, out ITuning t))
            {
                _cachedTuning = t;
            }

            return _cachedTuning;
        }

        public void SetTarget(IInstrument instrument, ITuning tuning)
        {
            if (null == instrument)
            {
                throw new ArgumentNullException(nameof(instrument));
            }

            if (null == tuning)
            {
                throw new ArgumentNullException(nameof(tuning));
            }

            if (tuning.Parent != instrument.Tunings)
            {
                throw new InstrumentTuningMismatchException(instrument, tuning);
            }

            Settings[Prefix + "instrument"] = instrument.Name;
            Settings[Prefix + "tuning"] = tuning.LongName;
            InstrumentTuningLevel = instrument.Level;

            _cachedInstrument = instrument;
            _cachedTuning = tuning;
        }

        public void SaveTargetAsDefault()
        {
            Settings.SetParent(Prefix + "instrument");
            Settings.SetParent(Prefix + "tuning");
            Settings.SetParent(Prefix + "itlevel");
        }
    }

    public class InstrumentTuningMismatchException : ChordiousException
    {
        public IInstrument Instrument { get; private set; }
        public ITuning Tuning { get; private set; }

        public override string Message
        {
            get
            {
                return string.Format(Strings.InstrumentTuningMismatchExceptionMessage, Instrument.Name, Tuning.LongName);
            }
        }

        public InstrumentTuningMismatchException(IInstrument instrument, ITuning tuning) : base()
        {
            Instrument = instrument;
            Tuning = tuning;
        }
    }
}