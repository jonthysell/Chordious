// Copyright (c) Jon Thysell <http://jonthysell.com>
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;

using CommunityToolkit.Mvvm.Input;

using Chordious.Core.ViewModel;

using Chordious.WPF.Resources;

namespace Chordious.WPF
{
    public class DiagramExportViewModel : DiagramExportViewModelBase
    {
        public string OutputPathLabel
        {
            get
            {
                return Strings.DiagramExportOutputPathLabel;
            }
        }

        public string OutputPathToolTip
        {
            get
            {
                return Strings.DiagramExportOutputPathToolTip;
            }
        }

        public string OutputPath
        {
            get
            {
                string outputPath;
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
                if (!string.IsNullOrWhiteSpace(value))
                {
                    SetSetting("diagramexport.outputpath", value);
                }
                OnPropertyChanged(nameof(OutputPath));
                OnPropertyChanged(nameof(ExampleFilenameFormat));
            }
        }

        public string SelectedFilenameFormatLabel
        {
            get
            {
                return Strings.DiagramExportSelectedFilenameFormatLabel;
            }
        }

        public string SelectedFilenameFormatToolTip
        {
            get
            {
                return Strings.DiagramExportSelectedFilenameFormatToolTip;
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
                if (!string.IsNullOrWhiteSpace(value))
                {
                    if (!FilenameFormats.Contains(value))
                    {
                        FilenameFormats.Add(value);
                    }
                    SetSetting("diagramexport.filenameformat", value);
                }
                OnPropertyChanged(nameof(SelectedFilenameFormat));
                OnPropertyChanged(nameof(ExampleFilenameFormat));
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
                _filenameFormats = value ?? throw new ArgumentNullException();
                OnPropertyChanged(nameof(FilenameFormats));
            }
        }
        private ObservableCollection<string> _filenameFormats;

        public string ExampleFilenameFormatLabel
        {
            get
            {
                return Strings.DiagramExportExampleFilenameFormatLabel;
            }
        }

        public string ExampleFilenameFormatToolTip
        {
            get
            {
                return Strings.DiagramExportExampleFilenameFormatToolTip;
            }
        }

        public string ExampleFilenameFormat
        {
            get
            {
                return GetFullFilePath();
            }
        }

        public string SelectedExportFormatLabel
        {
            get
            {
                return Strings.DiagramExportSelectedExportFormatLabel;
            }
        }

        public string SelectedExportFormatToolTip
        {
            get
            {
                return Strings.DiagramExportSelectedExportFormatToolTip;
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

                if (Enum.TryParse(GetSetting("diagramexport.exportformat"), out ExportFormat result))
                {
                    return result;
                }

                return ExportFormat.SVG;
            }
            set
            {
                SetSetting("diagramexport.exportformat", value);
                OnPropertyChanged(nameof(ExportFormat));
                OnPropertyChanged(nameof(SelectedExportFormatIndex));
                OnPropertyChanged(nameof(ExampleFilenameFormat));
                OnPropertyChanged(nameof(CanScale));
            }
        }

        public ObservableCollection<string> ExportFormats
        {
            get
            {
                return _exportFormats ??= GetExportFormats();
            }
        }
        private ObservableCollection<string> _exportFormats;

        public string OverwriteFilesLabel
        {
            get
            {
                return Strings.DiagramExportOverwriteFilesLabel;
            }
        }

        public string OverwriteFilesToolTip
        {
            get
            {
                return Strings.DiagramExportOverwriteFilesToolTip;
            }
        }

        public bool OverwriteFiles
        {
            get
            {

                if (bool.TryParse(GetSetting("diagramexport.overwritefiles"), out bool result))
                {
                    return result;
                }

                return false;
            }
            set
            {
                SetSetting("diagramexport.overwritefiles", value);
                OnPropertyChanged(nameof(OverwriteFiles));
            }
        }

        public bool CanScale
        {
            get
            {
                return ExportFormat != ExportFormat.SVG;
            }
        }

        public string ScaleFactorLabel
        {
            get
            {
                return Strings.DiagramExportScaleFactorLabel;
            }
        }

        public string ScaleFactorToolTip
        {
            get
            {
                return Strings.DiagramExportScaleFactorToolTip;
            }
        }

        public float ScaleFactor
        {
            get
            {

                if (float.TryParse(GetSetting("diagramexport.scalefactor"), out float result))
                {
                    return result;
                }

                return 1.0f;
            }
            set
            {
                try
                {
                    if (value <= 0.0f || value > MaxScaleFactor)
                    {
                        throw new ArgumentOutOfRangeException();
                    }

                    SetSetting("diagramexport.scalefactor", value);
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
                finally
                {
                    OnPropertyChanged(nameof(ScaleFactor));
                }
            }
        }

        public float MaxScaleFactor => ImageUtils.GetMaxScaleFactor(MaxWidth, MaxHeight);

        public string ChooseOutputPathLabel
        {
            get
            {
                return Strings.DiagramExportChooseOutputPathLabel;
            }
        }

        public string ChooseOutputPathToolTip
        {
            get
            {
                return Strings.DiagramExportChooseOutputPathToolTip;
            }
        }

        public RelayCommand ChooseOutputPath
        {
            get
            {
                return _chooseOutputPath ??= new RelayCommand(() =>
                {
                    try
                    {
                        if (FolderUtils.PromptForFolder(OutputPath, out string result))
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
        private RelayCommand _chooseOutputPath;

        private readonly List<string> _createdFiles;

        public DiagramExportViewModel(ObservableCollection<ObservableDiagram> diagramsToExport, string collectionName) : base(diagramsToExport, collectionName)
        {
            _filenameFormats = GetFilenameFormats();

            _createdFiles = new List<string>();

            ExportStart += (sender, e) =>
            {
                _createdFiles.Clear();
            };

            ExportEnd += (sender, e) =>
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
                        switch (nextChar)
                        {
                            case 't':
                                filePath += FolderUtils.CleanTitle(DiagramsToExport[diagramIndex].Title);
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
                bool fileCreatedThisExport = _createdFiles.Exists(createdFile => createdFile.Equals(testfileName, StringComparison.CurrentCultureIgnoreCase));

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

        protected override Task ExportDiagramAsync(int diagramIndex)
        {
            return Task.Factory.StartNew(() =>
            {
                try
                {
                    ObservableDiagram od = DiagramsToExport[diagramIndex];

                    string svgText = od.SvgText;
                    int width = od.TotalWidth;
                    int height = od.TotalHeight;

                    string filePath = GetFullFilePath(diagramIndex, OverwriteFiles);

                    ImageUtils.ExportImageFile(svgText, width, height, ExportFormat, ScaleFactor, filePath);

                    _createdFiles.Add(filePath);
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            });
        }

        private ObservableCollection<string> GetFilenameFormats()
        {
            ObservableCollection<string> collection = new ObservableCollection<string>
            {
                SelectedFilenameFormat
            };

            foreach (string filenameFormat in DefaultFileNameFormats)
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
            ObservableCollection<string> collection = new ObservableCollection<string>
            {
                "SVG",
                "PNG",
                "GIF",
                "JPG"
            };

            return collection;
        }

        private static readonly string[] DefaultFileNameFormats = {
                                                             "%t.%x",
                                                             "%1.%x",
                                                             "diagram (%1 of %#).%x",
                                                             "%c\\%t.%x",
                                                             "%c\\%1.%x",
                                                             "%c\\diagram (%1 of %#).%x"
                                                         };
    }
}
