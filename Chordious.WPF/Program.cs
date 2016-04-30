// 
// Program.cs
//  
// Author:
//       Jon Thysell <thysell@gmail.com>
// 
// Copyright (c) 2015, 2016 Jon Thysell <http://jonthysell.com>
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
using System.Threading;
using System.Windows;

using com.jonthysell.Chordious.WPF.Resources;

namespace com.jonthysell.Chordious.WPF
{
    public class Program
    {
        private static Mutex _mutex;

        [STAThread]
        public static void Main(string[] args)
        {
            _mutex = new Mutex(true, "Chordious.WPF");

            if (!_mutex.WaitOne(TimeSpan.Zero, false))
            {
                MessageBox.Show(Strings.ChordiousAlreadyRunningErrorMessage, Strings.ChordiousDialogCaption, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                try
                {
                    App app = new App();
                    app.InitializeComponent();
                    app.Run();
                }
                catch (Exception ex)
                {
                    String message = String.Join(Environment.NewLine, String.Format(Strings.ChordiousUnhandledExceptionMessageFormat, ex.Message), ex.StackTrace);
                    MessageBox.Show(message, Strings.ChordiousDialogCaption, MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
