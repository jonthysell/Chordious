// Copyright (c) Jon Thysell <http://jonthysell.com>
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Media;

namespace Chordious.WPF
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
                throw new ArgumentNullException(nameof(imageSource));
            }

            GetDisabledImage(imageSource);
        }

        protected static AutoDisableImage GetImageWithSource(DependencyObject source)
        {
            if (!(source is AutoDisableImage image))
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
            if (_cachedDisabledImages.TryGetValue(imageSource, out ImageSource disabledImage))
            {
                return disabledImage;
            }

            BitmapSource bitmapImage = imageSource is BitmapSource bitmapSource ? bitmapSource : new BitmapImage(new Uri(imageSource.ToString()));
            disabledImage = new FormatConvertedBitmap(bitmapImage, PixelFormats.Indexed2, BitmapPalettes.Gray4, 0);

            _cachedDisabledImages.Add(imageSource, disabledImage);

            return disabledImage;
        }

        protected static Dictionary<ImageSource, ImageSource> _cachedDisabledImages = new Dictionary<ImageSource, ImageSource>();
    }
}
