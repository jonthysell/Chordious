// 
// SvgTextToImageConverter.cs
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
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media.Imaging;

namespace com.jonthysell.Chordious.WPF
{
    public class SvgTextToImageConverter : MarkupExtension, IValueConverter
    {
        private static SvgTextToImageConverter _converter = null;
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (_converter == null)
            {
                _converter = new SvgTextToImageConverter();
            }
            return _converter;
        }

        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                if (null != value as IEnumerable<string>)
                {
                    ObservableCollection<DiagramItem> diagramItems = new ObservableCollection<DiagramItem>();
                    foreach (string svgText in (value as IEnumerable<string>))
                    {
                        diagramItems.Add(new DiagramItem(svgText));
                    }
                    return diagramItems;
                }
                else if (null != value as string)
                {
                    return new DiagramItem(value as string);
                }
            }
            catch { }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }

        #endregion

        
    }

    public class DiagramItem
    {
        public string SvgText { get; private set; }

        public BitmapImage BitmapImage
        {
            get
            {
                if (null == _bitmapImage)
                {
                    _bitmapImage = ImageUtils.BitmapToBitmapImage(ImageUtils.SvgTextToBitmap(SvgText), ImageFormat.Png);
                }
                return _bitmapImage;
            }
        }
        private BitmapImage _bitmapImage;

        public int Height
        {
            get
            {
                return BitmapImage.PixelHeight;
            }
        }
        
        public int Width
        {
            get
            {
                return BitmapImage.PixelWidth;
            }
        }

        public DiagramItem(string svgText)
        {
            SvgText = svgText;
        }
    }
}