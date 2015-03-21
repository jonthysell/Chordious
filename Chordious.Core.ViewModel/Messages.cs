// 
// Messages.cs
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

using GalaSoft.MvvmLight.Messaging;

namespace com.jonthysell.Chordious.Core.ViewModel
{
    public class ChordiousMessage : NotificationMessage
    {
        public string Title { get; private set; }

        public ChordiousMessage(string message) : base(message)
        {
            Title = "Chordious";
        }

        public ChordiousMessage(string message, string title) : this(message)
        {
            Title = title;
        }
    }

    public class ConfirmationMessage : NotificationMessageAction<bool>
    {
        public ConfirmationMessage(string notification, Action<bool> callback) : base(notification, callback) { }
    }

    public class PromptForTextMessage : MessageBase
    {
        public TextPromptViewModel TextPromptVM { get; private set; }

        public PromptForTextMessage(string prompt, Action<string> callback) : base()
        {
            TextPromptVM = new TextPromptViewModel(prompt, callback);
        }

        public PromptForTextMessage(string prompt, string defaultText, Action<string> callback) : this(prompt, callback)            
        {
            TextPromptVM.Text = defaultText;
        }
    }

    public class ExceptionMessage : MessageBase
    {
        public Exception Exception { get; private set; }

        public ExceptionMessage(Exception exception) : base()
        {
            Exception = exception;
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
        public void Process()
        {
            AppViewModel.Instance.SaveUserConfig();
        }
    }

    public class ShowChordFinderMessage : SaveUserConfigAfterHandlingMessageBase
    {
        public ShowChordFinderMessage() : base() { }
    }

    public class ShowScaleFinderMessage : SaveUserConfigAfterHandlingMessageBase
    {
        public ShowScaleFinderMessage() : base() { }
    }

    public class ShowDiagramLibraryMessage : SaveUserConfigAfterHandlingMessageBase
    {
        public ShowDiagramLibraryMessage() : base() { }
    }

    public class ShowDiagramEditorMessage : MessageBase
    {
        public DiagramEditorViewModel DiagramEditorVM { get; private set; }

        private Action<bool> Callback;

        public ShowDiagramEditorMessage(ObservableDiagram diagram, Action<bool> callback = null) : base()
        {
            DiagramEditorVM = new DiagramEditorViewModel(diagram);
            Callback = callback;
        }

        public void Process()
        {
            bool saveChanges = DiagramEditorVM.ProcessClose();
            if (null != Callback)
            {
                Callback(saveChanges);
            }
        }
    }

    public class ShowOptionsMessage : MessageBase
    {
        public OptionsViewModel OptionsVM { get; set; }

        private Action<bool> Callback;

        public ShowOptionsMessage(Action<bool> callback = null) : base()
        {
            Callback = callback;
        }

        public void Process()
        {
            bool itemsChanged = OptionsVM.ProcessClose();
            if (null != Callback)
            {
                Callback(itemsChanged);
            }
        }
    }

    public class ShowAdvancedDataMessage : MessageBase
    {
        public AdvancedDataViewModel AdvancedDataVM { get; private set; }

        private Action<bool> Callback;

        public ShowAdvancedDataMessage(InheritableDictionary inheritableDictionary, string filter, Action<bool> callback) : base()
        {
            AdvancedDataVM = new AdvancedDataViewModel(inheritableDictionary, filter);
            Callback = callback;
        }

        public void Process()
        {
            bool itemsChanged = AdvancedDataVM.ProcessClose();
            if (null != Callback)
            {
                Callback(itemsChanged);
            }
        }
    }

    public class PromptForExportMessage : MessageBase
    {
        private Action<IDiagramExporter> Callback;

        public int Count { get; private set; }

        public PromptForExportMessage(int count, Action<IDiagramExporter> callback = null) : base()
        {
            if (count < 1)
            {
                throw new ArgumentOutOfRangeException("count");
            }

            Count = count;
            Callback = callback;
        }

        public void Process(IDiagramExporter diagramExporter)
        {
            if (null != Callback)
            {
                Callback(diagramExporter);
            }
        }
    }
}
