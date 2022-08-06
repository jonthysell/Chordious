// Copyright (c) Jon Thysell <http://jonthysell.com>
// Licensed under the MIT License.

using System;
using System.Collections.Generic;

namespace Chordious.Core
{
    public class ReverseChordFinderOptions : FinderOptions, IReverseChordFinderOptions
    {
        public int[] Marks
        {
            get
            {
                return GetMarks();
            }
        }

        private int[] _cachedMarks;

        public IEnumerable<IChordQuality> ChordQualities { get; set; }

        public ReverseChordFinderOptions(ConfigFile configFile, ChordiousSettings chordiousSettings = null) : base(configFile, "reversechord")
        {
            if (chordiousSettings is not null)
            {
                Settings = chordiousSettings;
            }
            else
            {
                string settingsLevel = "ReverseChordFinderOptions";
                Settings = new ChordiousSettings(_configFile.ChordiousSettings, settingsLevel);
            }
            _cachedMarks = null;
        }

        public int[] GetMarks()
        {
            int[] marks = new int[Instrument.NumStrings];

            for (int i = 0; i < marks.Length; i++)
            {
                marks[i] = (_cachedMarks is not null && i < _cachedMarks.Length) ? _cachedMarks[i] : -1;
            }

            _cachedMarks = marks;

            return _cachedMarks;
        }

        public void SetTarget(int[] marks)
        {
            _cachedMarks = marks ?? throw new ArgumentNullException(nameof(marks));
        }

        public ReverseChordFinderOptions Clone()
        {
            ReverseChordFinderOptions rcfo = new ReverseChordFinderOptions(_configFile);
            rcfo.Settings.CopyFrom(Settings);
            rcfo.SetTarget(Instrument, Tuning);
            return rcfo;
        }
    }
}
