// 
// InheritableDictionary.cs
//  
// Author:
//       Jon Thysell <thysell@gmail.com>
// 
// Copyright (c) 2015, 2016 Jon Thysell <http://jonthysell.com>
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
using System.Collections.Generic;
using System.Text;
using System.Xml;

using com.jonthysell.Chordious.Core.Resources;

namespace com.jonthysell.Chordious.Core
{
    public abstract class InheritableDictionary : IReadOnly
    {
        public bool ReadOnly { get; private set; }

        public InheritableDictionary Parent
        {
            get
            {
                return this._parent;
            }
            set
            {
                if (this.ReadOnly)
                {
                    throw new ObjectIsReadOnlyException(this);
                }

                this._parent = value;
            }
        }
        private InheritableDictionary _parent;

        public string Level
        {
            get
            {
                return this._level;
            }
            set
            {
                if (this.ReadOnly)
                {
                    throw new ObjectIsReadOnlyException(this);
                }

                this._level = value;
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
                return this._localDictionary.Count;
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
            this.ReadOnly = false;
            this._localDictionary = new Dictionary<string, string>();
        }

        public InheritableDictionary(string level) : this()
        {
            this.Level = level;
        }

        public InheritableDictionary(InheritableDictionary parent) : this()
        {
            if (null == parent)
            {
                throw new ArgumentNullException("parent");
            }

            this.Parent = parent;
        }

        public InheritableDictionary(InheritableDictionary parent, string level) : this(parent)
        {
            this.Level = level;
        }

        public void MarkAsReadOnly()
        {
            this.ReadOnly = true;
        }

        public void Read(XmlReader xmlReader, string localName)
        {
            if (null == xmlReader)
            {
                throw new ArgumentNullException("xmlReader");
            }

            if (StringUtils.IsNullOrWhiteSpace(localName))
            {
                throw new ArgumentNullException("localName");
            }

            if (this.ReadOnly)
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
                throw new ArgumentNullException("xmlWriter");
            }

            foreach (string key in this.LocalKeys(filter))
            {
                xmlWriter.WriteStartElement(localName);
                xmlWriter.WriteAttributeString("key", key);
                xmlWriter.WriteAttributeString("value", this._localDictionary[key]);
                xmlWriter.WriteEndElement();
            }
        }

        public void Clear()
        {
            if (this.ReadOnly)
            {
                throw new ObjectIsReadOnlyException(this);
            }

            _localDictionary.Clear();
        }

        public void Clear(string key, bool recursive = false)
        {
            if (StringUtils.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentNullException("key");
            }

            if (this.ReadOnly)
            {
                throw new ObjectIsReadOnlyException(this);
            }

            key = CleanKey(key);

            if (_localDictionary.ContainsKey(key))
            {
                _localDictionary.Remove(key);
            }
            
            if (null != this.Parent && !this.Parent.ReadOnly && recursive) // Recursively check parent
            {
                this.Parent.Clear(key, recursive);
            }
        }

        public void ClearByPrefix(string prefix, bool recursive = false)
        {
            if (this.ReadOnly)
            {
                throw new ObjectIsReadOnlyException(this);
            }

            if (StringUtils.IsNullOrWhiteSpace(prefix))
            {
                throw new ArgumentNullException("prefix");
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

            if (null != this.Parent && recursive) // Recursively check parent
            {
                this.Parent.ClearByPrefix(prefix, recursive);
            }
        }

        public void Set(string key, Note value)
        {
            if (StringUtils.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentNullException("key");
            }

            Set(key, NoteUtils.ToString(value));
        }

        public void Set(string key, object value)
        {
            if (StringUtils.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentNullException("key");
            }

            if (null == value)
            {
                throw new ArgumentNullException("value");
            }

            Set(key, value.ToString());
        }

        public void Set(string key, string value)
        {
            if (StringUtils.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentNullException("key");
            }

            if (StringUtils.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentNullException("value");
            }

            if (this.ReadOnly)
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
                throw new ArgumentNullException("key");
            }

            string value;
            if (TryGet(key, out value, recursive))
            {
                return value;
            }

            throw new InheritableDictionaryKeyNotFoundException(this, key);
        }

        public bool TryGet(string key, out string result, bool recursive = true)
        {
            if (StringUtils.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentNullException("key");
            }

            key = CleanKey(key);

            if (_localDictionary.ContainsKey(key)) // Check locally
            {
                result = _localDictionary[key];
                return true;
            }
            else if (null != this.Parent && recursive) // Recursively check parent
            {
                return this.Parent.TryGet(key, out result, recursive);
            }

            result = null;
            return false;
        }

        public bool TryGet(string key, out object result, bool recursive = true)
        {
            if (StringUtils.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentNullException("key");
            }

            key = CleanKey(key);

            if (_localDictionary.ContainsKey(key)) // Check locally
            {
                result = _localDictionary[key];
                return true;
            }
            else if (null != this.Parent && recursive) // Recursively check parent
            {
                return this.Parent.TryGet(key, out result, recursive);
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
                throw new ArgumentNullException("key");
            }

            bool value;
            if (TryGet(key, out value, recursive))
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
            string rawResult;
            if (TryGet(key, out rawResult, recursive))
            {
                return Boolean.TryParse(rawResult, out result);
            }

            result = default(bool);
            return false;
        }

        public double GetDouble(string key, bool recursive = true)
        {
            if (StringUtils.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentNullException("key");
            }

            double value;
            if (TryGet(key, out value, recursive))
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
            string rawResult;
            if (TryGet(key, out rawResult, recursive))
            {
                return Double.TryParse(rawResult, out result);
            }

            result = default(double);
            return false;
        }

        public float GetFloat(string key, bool recursive = true)
        {
            if (StringUtils.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentNullException("key");
            }

            float value;
            if (TryGet(key, out value, recursive))
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
            string rawResult;
            if (TryGet(key, out rawResult, recursive))
            {
                return float.TryParse(rawResult, out result);
            }

            result = default(float);
            return false;
        }

        public int GetInt32(string key, bool recursive = true)
        {
            if (StringUtils.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentNullException("key");
            }

            int value;
            if (TryGet(key, out value, recursive))
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
            string rawResult;
            if (TryGet(key, out rawResult, recursive))
            {
                return Int32.TryParse(rawResult, out result);
            }

            result = default(int);
            return false;
        }

        public TEnum GetEnum<TEnum>(string key, bool recursive = true) where TEnum : struct
        {
            if (StringUtils.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentNullException("key");
            }

            TEnum value;
            if (TryGet<TEnum>(key, out value, recursive))
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
            string rawResult;
            if (TryGet(key, out rawResult, recursive))
            {
                try
                {
                    result = (TEnum)Enum.Parse(typeof(TEnum), rawResult);
                    return true;
                }
                catch (Exception) { }
            }

            result = default(TEnum);
            return false;
        }

        public Note GetNote(string key, bool recursive = true)
        {
            if (StringUtils.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentNullException("key");
            }

            Note value;
            if (TryGet(key, out value, recursive))
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
            string rawResult;
            if (TryGet(key, out rawResult, recursive))
            {
                return NoteUtils.TryParseNote(rawResult, out result);
            }

            result = default(Note);
            return false;
        }

        public string GetLevel(string key)
        {
            if (StringUtils.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentNullException("key");
            }

            string level;
            if (TryGetLevel(key, out level))
            {
                return level;
            }

            throw new InheritableDictionaryKeyNotFoundException(this, key);
        }

        public bool TryGetLevel(string key, out string level)
        {
            if (StringUtils.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentNullException("key");
            }

            key = CleanKey(key);

            if (_localDictionary.ContainsKey(key)) // Check locally
            {
                level = this.Level;
                return true;
            }
            else if (null != this.Parent)
            {
                return this.Parent.TryGetLevel(key, out level);
            }

            level = null;
            return false;
        }

        public void Flatten()
        {
            if (this.ReadOnly)
            {
                throw new ObjectIsReadOnlyException(this);
            }

            List<string> allKeys = new List<string>(this.AllKeys());

            foreach (string key in allKeys)
            {
                this.Set(key, this.Get(key));
            }
        }

        public void SetParent()
        {
            if (null == this.Parent)
            {
                throw new ParentNotFoundException();
            }

            foreach (string key in _localDictionary.Keys)
            {
                this.SetParent(key);
            }
        }

        public void SetParent(string key)
        {
            if (String.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException("key");
            }

            key = CleanKey(key);

            if (null == this.Parent)
            {
                throw new ParentNotFoundException();
            }

            this.Parent.Set(key, this[key]);
        }

        public void CopyFrom(InheritableDictionary source)
        {
            if (null == source)
            {
                throw new ArgumentNullException("source");
            }

            foreach (string key in source.LocalKeys())
            {
                this.Set(key, source.Get(key, false));
            }
        }

        public bool HasKey(string key, bool recursive = true)
        {
            if (String.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException("key");
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

            foreach (string key in this._localDictionary.Keys)
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
                    object parentValue;
                    if (TryGet(key, out parentValue))
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

        public virtual string GetFriendlyKeyName(string key)
        {
            if (String.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException("key");
            }

            return CleanKey(key);
        }

        public virtual string GetFriendlyValueName(string key, bool recursive = true)
        {
            if (String.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException("key");
            }

            return Get(key, recursive);
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(Level);

            bool hasItems = false;
            
            if (null != this.Parent)
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

                sb.AppendFormat("{0}: {1}", key, Get(key, false));
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
                throw new ArgumentNullException("key");
            }

            key = key.ToLower().Trim();

            return key;
        }

        protected virtual string GetFriendlyLevel()
        {
            switch (this.Level)
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
                    return this.Level;
            }
        }

        private string GetSummary()
        {
            StringBuilder sb = new StringBuilder();

            bool hasItems = false;

            if (null != this.Parent)
            {
                sb.Append(Parent.FriendlyLevel);
                hasItems = true;
            }

            foreach (string key in LocalKeys())
            {
                if (hasItems)
                {
                    sb.Append(" + ");
                }

                sb.AppendFormat("{0}: {1}", GetFriendlyKeyName(key), GetFriendlyValueName(key, false));
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
            this.InheritableDictionary = inheritableDictionary;
        }
    }

    public class LevelNotFoundException : ChordiousException
    {
        public string Level { get; protected set; }

        public override string Message
        {
            get
            {
                return String.Format(Resources.Strings.LevelNotFoundExceptionMessage, Level);
            }
        }

        public LevelNotFoundException(string level) : base()
        {
            this.Level = level;
        }
    }

    public class ParentNotFoundException : ChordiousException
    {
        public override string Message
        {
            get
            {
                return Resources.Strings.ParentNotFoundExceptionMessage;
            }
        }

        public ParentNotFoundException() : base() { }
    }
}
