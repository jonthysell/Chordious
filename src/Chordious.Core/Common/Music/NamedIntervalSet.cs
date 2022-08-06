// Copyright (c) Jon Thysell <http://jonthysell.com>
// Licensed under the MIT License.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

using Chordious.Core.Resources;

namespace Chordious.Core
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
            Parent = parent ?? throw new ArgumentNullException(nameof(parent));
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
                throw new ArgumentNullException(nameof(longName));
            }

            if (TryGet(longName, out NamedInterval namedInterval))
            {
                return namedInterval;
            }

            throw new NamedIntervalNotFoundException(this, longName);
        }

        public bool TryGet(string longName, out NamedInterval namedInterval)
        {
            if (StringUtils.IsNullOrWhiteSpace(longName))
            {
                throw new ArgumentNullException(nameof(longName));
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
            if (namedInterval is null)
            {
                throw new ArgumentNullException(nameof(namedInterval));
            }

            if (namedInterval.Parent != this)
            {
                throw new ArgumentOutOfRangeException(nameof(namedInterval));
            }

            if (!ListUtils.SortedInsert<NamedInterval>(_namedIntervals, namedInterval))
            {
                throw new NamedIntervalAlreadyExistsException(this, namedInterval.LongName);
            }
        }

        public bool Remove(NamedInterval namedInterval)
        {
            if (namedInterval is null)
            {
                throw new ArgumentNullException(nameof(namedInterval));
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
                    if (rollback is not null)
                    {
                        rollback();

                        // Re-add since remove succeeded but re-add failed
                        if (ex is NamedIntervalAlreadyExistsException)
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
                if (NamedIntervalSet is ChordQualitySet)
                {
                    return string.Format(Strings.ChordQualityNotFoundExceptionMessage, LongName);
                }
                else if (NamedIntervalSet is ScaleSet)
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
                if (NamedIntervalSet is ChordQualitySet)
                {
                    return string.Format(Strings.ChordQualityAlreadyExistsExceptionMessage, LongName);
                }
                else if (NamedIntervalSet is ScaleSet)
                {
                    return string.Format(Strings.ScaleAlreadyExistsExceptionMessage, LongName);
                }

                return string.Format(Strings.NamedIntervalAlreadyExistsExceptionMessage, LongName);
            }
        }

        public NamedIntervalAlreadyExistsException(NamedIntervalSet namedIntervalSet, string longName) : base(namedIntervalSet, longName) { }
    }
}
