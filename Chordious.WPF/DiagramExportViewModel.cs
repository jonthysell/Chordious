// 
// DiagramExportViewModel.cs
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
using System.Collections.Specialized;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing.Imaging;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;

using com.jonthysell.Chordious.Core.ViewModel;

namespace com.jonthysell.Chordious.WPF
{
    public class DiagramExportViewModel : DiagramExportViewModelBase
    {
        public string OutputPath
        {
            get
            {
                string outputPath = "";
                try
                {
                    outputPath = GetSetting("diagramexport.outputpath");
                }
                catch (Exception)
                {
                    outputPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                }
                return outputPath;
            }
            set
            {
                SetSetting("diagramexport.outputpath", value);
                RaisePropertyChanged("OutputPath");
                RaisePropertyChanged("ExampleFilenameFormat");
            }
        }

        public string SelectedFilenameFormat
        {
            get
            {
                return GetSetting("diagramexport.filenameformat");
            }
            set
            {
                if (!String.IsNullOrWhiteSpace(value))
                {
                    if (!FilenameFormats.Contains(value))
                    {
                        FilenameFormats.Add(value);
                    }
                    SetSetting("diagramexport.filenameformat", value);
                    RaisePropertyChanged("SelectedFilenameFormat");
                    RaisePropertyChanged("ExampleFilenameFormat");
                }
            }
        }

        public ObservableCollection<string> FilenameFormats
        {
            get
            {
                return _filenameFormats;
            }
            set
            {
                if (null == value)
                {
                    throw new ArgumentNullException();
                }
                _filenameFormats = value;
                RaisePropertyChanged("FilenameFormats");
            }
        }
        private ObservableCollection<string> _filenameFormats; 

        public string FilenameFormatHelp
        {
            get
            {
                return  String.Join("\n",
                    "%c - Diagram collection name",
                    "%t - Diagram title text",
                    "%h - Diagram height (in pixels)",
                    "%w - Diagram width (in pixels)",
                    "%x - Image extension (lowercase)",
                    "%X - Image extension (uppercase)",
                    "%0 - Diagram number (starts at 0)",
                    "%1 - Diagram number (starts at 1)",
                    "%# - Total number of diagrams to be exported",
                    "%% - Percent sign");
            }
        }

        public string ExampleFilenameFormat
        {
            get
            {
                return GetFullFilePath();
            }
        }

        public int SelectedExportFormatIndex
        {
            get
            {
                return (int)ExportFormat;
            }
            set
            {
                ExportFormat = (ExportFormat)value;
            }
        }

        private ExportFormat ExportFormat
        {
            get
            {
                ExportFormat result;

                if (Enum.TryParse<ExportFormat>(GetSetting("diagramexport.exportformat"), out result))
                {
                    return result;
                }

                return ExportFormat.SVG;
            }
            set
            {
                SetSetting("diagramexport.exportformat", value);
                RaisePropertyChanged("ExportFormat");
                RaisePropertyChanged("SelectedExportFormatIndex");
                RaisePropertyChanged("ExampleFilenameFormat");
            }
        }

        public ObservableCollection<string> ExportFormats
        {
            get
            {
                return GetExportFormats();
            }
        }

        public bool OverwriteFiles
        {
            get
            {
                bool result;

                if (Boolean.TryParse(GetSetting("diagramexport.overwritefiles"), out result))
                {
                    return result;
                }

                return false;
            }
            set
            {
                SetSetting("diagramexport.overwritefiles", value);
                RaisePropertyChanged("OverwriteFiles");
            }
        }

        public RelayCommand ChooseOutputPath
        {
            get
            {
                return new RelayCommand(() =>
                {
                    try
                    {
                        string result;
                        if (FolderUtils.PromptForFolder(OutputPath, out result))
                        {
                            OutputPath = result;
                        }
                    }
                    catch (Exception ex)
                    {
                        ExceptionUtils.HandleException(ex);
                    }
                });
            }
        }

        private List<string> _createdFiles;

        public DiagramExportViewModel(ObservableCollection<ObservableDiagram> diagramsToExport, string collectionName) : base(diagramsToExport, collectionName)
        {
            _filenameFormats = GetFilenameFormats();

            _createdFiles = new List<string>();

            this.ExportStart += () =>
                {
                    _createdFiles.Clear();
                };

            this.ExportEnd += () =>
                {
                    _createdFiles.Clear();
                };
        }

        public override void ProcessClose()
        {
            SaveSettingsAsDefault();
        }

        private string GetFullFilePath(int diagramIndex = 0, bool overwriteFiles = true)
        {
            string outputPath = OutputPath;
            string filenameFormat = SelectedFilenameFormat;

            string filePath = "";
            for (int i = 0; i < SelectedFilenameFormat.Length; i++)
            {
                if (SelectedFilenameFormat[i] != '%')
                {
                    filePath += SelectedFilenameFormat[i];
                }
                else
                {
                    if (i + 1 < SelectedFilenameFormat.Length)
                    {
                        i++; // step forward to look at the next character
                        char nextChar = SelectedFilenameFormat[i];
                        switch(nextChar)
                        {
                            case 't':
                                filePath += CleanTitle(DiagramsToExport[diagramIndex].Title);
                                break;
                            case 'c':
                                filePath += CollectionName;
                                break;
                            case 'h':
                                filePath += DiagramsToExport[diagramIndex].TotalHeight.ToString();
                                break;
                            case 'w':
                                filePath += DiagramsToExport[diagramIndex].TotalWidth.ToString();
                                break;
                            case '0':
                                filePath += diagramIndex.ToString();
                                break;
                            case '1':
                                filePath += (diagramIndex + 1).ToString();
                                break;
                            case '#':
                                filePath += DiagramsToExport.Count.ToString();
                                break;
                            case 'x':
                                filePath += ExportFormat.ToString().ToLower();
                                break;
                            case 'X':
                                filePath += ExportFormat.ToString().ToUpper();
                                break;
                            case '%':
                                filePath += "%";
                                break;
                            default:
                                //i--; // back up so we don't miss the next character
                                break;
                        }
                    }
                }
            }

            string rawFileName = FolderUtils.CleanPath(Path.Combine(outputPath, filePath));

            string folder = Path.GetDirectoryName(rawFileName);
            string fileName = Path.GetFileNameWithoutExtension(rawFileName);
            string extension = Path.GetExtension(rawFileName);

            string testfileName = Path.Combine(folder, fileName + extension);

            int attempt = 1;
            bool foundFilename = false;

            while (!foundFilename)
            {
                bool fileAlreadyExists = File.Exists(testfileName);
                bool fileCreatedThisExport = _createdFiles.Contains(testfileName);

                if (!fileAlreadyExists || (fileAlreadyExists && !fileCreatedThisExport && overwriteFiles))
                {
                    foundFilename = true;
                }
                else
                {
                    testfileName = Path.Combine(folder, fileName + " (" + attempt + ")" + extension);
                    attempt++;
                }
            }

            return testfileName;
        }

        private string CleanTitle(string title)
        {
            if (String.IsNullOrWhiteSpace(title))
            {
                return "";
            }

            return FolderUtils.ReplaceChars(title.Trim(), Path.GetInvalidFileNameChars());
        }

        protected override Task ExportDiagramAsync(int diagramIndex)
        {
            return Task.Factory.StartNew(() =>
            {
                string filePath = GetFullFilePath(diagramIndex, OverwriteFiles);

                string directoryPath = Path.GetDirectoryName(filePath);

                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                ObservableDiagram od = DiagramsToExport[diagramIndex];

                string svgText = od.SvgText;
                int width = od.TotalWidth;
                int height = od.TotalHeight;

                using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                {
                    if (ExportFormat == ExportFormat.SVG)
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

                        if (ExportFormat == ExportFormat.PNG)
                        {
                            encoder = new PngBitmapEncoder();
                        }
                        else if (ExportFormat == ExportFormat.GIF)
                        {
                            encoder = new GifBitmapEncoder();
                        }
                        else if (ExportFormat == ExportFormat.JPG)
                        {
                            encoder = new JpegBitmapEncoder();
                            background = Background.White;
                        }

                        bmpImage = ImageUtils.SvgTextToBitmapImage(svgText, width, height, ImageFormat.Png, background);

                        encoder.Frames.Add(BitmapFrame.Create(bmpImage));
                        encoder.Save(fs);
                    }

                    _createdFiles.Add(filePath);
                }

            });
        }

        private ObservableCollection<string> GetFilenameFormats()
        {
            ObservableCollection<string> collection = new ObservableCollection<string>();

            collection.Add(SelectedFilenameFormat);

            foreach(string filenameFormat in DefaultFileNameFormats)
            {
                if (!collection.Contains(filenameFormat))
                {
                    collection.Add(filenameFormat);
                }
            }

            return collection;
        }

        private ObservableCollection<string> GetExportFormats()
        {
            ObservableCollection<string> collection = new ObservableCollection<string>();

            collection.Add("SVG");
            collection.Add("PNG");
            collection.Add("GIF");
            collection.Add("JPG");

            return collection;
        }

        private static string[] DefaultFileNameFormats = {
                                                             "%t.%x",
                                                             "%1.%x",
                                                             "diagram (%1 of %#).%x",
                                                             "%c\\%t.%x",
                                                             "%c\\%1.%x",
                                                             "%c\\diagram (%1 of %#).%x"
                                                         };
    }

    public enum ExportFormat
    {
        SVG = 0,
        PNG,
        GIF,
        JPG
    }
}
