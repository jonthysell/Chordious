// 
// AutoDisableImage.cs
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
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Media;

namespace com.jonthysell.Chordious.WPF
{
    public class AutoDisableImage : Image
    {
        public ImageSource OriginalImageSource
        {
            get
            {
                return _originalImageSource ?? (_originalImageSource = Source);
            }
        }
        private ImageSource _originalImageSource;

        public ImageSource DisabledImageSource
        {
            get
            {
                return _disabledImageSource ?? (_disabledImageSource = GetDisabledImage(OriginalImageSource));
            }
        }
        private ImageSource _disabledImageSource;

        static AutoDisableImage()
        {
            IsEnabledProperty.OverrideMetadata(typeof(AutoDisableImage), new FrameworkPropertyMetadata(true, new PropertyChangedCallback(OnAutoDisableImageIsEnabledPropertyChanged)));
        }

        public static void LoadImage(ImageSource imageSource)
        {
            if (null == imageSource)
            {
                throw new ArgumentNullException("imageSource");
            }

            GetDisabledImage(imageSource);
        }

        protected static AutoDisableImage GetImageWithSource(DependencyObject source)
        {
            AutoDisableImage image = source as AutoDisableImage;

            if (null == image)
            {
                return null;
            }

            if (null == image.Source)
            {
                return null;
            }

            return image;
        }

        protected static void OnAutoDisableImageIsEnabledPropertyChanged(DependencyObject source, DependencyPropertyChangedEventArgs args)
        {
            AutoDisableImage image = GetImageWithSource(source);
            if (null != image)
            {
                bool isEnabled = Convert.ToBoolean(args.NewValue);

                image.Source = isEnabled ? image.OriginalImageSource : image.DisabledImageSource;
                image.Opacity = isEnabled ? 1.0 : 0.5;
                image.OpacityMask = isEnabled ? null : new ImageBrush(image.OriginalImageSource);
            }
        }

        protected static ImageSource GetDisabledImage(ImageSource imageSource)
        {
            ImageSource disabledImage = null;
            if (_cachedDisabledImages.TryGetValue(imageSource, out disabledImage))
            {
                return disabledImage;
            }

            BitmapSource bitmapImage = null;

            if (imageSource is BitmapSource)
            {
                bitmapImage = (BitmapSource)imageSource;
            }
            else
            {
                bitmapImage = new BitmapImage(new Uri(imageSource.ToString()));
            }

            disabledImage = new FormatConvertedBitmap(bitmapImage, PixelFormats.Indexed2, BitmapPalettes.Gray4, 0);

            _cachedDisabledImages.Add(imageSource, disabledImage);

            return disabledImage;
        }

        protected static Dictionary<ImageSource, ImageSource> _cachedDisabledImages = new Dictionary<ImageSource, ImageSource>();
    }
}
