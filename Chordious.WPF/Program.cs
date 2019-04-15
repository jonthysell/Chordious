// 
// Program.cs
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
using System.IO;
using System.Threading;
using System.Windows;

#if PORTABLE
using System.Globalization;
using System.Reflection;
#endif

using com.jonthysell.Chordious.WPF.Resources;

namespace com.jonthysell.Chordious.WPF
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
