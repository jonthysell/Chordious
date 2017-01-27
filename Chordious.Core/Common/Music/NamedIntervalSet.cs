// 
// NamedIntervalSet.cs
//  
// Author:
//       Jon Thysell <thysell@gmail.com>
// 
// Copyright (c) 2015, 2017 Jon Thysell <http://jonthysell.com>
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
using System.Collections;
using System.Collections.Generic;
using System.Xml;

using com.jonthysell.Chordious.Core.Resources;

namespace com.jonthysell.Chordious.Core
{
    public abstract class NamedIntervalSet : IReadOnly, IEnumerable<NamedInterval>
    {
        public bool ReadOnly { get; protected set; }

        public string Level
        {
            get
            {
                return _level;
            }
            set
            {
                if (StringUtils.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentNullException();
                }

                if (ReadOnly)
                {
                    throw new ObjectIsReadOnlyException(this);
                }

                _level = value;
            }
        }
        private string _level;

        public NamedIntervalSet Parent
        {
            get
            {
                return _parent;
            }
            set
            {
                if (ReadOnly)
                {
                    throw new ObjectIsReadOnlyException(this);
                }

                _parent = value;
            }
        }
        private NamedIntervalSet _parent;

        protected List<NamedInterval> _namedIntervals;

        protected NamedIntervalSet(string level)
        {
            Level = level;
            ReadOnly = false;
            _namedIntervals = new List<NamedInterval>();
        }

        protected NamedIntervalSet(NamedIntervalSet parent, string level) : this(level)
        {
            if (null == parent)
            {
                throw new ArgumentNullException("parent");
            }

            Parent = parent;
        }

        public void MarkAsReadOnly()
        {
            ReadOnly = true;
            foreach (NamedInterval namedInterval in _namedIntervals)
            {
                namedInterval.MarkAsReadOnly();
            }
        }

        public IEnumerator<NamedInterval> GetEnumerator()
        {
            foreach (NamedInterval namedInterval in _namedIntervals)
            {
                yield return namedInterval;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public NamedInterval Get(string longName)
        {
            if (StringUtils.IsNullOrWhiteSpace(longName))
            {
                throw new ArgumentNullException("longName");
            }

            NamedInterval namedInterval;
            if (TryGet(longName, out namedInterval))
            {
                return namedInterval;
            }

            throw new NamedIntervalNotFoundException(this, longName);
        }

        public bool TryGet(string longName, out NamedInterval namedInterval)
        {
            if (StringUtils.IsNullOrWhiteSpace(longName))
            {
                throw new ArgumentNullException("longName");
            }

            foreach (NamedInterval ni in _namedIntervals)
            {
                if (ni.LongName == longName)
                {
                    namedInterval = ni;
                    return true;
                }
            }

            namedInterval = null;
            return false;
        }

        protected void Add(NamedInterval namedInterval)
        {
            if (null == namedInterval)
            {
                throw new ArgumentNullException("namedInterval");
            }

            if (namedInterval.Parent != this)
            {
                throw new ArgumentOutOfRangeException("namedInterval");
            }

            if (!ListUtils.SortedInsert<NamedInterval>(_namedIntervals, namedInterval))
            {
                throw new NamedIntervalAlreadyExistsException(this, namedInterval.LongName);
            }
        }

        public bool Remove(NamedInterval namedInterval)
        {
            if (null == namedInterval)
            {
                throw new ArgumentNullException("namedinterval");
            }

            if (ReadOnly)
            {
                throw new ObjectIsReadOnlyException(this);
            }

            return _namedIntervals.Remove(namedInterval);
        }

        internal void Resort(NamedInterval namedInterval, Action rollback)
        {
            if (Remove(namedInterval))
            {
                try
                {
                    Add(namedInterval);
                }
                catch (Exception ex)
                {
                    if (null != rollback)
                    {
                        rollback();

                        // Re-add since remove succeeded but re-add failed
                        if (null != (ex as NamedIntervalAlreadyExistsException))
                        {
                            Add(namedInterval);
                        }
                    }
                    throw;
                }
            }
        }

        public abstract void Read(XmlReader xmlReader);

        public abstract void Write(XmlWriter xmlWriter);
    }

    public abstract class NamedIntervalSetException : ChordiousException
    {
        public NamedIntervalSet NamedIntervalSet { get; private set; }

        public NamedIntervalSetException(NamedIntervalSet namedIntervalSet) : base()
        {
            NamedIntervalSet = namedIntervalSet;
        }
    }

    public abstract class TargetNamedIntervalException : NamedIntervalSetException
    {
        public string LongName { get; private set; }

        public TargetNamedIntervalException(NamedIntervalSet namedIntervalSet, string longName) : base(namedIntervalSet)
        {
            LongName = longName;
        }
    }

    public class NamedIntervalNotFoundException : TargetNamedIntervalException
    {
        public override string Message
        {
            get
            {
                if (null != (NamedIntervalSet as ChordQualitySet))
                {
                    return string.Format(Strings.ChordQualityNotFoundExceptionMessage, LongName);
                }
                else if (null != (NamedIntervalSet as ScaleSet))
                {
                    return string.Format(Strings.ScaleNotFoundExceptionMessage, LongName);
                }

                return string.Format(Strings.NamedIntervalNotFoundExceptionMessage, LongName);
            }
        }

        public NamedIntervalNotFoundException(NamedIntervalSet namedIntervalSet, string longName) : base(namedIntervalSet, longName) { }
    }

    public class NamedIntervalAlreadyExistsException : TargetNamedIntervalException
    {
        public override string Message
        {
            get
            {
                if (null != (NamedIntervalSet as ChordQualitySet))
                {
                    return string.Format(Strings.ChordQualityAlreadyExistsExceptionMessage, LongName);
                }
                else if ((null != NamedIntervalSet as ScaleSet))
                {
                    return string.Format(Strings.ScaleAlreadyExistsExceptionMessage, LongName);
                }

                return string.Format(Strings.NamedIntervalAlreadyExistsExceptionMessage, LongName);
            }
        }

        public NamedIntervalAlreadyExistsException(NamedIntervalSet namedIntervalSet, string longName) : base(namedIntervalSet, longName) { }
    }
}
