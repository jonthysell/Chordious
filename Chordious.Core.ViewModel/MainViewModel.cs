// 
// MainViewModel.cs
//  
// Author:
//       Jon Thysell <thysell@gmail.com>
// 
// Copyright (c) 2015, 2016 Jon Thysell <http://jonthysell.com>
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

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;

using com.jonthysell.Chordious.Core;

using com.jonthysell.Chordious.Core.ViewModel.Resources;

namespace com.jonthysell.Chordious.Core.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        public AppViewModel AppVM
        {
            get
            {
                return AppViewModel.Instance;
            }
        }

        public string Title
        {
            get
            {
                return AppInfo.Product;
            }
        }

        public string FullProgramName
        {
            get
            {
                return AppInfo.Product + " " + AppInfo.FullVersion;
            }
        }

        public string Description
        {
            get
            {
                string[] descLines = { AppInfo.Comments,
                                   AppInfo.Copyright
                                   };

                return String.Join(Environment.NewLine, descLines);
            }
        }

        #region LaunchWebsite

        public string LaunchWebsiteLabel
        {
            get
            {
                return Strings.LaunchWebsiteLabel;
            }
        }

        public string LaunchWebsiteToolTip
        {
            get
            {
                return Strings.LaunchWebsiteToolTip;
            }
        }

        public RelayCommand LaunchWebsite
        {
            get
            {
                return new RelayCommand(() => {
                    try
                    {
                        Messenger.Default.Send<ConfirmationMessage>(new ConfirmationMessage(Strings.LaunchWebsitePromptMessage, (confirmed) =>
                        {
                            if (confirmed)
                            {
                                Messenger.Default.Send<LaunchUrlMessage>(new LaunchUrlMessage(AppInfo.Website));
                            }
                        }, "confirmation.main.launchwebsite"));
                    }
                    catch (Exception ex)
                    {
                        ExceptionUtils.HandleException(ex);
                    }
                });
            }
        }

        #endregion

        #region ShowLicense

        public string ShowLicenseLabel
        {
            get
            {
                return Strings.ShowLicenseLabel;
            }
        }

        public string ShowLicenseToolTip
        {
            get
            {
                return Strings.ShowLicenseToolTip;
            }
        }

        public RelayCommand ShowLicense
        {
            get
            {
                return new RelayCommand(() =>
                {
                    try
                    {
                        string text = String.Join(Environment.NewLine, AppInfo.Product + " " + AppInfo.Copyright, "", AppInfo.License);
                        Messenger.Default.Send<ChordiousMessage>(new ChordiousMessage(text, Strings.LicenseTitle));
                    }
                    catch (Exception ex)
                    {
                        ExceptionUtils.HandleException(ex);
                    }
                });
            }
        }

        #endregion

        #region ShowChordFinder

        public string ShowChordFinderLabel
        {
            get
            {
                return Strings.ShowChordFinderLabel;
            }
        }

        public string ShowChordFinderToolTip
        {
            get
            {
                return Strings.ShowChordFinderToolTip;
            }
        }

        public RelayCommand ShowChordFinder
        {
            get
            {
                return new RelayCommand(() =>
                {
                    try
                    {
                        Messenger.Default.Send<ShowChordFinderMessage>(new ShowChordFinderMessage());
                    }
                    catch (Exception ex)
                    {
                        ExceptionUtils.HandleException(ex);
                    }
                });
            }
        }

        #endregion

        #region ShowScaleFinder

        public string ShowScaleFinderLabel
        {
            get
            {
                return Strings.ShowScaleFinderLabel;
            }
        }

        public string ShowScaleFinderToolTip
        {
            get
            {
                return Strings.ShowScaleFinderToolTip;
            }
        }

        public RelayCommand ShowScaleFinder
        {
            get
            {
                return new RelayCommand(() =>
                {
                    try
                    {
                        Messenger.Default.Send<ShowScaleFinderMessage>(new ShowScaleFinderMessage());
                    }
                    catch (Exception ex)
                    {
                        ExceptionUtils.HandleException(ex);
                    }
                });
            }
        }

        #endregion

        #region Show Diagram Library

        public string ShowDiagramLibraryLabel
        {
            get
            {
                return Strings.ShowDiagramLibraryLabel;
            }
        }

        public string ShowDiagramLibraryToolTip
        {
            get
            {
                return Strings.ShowDiagramLibraryToolTip;
            }
        }

        public RelayCommand ShowDiagramLibrary
        {
            get
            {
                return new RelayCommand(() =>
                {
                    try
                    {
                        Messenger.Default.Send<ShowDiagramLibraryMessage>(new ShowDiagramLibraryMessage());
                    }
                    catch (Exception ex)
                    {
                        ExceptionUtils.HandleException(ex);
                    }
                });
            }
        }

        #endregion

        #region Show Instrument Manager

        public string ShowInstrumentManagerLabel
        {
            get
            {
                return Strings.ShowInstrumentManagerLabel;
            }
        }

        public string ShowInstrumentManagerToolTip
        {
            get
            {
                return Strings.ShowInstrumentManagerToolTip;
            }
        }

        public RelayCommand ShowInstrumentManager
        {
            get
            {
                return new RelayCommand(() =>
                {
                    try
                    {
                        Messenger.Default.Send<ShowInstrumentManagerMessage>(new ShowInstrumentManagerMessage());
                    }
                    catch (Exception ex)
                    {
                        ExceptionUtils.HandleException(ex);
                    }
                });
            }
        }

        #endregion

        #region Show Options

        public string ShowOptionsLabel
        {
            get
            {
                return Strings.ShowOptionsLabel;
            }
        }

        public string ShowOptionsToolTip
        {
            get
            {
                return Strings.ShowOptionsToolTip;
            }
        }

        public RelayCommand ShowOptions
        {
            get
            {
                return new RelayCommand(() =>
                {
                    try
                    {
                        Messenger.Default.Send<ShowOptionsMessage>(new ShowOptionsMessage((itemsChanged) =>
                        {
                            if (itemsChanged)
                            {
                                AppVM.SaveUserConfig();
                            }
                        }));
                    }
                    catch (Exception ex)
                    {
                        ExceptionUtils.HandleException(ex);
                    }
                });
            }
        }

        #endregion

        #region Show Help

        public string ShowHelpLabel
        {
            get
            {
                return Strings.ShowHelpLabel;
            }
        }

        public string ShowHelpToolTip
        {
            get
            {
                return Strings.ShowHelpToolTip;
            }
        }

        public RelayCommand ShowHelp
        {
            get
            {
                return new RelayCommand(() =>
                {
                    try
                    {
                        Messenger.Default.Send<ConfirmationMessage>(new ConfirmationMessage(Strings.ShowHelpPromptMessage, (confirmed) =>
                        {
                            if (confirmed)
                            {
                                Messenger.Default.Send<LaunchUrlMessage>(new LaunchUrlMessage("http://chordious.com/help/"));
                            }
                        }, "confirmation.main.showhelp"));
                    }
                    catch (Exception ex)
                    {
                        ExceptionUtils.HandleException(ex);
                    }
                });
            }
        }

        #endregion

        public MainViewModel()
        {
        }

    }
}
