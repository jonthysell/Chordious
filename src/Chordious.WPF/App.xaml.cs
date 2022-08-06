// Copyright (c) Jon Thysell <http://jonthysell.com>
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Threading;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

using Xceed.Wpf.Toolkit;

using Chordious.Core.ViewModel;
using Chordious.WPF.Resources;

namespace Chordious.WPF
{
    public partial class App : Application, IAppView
    {
        public static AppViewModel AppVM
        {
            get
            {
                return AppViewModel.Instance;
            }
        }

        public string UserConfigPath { get; private set; }

        public App(string userConfigPath = null)
        {
            MessageHandlers.RegisterMessageHandlers(this);

            UserConfigPath = GetUserConfigPath(userConfigPath, true);

            AppViewModel.Init(Assembly.GetEntryAssembly(), this, UserConfigPath);

            AppVM.LoadAppConfig();

            if (!File.Exists(UserConfigPath))
            {
                // Makes sure that LoadUserConfig will be successful
                AppVM.SaveUserConfig();
            }

            AppVM.LoadUserConfig(HandleUserConfigLoadException);

            // Prepopulate RecentColors
            foreach (var str in AppVM.GetUserColors())
            {
                if (StringToColorConverter.TryParseColorItem(str, out ColorItem result))
                {
                    RecentColors.Add(result);
                }
            }

            // Makes sure textboxes accept localized inputs
            FrameworkElement.LanguageProperty.OverrideMetadata(typeof(FrameworkElement), new FrameworkPropertyMetadata(XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.IetfLanguageTag)));

            Exit += App_Exit;
        }

        public static string GetUserConfigPath(string userConfigPath)
        {
            return GetUserConfigPath(userConfigPath, false);
        }

        private static string GetUserConfigPath(string userConfigPath, bool setup)
        {
#if APPDATACONFIG
            userConfigPath = !string.IsNullOrWhiteSpace(userConfigPath) ? userConfigPath.Trim() : GetAppDataUserConfigPath(setup);
#else
            userConfigPath = !string.IsNullOrWhiteSpace(userConfigPath) ? userConfigPath.Trim() : DefaultUserConfigFileName;
#endif
            return userConfigPath;
        }

        private static string GetAppDataUserConfigPath(bool setup)
        {
            string userFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            userFolder = Path.Combine(userFolder, "Chordious");

            string defaultConfigPath = Path.Combine(userFolder, DefaultUserConfigFileName);

            if (setup)
            {
                if (!Directory.Exists(userFolder))
                {
                    Directory.CreateDirectory(userFolder);
                }

                if (!File.Exists(defaultConfigPath))
                {
                    // Migrate old config if necessary
                    string oldConfigPath = Path.Combine(userFolder, "Chordious.WPF.xml");
                    if (File.Exists(oldConfigPath))
                    {
                        File.Move(oldConfigPath, defaultConfigPath);
                    }
                }
            }

            return defaultConfigPath;
        }

        private const string DefaultUserConfigFileName = "Chordious.User.xml";

        public void HandleUserConfigLoadException(Exception ex)
        {
            string userFile = UserConfigPath;
            StrongReferenceMessenger.Default.Send(new ConfirmationMessage(Strings.ResetAndBackupUserConfigConfirmationMessage, (confirmed) =>
            {
                if (!confirmed) // No, exit app
                {
                    Exit -= App_Exit; // Remove the handler so we don't overwrite the corruupt config with whatever we may have loaded 
                    Shutdown(1);
                }
                else // Yes, backup and continue
                {
                    string backupFile = ResetAndBackupUserConfig(userFile);
                    string message = string.Format(Strings.ResetAndBackupUserConfigBackupFileMessageFormat, backupFile);
                    ExceptionUtils.HandleException(new Exception(message, ex));
                }
            }));
        }

        private static string ResetAndBackupUserConfig(string userFile)
        {
            if (string.IsNullOrWhiteSpace(userFile))
            {
                throw new ArgumentNullException(nameof(userFile));
            }

            string userFolder = Path.GetDirectoryName(userFile);
            string userFileName = Path.GetFileNameWithoutExtension(userFile);
            string userFileExt = Path.GetExtension(userFile);

            string backupFile = Path.Combine(userFolder, string.Format("{0}.{1:yyyy.MM.dd.HH.mm.ss}{2}", userFileName, DateTime.UtcNow, userFileExt));

            File.Move(userFile, backupFile);

            AppVM.ResetUserConfig();
            AppVM.SaveUserConfig();

            return backupFile;
        }

        private void PreloadDisabledImages()
        {
            foreach (object obj in Resources.Values)
            {
                ImageSource imageSource = (obj as ImageSource);
                if (imageSource is not null)
                {
                    AutoDisableImage.LoadImage(imageSource);
                }
            }
        }

        private void App_Exit(object sender, ExitEventArgs e)
        {
            try
            {
                AppVM.Close();
                MessageHandlers.UnregisterMessageHandlers(this);
            }
            catch (Exception ex)
            {
                ExceptionUtils.HandleException(ex);
            }
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            PreloadDisabledImages();

            base.OnStartup(e);
        }

        #region Colors

        public ObservableCollection<ColorItem> StandardColors
        {
            get
            {
                if (_standardColors is null)
                {
                    _standardColors = new ObservableCollection<ColorItem>();
                    foreach (var kvp in StringToColorConverter.NamedColors)
                    {
                        _standardColors.Add(new ColorItem(kvp.Value, kvp.Key));
                    }
                }
                return _standardColors;
            }
        }
        private ObservableCollection<ColorItem> _standardColors;

        public ObservableCollection<ColorItem> RecentColors { get; } = new ObservableCollection<ColorItem>();

        #endregion

        #region IAppView

        public void DoOnUIThread(Action action)
        {
            Dispatcher.Invoke(action);
        }

        public object DoOnUIThread(Func<object> func)
        {
            return Dispatcher.Invoke(func);
        }

        public Stream GetAppConfigStream()
        {
            return typeof(App).GetTypeInfo().Assembly.GetManifestResourceStream("Chordious.WPF.Chordious.WPF.xml");
        }

        public Stream GetUserConfigStreamToRead()
        {
            return new FileStream(UserConfigPath, FileMode.Open);
        }

        public Stream GetUserConfigStreamToWrite()
        {
            return new FileStream(UserConfigPath, FileMode.Create);
        }

        public object SvgTextToImage(string svgText, int width, int height, bool editMode)
        {
            return ImageUtils.SvgTextToBitmapImage(svgText, width, height, editMode);
        }

        public void TextToClipboard(string text)
        {
            IntegrationUtils.TextToClipboard(text);
        }

        public void DiagramToClipboard(ObservableDiagram diagram, float scaleFactor)
        {
            IntegrationUtils.DiagramToClipboard(diagram, scaleFactor);
        }

        public IEnumerable<string> GetSystemFonts()
        {
            foreach (System.Drawing.FontFamily font in System.Drawing.FontFamily.Families)
            {
                yield return font.Name;
            }
        }

        #endregion
    }
}
