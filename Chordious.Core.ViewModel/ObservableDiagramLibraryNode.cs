// 
// ObservableDiagramLibraryNode.cs
//  
// Author:
//       Jon Thysell <thysell@gmail.com>
// 
// Copyright (c) 2015 Jon Thysell <http://jonthysell.com>
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
using System.Collections.ObjectModel;
using System.Collections.Specialized;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

using com.jonthysell.Chordious.Core;

namespace com.jonthysell.Chordious.Core.ViewModel
{
    public class ObservableDiagramLibraryNode : ObservableObject
    {
        public string Path
        {
            get
            {
                return _path;
            }
            private set
            {
                _path = value;
                RaisePropertyChanged("Path");
            }
        }
        private string _path;

        public string Name
        {
            get
            {
                return _name;
            }
            private set
            {
                _name = value;
                RaisePropertyChanged("Name");
            }
        }
        private string _name;

        public ObservableCollection<ObservableDiagram> Diagrams
        {
            get
            {
                ObservableCollection<ObservableDiagram> collection = new ObservableCollection<ObservableDiagram>();
                foreach (Diagram diagram in Library.Get(Path, Name))
                {
                    collection.Add(new ObservableDiagram(diagram));
                }
                return collection;
            }
        }

        public RelayCommand<string> Rename
        {
            get
            {
                return new RelayCommand<string>((newName) =>
                {
                    try
                    {
                        Library.Rename(Path, Name, newName);
                        Name = newName.Trim();
                    }
                    catch (Exception ex)
                    {
                        ExceptionUtils.HandleException(ex);
                    }
                });
            }
        }

        internal DiagramLibrary Library { get; private set; }

        public ObservableDiagramLibraryNode(string path, string name, DiagramLibrary library) : base()
        {
            if (null == library)
            {
                throw new ArgumentNullException("library");
            }

            Path = path;
            Name = name;

            Library = library;
        }
    }
}
