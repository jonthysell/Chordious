// Copyright (c) Jon Thysell <http://jonthysell.com>
// Licensed under the MIT License.

using System;
using System.Xml;

namespace Chordious.Core
{
    internal class DiagramLibraryNode : IComparable
    {
        public string Path
        {
            get
            {
                return _path;
            }
            set
            {
                _path = PathUtils.Clean(value);
            }
        }
        private string _path;

        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                if (StringUtils.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentNullException(nameof(value));
                }

                _name = value.Trim();
            }
        }
        private string _name;

        public DiagramCollection DiagramCollection { get; private set; }

        private DiagramLibraryNode(DiagramStyle parentStyle)
        {
            DiagramCollection = new DiagramCollection(parentStyle);
        }

        public DiagramLibraryNode(DiagramStyle parentStyle, string path, string name) : this(parentStyle)
        {
            Path = path;
            Name = name;
        }

        public DiagramLibraryNode(DiagramStyle parentStyle, XmlReader xmlReader) : this(parentStyle)
        {
            Read(xmlReader);
        }

        public void Read(XmlReader xmlReader)
        {
            if (xmlReader is null)
            {
                throw new ArgumentNullException(nameof(xmlReader));
            }

            using (xmlReader)
            {
                if (xmlReader.IsStartElement() && xmlReader.Name == "diagrams")
                {
                    Path = xmlReader.GetAttribute("path");
                    Name = xmlReader.GetAttribute("name");

                    DiagramCollection.Read(xmlReader.ReadSubtree());
                }
            }
        }

        public void Write(XmlWriter xmlWriter)
        {
            if (xmlWriter is null)
            {
                throw new ArgumentNullException(nameof(xmlWriter));
            }

            xmlWriter.WriteStartElement("diagrams");

            xmlWriter.WriteAttributeString("name", Name);
            xmlWriter.WriteAttributeString("path", Path);

            DiagramCollection.Write(xmlWriter);

            xmlWriter.WriteEndElement();
        }

        public int CompareTo(object obj)
        {
            if (obj is null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            if (obj is not DiagramLibraryNode node)
            {
                throw new ArgumentOutOfRangeException(nameof(obj));
            }

            int comparePaths = Path.CompareTo(node.Path);

            if (comparePaths != 0)
            {
                return comparePaths;
            }
            else
            {
                return Name.CompareTo(node.Name);
            }
        }

        public DiagramLibraryNode Clone()
        {
            DiagramLibraryNode clone = new DiagramLibraryNode(DiagramCollection.Style.Parent, Path, Name)
            {
                DiagramCollection = DiagramCollection.Clone()
            };
            return clone;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
