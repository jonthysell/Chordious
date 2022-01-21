// Copyright (c) Jon Thysell <http://jonthysell.com>
// Licensed under the MIT License.

using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace Chordious.WPF
{
    public class InverseBoolConverter : MarkupExtension, IValueConverter
    {
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return _converter ?? (_converter = new InverseBoolConverter());
        }
        private static InverseBoolConverter _converter = null;

        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (null != value as bool?)
            {
                return !((bool?)value);
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Convert(value, targetType, parameter, culture);
        }

        #endregion
    }
}