﻿// Copyright (c) Jon Thysell <http://jonthysell.com>
// Licensed under the MIT License.

using System;
using System.Xml;

using Chordious.Core.Resources;

namespace Chordious.Core
{
    public class ScaleSet : NamedIntervalSet
    {
        public new ScaleSet Parent
        {
            get
            {
                return (ScaleSet)base.Parent;
            }
            set
            {
                base.Parent = value;
            }
        }

        internal ScaleSet(string level) : base(level) { }

        internal ScaleSet(ScaleSet parent, string level) : base(parent, level) { }

        public new Scale Get(string longName)
        {
            return (Scale)(base.Get(longName));
        }

        public bool TryGet(string longName, out Scale scale)
        {
            if (TryGet(longName, out NamedInterval namedInterval))
            {
                scale = (Scale)namedInterval;
                return true;
            }

            scale = null;
            return false;
        }

        public Scale Add(string name, int[] intervals)
        {
            if (ReadOnly)
            {
                throw new ObjectIsReadOnlyException(this);
            }

            Scale scale = new Scale(this, name, intervals);
            Add(scale);
            return scale;
        }

        public string GetNewScaleName()
        {
            return GetNewScaleName(Strings.ScaleSetDefaultNewScaleName);
        }

        public string GetNewScaleName(string baseName)
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

        public void CopyFrom(ScaleSet scaleSet)
        {
            if (null == scaleSet)
            {
                throw new ArgumentNullException(nameof(scaleSet));
            }

            foreach (Scale sourceScale in scaleSet)
            {
                bool found = false;
                foreach (NamedInterval namedInterval in _namedIntervals)
                {
                    Scale scale = (Scale)namedInterval;
                    if (sourceScale == scale)
                    {
                        found = true;
                        break;
                    }
                }

                // Only add if it wasn't found
                if (!found)
                {
                    Add(sourceScale.Name, sourceScale.Intervals);
                }
            }
        }

        public override void Read(XmlReader xmlReader)
        {
            if (null == xmlReader)
            {
                throw new ArgumentNullException(nameof(xmlReader));
            }

            using (xmlReader)
            {
                do
                {
                    if (xmlReader.IsStartElement() && xmlReader.Name == "scale")
                    {
                        Scale scale = new Scale(this, xmlReader.ReadSubtree());
                        Add(scale);
                    }
                } while (xmlReader.Read());
            }
        }

        public override void Write(XmlWriter xmlWriter)
        {
            if (null == xmlWriter)
            {
                throw new ArgumentNullException(nameof(xmlWriter));
            }

            foreach (NamedInterval namedInterval in _namedIntervals)
            {
                Scale scale = (Scale)namedInterval;
                scale.Write(xmlWriter);
            }
        }
    }
}
