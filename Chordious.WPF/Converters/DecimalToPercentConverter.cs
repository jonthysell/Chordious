// 
// DecimalToPercentConverter.cs
//  
// Author:
//       Jon Thysell <thysell@gmail.com>
// 
// Copyright (c) 2016, 2017 Jon Thysell <http://jonthysell.com>
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
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace com.jonthysell.Chordious.WPF
{
    public class DecimalToPercentConverter : MarkupExtension, IValueConverter
    {
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return _converter ?? (_converter = new DecimalToPercentConverter());
        }
        private static DecimalToPercentConverter _converter = null;

        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType == typeof(string))
            {
                if (value is double)
                {
                    double dValue = (double)value;
                    return dValue.ToString("P0", culture);
                }
                else if (value is float)
                {
                    float fValue = (float)value;
                    return fValue.ToString("P0", culture);
                }
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string sValue = value as string;

            if (string.IsNullOrWhiteSpace(sValue))
            {
                return null;
            }

            sValue = sValue.Trim(' ', '\t', '%', '\n', '\r');

            if (targetType == typeof(double))
            {
                double pValue = 0.01 * double.Parse(sValue, culture);
                return Math.Round(pValue, 2);
            }
            else if (targetType == typeof(float))
            {
                float pValue = 0.01F * float.Parse(sValue, culture);
                return Math.Round(pValue, 2);
            }

            return value;
        }

        #endregion
    }
}