// 
// AppInfo.cs
//  
// Author:
//       Jon Thysell <thysell@gmail.com>
// 
// Copyright (c) 2013, 2014 Jon Thysell <http://jonthysell.com>
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
using System.Reflection;

namespace com.jonthysell.Chordious.Core
{
    /// <summary>
    /// AppInfo contains methods to get app's name and version information.
    /// </summary>
    public class AppInfo
    {
        /// <summary>
        /// Assembly information.
        /// </summary>
        public static Assembly Assembly { get; set; }

        /// <summary>
        /// Short version string if an official release, full version string otherwise.
        /// </summary>
        public static string Version
        {
            get
            {
                if (AppInfo.Assembly.GetName().Version.Minor % 2 == 0) // Official release
                {
                    return ShortVersion;
                }
                else // Internal release
                {
                    return FullVersion;
                }
            }
        }

        /// <summary>
        /// Short version string {Major}.{Minor}.{Build}
        /// </summary>
        public static string ShortVersion
        {
            get
            {
                Version vers = AppInfo.Assembly.GetName().Version;
                return String.Format("{0}.{1}.{2}", vers.Major, vers.Minor, vers.Build);
            }
        }

        /// <summary>
        /// Full version string {Major}.{Minor}.{Build}.{Revision}.
        /// </summary>
        public static string FullVersion
        {
            get
            {
                return AppInfo.Assembly.GetName().Version.ToString();
            }
        }

        /// <summary>
        /// The product name.
        /// </summary>
        public static string ProgramName
        {
            get
            {
                return AppInfo.Assembly.GetName().Name;
            }
        }

        /// <summary>
        /// The full program title.
        /// </summary>
        public static string ProgramTitle
        {
            get
            {
                return String.Format("{0} {1} by {2}",
                                     AppInfo.ProgramName,
                                     AppInfo.Version,
                                     "Jon Thysell");
            }
        }

        /// <summary>
        /// The product title.
        /// </summary>
        public static string Product
        {
            get
            {
                return AppInfo.Assembly.GetCustomAttribute<AssemblyProductAttribute>().Product;
            }
        }

        /// <summary>
        /// The product description.
        /// </summary>
        public static string Comments
        {
            get
            {
                return "Fretboard diagram generator for stringed instruments.";
            }
        }

        /// <summary>
        /// The product copyright.
        /// </summary>
        public static string Copyright
        {
            get
            {
                return Assembly.GetCustomAttribute<AssemblyCopyrightAttribute>().Copyright;
            }
        }

        /// <summary>
        /// The product website.
        /// </summary>
        public static string WebsiteLabel
        {
            get
            {
                return "Chordious Website";
            }
        }

        /// <summary>
        /// The product website.
        /// </summary>
        public static string Website
        {
            get
            {
                return "http://chordious.com/";
            }
        }

        /// <summary>
        /// The product license.
        /// </summary>
        public static string License
        {
            get
            {
                return String.Join(Environment.NewLine + Environment.NewLine, _license);
            }
        }

        private static string[] _license = {
            @"Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the ""Software""), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:",
            @"The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.",
            @"THE SOFTWARE IS PROVIDED ""AS IS"", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE."
        };
    }
}

