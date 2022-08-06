// Copyright (c) Jon Thysell <http://jonthysell.com>
// Licensed under the MIT License.

using System;
using System.Xml;

using Chordious.Core.Resources;

namespace Chordious.Core
{
    public class ChordQualitySet : NamedIntervalSet
    {
        public new ChordQualitySet Parent
        {
            get
            {
                return (ChordQualitySet)base.Parent;
            }
            set
            {
                base.Parent = value;
            }
        }

        internal ChordQualitySet(string level) : base(level) { }

        internal ChordQualitySet(ChordQualitySet parent, string level) : base(parent, level) { }

        public new ChordQuality Get(string longName)
        {
            return (ChordQuality)(base.Get(longName));
        }

        public bool TryGet(string longName, out ChordQuality chordQuality)
        {
            if (TryGet(longName, out NamedInterval namedInterval))
            {
                chordQuality = (ChordQuality)namedInterval;
                return true;
            }

            chordQuality = null;
            return false;
        }

        public ChordQuality Add(string name, string abbreviation, int[] intervals)
        {
            if (ReadOnly)
            {
                throw new ObjectIsReadOnlyException(this);
            }

            ChordQuality chordQuality = new ChordQuality(this, name, abbreviation, intervals);
            Add(chordQuality);
            return chordQuality;
        }

        public string GetNewChordQualityName()
        {
            return GetNewChordQualityName(Strings.ChordQualitySetDefaultNewChordQualityName);
        }

        public string GetNewChordQualityName(string baseName)
        {
            if (StringUtils.IsNullOrWhiteSpace(baseName))
            {
                throw new ArgumentNullException(nameof(baseName));
            }

            string name = baseName;

            bool valid = false;

            int count = 1;
            while (!valid)
            {
                if (!_namedIntervals.Exists(interval => interval.Name == name))
                {
                    valid = true; // Found an unused name
                }
                else
                {
                    name = string.Format("{0} ({1})", baseName, count);
                    count++;
                }
            }

            return name;
        }

        public void CopyFrom(ChordQualitySet chordQualitySet)
        {
            if (chordQualitySet is null)
            {
                throw new ArgumentNullException(nameof(chordQualitySet));
            }

            foreach (ChordQuality sourceChordQuality in chordQualitySet)
            {
                bool found = false;
                foreach (NamedInterval ni in _namedIntervals)
                {
                    ChordQuality chordQuality = (ChordQuality)ni;
                    if (sourceChordQuality == chordQuality)
                    {
                        found = true;
                        break;
                    }
                }

                // Only add if it wasn't found
                if (!found)
                {
                    Add(sourceChordQuality.Name, sourceChordQuality.Abbreviation, sourceChordQuality.Intervals);
                }
            }
        }

        public override void Read(XmlReader xmlReader)
        {
            if (xmlReader is null)
            {
                throw new ArgumentNullException(nameof(xmlReader));
            }

            using (xmlReader)
            {
                do
                {
                    if (xmlReader.IsStartElement() && xmlReader.Name == "quality")
                    {
                        ChordQuality chordQuality = new ChordQuality(this, xmlReader.ReadSubtree());
                        Add(chordQuality);
                    }
                } while (xmlReader.Read());
            }
        }

        public override void Write(XmlWriter xmlWriter)
        {
            if (xmlWriter is null)
            {
                throw new ArgumentNullException(nameof(xmlWriter));
            }

            foreach (NamedInterval namedInterval in _namedIntervals)
            {
                ChordQuality chordQuality = (ChordQuality)namedInterval;
                chordQuality.Write(xmlWriter);
            }
        }
    }
}
