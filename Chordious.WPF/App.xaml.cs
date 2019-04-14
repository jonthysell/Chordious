// 
// App.xaml.cs
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
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Threading;

using GalaSoft.MvvmLight.Messaging;

using com.jonthysell.Chordious.Core.ViewModel;
using com.jonthysell.Chordious.WPF.Resources;

namespace com.jonthysell.Chordious.WPF
{
    public partial class App : Application, IAppView
    {
        public AppViewModel AppVM
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
#if PORTABLE
            userConfigPath = !string.IsNullOrWhiteSpace(userConfigPath) ? userConfigPath.Trim() : DefaultUserConfigFileName;
#else
            userConfigPath = !string.IsNullOrWhiteSpace(userConfigPath) ? userConfigPath.Trim() : GetAppDataUserConfigPath(setup);
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
            Messenger.Default.Send(new ConfirmationMessage(Strings.ResetAndBackupUserConfigConfirmationMessage, (confirmed) =>
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

        private string ResetAndBackupUserConfig(string userFile)
        {
            if (string.IsNullOrWhiteSpace(userFile))
            {
                throw new ArgumentNullException("userFile");
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
                if (null != imageSource)
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
            EventManager.RegisterClassHandler(typeof(TextBox), UIElement.PreviewMouseLeftButtonDownEvent, new MouseButtonEventHandler(SelectivelyHandleMouseButton), true);
            EventManager.RegisterClassHandler(typeof(TextBox), UIElement.GotKeyboardFocusEvent, new RoutedEventHandler(SelectAllText), true);
            EventManager.RegisterClassHandler(typeof(TextBox), UIElement.PreviewKeyUpEvent, new RoutedEventHandler(EnterUpdatesSource), true);

            PreloadDisabledImages();

            base.OnStartup(e);
        }

        private static void SelectivelyHandleMouseButton(object sender, MouseButtonEventArgs e)
        {
            TextBox textBox = (sender as TextBox);
            if (null != textBox && !textBox.IsKeyboardFocusWithin)
            {
                if (e.OriginalSource.GetType().Name == "TextBoxView")
                {
                    e.Handled = true;
                    textBox.Focus();
                }
            }
        }

        private static void SelectAllText(object sender, RoutedEventArgs e)
        {
            TextBox textBox = (e.OriginalSource as TextBox);
            if (null != textBox)
            {
                textBox.SelectAll();
            }
        }

        private static void EnterUpdatesSource(object sender, RoutedEventArgs e)
        {
            TextBox textBox = (e.OriginalSource as TextBox);
            if (null != textBox && !textBox.IsReadOnly)
            {
                KeyEventArgs args = e as KeyEventArgs;
                if (null != args && args.Key == Key.Enter)
                {
                    textBox.GetBindingExpression(TextBox.TextProperty).UpdateSource();
                }
            }
        }

        #region IAppView

        public void DoOnUIThread(Action action)
        {
            Dispatcher.Invoke(action);
        }

        public Stream GetAppConfigStream()
        {
            return GetResourceStream(new Uri("pack://application:,,,/Chordious.WPF.xml")).Stream;
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
