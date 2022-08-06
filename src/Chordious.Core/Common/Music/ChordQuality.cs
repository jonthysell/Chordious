// Copyright (c) Jon Thysell <http://jonthysell.com>
// Licensed under the MIT License.

using System;
using System.Xml;

namespace Chordious.Core
{
    public class ChordQuality : NamedInterval, IChordQuality
    {
        public string Abbreviation
        {
            get
            {
                return _abbreviation;
            }
            set
            {
                if (StringUtils.IsNullOrWhiteSpace(value))
                {
                    value = "";
                }

                if (ReadOnly)
                {
                    throw new ObjectIsReadOnlyException(this);
                }

                string oldValue = _abbreviation;
                _abbreviation = value;

                // Resort with parent
                if (UpdateParent)
                {
                    Parent.Resort(this, () =>
                    {
                        _abbreviation = oldValue;
                    });
                }
            }
        }
        private string _abbreviation;

        internal ChordQuality(ChordQualitySet parent, string name, string abbreviation, int[] intervals) : base(parent)
        {
            if (parent is null)
            {
                throw new ArgumentNullException(nameof(parent));
            }

            Name = name;
            Abbreviation = abbreviation;
            Intervals = intervals;

            UpdateParent = true;
        }

        internal ChordQuality(ChordQualitySet parent, XmlReader xmlReader) : base(parent)
        {
            if (parent is null)
            {
                throw new ArgumentNullException(nameof(parent));
            }

            if (xmlReader is null)
            {
                throw new ArgumentNullException(nameof(xmlReader));
            }

            using (xmlReader)
            {
                if (ReadBase(xmlReader, "quality"))
                {
                    Abbreviation = xmlReader.GetAttribute("abbv");
                }
            }

            UpdateParent = true;
        }

        protected override string GetLongName()
        {
            if (!StringUtils.IsNullOrWhiteSpace(Abbreviation))
            {
                return string.Format("{0} \"{1}\" ({2})", Name, Abbreviation, GetIntervalString());
            }

            return base.GetLongName();
        }

        public void Update(string name, string abbreviation, int[] intervals)
        {
            if (StringUtils.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            if (ReadOnly)
            {
                throw new ObjectIsReadOnlyException(this);
            }

            UpdateParent = false;

            string oldName = Name;
            string oldAbbreviation = Abbreviation;
            int[] oldIntervals = Intervals;

            Name = name;
            Abbreviation = abbreviation;
            Intervals = intervals ?? throw new ArgumentNullException(nameof(intervals));

            Parent.Resort(this, () =>
            {
                Name = oldName;
                Abbreviation = oldAbbreviation;
                Intervals = oldIntervals;
                UpdateParent = true;
            });
        }

        public void Write(XmlWriter xmlWriter)
        {
            if (xmlWriter is null)
            {
                throw new ArgumentNullException(nameof(xmlWriter));
            }

            WriteBase(xmlWriter, "quality");

            xmlWriter.WriteAttributeString("abbv", Abbreviation);

            xmlWriter.WriteEndElement();
        }
    }
}
