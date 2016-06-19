// 
// DiagramCollectionSelectorViewModel.cs
//  
// Author:
//       Jon Thysell <thysell@gmail.com>
// 
// Copyright (c) 2016 Jon Thysell <http://jonthysell.com>
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

using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight;

using com.jonthysell.Chordious.Core.ViewModel.Resources;

namespace com.jonthysell.Chordious.Core.ViewModel
{
    public class DiagramCollectionSelectorViewModel : ViewModelBase
    {
        public AppViewModel AppVM
        {
            get
            {
                return AppViewModel.Instance;
            }
        }

        public string Title
        {
            get
            {
                return Strings.DiagramCollectionSelectorTitle;
            }
        }

        public string CollectionNameLabel
        {
            get
            {
                return Strings.DiagramCollectionSelectorCollectionNameLabel;
            }
        }

        public string CollectionNameToolTip
        {
            get
            {
                return Strings.DiagramCollectionSelectorCollectionNameLabelToolTip;
            }
        }

        public string CollectionName
        {
            get
            {
                return _collectionName;
            }
            set
            {
                string lastValue = _collectionName;
                try
                {
                    if (null != value)
                    {
                        _collectionName = value.Trim();
                        ObservableEnums.SortedInsert(CollectionNames, CollectionName);
                    }
                }
                catch (Exception ex)
                {
                    _collectionName = lastValue;
                    ExceptionUtils.HandleException(ex);
                }
                RaisePropertyChanged("CollectionName");
                RaisePropertyChanged("Accept");
            }
        }
        private string _collectionName;

        public ObservableCollection<string> CollectionNames
        {
            get
            {
                if (null == _existingCollections)
                {
                    _existingCollections = GetCollectionNames();
                }
                return _existingCollections;
            }
        }
        private ObservableCollection<string> _existingCollections;

        public RelayCommand Accept
        {
            get
            {
                return new RelayCommand(() =>
                {
                    try
                    {
                        if (null != RequestClose)
                        {
                            RequestClose();
                        }
                        ProcessClose();
                    }
                    catch (Exception ex)
                    {
                        ExceptionUtils.HandleException(ex);
                    }
                }, () =>
                {
                    return !String.IsNullOrWhiteSpace(CollectionName);
                });
            }
        }

        public RelayCommand Cancel
        {
            get
            {
                return new RelayCommand(() =>
                {
                    try
                    {
                        if (null != RequestClose)
                        {
                            RequestClose();
                        }
                    }
                    catch (Exception ex)
                    {
                        ExceptionUtils.HandleException(ex);
                    }
                });
            }
        }

        public event Action RequestClose;

        public Action<string, bool> Callback { get; private set; }

        public DiagramCollectionSelectorViewModel(Action<string, bool> callback, string defaultCollectionName = null)
        {
            if (null == callback)
            {
                throw new ArgumentNullException("callback");
            }

            Callback = callback;

            if (String.IsNullOrWhiteSpace(defaultCollectionName))
            {
                defaultCollectionName = AppVM.UserConfig.DiagramLibrary.GetNewCollectionName();
            }

            ObservableEnums.SortedInsert(CollectionNames, defaultCollectionName);

            CollectionName = defaultCollectionName;
        }

        private void ProcessClose()
        {
            DiagramLibrary library = AppVM.UserConfig.DiagramLibrary;
            DiagramCollection targetCollection = null;

            bool newCollection = false;
            if (!library.TryGet(CollectionName, out targetCollection))
            {
                targetCollection = library.Add(CollectionName);
                newCollection = true;
            }

            Callback(CollectionName, newCollection);
        }

        private ObservableCollection<string> GetCollectionNames()
        {
            ObservableCollection<string> collection = new ObservableCollection<string>();

            foreach (KeyValuePair<string, DiagramCollection> kvp in AppVM.UserConfig.DiagramLibrary.GetAll())
            {
                collection.Add(kvp.Key);
            }

            return collection;
        }
    }
}
