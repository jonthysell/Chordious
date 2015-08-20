// 
// FolderUtils.cs
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
using System.IO;
using System.Windows.Forms;

namespace com.jonthysell.Chordious.WPF
{
    public class FolderUtils
    {
        public static bool PromptForFolder(string defaultPath, out string value)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();

            if (!String.IsNullOrWhiteSpace(defaultPath))
            {
                dialog.SelectedPath = defaultPath;
            }

            if (DialogResult.OK == dialog.ShowDialog())
            {
                value = dialog.SelectedPath;
                return true;
            }

            value = "";
            return false;
        }

        public static string CleanPath(string path)
        {
            if (String.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentNullException("path");
            }

            path = path.Trim();

            bool isUnc = path.StartsWith(@"\\");
            bool containsFile = !String.IsNullOrEmpty(Path.GetFileName(path));

            string[] split = path.Trim().Split(Path.DirectorySeparatorChar);

            string cleanPath = isUnc ? @"\\" : "";

            for (int i = 0; i < split.Length; i++)
            {
                string part = split[i].Trim();

                if (!String.IsNullOrWhiteSpace(part))
                {
                    if (i == 0 && !isUnc) // First item, aka root with drive letter
                    {
                        cleanPath += ReplaceChars(split[i], Path.GetInvalidPathChars());
                    }
                    else
                    {
                        cleanPath += ReplaceChars(split[i], Path.GetInvalidFileNameChars());
                    }

                    cleanPath += Path.DirectorySeparatorChar;
                }
            }

            if (containsFile)
            {
                cleanPath = cleanPath.TrimEnd(Path.DirectorySeparatorChar);
            }

            return cleanPath;
        }

        public static string ReplaceChars(string source, char[] oldValues, char? newValue = null)
        {
            if (String.IsNullOrWhiteSpace(source))
            {
                throw new ArgumentNullException("source");
            }

            foreach (char oldValue in oldValues)
            {
                source = source.Replace(oldValue.ToString(), (null == newValue) ? "" : newValue.ToString());
            }

            return source;
        }
    }
}
