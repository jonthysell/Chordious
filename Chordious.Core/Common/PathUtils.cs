// 
// PathUtils.cs
//  
// Author:
//       Jon Thysell <thysell@gmail.com>
// 
// Copyright (c) 2015, 2017 Jon Thysell <http://jonthysell.com>
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

namespace com.jonthysell.Chordious.Core
{
    public class PathUtils
    {
        public static string Clean(string path)
        {
            if (null == path)
            {
                path = "";
            }

            path = path.Trim();
            path = path.TrimEnd(PathUtils.PathSeperator[0]);

            if (!path.StartsWith(PathUtils.PathRoot))
            {
                path = PathUtils.PathRoot + path;
            }

            return path;
        }

        public static string Join(string path1, string path2)
        {

            if (StringUtils.IsNullOrWhiteSpace(path2))
            {
                throw new ArgumentNullException("path2");
            }

            path1 = Clean(path1);
            path2 = path2.Trim();

            return string.Join(PathSeperator, path1, path2);
        }

        public static string PathSeperator = "/";

        public static string PathRoot = "/";
    }
}
