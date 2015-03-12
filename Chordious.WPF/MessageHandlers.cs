// 
// MessageHandlers.cs
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
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

using GalaSoft.MvvmLight.Messaging;

using com.jonthysell.Chordious.Core.ViewModel;

namespace com.jonthysell.Chordious.WPF
{
    public class MessageHandlers
    {
        public static void RegisterMessageHandlers(object recipient)
        {
            Messenger.Default.Register<ChordiousMessage>(recipient, (message) => MessageHandlers.ShowNotification(message));
            Messenger.Default.Register<ExceptionMessage>(recipient, (message) => MessageHandlers.ShowException(message));
            Messenger.Default.Register<ConfirmationMessage>(recipient, (message) => MessageHandlers.ConfirmAction(message));
            Messenger.Default.Register<PromptForTextMessage>(recipient, (message) => MessageHandlers.PromptForText(message));

            Messenger.Default.Register<LaunchUrlMessage>(recipient, (message) => MessageHandlers.LaunchUrl(message));

            Messenger.Default.Register<ShowChordFinderMessage>(recipient, (message) => MessageHandlers.ShowChordFinder(message));
            Messenger.Default.Register<ShowScaleFinderMessage>(recipient, (message) => MessageHandlers.ShowScaleFinder(message));
            Messenger.Default.Register<ShowDiagramLibraryMessage>(recipient, (message) => MessageHandlers.ShowDiagramLibrary(message));
            Messenger.Default.Register<ShowDiagramEditorMessage>(recipient, (message) => MessageHandlers.ShowDiagramEditor(message));
            Messenger.Default.Register<ShowOptionsMessage>(recipient, (message) => MessageHandlers.ShowOptions(message));
            Messenger.Default.Register<ShowAdvancedDataMessage>(recipient, (message) => MessageHandlers.ShowAdvancedData(message));
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
            Messenger.Default.Unregister<ShowOptionsMessage>(recipient);
            Messenger.Default.Unregister<ShowAdvancedDataMessage>(recipient);
        }

        private static void ShowNotification(ChordiousMessage message)
        {
            MessageBox.Show(message.Notification, message.Title, MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private static void ShowException(ExceptionMessage message)
        {
            MessageBox.Show(message.Exception.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private static void LaunchUrl(LaunchUrlMessage message)
        {
            Process.Start(message.Url, null);
        }

        private static void ConfirmAction(ConfirmationMessage message)
        {
            MessageBoxResult result = MessageBox.Show(message.Notification, "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            message.Execute(result == MessageBoxResult.Yes);
        }

        private static void PromptForText(PromptForTextMessage message)
        {
            TextPromptWindow window = new TextPromptWindow();
            window.DataContext = message.TextPromptVM;
            message.TextPromptVM.RequestClose += () =>
            {
                window.Close();
            };
            window.ShowDialog();
        }

        private static void ShowChordFinder(ShowChordFinderMessage message)
        {
            ChordFinderWindow window = new ChordFinderWindow();
            window.ShowDialog();
            message.Process();
        }

        private static void ShowScaleFinder(ShowScaleFinderMessage message)
        {
            ScaleFinderWindow window = new ScaleFinderWindow();
            window.ShowDialog();
            message.Process();
        }

        private static void ShowDiagramLibrary(ShowDiagramLibraryMessage message)
        {
            DiagramLibraryWindow window = new DiagramLibraryWindow();
            window.ShowDialog();
            message.Process();
        }

        private static void ShowDiagramEditor(ShowDiagramEditorMessage message)
        {
            DiagramEditorWindow window = new DiagramEditorWindow();
            window.DataContext = message.DiagramEditorVM;
            message.DiagramEditorVM.RequestClose += () =>
            {
                window.Close();
            };
            window.ShowDialog();
            message.Process();
        }

        private static void ShowOptions(ShowOptionsMessage message)
        {
            OptionsWindow window = new OptionsWindow();
            window.DataContext = message.OptionsVM;
            message.OptionsVM.RequestClose += () =>
            {
                window.Close();
            };
            window.ShowDialog();
            message.Process();
        }

        private static void ShowAdvancedData(ShowAdvancedDataMessage message)
        {
            AdvancedDataWindow window = new AdvancedDataWindow();
            window.DataContext = message.AdvancedDataVM;
            message.AdvancedDataVM.RequestClose += () => {
                window.Close();
            };
            window.ShowDialog();
            message.Process();
        }
    }
}
