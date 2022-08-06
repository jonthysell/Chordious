// Copyright (c) Jon Thysell <http://jonthysell.com>
// Licensed under the MIT License.

using System;
using System.Collections.ObjectModel;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Chordious.Core.ViewModel
{
    public class ObservableTuning : ObservableObject
    {
        public string Name
        {
            get
            {
                return Tuning.Name;
            }
        }

        public string LongName
        {
            get
            {
                return Tuning.LongName;
            }
        }

        public string Level
        {
            get
            {
                return Tuning.Level;
            }
        }

        public bool ReadOnly
        {
            get
            {
                return Tuning.ReadOnly;
            }
        }

        public ObservableCollection<ObservableNote> Notes
        {
            get
            {
                return GetNotes();
            }
        }

        internal Tuning Tuning { get; private set; }

        public ObservableTuning(Tuning tuning)
        {
            Tuning = tuning ?? throw new ArgumentNullException(nameof(tuning));
        }

        private ObservableCollection<ObservableNote> GetNotes()
        {
            ObservableCollection<ObservableNote> collection = new ObservableCollection<ObservableNote>();

            foreach (FullNote note in Tuning.RootNotes)
            {
                collection.Add(new ObservableNote(note));
            }

            return collection;
        }

        public override string ToString()
        {
            if (Tuning is not null)
            {
                return Tuning.ToString();
            }
            return base.ToString();
        }
    }
}
