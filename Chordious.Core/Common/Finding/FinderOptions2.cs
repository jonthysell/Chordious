// 
// FinderOptions.cs
//  
// Author:
//       Jon Thysell <thysell@gmail.com>
// 
// Copyright (c) 2015, 2016, 2017, 2019 Jon Thysell <http://jonthysell.com>
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
                    throw new ArgumentOutOfRangeException();
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
                    throw new ArgumentOutOfRangeException();
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
                    throw new ArgumentOutOfRangeException();
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