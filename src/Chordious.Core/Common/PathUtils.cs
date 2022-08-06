// Copyright (c) Jon Thysell <http://jonthysell.com>
// Licensed under the MIT License.

using System;

namespace Chordious.Core
{
    public class PathUtils
    {
        public static string Clean(string path)
        {
            if (path is null)
            {
                path = "";
            }

            path = path.Trim();
            path = path.TrimEnd(PathSeperator[0]);

            if (!path.StartsWith(PathRoot))
            {
                path = PathRoot + path;
            }

            return path;
        }

        public static string Join(string path1, string path2)
        {

            if (StringUtils.IsNullOrWhiteSpace(path2))
            {
                throw new ArgumentNullException(nameof(path2));
            }

            path1 = Clean(path1);
            path2 = path2.Trim();

            return string.Join(PathSeperator, path1, path2);
        }

        public static readonly string PathSeperator = "/";

        public static readonly string PathRoot = "/";
    }
}
