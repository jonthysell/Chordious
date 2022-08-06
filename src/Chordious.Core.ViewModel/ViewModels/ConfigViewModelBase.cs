// Copyright (c) Jon Thysell <http://jonthysell.com>
// Licensed under the MIT License.

using System;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using Chordious.Core.ViewModel.Resources;

namespace Chordious.Core.ViewModel
{
    public abstract class ConfigViewModelBase : ObservableObject
    {
        public AppViewModel AppVM
        {
            get
            {
                return AppViewModel.Instance;
            }
        }

        public abstract string Title { get; }

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

        public string IncludeSettingsLabel
        {
            get
            {
                return Strings.ConfigPartsSettingsLabel;
            }
        }

        public string IncludeSettingsToolTip
        {
            get
            {
                return Strings.ConfigPartsSettingsToolTip;
            }
        }

        public bool IncludeSettings
        {
            get
            {
                return _includeSettings;
            }
            set
            {
                _includeSettings = value;
                OnPropertyChanged(nameof(IncludeSettings));
            }
        }
        private bool _includeSettings;

        public string IncludeStylesLabel
        {
            get
            {
                return Strings.ConfigPartsStylesLabel;
            }
        }

        public string IncludeStylesToolTip
        {
            get
            {
                return Strings.ConfigPartsStylesToolTip;
            }
        }

        public bool IncludeStyles
        {
            get
            {
                return _includeStyles;
            }
            set
            {
                _includeStyles = value;
                OnPropertyChanged(nameof(IncludeStyles));
            }
        }
        private bool _includeStyles;

        public string IncludeInstrumentsLabel
        {
            get
            {
                return Strings.ConfigPartsInstrumentsLabel;
            }
        }

        public string IncludeInstrumentsToolTip
        {
            get
            {
                return Strings.ConfigPartsInstrumentsToolTip;
            }
        }

        public bool IncludeInstruments
        {
            get
            {
                return _includeInstruments;
            }
            set
            {
                _includeInstruments = value;
                OnPropertyChanged(nameof(IncludeInstruments));
            }
        }
        private bool _includeInstruments;

        public string IncludeChordQualitiesLabel
        {
            get
            {
                return Strings.ConfigPartsChordQualitiesLabel;
            }
        }

        public string IncludeChordQualitiesToolTip
        {
            get
            {
                return Strings.ConfigPartsChordQualitiesToolTip;
            }
        }

        public bool IncludeChordQualities
        {
            get
            {
                return _includeChordQualities;
            }
            set
            {
                _includeChordQualities = value;
                OnPropertyChanged(nameof(IncludeChordQualities));
            }
        }
        private bool _includeChordQualities;

        public string IncludeScalesLabel
        {
            get
            {
                return Strings.ConfigPartsScalesLabel;
            }
        }

        public string IncludeScalesToolTip
        {
            get
            {
                return Strings.ConfigPartsScalesToolTip;
            }
        }

        public bool IncludeScales
        {
            get
            {
                return _includeScales;
            }
            set
            {
                _includeScales = value;
                OnPropertyChanged(nameof(IncludeScales));
            }
        }
        private bool _includeScales;

        public string IncludeLibraryLabel
        {
            get
            {
                return Strings.ConfigPartsLibraryLabel;
            }
        }

        public string IncludeLibraryToolTip
        {
            get
            {
                return Strings.ConfigPartsLibraryToolTip;
            }
        }

        public bool IncludeLibrary
        {
            get
            {
                return _includeLibrary;
            }
            set
            {
                _includeLibrary = value;
                OnPropertyChanged(nameof(IncludeLibrary));
            }
        }
        private bool _includeLibrary;

        public abstract RelayCommand Accept { get; }

        public RelayCommand Cancel
        {
            get
            {
                return _cancel ?? (_cancel = new RelayCommand(() =>
                {
                    try
                    {
                        OnRequestClose();
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

        public ConfigViewModelBase()
        {
            _includeSettings = true;
            _includeStyles = true;
            _includeInstruments = true;
            _includeChordQualities = true;
            _includeScales = true;
            _includeLibrary = true;
            _isIdle = true;
        }

        protected void OnRequestClose()
        {
            RequestClose?.Invoke();
        }

        protected ConfigParts GetConfigParts()
        {
            ConfigParts configParts = ConfigParts.None;

            configParts |= IncludeSettings ? ConfigParts.Settings : ConfigParts.None;
            configParts |= IncludeStyles ? ConfigParts.Styles : ConfigParts.None;
            configParts |= IncludeInstruments ? ConfigParts.Instruments : ConfigParts.None;
            configParts |= IncludeChordQualities ? ConfigParts.Qualities : ConfigParts.None;
            configParts |= IncludeScales ? ConfigParts.Scales : ConfigParts.None;
            configParts |= IncludeLibrary ? ConfigParts.Library : ConfigParts.None;

            return configParts;
        }
    }
}