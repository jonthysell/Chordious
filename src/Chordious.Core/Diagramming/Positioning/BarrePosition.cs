// Copyright (c) Jon Thysell <http://jonthysell.com>
// Licensed under the MIT License.

using System;

using Chordious.Core.Resources;

namespace Chordious.Core
{
    public class BarrePosition : ElementPosition
    {
        public int Fret
        {
            get
            {
                return _fret;
            }
            private set
            {
                if (value < 1)
                {
                    throw new ArgumentOutOfRangeException();
                }
                _fret = value;
            }
        }
        private int _fret;

        public int StartString
        {
            get
            {
                return _startString;
            }
            private set
            {
                if (value < 1)
                {
                    throw new ArgumentOutOfRangeException();
                }
                _startString = value;
            }
        }
        private int _startString;

        public int EndString
        {
            get
            {
                return _endString;
            }
            private set
            {
                if (value < 1)
                {
                    throw new ArgumentOutOfRangeException();
                }
                _endString = value;
            }
        }
        private int _endString;

        public int Width
        {
            get
            {
                return (EndString - StartString) + 1;
            }
        }

        public BarrePosition(int fret, int startString, int endString)
        {
            Fret = fret;

            if (startString >= endString)
            {
                throw new BarrePositionInvalidSpanException(startString, endString);
            }

            StartString = startString;
            EndString = endString;
        }

        public override ElementPosition Clone()
        {
            return new BarrePosition(Fret, StartString, EndString);
        }

        public bool Contains(int fret, int @string)
        {
            if (fret < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(fret));
            }

            if (@string < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(@string));
            }

            if (Fret == fret)
            {
                if (StartString <= @string && EndString >= @string)
                {
                    return true;
                }
            }

            return false;
        }

        public bool Overlaps(BarrePosition position)
        {
            if (null == position)
            {
                throw new ArgumentNullException(nameof(position));
            }

            if (this == position)
            {
                return true;
            }

            if (Fret == position.Fret)
            {
                if (StartString == position.StartString || EndString == position.EndString)
                {
                    return true;
                }
                else if (StartString < position.StartString)
                {
                    if (EndString >= position.StartString)
                    {
                        return true;
                    }
                }
                else if (StartString > position.StartString)
                {
                    if (position.EndString >= StartString)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public static BarrePosition Parse(string s)
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

            string[] vals = s.Split(':', '-');

            return new BarrePosition(int.Parse(vals[0]), int.Parse(vals[1]), int.Parse(vals[2]));
        }

        public override bool Equals(ElementPosition obj)
        {
            BarrePosition bp = (obj as BarrePosition);
            return null != bp && bp.Fret == Fret && bp.StartString == StartString && bp.EndString == EndString;
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

        public override string ToString()
        {
            return string.Format("{0}:{1}-{2}", Fret, StartString, EndString);
        }
    }

    public class BarrePositionInvalidSpanException : ElementPositionException
    {
        public int AttemptedStartString { get; private set; }
        public int AttemptedEndString { get; private set; }

        public override string Message
        {
            get
            {
                return string.Format(Strings.BarrePositionInvalidSpanExceptionMessage, AttemptedStartString, AttemptedEndString);
            }
        }

        public BarrePositionInvalidSpanException(int startString, int endString) : base()
        {
            AttemptedStartString = startString;
            AttemptedEndString = endString;
        }
    }
}
