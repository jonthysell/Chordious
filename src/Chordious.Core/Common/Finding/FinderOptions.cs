// Copyright (c) Jon Thysell <http://jonthysell.com>
// Licensed under the MIT License.

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