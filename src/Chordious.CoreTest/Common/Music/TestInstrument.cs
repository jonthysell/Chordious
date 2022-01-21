// Copyright (c) Jon Thysell <http://jonthysell.com>
// Licensed under the MIT License.

using System;

using Chordious.Core;

namespace Chordious.CoreTest
{
    public class TestInstrument : IInstrument
    {
        public static TestInstrument Ukulele
        {
            get
            {
                if (null == _ukulele)
                {
                    _ukulele = new TestInstrument(UkuleleInstrumentName, 4);
                    _ukulele.Tunings.Add(UkuleleStandardTuningName,
                        new FullNote[]
                        {
                            new FullNote(Note.G, 4),
                            new FullNote(Note.C, 4),
                            new FullNote(Note.E, 4),
                            new FullNote(Note.A, 4),
                        });
                }
                return _ukulele;
            }
        }
        private static TestInstrument _ukulele = null;

        public bool ReadOnly
        {
            get
            {
                return false;
            }
        }

        public InstrumentSet Parent
        {
            get
            {
                return null;
            }
        }

        public string Level
        {
            get
            {
                return "Test";
            }
        }

        public string Name { get; private set; }
        public int NumStrings { get; private set; }

        public ITuningSet Tunings { get; private set; }

        public TestInstrument(string name, int numStrings)
        {
            Name = name;
            NumStrings = numStrings;

            Tunings = new TestTuningSet(this);
        }

        public void MarkAsReadOnly()
        {
            throw new NotImplementedException();
        }

        public int CompareTo(object obj)
        {
            throw new NotImplementedException();
        }

        public const string UkuleleInstrumentName = "Ukulele";
        public const string UkuleleStandardTuningName = "UkuleleStandard";
    }
}
