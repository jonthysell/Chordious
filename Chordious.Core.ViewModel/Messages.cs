// 
// Messages.cs
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
using System.IO;

using GalaSoft.MvvmLight.Messaging;

using com.jonthysell.Chordious.Core.ViewModel.Resources;

namespace com.jonthysell.Chordious.Core.ViewModel
{
    public class ChordiousMessage : MessageBase
    {
        public InformationViewModel InformationVM { get; private set; }

        public ChordiousMessage(string message, string title, Action callback = null) : base()
        {
            InformationVM = new InformationViewModel(message, title, callback);
        }

        public ChordiousMessage(string message, Action callback = null) : this(message, Strings.ChordiousMessageTitle, callback) { }

        public void Process()
        {
            InformationVM.ProcessClose();
        }
    }

    public class ConfirmationMessage : MessageBase
    {
        public ConfirmationViewModel ConfirmationVM { get; private set; }

        public ConfirmationMessage(string message, Action<bool> callback, string rememberAnswerKey = null) : base()
        {
            ConfirmationVM = new ConfirmationViewModel(message, callback, rememberAnswerKey);
        }

        public void Process()
        {
            ConfirmationVM.ProcessClose();
        }
    }

    public class PromptForTextMessage : MessageBase
    {
        public TextPromptViewModel TextPromptVM { get; private set; }

        public PromptForTextMessage(string prompt, Action<string> callback, bool allowBlank = false) : base()
        {
            TextPromptVM = new TextPromptViewModel(prompt, callback)
            {
                AllowBlank = allowBlank
            };
        }

        public PromptForTextMessage(string prompt, string defaultText, Action<string> callback, bool allowBlank = false) : this(prompt, callback, allowBlank)
        {
            TextPromptVM.Text = defaultText;
        }
    }

    public class ExceptionMessage : MessageBase
    {
        public ExceptionViewModel ExceptionVM { get; private set; }

        public ExceptionMessage(Exception exception) : base()
        {
            ExceptionVM = new ExceptionViewModel(exception);
        }
    }

    public class LaunchUrlMessage : MessageBase
    {
        public string Url { get; private set; }

        public LaunchUrlMessage(string url) : base()
        {
            Url = url;
        }
    }

    public abstract class SaveUserConfigAfterHandlingMessageBase : MessageBase
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
                    Messenger.Default.Send(new ConfirmationMessage(message, (confirmed) =>
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
                    Messenger.Default.Send(new ConfirmationMessage(message, (confirmed) =>
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

    public class ShowInstrumentEditorMessage : MessageBase
    {
        public InstrumentEditorViewModel InstrumentEditorVM { get; private set; }

        public ShowInstrumentEditorMessage(Action<string, int> callback) : base()
        {
            InstrumentEditorVM = new InstrumentEditorViewModel(callback);
        }

        public ShowInstrumentEditorMessage(string name, int numStrings, bool readOnly, Action<string, int> callback) : base()
        {
            InstrumentEditorVM = new InstrumentEditorViewModel(name, numStrings, readOnly, callback);
        }
    }

    public class ShowTuningEditorMessage : MessageBase
    {
        public TuningEditorViewModel TuningEditorVM { get; private set; }

        private readonly Action<bool> Callback;

        public ShowTuningEditorMessage(ObservableInstrument instrument, Action<bool> callback = null) : base()
        {
            TuningEditorVM = TuningEditorViewModel.AddNewTuning(instrument);
            Callback = callback;
        }

        public ShowTuningEditorMessage(ObservableTuning tuning, Action<bool> callback = null) : base()
        {
            TuningEditorVM = TuningEditorViewModel.EditExistingTuning(tuning);
            Callback = callback;
        }

        public ShowTuningEditorMessage(ObservableTuning tuning, ObservableInstrument targetInstrument, Action<bool> callback = null) : base()
        {
            TuningEditorVM = TuningEditorViewModel.CopyExistingTuning(tuning, targetInstrument);
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

    public class ShowChordQualityEditorMessage : MessageBase
    {
        public ChordQualityEditorViewModel ChordQualityEditorVM { get; private set; }

        public ShowChordQualityEditorMessage(Action<string, string, int[]> callback) : base()
        {
            ChordQualityEditorVM = new ChordQualityEditorViewModel(callback);
        }

        public ShowChordQualityEditorMessage(string name, string abbreviation, int[] intervals, bool readOnly, Action<string, string, int[]> callback) : base()
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

    public class ShowScaleEditorMessage : MessageBase
    {
        public ScaleEditorViewModel ScaleEditorVM { get; private set; }

        public ShowScaleEditorMessage(Action<string, int[]> callback) : base()
        {
            ScaleEditorVM = new ScaleEditorViewModel(callback);
        }

        public ShowScaleEditorMessage(string name, int[] intervals, bool readOnly, Action<string, int[]> callback) : base()
        {
            ScaleEditorVM = new ScaleEditorViewModel(name, intervals, readOnly, callback);
        }
    }

    public class ShowDiagramEditorMessage : MessageBase
    {
        public DiagramEditorViewModel DiagramEditorVM { get; set; }

        public ObservableDiagram Diagram { get; private set; }

        public bool IsNew { get; private set; }

        private readonly Action<bool> Callback;

        public ShowDiagramEditorMessage(ObservableDiagram diagram, bool isNew, Action<bool> callback = null) : base()
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

    public class ShowDiagramMarkEditorMessage : MessageBase
    {
        public DiagramMarkEditorViewModel DiagramMarkEditorVM { get; private set; }

        private readonly Action<bool> Callback;

        public ShowDiagramMarkEditorMessage(DiagramMark diagramMark, bool isNew, Action<bool> callback = null) : base()
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

    public class ShowDiagramFretLabelEditorMessage : MessageBase
    {
        public DiagramFretLabelEditorViewModel DiagramFretLabelEditorVM { get; private set; }

        private readonly Action<bool> Callback;

        public ShowDiagramFretLabelEditorMessage(DiagramFretLabel diagramFretLabel, bool isNew, Action<bool> callback = null) : base()
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

    public class ShowDiagramBarreEditorMessage : MessageBase
    {
        public DiagramBarreEditorViewModel DiagramBarreEditorVM { get; private set; }

        private readonly Action<bool> Callback;

        public ShowDiagramBarreEditorMessage(DiagramBarre diagrmBarre, bool isNew, Action<bool> callback = null) : base()
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

    public class ShowDiagramStyleEditorMessage : MessageBase
    {
        public DiagramStyleEditorViewModel DiagramStyleEditorVM { get; private set; }

        private readonly Action<bool> Callback;

        public ShowDiagramStyleEditorMessage(ObservableDiagramStyle diagramStyle, Action<bool> callback = null) : base()
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

    public class ShowOptionsMessage : MessageBase
    {
        public OptionsViewModel OptionsVM { get; set; }

        private readonly Action<bool> Callback;

        public ShowOptionsMessage(Action<bool> callback = null) : base()
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

    public class ShowAdvancedDataMessage : MessageBase
    {
        public AdvancedDataViewModel AdvancedDataVM { get; private set; }

        private readonly Action<bool> Callback;

        public ShowAdvancedDataMessage(InheritableDictionary inheritableDictionary, string filter, Action<bool> callback) : base()
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
            DiagramsToExport = diagramsToExport ?? throw new ArgumentNullException("diagramsToExport");
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

    public abstract class PromptForStreamMessage : MessageBase
    {
        private readonly Action<Stream> Callback;

        public PromptForStreamMessage(Action<Stream> callback) : base()
        {
            Callback = callback ?? throw new ArgumentNullException("callback");
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

    public class ShowDiagramCollectionSelectorMessage : MessageBase
    {
        public DiagramCollectionSelectorViewModel DiagramCollectionSelectorVM { get; private set; }

        public ShowDiagramCollectionSelectorMessage(Action<string, bool> callback, string defaultCollectionName = null) : base()
        {
            DiagramCollectionSelectorVM = new DiagramCollectionSelectorViewModel(callback, defaultCollectionName);
        }
    }

    public class PromptForLegacyImportMessage : MessageBase
    {
        private readonly Action<string, Stream> Callback;

        public PromptForLegacyImportMessage(Action<string, Stream> callback) : base()
        {
            Callback = callback ?? throw new ArgumentNullException("callback");
        }

        public void Process(string fileName, Stream inputStream)
        {
            Callback?.Invoke(fileName, inputStream);
        }
    }
}
