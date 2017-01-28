// 
// DiagramExportViewModelBase.cs
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
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

using com.jonthysell.Chordious.Core.ViewModel.Resources;

namespace com.jonthysell.Chordious.Core.ViewModel
{
    public delegate void ExportStartEventHandler(object sender, EventArgs e);

    public delegate void ExportEndEventHandler(object sender, EventArgs e);

    public abstract class DiagramExportViewModelBase : ViewModelBase
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
                RaisePropertyChanged("IsIdle");
                ExportAsync.RaiseCanExecuteChanged();
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
                RaisePropertyChanged("PercentComplete");
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
                    try
                    {
                        IsIdle = false;
                        PercentComplete = 0;

                        OnExportStart();

                        for (int i = 0; i < DiagramsToExport.Count; i++)
                        {
                            await ExportDiagramAsync(i);
                            PercentComplete = (i + 1) / (double)DiagramsToExport.Count;
                        }
                    }
                    catch (Exception ex)
                    {
                        ExceptionUtils.HandleException(ex);
                    }
                    finally
                    {
                        OnExportEnd();

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

        public event ExportStartEventHandler ExportStart;

        public event ExportEndEventHandler ExportEnd;

        internal ChordiousSettings SettingsBuffer { get; private set; }

        public DiagramExportViewModelBase(ObservableCollection<ObservableDiagram> diagramsToExport, string collectionName = "")
        {
            if (null == diagramsToExport)
            {
                throw new ArgumentNullException("diagramsToExport");
            }

            DiagramsToExport = diagramsToExport;

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

        private void OnExportEnd()
        {
            ExportEnd?.Invoke(this, new EventArgs());
        }
    }
}
