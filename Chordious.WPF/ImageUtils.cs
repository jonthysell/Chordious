// 
// ImageUtils.cs
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
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Media.Imaging;

using Svg;

using SharpVectors.Dom.Svg;
using SharpVectors.Renderers.Gdi;
using SharpVectors.Renderers.Forms;

namespace com.jonthysell.Chordious.WPF
{
    public class ImageUtils
    {
        public static Bitmap SvgTextToBitmap(string svgText)
        {
            return SvgTextToBitmap(svgText, 0, 0, SvgRenderer.SvgSharp);
        }

        public static Bitmap SvgTextToBitmap(string svgText, int width, int height, SvgRenderer svgRenderer)
        {
            if (String.IsNullOrEmpty(svgText))
            {
                throw new ArgumentNullException("svgText");
            }

            Bitmap svgBitmap = null;

            if (svgRenderer == SvgRenderer.SvgSharp)
            {
                Svg.SvgDocument doc = Svg.SvgDocument.FromSvg<Svg.SvgDocument>(svgText);
                svgBitmap = doc.Draw();
            }
            else if (svgRenderer == SvgRenderer.SharpVectors)
            {
                // make SVG document with associated window so we can render it.
                SharpVectors.Dom.Svg.SvgWindow window = new SvgPictureBoxWindow(width, height, new GdiGraphicsRenderer());

                SharpVectors.Dom.Svg.SvgDocument doc = new SharpVectors.Dom.Svg.SvgDocument(window);
                doc.LoadXml(svgText);

                window.Renderer.Render(doc);

                svgBitmap = ((GdiGraphicsRenderer)window.Renderer).RasterImage;
            }

            return svgBitmap;
        }

        public static Bitmap CenterAndScale(Bitmap source, float width, float height, SvgRenderer svgRenderer)
        {
            float totalWidth = (float)source.Width;
            float totalHeight = (float)source.Height;

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
    }

    public enum SvgRenderer
    {
        SvgSharp,
        SharpVectors,
    }
}
