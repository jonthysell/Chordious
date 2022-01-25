// Copyright (c) Jon Thysell <http://jonthysell.com>
// Licensed under the MIT License.

using System;
using System.Collections.ObjectModel;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;

using Chordious.Core.ViewModel.Resources;

namespace Chordious.Core.ViewModel
{
    public class InstrumentManagerViewModel : ViewModelBase
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
                return Strings.InstrumentManagerTitle;
            }
        }

        public bool InstrumentIsSelected
        {
            get
            {
                return (null != SelectedInstrument);
            }
        }

        public ObservableInstrument SelectedInstrument
        {
            get
            {
                return _instrument;
            }
            set
            {
                try
                {
                    _instrument = value;
                    SelectedTuning = null;

                    if (null == SelectedInstrument)
                    {
                        Tunings = null;
                    }
                    else
                    {
                        Tunings = SelectedInstrument.GetTunings();
                        if (Tunings.Count > 0)
                        {
                            SelectedTuning = Tunings[0];
                        }
                    }
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
                finally
                {
                    RaisePropertyChanged(nameof(SelectedInstrument));
                    RaisePropertyChanged(nameof(InstrumentIsSelected));
                    EditInstrument.RaiseCanExecuteChanged();
                    RaisePropertyChanged(nameof(EditInstrumentLabel));
                    DeleteInstrument.RaiseCanExecuteChanged();
                    RaisePropertyChanged(nameof(DeleteInstrumentLabel));
                    AddTuning.RaiseCanExecuteChanged();
                }
            }
        }
        private ObservableInstrument _instrument;

        public string DefaultInstrumentsGroupLabel
        {
            get
            {
                return Strings.InstrumentManagerDefaultInstrumentsGroupLabel;
            }
        }

        public string DefaultInstrumentsGroupToolTip
        {
            get
            {
                return Strings.InstrumentManagerDefaultInstrumentsGroupToolTip;
            }
        }

        public int SelectedDefaultInstrumentIndex
        {
            get
            {
                return _selectedDefaultInstrumentIndex;
            }
            set
            {
                if (value < 0 || value >= DefaultInstruments.Count)
                {
                    _selectedDefaultInstrumentIndex = -1;
                }
                else
                {
                    _selectedDefaultInstrumentIndex = value;
                    SelectedInstrument = DefaultInstruments[_selectedDefaultInstrumentIndex];
                    SelectedUserInstrumentIndex = -1; // Unselect user instrument
                }
                RaisePropertyChanged(nameof(SelectedDefaultInstrumentIndex));
            }
        }
        private int _selectedDefaultInstrumentIndex = -1;

        public ObservableCollection<ObservableInstrument> DefaultInstruments
        {
            get
            {
                return _defaultInstruments;
            }
            private set
            {
                _defaultInstruments = value;
                RaisePropertyChanged(nameof(DefaultInstruments));
            }
        }
        private ObservableCollection<ObservableInstrument> _defaultInstruments;

        public string UserInstrumentsGroupLabel
        {
            get
            {
                return Strings.InstrumentManagerUserInstrumentsGroupLabel;
            }
        }

        public string UserInstrumentsGroupToolTip
        {
            get
            {
                return Strings.InstrumentManagerUserInstrumentsGroupToolTip;
            }
        }

        public int SelectedUserInstrumentIndex
        {
            get
            {
                return _selectedUserInstrumentIndex;
            }
            set
            {
                if (value < 0 || value >= UserInstruments.Count)
                {
                    _selectedUserInstrumentIndex = -1;
                }
                else
                {
                    _selectedUserInstrumentIndex = value;
                    SelectedInstrument = UserInstruments[_selectedUserInstrumentIndex];
                    SelectedDefaultInstrumentIndex = -1; // Unselect default instrument
                }
                RaisePropertyChanged(nameof(SelectedUserInstrumentIndex));
            }
        }
        private int _selectedUserInstrumentIndex = -1;

        public ObservableCollection<ObservableInstrument> UserInstruments
        {
            get
            {
                return _userInstruments;
            }
            private set
            {
                _userInstruments = value;
                RaisePropertyChanged(nameof(UserInstruments));
            }
        }
        private ObservableCollection<ObservableInstrument> _userInstruments;

        public string TuningsGroupLabel
        {
            get
            {
                return Strings.InstrumentManagerTuningsGroupLabel;
            }
        }

        public string TuningsGroupToolTip
        {
            get
            {
                return Strings.InstrumentManagerTuningsGroupToolTip;
            }
        }

        public bool TuningIsSelected
        {
            get
            {
                return (null != SelectedTuning);
            }
        }

        public ObservableTuning SelectedTuning
        {
            get
            {
                return _tuning;
            }
            set
            {
                try
                {
                    _tuning = value;
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
                finally
                {
                    RaisePropertyChanged(nameof(SelectedTuning));
                    RaisePropertyChanged(nameof(TuningIsSelected));
                    EditTuning.RaiseCanExecuteChanged();
                    RaisePropertyChanged(nameof(EditTuningLabel));
                    DeleteTuning.RaiseCanExecuteChanged();
                    RaisePropertyChanged(nameof(DeleteTuningLabel));
                }
            }
        }
        private ObservableTuning _tuning;

        public ObservableCollection<ObservableTuning> Tunings
        {
            get
            {
                return _tunings;
            }
            private set
            {
                _tunings = value;
                RaisePropertyChanged(nameof(Tunings));
            }
        }
        private ObservableCollection<ObservableTuning> _tunings;

        #region AddInstrument

        public string AddInstrumentLabel
        {
            get
            {
                return Strings.NewLabel;
            }
        }

        public string AddInstrumentToolTip
        {
            get
            {
                return Strings.InstrumentManagerAddInstrumentToolTip;
            }
        }

        public RelayCommand AddInstrument
        {
            get
            {
                return _addInstrument ?? (_addInstrument= new RelayCommand(() =>
                {
                    try
                    {
                        Messenger.Default.Send(new ShowInstrumentEditorMessage((name, numStrings) =>
                        {
                            try
                            {
                                Instrument addedInstrument = AppVM.UserConfig.Instruments.Add(name, numStrings);
                                Refresh(addedInstrument);
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
        private RelayCommand _addInstrument;

        #endregion

        #region EditInstrument

        public string EditInstrumentLabel
        {
            get
            {
                if (InstrumentIsSelected)
                {
                    return string.Format(Strings.InstrumentManagerEditInstrumentLabelFormat, SelectedInstrument.Name);
                }

                return Strings.EditLabel;
            }
        }

        public string EditInstrumentToolTip
        {
            get
            {
                return Strings.InstrumentManagerEditInstrumentToolTip;
            }
        }

        public RelayCommand EditInstrument
        {
            get
            {
                return _editInstrument ?? (_editInstrument = new RelayCommand(() =>
                {
                    try
                    {
                        Messenger.Default.Send(new ShowInstrumentEditorMessage(SelectedInstrument.Name, SelectedInstrument.NumStrings, SelectedInstrument.ReadOnly, (name, numStrings) =>
                        {
                            try
                            {
                                SelectedInstrument.Instrument.Name = name;
                                Refresh(SelectedInstrument.Instrument);
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
                }, () =>
                {
                    return InstrumentIsSelected;
                }));
            }
        }
        private RelayCommand _editInstrument;

        #endregion

        #region DeleteInstrument

        public string DeleteInstrumentLabel
        {
            get
            {
                if (InstrumentIsSelected)
                {
                    return string.Format(Strings.InstrumentManagerDeleteInstrumentLabelFormat, SelectedInstrument.Name);
                }

                return Strings.DeleteLabel;
            }
        }

        public string DeleteInstrumentToolTip
        {
            get
            {
                return Strings.InstrumentManagerDeleteInstrumentToolTip;
            }
        }

        public RelayCommand DeleteInstrument
        {
            get
            {
                return _deleteInstrument ?? (_deleteInstrument = new RelayCommand(() =>
                {
                    try
                    {
                        Messenger.Default.Send(new ConfirmationMessage(string.Format(Strings.InstrumentManagerDeleteInstrumentPromptFormat, SelectedInstrument.Name), (confirm) =>
                        {
                            try
                            {
                                if (confirm)
                                {
                                    AppVM.UserConfig.Instruments.Remove(SelectedInstrument.Name);
                                    Refresh();
                                }
                            }
                            catch (Exception ex)
                            {
                                ExceptionUtils.HandleException(ex);
                            }
                        }, "confirmation.instrumentmanager.deleteinstrument"));
                    }
                    catch (Exception ex)
                    {
                        ExceptionUtils.HandleException(ex);
                    }
                }, () =>
                {
                    return InstrumentIsSelected && !SelectedInstrument.ReadOnly;
                }));
            }
        }
        private RelayCommand _deleteInstrument;

        #endregion

        #region AddTuning

        public string AddTuningLabel
        {
            get
            {
                return Strings.NewLabel;
            }
        }

        public string AddTuningToolTip
        {
            get
            {
                return Strings.InstrumentManagerAddTuningToolTip;
            }
        }

        public RelayCommand AddTuning
        {
            get
            {
                return _addTuning ?? (_addTuning = new RelayCommand(() =>
                {
                    try
                    {
                        Messenger.Default.Send(new ShowTuningEditorMessage(SelectedInstrument, (accepted) =>
                        {
                            try
                            {
                                if (accepted)
                                {
                                    Refresh(SelectedInstrument.Instrument);
                                }
                            }
                            catch(Exception ex)
                            {
                                ExceptionUtils.HandleException(ex);
                            }
                        }));
                    }
                    catch (Exception ex)
                    {
                        ExceptionUtils.HandleException(ex);
                    }
                }, () =>
                {
                    return InstrumentIsSelected && !SelectedInstrument.ReadOnly;
                }));
            }
        }
        private RelayCommand _addTuning;

        #endregion

        #region EditTuning

        public string EditTuningLabel
        {
            get
            {
                if (TuningIsSelected)
                {
                    return string.Format(Strings.InstrumentManagerEditTuningLabelFormat, SelectedTuning.Name);
                }

                return Strings.EditLabel;
            }
        }

        public string EditTuningToolTip
        {
            get
            {
                return Strings.InstrumentManagerEditTuningToolTip;
            }
        }

        public RelayCommand EditTuning
        {
            get
            {
                return _editTuning ?? (_editTuning = new RelayCommand(() =>
                {
                    try
                    {
                        Messenger.Default.Send(new ShowTuningEditorMessage(SelectedTuning, (accepted) =>
                        {
                            try
                            {
                                if (accepted)
                                {
                                    Refresh(SelectedInstrument.Instrument, SelectedTuning.Tuning);
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
                }, () =>
                {
                    return InstrumentIsSelected && TuningIsSelected;
                }));
            }
        }
        private RelayCommand _editTuning;

        #endregion

        #region DeleteTuning

        public string DeleteTuningLabel
        {
            get
            {
                if (TuningIsSelected)
                {
                    return string.Format(Strings.InstrumentManagerDeleteTuningLabelFormat, SelectedTuning.Name);
                }

                return Strings.DeleteLabel;
            }
        }

        public string DeleteTuningToolTip
        {
            get
            {
                return Strings.InstrumentManagerDeleteTuningToolTip;
            }
        }

        public RelayCommand DeleteTuning
        {
            get
            {
                return _deleteTuning ?? (_deleteTuning = new RelayCommand(() =>
                {
                    try
                    {
                        Messenger.Default.Send(new ConfirmationMessage(string.Format(Strings.InstrumentManagerDeleteTuningPromptFormat, SelectedTuning.Name), (confirm) =>
                        {
                            try
                            {
                                if (confirm)
                                {
                                    SelectedInstrument.Instrument.Tunings.Remove(SelectedTuning.Tuning);
                                    Refresh(SelectedInstrument.Instrument);
                                }
                            }
                            catch (Exception ex)
                            {
                                ExceptionUtils.HandleException(ex);
                            }
                        }, "confirmation.instrumentmanager.deletetuning"));
                    }
                    catch (Exception ex)
                    {
                        ExceptionUtils.HandleException(ex);
                    }
                }, () =>
                {
                    return InstrumentIsSelected && !SelectedInstrument.ReadOnly && TuningIsSelected && !SelectedTuning.ReadOnly;
                }));
            }
        }
        private RelayCommand _deleteTuning;

        #endregion

        #region CopyTuning

        public string CopyTuningLabel
        {
            get
            {
                if (TuningIsSelected)
                {
                    return string.Format(Strings.InstrumentManagerCopyTuningLabelFormat, SelectedTuning.Name);
                }

                return Strings.CopyLabel;
            }
        }

        public string CopyTuningToolTip
        {
            get
            {
                return Strings.InstrumentManagerCopyTuningToolTip;
            }
        }

        public RelayCommand CopyTuning
        {
            get
            {
                return _copyTuning ?? (_copyTuning = new RelayCommand(() =>
                {
                    try
                    {
                        ObservableInstrument targetInstrument = SelectedInstrument;

                        if (DefaultInstruments.Contains(targetInstrument))
                        {
                            // Try to find a suitable user instrument
                            ObservableInstrument userInstrument = null;
                            foreach (ObservableInstrument oi in UserInstruments)
                            {
                                if (oi.Name == targetInstrument.Name && oi.NumStrings == targetInstrument.NumStrings)
                                {
                                    userInstrument = oi;
                                }
                            }

                            // No user instrument, try to create one
                            if (null == userInstrument)
                            {
                                Instrument addedInstrument = AppVM.UserConfig.Instruments.Add(targetInstrument.Name, targetInstrument.NumStrings);
                                userInstrument = new ObservableInstrument(addedInstrument);
                            }

                            targetInstrument = userInstrument;
                        }

                        Messenger.Default.Send(new ShowTuningEditorMessage(SelectedTuning, targetInstrument, (accepted) =>
                        {
                            try
                            {
                                if (accepted)
                                {
                                    Refresh(targetInstrument.Instrument);
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
                }, () =>
                {
                    return InstrumentIsSelected;
                }));
            }
        }
        private RelayCommand _copyTuning;

        #endregion

        public Action RequestClose;

        public RelayCommand Close
        {
            get
            {
                return _close ?? (_close = new RelayCommand(() =>
                {
                    try
                    {
                        RequestClose?.Invoke();
                    }
                    catch (Exception ex)
                    {
                        ExceptionUtils.HandleException(ex);
                    }
                }));
            }
        }
        private RelayCommand _close;

        public InstrumentManagerViewModel()
        {
            _defaultInstruments = AppVM.GetDefaultInstruments();
            _userInstruments = AppVM.GetUserInstruments();
        }

        internal void Refresh(Instrument selectedInstrument = null, Tuning selectedTuning = null)
        {
            DefaultInstruments = AppVM.GetDefaultInstruments();
            UserInstruments = AppVM.GetUserInstruments();

            if (null == selectedInstrument)
            {
                SelectedInstrument = null;
            }
            else
            {
                foreach (ObservableInstrument oi in UserInstruments)
                {
                    if (oi.Instrument == selectedInstrument)
                    {
                        SelectedInstrument = oi;
                        break;
                    }
                }

                if (null != SelectedInstrument)
                {
                    foreach (ObservableInstrument oi in DefaultInstruments)
                    {
                        if (oi.Instrument == selectedInstrument)
                        {
                            SelectedInstrument = oi;
                            break;
                        }
                    }
                }

                if (null != SelectedInstrument && null != selectedTuning)
                {
                    foreach (ObservableTuning ot in Tunings)
                    {
                        if (ot.Tuning == selectedTuning)
                        {
                            SelectedTuning = ot;
                            break;
                        }
                    }
                }
            }
        }
    }
}
