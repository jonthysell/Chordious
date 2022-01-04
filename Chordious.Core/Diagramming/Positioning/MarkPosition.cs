// Copyright (c) Jon Thysell <http://jonthysell.com>
// Licensed under the MIT License.

using System;

namespace Chordious.Core
{
    public class MarkPosition : ElementPosition
    {
        public int String
        {
            get
            {
                return _string;
            }
            private set
            {
                if (value < 1)
                {
                    throw new ArgumentOutOfRangeException();
                }
                _string = value;
            }
        }
        private int _string;

        public int Fret
        {
            get
            {
                return _fret;
            }
            private set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException();
                }
                _fret = value;
            }
        }
        private int _fret;

        public MarkPosition(int @string, int fret)
        {
            String = @string;
            Fret = fret;
        }

        public override ElementPosition Clone()
        {
            return new MarkPosition(String, Fret);
        }

        public static MarkPosition Parse(string s)
        {
            if (StringUtils.IsNullOrWhiteSpace(s))
            {
                throw new ArgumentNullException(s);
            }

            s = s.Trim();

            if (s.Equals("null", StringComparison.CurrentCultureIgnoreCase))
            {
                return null;
            }

            string[] vals = s.Split(':');

            return new MarkPosition(int.Parse(vals[0]), int.Parse(vals[1]));
        }

        public override bool Equals(ElementPosition obj)
        {
            MarkPosition mp = (obj as MarkPosition);
            return null != mp && mp.String == String && mp.Fret == Fret;
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

        public override string ToString()
        {
            return string.Format("{0}:{1}", String, Fret);
        }
    }
}
