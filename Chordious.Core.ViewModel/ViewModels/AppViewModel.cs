// Copyright (c) Jon Thysell <http://jonthysell.com>
// Licensed under the MIT License.

using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Reflection;

using GalaSoft.MvvmLight;

using Chordious.Core.ViewModel.Resources;

namespace Chordious.Core.ViewModel
{
    public class AppViewModel : ViewModelBase
    {
        public static AppViewModel Instance { get; private set; }

        public IAppView AppView { get; private set; }

        public string ProgramTitle
        {
            get
            {
                return AppInfo.ProgramTitle;
            }
        }

        public string FullVersion
        {
            get
            {
                return AppInfo.FullVersion;
            }
        }

        public string Watermark
        {
            get
            {
                return AppInfo.Watermark;
            }
        }

        #region General Labels

        public string PathLabel
        {
            get
            {
                return Strings.PathLabel;
            }
        }

        public string NameLabel
        {
            get
            {
                return Strings.NameLabel;
            }
        }

        public string CountLabel
        {
            get
            {
                return Strings.CountLabel;
            }
        }

        public string SelectedLabel
        {
            get
            {
                return Strings.SelectedLabel;
            }
        }

        public string YesLabel
        {
            get
            {
                return Strings.YesLabel;
            }
        }

        public string NoLabel
        {
            get
            {
                return Strings.NoLabel;
            }
        }

        public string IncludeLabel
        {
            get
            {
                return Strings.IncludeLabel;
            }
        }
        
        public string OptionsLabel
        {
            get
            {
                return Strings.OptionsLabel;
            }
        }

        public string StyleLabel
        {
            get
            {
                return Strings.StyleLabel;
            }
        }

        public string ApplyLabel
        {
            get
            {
                return Strings.ApplyLabel;
            }
        }

        public string AcceptLabel
        {
            get
            {
                return Strings.AcceptLabel;
            }
        }

        public string CancelLabel
        {
            get
            {
                return Strings.CancelLabel;
            }
        }

        #endregion

        internal ConfigFile DefaultConfig
        {
            get
            {
                return ConfigFile.DefaultConfig;
            }
        }

        internal ConfigFile AppConfig { get; private set; }

        public bool AppConfigLoaded
        {
            get
            {
                return _appConfigLoaded;
            }
            private set
            {
                _appConfigLoaded = value;
                RaisePropertyChanged(nameof(AppConfigLoaded));
            }
        }
        private bool _appConfigLoaded = false;

        internal ConfigFile UserConfig { get; private set; }

        public bool UserConfigLoaded
        {
            get
            {
                return _userConfigLoaded;
            }
            private set
            {
                _userConfigLoaded = value;
                RaisePropertyChanged(nameof(UserConfigLoaded));
            }
        }
        private bool _userConfigLoaded = false;

        public string UserConfigPath { get; private set; } = "";

        private Exception _userConfigLoadException = null;
        private Action<Exception> _userConfigLoadExceptionCallback = null;

        internal ChordiousSettings Settings
        {
            get
            {
                if (UserConfigLoaded)
                {
                    return UserConfig.ChordiousSettings;
                }

                if (AppConfigLoaded)
                {
                    return AppConfig.ChordiousSettings;
                }

                return DefaultConfig.ChordiousSettings;
            }
        }

        public static void Init(Assembly assembly, IAppView appView, string userConfigPath = "")
        {
            if (null != Instance)
            {
                throw new NotSupportedException();
            }

            Instance = new AppViewModel(assembly, appView, userConfigPath);
        }

        private AppViewModel(Assembly assembly, IAppView appView, string userConfigPath)
        {
            AppInfo.Assembly = assembly ?? throw new ArgumentNullException(nameof(assembly));
            AppView = appView ?? throw new ArgumentNullException(nameof(appView));
            UserConfigPath = userConfigPath;

            AppConfig = new ConfigFile(DefaultConfig, ConfigFile.AppLevelKey);
            UserConfig = new ConfigFile(AppConfig, ConfigFile.UserLevelKey);
        }

        public void LoadAppConfig()
        {
            using (Stream inputStream = AppView.GetAppConfigStream())
            {
                AppConfig.LoadFile(inputStream);
            }
            AppConfig.MarkAsReadOnly();
            AppConfigLoaded = true;
        }

        public void LoadUserConfig(Action<Exception> loadFailedCallback = null)
        {
            _userConfigLoadExceptionCallback = loadFailedCallback;

            try
            {
                using (Stream inputStream = AppView.GetUserConfigStreamToRead())
                {
                    UserConfig.LoadFile(inputStream);
                }
                UserConfigLoaded = true;
            }
            catch (Exception ex)
            {
                _userConfigLoadException = ex;
            }
        }

        public void SaveUserConfig()
        {
            using (Stream outputStream = AppView.GetUserConfigStreamToWrite())
            {
                UserConfig.SaveFile(outputStream);
            }
        }

        public void ResetUserConfig()
        {
            UserConfig = new ConfigFile(AppConfig, ConfigFile.UserLevelKey);
            UserConfigLoaded = true;
        }

        public void TryHandleFailedUserConfigLoad()
        {
            if (null != _userConfigLoadException)
            {
                if (null != _userConfigLoadExceptionCallback)
                {
                    AppView.DoOnUIThread(() =>
                    {
                        _userConfigLoadExceptionCallback(_userConfigLoadException);
                    });
                }
            }
        }

        public void Close()
        {
            SaveUserConfig();
        }

        #region Settings

        public string GetSetting(string key)
        {
            return Settings.Get(key);
        }

        public void SetSetting(string key, object value)
        {
            Settings.Set(key, value);
        }

        #endregion

        public ObservableCollection<ObservableInstrument> GetInstruments()
        {
            ObservableCollection<ObservableInstrument> collection = new ObservableCollection<ObservableInstrument>();

            ObservableCollection<ObservableInstrument> userCollection = GetUserInstruments();
            ObservableCollection<ObservableInstrument> defaultCollection = GetDefaultInstruments();

            bool haveUser = userCollection.Count > 0;
            bool haveDefault = defaultCollection.Count > 0;

            if (haveUser)
            {
                if (haveDefault)
                {
                    collection.Add(new ObservableInstrument(Strings.UserInstrumentsHeader));
                }

                foreach (ObservableInstrument userInstrument in userCollection)
                {
                    collection.Add(userInstrument);
                }
            }

            if (haveDefault)
            {
                if (haveUser)
                {
                    collection.Add(new ObservableInstrument(Strings.DefaultInstrumentsHeader));
                }

                foreach (ObservableInstrument defaultInstrument in defaultCollection)
                {
                    collection.Add(defaultInstrument);
                }
            }

            return collection;
        }

        public ObservableCollection<ObservableInstrument> GetDefaultInstruments()
        {
            ObservableCollection<ObservableInstrument> collection = new ObservableCollection<ObservableInstrument>();

            foreach (Instrument instrument in DefaultConfig.Instruments)
            {
                collection.Add(new ObservableInstrument(instrument));
            }

            return collection;
        }

        public ObservableCollection<ObservableInstrument> GetUserInstruments()
        {
            ObservableCollection<ObservableInstrument> collection = new ObservableCollection<ObservableInstrument>();

            foreach (Instrument instrument in UserConfig.Instruments)
            {
                collection.Add(new ObservableInstrument(instrument));
            }

            return collection;
        }

        public ObservableCollection<ObservableChordQuality> GetChordQualities()
        {
            ObservableCollection<ObservableChordQuality> collection = new ObservableCollection<ObservableChordQuality>();

            ObservableCollection<ObservableChordQuality> userCollection = GetUserChordQualities();
            ObservableCollection<ObservableChordQuality> defaultCollection = GetDefaultChordQualities();

            bool haveUser = userCollection.Count > 0;
            bool haveDefault = defaultCollection.Count > 0;

            if (haveUser)
            {
                if (haveDefault)
                {
                    collection.Add(new ObservableChordQuality(Strings.UserChordQualitiesHeader));
                }

                foreach (ObservableChordQuality userChordQuality in userCollection)
                {
                    collection.Add(userChordQuality);
                }
            }

            if (haveDefault)
            {
                if (haveUser)
                {
                    collection.Add(new ObservableChordQuality(Strings.DefaultChordQualitiesHeader));
                }

                foreach (ObservableChordQuality defaultChordQuality in defaultCollection)
                {
                    collection.Add(defaultChordQuality);
                }
            }

            return collection;
        }

        public ObservableCollection<ObservableChordQuality> GetDefaultChordQualities()
        {
            ObservableCollection<ObservableChordQuality> collection = new ObservableCollection<ObservableChordQuality>();

            foreach (ChordQuality chordQuality in DefaultConfig.ChordQualities)
            {
                collection.Add(new ObservableChordQuality(chordQuality));
            }

            return collection;
        }

        public ObservableCollection<ObservableChordQuality> GetUserChordQualities()
        {
            ObservableCollection<ObservableChordQuality> collection = new ObservableCollection<ObservableChordQuality>();

            foreach (ChordQuality chordQuality in UserConfig.ChordQualities)
            {
                collection.Add(new ObservableChordQuality(chordQuality));
            }

            return collection;
        }

        public ObservableCollection<ObservableScale> GetScales()
        {
            ObservableCollection<ObservableScale> collection = new ObservableCollection<ObservableScale>();

            ObservableCollection<ObservableScale> userCollection = GetUserScales();
            ObservableCollection<ObservableScale> defaultCollection = GetDefaultScales();

            bool haveUser = userCollection.Count > 0;
            bool haveDefault = defaultCollection.Count > 0;

            if (haveUser)
            {
                if (haveDefault)
                {
                    collection.Add(new ObservableScale(Strings.UserScalesHeader));
                }

                foreach (ObservableScale userScale in userCollection)
                {
                    collection.Add(userScale);
                }
            }

            if (haveDefault)
            {
                if (haveUser)
                {
                    collection.Add(new ObservableScale(Strings.DefaultScalesHeader));
                }

                foreach (ObservableScale defaultScale in defaultCollection)
                {
                    collection.Add(defaultScale);
                }
            }

            return collection;
        }

        public ObservableCollection<ObservableScale> GetDefaultScales()
        {
            ObservableCollection<ObservableScale> collection = new ObservableCollection<ObservableScale>();

            foreach (Scale scale in DefaultConfig.Scales)
            {
                collection.Add(new ObservableScale(scale));
            }

            return collection;
        }

        public ObservableCollection<ObservableScale> GetUserScales()
        {
            ObservableCollection<ObservableScale> collection = new ObservableCollection<ObservableScale>();

            foreach (Scale scale in UserConfig.Scales)
            {
                collection.Add(new ObservableScale(scale));
            }

            return collection;
        }

        public ObservableCollection<string> GetUserColors()
        {
            ObservableCollection<string> collection = new ObservableCollection<string>();

            foreach (var collectionKvp in UserConfig.DiagramLibrary.GetAll())
            {
                foreach (var diagram in collectionKvp.Value)
                {
                    foreach (var color in diagram.GetUsedColors())
                    {
                        collection.SortedInsert(color);
                    }
                }
            }

            return collection;
        }
    }
}
