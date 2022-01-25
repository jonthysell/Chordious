// Copyright (c) Jon Thysell <http://jonthysell.com>
// Licensed under the MIT License.

// Borrowed from http://tech.pro/tutorial/893/wpf-snippet-reliably-getting-the-mouse-position

using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Media;

namespace Chordious.WPF
{
    public static class MouseUtils
    {
        public static Point CorrectGetPosition(Visual relativeTo)
        {
            NativeMethods.Win32Point w32Mouse = new NativeMethods.Win32Point();
            NativeMethods.GetCursorPos(ref w32Mouse);
            return relativeTo.PointFromScreen(new Point(w32Mouse.X, w32Mouse.Y));
        }
    }

    internal static partial class NativeMethods
    {
        [StructLayout(LayoutKind.Sequential)]
        internal struct Win32Point
        {
            public int X;
            public int Y;
        };

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool GetCursorPos(ref Win32Point pt);
    }
}