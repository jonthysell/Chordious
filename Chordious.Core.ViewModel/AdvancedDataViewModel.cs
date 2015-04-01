// 
// AdvancedDataViewModel.cs
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
using System.Linq;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

using com.jonthysell.Chordious.Core;

namespace com.jonthysell.Chordious.Core.ViewModel
{
    public class AdvancedDataViewModel : ViewModelBase
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
                string title = "Advanced";

                if (null != LocalBuffer.Parent as ChordiousSettings)
                {
                    title += " Settings";
                }
                else if (null != LocalBuffer.Parent as DiagramStyle)
                {
                    title += " Style";
                }

                title += " Editor";

                if (Dirty)
                {
                    title += "*";
                }

                return title;
            }
        }

        public ObservableCollection<AdvancedDataKVLT> Items
        {
            get
            {
                ObservableCollection<AdvancedDataKVLT> collection = new ObservableCollection<AdvancedDataKVLT>();

                foreach (string key in LocalBuffer.AllKeys())
                {
                    collection.Add(new AdvancedDataKVLT(key, this));
                }

                return collection;
            }
        }

        public RelayCommand Apply
        {
            get
            {
                return new RelayCommand(() =>
                {
                    try
                    {
                        ApplyChanges();
                    }
                    catch (Exception ex)
                    {
                        ExceptionUtils.HandleException(ex);
                    }
                }, () =>
                {
                    return Dirty;
                });
            }
        }

        public RelayCommand Accept
        {
            get
            {
                return new RelayCommand(() =>
                {
                    try
                    {
                        ApplyChangesOnClose = true;
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

        public RelayCommand Cancel
        {
            get
            {
                return new RelayCommand(() =>
                {
                    try
                    {
                        ApplyChangesOnClose = false;
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

        internal InheritableDictionary LocalBuffer { get; private set; }

        internal string Filter { get; private set; }

        private List<string> _clearedKeys;

        public bool ApplyChangesOnClose
        {
            get
            {
                return _applyChangesOnClose;
            }
            private set
            {
                _applyChangesOnClose = value;
                RaisePropertyChanged("ApplyChangesOnClose");
            }
        }
        private bool _applyChangesOnClose = false;

        public bool Dirty
        {
            get
            {
                return _dirty;
            }
            private set
            {
                _dirty = value;
                RaisePropertyChanged("Dirty");
                RaisePropertyChanged("Title");
                RaisePropertyChanged("Apply");
            }
        }
        private bool _dirty = false;

        public bool ItemsChanged
        {
            get
            {
                return _itemsChanged;
            }
            private set
            {
                _itemsChanged = value;
                RaisePropertyChanged("ItemsChanged");
            }
        }
        private bool _itemsChanged = false;

        internal AdvancedDataViewModel(InheritableDictionary inheritableDictionary, string filter)
        {
            if (null == inheritableDictionary)
            {
                throw new ArgumentNullException("inheritableDictionary");
            }

            _clearedKeys = new List<string>();

            Filter = String.IsNullOrEmpty(filter) ? "" : filter;

            LocalBuffer = new InheritableDictionary(inheritableDictionary, "Changed");
        }

        public string Get(string key)
        {
            if (_clearedKeys.Contains(key) && !LocalBuffer.LocalKeys().Contains<string>(key))
            {
                return "";
            }
            return LocalBuffer.Get(key);
        }

        public string GetLevel(string key)
        {
            if (_clearedKeys.Contains(key) && !LocalBuffer.LocalKeys().Contains<string>(key))
            {
                return "Cleared";
            }
            return LocalBuffer.GetLevel(key);
        }

        public void Clear(string key)
        {
            LocalBuffer.Clear(key);
            _clearedKeys.Add(key);
            Dirty = true;
        }

        public void Set(string key, string value)
        {
            LocalBuffer.Set(key, value);
            if (_clearedKeys.Contains(key))
            {
                _clearedKeys.Remove(key);
            }
            Dirty = true;
        }

        public bool ProcessClose()
        {
            if (ApplyChangesOnClose)
            {
                ApplyChanges();
            }
            return ItemsChanged;
        }

        private void ApplyChanges()
        {
            LocalBuffer.SetParent();
            ApplyClears();
            LocalBuffer.Clear();
            Dirty = false;
            ItemsChanged = true;
            RaisePropertyChanged("Items");
        }

        private void ApplyClears()
        {
            foreach (string key in _clearedKeys)
            {
                LocalBuffer.Parent.Clear(key, true);
            }
            _clearedKeys.Clear();
        }
    }

    public class AdvancedDataKVLT : ObservableObject
    {
        internal AdvancedDataViewModel AdvancedDataVM { get; private set; }

        public string Key { get; private set; }

        public string Value
        {
            get
            {
                return AdvancedDataVM.Get(Key);
            }
            set
            {
                if (String.IsNullOrEmpty(value))
                {
                    AdvancedDataVM.Clear(Key);
                }
                else
                {
                    AdvancedDataVM.Set(Key, value);
                }
                RaisePropertyChanged("Value");
                RaisePropertyChanged("Level");
            }
        }

        public string Level
        {
            get
            {
                return AdvancedDataVM.GetLevel(Key);
            }
        }

        public AdvancedDataKVLT(string key, AdvancedDataViewModel advancedDataVM)
        {
            Key = key;
            AdvancedDataVM = advancedDataVM;
        }
    }

    public class AdvancedDataValidationError : ChordiousException
    {
        public Exception InnerException { get; private set; }

        public AdvancedDataValidationError(Exception exception) : base()
        {
            InnerException = exception;
        }
    }
}
