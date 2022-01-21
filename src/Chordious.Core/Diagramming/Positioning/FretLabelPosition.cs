// Copyright (c) Jon Thysell <http://jonthysell.com>
// Licensed under the MIT License.

using System;

namespace Chordious.Core
{
    public class FretLabelPosition : ElementPosition
    {
        public FretLabelSide Side { get; private set; }

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

        public FretLabelPosition(FretLabelSide side, int fret)
        {
            Side = side;
            Fret = fret;
        }

        public override ElementPosition Clone()
        {
            return new FretLabelPosition(Side, Fret);
        }

        public override bool Equals(ElementPosition obj)
        {
            FretLabelPosition flp = (obj as FretLabelPosition);
            return null != flp && flp.Side == Side && flp.Fret == Fret;
        }

        public override int GetHashCode()
        {
            return string.Format("{0}:{1}", Side, Fret).GetHashCode();
        }
    }

    public enum FretLabelSide
    {
        Left,
        Right
    }
}
