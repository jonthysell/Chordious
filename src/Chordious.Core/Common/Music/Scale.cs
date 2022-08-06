// Copyright (c) Jon Thysell <http://jonthysell.com>
// Licensed under the MIT License.

using System;
using System.Xml;

namespace Chordious.Core
{
    public class Scale : NamedInterval, IScale
    {
        internal Scale(ScaleSet parent, string name, int[] intervals) : base(parent)
        {
            if (parent is null)
            {
                throw new ArgumentNullException(nameof(parent));
            }

            Name = name;
            Intervals = intervals;

            UpdateParent = true;
        }

        internal Scale(ScaleSet parent, XmlReader xmlReader) : base(parent)
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
                ReadBase(xmlReader, "scale");
            }

            UpdateParent = true;
        }

        public void Write(XmlWriter xmlWriter)
        {
            if (xmlWriter is null)
            {
                throw new ArgumentNullException(nameof(xmlWriter));
            }

            WriteBase(xmlWriter, "scale");

            xmlWriter.WriteEndElement();
        }
    }
}