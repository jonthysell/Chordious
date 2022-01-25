// Copyright (c) Jon Thysell <http://jonthysell.com>
// Licensed under the MIT License.

using System;
using System.Xml;

using Chordious.Core.Resources;

namespace Chordious.Core
{
    public class ChordiousSettings : InheritableDictionary
    {
        public new ChordiousSettings Parent
        {
            get
            {
                return (ChordiousSettings)base.Parent;
            }
            internal set
            {
                base.Parent = value;
            }
        }

        public ChordiousSettings() : base() { }

        public ChordiousSettings(string level) : base(level) { }

        public ChordiousSettings(ChordiousSettings parentSettings) : base(parentSettings) { }

        public ChordiousSettings(ChordiousSettings parentSettings, string level) : base(parentSettings, level) { }

        public void Read(XmlReader xmlReader)
        {
            Read(xmlReader, "setting");
        }

        public void Write(XmlWriter xmlWriter, string filter = "")
        {
            Write(xmlWriter, "setting", filter);
        }

        public ChordiousSettings Clone()
        {
            ChordiousSettings cs = new ChordiousSettings(Level);

            if (null != Parent)
            {
                cs.Parent = Parent;
            }

            cs.CopyFrom(this);

            return cs;
        }

        public override string GetFriendlyKeyName(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException(nameof(key));
            }

            key = CleanKey(key);

            switch (key)
            {
                default:
                    return base.GetFriendlyKeyName(key);
            }
        }

        protected override string GetFriendlyLevel()
        {
            return string.Format(Strings.ChordiousSettingsFriendlyLevelFormat, base.GetFriendlyLevel());
        }
    }
}
