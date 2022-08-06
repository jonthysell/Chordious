// Copyright (c) Jon Thysell <http://jonthysell.com>
// Licensed under the MIT License.

using System;
using System.IO;
using System.Reflection;
using System.Xml;

namespace Chordious.Core
{
    public class ConfigFile : IReadOnly
    {
        public static ConfigFile DefaultConfig
        {
            get
            {
                if (_defaultConfig is null)
                {
                    _defaultConfig = new ConfigFile(DefaultLevelKey);
                    _defaultConfig.LoadFile(typeof(ConfigFile).GetTypeInfo().Assembly.GetManifestResourceStream("Chordious.Core.Chordious.Core.xml"));
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
            if (inputStream is null)
            {
                throw new ArgumentNullException(nameof(inputStream));
            }

            using XmlReader reader = XmlReader.Create(inputStream);
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

        public void SaveFile(Stream outputStream)
        {
            SaveFile(outputStream, ConfigParts.All);
        }

        public void SaveFile(Stream outputStream, ConfigParts configParts)
        {
            if (outputStream is null)
            {
                throw new ArgumentNullException(nameof(outputStream));
            }

            XmlWriterSettings settings = new XmlWriterSettings
            {
                Indent = true
            };

            using XmlWriter writer = XmlWriter.Create(outputStream, settings);
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

        public void ImportConfig(ConfigFile configFile)
        {
            ImportConfig(configFile, ConfigParts.All);
        }

        public void ImportConfig(ConfigFile configFile, ConfigParts configParts)
        {
            if (configFile is null)
            {
                throw new ArgumentNullException(nameof(configFile));
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
            ChordiousSettings.Parent = Parent?.ChordiousSettings;
            DiagramStyle.Parent = Parent?.DiagramStyle;
            Instruments.Parent = Parent?.Instruments;
            ChordQualities.Parent = Parent?.ChordQualities;
            Scales.Parent = Parent?.Scales;
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
