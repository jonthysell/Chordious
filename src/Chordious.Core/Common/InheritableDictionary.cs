﻿// Copyright (c) Jon Thysell <http://jonthysell.com>
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Xml;

using Chordious.Core.Resources;

namespace Chordious.Core
{
    public abstract class InheritableDictionary : IReadOnly
    {
        public bool ReadOnly { get; private set; }

        public InheritableDictionary Parent
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
        private InheritableDictionary _parent;

        public string Level
        {
            get
            {
                return _level;
            }
            set
            {
                if (ReadOnly)
                {
                    throw new ObjectIsReadOnlyException(this);
                }

                _level = value;
            }
        }
        private string _level;

        public string FriendlyLevel
        {
            get
            {
                return GetFriendlyLevel();
            }
        }

        public string Summary
        {
            get
            {
                return GetSummary();
            }
        }

        public int LocalCount
        {
            get
            {
                return _localDictionary.Count;
            }
        }

        public string this[string key]
        {
            get
            {
                return Get(key);
            }
            set
            {
                Set(key, value);
            }
        }
        protected Dictionary<string, string> _localDictionary;

        public InheritableDictionary()
        {
            ReadOnly = false;
            _localDictionary = new Dictionary<string, string>();
        }

        public InheritableDictionary(string level) : this()
        {
            Level = level;
        }

        public InheritableDictionary(InheritableDictionary parent) : this()
        {
            Parent = parent ?? throw new ArgumentNullException(nameof(parent));
        }

        public InheritableDictionary(InheritableDictionary parent, string level) : this(parent)
        {
            Level = level;
        }

        public void MarkAsReadOnly()
        {
            ReadOnly = true;
        }

        public void Read(XmlReader xmlReader, string localName)
        {
            if (null == xmlReader)
            {
                throw new ArgumentNullException(nameof(xmlReader));
            }

            if (StringUtils.IsNullOrWhiteSpace(localName))
            {
                throw new ArgumentNullException(nameof(localName));
            }

            if (ReadOnly)
            {
                throw new ObjectIsReadOnlyException(this);
            }

            using (xmlReader)
            {
                do
                {
                    if (xmlReader.IsStartElement() && xmlReader.Name == localName)
                    {
                        string key = xmlReader.GetAttribute("key");
                        string value = xmlReader.GetAttribute("value");

                        this[key] = value;
                    }
                } while (xmlReader.Read());
            }
        }

        public void Write(XmlWriter xmlWriter, string localName, string filter = "")
        {
            if (null == xmlWriter)
            {
                throw new ArgumentNullException(nameof(xmlWriter));
            }

            foreach (string key in LocalKeys(filter))
            {
                xmlWriter.WriteStartElement(localName);
                xmlWriter.WriteAttributeString("key", key);
                xmlWriter.WriteAttributeString("value", _localDictionary[key]);
                xmlWriter.WriteEndElement();
            }
        }

        public void Clear()
        {
            if (ReadOnly)
            {
                throw new ObjectIsReadOnlyException(this);
            }

            _localDictionary.Clear();
        }

        public void Clear(string key, bool recursive = false)
        {
            if (StringUtils.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentNullException(nameof(key));
            }

            if (ReadOnly)
            {
                throw new ObjectIsReadOnlyException(this);
            }

            key = CleanKey(key);

            if (_localDictionary.ContainsKey(key))
            {
                _localDictionary.Remove(key);
            }
            
            if (null != Parent && !Parent.ReadOnly && recursive) // Recursively check parent
            {
                Parent.Clear(key, recursive);
            }
        }

        public void ClearByPrefix(string prefix, bool recursive = false)
        {
            if (ReadOnly)
            {
                throw new ObjectIsReadOnlyException(this);
            }

            if (StringUtils.IsNullOrWhiteSpace(prefix))
            {
                throw new ArgumentNullException(nameof(prefix));
            }

            prefix = CleanPrefix(prefix);

            List<string> keysToRemove = new List<string>();

            foreach (string key in _localDictionary.Keys)
            {
                if (key.StartsWith(prefix))
                {
                    keysToRemove.Add(key);
                }
            }

            foreach (string key in keysToRemove)
            {
                _localDictionary.Remove(key);
            }

            if (null != Parent && recursive) // Recursively check parent
            {
                Parent.ClearByPrefix(prefix, recursive);
            }
        }

        public void Set(string key, Note value)
        {
            if (StringUtils.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentNullException(nameof(key));
            }

            Set(key, NoteUtils.ToString(value));
        }

        public void Set(string key, double value)
        {
            if (StringUtils.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentNullException(nameof(key));
            }

            Set(key, value.ToString(CultureInfo.InvariantCulture));
        }

        public void Set(string key, object value)
        {
            if (StringUtils.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentNullException(nameof(key));
            }

            if (null == value)
            {
                throw new ArgumentNullException(nameof(value));
            }

            Set(key, value.ToString());
        }

        public void Set(string key, string value)
        {
            if (StringUtils.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentNullException(nameof(key));
            }

            if (StringUtils.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentNullException(nameof(value));
            }

            if (ReadOnly)
            {
                throw new ObjectIsReadOnlyException(this);
            }

            key = CleanKey(key);

            if (_localDictionary.ContainsKey(key))
            {
                _localDictionary[key] = value;
            }
            else
            {
                _localDictionary.Add(key, value);
            }
        }

        public string Get(string key, bool recursive = true)
        {
            if (StringUtils.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentNullException(nameof(key));
            }

            if (TryGet(key, out string value, recursive))
            {
                return value;
            }

            throw new InheritableDictionaryKeyNotFoundException(this, key);
        }

        public bool TryGet(string key, out string result, bool recursive = true)
        {
            if (StringUtils.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentNullException(nameof(key));
            }

            key = CleanKey(key);

            if (_localDictionary.ContainsKey(key)) // Check locally
            {
                result = _localDictionary[key];
                return true;
            }
            else if (null != Parent && recursive) // Recursively check parent
            {
                return Parent.TryGet(key, out result, recursive);
            }

            result = null;
            return false;
        }

        public bool TryGet(string key, out object result, bool recursive = true)
        {
            if (StringUtils.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentNullException(nameof(key));
            }

            key = CleanKey(key);

            if (_localDictionary.ContainsKey(key)) // Check locally
            {
                result = _localDictionary[key];
                return true;
            }
            else if (null != Parent && recursive) // Recursively check parent
            {
                return Parent.TryGet(key, out result, recursive);
            }

            result = null;
            return false;
        }

        public bool GetBoolean(string key)
        {
            return GetBoolean(key, true);
        }

        public bool GetBoolean(string key, bool recursive)
        {
            if (StringUtils.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentNullException(nameof(key));
            }

            if (TryGet(key, out bool value, recursive))
            {
                return value;
            }

            throw new InheritableDictionaryKeyNotFoundException(this, key);
        }

        public bool GetBoolean(string key, bool defaultValue, bool recursive)
        {
            try
            {
                return GetBoolean(key, recursive);
            }
            catch (InheritableDictionaryKeyNotFoundException) { }

            return defaultValue;
        }

        public bool TryGet(string key, out bool result, bool recursive = true)
        {
            if (TryGet(key, out string rawResult, recursive))
            {
                return bool.TryParse(rawResult, out result);
            }

            result = default;
            return false;
        }

        public double GetDouble(string key, bool recursive = true)
        {
            if (StringUtils.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentNullException(nameof(key));
            }

            if (TryGet(key, out double value, recursive))
            {
                return value;
            }

            throw new InheritableDictionaryKeyNotFoundException(this, key);
        }

        public double GetDouble(string key, double defaultValue, bool recursive = true)
        {
            try
            {
                return GetDouble(key, recursive);
            }
            catch (InheritableDictionaryKeyNotFoundException) { }

            return defaultValue;
        }

        public bool TryGet(string key, out double result, bool recursive = true)
        {
            if (TryGet(key, out string rawResult, recursive))
            {
                // Check with InvariantCulture as first default
                if (double.TryParse(rawResult, NumberStyles.Any, CultureInfo.InvariantCulture, out result))
                {
                    return true;
                }

                // Try again with CurrentCulture
                return double.TryParse(rawResult, out result);
            }

            result = default;
            return false;
        }

        public float GetFloat(string key, bool recursive = true)
        {
            if (StringUtils.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentNullException(nameof(key));
            }

            if (TryGet(key, out float value, recursive))
            {
                return value;
            }

            throw new InheritableDictionaryKeyNotFoundException(this, key);
        }

        public float GetFloat(string key, float defaultValue, bool recursive = true)
        {
            try
            {
                return GetFloat(key, recursive);
            }
            catch (InheritableDictionaryKeyNotFoundException) { }

            return defaultValue;
        }

        public bool TryGet(string key, out float result, bool recursive = true)
        {
            if (TryGet(key, out string rawResult, recursive))
            {
                return float.TryParse(rawResult, out result);
            }

            result = default;
            return false;
        }

        public int GetInt32(string key, bool recursive = true)
        {
            if (StringUtils.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentNullException(nameof(key));
            }

            if (TryGet(key, out int value, recursive))
            {
                return value;
            }

            throw new InheritableDictionaryKeyNotFoundException(this, key);
        }

        public int GetInt32(string key, int defaultValue, bool recursive = true)
        {
            try
            {
                return GetInt32(key, recursive);
            }
            catch (InheritableDictionaryKeyNotFoundException) { }

            return defaultValue;
        }

        public bool TryGet(string key, out int result, bool recursive = true)
        {
            if (TryGet(key, out string rawResult, recursive))
            {
                return int.TryParse(rawResult, out result);
            }

            result = default;
            return false;
        }

        public TEnum GetEnum<TEnum>(string key, bool recursive = true) where TEnum : struct
        {
            if (StringUtils.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentNullException(nameof(key));
            }

            if (TryGet<TEnum>(key, out TEnum value, recursive))
            {
                return value;
            }

            throw new InheritableDictionaryKeyNotFoundException(this, key);
        }

        public TEnum GetEnum<TEnum>(string key, TEnum defaultValue, bool recursive = true) where TEnum : struct
        {
            try
            {
                return GetEnum<TEnum>(key, recursive);
            }
            catch (InheritableDictionaryKeyNotFoundException) { }

            return defaultValue;
        }

        public bool TryGet<TEnum>(string key, out TEnum result, bool recursive = true) where TEnum : struct
        {
            if (TryGet(key, out string rawResult, recursive))
            {
                try
                {
                    result = (TEnum)Enum.Parse(typeof(TEnum), rawResult);
                    return true;
                }
                catch (Exception) { }
            }

            result = default;
            return false;
        }

        public Note GetNote(string key, bool recursive = true)
        {
            if (StringUtils.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentNullException(nameof(key));
            }

            if (TryGet(key, out Note value, recursive))
            {
                return value;
            }

            throw new InheritableDictionaryKeyNotFoundException(this, key);
        }

        public Note GetNote(string key, Note defaultValue, bool recursive = true)
        {
            try
            {
                return GetNote(key, recursive);
            }
            catch (InheritableDictionaryKeyNotFoundException) { }

            return defaultValue;
        }

        public bool TryGet(string key, out Note result, bool recursive = true)
        {
            if (TryGet(key, out string rawResult, recursive))
            {
                return NoteUtils.TryParseNote(rawResult, out result);
            }

            result = default;
            return false;
        }

        public string GetLevel(string key)
        {
            if (StringUtils.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentNullException(nameof(key));
            }

            if (TryGetLevel(key, out string level))
            {
                return level;
            }

            throw new InheritableDictionaryKeyNotFoundException(this, key);
        }

        public bool TryGetLevel(string key, out string level)
        {
            if (StringUtils.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentNullException(nameof(key));
            }

            key = CleanKey(key);

            if (_localDictionary.ContainsKey(key)) // Check locally
            {
                level = Level;
                return true;
            }
            else if (null != Parent)
            {
                return Parent.TryGetLevel(key, out level);
            }

            level = null;
            return false;
        }

        public void Flatten()
        {
            if (ReadOnly)
            {
                throw new ObjectIsReadOnlyException(this);
            }

            List<string> allKeys = new List<string>(AllKeys());

            foreach (string key in allKeys)
            {
                Set(key, Get(key));
            }
        }

        public void SetParent()
        {
            if (null == Parent)
            {
                throw new ParentNotFoundException();
            }

            foreach (string key in _localDictionary.Keys)
            {
                SetParent(key);
            }
        }

        public void SetParent(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException(nameof(key));
            }

            key = CleanKey(key);

            if (null == Parent)
            {
                throw new ParentNotFoundException();
            }

            Parent.Set(key, this[key]);
        }

        public void CopyFrom(InheritableDictionary source)
        {
            if (null == source)
            {
                throw new ArgumentNullException(nameof(source));
            }

            foreach (string key in source.LocalKeys())
            {
                Set(key, source.Get(key, false));
            }
        }

        public bool HasKey(string key, bool recursive = true)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException(nameof(key));
            }

            key = CleanKey(key);

            if (_localDictionary.ContainsKey(key))
            {
                return true;
            }
            else if (null != Parent && recursive)
            {
                return Parent.HasKey(key, recursive);
            }

            return false;
        }

        public IEnumerable<string> LocalKeys(string filter = "")
        {
            bool hasFilter = false;
            if (!StringUtils.IsNullOrWhiteSpace(filter))
            {
                filter = CleanKey(filter);
                hasFilter = true;
            }

            foreach (string key in _localDictionary.Keys)
            {
                if (!hasFilter || (hasFilter && key.StartsWith(filter)))
                {
                    yield return key;
                }
            }
        }

        public IEnumerable<string> AllKeys()
        {
            List<string> existingKeys = new List<string>();

            InheritableDictionary pointer = this;
            do
            {
                foreach (string key in pointer.LocalKeys())
                {
                    if (!existingKeys.Contains(key))
                    {
                        existingKeys.Add(key);
                        yield return key;
                    }
                }
                pointer = pointer.Parent;
            } while (null != pointer);
        }

        public bool IsLocalGet(string key)
        {
            return HasKey(key, false);
        }

        public void IsLocalSet(string key, bool value, object defaultValue = null)
        {
            bool oldValue = IsLocalGet(key);

            if (value != oldValue)
            {
                if (value)
                {
                    if (TryGet(key, out object parentValue))
                    {
                        Set(key, parentValue);
                    }
                    else
                    {
                        Set(key, defaultValue);
                    }
                }
                else
                {
                    Clear(key, false);
                }
            }
        }

        public bool MatchesParentValue(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException(nameof(key));
            }

            if (TryGet(key, out string result) && null != Parent && Parent.TryGet(key, out string parentResult))
            {
                return result == parentResult;
            }

            return false;
        }

        public virtual string GetFriendlyKeyName(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException(nameof(key));
            }

            return CleanKey(key);
        }

        public virtual string GetFriendlyValue(string key, bool recursive = true)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException(nameof(key));
            }

            return Get(key, recursive);
        }

        protected string GetFriendlyDoubleValue(string key, bool recursive = true)
        {
            return GetFriendlyDoubleValue(key, "G", recursive);
        }

        protected string GetFriendlyDoubleValue(string key, string format, bool recursive = true)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException(nameof(key));
            }

            double value = GetDouble(key, recursive);

            return value.ToString(format);
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(Level);

            bool hasItems = false;
            
            if (null != Parent)
            {
                sb.AppendFormat(": {0}", Parent.Level);
                hasItems = true;
            }

            foreach (string key in LocalKeys())
            {
                if (!hasItems)
                {
                    sb.Append(": ");
                }
                else
                {
                    sb.Append(" + ");
                }

                sb.AppendFormat(CultureInfo.InvariantCulture, "{0}: {1}", key, Get(key, false));
                hasItems = true;
            }

            return sb.ToString();
        }

        protected string CleanPrefix(string prefix)
        {
            if (StringUtils.IsNullOrWhiteSpace(prefix))
            {
                return "";
            }

            prefix = prefix.ToLower().Trim();

            return prefix;
        }

        protected string CleanKey(string key)
        {
            if (StringUtils.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentNullException(nameof(key));
            }

            key = key.ToLower().Trim();

            return key;
        }

        protected virtual string GetFriendlyLevel()
        {
            switch (Level)
            {
                case ConfigFile.DefaultLevelKey:
                    return Strings.DefaultConfigFileFriendlyLevel;
                case ConfigFile.AppLevelKey:
                    return Strings.AppConfigFileFriendlyLevel;
                case ConfigFile.UserLevelKey:
                    return Strings.UserConfigFileFriendlyLevel;
                case Diagram.LevelKey:
                    return Strings.DiagramFriendlyLevel;
                case DiagramCollection.LevelKey:
                    return Strings.DiagramCollectionFriendlyLevel;
                case DiagramLibrary.LevelKey:
                    return Strings.DiagramLibraryFriendlyLevel;
                default:
                    return Level;
            }
        }

        private string GetSummary()
        {
            StringBuilder sb = new StringBuilder();

            bool hasItems = false;

            if (null != Parent)
            {
                sb.Append(Parent.FriendlyLevel);
                hasItems = true;
            }

            foreach (string key in LocalKeys().OrderBy(key => GetFriendlyKeyName(key)))
            {
                if (hasItems)
                {
                    sb.Append(" + ");
                }

                sb.AppendFormat(CultureInfo.InvariantCulture, "{0}: {1}", GetFriendlyKeyName(key), GetFriendlyValue(key, false));
                hasItems = true;
            }

            return sb.ToString();
        }

    }

    public class InheritableDictionaryKeyNotFoundException : ChordiousKeyNotFoundException
    {
        public InheritableDictionary InheritableDictionary { get; protected set; }
        public InheritableDictionaryKeyNotFoundException(InheritableDictionary inheritableDictionary, string key) : base(key)
        {
            InheritableDictionary = inheritableDictionary;
        }
    }

    public class LevelNotFoundException : ChordiousException
    {
        public string Level { get; protected set; }

        public override string Message
        {
            get
            {
                return string.Format(Strings.LevelNotFoundExceptionMessage, Level);
            }
        }

        public LevelNotFoundException(string level) : base()
        {
            Level = level;
        }
    }

    public class ParentNotFoundException : ChordiousException
    {
        public override string Message
        {
            get
            {
                return Strings.ParentNotFoundExceptionMessage;
            }
        }

        public ParentNotFoundException() : base() { }
    }
}
