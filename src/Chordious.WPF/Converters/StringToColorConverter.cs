// Copyright (c) Jon Thysell <http://jonthysell.com>
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;

using Chordious.Core.ViewModel;

using Xceed.Wpf.Toolkit;

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

        public static bool TryParseColorItem(string str, out ColorItem result)
        {
            try
            {
                Color color = ParseColor(str);
                result = new ColorItem(color, ColorToString(color));
                return true;
            }
            catch { }

            result = default;
            return false;
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