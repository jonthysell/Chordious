// 
// ImageUtils.cs
//  
// Author:
//       Jon Thysell <thysell@gmail.com>
// 
// Copyright (c) 2015, 2016, 2017, 2019 Jon Thysell <http://jonthysell.com>
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
using System.Collections.ObjectModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
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

            float maxDimension = scaleFactor * Math.Max(width, height);

            SvgDocument doc = SvgDocument.FromSvg<Svg.SvgDocument>(svgText);

            if (scaleFactor != 1.0f)
            {
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
            if (null == source)
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

        public static Bitmap CenterAndScale(Bitmap source, float width, float height, SvgRenderer svgRenderer)
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
                using (Metafile metafile = new Metafile(stream, hdc))
                {
                    g.ReleaseHdc();
                    using (Graphics g2 = Graphics.FromImage(metafile))
                    {
                        g2.DrawImage(image, 0, 0);
                    }
                }
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

                BitmapImage bmpImage =  BitmapToBitmapImage(image, ImageFormat.Png);
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
                return _transparent16 ?? (_transparent16 = new Bitmap(Application.GetResourceStream(new Uri("pack://application:,,,/Resources/transparent16.png")).Stream));
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

            using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
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
        }

        private static BitmapMetadata GetExportMetadata(ExportFormat exportFormat)
        {
            BitmapMetadata metadata = null;

            string appName = AppVM.ProgramTitle;
            string comment = AppVM.Watermark;

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
            if (null == metadata)
            {
                throw new ArgumentNullException(nameof(metadata));
            }
           
            if (null == tags)
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

        #region SVG.NET License

        public static ObservableLicense GetSvgNetLicense()
        {
            return new ObservableLicense("SVG.NET", "Copyright © 2013-2019 vvvv.org", "Microsoft Public License (Ms-PL)", string.Join(Environment.NewLine + Environment.NewLine, _msPlLicense));
        }

        private static readonly string[] _msPlLicense = {
            @"This license governs use of the accompanying software. If you use the software, you accept this license. If you do not accept the license, do not use the software.",
            @"1. Definitions",
            @"The terms ""reproduce,"" ""reproduction,"" ""derivative works,"" and ""distribution"" have the same meaning here as under U.S. copyright law.",
            @"A ""contribution"" is the original software, or any additions or changes to the software.",
            @"A ""contributor"" is any person that distributes its contribution under this license.",
            @"""Licensed patents"" are a contributor's patent claims that read directly on its contribution.",
            @"2. Grant of Rights",
            @"(A) Copyright Grant- Subject to the terms of this license, including the license conditions and limitations in section 3, each contributor grants you a non-exclusive, worldwide, royalty-free copyright license to reproduce its contribution, prepare derivative works of its contribution, and distribute its contribution or any derivative works that you create.",
            @"(B) Patent Grant- Subject to the terms of this license, including the license conditions and limitations in section 3, each contributor grants you a non-exclusive, worldwide, royalty-free license under its licensed patents to make, have made, use, sell, offer for sale, import, and/or otherwise dispose of its contribution in the software or derivative works of the contribution in the software.",
            @"3. Conditions and Limitations",
            @"(A) No Trademark License- This license does not grant you rights to use any contributors' name, logo, or trademarks.",
            @"(B) If you bring a patent claim against any contributor over patents that you claim are infringed by the software, your patent license from such contributor to the software ends automatically.",
            @"(C) If you distribute any portion of the software, you must retain all copyright, patent, trademark, and attribution notices that are present in the software.",
            @"(D) If you distribute any portion of the software in source code form, you may do so only under this license by including a complete copy of this license with your distribution. If you distribute any portion of the software in compiled or object code form, you may only do so under a license that complies with this license.",
            @"(E) The software is licensed ""as-is."" You bear the risk of using it. The contributors give no express warranties, guarantees or conditions.You may have additional consumer rights under your local laws which this license cannot change.To the extent permitted under your local laws, the contributors exclude the implied warranties of merchantability, fitness for a particular purpose and non-infringement.",
        };

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
