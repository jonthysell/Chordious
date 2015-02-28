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
    public class ChordFinderOptions
    {
        public ChordiousSettings Settings { get; private set; }

        public Instrument Instrument
        {
            get
            {
                return GetInstrument();
            }
        }
        private Instrument _cachedInstrument;

        public Tuning Tuning
        {
            get
            {
                return GetTuning();
            }
        }
        private Tuning _cachedTuning;

        public string InstrumentTuningLevel
        {
            get
            {
                return this.Settings["chordfinderoptions.itlevel"];
            }
            set
            {
                this.Settings["chordfinderoptions.itlevel"] = value;
            }
        }

        public Note RootNote
        {
            get
            {
                return this.Settings.GetNote("chordfinderoptions.rootnote");
            }
            set
            {
                this.Settings.Set("chordfinderoptions.rootnote", value);
            }
        }

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
                return this.Settings["chordfinderoptions.cqlevel"];
            }
            set
            {
                this.Settings["chordfinderoptions.cqlevel"] = value;
            }
        }

        public int NumFrets
        {
            get
            {
                return this.Settings.GetInt32("chordfinderoptions.numfrets");
            }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException();
                }
                this.Settings.Set("chordfinderoptions.numfrets", value);
            }
        }

        public int MaxFret
        {
            get
            {
                return this.Settings.GetInt32("chordfinderoptions.maxfret");
            }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException();
                }
                this.Settings.Set("chordfinderoptions.maxfret", value);
            }
        }

        public int MaxReach
        {
            get
            {
                return this.Settings.GetInt32("chordfinderoptions.maxreach");
            }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException();
                }
                this.Settings.Set("chordfinderoptions.maxreach", value);
            }
        }

        public bool AllowOpenStrings
        {
            get
            {
                return this.Settings.GetBoolean("chordfinderoptions.allowopenstrings");
            }
            set
            {
                this.Settings.Set("chordfinderoptions.allowopenstrings", value);
            }
        }

        public bool AllowMutedStrings
        {
            get
            {
                return this.Settings.GetBoolean("chordfinderoptions.allowmutedstrings");
            }
            set
            {
                this.Settings.Set("chordfinderoptions.allowmutedstrings", value);
            }
        }

        public bool AllowRootlessChords
        {
            get
            {
                return this.Settings.GetBoolean("chordfinderoptions.allowrootlesschords");
            }
            set
            {
                this.Settings.Set("chordfinderoptions.allowrootlesschords", value);
            }
        }
        
        private ConfigFile _configFile;

        public ChordFinderOptions(ConfigFile configFile)
        {
            if (null == configFile)
            {
                throw new ArgumentNullException("configFile");
            }

            this._configFile = configFile;
            this.Settings = new ChordiousSettings(this._configFile.ChordiousSettings, "ChordFinderOptions");
            this._cachedInstrument = null;
            this._cachedTuning = null;
            this._cachedChordQuality = null;
        }

        public Instrument GetInstrument()
        {
            string name = this.Settings["chordfinderoptions.instrument"];

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
            string name = this.Settings["chordfinderoptions.tuning"];

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

        public ChordQuality GetChordQuality()
        {
            string name = this.Settings["chordfinderoptions.chordquality"];

            string level = this.ChordQualityLevel;

            if (null != this._cachedChordQuality)
            {
                if (this._cachedChordQuality.Name == name && this._cachedInstrument.Level == level)
                {
                    return this._cachedChordQuality;
                }
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

            this.Settings["chordfinderoptions.instrument"] = instrument.Name;
            this.Settings["chordfinderoptions.tuning"] = tuning.Name;
            this.InstrumentTuningLevel = instrument.Level;

            this._cachedInstrument = instrument;
            this._cachedTuning = tuning;
        }

        public void SetTarget(Note rootNote, ChordQuality chordQuality)
        {
            if (null == chordQuality)
            {
                throw new ArgumentNullException("chordQuality");
            }

            this.Settings["chordfinderoptions.rootnote"] = NoteUtils.ToString(rootNote);
            this.Settings["chordfinderoptions.chordquality"] = chordQuality.Name;
            this.ChordQualityLevel = chordQuality.Level;

            this._cachedChordQuality = chordQuality;
        }

        public ChordFinderOptions Clone()
        {
            ChordFinderOptions cfo = new ChordFinderOptions(this._configFile);
            cfo.Settings.CopyFrom(this.Settings);
            return cfo;
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