// Copyright (c) Jon Thysell <http://jonthysell.com>
// Licensed under the MIT License.

using System;
using System.IO;
using System.Threading;
using System.Windows;

#if PORTABLE
using System.Globalization;
using System.Reflection;
#endif

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
#if PORTABLE
            // Hook into assembly resolution since the assemblies are embedded
            AppDomain.CurrentDomain.AssemblyResolve += OnResolveAssembly;
#endif
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

#if PORTABLE
        // Adapted from http://www.digitallycreated.net/Blog/61/combining-multiple-assemblies-into-a-single-exe-for-a-wpf-application
        private static Assembly OnResolveAssembly(object sender, ResolveEventArgs args)
        {
            Assembly executingAssembly = Assembly.GetExecutingAssembly();
            AssemblyName assemblyName = new AssemblyName(args.Name);

            string path = assemblyName.Name + ".dll";
            if (assemblyName.CultureInfo.Equals(CultureInfo.InvariantCulture) == false)
            {
                path = string.Format(@"{0}\{1}", assemblyName.CultureInfo, path);
            }

            using (Stream stream = executingAssembly.GetManifestResourceStream(path))
            {
                if (null == stream)
                {
                    return null;
                }

                byte[] assemblyRawBytes = new byte[stream.Length];
                stream.Read(assemblyRawBytes, 0, assemblyRawBytes.Length);
                return Assembly.Load(assemblyRawBytes);
            }
        }
#endif
    }
}
