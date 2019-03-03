// 
// AppViewModel.cs
//  
// Author:
//       Jon Thysell <thysell@gmail.com>
// 
// Copyright (c) 2015, 2016, 2017, 2019 Jon Thysell <http://jonthysell.com>
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
using System.IO;
using System.Reflection;

using GalaSoft.MvvmLight;

using com.jonthysell.Chordious.Core.ViewModel.Resources;

namespace com.jonthysell.Chordious.Core.ViewModel
{
    public delegate Stream GetConfigStream();
    public delegate object SvgTextToImage(string svgText, int width, int height, bool editMode);
    public delegate void DiagramToClipboard(ObservableDiagram diagram, bool renderImage, float scaleFactor);
    public delegate IEnumerable<string> GetSystemFonts();

    public delegate void DoOnUIThread(Action action);

    public class AppViewModel : ViewModelBase
    {
        public static AppViewModel Instance { get; private set; }

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
                RaisePropertyChanged("AppConfigLoaded");
            }
        }
        private bool _appConfigLoaded = false;

        private GetConfigStream _loadAppConfigStream;

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
                RaisePropertyChanged("UserConfigLoaded");
            }
        }
        private bool _userConfigLoaded = false;

        private GetConfigStream _loadUserConfigStream;

        private GetConfigStream _saveUserConfigStream;

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

        public SvgTextToImage SvgTextToImage { get; private set; }

        public DiagramToClipboard DiagramToClipboard { get; private set; }

        public GetSystemFonts GetSystemFonts { get; private set; }

        public DoOnUIThread DoOnUIThread { get; private set; }

        public static void Init(Assembly assembly, GetConfigStream loadAppConfigStream, GetConfigStream loadUserConfigStream, GetConfigStream saveUserConfigStream, SvgTextToImage svgTextToImage, DiagramToClipboard diagramToClipboard, DoOnUIThread doOnUIThread, GetSystemFonts getSystemFonts, string userConfigPath = "")
        {
            if (null != Instance)
            {
                throw new NotSupportedException();
            }

            Instance = new AppViewModel(assembly, loadAppConfigStream, loadUserConfigStream, saveUserConfigStream, svgTextToImage, diagramToClipboard, doOnUIThread, getSystemFonts, userConfigPath);
        }

        private AppViewModel(Assembly assembly, GetConfigStream loadAppConfigStream, GetConfigStream loadUserConfigStream, GetConfigStream saveUserConfigStream, SvgTextToImage svgTextToImage, DiagramToClipboard diagramToClipboard, DoOnUIThread doOnUIThread, GetSystemFonts getSystemFonts, string userConfigPath)
        {
            if (null == assembly)
            {
                throw new ArgumentNullException("assembly");
            }

            if (null == loadAppConfigStream)
            {
                throw new ArgumentNullException("loadAppConfigStream");
            }

            if (null == loadUserConfigStream)
            {
                throw new ArgumentNullException("loadUserConfigStream");
            }

            if (null == saveUserConfigStream)
            {
                throw new ArgumentNullException("saveUserConfigStream");
            }

            if (null == getSystemFonts)
            {
                throw new ArgumentNullException("getSystemFonts");
            }

            AppInfo.Assembly = assembly;
            _loadAppConfigStream = loadAppConfigStream;
            _loadUserConfigStream = loadUserConfigStream;
            _saveUserConfigStream = saveUserConfigStream;
            GetSystemFonts = getSystemFonts;
            UserConfigPath = userConfigPath;

            SvgTextToImage = svgTextToImage;
            DiagramToClipboard = diagramToClipboard;
            DoOnUIThread = doOnUIThread;

            AppConfig = new ConfigFile(DefaultConfig, ConfigFile.AppLevelKey);
            UserConfig = new ConfigFile(AppConfig, ConfigFile.UserLevelKey);
        }

        public void LoadAppConfig()
        {
            using (Stream inputStream = _loadAppConfigStream())
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
                using (Stream inputStream = _loadUserConfigStream())
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
            using (Stream outputStream = _saveUserConfigStream())
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
                    DoOnUIThread(() =>
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
    }
}
