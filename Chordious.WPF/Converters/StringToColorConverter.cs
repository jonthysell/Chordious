// 
// StringToColorConverter.cs
//  
// Author:
//       Jon Thysell <thysell@gmail.com>
// 
// Copyright (c) 2020 Jon Thysell <http://jonthysell.com>
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
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;

using Chordious.Core.ViewModel;

namespace Chordious.WPF
{
    public class StringToColorConverter : MarkupExtension, IValueConverter
    {
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return _converter ?? (_converter = new StringToColorConverter());
        }
        private static StringToColorConverter _converter = null;

        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string str && TryParseColor(str, out Color result))
            {
                return result;
            }

            return default(Color);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Color color)
            {
                return ColorToString(color);
            }

            return default(string);
        }

        #endregion

        public static bool TryParseColor(string str, out Color result)
        {
            try
            {
                result = ParseColor(str);
                return true;
            }
            catch { }

            result = default;
            return false;
        }

        public static Color ParseColor(string str)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                throw new ArgumentNullException(nameof(str));
            }

            str = str.Trim();

            if (ColorUtils.TryParseRGB(str, out byte r, out byte g, out byte b))
            {
                return Color.FromRgb(r, g, b);
            }
            else if (NamedColors.TryGetValue(str, out Color result))
            {
                return result;
            }

            throw new ArgumentOutOfRangeException(nameof(str));
        }

        public static string ColorToString(Color color)
        {
            foreach (var kvp in NamedColors)
            {
                if (kvp.Value == color)
                {
                    return kvp.Key;
                }
            }

            return $"#{color.R:X2}{color.G:X2}{color.B:X2}";
        }

        public static IReadOnlyDictionary<string, Color> NamedColors = new Dictionary<string, Color>
        {
            { "Aqua", Colors.Aqua },
            { "Black", Colors.Black },
            { "Blue", Colors.Blue },
            { "Fuchsia", Colors.Fuchsia},
            { "Gray", Colors.Gray },
            { "Green", Colors.Green },
            { "Lime", Colors.Lime },
            { "Maroon", Colors.Maroon },
            { "Navy", Colors.Navy },
            { "Olive", Colors.Olive },
            { "Purple", Colors.Purple },
            { "Red", Colors.Red },
            { "Silver", Colors.Silver },
            { "Teal", Colors.Teal },
            { "White", Colors.White },
            { "Yellow", Colors.Yellow },
        };
    }
}