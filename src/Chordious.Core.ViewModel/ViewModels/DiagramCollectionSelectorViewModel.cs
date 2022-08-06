// Copyright (c) Jon Thysell <http://jonthysell.com>
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using Chordious.Core.ViewModel.Resources;

namespace Chordious.Core.ViewModel
{
    public class DiagramCollectionSelectorViewModel : ObservableObject
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
                        CollectionNames.SortedInsert(CollectionName);
                    }
                }
                catch (Exception ex)
                {
                    _collectionName = lastValue;
                    ExceptionUtils.HandleException(ex);
                }
                OnPropertyChanged(nameof(CollectionName));
                Accept.NotifyCanExecuteChanged();
            }
        }
        private string _collectionName;

        public ObservableCollection<string> CollectionNames
        {
            get
            {
                return _existingCollections ?? (_existingCollections = GetCollectionNames());
            }
        }
        private ObservableCollection<string> _existingCollections;

        public RelayCommand Accept
        {
            get
            {
                return _accept ?? (_accept = new RelayCommand(() =>
                {
                    try
                    {
                        RequestClose?.Invoke();
                        ProcessClose();
                    }
                    catch (Exception ex)
                    {
                        ExceptionUtils.HandleException(ex);
                    }
                }, () =>
                {
                    return !string.IsNullOrWhiteSpace(CollectionName);
                }));
            }
        }
        private RelayCommand _accept;

        public RelayCommand Cancel
        {
            get
            {
                return _cancel ?? (_cancel = new RelayCommand(() =>
                {
                    try
                    {
                        RequestClose?.Invoke();
                    }
                    catch (Exception ex)
                    {
                        ExceptionUtils.HandleException(ex);
                    }
                }));
            }
        }
        private RelayCommand _cancel;

        public Action RequestClose;

        public Action<string, bool> Callback { get; private set; }

        public DiagramCollectionSelectorViewModel(Action<string, bool> callback, string defaultCollectionName = null)
        {
            Callback = callback ?? throw new ArgumentNullException(nameof(callback));

            if (string.IsNullOrWhiteSpace(defaultCollectionName))
            {
                defaultCollectionName = AppVM.UserConfig.DiagramLibrary.GetNewCollectionName();
            }

            defaultCollectionName = defaultCollectionName.Trim();

            _collectionName = defaultCollectionName;
        }

        private void ProcessClose()
        {
            DiagramLibrary library = AppVM.UserConfig.DiagramLibrary;

            bool newCollection = false;
            if (!library.TryGet(CollectionName, out DiagramCollection targetCollection))
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
