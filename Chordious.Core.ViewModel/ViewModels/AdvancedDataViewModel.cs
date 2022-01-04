// Copyright (c) Jon Thysell <http://jonthysell.com>
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

using Chordious.Core.ViewModel.Resources;

namespace Chordious.Core.ViewModel
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
                string title = Strings.AdvancedDataEditorTitle;

                if (null != LocalBuffer.Parent as ChordiousSettings)
                {
                    title = Strings.AdvancedSettingsEditorTitle;
                }
                else if (null != LocalBuffer.Parent as DiagramStyle)
                {
                    title = Strings.AdvancedStyleEditorTitle;
                }

                if (Dirty)
                {
                    title += "*";
                }

                return title;
            }
        }

        public string KeyHeaderLabel
        {
            get
            {
                return Strings.AdvancedDataEditorKeyHeaderLabel;
            }
        }

        public string ValueHeaderLabel
        {
            get
            {
                return Strings.AdvancedDataEditorValueHeaderLabel;
            }
        }

        public string LevelHeaderLabel
        {
            get
            {
                return Strings.AdvancedDataEditorLevelHeaderLabel;
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
                return _apply ?? (_apply = new RelayCommand(() =>
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
                }));
            }
        }
        private RelayCommand _apply;

        public RelayCommand Accept
        {
            get
            {
                return _accept ?? (_accept = new RelayCommand(() =>
                {
                    try
                    {
                        ApplyChangesOnClose = true;
                        RequestClose?.Invoke();
                    }
                    catch (Exception ex)
                    {
                        ExceptionUtils.HandleException(ex);
                    }
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
                        ApplyChangesOnClose = false;
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

        internal InheritableDictionary LocalBuffer { get; private set; }

        internal string Filter { get; private set; }

        private readonly List<string> _clearedKeys;

        public bool ApplyChangesOnClose
        {
            get
            {
                return _applyChangesOnClose;
            }
            private set
            {
                _applyChangesOnClose = value;
                RaisePropertyChanged(nameof(ApplyChangesOnClose));
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
                RaisePropertyChanged(nameof(Dirty));
                RaisePropertyChanged(nameof(Title));
                Apply.RaiseCanExecuteChanged();
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
                RaisePropertyChanged(nameof(ItemsChanged));
            }
        }
        private bool _itemsChanged = false;

        internal AdvancedDataViewModel(InheritableDictionary inheritableDictionary, string filter)
        {
            if (null == inheritableDictionary)
            {
                throw new ArgumentNullException(nameof(inheritableDictionary));
            }

            _clearedKeys = new List<string>();

            Filter = string.IsNullOrEmpty(filter) ? "" : filter;

            if (null != (inheritableDictionary as ChordiousSettings))
            {
                LocalBuffer = new ChordiousSettings(inheritableDictionary as ChordiousSettings, "Changed");
            }
            else if (null != (inheritableDictionary as DiagramStyle))
            {
                LocalBuffer = new DiagramStyle(inheritableDictionary as DiagramStyle, "Changed");
            }
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
            RaisePropertyChanged(nameof(Items));
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
                if (string.IsNullOrEmpty(value))
                {
                    AdvancedDataVM.Clear(Key);
                }
                else
                {
                    AdvancedDataVM.Set(Key, value);
                }
                RaisePropertyChanged(nameof(Value));
                RaisePropertyChanged(nameof(Level));
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
        public override string Message
        {
            get
            {
                return string.Format(Strings.AdvancedDataValidationErrorMessage, InnerException.Message);
            }
        }

        public new Exception InnerException { get; private set; }

        public AdvancedDataValidationError(Exception exception) : base()
        {
            InnerException = exception;
        }

        public override string ToString()
        {
            return string.Join(Environment.NewLine, Message, InnerException.StackTrace);
        }
    }
}
