// Copyright (c) Jon Thysell <http://jonthysell.com>
// Licensed under the MIT License.

using System;
using System.IO;

using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;

using Chordious.Core.ViewModel.Resources;

namespace Chordious.Core.ViewModel
{
    public class ConfigImportViewModel : ConfigViewModelBase
    {
        public override string Title
        {
            get
            {
                return Strings.ConfigImportTitle;
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
                        Messenger.Default.Send(new ConfirmationMessage(Strings.ConfigImportOverwritePromptMessage, (confirmed) =>
                        {
                            try
                            {
                                if (confirmed)
                                {
                                    TryImport();
                                    OnRequestClose();
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
                    catch (Exception ex)
                    {
                        ExceptionUtils.HandleException(ex);
                    }
                }));
            }
        }
        private RelayCommand _accept;

        private readonly Stream _inputStream;

        public ConfigImportViewModel(Stream inputStream) : base()
        {
            _inputStream = inputStream ?? throw new ArgumentNullException(nameof(inputStream));
        }

        private void TryImport()
        {
            try
            {
                ConfigParts configParts = GetConfigParts();

                ConfigFile importedConfigFile = new ConfigFile("Imported");
                using (_inputStream)
                {
                    importedConfigFile.LoadFile(_inputStream, configParts);
                }
                AppVM.UserConfig.ImportConfig(importedConfigFile, configParts);
            }
            catch (Exception ex)
            {
                ExceptionUtils.HandleException(ex);
            }
        }
    }
}
