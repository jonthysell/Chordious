// Copyright (c) Jon Thysell <http://jonthysell.com>
// Licensed under the MIT License.

using System;

namespace Chordious.Core
{
    public abstract class FinderOptions2 : FinderOptions, IFinderOptions2
    {
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
                    throw new ArgumentOutOfRangeException(nameof(value));
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
                    throw new ArgumentOutOfRangeException(nameof(value));
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
                    throw new ArgumentOutOfRangeException(nameof(value));
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

        protected FinderOptions2(ConfigFile configFile, string prefix) : base(configFile, prefix) { }
    }
}