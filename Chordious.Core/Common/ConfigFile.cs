// 
// ConfigFile.cs
//  
// Author:
//       Jon Thysell <thysell@gmail.com>
// 
// Copyright (c) 2013, 2015, 2016, 2017 Jon Thysell <http://jonthysell.com>
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
using System.IO;
using System.Reflection;
using System.Xml;

namespace com.jonthysell.Chordious.Core
{
    public class ConfigFile : IReadOnly
    {
        public static ConfigFile DefaultConfig
        {
            get
            {
                if (null == _defaultConfig)
                {
                    _defaultConfig = new ConfigFile(DefaultLevelKey);
                    _defaultConfig.LoadFile(typeof(ConfigFile).GetTypeInfo().Assembly.GetManifestResourceStream("com.jonthysell.Chordious.Core.Chordious.Core.xml"));
                    _defaultConfig.MarkAsReadOnly();
                }

                return _defaultConfig;
            }
        }
        private static ConfigFile _defaultConfig;

        public bool ReadOnly { get; private set; }

        public ConfigFile Parent
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
                FixInheritance();
            }
        }
        private ConfigFile _parent;

        public string Level
        {
            get
            {
                return _level;
            }
            private set
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

        public ChordiousSettings ChordiousSettings { get; private set; }

        public DiagramStyle DiagramStyle { get; private set; }

        public InstrumentSet Instruments { get; private set; }

        public ChordQualitySet ChordQualities { get; private set; }

        public ScaleSet Scales { get; private set; }

        public DiagramLibrary DiagramLibrary { get; private set; }

        public ConfigFile(string level)
        {
            Level = level;

            ChordiousSettings = new ChordiousSettings(level);
            DiagramStyle = new DiagramStyle(level);
            Instruments = new InstrumentSet(level);
            ChordQualities = new ChordQualitySet(level);
            Scales = new ScaleSet(level);
            DiagramLibrary = new DiagramLibrary(DiagramStyle);
        }

        public ConfigFile(ConfigFile parent, string level) : this(level)
        {
            Parent = parent;
        }

        public ConfigFile(Stream inputStream, string level) : this(level)
        {
            LoadFile(inputStream);
        }

        public ConfigFile(ConfigFile parent, Stream inputStream, string level) : this(parent, level)
        {
            LoadFile(inputStream);
        }

        public void LoadFile(Stream inputStream)
        {
            LoadFile(inputStream, ConfigParts.All);
        }

        public void LoadFile(Stream inputStream, ConfigParts configParts)
        {
            if (null == inputStream)
            {
                throw new ArgumentNullException("inputStream");
            }

            using (XmlReader reader = XmlReader.Create(inputStream))
            {
                while (reader.Read())
                {
                    if (reader.IsStartElement())
                    {
                        if (reader.Name == "settings" && ((configParts & ConfigParts.Settings) == ConfigParts.Settings))
                        {
                            ChordiousSettings.Read(reader.ReadSubtree());
                        }
                        else if (reader.Name == "styles" && ((configParts & ConfigParts.Styles) == ConfigParts.Styles))
                        {
                            DiagramStyle.Read(reader.ReadSubtree());
                        }
                        else if (reader.Name == "instruments" && ((configParts & ConfigParts.Instruments) == ConfigParts.Instruments))
                        {
                            Instruments.Read(reader.ReadSubtree());
                        }
                        else if (reader.Name == "qualities" && ((configParts & ConfigParts.Qualities) == ConfigParts.Qualities))
                        {
                            ChordQualities.Read(reader.ReadSubtree());
                        }
                        else if (reader.Name == "scales" && ((configParts & ConfigParts.Scales) == ConfigParts.Scales))
                        {
                            Scales.Read(reader.ReadSubtree());
                        }
                        else if (reader.Name == "library" && ((configParts & ConfigParts.Library) == ConfigParts.Library))
                        {
                            DiagramLibrary.Read(reader.ReadSubtree());
                        }
                    }
                }
            }
        }

        public void SaveFile(Stream outputStream)
        {
            SaveFile(outputStream, ConfigParts.All);
        }

        public void SaveFile(Stream outputStream, ConfigParts configParts)
        {
            if (null == outputStream)
            {
                throw new ArgumentNullException("outputStream");
            }

            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;

            using (XmlWriter writer = XmlWriter.Create(outputStream, settings))
            {
                writer.WriteStartElement("chordious");

                writer.WriteAttributeString("version", AppInfo.ProgramTitle);
                writer.WriteAttributeString("date", DateTime.UtcNow.ToString());

                if ((configParts & ConfigParts.Settings) == ConfigParts.Settings)
                {
                    writer.WriteStartElement("settings");
                    ChordiousSettings.Write(writer);
                    writer.WriteEndElement();
                }

                if ((configParts & ConfigParts.Styles) == ConfigParts.Styles)
                {
                    writer.WriteStartElement("styles");
                    DiagramStyle.Write(writer);
                    writer.WriteEndElement();
                }

                if ((configParts & ConfigParts.Instruments) == ConfigParts.Instruments)
                {
                    writer.WriteStartElement("instruments");
                    Instruments.Write(writer);
                    writer.WriteEndElement();
                }

                if ((configParts & ConfigParts.Qualities) == ConfigParts.Qualities)
                {
                    writer.WriteStartElement("qualities");
                    ChordQualities.Write(writer);
                    writer.WriteEndElement();
                }

                if ((configParts & ConfigParts.Scales) == ConfigParts.Scales)
                {
                    writer.WriteStartElement("scales");
                    Scales.Write(writer);
                    writer.WriteEndElement();
                }

                if ((configParts & ConfigParts.Library) == ConfigParts.Library)
                {
                    writer.WriteStartElement("library");
                    DiagramLibrary.Write(writer);
                    writer.WriteEndElement();
                }

                writer.WriteEndElement();
            }
        }

        public void ImportConfig(ConfigFile configFile)
        {
            ImportConfig(configFile, ConfigParts.All);
        }

        public void ImportConfig(ConfigFile configFile, ConfigParts configParts)
        {
            if (null == configFile)
            {
                throw new ArgumentNullException("configFile");
            }

            if ((configParts & ConfigParts.Settings) == ConfigParts.Settings)
            {
                ChordiousSettings.CopyFrom(configFile.ChordiousSettings);
            }

            if ((configParts & ConfigParts.Styles) == ConfigParts.Styles)
            {
                DiagramStyle.CopyFrom(configFile.DiagramStyle);
            }

            if ((configParts & ConfigParts.Instruments) == ConfigParts.Instruments)
            {
                Instruments.CopyFrom(configFile.Instruments);
            }

            if ((configParts & ConfigParts.Qualities) == ConfigParts.Qualities)
            {
                ChordQualities.CopyFrom(configFile.ChordQualities);
            }

            if ((configParts & ConfigParts.Scales) == ConfigParts.Scales)
            {
                Scales.CopyFrom(configFile.Scales);
            }

            if ((configParts & ConfigParts.Library) == ConfigParts.Library)
            {
                DiagramLibrary.CopyFrom(configFile.DiagramLibrary);
            }
        }

        public void MarkAsReadOnly()
        {
            ReadOnly = true;
            ChordiousSettings.MarkAsReadOnly();
            DiagramStyle.MarkAsReadOnly();
            Instruments.MarkAsReadOnly();
            ChordQualities.MarkAsReadOnly();
            Scales.MarkAsReadOnly();
        }

        private void FixInheritance()
        {
            ChordiousSettings.Parent = (null == Parent) ? null : Parent.ChordiousSettings;
            DiagramStyle.Parent = (null == Parent) ? null : Parent.DiagramStyle;
            Instruments.Parent = (null == Parent) ? null : Parent.Instruments;
            ChordQualities.Parent = (null == Parent) ? null : Parent.ChordQualities;
            Scales.Parent = (null == Parent) ? null : Parent.Scales;
        }

        public const string DefaultLevelKey = "Default";
        public const string AppLevelKey = "App";
        public const string UserLevelKey = "User";
    }

    [Flags]
    public enum ConfigParts
    {
        None = 0,
        Settings = 1,
        Styles = 2,
        Instruments = 4,
        Qualities = 8,
        Scales = 16,
        Library = 32,
        All = 63
    }
}