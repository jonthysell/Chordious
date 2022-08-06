// Copyright (c) Jon Thysell <http://jonthysell.com>
// Licensed under the MIT License.

using System;
using System.Collections.ObjectModel;

using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

using Chordious.Core.ViewModel;

using Chordious.WPF.Resources;

namespace Chordious.WPF
{
    public class OptionsViewModelExtended : OptionsViewModel, IIdle
    {
        #region Backgrounds

        public static string SettingsBackgroundGroupLabel
        {
            get
            {
                return Strings.OptionsSettingsBackgroundGroupLabel;
            }
        }

        public static string SelectedRenderBackgroundLabel
        {
            get
            {
                return Strings.OptionsSelectedRendererBackgroundLabel;
            }
        }

        public static string SelectedRenderBackgroundToolTip
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
                return _renderBackgrounds ??= ImageUtils.GetBackgrounds();
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
                OnPropertyChanged(nameof(SelectedRenderBackgroundIndex));
            }
        }

        public static string SelectedEditorRenderBackgroundLabel
        {
            get
            {
                return Strings.OptionsSelectedEditorRendererBackgroundLabel;
            }
        }

        public static string SelectedEditorRenderBackgroundToolTip
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
                return _editorRenderBackgrounds ??= ImageUtils.GetBackgrounds();
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
                OnPropertyChanged(nameof(SelectedEditorRenderBackgroundIndex));
            }
        }

        #endregion

        #region Integrations

        public static string SettingsIntegrationGroupLabel
        {
            get
            {
                return Strings.OptionsSettingsIntegrationGroupLabel;
            }
        }

        public static string EnhancedCopyLabel
        {
            get
            {
                return Strings.OptionsEnhancedCopyLabel;
            }
        }

        public static string EnhancedCopyToolTip
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
                OnPropertyChanged(nameof(EnhancedCopy));
            }
        }

        public static string OpenTempFolderLabel
        {
            get
            {
                return Strings.OptionsOpenTempFolderLabel;
            }
        }

        public static string OpenTempFolderToolTip
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
                return _openTempFolder ??= new RelayCommand(() =>
                {
                    try
                    {
                        string path = IntegrationUtils.GetTempPathAndCreateIfNecessary();
                        StrongReferenceMessenger.Default.Send(new LaunchUrlMessage(path));
                    }
                    catch (Exception ex)
                    {
                        ExceptionUtils.HandleException(ex);
                    }
                });
            }
        }
        private RelayCommand _openTempFolder;

        #endregion

        #region Updates

        public static bool UpdateEnabled
        {
            get
            {
                return UpdateUtils.UpdateEnabled;
            }
        }

        public static string UpdatesGroupLabel
        {
            get
            {
                return Strings.OptionsUpdatesGroupLabel;
            }
        }

        public static string SelectedReleaseChannelLabel
        {
            get
            {
                return Strings.OptionsSelectedReleaseChannelLabel;
            }
        }

        public static string SelectedReleaseChannelToolTip
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
                return _releaseChannels ??= GetReleaseChannels();
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
                OnPropertyChanged(nameof(SelectedReleaseChannelIndex));
            }
        }

        public static string CheckUpdateOnStartLabel
        {
            get
            {
                return Strings.OptionsCheckUpdateOnStartLabel;
            }
        }

        public static string CheckUpdateOnStartToolTip
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
                OnPropertyChanged(nameof(CheckUpdateOnStart));
            }
        }

        public static string LastUpdateCheckLabel
        {
            get
            {
                return Strings.OptionsLastUpdateCheckLabel;
            }
        }

        public static string LastUpdateCheckToolTip
        {
            get
            {
                return Strings.OptionsLastUpdateCheckToolTip;
            }
        }

        public static string LastUpdateCheck
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

        public static string CheckForUpdatesAsyncLabel
        {
            get
            {
                return Strings.OptionsCheckForUpdatesAsyncLabel;
            }
        }

        public static string CheckForUpdatesAsyncToolTip
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
                return _checkForUpdatesAsync ??= new RelayCommand(async () =>
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
                        OnPropertyChanged(nameof(WPF.OptionsViewModelExtended.LastUpdateCheck));
                    }
                }, () =>
                {
                    return UpdateUtils.UpdateEnabled;
                });
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
            OnPropertyChanged(nameof(SelectedRenderBackgroundIndex));
            OnPropertyChanged(nameof(SelectedEditorRenderBackgroundIndex));
            OnPropertyChanged(nameof(SelectedReleaseChannelIndex));
            OnPropertyChanged(nameof(EnhancedCopy));
            OnPropertyChanged(nameof(CheckUpdateOnStart));
            OnPropertyChanged(nameof(LastUpdateCheck));
        }
    }
}
