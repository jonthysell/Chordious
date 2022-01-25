// Copyright (c) Jon Thysell <http://jonthysell.com>
// Licensed under the MIT License.

using System;
using System.Collections.Generic;

using Chordious.Core.Resources;

namespace Chordious.Core
{
    public class ChordiousException : Exception
    {
        public ChordiousException() : base() { }
        public ChordiousException(string message) : base(message) { }
    }

    public abstract class ChordiousKeyNotFoundException : KeyNotFoundException
    {
        public string Key { get; protected set; }

        public override string Message
        {
            get
            {
                return string.Format(Strings.ChordiousKeyNotFoundExceptionMessage, Key);
            }
        }

        public ChordiousKeyNotFoundException(string key) : base()
        {
            Key = key;
        }
    }
}
