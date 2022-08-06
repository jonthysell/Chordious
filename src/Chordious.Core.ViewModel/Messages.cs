// Copyright (c) Jon Thysell <http://jonthysell.com>
// Licensed under the MIT License.

using System;
using System.Collections.ObjectModel;
using System.IO;

using CommunityToolkit.Mvvm.Messaging;

using Chordious.Core.ViewModel.Resources;

namespace Chordious.Core.ViewModel
{
    public class ChordiousMessage
    {
        public InformationViewModel InformationVM { get; private set; }

        public ChordiousMessage(string message, string title, Action callback = null)
        {
            InformationVM = new InformationViewModel(message, title, callback);
        }

        public ChordiousMessage(string message, Action callback = null) : this(message, Strings.ChordiousMessageTitle, callback) { }

        public void Process()
        {
            InformationVM.ProcessClose();
        }
    }

    public class ConfirmationMessage
    {
        public ConfirmationViewModel ConfirmationVM { get; private set; }

        public ConfirmationMessage(string message, Action<bool> callback, string rememberAnswerKey = null)
        {
            ConfirmationVM = new ConfirmationViewModel(message, callback, rememberAnswerKey);
        }

        public void Process()
        {
            ConfirmationVM.ProcessClose();
        }
    }

    public class PromptForTextMessage
    {
        public TextPromptViewModel TextPromptVM { get; private set; }

        public PromptForTextMessage(string prompt, Action<string> callback, bool allowBlank = false, bool requireInteger = false)
        {
            TextPromptVM = new TextPromptViewModel(prompt, callback)
            {
                AllowBlank = allowBlank,
                RequireInteger = requireInteger,
            };
        }

        public PromptForTextMessage(string prompt, string defaultText, Action<string> callback, bool allowBlank = false, bool requireInteger = false) : this(prompt, callback, allowBlank, requireInteger)
        {
            TextPromptVM.Text = defaultText;
        }
    }

    public class ExceptionMessage
    {
        public ExceptionViewModel ExceptionVM { get; private set; }

        public ExceptionMessage(Exception exception)
        {
            ExceptionVM = new ExceptionViewModel(exception);
        }
    }

    public class LaunchUrlMessage
    {
        public string Url { get; private set; }

        public LaunchUrlMessage(string url)
        {
            Url = url;
        }
    }

    public abstract class SaveUserConfigAfterHandlingMessageBase
    {
        protected Action Callback;

        public SaveUserConfigAfterHandlingMessageBase(Action callback = null)
        {
            Callback = callback;
        }

        public virtual void Process()
        {
            try
            {
                AppViewModel.Instance.SaveUserConfig();
            }
            catch (Exception ex)
            {
                ExceptionUtils.HandleException(ex);
            }
            finally
            {
                Callback?.Invoke();
            }
        }
    }

    public class ShowChordFinderMessage : SaveUserConfigAfterHandlingMessageBase
    {
        public ChordFinderViewModel ChordFinderVM { get; private set; }

        public ShowChordFinderMessage(Action callback = null) : base(callback)
        {
            ChordFinderVM = new ChordFinderViewModel();
        }

        public override void Process()
        {
            bool callBaseProcessInFinally = true;
            try
            {
                if (!ChordFinderVM.Options.UsingDefaultTarget)
                {
                    string message = string.Format(Strings.FinderOptionsSaveTargetOnClosePromptMessageFormat, ChordFinderVM.Options.Instrument.Name, ChordFinderVM.Options.Tuning.LongName);
                    StrongReferenceMessenger.Default.Send(new ConfirmationMessage(message, (confirmed) =>
                    {
                        try
                        {
                            if (confirmed)
                            {
                                ChordFinderVM.Options.SaveTargetAsDefault();
                            }
                        }
                        catch (Exception ex)
                        {
                            ExceptionUtils.HandleException(ex);
                        }
                        finally
                        {
                            base.Process();
                        }
                    }));

                    callBaseProcessInFinally = false; // Base process is called in the confirmation callback
                }
            }
            catch (Exception ex)
            {
                ExceptionUtils.HandleException(ex);
            }
            finally
            {
                if (callBaseProcessInFinally)
                {
                    base.Process();
                }
            }
        }
    }

    public class ShowScaleFinderMessage : SaveUserConfigAfterHandlingMessageBase
    {
        public ScaleFinderViewModel ScaleFinderVM { get; private set; }

        public ShowScaleFinderMessage(Action callback = null) : base(callback)
        {
            ScaleFinderVM = new ScaleFinderViewModel();
        }

        public override void Process()
        {
            bool callBaseProcessInFinally = true;
            try
            {
                if (!ScaleFinderVM.Options.UsingDefaultTarget)
                {
                    string message = string.Format(Strings.FinderOptionsSaveTargetOnClosePromptMessageFormat, ScaleFinderVM.Options.Instrument.Name, ScaleFinderVM.Options.Tuning.LongName);
                    StrongReferenceMessenger.Default.Send(new ConfirmationMessage(message, (confirmed) =>
                    {
                        try
                        {
                            if (confirmed)
                            {
                                ScaleFinderVM.Options.SaveTargetAsDefault();
                            }
                        }
                        catch (Exception ex)
                        {
                            ExceptionUtils.HandleException(ex);
                        }
                        finally
                        {
                            base.Process();
                        }
                    }));

                    callBaseProcessInFinally = false; // Base process is called in the confirmation callback
                }
            }
            catch (Exception ex)
            {
                ExceptionUtils.HandleException(ex);
            }
            finally
            {
                if (callBaseProcessInFinally)
                {
                    base.Process();
                }
            }
        }
    }

    public class ShowDiagramLibraryMessage : SaveUserConfigAfterHandlingMessageBase
    {
        public DiagramLibraryViewModel DiagramLibraryVM { get; private set; }

        public ShowDiagramLibraryMessage(Action callback = null) : base(callback)
        {
            DiagramLibraryVM = new DiagramLibraryViewModel();
        }
    }

    public class ShowInstrumentManagerMessage : SaveUserConfigAfterHandlingMessageBase
    {
        public InstrumentManagerViewModel InstrumentManagerVM { get; private set; }

        public ShowInstrumentManagerMessage(Action callback = null) : base(callback)
        {
            InstrumentManagerVM = new InstrumentManagerViewModel();
        }
    }

    public class ShowInstrumentEditorMessage
    {
        public InstrumentEditorViewModel InstrumentEditorVM { get; private set; }

        public ShowInstrumentEditorMessage(Action<string, int> callback)
        {
            InstrumentEditorVM = new InstrumentEditorViewModel(callback)
            {
                Name = AppViewModel.Instance.UserConfig.Instruments.GetNewInstrumentName()
            };
        }

        public ShowInstrumentEditorMessage(string name, int numStrings, bool readOnly, Action<string, int> callback)
        {
            InstrumentEditorVM = new InstrumentEditorViewModel(name, numStrings, readOnly, callback);
        }
    }

    public class ShowTuningEditorMessage
    {
        public TuningEditorViewModel TuningEditorVM { get; private set; }

        private readonly Action<bool> Callback;

        public ShowTuningEditorMessage(ObservableInstrument instrument, Action<bool> callback = null)
        {
            TuningEditorVM = TuningEditorViewModel.AddNewTuning(instrument);
            TuningEditorVM.Name = (instrument.Instrument.Tunings as TuningSet)?.GetNewTuningName() ?? "";
            Callback = callback;
        }

        public ShowTuningEditorMessage(ObservableTuning tuning, Action<bool> callback = null)
        {
            TuningEditorVM = TuningEditorViewModel.EditExistingTuning(tuning);
            Callback = callback;
        }

        public ShowTuningEditorMessage(ObservableTuning tuning, ObservableInstrument targetInstrument, Action<bool> callback = null)
        {
            TuningEditorVM = TuningEditorViewModel.CopyExistingTuning(tuning, targetInstrument);
            TuningEditorVM.Name = (targetInstrument.Instrument.Tunings as TuningSet)?.GetNewTuningName(tuning.Name) ?? "";
            Callback = callback;
        }

        public void Process()
        {
            Callback?.Invoke(TuningEditorVM.Accepted);
        }
    }

    public class ShowChordQualityManagerMessage : SaveUserConfigAfterHandlingMessageBase
    {
        public ChordQualityManagerViewModel ChordQualityManagerVM { get; private set; }

        public ShowChordQualityManagerMessage(Action callback = null) : base(callback)
        {
            ChordQualityManagerVM = new ChordQualityManagerViewModel();
        }
    }

    public class ShowChordQualityEditorMessage
    {
        public ChordQualityEditorViewModel ChordQualityEditorVM { get; private set; }

        public ShowChordQualityEditorMessage(Action<string, string, int[]> callback)
        {
            ChordQualityEditorVM = new ChordQualityEditorViewModel(callback)
            {
                Name = AppViewModel.Instance.UserConfig.ChordQualities.GetNewChordQualityName()
            };
        }

        public ShowChordQualityEditorMessage(string name, string abbreviation, int[] intervals, bool readOnly, Action<string, string, int[]> callback)
        {
            ChordQualityEditorVM = new ChordQualityEditorViewModel(name, abbreviation, intervals, readOnly, callback);
        }
    }

    public class ShowScaleManagerMessage : SaveUserConfigAfterHandlingMessageBase
    {
        public ScaleManagerViewModel ScaleManagerVM { get; private set; }

        public ShowScaleManagerMessage(Action callback = null) : base(callback)
        {
            ScaleManagerVM = new ScaleManagerViewModel();
        }
    }

    public class ShowScaleEditorMessage
    {
        public ScaleEditorViewModel ScaleEditorVM { get; private set; }

        public ShowScaleEditorMessage(Action<string, int[]> callback)
        {
            ScaleEditorVM = new ScaleEditorViewModel(callback)
            {
                Name = AppViewModel.Instance.UserConfig.Scales.GetNewScaleName()
            };
        }

        public ShowScaleEditorMessage(string name, int[] intervals, bool readOnly, Action<string, int[]> callback)
        {
            ScaleEditorVM = new ScaleEditorViewModel(name, intervals, readOnly, callback);
        }
    }

    public class ShowDiagramEditorMessage
    {
        public DiagramEditorViewModel DiagramEditorVM { get; set; }

        public ObservableDiagram Diagram { get; private set; }

        public bool IsNew { get; private set; }

        private readonly Action<bool> Callback;

        public ShowDiagramEditorMessage(ObservableDiagram diagram, bool isNew, Action<bool> callback = null)
        {
            Diagram = diagram;
            IsNew = isNew;
            Callback = callback;
        }

        public void Process()
        {
            bool saveChanges = false;
            try
            {
                saveChanges = DiagramEditorVM.ProcessClose();
            }
            catch (Exception ex)
            {
                ExceptionUtils.HandleException(ex);
            }
            finally
            {
                Callback?.Invoke(saveChanges);
            }
        }
    }

    public class ShowDiagramMarkEditorMessage
    {
        public DiagramMarkEditorViewModel DiagramMarkEditorVM { get; private set; }

        private readonly Action<bool> Callback;

        public ShowDiagramMarkEditorMessage(DiagramMark diagramMark, bool isNew, Action<bool> callback = null)
        {
            DiagramMarkEditorVM = new DiagramMarkEditorViewModel(diagramMark, isNew);
            Callback = callback;
        }

        public void Process()
        {
            bool saveChanges = false;
            try
            {
                saveChanges = DiagramMarkEditorVM.ProcessClose();
            }
            catch (Exception ex)
            {
                ExceptionUtils.HandleException(ex);
            }
            finally
            {
                Callback?.Invoke(saveChanges);
            }
        }
    }

    public class ShowDiagramFretLabelEditorMessage
    {
        public DiagramFretLabelEditorViewModel DiagramFretLabelEditorVM { get; private set; }

        private readonly Action<bool> Callback;

        public ShowDiagramFretLabelEditorMessage(DiagramFretLabel diagramFretLabel, bool isNew, Action<bool> callback = null)
        {
            DiagramFretLabelEditorVM = new DiagramFretLabelEditorViewModel(diagramFretLabel, isNew);
            Callback = callback;
        }

        public void Process()
        {
            bool saveChanges = false;
            try
            {
                saveChanges = DiagramFretLabelEditorVM.ProcessClose();
            }
            catch (Exception ex)
            {
                ExceptionUtils.HandleException(ex);
            }
            finally
            {
                Callback?.Invoke(saveChanges);
            }
        }
    }

    public class ShowDiagramBarreEditorMessage
    {
        public DiagramBarreEditorViewModel DiagramBarreEditorVM { get; private set; }

        private readonly Action<bool> Callback;

        public ShowDiagramBarreEditorMessage(DiagramBarre diagrmBarre, bool isNew, Action<bool> callback = null)
        {
            DiagramBarreEditorVM = new DiagramBarreEditorViewModel(diagrmBarre, isNew);
            Callback = callback;
        }

        public void Process()
        {
            bool saveChanges = false;
            try
            {
                saveChanges = DiagramBarreEditorVM.ProcessClose();
            }
            catch (Exception ex)
            {
                ExceptionUtils.HandleException(ex);
            }
            finally
            {
                Callback?.Invoke(saveChanges);
            }
        }
    }

    public class ShowDiagramStyleEditorMessage
    {
        public DiagramStyleEditorViewModel DiagramStyleEditorVM { get; private set; }

        private readonly Action<bool> Callback;

        public ShowDiagramStyleEditorMessage(ObservableDiagramStyle diagramStyle, Action<bool> callback = null)
        {
            DiagramStyleEditorVM = new DiagramStyleEditorViewModel(diagramStyle);
            Callback = callback;
        }

        public void Process()
        {
            bool saveChanges = false;
            try
            {
                saveChanges = DiagramStyleEditorVM.ProcessClose();
            }
            catch (Exception ex)
            {
                ExceptionUtils.HandleException(ex);
            }
            finally
            {
                Callback?.Invoke(saveChanges);
            }
        }
    }

    public class ShowOptionsMessage
    {
        public OptionsViewModel OptionsVM { get; set; }

        private readonly Action<bool> Callback;

        public ShowOptionsMessage(Action<bool> callback = null)
        {
            Callback = callback;
        }

        public void Process()
        {
            bool itemsChanged = false;
            try
            {
                itemsChanged = OptionsVM.ProcessClose();
            }
            catch (Exception ex)
            {
                ExceptionUtils.HandleException(ex);
            }
            finally
            {
                Callback?.Invoke(itemsChanged);
            }
        }
    }

    public class ShowLicensesMessage
    {
        public LicensesViewModel LicensesVM { get; private set; }

        public ShowLicensesMessage(Action callback = null)
        {
            LicensesVM = new LicensesViewModel(callback);
        }

        public void Process()
        {
            LicensesVM.ProcessClose();
        }
    }

    public class ShowAdvancedDataMessage
    {
        public AdvancedDataViewModel AdvancedDataVM { get; private set; }

        private readonly Action<bool> Callback;

        public ShowAdvancedDataMessage(InheritableDictionary inheritableDictionary, string filter, Action<bool> callback)
        {
            AdvancedDataVM = new AdvancedDataViewModel(inheritableDictionary, filter);
            Callback = callback;
        }

        public void Process()
        {
            bool itemsChanged = false;
            try
            {
                itemsChanged = AdvancedDataVM.ProcessClose();
            }
            catch (Exception ex)
            {
                ExceptionUtils.HandleException(ex);
            }
            finally
            {
                Callback?.Invoke(itemsChanged);
            }
        }
    }

    public class ShowDiagramExportMessage : SaveUserConfigAfterHandlingMessageBase
    {
        public DiagramExportViewModelBase DiagramExportVM { get; set; }

        public ObservableCollection<ObservableDiagram> DiagramsToExport { get; private set; }
        public string CollectionName { get; private set; }

        public ShowDiagramExportMessage(ObservableCollection<ObservableDiagram> diagramsToExport, string collectionName, Action callback = null) : base(callback)
        {
            DiagramsToExport = diagramsToExport ?? throw new ArgumentNullException(nameof(diagramsToExport));
            CollectionName = collectionName;
        }

        public override void Process()
        {
            try
            {
                DiagramExportVM.ProcessClose();
            }
            catch (Exception ex)
            {
                ExceptionUtils.HandleException(ex);
            }
            finally
            {
                base.Process();
            }
        }
    }

    public class ShowConfigExportMessage : SaveUserConfigAfterHandlingMessageBase
    {
        public ConfigExportViewModel ConfigExportVM { get; private set; }

        public ShowConfigExportMessage(Action callback = null) : base(callback)
        {
            ConfigExportVM = new ConfigExportViewModel();
        }
    }

    public class ShowConfigImportMessage : SaveUserConfigAfterHandlingMessageBase
    {
        public ConfigImportViewModel ConfigImportVM { get; private set; }

        public ShowConfigImportMessage(Stream inputStream, Action callback = null) : base(callback)
        {
            ConfigImportVM = new ConfigImportViewModel(inputStream);
        }
    }

    public abstract class PromptForStreamMessage
    {
        private readonly Action<Stream> Callback;

        public PromptForStreamMessage(Action<Stream> callback)
        {
            Callback = callback ?? throw new ArgumentNullException(nameof(callback));
        }

        public void Process(Stream stream)
        {
            Callback?.Invoke(stream);
        }
    }

    public class PromptForConfigOutputStreamMessage : PromptForStreamMessage
    {
        public PromptForConfigOutputStreamMessage(Action<Stream> callback) : base(callback) { }
    }

    public class PromptForConfigInputStreamMessage : PromptForStreamMessage
    {
        public PromptForConfigInputStreamMessage(Action<Stream> callback) : base(callback) { }
    }

    public class ShowDiagramCollectionSelectorMessage
    {
        public DiagramCollectionSelectorViewModel DiagramCollectionSelectorVM { get; private set; }

        public ShowDiagramCollectionSelectorMessage(Action<string, bool> callback, string defaultCollectionName = null)
        {
            DiagramCollectionSelectorVM = new DiagramCollectionSelectorViewModel(callback, defaultCollectionName);
        }
    }

    public class PromptForLegacyImportMessage
    {
        private readonly Action<string, Stream> Callback;

        public PromptForLegacyImportMessage(Action<string, Stream> callback)
        {
            Callback = callback ?? throw new ArgumentNullException(nameof(callback));
        }

        public void Process(string fileName, Stream inputStream)
        {
            Callback?.Invoke(fileName, inputStream);
        }
    }
}
