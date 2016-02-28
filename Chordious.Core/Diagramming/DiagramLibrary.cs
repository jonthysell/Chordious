// 
// DiagramLibrary.cs
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
using System.Xml;

namespace com.jonthysell.Chordious.Core
{
    public class DiagramLibrary
    {
        public DiagramStyle Style { get; private set; }

        private List<DiagramLibraryNode> _nodes;

        public DiagramLibrary(DiagramStyle parentStyle)
        {
            Style = new DiagramStyle(parentStyle, "DiagramLibrary");
            _nodes = new List<DiagramLibraryNode>();
        }

        public DiagramCollection Get(string name)
        {
            return Get(PathUtils.PathRoot, name);
        }

        public DiagramCollection Get(string path, string name)
        {
            if (StringUtils.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException("name");
            }

            path = PathUtils.Clean(path);

            DiagramCollection diagramCollection;
            if (TryGet(path, name, out diagramCollection))
            {
                return diagramCollection;
            }

            throw new DiagramCollectionNotFoundException(this, path, name);
        }

        public bool TryGet(string name, out DiagramCollection diagramCollection)
        {
            return TryGet(PathUtils.PathRoot, name, out diagramCollection);
        }

        public bool TryGet(string path, string name, out DiagramCollection diagramCollection)
        {
            if (StringUtils.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException("name");
            }

            path = PathUtils.Clean(path);

            foreach (DiagramLibraryNode node in _nodes)
            {
                if (node.Name == name && node.Path == path)
                {
                    diagramCollection = node.DiagramCollection;
                    return true;
                }
            }

            diagramCollection = null;
            return false;
        }

        public DiagramCollection Add(string name)
        {
            return Add(PathUtils.PathRoot, name);
        }

        public DiagramCollection Add(string path, string name)
        {
            DiagramLibraryNode node = new DiagramLibraryNode(this.Style, path, name);
            AddNode(node);
            return node.DiagramCollection;
        }

        public void Remove(string name)
        {
            Remove(PathUtils.PathRoot, name);
        }

        public void Remove(string path, string name)
        {
            DiagramLibraryNode diagramLibraryNode;

            if (!TryGetNode(path, name, out diagramLibraryNode))
            {
                throw new DiagramCollectionNotFoundException(this, path, name);
            }

            _nodes.Remove(diagramLibraryNode);
        }

        public void MoveAll(string sourcePath, string destinationPath)
        {
            sourcePath = PathUtils.Clean(sourcePath);
            destinationPath = PathUtils.Clean(destinationPath);

            if (sourcePath == destinationPath)
            {
                return;
            }

            bool oldPathExists = PathExists(sourcePath);
            bool newPathExists = PathExists(destinationPath);

            if (!oldPathExists)
            {
                throw new PathNotFoundException(this, sourcePath);
            }
            else if (oldPathExists && !newPathExists)
            {
                foreach (DiagramLibraryNode node in _nodes)
                {
                    if (node.Path == sourcePath || node.Path.StartsWith(sourcePath + PathUtils.PathSeperator))
                    {
                        node.Path = node.Path.Replace(sourcePath, destinationPath);
                    }
                }
            }
            else if (oldPathExists && newPathExists)
            {
                // check for any conflicting nodes

                // move nodes if everything is okay
            }

        }

        public void Move(string sourcePath, string name, string destinationPath)
        {
            if (StringUtils.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException("name");
            }

            sourcePath = PathUtils.Clean(sourcePath);
            destinationPath = PathUtils.Clean(destinationPath);

            name = name.Trim();

            DiagramLibraryNode node;
            if (TryGetNode(sourcePath, name, out node))
            {
                DiagramLibraryNode existingNode;
                if (TryGetNode(destinationPath, name, out existingNode))
                {
                    throw new DiagramCollectionNameAlreadyExistsException(this, destinationPath, name);
                }

                node.Path = destinationPath;
            }
            else
            {
                throw new DiagramCollectionNotFoundException(this, sourcePath, name);
            }
        }

        public void Rename(string name, string newName)
        {
            Rename(PathUtils.PathRoot, name, newName);
        }

        public void Rename(string path, string name, string newName)
        {
            if (StringUtils.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException("name");
            }

            if (StringUtils.IsNullOrWhiteSpace(newName))
            {
                throw new ArgumentNullException("newName");
            }

            path = PathUtils.Clean(path);

            name = name.Trim();
            newName = newName.Trim();

            if (name != newName)
            {
                DiagramLibraryNode node;
                if (TryGetNode(path, name, out node))
                {
                    DiagramLibraryNode existingNode;
                    if (TryGetNode(path, newName, out existingNode))
                    {
                        throw new DiagramCollectionNameAlreadyExistsException(this, path, newName);
                    }

                    _nodes.Remove(node);
                    node.Name = newName;
                    AddNode(node);
                }
                else
                {
                    throw new DiagramCollectionNotFoundException(this, path, name);
                }
            }
        }

        public void Clone(string name, string newName)
        {
            Clone(PathUtils.PathRoot, name, newName);
        }

        public void Clone(string path, string name, string newName)
        {
            if (StringUtils.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException("name");
            }

            if (StringUtils.IsNullOrWhiteSpace(newName))
            {
                throw new ArgumentNullException("newName");
            }

            path = PathUtils.Clean(path);

            name = name.Trim();
            newName = newName.Trim();

            if (name != newName)
            {
                DiagramLibraryNode node;
                if (TryGetNode(path, name, out node))
                {
                    DiagramLibraryNode clonedNode = node.Clone();
                    clonedNode.Name = newName;
                    AddNode(clonedNode);
                }
                else
                {
                    throw new DiagramCollectionNotFoundException(this, path, name);
                }
            }
        }

        public string GetNewCollectionName()
        {
            return GetNewCollectionName(PathUtils.PathRoot, "New Collection");
        }

        public string GetNewCollectionName(string baseName)
        {
            return GetNewCollectionName(PathUtils.PathRoot, baseName);
        }

        public string GetNewCollectionName(string path, string baseName)
        {
            if (StringUtils.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentNullException("path");
            }

            if (StringUtils.IsNullOrWhiteSpace(baseName))
            {
                throw new ArgumentNullException("baseName");
            }

            string name = baseName;

            bool valid = false;

            int count = 1;
            DiagramLibraryNode node;
            while (!valid)
            {
                if (!TryGetNode(path, name, out node))
                {
                    valid = true; // Found an unused name
                }
                else
                {
                    name = String.Format("{0} ({1})", baseName, count);
                    count++;
                }
            }

            return name;
        }

        public bool PathExists(string path)
        {
            path = PathUtils.Clean(path);

            foreach (DiagramLibraryNode node in _nodes)
            {
                if (node.Path == path || node.Path.StartsWith(path + PathUtils.PathSeperator))
                {
                    return true;
                }
            }

            return false;
        }

        public IEnumerable<string> GetSubFolders()
        {
            return GetSubFolders(PathUtils.PathRoot);
        }

        public IEnumerable<string> GetSubFolders(string path)
        {
            path = PathUtils.Clean(path);

            SortedSet<string> subFolders = new SortedSet<string>();
            foreach (DiagramLibraryNode node in _nodes)
            {
                string nodePath = node.Path;
                if (nodePath.StartsWith(path))
                {
                    nodePath = nodePath.Substring(path.Length);

                    int firstSeperator = nodePath.IndexOf(PathUtils.PathSeperator);
                    if (firstSeperator > 0)
                    {
                        nodePath = nodePath.Substring(0, firstSeperator);
                    }

                    if (!StringUtils.IsNullOrWhiteSpace(nodePath))
                    {
                        subFolders.Add(nodePath);
                    }
                }
            }
            return subFolders;
        }

        public IEnumerable<KeyValuePair<string, DiagramCollection>> GetAll()
        {
            return GetAll(PathUtils.PathRoot);
        }

        public IEnumerable<KeyValuePair<string, DiagramCollection>> GetAll(string path)
        {
            path = PathUtils.Clean(path);

            List<KeyValuePair<string, DiagramCollection>> collections = new List<KeyValuePair<string, DiagramCollection>>();
            foreach (DiagramLibraryNode node in _nodes)
            {
                string nodePath = node.Path;
                if (nodePath == path)
                {
                    collections.Add(new KeyValuePair<string,DiagramCollection>(node.Name, node.DiagramCollection));
                }
            }
            return collections;
        }

        public void CopyFrom(DiagramLibrary diagramLibrary)
        {
            if (null == diagramLibrary)
            {
                throw new ArgumentNullException("diagramLibrary");
            }

            foreach (KeyValuePair<string, DiagramCollection> sourceKVP in diagramLibrary.GetAll())
            {
                DiagramCollection collection = null;

                if (!TryGet(sourceKVP.Key, out collection))
                {
                    collection = Add(sourceKVP.Key); 
                }

                collection.Add(sourceKVP.Value);
            }
        }

        public void Read(XmlReader xmlReader)
        {
            if (null == xmlReader)
            {
                throw new ArgumentNullException("xmlReader");
            }

            using (xmlReader)
            {
                do
                {
                    if (xmlReader.IsStartElement())
                    {
                        switch (xmlReader.Name)
                        {
                            case "diagrams":
                                DiagramLibraryNode node = new DiagramLibraryNode(this.Style, xmlReader.ReadSubtree());
                                AddNode(node);
                                break;
                            case "style":
                                this.Style.Read(xmlReader.ReadSubtree());
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
                throw new ArgumentNullException("xmlWriter");
            }

            this.Style.Write(xmlWriter);

            foreach (DiagramLibraryNode node in _nodes)
            {
                node.Write(xmlWriter);
            }
        }

        private bool TryGetNode(string path, string name, out DiagramLibraryNode diagramLibraryNode)
        {
            if (StringUtils.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException("name");
            }

            path = PathUtils.Clean(path);
            name = name.Trim();

            foreach (DiagramLibraryNode node in _nodes)
            {
                if (node.Name == name && node.Path == path)
                {
                    diagramLibraryNode = node;
                    return true;
                }
            }

            diagramLibraryNode = null;
            return false;
        }

        private void AddNode(DiagramLibraryNode node)
        {
            if (null == node)
            {
                throw new ArgumentNullException("node");
            }

            string path = node.Path;
            string name = node.Name;

            DiagramLibraryNode existingNode;
            if (TryGetNode(path, name, out existingNode))
            {
                throw new DiagramCollectionNameAlreadyExistsException(this, path, name);
            }

            ListUtils.SortedInsert<DiagramLibraryNode>(_nodes, node);
        }
    }

    public abstract class DiagramLibraryException : ChordiousException
    {
        public DiagramLibrary DiagramLibrary { get; private set; }

        public DiagramLibraryException(DiagramLibrary diagramLibrary) : base()
        {
            this.DiagramLibrary = diagramLibrary;
        }
    }

    public abstract class TargetDiagramCollectionException : DiagramLibraryException
    {
        public string Path { get; private set; }
        public string Name { get; private set; }

        public TargetDiagramCollectionException(DiagramLibrary diagramLibrary, string path, string name) : base(diagramLibrary)
        {
            this.Path = path;
            this.Name = name;
        }
    }

    public class DiagramCollectionNotFoundException : TargetDiagramCollectionException
    {
        public override string Message
        {
            get
            {
                return String.Format(Resources.Strings.DiagramCollectionNotFoundExceptionMessage, Name);
            }
        }

        public DiagramCollectionNotFoundException(DiagramLibrary diagramLibrary, string path, string name) : base(diagramLibrary, path, name) { }
    }

    public class DiagramCollectionNameAlreadyExistsException : TargetDiagramCollectionException
    {
        public override string Message
        {
            get
            {
                return String.Format(Resources.Strings.DiagramCollectionNameAlreadyExistsMessage, Name);
            }
        }

        public DiagramCollectionNameAlreadyExistsException(DiagramLibrary diagramLibrary, string path, string name) : base(diagramLibrary, path, name) { }
    }

    public abstract class TargetPathException : DiagramLibraryException
    {
        public string Path { get; private set; }

        public TargetPathException(DiagramLibrary diagramLibrary, string path) : base(diagramLibrary)
        {
            this.Path = path;
        }
    }

    public class PathNotFoundException : TargetPathException
    {
        public override string Message
        {
            get
            {
                return String.Format(Resources.Strings.PathNotFoundExceptionMessage, Path);
            }
        }

        public PathNotFoundException(DiagramLibrary diagramLibrary, string path) : base(diagramLibrary, path) { }
    }
}
