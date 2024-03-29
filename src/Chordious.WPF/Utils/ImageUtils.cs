﻿// Copyright (c) Jon Thysell <http://jonthysell.com>
// Licensed under the MIT License.

using System;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Media.Imaging;

using Svg;

using Chordious.Core.ViewModel;

using Chordious.WPF.Resources;

namespace Chordious.WPF
{
    public class ImageUtils
    {
        public static AppViewModel AppVM
        {
            get
            {
                return AppViewModel.Instance;
            }
        }

        #region SVG Rendering

        public static BitmapImage SvgTextToBitmapImage(string svgText, int width, int height, bool editMode)
        {
            Background background = editMode ? GetEditorRenderBackground() : GetRenderBackground();
            return SvgTextToBitmapImage(svgText, width, height, ImageFormat.Png, background, 1.0f);
        }

        public static BitmapImage SvgTextToBitmapImage(string svgText, int width, int height, ImageFormat imageFormat, Background background, float scaleFactor)
        {
            Bitmap diagram = SvgTextToBitmap(svgText, width, height, scaleFactor);

            if (background != Background.None)
            {
                diagram = AddBackground(diagram, background);
            }

            return BitmapToBitmapImage(diagram, imageFormat);
        }

        public static Bitmap SvgTextToBitmap(string svgText, int width, int height, float scaleFactor)
        {
            if (string.IsNullOrEmpty(svgText))
            {
                throw new ArgumentNullException(nameof(svgText));
            }

            if (width <= 0 || width > MaxBitmapDimension)
            {
                throw new ArgumentOutOfRangeException(nameof(width));
            }

            if (height <= 0 || height > MaxBitmapDimension)
            {
                throw new ArgumentOutOfRangeException(nameof(height));
            }

            if (scaleFactor <= 0 || scaleFactor > GetMaxScaleFactor(width, height))
            {
                throw new ArgumentOutOfRangeException(nameof(scaleFactor));
            }

            SvgDocument doc = SvgDocument.FromSvg<SvgDocument>(svgText);

            // SVG.NET doesn't render text properly on high DPI screens, correct it here
            float textScaleFactor = 1.0f / (float)IntegrationUtils.DpiScale;
            if (textScaleFactor != 1.0f)
            {
                doc.ApplyRecursive((element) =>
                {
                    if (element is SvgTextBase text)
                    {
                        text.FontSize = new SvgUnit(text.FontSize.Type, text.FontSize.Value * textScaleFactor);
                    }
                });
            }

            if (scaleFactor != 1.0f)
            {
                if (doc.Transforms is null)
                {
                    doc.Transforms = new Svg.Transforms.SvgTransformCollection();
                }
                doc.Transforms.Add(new Svg.Transforms.SvgScale(scaleFactor));
                doc.Width = new SvgUnit(scaleFactor * width);
                doc.Height = new SvgUnit(scaleFactor * height);
            }

            Bitmap svgBitmap = doc.Draw();
            return svgBitmap;
        }

        #endregion

        #region Manipulations

        public static Bitmap AddBackground(Bitmap source, Background background)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (background == Background.None)
            {
                throw new ArgumentOutOfRangeException(nameof(background));
            }

            Bitmap baseImage = new Bitmap(source.Width, source.Height);

            Graphics g = Graphics.FromImage(baseImage);

            if (background == Background.White)
            {
                g.Clear(Color.White);
            }
            else if (background == Background.Transparent)
            {
                Image transparent = Transparent16;
                for (int x = 0; x < source.Width; x += transparent.Width)
                {
                    for (int y = 0; y < source.Height; y += transparent.Height)
                    {
                        g.DrawImage(transparent, x, y, transparent.Width, transparent.Height);
                    }
                }
            }

            g.DrawImage(source, 0, 0, source.Width, source.Height);

            return baseImage;
        }

        public static Bitmap CenterAndScale(Bitmap source, float width, float height)
        {
            float totalWidth = source.Width;
            float totalHeight = source.Height;

            // Need to resize?
            if ((totalHeight != height || totalWidth != width))
            {
                // Figure out the ratio
                float ratioX = width / totalWidth;
                float ratioY = height / totalHeight;
                // use whichever multiplier is smaller
                float ratio = ratioX < ratioY ? ratioX : ratioY;

                // Calculate x,y
                float posX = (width - (totalWidth * ratio)) / 2.0f;
                float posY = (height - (totalHeight * ratio)) / 2.0f;

                Bitmap baseImage = new Bitmap((int)width, (int)height);

                Graphics g = Graphics.FromImage(baseImage);
                g.Clear(Color.White);
                g.DrawImage(source, posX, posY, (totalWidth * ratio), (totalHeight * ratio));

                return baseImage;
            }

            return source;
        }

        public static BitmapImage BitmapToBitmapImage(Bitmap bitmap, ImageFormat imageFormat)
        {
            BitmapImage bitmapImage = new BitmapImage();
            using (MemoryStream memory = new MemoryStream())
            {
                bitmap.Save(memory, imageFormat);
                memory.Position = 0;
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memory;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
            }
            return bitmapImage;
        }

        public static MemoryStream BitmapToPngStream(Bitmap image)
        {
            MemoryStream stream = new MemoryStream();
            image.Save(stream, ImageFormat.Png);
            return stream;
        }

        // Adapted from http://stackoverflow.com/questions/5270763/convert-an-image-into-wmf-with-net
        public static MemoryStream BitmapToMetafileStream(Bitmap image)
        {
            MemoryStream stream = new MemoryStream();

            using (Graphics g = Graphics.FromImage(image))
            {
                IntPtr hdc = g.GetHdc();
                using Metafile metafile = new Metafile(stream, hdc);
                g.ReleaseHdc();
                using Graphics g2 = Graphics.FromImage(metafile);
                g2.DrawImage(image, 0, 0);
            }

            return stream;
        }

        public static string BitmapToPngTempFile(Bitmap image, string title)
        {
            string tempPath = IntegrationUtils.GetTempPathAndCreateIfNecessary();

            string random = Path.GetRandomFileName().Substring(0, 8);

            string filePath = FolderUtils.CleanPath(Path.Combine(tempPath, string.IsNullOrWhiteSpace(title) ? $"chordious.{random}.png" : $"chordious.{FolderUtils.CleanTitle(title)}.{random}.png"));

            using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                BitmapEncoder encoder = new PngBitmapEncoder();

                BitmapImage bmpImage = BitmapToBitmapImage(image, ImageFormat.Png);
                BitmapMetadata frameMetadata = GetExportMetadata(ExportFormat.PNG);

                BitmapFrame frame = BitmapFrame.Create(bmpImage, null, frameMetadata, null);

                encoder.Frames.Add(frame);
                encoder.Save(fs);
            }

            return filePath;
        }

        public static Bitmap Transparent16
        {
            get
            {
                return _transparent16 ??= new Bitmap(Application.GetResourceStream(new Uri("pack://application:,,,/Resources/transparent16.png")).Stream);
            }
        }
        private static Bitmap _transparent16;

        #endregion

        #region Export

        public static void ExportImageFile(string svgText, int width, int height, ExportFormat exportFormat, float scaleFactor, string filePath)
        {
            if (string.IsNullOrWhiteSpace(svgText))
            {
                throw new ArgumentNullException(nameof(svgText));
            }

            if (width <= 0 || (exportFormat != ExportFormat.SVG && width > MaxBitmapDimension))
            {
                throw new ArgumentOutOfRangeException(nameof(width));
            }

            if (height <= 0 || (exportFormat != ExportFormat.SVG && height > MaxBitmapDimension))
            {
                throw new ArgumentOutOfRangeException(nameof(height));
            }

            if (scaleFactor <= 0 || (exportFormat != ExportFormat.SVG && scaleFactor > GetMaxScaleFactor(width, height)))
            {
                throw new ArgumentOutOfRangeException(nameof(scaleFactor));
            }

            if (string.IsNullOrWhiteSpace(filePath))
            {
                throw new ArgumentNullException(nameof(filePath));
            }

            string directoryPath = Path.GetDirectoryName(filePath);

            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            using FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write);
            if (exportFormat == ExportFormat.SVG)
            {
                StreamWriter sw = new StreamWriter(fs);
                sw.Write(svgText);
                sw.Flush();
            }
            else
            {
                BitmapEncoder encoder = null;

                Background background = Background.None;

                if (exportFormat == ExportFormat.PNG)
                {
                    encoder = new PngBitmapEncoder();
                }
                else if (exportFormat == ExportFormat.GIF)
                {
                    encoder = new GifBitmapEncoder();
                }
                else if (exportFormat == ExportFormat.JPG)
                {
                    encoder = new JpegBitmapEncoder();
                    background = Background.White;
                }

                BitmapImage bmpImage = SvgTextToBitmapImage(svgText, width, height, ImageFormat.Png, background, scaleFactor);
                BitmapMetadata frameMetadata = GetExportMetadata(exportFormat);

                BitmapFrame frame = BitmapFrame.Create(bmpImage, null, frameMetadata, null);

                encoder.Frames.Add(frame);
                encoder.Save(fs);
            }
        }

        private static BitmapMetadata GetExportMetadata(ExportFormat exportFormat)
        {
            BitmapMetadata metadata = null;

            string appName = AppViewModel.ProgramTitle;
            string comment = AppViewModel.Watermark;

            if (exportFormat == ExportFormat.PNG)
            {
                metadata = new BitmapMetadata("png");

                AddMetaData(metadata, _pngAppNameTags, appName);
                AddMetaData(metadata, _pngCommentTags, comment);
            }
            else if (exportFormat == ExportFormat.GIF)
            {
                metadata = new BitmapMetadata("gif");

                AddMetaData(metadata, _gifAppNameTags, appName);
                AddMetaData(metadata, _gifCommentTags, comment);
            }
            else if (exportFormat == ExportFormat.JPG)
            {
                metadata = new BitmapMetadata("jpg");

                AddMetaData(metadata, _jpgAppNameTags, appName);
                AddMetaData(metadata, _jpgCommentTags, comment);
            }

            return metadata;
        }

        private static void AddMetaData(BitmapMetadata metadata, string[] tags, string value)
        {
            if (metadata is null)
            {
                throw new ArgumentNullException(nameof(metadata));
            }

            if (tags is null)
            {
                throw new ArgumentNullException(nameof(tags));
            }

            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentNullException(nameof(value));
            }

            value = value.Trim();

            foreach (string tag in tags)
            {
                metadata.SetQuery(tag, value);
            }
        }

        private static readonly string[] _pngAppNameTags = { "/tEXt/Software" };
        private static readonly string[] _pngCommentTags = { "/tEXt/Comment" };

        private static readonly string[] _gifAppNameTags = { "/xmp/xmp:CreatorTool" };
        private static readonly string[] _gifCommentTags = { "/xmp/dc:description" };

        private static readonly string[] _jpgAppNameTags = { "/app1/ifd/{ushort=305}" };
        private static readonly string[] _jpgCommentTags = { "/app1/ifd/exif/{ushort=37510}" };

        #endregion

        #region Limits

        public static float GetMaxScaleFactor(int width, int height)
        {
            if (width <= 0 || width > MaxBitmapDimension)
            {
                throw new ArgumentOutOfRangeException(nameof(width));
            }

            if (height <= 0 || height > MaxBitmapDimension)
            {
                throw new ArgumentOutOfRangeException(nameof(height));
            }

            float maxByDimensionConstraint = MaxBitmapDimension / (float)Math.Max(width, height);

            float maxByMemoryConstraint = (float)Math.Sqrt(MaxBitmapDimension * (MaxBitmapDimension / (width * (double)height)));

            return Math.Min(maxByDimensionConstraint, maxByMemoryConstraint);
        }

        private static readonly int MaxBitmapDimension = 32768;

        #endregion

        #region Settings

        public static Background GetRenderBackground()
        {

            if (Enum.TryParse(AppVM.GetSetting("app.renderbackground"), out Background result))
            {
                return result;
            }

            return Background.None;
        }

        public static Background GetEditorRenderBackground()
        {

            if (Enum.TryParse(AppVM.GetSetting("diagrameditor.renderbackground"), out Background result))
            {
                return result;
            }

            return Background.None;
        }

        public static ObservableCollection<string> GetBackgrounds()
        {
            ObservableCollection<string> collection = new ObservableCollection<string>
            {
                Strings.BackgroundNoneFriendlyValue,
                Strings.BackgroundWhiteFriendlyValue,
                Strings.BackgroundTransparentFriendlyValue
            };

            return collection;
        }

        #endregion
    }

    public enum Background
    {
        None,
        White,
        Transparent
    }

    public enum ExportFormat
    {
        SVG = 0,
        PNG,
        GIF,
        JPG
    }
}
