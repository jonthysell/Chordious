// 
// OptionsViewModel.cs
//  
// Author:
//       Jon Thysell <thysell@gmail.com>
// 
// Copyright (c) 2015, 2016, 2017 Jon Thysell <http://jonthysell.com>
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

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using com.jonthysell.Chordious.Core.Legacy;

using com.jonthysell.Chordious.Core.ViewModel.Resources;

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
                string title = Strings.OptionsTitle;
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

        #region Settings

        public string SettingsGroupLabel
        {
            get
            {
                return Strings.OptionsSettingsGroupLabel;
            }
        }

        public string SettingsAdvancedGroupLabel
        {
            get
            {
                return Strings.OptionsSettingsAdvancedGroupLabel;
            }
        }

        public string ShowAdvancedSettingsLabel
        {
            get
            {
                return Strings.OptionsShowAdvancedSettingsLabel;
            }
        }

        public string ShowAdvancedSettingsToolTip
        {
            get
            {
                return Strings.OptionsShowAdvancedSettingsToolTip;
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
                        Messenger.Default.Send<ConfirmationMessage>(new ConfirmationMessage(Strings.OptionsShowAdvancedSettingsPromptMessage, (confirmed) =>
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

        public string ResetUserSettingsLabel
        {
            get
            {
                return Strings.OptionsResetUserSettingsLabel;
            }
        }

        public string ResetUserSettingsToolTip
        {
            get
            {
                return Strings.OptionsResetUserSettingsToolTip;
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
                        Messenger.Default.Send<ConfirmationMessage>(new ConfirmationMessage(Strings.OptionsResetUserSettingsPromptMessage, (confirmed) =>
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

        public string ResetConfirmationsLabel
        {
            get
            {
                return Strings.OptionsResetConfirmationsLabel;
            }
        }

        public string ResetConfirmationsToolTip
        {
            get
            {
                return Strings.OptionsResetConfirmationsToolTip;
            }
        }

        public RelayCommand ResetConfirmations
        {
            get
            {
                return new RelayCommand(() =>
                {
                    try
                    {
                        Messenger.Default.Send<ConfirmationMessage>(new ConfirmationMessage(Strings.OptionsResetConfirmationsPromptMessage, (confirmed) =>
                        {
                            if (confirmed)
                            {
                                ClearConfirmations();
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

        #endregion

        #region Styles

        public string StylesGroupLabel
        {
            get
            {
                return Strings.OptionsStylesGroupLabel;
            }
        }

        public string StylesBasicGroupLabel
        {
            get
            {
                return Strings.OptionsStylesBasicGroupLabel;
            }
        }

        public string ShowStyleEditorLabel
        {
            get
            {
                return Strings.OptionsShowStyleEditorLabel;
            }
        }

        public string ShowStyleEditorToolTip
        {
            get
            {
                return Strings.OptionsShowStyleEditorToolTip;
            }
        }

        public RelayCommand ShowStyleEditor
        {
            get
            {
                if (null == _userStyle)
                {
                    _userStyle = new ObservableDiagramStyle(StyleBuffer.Parent);
                }
                return _userStyle.ShowEditor;
            }
        }
        private ObservableDiagramStyle _userStyle;

        public string StylesAdvancedGroupLabel
        {
            get
            {
                return Strings.OptionsStylesAdvancedGroupLabel;
            }
        }

        public string ShowAdvancedStyleLabel
        {
            get
            {
                return Strings.OptionsShowAdvancedStyleLabel;
            }
        }

        public string ShowAdvancedStyleToolTip
        {
            get
            {
                return Strings.OptionsShowAdvancedStyleToolTip;
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
                        Messenger.Default.Send<ConfirmationMessage>(new ConfirmationMessage(Strings.OptionsShowAdvancedStylePromptMessage, (confirmed) =>
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

        public string ResetUserStylesLabel
        {
            get
            {
                return Strings.OptionsResetUserStylesLabel;
            }
        }

        public string ResetUserStylesToolTip
        {
            get
            {
                return Strings.OptionsResetUserStylesToolTip;
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
                        Messenger.Default.Send<ConfirmationMessage>(new ConfirmationMessage(Strings.OptionsResetUserStylesPromptMessage, (confirmed) =>
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

        #endregion

        #region Finders

        public string FindersGroupLabel
        {
            get
            {
                return Strings.OptionsFindersGroupLabel;
            }
        }

        public string FindersManagersGroupLabel
        {
            get
            {
                return Strings.OptionsFindersManagersGroupLabel;
            }
        }

        public string ShowInstrumentManagerLabel
        {
            get
            {
                return Strings.ShowInstrumentManagerLabel;
            }
        }

        public string ShowInstrumentManagerToolTip
        {
            get
            {
                return Strings.ShowInstrumentManagerToolTip;
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

        public string ShowChordQualityManagerLabel
        {
            get
            {
                return Strings.ShowChordQualityManagerLabel;
            }
        }

        public string ShowChordQualityManagerToolTip
        {
            get
            {
                return Strings.ShowChordQualityManagerToolTip;
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

        public string ShowScaleManagerLabel
        {
            get
            {
                return Strings.ShowScaleManagerLabel;
            }
        }

        public string ShowScaleManagerToolTip
        {
            get
            {
                return Strings.ShowScaleManagerToolTip;
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

        public string FindersSearchDefaultsGroupLabel
        {
            get
            {
                return Strings.OptionsFindersSearchDefaultsGroupLabel;
            }
        }

        public string ResetChordFinderDefaultsLabel
        {
            get
            {
                return Strings.OptionsResetChordFinderDefaultsLabel;
            }
        }

        public string ResetChordFinderDefaultsToolTip
        {
            get
            {
                return Strings.OptionsResetChordFinderDefaultsToolTip;
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
                        Messenger.Default.Send<ConfirmationMessage>(new ConfirmationMessage(Strings.OptionsResetChordFinderDefaultsPromptMessage, (confirmed) =>
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

        public string ResetScaleFinderDefaultsLabel
        {
            get
            {
                return Strings.OptionsResetScaleFinderDefaultsLabel;
            }
        }

        public string ResetScaleFinderDefaultsToolTip
        {
            get
            {
                return Strings.OptionsResetScaleFinderDefaultsToolTip;
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
                        Messenger.Default.Send<ConfirmationMessage>(new ConfirmationMessage(Strings.OptionsResetScaleFinderDefaultsPromptMessage, (confirmed) =>
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

        #endregion

        #region Config

        public string ConfigGroupLabel
        {
            get
            {
                return Strings.OptionsConfigGroupLabel;
            }
        }

        public string ConfigImportExportGroupLabel
        {
            get
            {
                return Strings.OptionsConfigImportExportGroupLabel;
            }
        }

        public string ShowConfigImportLabel
        {
            get
            {
                return Strings.OptionsShowConfigImportLabel;
            }
        }

        public string ShowConfigImportToolTip
        {
            get
            {
                return Strings.OptionsShowConfigImportToolTip;
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

        public string ShowConfigExportLabel
        {
            get
            {
                return Strings.OptionsShowConfigExportLabel;
            }
        }

        public string ShowConfigExportToolTip
        {
            get
            {
                return Strings.OptionsShowConfigExportToolTip;
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

        public string ConfigLegacyGroupLabel
        {
            get
            {
                return Strings.OptionsConfigLegacyGroupLabel;
            }
        }

        public string LegacyImportLabel
        {
            get
            {
                return Strings.OptionsLegacyImportLabel;
            }
        }

        public string LegacyImportToolTip
        {
            get
            {
                return Strings.OptionsLegacyImportToolTip;
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
                            Messenger.Default.Send<PromptForTextMessage>(new PromptForTextMessage(Strings.OptionsLegacyImportNewCollectionPrompt, proposedName, (name) =>
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

        #endregion

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

        private void ClearConfirmations()
        {
            SettingsBuffer.ClearByPrefix("confirmation.");
            SettingsBuffer.Parent.ClearByPrefix("confirmation.");

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
        }
    }
}
