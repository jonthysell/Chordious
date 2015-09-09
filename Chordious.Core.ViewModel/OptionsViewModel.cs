// 
// OptionsViewModel.cs
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
using System.IO;
using System.Collections.ObjectModel;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;

using com.jonthysell.Chordious.Core;
using com.jonthysell.Chordious.Core.Legacy;

namespace com.jonthysell.Chordious.Core.ViewModel
{
    public class OptionsViewModel : ViewModelBase
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
                string title = "Options";
                if (Dirty)
                {
                    title += "*";
                }
                return title;
            }
        }

        public string UserConfigPath
        {
            get
            {
                return AppVM.UserConfigPath;
            }
        }

        public bool IsIdle
        {
            get
            {
                return _isIdle;
            }
            protected set
            {
                _isIdle = value;
                RaisePropertyChanged("IsIdle");
            }
        }
        private bool _isIdle;

        public int NewDiagramNumFrets
        {
            get
            {

                return StyleBuffer.NewDiagramNumFretsGet();
            }
            set
            {
                try
                {
                    StyleBuffer.NewDiagramNumFretsSet(value);
                    Dirty = true;
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
                finally
                {
                    RaisePropertyChanged("NewDiagramNumFrets");
                }
            }
        }

        public int NewDiagramNumStrings
        {
            get
            {

                return StyleBuffer.NewDiagramNumStringsGet();
            }
            set
            {
                try
                {
                    StyleBuffer.NewDiagramNumStringsSet(value);
                    Dirty = true;
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
                finally
                {
                    RaisePropertyChanged("NewDiagramNumStrings");
                }
            }
        }

        public RelayCommand ShowAdvancedSettings
        {
            get
            {
                return new RelayCommand(() =>
                {
                    try
                    {
                        Messenger.Default.Send<ConfirmationMessage>(new ConfirmationMessage("Careful! You can cause problems with Chordious if you don't know what you're doing in here. Do you want to continue?", (confirmed) =>
                        {
                            if (confirmed)
                            {
                                Messenger.Default.Send<ShowAdvancedDataMessage>(new ShowAdvancedDataMessage(SettingsBuffer, "", (itemsChanged) =>
                                {
                                    try
                                    {
                                        if (itemsChanged)
                                        {
                                            Dirty = true;
                                            RefreshProperties();
                                            AdvancedSettingsClean = true;
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        ExceptionUtils.HandleException(new AdvancedDataValidationError(ex));
                                        AdvancedSettingsClean = false;
                                    }
                                }));
                            }
                        }));
                    }
                    catch (Exception ex)
                    {
                        ExceptionUtils.HandleException(ex);
                    }
                });
            }
        }

        public RelayCommand ResetUserSettings
        {
            get
            {
                return new RelayCommand(() =>
                {
                    try
                    {
                        Messenger.Default.Send<ConfirmationMessage>(new ConfirmationMessage("This will revert all of your user settings to the defaults. This cannot be undone. Do you want to continue?", (confirmed) =>
                        {
                            if (confirmed)
                            {
                                ClearUserSettings();
                                RefreshProperties();
                            }
                        }));
                    }
                    catch (Exception ex)
                    {
                        ExceptionUtils.HandleException(ex);
                    }
                });
            }
        }

        public RelayCommand ShowAdvancedStyle
        {
            get
            {
                return new RelayCommand(() =>
                {
                    try
                    {
                        Messenger.Default.Send<ConfirmationMessage>(new ConfirmationMessage("Careful! You can cause problems with Chordious if you don't know what you're doing in here. Do you want to continue?", (confirmed) =>
                        {
                            if (confirmed)
                            {
                                Messenger.Default.Send<ShowAdvancedDataMessage>(new ShowAdvancedDataMessage(StyleBuffer, "", (itemsChanged) =>
                                {
                                    try
                                    {
                                        if (itemsChanged)
                                        {
                                            Dirty = true;
                                            RefreshProperties();
                                            AdvancedStyleClean = true;
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        ExceptionUtils.HandleException(new AdvancedDataValidationError(ex));
                                        AdvancedStyleClean = false;
                                    }
                                }));
                            }
                        }));
                    }
                    catch (Exception ex)
                    {
                        ExceptionUtils.HandleException(ex);
                    }
                });
            }
        }

        public RelayCommand ResetUserStyles
        {
            get
            {
                return new RelayCommand(() =>
                {
                    try
                    {
                        Messenger.Default.Send<ConfirmationMessage>(new ConfirmationMessage("This will revert your base user styles to the defaults. This cannot be undone. Do you want to continue?", (confirmed) =>
                        {
                            if (confirmed)
                            {
                                ClearUserStyles();
                                RefreshProperties();
                            }
                        }));
                    }
                    catch (Exception ex)
                    {
                        ExceptionUtils.HandleException(ex);
                    }
                });
            }
        }

        public RelayCommand ShowInstrumentManager
        {
            get
            {
                return new RelayCommand(() =>
                {
                    try
                    {
                        Messenger.Default.Send<ShowInstrumentManagerMessage>(new ShowInstrumentManagerMessage());
                    }
                    catch (Exception ex)
                    {
                        ExceptionUtils.HandleException(ex);
                    }
                });
            }
        }

        public RelayCommand ShowChordQualityManager
        {
            get
            {
                return new RelayCommand(() =>
                {
                    try
                    {
                        Messenger.Default.Send<ShowChordQualityManagerMessage>(new ShowChordQualityManagerMessage());
                    }
                    catch (Exception ex)
                    {
                        ExceptionUtils.HandleException(ex);
                    }
                });
            }
        }

        public RelayCommand ShowScaleManager
        {
            get
            {
                return new RelayCommand(() =>
                {
                    try
                    {
                        Messenger.Default.Send<ShowScaleManagerMessage>(new ShowScaleManagerMessage());
                    }
                    catch (Exception ex)
                    {
                        ExceptionUtils.HandleException(ex);
                    }
                });
            }
        }

        public RelayCommand ResetChordFinderDefaults
        {
            get
            {
                return new RelayCommand(() =>
                {
                    try
                    {
                        Messenger.Default.Send<ConfirmationMessage>(new ConfirmationMessage("This will reset your default search parameters to the built-in values. This cannot be undone. Do you want to continue?", (confirmed) =>
                        {
                            if (confirmed)
                            {
                                ResetByPrefix("chordfinder");
                                RefreshProperties();
                            }
                        }));
                    }
                    catch (Exception ex)
                    {
                        ExceptionUtils.HandleException(ex);
                    }
                });
            }
        }

        public RelayCommand ResetScaleFinderDefaults
        {
            get
            {
                return new RelayCommand(() =>
                {
                    try
                    {
                        Messenger.Default.Send<ConfirmationMessage>(new ConfirmationMessage("This will reset your default search parameters to the built-in values. This cannot be undone. Do you want to continue?", (confirmed) =>
                        {
                            if (confirmed)
                            {
                                ResetByPrefix("scalefinder");
                                RefreshProperties();
                            }
                        }));
                    }
                    catch (Exception ex)
                    {
                        ExceptionUtils.HandleException(ex);
                    }
                });
            }
        }

        public RelayCommand ShowConfigImport
        {
            get
            {
                return new RelayCommand(() =>
                {
                    try
                    {
                        Messenger.Default.Send<PromptForConfigInputStreamMessage>(new PromptForConfigInputStreamMessage((inputStream) =>
                        {
                            try
                            {
                                if (null != inputStream)
                                {
                                    Messenger.Default.Send<ShowConfigImportMessage>(new ShowConfigImportMessage(inputStream));
                                }
                            }
                            catch (Exception ex)
                            {
                                ExceptionUtils.HandleException(ex);
                            }
                        }));                        
                    }
                    catch (Exception ex)
                    {
                        ExceptionUtils.HandleException(ex);
                    }
                });
            }
        }

        public RelayCommand ShowConfigExport
        {
            get
            {
                return new RelayCommand(() =>
                {
                    try
                    {
                        Messenger.Default.Send<ShowConfigExportMessage>(new ShowConfigExportMessage());
                    }
                    catch (Exception ex)
                    {
                        ExceptionUtils.HandleException(ex);
                    }
                });
            }
        }

        public RelayCommand LegacyImport
        {
            get
            {
                return new RelayCommand(() =>
                {
                    try
                    {
                        DiagramLibrary library = AppVM.UserConfig.DiagramLibrary;
                        Messenger.Default.Send<PromptForLegacyImportMessage>(new PromptForLegacyImportMessage((fileName, inputStream) =>
                        {
                            string proposedName = String.IsNullOrWhiteSpace(fileName) ? library.GetNewCollectionName() : fileName.Trim();
                            Messenger.Default.Send<PromptForTextMessage>(new PromptForTextMessage("Name for the new collection:", proposedName, (name) =>
                            {
                                DiagramCollection importedCollection = ChordDocument.Load(library.Style, inputStream);
                                DiagramCollection newCollection = library.Add(name);

                                newCollection.Add(importedCollection);
                                ItemsChanged = true;
                            }));
                        }));
                    }
                    catch (Exception ex)
                    {
                        ExceptionUtils.HandleException(ex);
                    }
                });
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
                    return Dirty && AdvancedSettingsClean && AdvancedStyleClean;
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
                }, () =>
                {
                    return AdvancedSettingsClean && AdvancedStyleClean;
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

        public bool AdvancedSettingsClean
        {
            get
            {
                return _advancedSettingsClean;
            }
            private set
            {
                _advancedSettingsClean = value;
                RaisePropertyChanged("AdvancedSettingsClean");
                RaisePropertyChanged("Accept");
                RaisePropertyChanged("Apply");
            }
        }
        private bool _advancedSettingsClean;

        public bool AdvancedStyleClean
        {
            get
            {
                return _advancedStyleClean;
            }
            private set
            {
                _advancedStyleClean = value;
                RaisePropertyChanged("AdvancedStyleClean");
                RaisePropertyChanged("Accept");
                RaisePropertyChanged("Apply");
            }
        }
        private bool _advancedStyleClean;

        internal ChordiousSettings SettingsBuffer { get; private set; }
        internal DiagramStyle StyleBuffer { get; private set; }

        public OptionsViewModel()
        {
            SettingsBuffer = new ChordiousSettings(AppVM.UserConfig.ChordiousSettings, "Options");
            StyleBuffer = new DiagramStyle(AppVM.UserConfig.DiagramStyle, "Options");

            AdvancedSettingsClean = true;
            AdvancedStyleClean = true;
        }

        public bool ProcessClose()
        {
            if (ApplyChangesOnClose)
            {
                ApplyChanges();
            }
            return ItemsChanged;
        }

        protected string GetSetting(string key)
        {
            return SettingsBuffer.Get(key);
        }

        protected void SetSetting(string key, object value)
        {
            SettingsBuffer.Set(key, value);
            Dirty = true;
        }

        protected string GetStyle(string key)
        {
            return StyleBuffer.Get(key);
        }

        protected void SetStyle(string key, object value)
        {
            StyleBuffer.Set(key, value);
            Dirty = true;
        }

        private void ApplyChanges()
        {
            SettingsBuffer.SetParent();
            SettingsBuffer.Clear();

            StyleBuffer.SetParent();
            StyleBuffer.Clear();

            Dirty = false;
            ItemsChanged = true;
        }

        private void ClearUserSettings()
        {
            SettingsBuffer.Clear();
            SettingsBuffer.Parent.Clear();

            Dirty = false;
            ItemsChanged = true;
            AdvancedSettingsClean = true;
        }

        private void ClearUserStyles()
        {
            StyleBuffer.Clear();
            StyleBuffer.Parent.Clear();

            Dirty = false;
            ItemsChanged = true;
            AdvancedStyleClean = true;
        }

        protected void ResetByPrefix(string prefix)
        {
            SettingsBuffer.ClearByPrefix(prefix);
            SettingsBuffer.Parent.ClearByPrefix(prefix);

            ItemsChanged = true;
        }

        public virtual void RefreshProperties()
        {
            // refresh all properties
            RaisePropertyChanged("NewDiagramNumFrets");
            RaisePropertyChanged("NewDiagramNumStrings");
        }
    }
}
