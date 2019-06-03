// 
// cs
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
using System.Diagnostics;
using System.IO;

using Microsoft.Win32;

using GalaSoft.MvvmLight.Messaging;

using com.jonthysell.Chordious.Core.ViewModel;

namespace com.jonthysell.Chordious.WPF
{
    public class MessageHandlers
    {
        public static void RegisterMessageHandlers(object recipient)
        {
            Messenger.Default.Register<ChordiousMessage>(recipient, (message) => ShowNotification(message));
            Messenger.Default.Register<ExceptionMessage>(recipient, (message) => ShowException(message));
            Messenger.Default.Register<ConfirmationMessage>(recipient, (message) => ConfirmAction(message));
            Messenger.Default.Register<PromptForTextMessage>(recipient, (message) => PromptForText(message));

            Messenger.Default.Register<LaunchUrlMessage>(recipient, (message) => LaunchUrl(message));

            Messenger.Default.Register<ShowChordFinderMessage>(recipient, (message) => ShowChordFinder(message));
            Messenger.Default.Register<ShowScaleFinderMessage>(recipient, (message) => ShowScaleFinder(message));

            Messenger.Default.Register<ShowDiagramLibraryMessage>(recipient, (message) => ShowDiagramLibrary(message));

            Messenger.Default.Register<ShowDiagramEditorMessage>(recipient, (message) => ShowDiagramEditor(message));
            Messenger.Default.Register<ShowDiagramMarkEditorMessage>(recipient, (message) => ShowDiagramMarkEditor(message));
            Messenger.Default.Register<ShowDiagramFretLabelEditorMessage>(recipient, (message) => ShowDiagramFretLabelEditor(message));
            Messenger.Default.Register<ShowDiagramBarreEditorMessage>(recipient, (message) => ShowDiagramBarreEditor(message));
            Messenger.Default.Register<ShowDiagramStyleEditorMessage>(recipient, (message) => ShowDiagramStyleEditor(message));

            Messenger.Default.Register<ShowInstrumentManagerMessage>(recipient, (message) => ShowInstrumentManager(message));
            Messenger.Default.Register<ShowInstrumentEditorMessage>(recipient, (message) => ShowInstrumentEditor(message));
            Messenger.Default.Register<ShowTuningEditorMessage>(recipient, (message) => ShowTuningEditor(message));

            Messenger.Default.Register<ShowChordQualityManagerMessage>(recipient, (message) => ShowChordQualityManager(message));
            Messenger.Default.Register<ShowChordQualityEditorMessage>(recipient, (message) => ShowChordQualityEditor(message));

            Messenger.Default.Register<ShowScaleManagerMessage>(recipient, (message) => ShowScaleManager(message));
            Messenger.Default.Register<ShowScaleEditorMessage>(recipient, (message) => ShowScaleEditor(message));

            Messenger.Default.Register<ShowOptionsMessage>(recipient, (message) => ShowOptions(message));

            Messenger.Default.Register<ShowLicensesMessage>(recipient, (message) => ShowLicense(message));

            Messenger.Default.Register<ShowAdvancedDataMessage>(recipient, (message) => ShowAdvancedData(message));

            Messenger.Default.Register<ShowDiagramExportMessage>(recipient, (message) => ShowDiagramExport(message));

            Messenger.Default.Register<ShowConfigImportMessage>(recipient, (message) => ShowConfigImport(message));
            Messenger.Default.Register<PromptForConfigInputStreamMessage>(recipient, (message) => PromptForConfigInputStream(message));

            Messenger.Default.Register<ShowConfigExportMessage>(recipient, (message) => ShowConfigExport(message));
            Messenger.Default.Register<PromptForConfigOutputStreamMessage>(recipient, (message) => PromptForConfigOutputStream(message));

            Messenger.Default.Register<ShowDiagramCollectionSelectorMessage>(recipient, (message) => ShowDiagramCollectionSelector(message));
            
            Messenger.Default.Register<PromptForLegacyImportMessage>(recipient, (message) => PromptForLegacyImport(message));
        }

        public static void UnregisterMessageHandlers(object recipient)
        {
            Messenger.Default.Unregister<ChordiousMessage>(recipient);
            Messenger.Default.Unregister<ExceptionMessage>(recipient);
            Messenger.Default.Unregister<ConfirmationMessage>(recipient);
            Messenger.Default.Unregister<PromptForTextMessage>(recipient);

            Messenger.Default.Unregister<LaunchUrlMessage>(recipient);

            Messenger.Default.Unregister<ShowChordFinderMessage>(recipient);
            Messenger.Default.Unregister<ShowScaleFinderMessage>(recipient);

            Messenger.Default.Unregister<ShowDiagramLibraryMessage>(recipient);

            Messenger.Default.Unregister<ShowDiagramEditorMessage>(recipient);
            Messenger.Default.Unregister<ShowDiagramMarkEditorMessage>(recipient);
            Messenger.Default.Unregister<ShowDiagramFretLabelEditorMessage>(recipient);
            Messenger.Default.Unregister<ShowDiagramBarreEditorMessage>(recipient);
            Messenger.Default.Unregister<ShowDiagramStyleEditorMessage>(recipient);

            Messenger.Default.Unregister<ShowInstrumentManagerMessage>(recipient);

            Messenger.Default.Unregister<ShowInstrumentEditorMessage>(recipient);
            Messenger.Default.Unregister<ShowTuningEditorMessage>(recipient);

            Messenger.Default.Unregister<ShowChordQualityManagerMessage>(recipient);
            Messenger.Default.Unregister<ShowChordQualityEditorMessage>(recipient);

            Messenger.Default.Unregister<ShowScaleManagerMessage>(recipient);
            Messenger.Default.Unregister<ShowScaleEditorMessage>(recipient);
            
            Messenger.Default.Unregister<ShowOptionsMessage>(recipient);

            Messenger.Default.Unregister<ShowLicensesMessage>(recipient);

            Messenger.Default.Unregister<ShowAdvancedDataMessage>(recipient);

            Messenger.Default.Unregister<ShowDiagramExportMessage>(recipient);

            Messenger.Default.Unregister<ShowConfigImportMessage>(recipient);
            Messenger.Default.Unregister<PromptForConfigInputStreamMessage>(recipient);

            Messenger.Default.Unregister<ShowConfigExportMessage>(recipient);
            Messenger.Default.Unregister<PromptForConfigOutputStreamMessage>(recipient);

            Messenger.Default.Unregister<ShowDiagramCollectionSelectorMessage>(recipient);

            Messenger.Default.Unregister<PromptForLegacyImportMessage>(recipient);
        }

        private static void ShowNotification(ChordiousMessage message)
        {
            InformationWindow window = new InformationWindow
            {
                DataContext = message.InformationVM
            };
            message.InformationVM.RequestClose += () =>
            {
                window.Close();
            };
            window.ShowDialog();
            message.Process();
        }

        private static void ShowException(ExceptionMessage message)
        {
            ExceptionWindow window = new ExceptionWindow
            {
                DataContext = message.ExceptionVM
            };
            message.ExceptionVM.RequestClose += () =>
            {
                window.Close();
            };
            window.ShowDialog();
        }

        private static void LaunchUrl(LaunchUrlMessage message)
        {
            Process.Start(message.Url, null);
        }

        private static void ConfirmAction(ConfirmationMessage message)
        {
            ConfirmationWindow window = new ConfirmationWindow
            {
                DataContext = message.ConfirmationVM
            };
            message.ConfirmationVM.RequestClose += () =>
            {
                window.Close();
            };

            if (message.ConfirmationVM.DisplayDialog)
            {
                window.ShowDialog();
            }

            message.Process();
        }

        private static void PromptForText(PromptForTextMessage message)
        {
            TextPromptWindow window = new TextPromptWindow
            {
                DataContext = message.TextPromptVM
            };
            message.TextPromptVM.RequestClose += () =>
            {
                window.Close();
            };
            window.ShowDialog();
        }

        private static void ShowChordFinder(ShowChordFinderMessage message)
        {
            ChordFinderWindow window = new ChordFinderWindow(message.ChordFinderVM);
            window.VM.RequestClose += () =>
            {
                window.Close();
            };
            window.ShowDialog();
            message.Process();
        }

        private static void ShowScaleFinder(ShowScaleFinderMessage message)
        {
            ScaleFinderWindow window = new ScaleFinderWindow(message.ScaleFinderVM);
            window.VM.RequestClose += () =>
            {
                window.Close();
            };
            window.ShowDialog();
            message.Process();
        }

        private static void ShowDiagramLibrary(ShowDiagramLibraryMessage message)
        {
            DiagramLibraryWindow window = new DiagramLibraryWindow
            {
                DataContext = message.DiagramLibraryVM
            };
            message.DiagramLibraryVM.RequestClose += () =>
            {
                window.Close();
            };
            window.ShowDialog();
            message.Process();
        }

        private static void ShowInstrumentManager(ShowInstrumentManagerMessage message)
        {
            InstrumentManagerWindow window = new InstrumentManagerWindow
            {
                DataContext = message.InstrumentManagerVM
            };
            message.InstrumentManagerVM.RequestClose += () =>
            {
                window.Close();
            };
            window.ShowDialog();
            message.Process();
        }

        private static void ShowInstrumentEditor(ShowInstrumentEditorMessage message)
        {
            InstrumentEditorWindow window = new InstrumentEditorWindow
            {
                DataContext = message.InstrumentEditorVM
            };
            message.InstrumentEditorVM.RequestClose += () =>
            {
                window.Close();
            };
            window.ShowDialog();
        }

        private static void ShowTuningEditor(ShowTuningEditorMessage message)
        {
            TuningEditorWindow window = new TuningEditorWindow
            {
                DataContext = message.TuningEditorVM
            };
            message.TuningEditorVM.RequestClose += () =>
            {
                window.Close();
            };
            window.ShowDialog();
            message.Process();
        }

        private static void ShowChordQualityManager(ShowChordQualityManagerMessage message)
        {
            NamedIntervalManagerWindow window = new NamedIntervalManagerWindow(message.ChordQualityManagerVM);
            window.VM.RequestClose += () =>
            {
                window.Close();
            };
            window.ShowDialog();
            message.Process();
        }

        private static void ShowChordQualityEditor(ShowChordQualityEditorMessage message)
        {
            ChordQualityEditorWindow window = new ChordQualityEditorWindow
            {
                DataContext = message.ChordQualityEditorVM
            };
            message.ChordQualityEditorVM.RequestClose += () =>
            {
                window.Close();
            };
            window.ShowDialog();
        }

        private static void ShowScaleManager(ShowScaleManagerMessage message)
        {
            NamedIntervalManagerWindow window = new NamedIntervalManagerWindow(message.ScaleManagerVM);
            window.VM.RequestClose += () =>
            {
                window.Close();
            };
            window.ShowDialog();
            message.Process();
        }

        private static void ShowScaleEditor(ShowScaleEditorMessage message)
        {
            ScaleEditorWindow window = new ScaleEditorWindow
            {
                DataContext = message.ScaleEditorVM
            };
            message.ScaleEditorVM.RequestClose += () =>
            {
                window.Close();
            };
            window.ShowDialog();
        }

        private static void ShowDiagramEditor(ShowDiagramEditorMessage message)
        {
            DiagramEditorWindow window = new DiagramEditorWindow();
            message.DiagramEditorVM = new DiagramEditorViewModelExtended(message.Diagram, message.IsNew);
            window.DataContext = message.DiagramEditorVM;
            message.DiagramEditorVM.RequestClose += () =>
            {
                window.Close();
            };
            window.ShowDialog();
            message.Process();
        }

        private static void ShowDiagramMarkEditor(ShowDiagramMarkEditorMessage message)
        {
            DiagramMarkEditorWindow window = new DiagramMarkEditorWindow
            {
                DataContext = message.DiagramMarkEditorVM
            };
            message.DiagramMarkEditorVM.RequestClose += () =>
            {
                window.Close();
            };
            window.ShowDialog();
            message.Process();
        }

        private static void ShowDiagramFretLabelEditor(ShowDiagramFretLabelEditorMessage message)
        {
            DiagramFretLabelEditorWindow window = new DiagramFretLabelEditorWindow
            {
                DataContext = message.DiagramFretLabelEditorVM
            };
            message.DiagramFretLabelEditorVM.RequestClose += () =>
            {
                window.Close();
            };
            window.ShowDialog();
            message.Process();
        }

        private static void ShowDiagramBarreEditor(ShowDiagramBarreEditorMessage message)
        {
            DiagramBarreEditorWindow window = new DiagramBarreEditorWindow
            {
                DataContext = message.DiagramBarreEditorVM
            };
            message.DiagramBarreEditorVM.RequestClose += () =>
            {
                window.Close();
            };
            window.ShowDialog();
            message.Process();
        }

        private static void ShowDiagramStyleEditor(ShowDiagramStyleEditorMessage message)
        {
            DiagramStyleEditorWindow window = new DiagramStyleEditorWindow
            {
                DataContext = message.DiagramStyleEditorVM
            };
            message.DiagramStyleEditorVM.RequestClose += () =>
            {
                window.Close();
            };
            window.ShowDialog();
            message.Process();
        }

        private static void ShowOptions(ShowOptionsMessage message)
        {
            OptionsWindow window = new OptionsWindow();
            message.OptionsVM = window.VM;
            message.OptionsVM.RequestClose += () =>
            {
                window.Close();
            };
            window.ShowDialog();
            message.Process();
        }

        private static void ShowLicense(ShowLicensesMessage message)
        {
            LicensesWindow window = new LicensesWindow();

            message.LicensesVM.Licenses.Add(ImageUtils.GetSvgNetLicense());

            window.DataContext = message.LicensesVM;
            message.LicensesVM.RequestClose += () =>
            {
                window.Close();
            };
            window.ShowDialog();
            message.Process();
        }

        private static void ShowAdvancedData(ShowAdvancedDataMessage message)
        {
            AdvancedDataWindow window = new AdvancedDataWindow
            {
                DataContext = message.AdvancedDataVM
            };
            message.AdvancedDataVM.RequestClose += () =>
            {
                window.Close();
            };
            window.ShowDialog();
            message.Process();
        }

        public static void ShowDiagramExport(ShowDiagramExportMessage message)
        {
            DiagramExportWindow window = new DiagramExportWindow();
            message.DiagramExportVM = new DiagramExportViewModel(message.DiagramsToExport, message.CollectionName);
            window.DataContext = message.DiagramExportVM;
            window.ShowDialog();
            message.Process();
        }

        private static void ShowConfigExport(ShowConfigExportMessage message)
        {
            ConfigPartsWindow window = new ConfigPartsWindow
            {
                DataContext = message.ConfigExportVM
            };
            message.ConfigExportVM.RequestClose += () =>
            {
                window.Close();
            };
            window.ShowDialog();
            message.Process();
        }

        public static void PromptForConfigOutputStream(PromptForConfigOutputStreamMessage message)
        {
            SaveFileDialog dialog = new SaveFileDialog
            {
                AddExtension = true,
                DefaultExt = "*.xml",
                Filter = "Chordious Config|*.xml",
                InitialDirectory = LastPath
            };

            bool? result = dialog.ShowDialog();

            if (result.HasValue && result.Value)
            {
                LastPath = Path.GetDirectoryName(dialog.FileName);
                string filename = dialog.FileName;
                message.Process(new FileStream(filename, FileMode.Create));
            }
        }

        private static void ShowConfigImport(ShowConfigImportMessage message)
        {
            ConfigPartsWindow window = new ConfigPartsWindow
            {
                DataContext = message.ConfigImportVM
            };
            message.ConfigImportVM.RequestClose += () =>
            {
                window.Close();
            };
            window.ShowDialog();
            message.Process();
        }

        public static void PromptForConfigInputStream(PromptForConfigInputStreamMessage message)
        {
            OpenFileDialog dialog = new OpenFileDialog
            {
                AddExtension = true,
                DefaultExt = "*.xml",
                Filter = "Chordious Config|*.xml",
                InitialDirectory = LastPath
            };

            bool? result = dialog.ShowDialog();

            if (result.HasValue && result.Value)
            {
                LastPath = Path.GetDirectoryName(dialog.FileName);
                string filename = dialog.FileName;
                message.Process(new FileStream(filename, FileMode.Open, FileAccess.Read));
            }
        }

        public static void ShowDiagramCollectionSelector(ShowDiagramCollectionSelectorMessage message)
        {
            DiagramCollectionSelectorWindow window = new DiagramCollectionSelectorWindow
            {
                DataContext = message.DiagramCollectionSelectorVM
            };
            message.DiagramCollectionSelectorVM.RequestClose += () =>
            {
                window.Close();
            };
            window.ShowDialog();
        }

        public static void PromptForLegacyImport(PromptForLegacyImportMessage message)
        {
            OpenFileDialog dialog = new OpenFileDialog
            {
                AddExtension = true,
                DefaultExt = "*.txt",
                Filter = "All Files|*.*|ChordLines|*.txt",
                InitialDirectory = LastPath
            };

            bool? result = dialog.ShowDialog();

            if (result.HasValue && result.Value)
            {
                LastPath = Path.GetDirectoryName(dialog.FileName);
                string filename = dialog.FileName;
                message.Process(Path.GetFileName(filename), new FileStream(filename, FileMode.Open, FileAccess.Read));
            }
        }

        private static string LastPath
        {
            get
            {
                string lastPath = "";
                try
                {
                    lastPath = AppViewModel.Instance.GetSetting("app.lastpath");
                    if (!Directory.Exists(lastPath))
                    {
                        lastPath = "";
                    }
                }
                catch (Exception) { }

                if (string.IsNullOrWhiteSpace(lastPath))
                {
                    lastPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                }

                return lastPath;
            }
            set
            {
                AppViewModel.Instance.SetSetting("app.lastpath", value);
            }
        }
    }
}
