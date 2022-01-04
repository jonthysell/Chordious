// Copyright (c) Jon Thysell <http://jonthysell.com>
// Licensed under the MIT License.

using System;
using System.IO;
using System.Windows.Forms;

namespace Chordious.WPF
{
    public class FolderUtils
    {
        public static bool PromptForFolder(string defaultPath, out string value)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();

            if (!string.IsNullOrWhiteSpace(defaultPath))
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

        public static string CleanTitle(string title)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                return "";
            }

            return ReplaceChars(title.Trim(), Path.GetInvalidFileNameChars());
        }

        public static string CleanPath(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentNullException(nameof(path));
            }

            path = path.Trim();

            bool isUnc = path.StartsWith(@"\\");
            bool containsFile = !string.IsNullOrEmpty(Path.GetFileName(path));

            string[] split = path.Trim().Split(Path.DirectorySeparatorChar);

            string cleanPath = isUnc ? @"\\" : "";

            for (int i = 0; i < split.Length; i++)
            {
                string part = split[i].Trim();

                if (!string.IsNullOrWhiteSpace(part))
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
            if (string.IsNullOrWhiteSpace(source))
            {
                throw new ArgumentNullException(nameof(source));
            }

            foreach (char oldValue in oldValues)
            {
                source = source.Replace(oldValue.ToString(), (null == newValue) ? "" : newValue.ToString());
            }

            return source;
        }
    }
}
