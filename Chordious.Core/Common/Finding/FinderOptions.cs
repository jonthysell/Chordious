// 
// FinderOptions.cs
//  
// Author:
//       Jon Thysell <thysell@gmail.com>
// 
// Copyright (c) 2015, 2016, 2017 Jon Thysell <http://jonthysell.com>
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

using com.jonthysell.Chordious.Core.Resources;

namespace com.jonthysell.Chordious.Core
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

        public IInstrument GetInstrument()
        {
            string name = this.Settings[Prefix + "instrument"];

            string level = this.InstrumentTuningLevel;

            if (null != this._cachedInstrument)
            {
                if (this._cachedInstrument.Name == name && this._cachedInstrument.Level == level)
                {
                    return this._cachedInstrument;
                }
                this._cachedInstrument = null;
                this._cachedTuning = null;
            }

            InstrumentSet instruments = this._configFile.Instruments;
            while (null != instruments)
            {
                if (instruments.Level == level)
                {
                    Instrument i;
                    if (instruments.TryGet(name, out i))
                    {
                        this._cachedInstrument = i;
                        break;
                    }
                }

                instruments = instruments.Parent;
            }

            return this._cachedInstrument;
        }

        public ITuning GetTuning()
        {
            string longName = this.Settings[Prefix + "tuning"];

            if (null != this._cachedInstrument && null != this._cachedTuning)
            {
                string level = this.InstrumentTuningLevel;
                if (this._cachedInstrument.Level == level && this._cachedTuning.LongName == longName && this._cachedTuning.Level == level)
                {
                    return this._cachedTuning;
                }
                this._cachedTuning = null;
            }

            ITuning t;
            if (null != Instrument && Instrument.Tunings.TryGet(longName, out t))
            {
                this._cachedTuning = t;
            }

            return this._cachedTuning;
        }

        public void SetTarget(IInstrument instrument, ITuning tuning)
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
            this.Settings[Prefix + "tuning"] = tuning.LongName;
            this.InstrumentTuningLevel = instrument.Level;

            this._cachedInstrument = instrument;
            this._cachedTuning = tuning;
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
                return String.Format(Strings.InstrumentTuningMismatchExceptionMessage, Instrument.Name, Tuning.LongName);
            }
        }

        public InstrumentTuningMismatchException(IInstrument instrument, ITuning tuning) : base()
        {
            this.Instrument = instrument;
            this.Tuning = tuning;
        }
    }
}