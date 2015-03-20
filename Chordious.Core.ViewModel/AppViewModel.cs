// 
// AppViewModel.cs
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
using System.Collections.ObjectModel;
using System.IO;
using System.Reflection;

using GalaSoft.MvvmLight;

using com.jonthysell.Chordious.Core;

namespace com.jonthysell.Chordious.Core.ViewModel
{
    public delegate Stream GetConfigStream();
    public delegate object SvgTextToImage(string svgText, int width, int height);

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

        internal ConfigFile DefaultConfig { get; private set; }

        public bool DefaultConfigLoaded
        {
            get
            {
                return _defaultConfigLoaded;
            }
            private set
            {
                _defaultConfigLoaded = value;
                RaisePropertyChanged("DefaultConfigLoaded");
            }
        }
        private bool _defaultConfigLoaded = false;

        private GetConfigStream _loadDefaultConfigStream;

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

        public string UserConfigPath
        {
            get
            {
                return _userConfigPath;
            }
            private set
            {
                _userConfigPath = value;
                RaisePropertyChanged("UserConfigPath");
            }
        }
        private string _userConfigPath = "";

        public SvgTextToImage SvgTextToImage { get; private set; }

        public DoOnUIThread DoOnUIThread { get; private set; }

        public static void Init(Assembly assembly, GetConfigStream loadDefaultConfigStream, GetConfigStream loadAppConfigStream, GetConfigStream loadUserConfigStream, GetConfigStream saveUserConfigStream, SvgTextToImage svgTextToImage, DoOnUIThread doOnUIThread, string userConfigPath = "")
        {
            if (null != Instance)
            {
                throw new NotSupportedException();
            }

            Instance = new AppViewModel(assembly, loadDefaultConfigStream, loadAppConfigStream, loadUserConfigStream, saveUserConfigStream, svgTextToImage, doOnUIThread, userConfigPath);
        }

        private AppViewModel(Assembly assembly, GetConfigStream loadDefaultConfigStream, GetConfigStream loadAppConfigStream, GetConfigStream loadUserConfigStream, GetConfigStream saveUserConfigStream, SvgTextToImage svgTextToImage, DoOnUIThread doOnUIThread, string userConfigPath)
        {
            if (null == assembly)
            {
                throw new ArgumentNullException("assembly");
            }

            if (null == loadDefaultConfigStream)
            {
                throw new ArgumentNullException("loadDefaultConfigStream");
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

            AppInfo.Assembly = assembly;
            _loadDefaultConfigStream = loadDefaultConfigStream;
            _loadAppConfigStream = loadAppConfigStream;
            _loadUserConfigStream = loadUserConfigStream;
            _saveUserConfigStream = saveUserConfigStream;
            UserConfigPath = userConfigPath;

            SvgTextToImage = svgTextToImage;
            DoOnUIThread = doOnUIThread;

            DefaultConfig = new ConfigFile("Default");
            AppConfig = new ConfigFile(DefaultConfig, "App");
            UserConfig = new ConfigFile(AppConfig, "User");
        }

        public void LoadDefaultConfig()
        {
            using (Stream inputStream = _loadDefaultConfigStream())
            {
                DefaultConfig.LoadFile(inputStream);
            }
            DefaultConfig.MarkAsReadOnly();
            DefaultConfigLoaded = true;
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

        public void LoadUserConfig()
        {
            using (Stream inputStream = _loadUserConfigStream())
            {
                UserConfig.LoadFile(inputStream);
            }
            UserConfigLoaded = true;
        }

        public void SaveUserConfig()
        {
            using (Stream outputStream = _saveUserConfigStream())
            {
                UserConfig.SaveFile(outputStream);
            }
        }

        public void Close()
        {
            SaveUserConfig();
        }

        public string GetSetting(string key)
        {
            return UserConfig.ChordiousSettings.Get(key);
        }

        public void SetSetting(string key, object value)
        {
            UserConfig.ChordiousSettings.Set(key, value);
        }

        public ObservableCollection<string> GetNotes()
        {
            ObservableCollection<string> collection = new ObservableCollection<string>();

            for (int i = 0; i < Enum.GetValues(typeof(Note)).Length; i++)
            {
                collection.Add(NoteUtils.ToString((Note)i));
            }

            return collection;
        }

        public ObservableCollection<string> GetInternalNotes()
        {
            ObservableCollection<string> collection = new ObservableCollection<string>();

            for (int i = 0; i < Enum.GetValues(typeof(InternalNote)).Length; i++)
            {
                collection.Add(NoteUtils.ToString((InternalNote)i, InternalNoteStringStyle.ShowBoth));
            }

            return collection;
        }

        public ObservableCollection<ObservableInstrument> GetInstruments()
        {
            ObservableCollection<ObservableInstrument> collection = new ObservableCollection<ObservableInstrument>();

            foreach (Instrument instrument in UserConfig.Instruments)
            {
                collection.Add(new ObservableInstrument(instrument));
            }

            foreach (Instrument instrument in DefaultConfig.Instruments)
            {
                collection.Add(new ObservableInstrument(instrument));
            }

            return collection;
        }

        public ObservableCollection<ObservableChordQuality> GetChordQualities()
        {
            ObservableCollection<ObservableChordQuality> collection = new ObservableCollection<ObservableChordQuality>();

            foreach (ChordQuality chordQuality in UserConfig.ChordQualities)
            {
                collection.Add(new ObservableChordQuality(chordQuality));
            }

            foreach (ChordQuality chordQuality in DefaultConfig.ChordQualities)
            {
                collection.Add(new ObservableChordQuality(chordQuality));
            }

            return collection;
        }

        public ObservableCollection<ObservableScale> GetScales()
        {
            ObservableCollection<ObservableScale> collection = new ObservableCollection<ObservableScale>();

            foreach (Scale scale in UserConfig.Scales)
            {
                collection.Add(new ObservableScale(scale));
            }

            foreach (Scale scale in DefaultConfig.Scales)
            {
                collection.Add(new ObservableScale(scale));
            }

            return collection;
        }
    }
}
