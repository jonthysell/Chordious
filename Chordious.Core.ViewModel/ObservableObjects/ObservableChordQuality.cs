// Copyright (c) Jon Thysell <http://jonthysell.com>
// Licensed under the MIT License.

namespace Chordious.Core.ViewModel
{
    public class ObservableChordQuality : ObservableNamedInterval
    {
        public string Abbreviation
        {
            get
            {
                return ChordQuality.Abbreviation;
            }
        }

        internal ChordQuality ChordQuality
        {
            get
            {
                return NamedInterval as ChordQuality;
            }
        }

        public ObservableChordQuality(ChordQuality chordQuality) : base(chordQuality) { }

        public ObservableChordQuality(string headerName) : base(headerName) { }
    }
}
