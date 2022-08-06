// Copyright (c) Jon Thysell <http://jonthysell.com>
// Licensed under the MIT License.

using System;

using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

using Chordious.Core.ViewModel.Resources;

namespace Chordious.Core.ViewModel
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
                return _accept ?? (_accept = new RelayCommand(() =>
                {
                    try
                    {
                        IsIdle = false;
                        if (IncludeLibrary && !IncludeStyles)
                        {
                            StrongReferenceMessenger.Default.Send(new ConfirmationMessage(Strings.ConfigExportLibraryWithoutStylesPromptMessage, (confirmed) =>
                            {
                                try
                                {
                                    if (confirmed)
                                    {
                                        PromptForExport();
                                    }
                                }
                                catch (Exception ex)
                                {
                                    ExceptionUtils.HandleException(ex);
                                }
                                finally
                                {
                                    IsIdle = true;
                                }
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
                }));
            }
        }
        private RelayCommand _accept;

        public ConfigExportViewModel() : base()
        {
        }

        private void PromptForExport()
        {
            ConfigParts configParts = GetConfigParts();
            StrongReferenceMessenger.Default.Send(new PromptForConfigOutputStreamMessage((outputStream) =>
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