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
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;

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
                if (firstLoad)
                {
                    foreach (Diagram diagram in Library.Get(Path, Name))
                    {
                        _diagrams.Add(CreateObservableDiagram(diagram));
                    }
                    firstLoad = false;
                }
                return _diagrams;
            }
            private set
            {
                _diagrams = value;
                RaisePropertyChanged("Diagrams");
            }
        }
        public ObservableCollection<ObservableDiagram> _diagrams;

        private ObservableDiagram CreateObservableDiagram(Diagram diagram)
        {
            ObservableDiagram od = new ObservableDiagram(diagram);
            od.PostEditCallback = GetDiagramPostEditCallback(od);
            return od;
        }

        private Action<bool> GetDiagramPostEditCallback(ObservableDiagram od)
        {
            ObservableDiagram originalObservableDiagram = od;
            Diagram originalDiagram = od.Diagram;

            return (changed) =>
            {
                if (changed)
                {
                    DiagramCollection collection = Library.Get(Path, Name);
                    collection.Replace(originalDiagram, originalObservableDiagram.Diagram);
                    originalObservableDiagram.Refresh();
                    originalObservableDiagram.PostEditCallback = GetDiagramPostEditCallback(originalObservableDiagram);
                }
            };
        }

        public ObservableCollection<ObservableDiagram> SelectedDiagrams
        {
            get
            {
                return _selectedDiagrams;
            }
            private set
            {
                if (null == value)
                {
                    throw new ArgumentNullException();
                }

                _selectedDiagrams = value;
                RaisePropertyChanged("SelectedDiagrams");
                UpdateCommands();
            }
        }
        private ObservableCollection<ObservableDiagram> _selectedDiagrams;

        public RelayCommand DeleteSelected
        {
            get
            {
                return new RelayCommand(() =>
                {
                    try
                    {
                        Messenger.Default.Send<ConfirmationMessage>(new ConfirmationMessage(String.Format("This will delete the {0} selected diagrams. This cannot be undone. Do you want to continue?", SelectedDiagrams.Count), (confirmed) =>
                        {
                            if (confirmed)
                            {
                                List<ObservableDiagram> itemsToDelete = new List<ObservableDiagram>(SelectedDiagrams);

                                DiagramCollection collection = Library.Get(Path, Name);
                                foreach (ObservableDiagram od in itemsToDelete)
                                {
                                    collection.Remove(od.Diagram);
                                    Diagrams.Remove(od);
                                }
                                SelectedDiagrams.Clear();
                            }
                        }));
                    }
                    catch (Exception ex)
                    {
                        ExceptionUtils.HandleException(ex);
                    }
                }, () =>
                {
                    return SelectedDiagrams.Count > 0;
                });
            }
        }

        public RelayCommand ExportSelected
        {
            get
            {
                return new RelayCommand(() =>
                {
                    try
                    {
                        Messenger.Default.Send<ShowDiagramExportMessage>(new ShowDiagramExportMessage(SelectedDiagrams, Name));
                    }
                    catch (Exception ex)
                    {
                        ExceptionUtils.HandleException(ex);
                    }
                }, () =>
                {
                    return SelectedDiagrams.Count > 0;
                });
            }
        }

        private void SelectedDiagrams_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            UpdateCommands();
        }

        internal DiagramLibrary Library { get; private set; }

        private bool firstLoad = true;

        public ObservableDiagramLibraryNode(string path, string name, DiagramLibrary library) : base()
        {
            if (null == library)
            {
                throw new ArgumentNullException("library");
            }

            Path = path;
            Name = name;

            Library = library;

            Diagrams = new ObservableCollection<ObservableDiagram>();

            SelectedDiagrams = new ObservableCollection<ObservableDiagram>();

            SelectedDiagrams.CollectionChanged += SelectedDiagrams_CollectionChanged;
        }

        private void UpdateCommands()
        {
            RaisePropertyChanged("EditSelected");
            RaisePropertyChanged("DeleteSelected");
            RaisePropertyChanged("ExportSelected");
        }
    }
}
