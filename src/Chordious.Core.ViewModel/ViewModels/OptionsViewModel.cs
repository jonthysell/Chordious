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
        public static AppViewModel AppVM
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

        public static string UserConfigPath
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

        public static string SettingsGroupLabel
        {
            get
            {
                return Strings.OptionsSettingsGroupLabel;
            }
        }

        public static string SettingsAdvancedGroupLabel
        {
            get
            {
                return Strings.OptionsSettingsAdvancedGroupLabel;
            }
        }

        public static string ShowAdvancedSettingsLabel
        {
            get
            {
                return Strings.OptionsShowAdvancedSettingsLabel;
            }
        }

        public static string ShowAdvancedSettingsToolTip
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
                return _showAdvancedSettings ??= new RelayCommand(() =>
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
                });
            }
        }
        private RelayCommand _showAdvancedSettings;

        public static string ResetUserSettingsLabel
        {
            get
            {
                return Strings.OptionsResetUserSettingsLabel;
            }
        }

        public static string ResetUserSettingsToolTip
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
                return _resetUserSettings ??= new RelayCommand(() =>
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
                });
            }
        }
        private RelayCommand _resetUserSettings;

        public static string ResetConfirmationsLabel
        {
            get
            {
                return Strings.OptionsResetConfirmationsLabel;
            }
        }

        public static string ResetConfirmationsToolTip
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
                return _resetConfirmations ??= new RelayCommand(() =>
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
                });
            }
        }
        private RelayCommand _resetConfirmations;

        #endregion

        #region Styles

        public static string StylesGroupLabel
        {
            get
            {
                return Strings.OptionsStylesGroupLabel;
            }
        }

        public static string StylesBasicGroupLabel
        {
            get
            {
                return Strings.OptionsStylesBasicGroupLabel;
            }
        }

        public static string ShowStyleEditorLabel
        {
            get
            {
                return Strings.OptionsShowStyleEditorLabel;
            }
        }

        public static string ShowStyleEditorToolTip
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
                if (_userStyle is null)
                {
                    _userStyle = new ObservableDiagramStyle(StyleBuffer.Parent);
                }
                return _userStyle.ShowEditor;
            }
        }
        private ObservableDiagramStyle _userStyle;

        public static string StylesAdvancedGroupLabel
        {
            get
            {
                return Strings.OptionsStylesAdvancedGroupLabel;
            }
        }

        public static string ShowAdvancedStyleLabel
        {
            get
            {
                return Strings.OptionsShowAdvancedStyleLabel;
            }
        }

        public static string ShowAdvancedStyleToolTip
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
                return _showAdvancedStyle ??= new RelayCommand(() =>
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
                });
            }
        }
        private RelayCommand _showAdvancedStyle;

        public static string ResetUserStylesLabel
        {
            get
            {
                return Strings.OptionsResetUserStylesLabel;
            }
        }

        public static string ResetUserStylesToolTip
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
                return _resetUserStyles ??= new RelayCommand(() =>
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
                });
            }
        }
        private RelayCommand _resetUserStyles;

        #endregion

        #region Finders

        public static string FindersGroupLabel
        {
            get
            {
                return Strings.OptionsFindersGroupLabel;
            }
        }

        public static string FindersManagersGroupLabel
        {
            get
            {
                return Strings.OptionsFindersManagersGroupLabel;
            }
        }

        public static string ShowInstrumentManagerLabel
        {
            get
            {
                return Strings.OptionsShowInstrumentManagerLabel;
            }
        }

        public static string ShowInstrumentManagerToolTip
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
                return _showInstrumentManager ??= new RelayCommand(() =>
                {
                    try
                    {
                        StrongReferenceMessenger.Default.Send(new ShowInstrumentManagerMessage());
                    }
                    catch (Exception ex)
                    {
                        ExceptionUtils.HandleException(ex);
                    }
                });
            }
        }
        private RelayCommand _showInstrumentManager;

        public static string ShowChordQualityManagerLabel
        {
            get
            {
                return Strings.OptionsShowChordQualityManagerLabel;
            }
        }

        public static string ShowChordQualityManagerToolTip
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
                return _showChordQualityManager ??= new RelayCommand(() =>
                {
                    try
                    {
                        StrongReferenceMessenger.Default.Send(new ShowChordQualityManagerMessage());
                    }
                    catch (Exception ex)
                    {
                        ExceptionUtils.HandleException(ex);
                    }
                });
            }
        }
        private RelayCommand _showChordQualityManager;

        public static string ShowScaleManagerLabel
        {
            get
            {
                return Strings.OptionsShowScaleManagerLabel;
            }
        }

        public static string ShowScaleManagerToolTip
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
                return _showScaleManager ??= new RelayCommand(() =>
                {
                    try
                    {
                        StrongReferenceMessenger.Default.Send(new ShowScaleManagerMessage());
                    }
                    catch (Exception ex)
                    {
                        ExceptionUtils.HandleException(ex);
                    }
                });
            }
        }
        private RelayCommand _showScaleManager;

        public static string FindersSearchDefaultsGroupLabel
        {
            get
            {
                return Strings.OptionsFindersSearchDefaultsGroupLabel;
            }
        }

        public static string ResetChordFinderDefaultsLabel
        {
            get
            {
                return Strings.OptionsResetChordFinderDefaultsLabel;
            }
        }

        public static string ResetChordFinderDefaultsToolTip
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
                return _resetChordFinderDefaults ??= new RelayCommand(() =>
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
                });
            }
        }
        private RelayCommand _resetChordFinderDefaults;

        public static string ResetScaleFinderDefaultsLabel
        {
            get
            {
                return Strings.OptionsResetScaleFinderDefaultsLabel;
            }
        }

        public static string ResetScaleFinderDefaultsToolTip
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
                return _resetScaleFinderDefaults ??= new RelayCommand(() =>
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
                });
            }
        }
        private RelayCommand _resetScaleFinderDefaults;

        #endregion

        #region Config

        public static string ConfigGroupLabel
        {
            get
            {
                return Strings.OptionsConfigGroupLabel;
            }
        }

        public static string ConfigImportExportGroupLabel
        {
            get
            {
                return Strings.OptionsConfigImportExportGroupLabel;
            }
        }

        public static string ShowConfigImportLabel
        {
            get
            {
                return Strings.OptionsShowConfigImportLabel;
            }
        }

        public static string ShowConfigImportToolTip
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
                return _showConfigImport ??= new RelayCommand(() =>
                {
                    try
                    {
                        StrongReferenceMessenger.Default.Send(new PromptForConfigInputStreamMessage((inputStream) =>
                        {
                            try
                            {
                                if (inputStream is not null)
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
                });
            }
        }
        private RelayCommand _showConfigImport;

        public static string ShowConfigExportLabel
        {
            get
            {
                return Strings.OptionsShowConfigExportLabel;
            }
        }

        public static string ShowConfigExportToolTip
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
                return _showConfigExport ??= new RelayCommand(() =>
                {
                    try
                    {
                        StrongReferenceMessenger.Default.Send(new ShowConfigExportMessage());
                    }
                    catch (Exception ex)
                    {
                        ExceptionUtils.HandleException(ex);
                    }
                });
            }
        }
        private RelayCommand _showConfigExport;

        public static string ConfigLegacyGroupLabel
        {
            get
            {
                return Strings.OptionsConfigLegacyGroupLabel;
            }
        }

        public static string LegacyImportLabel
        {
            get
            {
                return Strings.OptionsLegacyImportLabel;
            }
        }

        public static string LegacyImportToolTip
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
                return _legacyImport ??= new RelayCommand(() =>
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
                });
            }
        }
        private RelayCommand _legacyImport;

        #endregion

        public RelayCommand Apply
        {
            get
            {
                return _apply ??= new RelayCommand(() =>
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
        private RelayCommand _apply;

        public RelayCommand Accept
        {
            get
            {
                return _accept ??= new RelayCommand(() =>
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
                });
            }
        }
        private RelayCommand _accept;

        public RelayCommand Cancel
        {
            get
            {
                return _cancel ??= new RelayCommand(() =>
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
                });
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
