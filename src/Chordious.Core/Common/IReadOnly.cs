// Copyright (c) Jon Thysell <http://jonthysell.com>
// Licensed under the MIT License.


using Chordious.Core.Resources;

namespace Chordious.Core
{
    public interface IReadOnly
    {
        bool ReadOnly { get; }
        void MarkAsReadOnly();
    }

    public class ObjectIsReadOnlyException : ChordiousException
    {
        public IReadOnly ReadOnlyObject { get; private set; }

        public override string Message
        {
            get
            {
                string name = ReadOnlyObject.GetType().Name;
                return string.Format(Strings.ObjectIsReadOnlyExceptionMessage, name);
            }
        }

        public ObjectIsReadOnlyException(IReadOnly obj) : base()
        {
            ReadOnlyObject = obj;
        }
    }
}
