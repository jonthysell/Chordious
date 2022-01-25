// Copyright (c) Jon Thysell <http://jonthysell.com>
// Licensed under the MIT License.

using System;
using System.IO;
using System.Threading;
using System.Windows;

using Chordious.WPF.Resources;

namespace Chordious.WPF
{
    public class Program
    {
        private static Mutex _mutex;

        private static string _userFile = null;

        [STAThread]
        public static void Main(string[] args)
        {
            ChordiousMain(args);
        }

        private static void ChordiousMain(string[] args)
        {
            // Set user file from command-line args
            if (null != args && args.Length > 0)
            {
                _userFile = args[0];
            }

            try
            {
                _mutex = new Mutex(true, GetMutexName());

                if (!_mutex.WaitOne(TimeSpan.Zero, false))
                {
                    MessageBox.Show(Strings.ChordiousAlreadyRunningErrorMessage, Strings.ChordiousDialogCaption, MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                App app = new App(_userFile);
                app.InitializeComponent();
                app.Run();
            }
            catch (Exception ex)
            {
                string message = string.Join(Environment.NewLine, string.Format(Strings.ChordiousUnhandledExceptionMessageFormat, ex.Message), ex.StackTrace);
                MessageBox.Show(message, Strings.ChordiousDialogCaption, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private static string GetMutexName()
        {
            return string.Format("{0}.{1}", MutexBase, Path.GetFullPath(App.GetUserConfigPath(_userFile)).GetHashCode());
        }

        private const string MutexBase = "Chordious.WPF";
    }
}
