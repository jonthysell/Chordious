// Copyright (c) Jon Thysell <http://jonthysell.com>
// Licensed under the MIT License.

using System;

namespace Chordious.Core
{
    public abstract class ElementPosition : IEquatable<ElementPosition>
    {
        public abstract ElementPosition Clone();

        public override bool Equals(object obj)
        {
            return Equals(obj as ElementPosition);
        }

        public static bool operator ==(ElementPosition a, ElementPosition b)
        {
            if (a is null)
            {
                return b is null;
            }

            return a.Equals(b);
        }
        public static bool operator !=(ElementPosition a, ElementPosition b)
        {
            return !(a == b);
        }

        public abstract bool Equals(ElementPosition obj);
        public override abstract int GetHashCode();
    }

    public abstract class ElementPositionException : ChordiousException
    {
        public ElementPosition Position { get; protected set; }

        public ElementPositionException() : base()
        {
            Position = null;
        }

        public ElementPositionException(ElementPosition position) : base()
        {
            Position = position.Clone();
        }
    }
}
