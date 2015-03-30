// 
// FinderOptions.cs
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
    public abstract class FinderOptions
    {
        public ChordiousSettings Settings { get; protected set; }

        public Instrument Instrument
        {
            get
            {
                return GetInstrument();
            }
        }
        protected Instrument _cachedInstrument;

        public Tuning Tuning
        {
            get
            {
                return GetTuning();
            }
        }
        protected Tuning _cachedTuning;

        public string InstrumentTuningLevel
        {
            get
            {
                return this.Settings[Prefix + "itlevel"];
            }
            set
            {
                this.Settings[Prefix + "itlevel"] = value;
            }
        }

        public Note RootNote
        {
            get
            {
                return this.Settings.GetNote(Prefix + "rootnote");
            }
            set
            {
                this.Settings.Set(Prefix + "rootnote", value);
            }
        }

        public int NumFrets
        {
            get
            {
                return this.Settings.GetInt32(Prefix + "numfrets");
            }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException();
                }
                this.Settings.Set(Prefix + "numfrets", value);
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
                return this.Settings.GetInt32(Prefix + "maxfret");
            }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException();
                }
                this.Settings.Set(Prefix + "maxfret", value);
            }
        }

        public int MaxReach
        {
            get
            {
                return this.Settings.GetInt32(Prefix + "maxreach");
            }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException();
                }
                this.Settings.Set(Prefix + "maxreach", value);
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
                return this.Settings.GetBoolean(Prefix + "allowopenstrings");
            }
            set
            {
                this.Settings.Set(Prefix + "allowopenstrings", value);
            }
        }

        public bool AllowMutedStrings
        {
            get
            {
                return this.Settings.GetBoolean(Prefix + "allowmutedstrings");
            }
            set
            {
                this.Settings.Set(Prefix + "allowmutedstrings", value);
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
            if (null == configFile)
            {
                throw new ArgumentNullException("configFile");
            }

            if (StringUtils.IsNullOrWhiteSpace(prefix))
            {
                throw new ArgumentNullException("prefix");
            }

            this._configFile = configFile;
            this.Prefix = prefix;

            this._cachedInstrument = null;
            this._cachedTuning = null;
        }

        public Instrument GetInstrument()
        {
            string name = this.Settings[Prefix + "instrument"];

            string level = this.InstrumentTuningLevel;

            if (null != this._cachedInstrument)
            {
                if (this._cachedInstrument.Name == name && this._cachedInstrument.Level == level)
                {
                    return this._cachedInstrument;
                }
            }

            InstrumentSet instruments = this._configFile.Instruments;
            while (null != instruments)
            {
                if (instruments.Level == level)
                {
                    this._cachedInstrument = instruments.Get(name);
                    return _cachedInstrument;
                }
                instruments = instruments.Parent;
            }

            throw new LevelNotFoundException(level);
        }

        public Tuning GetTuning()
        {
            string name = this.Settings[Prefix + "tuning"];

            if (null != this._cachedInstrument && null != this._cachedTuning)
            {
                string level = this.InstrumentTuningLevel;
                if (this._cachedInstrument.Level == level && this._cachedTuning.Name == name && this._cachedTuning.Level == level)
                {
                    return this._cachedTuning;
                }
            }

            this._cachedTuning = Instrument.Tunings.Get(name);
            return this._cachedTuning;
        }

        public void SetTarget(Instrument instrument, Tuning tuning)
        {
            if (null == instrument)
            {
                throw new ArgumentNullException("instrument");
            }

            if (null == tuning)
            {
                throw new ArgumentNullException("tuning");
            }

            if (tuning.Parent != instrument.Tunings)
            {
                throw new InstrumentTuningMismatchException(instrument, tuning);
            }

            this.Settings[Prefix + "instrument"] = instrument.Name;
            this.Settings[Prefix + "tuning"] = tuning.Name;
            this.InstrumentTuningLevel = instrument.Level;

            this._cachedInstrument = instrument;
            this._cachedTuning = tuning;
        }
    }

    public class InstrumentTuningMismatchException : ChordiousException
    {
        public Instrument Instrument { get; private set; }
        public Tuning Tuning { get; private set; }

        public InstrumentTuningMismatchException(Instrument instrument, Tuning tuning) : base()
        {
            this.Instrument = instrument;
            this.Tuning = tuning;
        }
    }
}