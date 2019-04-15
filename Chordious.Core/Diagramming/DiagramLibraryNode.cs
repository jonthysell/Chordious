// 
// DiagramLibraryNode.cs
//  
// Author:
//       Jon Thysell <thysell@gmail.com>
// 
// Copyright (c) 2015, 2016, 2017, 2019 Jon Thysell <http://jonthysell.com>
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
using System.Xml;

namespace com.jonthysell.Chordious.Core
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
                    throw new ArgumentNullException();
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
            if (null == xmlReader)
            {
                throw new ArgumentNullException("xmlReader");
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
            if (null == xmlWriter)
            {
                throw new ArgumentNullException("xmlWriter");
            }

            xmlWriter.WriteStartElement("diagrams");

            xmlWriter.WriteAttributeString("name", Name);
            xmlWriter.WriteAttributeString("path", Path);

            DiagramCollection.Write(xmlWriter);

            xmlWriter.WriteEndElement();
        }

        public int CompareTo(object obj)
        {
            if (null == obj)
            {
                throw new ArgumentNullException("obj");
            }

            DiagramLibraryNode node = obj as DiagramLibraryNode;
            if (null == node)
            {
                throw new ArgumentException();
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
