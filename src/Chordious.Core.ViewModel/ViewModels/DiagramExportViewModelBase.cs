﻿// Copyright (c) Jon Thysell <http://jonthysell.com>
// Licensed under the MIT License.

using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

using Chordious.Core.ViewModel.Resources;

namespace Chordious.Core.ViewModel
{
    public abstract class DiagramExportViewModelBase : ObservableObject
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
                return Strings.DiagramExportTitle;
            }
        }

        public bool IsIdle
        {
            get
            {
                return _isIdle;
            }
            protected set
            {
                _isIdle = value;
                OnPropertyChanged(nameof(IsIdle));
                ExportAsync.NotifyCanExecuteChanged();
            }
        }
        private bool _isIdle = true;

        public double PercentComplete
        {
            get
            {
                return _percentComplete;
            }
            protected set
            {
                if (value < 0 || value > 1.0)
                {
                    throw new ArgumentOutOfRangeException();
                }
                _percentComplete = value;
                OnPropertyChanged(nameof(PercentComplete));
            }
        }
        private double _percentComplete = 0.0;

        public string CollectionName { get; private set; }

        public ObservableCollection<ObservableDiagram> DiagramsToExport { get; private set; }

        public int MaxWidth
        {
            get
            {
                return DiagramsToExport.Max((od) => od.TotalWidth);
            }
        }

        public int MaxHeight
        {
            get
            {
                return DiagramsToExport.Max((od) => od.TotalHeight);
            }
        }

        public string ExportAsyncLabel
        {
            get
            {
                return Strings.DiagramExportExportAsyncLabel;
            }
        }

        public string ExportAsyncToolTip
        {
            get
            {
                return Strings.DiagramExportExportAsyncToolTip;
            }
        }

        public RelayCommand ExportAsync
        {
            get
            {
                return _exportAsync ?? (_exportAsync = new RelayCommand(async () =>
                {
                    _exportAsyncCancellationTokenSource = new CancellationTokenSource();
                    bool cancelled = false;

                    try
                    {
                        IsIdle = false;
                        PercentComplete = 0;

                        OnExportStart();

                        for (int i = 0; i < DiagramsToExport.Count; i++)
                        {
                            if (_exportAsyncCancellationTokenSource.IsCancellationRequested)
                            {
                                cancelled = true;
                                break;
                            }
                            await ExportDiagramAsync(i);
                            PercentComplete = (i + 1) / (double)DiagramsToExport.Count;
                        }

                        if (cancelled)
                        {
                            StrongReferenceMessenger.Default.Send(new ChordiousMessage(Strings.DiagramExportCancelledMessage));
                        }
                        else
                        {
                            StrongReferenceMessenger.Default.Send(new ConfirmationMessage(Strings.DiagramExportCloseAfterCompletePrompt, (confirm) =>
                            {
                                if (confirm)
                                {
                                    RequestClose?.Invoke();
                                }
                            }, "confirmation.diagramexport.close"));
                        }
                    }
                    catch (Exception ex)
                    {
                        cancelled = true;
                        ExceptionUtils.HandleException(ex);
                    }
                    finally
                    {
                        OnExportEnd(cancelled);

                        IsIdle = true;
                        PercentComplete = 0;
                    }
                }, () =>
                {
                    return CanExport();
                }));
            }
        }
        private RelayCommand _exportAsync;

        private CancellationTokenSource _exportAsyncCancellationTokenSource;

        public Action RequestClose;

        public RelayCommand CancelOrClose
        {
            get
            {
                return _cancelOrClose ?? (_cancelOrClose = new RelayCommand(() =>
                {
                    try
                    {
                        if (null != _exportAsyncCancellationTokenSource)
                        {
                            _exportAsyncCancellationTokenSource.Cancel();
                        }
                        else if (!_lastExportComplete.HasValue || (DateTime.Now - _lastExportComplete.Value) > TimeSpan.FromMilliseconds(500))
                        {
                            RequestClose?.Invoke();
                        }
                    }
                    catch (Exception ex)
                    {
                        ExceptionUtils.HandleException(ex);
                    }
                }));
            }
        }
        private RelayCommand _cancelOrClose;

        private readonly DateTime? _lastExportComplete = null;

        public event EventHandler ExportStart;

        public event EventHandler<ExportEndEventArgs> ExportEnd;

        internal ChordiousSettings SettingsBuffer { get; private set; }

        public DiagramExportViewModelBase(ObservableCollection<ObservableDiagram> diagramsToExport, string collectionName = "")
        {
            DiagramsToExport = diagramsToExport ?? throw new ArgumentNullException(nameof(diagramsToExport));

            if (string.IsNullOrWhiteSpace(collectionName))
            {
                collectionName = "";
            }

            CollectionName = collectionName.Trim();

            SettingsBuffer = new ChordiousSettings(AppVM.UserConfig.ChordiousSettings, "DiagramExport");
        }

        public abstract void ProcessClose();

        protected string GetSetting(string key)
        {
            return SettingsBuffer.Get(key);
        }

        protected void SetSetting(string key, object value)
        {
            SettingsBuffer.Set(key, value);
        }

        protected void SaveSettingsAsDefault()
        {
            SettingsBuffer.SetParent();
        }

        protected void SaveSettingAsDefault(string key)
        {
            SettingsBuffer.SetParent(key);
        }

        protected abstract Task ExportDiagramAsync(int diagramIndex);

        protected bool CanExport()
        {
            return (null != DiagramsToExport && DiagramsToExport.Count > 0) && IsIdle;
        }

        private void OnExportStart()
        {
            ExportStart?.Invoke(this, new EventArgs());
        }

        private void OnExportEnd(bool canceled)
        {
            ExportEnd?.Invoke(this, new ExportEndEventArgs(canceled));
        }
    }

    public class ExportEndEventArgs : EventArgs
    {
        public bool Cancelled { get; private set; }

        public ExportEndEventArgs(bool cancelled)
        {
            Cancelled = cancelled;
        }
    }
}
