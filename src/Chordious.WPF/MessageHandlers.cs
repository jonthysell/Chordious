// Copyright (c) Jon Thysell <http://jonthysell.com>
// Licensed under the MIT License.

using System;
using System.Diagnostics;
using System.IO;

using Microsoft.Win32;

using CommunityToolkit.Mvvm.Messaging;

using Chordious.Core.ViewModel;

namespace Chordious.WPF
{
    public class MessageHandlers
    {
        public static void RegisterMessageHandlers(object recipient)
        {
            StrongReferenceMessenger.Default.Register<ChordiousMessage>(recipient, (recipient, message) => ShowNotification(message));
            StrongReferenceMessenger.Default.Register<ExceptionMessage>(recipient, (recipient, message) => ShowException(message));
            StrongReferenceMessenger.Default.Register<ConfirmationMessage>(recipient, (recipient, message) => ConfirmAction(message));
            StrongReferenceMessenger.Default.Register<PromptForTextMessage>(recipient, (recipient, message) => PromptForText(message));

            StrongReferenceMessenger.Default.Register<LaunchUrlMessage>(recipient, (recipient, message) => LaunchUrl(message));

            StrongReferenceMessenger.Default.Register<ShowChordFinderMessage>(recipient, (recipient, message) => ShowChordFinder(message));
            StrongReferenceMessenger.Default.Register<ShowScaleFinderMessage>(recipient, (recipient, message) => ShowScaleFinder(message));

            StrongReferenceMessenger.Default.Register<ShowDiagramLibraryMessage>(recipient, (recipient, message) => ShowDiagramLibrary(message));

            StrongReferenceMessenger.Default.Register<ShowDiagramEditorMessage>(recipient, (recipient, message) => ShowDiagramEditor(message));
            StrongReferenceMessenger.Default.Register<ShowDiagramMarkEditorMessage>(recipient, (recipient, message) => ShowDiagramMarkEditor(message));
            StrongReferenceMessenger.Default.Register<ShowDiagramFretLabelEditorMessage>(recipient, (recipient, message) => ShowDiagramFretLabelEditor(message));
            StrongReferenceMessenger.Default.Register<ShowDiagramBarreEditorMessage>(recipient, (recipient, message) => ShowDiagramBarreEditor(message));
            StrongReferenceMessenger.Default.Register<ShowDiagramStyleEditorMessage>(recipient, (recipient, message) => ShowDiagramStyleEditor(message));

            StrongReferenceMessenger.Default.Register<ShowInstrumentManagerMessage>(recipient, (recipient, message) => ShowInstrumentManager(message));
            StrongReferenceMessenger.Default.Register<ShowInstrumentEditorMessage>(recipient, (recipient, message) => ShowInstrumentEditor(message));
            StrongReferenceMessenger.Default.Register<ShowTuningEditorMessage>(recipient, (recipient, message) => ShowTuningEditor(message));

            StrongReferenceMessenger.Default.Register<ShowChordQualityManagerMessage>(recipient, (recipient, message) => ShowChordQualityManager(message));
            StrongReferenceMessenger.Default.Register<ShowChordQualityEditorMessage>(recipient, (recipient, message) => ShowChordQualityEditor(message));

            StrongReferenceMessenger.Default.Register<ShowScaleManagerMessage>(recipient, (recipient, message) => ShowScaleManager(message));
            StrongReferenceMessenger.Default.Register<ShowScaleEditorMessage>(recipient, (recipient, message) => ShowScaleEditor(message));

            StrongReferenceMessenger.Default.Register<ShowOptionsMessage>(recipient, (recipient, message) => ShowOptions(message));

            StrongReferenceMessenger.Default.Register<ShowLicensesMessage>(recipient, (recipient, message) => ShowLicense(message));

            StrongReferenceMessenger.Default.Register<ShowAdvancedDataMessage>(recipient, (recipient, message) => ShowAdvancedData(message));

            StrongReferenceMessenger.Default.Register<ShowDiagramExportMessage>(recipient, (recipient, message) => ShowDiagramExport(message));

            StrongReferenceMessenger.Default.Register<ShowConfigImportMessage>(recipient, (recipient, message) => ShowConfigImport(message));
            StrongReferenceMessenger.Default.Register<PromptForConfigInputStreamMessage>(recipient, (recipient, message) => PromptForConfigInputStream(message));

            StrongReferenceMessenger.Default.Register<ShowConfigExportMessage>(recipient, (recipient, message) => ShowConfigExport(message));
            StrongReferenceMessenger.Default.Register<PromptForConfigOutputStreamMessage>(recipient, (recipient, message) => PromptForConfigOutputStream(message));

            StrongReferenceMessenger.Default.Register<ShowDiagramCollectionSelectorMessage>(recipient, (recipient, message) => ShowDiagramCollectionSelector(message));
            
            StrongReferenceMessenger.Default.Register<PromptForLegacyImportMessage>(recipient, (recipient, message) => PromptForLegacyImport(message));
        }

        public static void UnregisterMessageHandlers(object recipient)
        {
            StrongReferenceMessenger.Default.Unregister<ChordiousMessage>(recipient);
            StrongReferenceMessenger.Default.Unregister<ExceptionMessage>(recipient);
            StrongReferenceMessenger.Default.Unregister<ConfirmationMessage>(recipient);
            StrongReferenceMessenger.Default.Unregister<PromptForTextMessage>(recipient);

            StrongReferenceMessenger.Default.Unregister<LaunchUrlMessage>(recipient);

            StrongReferenceMessenger.Default.Unregister<ShowChordFinderMessage>(recipient);
            StrongReferenceMessenger.Default.Unregister<ShowScaleFinderMessage>(recipient);

            StrongReferenceMessenger.Default.Unregister<ShowDiagramLibraryMessage>(recipient);

            StrongReferenceMessenger.Default.Unregister<ShowDiagramEditorMessage>(recipient);
            StrongReferenceMessenger.Default.Unregister<ShowDiagramMarkEditorMessage>(recipient);
            StrongReferenceMessenger.Default.Unregister<ShowDiagramFretLabelEditorMessage>(recipient);
            StrongReferenceMessenger.Default.Unregister<ShowDiagramBarreEditorMessage>(recipient);
            StrongReferenceMessenger.Default.Unregister<ShowDiagramStyleEditorMessage>(recipient);

            StrongReferenceMessenger.Default.Unregister<ShowInstrumentManagerMessage>(recipient);

            StrongReferenceMessenger.Default.Unregister<ShowInstrumentEditorMessage>(recipient);
            StrongReferenceMessenger.Default.Unregister<ShowTuningEditorMessage>(recipient);

            StrongReferenceMessenger.Default.Unregister<ShowChordQualityManagerMessage>(recipient);
            StrongReferenceMessenger.Default.Unregister<ShowChordQualityEditorMessage>(recipient);

            StrongReferenceMessenger.Default.Unregister<ShowScaleManagerMessage>(recipient);
            StrongReferenceMessenger.Default.Unregister<ShowScaleEditorMessage>(recipient);
            
            StrongReferenceMessenger.Default.Unregister<ShowOptionsMessage>(recipient);

            StrongReferenceMessenger.Default.Unregister<ShowLicensesMessage>(recipient);

            StrongReferenceMessenger.Default.Unregister<ShowAdvancedDataMessage>(recipient);

            StrongReferenceMessenger.Default.Unregister<ShowDiagramExportMessage>(recipient);

            StrongReferenceMessenger.Default.Unregister<ShowConfigImportMessage>(recipient);
            StrongReferenceMessenger.Default.Unregister<PromptForConfigInputStreamMessage>(recipient);

            StrongReferenceMessenger.Default.Unregister<ShowConfigExportMessage>(recipient);
            StrongReferenceMessenger.Default.Unregister<PromptForConfigOutputStreamMessage>(recipient);

            StrongReferenceMessenger.Default.Unregister<ShowDiagramCollectionSelectorMessage>(recipient);

            StrongReferenceMessenger.Default.Unregister<PromptForLegacyImportMessage>(recipient);
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
            Process.Start(new ProcessStartInfo(message.Url)
            {
                UseShellExecute = true
            });
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

            message.LicensesVM.Licenses.Add(IntegrationUtils.GetSvgNetLicense());
            message.LicensesVM.Licenses.Add(IntegrationUtils.GetExtendedWPFToolkitLicense());

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
            message.DiagramExportVM.RequestClose += () =>
            {
                window.Close();
            };
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
