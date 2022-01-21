// Copyright (c) Jon Thysell <http://jonthysell.com>
// Licensed under the MIT License.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

namespace Chordious.Core
{
    public class DiagramCollection : IEnumerable<Diagram>
    {
        public DiagramStyle Style { get; private set; }

        public int Count
        {
            get
            {
                return _diagrams.Count;
            }
        }

        private readonly List<Diagram> _diagrams;

        public DiagramCollection(DiagramStyle parentStyle)
        {
            if (null == parentStyle)
            {
                throw new ArgumentNullException(nameof(parentStyle));
            }

            Style = new DiagramStyle(parentStyle, LevelKey);

            _diagrams = new List<Diagram>();
        }

        public DiagramCollection(DiagramStyle parentStyle, XmlReader xmlReader) : this(parentStyle)
        {
            Read(xmlReader);
        }

        public void Add(Diagram diagram)
        {
            if (null == diagram)
            {
                throw new ArgumentNullException(nameof(diagram));
            }

            diagram.Style.Parent = Style;
            _diagrams.Add(diagram);
        }

        public void Add(DiagramCollection diagramCollection)
        {
            if (null == diagramCollection)
            {
                throw new ArgumentNullException(nameof(diagramCollection));
            }

            Style.CopyFrom(diagramCollection.Style);

            foreach (Diagram d in diagramCollection)
            {
                Add(d);
            }
        }

        public void Remove(Diagram diagram)
        {
            if (null == diagram)
            {
                throw new ArgumentNullException(nameof(diagram));
            }

            _diagrams.Remove(diagram);
        }

        public void Replace(Diagram oldDiagram, Diagram newDiagram)
        {
            if (null == oldDiagram)
            {
                throw new ArgumentNullException(nameof(oldDiagram));
            }

            if (null == newDiagram)
            {
                throw new ArgumentNullException(nameof(newDiagram));
            }

            int index = _diagrams.IndexOf(oldDiagram);
            _diagrams.RemoveAt(index);

            newDiagram.Style.Parent = Style;
            _diagrams.Insert(index, newDiagram);
        }

        public bool Contains(Diagram diagram)
        {
            if (null == diagram)
            {
                throw new ArgumentNullException(nameof(diagram));
            }

            return _diagrams.Contains(diagram);
        }

        public void Clear()
        {
            _diagrams.Clear();
        }

        public Diagram DiagramAt(int index)
        {
            if (index < 0 || index > _diagrams.Count)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }

            return _diagrams[index];
        }

        public IEnumerator<Diagram> GetEnumerator()
        {
            foreach (Diagram diagram in _diagrams)
            {
                yield return diagram;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Read(XmlReader xmlReader)
        {
            if (null == xmlReader)
            {
                throw new ArgumentNullException(nameof(xmlReader));
            }

            using (xmlReader)
            {
                do
                {
                    if (xmlReader.IsStartElement())
                    {
                        switch(xmlReader.Name)
                        {
                            case "diagram":
                                Diagram diagram = new Diagram(Style, xmlReader.ReadSubtree());
                                Add(diagram);
                                break;
                            case "style":
                                Style.Read(xmlReader.ReadSubtree());
                                break;
                        }
                    }
                } while (xmlReader.Read());
            }
        }

        public void Write(XmlWriter xmlWriter)
        {
            if (null == xmlWriter)
            {
                throw new ArgumentNullException(nameof(xmlWriter));
            }

            Style.Write(xmlWriter);

            foreach (Diagram d in _diagrams)
            {
                d.Write(xmlWriter);
            }
        }

        public DiagramCollection Clone()
        {
            DiagramCollection clone = new DiagramCollection(Style.Parent);

            clone.Style.CopyFrom(Style);

            foreach (Diagram diagram in this)
            {
                clone.Add(diagram.Clone());
            }

            return clone;
        }

        public const string LevelKey = "DiagramCollection";
    }
}
