// Copyright (c) Jon Thysell <http://jonthysell.com>
// Licensed under the MIT License.

using System;
using System.Globalization;
using System.Windows.Input;
using System.Windows.Data;
using System.Windows.Markup;

namespace Chordious.WPF
{
    public class IdleBoolToWaitCursorConverter : MarkupExtension, IValueConverter
    {
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return _converter ??= new IdleBoolToWaitCursorConverter();
        }
        private static IdleBoolToWaitCursorConverter _converter = null;

        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (null != value as bool?)
            {
                if (!(bool)value)
                {
                    return Cursors.Wait;
                }
            }

            return Cursors.Arrow;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }

        #endregion
    }
}