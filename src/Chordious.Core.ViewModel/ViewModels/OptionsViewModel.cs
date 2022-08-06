﻿// Copyright (c) Jon Thysell <http://jonthysell.com>
// Licensed under the MIT License.

using System;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

using Chordious.Core.Legacy;

using Chordious.Core.ViewModel.Resources;

namespace Chordious.Core.ViewModel
{
    public class OptionsViewModel : ObservableObject
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
                OnPropertyChanged(nameof(IsIdle));
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
                return _showAdvancedSettings ?? (_showAdvancedSettings = new RelayCommand(() =>
                {
                    try
                    {
                        StrongReferenceMessenger.Default.Send(new ConfirmationMessage(Strings.OptionsShowAdvancedSettingsPromptMessage, (confirmed) =>
                        {
                            try
                            {
                                if (confirmed)
                                {
                                    StrongReferenceMessenger.Default.Send(new ShowAdvancedDataMessage(SettingsBuffer, "", (itemsChanged) =>
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
                }));
            }
        }
        private RelayCommand _showAdvancedSettings;

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
                return _resetUserSettings ?? (_resetUserSettings = new RelayCommand(() =>
                {
                    try
                    {
                        StrongReferenceMessenger.Default.Send(new ConfirmationMessage(Strings.OptionsResetUserSettingsPromptMessage, (confirmed) =>
                        {
                            try
                            {
                                if (confirmed)
                                {
                                    ClearUserSettings();
                                    RefreshProperties();
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
                }));
            }
        }
        private RelayCommand _resetUserSettings;

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
                return _resetConfirmations ?? (_resetConfirmations = new RelayCommand(() =>
                {
                    try
                    {
                        StrongReferenceMessenger.Default.Send(new ConfirmationMessage(Strings.OptionsResetConfirmationsPromptMessage, (confirmed) =>
                        {
                            try
                            {
                                if (confirmed)
                                {
                                    ClearConfirmations();
                                    RefreshProperties();
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
                }));
            }
        }
        private RelayCommand _resetConfirmations;

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
                return _showAdvancedStyle ?? (_showAdvancedStyle = new RelayCommand(() =>
                {
                    try
                    {
                        StrongReferenceMessenger.Default.Send(new ConfirmationMessage(Strings.OptionsShowAdvancedStylePromptMessage, (confirmed) =>
                        {
                            try
                            {
                                if (confirmed)
                                {
                                    StrongReferenceMessenger.Default.Send(new ShowAdvancedDataMessage(StyleBuffer, "", (itemsChanged) =>
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
                }));
            }
        }
        private RelayCommand _showAdvancedStyle;

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
                return _resetUserStyles ?? (_resetUserStyles = new RelayCommand(() =>
                {
                    try
                    {
                        StrongReferenceMessenger.Default.Send(new ConfirmationMessage(Strings.OptionsResetUserStylesPromptMessage, (confirmed) =>
                        {
                            try
                            {
                                if (confirmed)
                                {
                                    ClearUserStyles();
                                    RefreshProperties();
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
                }));
            }
        }
        private RelayCommand _resetUserStyles;

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
                return Strings.OptionsShowInstrumentManagerLabel;
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
                return _showInstrumentManager ?? (_showInstrumentManager = new RelayCommand(() =>
                {
                    try
                    {
                        StrongReferenceMessenger.Default.Send(new ShowInstrumentManagerMessage());
                    }
                    catch (Exception ex)
                    {
                        ExceptionUtils.HandleException(ex);
                    }
                }));
            }
        }
        private RelayCommand _showInstrumentManager;

        public string ShowChordQualityManagerLabel
        {
            get
            {
                return Strings.OptionsShowChordQualityManagerLabel;
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
                return _showChordQualityManager ?? (_showChordQualityManager = new RelayCommand(() =>
                {
                    try
                    {
                        StrongReferenceMessenger.Default.Send(new ShowChordQualityManagerMessage());
                    }
                    catch (Exception ex)
                    {
                        ExceptionUtils.HandleException(ex);
                    }
                }));
            }
        }
        private RelayCommand _showChordQualityManager;

        public string ShowScaleManagerLabel
        {
            get
            {
                return Strings.OptionsShowScaleManagerLabel;
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
                return _showScaleManager ?? (_showScaleManager = new RelayCommand(() =>
                {
                    try
                    {
                        StrongReferenceMessenger.Default.Send(new ShowScaleManagerMessage());
                    }
                    catch (Exception ex)
                    {
                        ExceptionUtils.HandleException(ex);
                    }
                }));
            }
        }
        private RelayCommand _showScaleManager;

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
                return _resetChordFinderDefaults ?? (_resetChordFinderDefaults = new RelayCommand(() =>
                {
                    try
                    {
                        StrongReferenceMessenger.Default.Send(new ConfirmationMessage(Strings.OptionsResetChordFinderDefaultsPromptMessage, (confirmed) =>
                        {
                            try
                            {
                                if (confirmed)
                                {
                                    ResetByPrefix("chordfinder");
                                    RefreshProperties();
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
                }));
            }
        }
        private RelayCommand _resetChordFinderDefaults;

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
                return _resetScaleFinderDefaults ?? (_resetScaleFinderDefaults = new RelayCommand(() =>
                {
                    try
                    {
                        StrongReferenceMessenger.Default.Send(new ConfirmationMessage(Strings.OptionsResetScaleFinderDefaultsPromptMessage, (confirmed) =>
                        {
                            try
                            {
                                if (confirmed)
                                {
                                    ResetByPrefix("scalefinder");
                                    RefreshProperties();
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
                }));
            }
        }
        private RelayCommand _resetScaleFinderDefaults;

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
                return _showConfigImport ?? (_showConfigImport = new RelayCommand(() =>
                {
                    try
                    {
                        StrongReferenceMessenger.Default.Send(new PromptForConfigInputStreamMessage((inputStream) =>
                        {
                            try
                            {
                                if (null != inputStream)
                                {
                                    StrongReferenceMessenger.Default.Send(new ShowConfigImportMessage(inputStream));
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
                }));
            }
        }
        private RelayCommand _showConfigImport;

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
                return _showConfigExport ?? (_showConfigExport = new RelayCommand(() =>
                {
                    try
                    {
                        StrongReferenceMessenger.Default.Send(new ShowConfigExportMessage());
                    }
                    catch (Exception ex)
                    {
                        ExceptionUtils.HandleException(ex);
                    }
                }));
            }
        }
        private RelayCommand _showConfigExport;

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
                return _legacyImport ?? (_legacyImport = new RelayCommand(() =>
                {
                    try
                    {
                        DiagramLibrary library = AppVM.UserConfig.DiagramLibrary;
                        StrongReferenceMessenger.Default.Send(new PromptForLegacyImportMessage((fileName, inputStream) =>
                        {
                            try
                            {
                                string proposedName = string.IsNullOrWhiteSpace(fileName) ? library.GetNewCollectionName() : fileName.Trim();
                                StrongReferenceMessenger.Default.Send(new PromptForTextMessage(Strings.OptionsLegacyImportNewCollectionPrompt, proposedName, (name) =>
                                {
                                    try
                                    {
                                        DiagramCollection importedCollection = ChordDocument.Load(library.Style, inputStream);
                                        DiagramCollection newCollection = library.Add(name);

                                        newCollection.Add(importedCollection);
                                        ItemsChanged = true;
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
                        }));
                    }
                    catch (Exception ex)
                    {
                        ExceptionUtils.HandleException(ex);
                    }
                }));
            }
        }
        private RelayCommand _legacyImport;

        #endregion

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
                    return Dirty && AdvancedSettingsClean && AdvancedStyleClean;
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
                }, () =>
                {
                    return AdvancedSettingsClean && AdvancedStyleClean;
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

        public bool ApplyChangesOnClose
        {
            get
            {
                return _applyChangesOnClose;
            }
            private set
            {
                _applyChangesOnClose = value;
                OnPropertyChanged(nameof(ApplyChangesOnClose));
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
                OnPropertyChanged(nameof(Dirty));
                OnPropertyChanged(nameof(Title));
                Apply.NotifyCanExecuteChanged();
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
                OnPropertyChanged(nameof(ItemsChanged));
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
                OnPropertyChanged(nameof(AdvancedSettingsClean));
                Accept.NotifyCanExecuteChanged();
                Apply.NotifyCanExecuteChanged();
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
                OnPropertyChanged(nameof(AdvancedStyleClean));
                Accept.NotifyCanExecuteChanged();
                Apply.NotifyCanExecuteChanged();
            }
        }
        private bool _advancedStyleClean;

        internal ChordiousSettings SettingsBuffer { get; private set; }
        internal DiagramStyle StyleBuffer { get; private set; }

        public OptionsViewModel()
        {
            SettingsBuffer = new ChordiousSettings(AppVM.UserConfig.ChordiousSettings, "Options");
            StyleBuffer = new DiagramStyle(AppVM.UserConfig.DiagramStyle, "Options");

            _advancedSettingsClean = true;
            _advancedStyleClean = true;
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
