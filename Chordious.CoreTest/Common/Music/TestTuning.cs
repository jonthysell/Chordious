// Copyright (c) Jon Thysell <http://jonthysell.com>
// Licensed under the MIT License.

using System;

using Chordious.Core;

namespace Chordious.CoreTest
{
    public class TestTuning : ITuning
    {
        public bool ReadOnly
        {
            get
            {
                return false;
            }
        }

        public ITuningSet Parent { get; private set; }

        public string Level
        {
            get
            {
                return "Test";
            }
        }

        public string Name { get; private set; }
        public string LongName
        {
            get
            {
                return Name;
            }
        }

        public FullNote[] RootNotes { get; private set; }

        public TestTuning(TestTuningSet parent, string name, FullNote[] rootNotes)
        {
            Parent = parent;
            Name = name;
            RootNotes = rootNotes;
        }

        public void MarkAsReadOnly()
        {
            throw new NotImplementedException();
        }

        public int CompareTo(object obj)
        {
            throw new NotImplementedException();
        }
    }
}
