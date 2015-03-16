// 
// SvgExporter.cs
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
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Media.Imaging;

using com.jonthysell.Chordious.Core.ViewModel;

namespace com.jonthysell.Chordious.WPF
{
    public class SvgExporter : IDiagramExporter
    {
        public string Filename { get; private set; }

        public ExportFormat ExportFormat { get; private set; }

        public bool SingleFile { get; private set; }

        public SvgExporter(string filename, ExportFormat exportFormat, bool singleFile)
        {
            if (String.IsNullOrWhiteSpace(filename))
            {
                throw new ArgumentNullException("filename");
            }

            Filename = filename;
            ExportFormat = exportFormat;
            SingleFile = singleFile;
        }

        public void Export(ObservableDiagram observableDiagram)
        {
            if (null == observableDiagram)
            {
                throw new ArgumentNullException("observableDiagram");
            }

            string folder = Path.GetDirectoryName(Filename);

            string fileName = Path.Combine(folder, GenerateFileName(observableDiagram));

            string svgText = observableDiagram.SvgText;

            using (FileStream fs = new FileStream(fileName, FileMode.Create, FileAccess.Write))
            {
                if (ExportFormat == ExportFormat.Svg)
                {
                    using (StreamWriter sw = new StreamWriter(fs))
                    {
                        sw.Write(svgText);
                    }
                }
                else
                {
                    BitmapImage bmpImage = null;
                    BitmapEncoder encoder = null;

                    Background background = Background.None;

                    if (ExportFormat == ExportFormat.Png)
                    {
                        encoder = new PngBitmapEncoder();
                    }
                    else if (ExportFormat == ExportFormat.Gif)
                    {
                        encoder = new GifBitmapEncoder();
                    }
                    else if (ExportFormat == ExportFormat.Jpg)
                    {
                        encoder = new JpegBitmapEncoder();
                        background = Background.White;
                    }

                    bmpImage = ImageUtils.SvgTextToBitmapImage(svgText, ImageFormat.Png, background);

                    encoder.Frames.Add(BitmapFrame.Create(bmpImage));
                    encoder.Save(fs);
                }
            }
        }

        public string GenerateFileName(ObservableDiagram observableDiagram)
        {
            string folder = Path.GetDirectoryName(Filename);
            string fileName = Path.GetFileNameWithoutExtension(Filename);
            string extension = "." + ExportFormat.ToString().ToLower();

            if (!String.IsNullOrWhiteSpace(Filename) && SingleFile)
            {
                return fileName + extension;
            }

            if (observableDiagram.TitleVisible && !String.IsNullOrWhiteSpace(observableDiagram.TitleText))
            {
                fileName += " " + observableDiagram.TitleText.Trim();
            }

            string testfileName = Path.Combine(folder, fileName + extension);

            int attempt = 1;

            while (File.Exists(testfileName))
            {
                testfileName = Path.Combine(folder, fileName + " (" + attempt + ")" + extension);
                attempt++;
            }

            return Path.GetFileName(testfileName);
        }
    }

    public enum ExportFormat
    {
        Svg = 0,
        Png,
        Gif,
        Jpg,
    }
}
