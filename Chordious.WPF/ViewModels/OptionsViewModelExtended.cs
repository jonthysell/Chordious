// Copyright (c) Jon Thysell <http://jonthysell.com>
// Licensed under the MIT License.

using System;
using System.Collections.ObjectModel;

using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;

using Chordious.Core.ViewModel;

using Chordious.WPF.Resources;

namespace Chordious.WPF
{
    public class OptionsViewModelExtended : OptionsViewModel, IIdle
    {
        #region Backgrounds

        public string SettingsBackgroundGroupLabel
        {
            get
            {
                return Strings.OptionsSettingsBackgroundGroupLabel;
            }
        }

        public string SelectedRenderBackgroundLabel
        {
            get
            {
                return Strings.OptionsSelectedRendererBackgroundLabel;
            }
        }

        public string SelectedRenderBackgroundToolTip
        {
            get
            {
                return Strings.OptionsSelectedRendererBackgroundToolTip;
            }
        }

        public int SelectedRenderBackgroundIndex
        {
            get
            {
                return (int)RenderBackground;
            }
            set
            {
                RenderBackground = (Background)(value);
            }
        }

        public ObservableCollection<string> RenderBackgrounds
        {
            get
            {
                return _renderBackgrounds ?? (_renderBackgrounds = ImageUtils.GetBackgrounds());
            }
        }
        private ObservableCollection<string> _renderBackgrounds;

        private Background RenderBackground
        {
            get
            {

                if (Enum.TryParse(GetSetting("app.renderbackground"), out Background result))
                {
                    return result;
                }

                return Background.None;
            }
            set
            {
                SetSetting("app.renderbackground", value);
                RaisePropertyChanged(nameof(SelectedRenderBackgroundIndex));
            }
        }

        public string SelectedEditorRenderBackgroundLabel
        {
            get
            {
                return Strings.OptionsSelectedEditorRendererBackgroundLabel;
            }
        }

        public string SelectedEditorRenderBackgroundToolTip
        {
            get
            {
                return Strings.OptionsSelectedEditorRendererBackgroundToolTip;
            }
        }

        public int SelectedEditorRenderBackgroundIndex
        {
            get
            {
                return (int)EditorRenderBackground;
            }
            set
            {
                EditorRenderBackground = (Background)(value);
            }
        }

        public ObservableCollection<string> EditorRenderBackgrounds
        {
            get
            {
                return _editorRenderBackgrounds ?? (_editorRenderBackgrounds = ImageUtils.GetBackgrounds());
            }
        }
        private ObservableCollection<string> _editorRenderBackgrounds;

        private Background EditorRenderBackground
        {
            get
            {

                if (Enum.TryParse(GetSetting("diagrameditor.renderbackground"), out Background result))
                {
                    return result;
                }

                return Background.None;
            }
            set
            {
                SetSetting("diagrameditor.renderbackground", value);
                RaisePropertyChanged(nameof(SelectedEditorRenderBackgroundIndex));
            }
        }

        #endregion

        #region Integrations

        public string SettingsIntegrationGroupLabel
        {
            get
            {
                return Strings.OptionsSettingsIntegrationGroupLabel;
            }
        }

        public string EnhancedCopyLabel
        {
            get
            {
                return Strings.OptionsEnhancedCopyLabel;
            }
        }

        public string EnhancedCopyToolTip
        {
            get
            {
                return Strings.OptionsEnhancedCopyToolTip;
            }
        }

        public bool EnhancedCopy
        {
            get
            {

                if (bool.TryParse(GetSetting("integration.enhancedcopy"), out bool result))
                {
                    return result;
                }

                return false;
            }
            set
            {
                SetSetting("integration.enhancedcopy", value);
                RaisePropertyChanged(nameof(EnhancedCopy));
            }
        }

        public string OpenTempFolderLabel
        {
            get
            {
                return Strings.OptionsOpenTempFolderLabel;
            }
        }

        public string OpenTempFolderToolTip
        {
            get
            {
                return Strings.OptionsOpenTempFolderToolTip;
            }
        }

        public RelayCommand OpenTempFolder
        {
            get
            {
                return _openTempFolder ?? (_openTempFolder = new RelayCommand(() =>
                {
                    try
                    {
                        string path = IntegrationUtils.GetTempPathAndCreateIfNecessary();
                        Messenger.Default.Send(new LaunchUrlMessage(path));
                    }
                    catch (Exception ex)
                    {
                        ExceptionUtils.HandleException(ex);
                    }
                }));
            }
        }
        private RelayCommand _openTempFolder;

        #endregion

        #region Updates

        public bool UpdateEnabled
        {
            get
            {
                return UpdateUtils.UpdateEnabled;
            }
        }

        public string UpdatesGroupLabel
        {
            get
            {
                return Strings.OptionsUpdatesGroupLabel;
            }
        }

        public string SelectedReleaseChannelLabel
        {
            get
            {
                return Strings.OptionsSelectedReleaseChannelLabel;
            }
        }

        public string SelectedReleaseChannelToolTip
        {
            get
            {
                return Strings.OptionsSelectedReleaseChannelToolTip;
            }
        }

        public int SelectedReleaseChannelIndex
        {
            get
            {
                return (int)ReleaseChannel;
            }
            set
            {
                ReleaseChannel = (ReleaseChannel)(value);
            }
        }

        public ObservableCollection<string> ReleaseChannels
        {
            get
            {
                return _releaseChannels ?? (_releaseChannels = GetReleaseChannels());
            }
        }
        private ObservableCollection<string> _releaseChannels;

        private ReleaseChannel ReleaseChannel
        {
            get
            {

                if (Enum.TryParse(GetSetting("app.releasechannel"), out ReleaseChannel result))
                {
                    return result;
                }

                return ReleaseChannel.Official;
            }
            set
            {
                SetSetting("app.releasechannel", value);
                RaisePropertyChanged(nameof(SelectedReleaseChannelIndex));
            }
        }

        public string CheckUpdateOnStartLabel
        {
            get
            {
                return Strings.OptionsCheckUpdateOnStartLabel;
            }
        }

        public string CheckUpdateOnStartToolTip
        {
            get
            {
                return Strings.OptionsCheckUpdateOnStartToolTip;
            }
        }

        public bool CheckUpdateOnStart
        {
            get
            {

                if (bool.TryParse(GetSetting("app.checkupdateonstart"), out bool result))
                {
                    return result;
                }

                return false;
            }
            set
            {
                SetSetting("app.checkupdateonstart", value);
                RaisePropertyChanged(nameof(CheckUpdateOnStart));
            }
        }

        public string LastUpdateCheckLabel
        {
            get
            {
                return Strings.OptionsLastUpdateCheckLabel;
            }
        }

        public string LastUpdateCheckToolTip
        {
            get
            {
                return Strings.OptionsLastUpdateCheckToolTip;
            }
        }

        public string LastUpdateCheck
        {
            get
            {
                DateTime lastCheck = UpdateUtils.LastUpdateCheck;

                if (lastCheck == DateTime.MinValue)
                {
                    return Strings.OptionsLastUpdateCheckNeverCheckedValue;
                }

                return lastCheck.ToLocalTime().ToString("G");
            }
        }

        public string CheckForUpdatesAsyncLabel
        {
            get
            {
                return Strings.OptionsCheckForUpdatesAsyncLabel;
            }
        }

        public string CheckForUpdatesAsyncToolTip
        {
            get
            {
                return Strings.OptionsCheckForUpdatesAsyncToolTip;
            }
        }

        public RelayCommand CheckForUpdatesAsync
        {
            get
            {
                return _checkForUpdatesAsync ?? (_checkForUpdatesAsync = new RelayCommand(async () =>
                {
                    try
                    {
                        IsIdle = false;
                        await UpdateUtils.UpdateCheckAsync(true, true);
                    }
                    catch (Exception ex)
                    {
                        ExceptionUtils.HandleException(ex);
                    }
                    finally
                    {
                        IsIdle = true;
                        RaisePropertyChanged(nameof(LastUpdateCheck));
                    }
                }, () =>
                {
                    return UpdateUtils.UpdateEnabled;
                }));
            }
        }
        private RelayCommand _checkForUpdatesAsync;

        #endregion

        public OptionsViewModelExtended() : base()
        {
            IsIdle = true;
        }

        private static ObservableCollection<string> GetReleaseChannels()
        {
            ObservableCollection<string> collection = new ObservableCollection<string>
            {
                Strings.ReleaseChannelOfficialFriendlyValue,
                Strings.ReleaseChannelPreviewFriendlyValue
            };

            return collection;
        }

        public override void RefreshProperties()
        {
            base.RefreshProperties();
            RaisePropertyChanged(nameof(SelectedRenderBackgroundIndex));
            RaisePropertyChanged(nameof(SelectedEditorRenderBackgroundIndex));
            RaisePropertyChanged(nameof(SelectedReleaseChannelIndex));
            RaisePropertyChanged(nameof(EnhancedCopy));
            RaisePropertyChanged(nameof(CheckUpdateOnStart));
            RaisePropertyChanged(nameof(LastUpdateCheck));
        }
    }
}
