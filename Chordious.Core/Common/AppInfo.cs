// 
// AppInfo.cs
//  
// Author:
//       Jon Thysell <thysell@gmail.com>
// 
// Copyright (c) 2013, 2014, 2015, 2016, 2017, 2018, 2019, 2020 Jon Thysell <http://jonthysell.com>
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

using Chordious.Core.Resources;

namespace Chordious.Core
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
        /// Whether or not the app is running on Mono
        /// </summary>
        public static bool IsRunningOnMono
        {
            get
            {
                return null != Type.GetType("Mono.Runtime");
            }
        }

        /// <summary>
        /// Short version string if an official release, full version string otherwise.
        /// </summary>
        public static string Version
        {
            get
            {
                if (Assembly.GetName().Version.Minor % 2 == 0) // Official release
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
                Version vers = Assembly.GetName().Version;
                return vers.Build == 0 ? $"{vers.Major}.{vers.Minor}" : $"{vers.Major}.{vers.Minor}.{vers.Build}";
            }
        }

        /// <summary>
        /// Full version string {Major}.{Minor}.{Build}.{Revision}.
        /// </summary>
        public static string FullVersion
        {
            get
            {
                return Assembly.GetName().Version.ToString();
            }
        }

        /// <summary>
        /// The product name.
        /// </summary>
        public static string ProgramName
        {
            get
            {
                return Assembly.GetName().Name;
            }
        }

        /// <summary>
        /// The full program title.
        /// </summary>
        public static string ProgramTitle
        {
            get
            {
                return $"{ProgramName} {Version}";
            }
        }

        /// <summary>
        /// The product title.
        /// </summary>
        public static string Product
        {
            get
            {
                return Assembly.GetCustomAttribute<AssemblyProductAttribute>().Product;
            }
        }

        /// <summary>
        /// The product description.
        /// </summary>
        public static string Comments
        {
            get
            {
                return Strings.AppDescription;
            }
        }

        /// <summary>
        /// The product copyright.
        /// </summary>
        public static string Copyright
        {
            get
            {
                return Assembly.GetCustomAttribute<AssemblyCopyrightAttribute>().Copyright; ;
            }
        }

        /// <summary>
        /// The watermark text to put in images.
        /// </summary>
        public static string Watermark
        {
            get
            {
                return string.Format(Strings.WatermarkFormat, ProgramTitle);
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
        /// The product license name.
        /// </summary>
        public static string MitLicenseName
        {
            get
            {
                return "The MIT License (MIT)";
            }
        }

        /// <summary>
        /// The product license body.
        /// </summary>
        public static string MitLicenseBody
        {
            get
            {
                return string.Join(Environment.NewLine + Environment.NewLine, _mitLicense);
            }
        }

        private static readonly string[] _mitLicense = {
            @"Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the ""Software""), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:",
            @"The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.",
            @"THE SOFTWARE IS PROVIDED ""AS IS"", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE."
        };
    }
}

