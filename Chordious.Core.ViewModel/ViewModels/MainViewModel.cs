// Copyright (c) Jon Thysell <http://jonthysell.com>
// Licensed under the MIT License.

using System;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;

using Chordious.Core.ViewModel.Resources;

namespace Chordious.Core.ViewModel
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
                return AppInfo.Product + " " + AppInfo.Version;
            }
        }

        public string Description
        {
            get
            {
                string[] descLines = { AppInfo.Comments,
                                   AppInfo.Copyright
                                   };

                return string.Join(Environment.NewLine, descLines);
            }
        }

        #region LaunchWebsite

        public string LaunchWebsiteLabel
        {
            get
            {
                return Strings.MainLaunchWebsiteLabel;
            }
        }

        public string LaunchWebsiteToolTip
        {
            get
            {
                return Strings.MainLaunchWebsiteToolTip;
            }
        }

        public RelayCommand LaunchWebsite
        {
            get
            {
                return _launchWebsite ?? (_launchWebsite = new RelayCommand(() =>
                {
                    try
                    {
                        Messenger.Default.Send(new ConfirmationMessage(Strings.MainLaunchWebsitePromptMessage, (confirmed) =>
                        {
                            try
                            {
                                if (confirmed)
                                {
                                    Messenger.Default.Send(new LaunchUrlMessage(AppInfo.Website));
                                }
                            }
                            catch (Exception ex)
                            {
                                ExceptionUtils.HandleException(ex);
                            }
                        }, "confirmation.main.launchwebsite"));
                    }
                    catch (Exception ex)
                    {
                        ExceptionUtils.HandleException(ex);
                    }
                }));
            }
        }
        private RelayCommand _launchWebsite;

        #endregion

        #region ShowLicenses

        public string ShowLicensesLabel
        {
            get
            {
                return Strings.MainShowLicensesLabel;
            }
        }

        public string ShowLicensesToolTip
        {
            get
            {
                return Strings.MainShowLicensesToolTip;
            }
        }

        public RelayCommand ShowLicenses
        {
            get
            {
                return _showLicenses ?? (_showLicenses = new RelayCommand(() =>
                {
                    try
                    {
                        Messenger.Default.Send(new ShowLicensesMessage());
                    }
                    catch (Exception ex)
                    {
                        ExceptionUtils.HandleException(ex);
                    }
                }));
            }
        }
        private RelayCommand _showLicenses;

        #endregion

        #region ShowChordFinder

        public string ShowChordFinderLabel
        {
            get
            {
                return Strings.MainShowChordFinderLabel;
            }
        }

        public string ShowChordFinderToolTip
        {
            get
            {
                return Strings.MainShowChordFinderToolTip;
            }
        }

        public RelayCommand ShowChordFinder
        {
            get
            {
                return _showChordFinder ?? (_showChordFinder = new RelayCommand(() =>
                {
                    try
                    {
                        Messenger.Default.Send(new ShowChordFinderMessage());
                    }
                    catch (Exception ex)
                    {
                        ExceptionUtils.HandleException(ex);
                    }
                }));
            }
        }
        private RelayCommand _showChordFinder;

        #endregion

        #region ShowScaleFinder

        public string ShowScaleFinderLabel
        {
            get
            {
                return Strings.MainShowScaleFinderLabel;
            }
        }

        public string ShowScaleFinderToolTip
        {
            get
            {
                return Strings.MainShowScaleFinderToolTip;
            }
        }

        public RelayCommand ShowScaleFinder
        {
            get
            {
                return _showScaleFinder ?? (_showScaleFinder = new RelayCommand(() =>
                {
                    try
                    {
                        Messenger.Default.Send(new ShowScaleFinderMessage());
                    }
                    catch (Exception ex)
                    {
                        ExceptionUtils.HandleException(ex);
                    }
                }));
            }
        }
        private RelayCommand _showScaleFinder;

        #endregion

        #region Show Diagram Library

        public string ShowDiagramLibraryLabel
        {
            get
            {
                return Strings.MainShowDiagramLibraryLabel;
            }
        }

        public string ShowDiagramLibraryToolTip
        {
            get
            {
                return Strings.MainShowDiagramLibraryToolTip;
            }
        }

        public RelayCommand ShowDiagramLibrary
        {
            get
            {
                return _showDiagramLibrary ?? (_showDiagramLibrary = new RelayCommand(() =>
                {
                    try
                    {
                        Messenger.Default.Send(new ShowDiagramLibraryMessage());
                    }
                    catch (Exception ex)
                    {
                        ExceptionUtils.HandleException(ex);
                    }
                }));
            }
        }
        private RelayCommand _showDiagramLibrary;

        #endregion

        #region Show Instrument Manager

        public string ShowInstrumentManagerLabel
        {
            get
            {
                return Strings.MainShowInstrumentManagerLabel;
            }
        }

        public string ShowInstrumentManagerToolTip
        {
            get
            {
                return Strings.MainShowInstrumentManagerToolTip;
            }
        }

        public RelayCommand ShowInstrumentManager
        {
            get
            {
                return _showInstrumentManager ?? (_showInstrumentManager = new RelayCommand(() =>
                {
                    try
                    {
                        Messenger.Default.Send(new ShowInstrumentManagerMessage());
                    }
                    catch (Exception ex)
                    {
                        ExceptionUtils.HandleException(ex);
                    }
                }));
            }
        }
        private RelayCommand _showInstrumentManager;

        #endregion

        #region Show Options

        public string ShowOptionsLabel
        {
            get
            {
                return Strings.MainShowOptionsLabel;
            }
        }

        public string ShowOptionsToolTip
        {
            get
            {
                return Strings.MainShowOptionsToolTip;
            }
        }

        public RelayCommand ShowOptions
        {
            get
            {
                return _showOptions ?? (_showOptions = new RelayCommand(() =>
                {
                    try
                    {
                        Messenger.Default.Send(new ShowOptionsMessage((itemsChanged) =>
                        {
                            try
                            {
                                if (itemsChanged)
                                {
                                    AppVM.SaveUserConfig();
                                }
                            }
                            catch (Exception ex)
                            {
                                ExceptionUtils.HandleException(ex);
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
        private RelayCommand _showOptions;

        #endregion

        #region Show Help

        public string ShowHelpLabel
        {
            get
            {
                return Strings.MainShowHelpLabel;
            }
        }

        public string ShowHelpToolTip
        {
            get
            {
                return Strings.MainShowHelpToolTip;
            }
        }

        public RelayCommand ShowHelp
        {
            get
            {
                return _showHelp ?? (_showHelp = new RelayCommand(() =>
                {
                    try
                    {
                        Messenger.Default.Send(new ConfirmationMessage(Strings.MainShowHelpPromptMessage, (confirmed) =>
                        {
                            try
                            {
                                if (confirmed)
                                {
                                    Messenger.Default.Send(new LaunchUrlMessage("http://chordious.com/help/"));
                                }
                            }
                            catch (Exception ex)
                            {
                                ExceptionUtils.HandleException(ex);
                            }
                        }, "confirmation.main.showhelp"));
                    }
                    catch (Exception ex)
                    {
                        ExceptionUtils.HandleException(ex);
                    }
                }));
            }
        }
        private RelayCommand _showHelp;

        #endregion

        public MainViewModel()
        {
        }

    }
}
