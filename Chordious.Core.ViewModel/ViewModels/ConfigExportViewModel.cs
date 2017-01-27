// 
// ConfigExportViewModel.cs
//  
// Author:
//       Jon Thysell <thysell@gmail.com>
// 
// Copyright (c) 2015, 2016, 2017 Jon Thysell <http://jonthysell.com>
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
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;

using com.jonthysell.Chordious.Core.ViewModel.Resources;

namespace com.jonthysell.Chordious.Core.ViewModel
{
    public class ConfigExportViewModel : ConfigViewModelBase
    {
        public override string Title
        {
            get
            {
                return Strings.ConfigExportTitle;
            }
        }

        public override RelayCommand Accept
        {
            get
            {
                return new RelayCommand(() =>
                {
                    try
                    {
                        IsIdle = false;
                        if (IncludeLibrary && !IncludeStyles)
                        {
                            Messenger.Default.Send<ConfirmationMessage>(new ConfirmationMessage(Strings.ConfigExportLibraryWithoutStylesPromptMessage, (confirmed) =>
                            {
                                if (confirmed)
                                {
                                    PromptForExport();
                                }
                                IsIdle = true;
                            }));
                        }
                        else
                        {
                            PromptForExport();
                            IsIdle = true;
                        }
                    }
                    catch (Exception ex)
                    {
                        ExceptionUtils.HandleException(ex);
                    }
                });
            }
        }

        public ConfigExportViewModel() : base()
        {
        }

        private void PromptForExport()
        {
            ConfigParts configParts = GetConfigParts();
            Messenger.Default.Send<PromptForConfigOutputStreamMessage>(new PromptForConfigOutputStreamMessage((outputStream) =>
            {
                try
                {
                    if (null != outputStream)
                    {
                        using (outputStream)
                        {
                            AppVM.UserConfig.SaveFile(outputStream, configParts);
                        }

                        OnRequestClose();
                    }
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
            }));
        }
    }
}