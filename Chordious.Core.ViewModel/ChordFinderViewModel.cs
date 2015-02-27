// 
// ChordFinderViewModel.cs
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
using System.Collections.ObjectModel;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

using com.jonthysell.Chordious.Core;

namespace com.jonthysell.Chordious.Core.ViewModel
{
    public class ChordFinderViewModel : ViewModelBase
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
                return "Chord Finder";
            }
        }

        public bool IsSearching
        {
            get
            {
                return _isSearching;
            }
            private set
            {
                _isSearching = value;
                RaisePropertyChanged("IsSearching");
            }
        }
        private bool _isSearching = false;

        public int NumFrets
        {
            get
            {
                return Options.NumFrets;
            }
            set
            {
                try
                {
                    Options.NumFrets = value;
                }
                catch(Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
                RaisePropertyChanged("NumFrets");
                if (NumFrets < MaxReach)
                {
                    MaxReach = NumFrets;
                }
            }
        }

        public int MaxReach
        {
            get
            {
                return Options.MaxReach;
            }
            set
            {
                try
                {
                    Options.MaxReach = value;
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
                RaisePropertyChanged("MaxReach");
                if (MaxReach > NumFrets)
                {
                    NumFrets = MaxReach;
                }
            }
        }

        public int MaxFret
        {
            get
            {
                return Options.MaxFret;
            }
            set
            {
                try
                {
                    Options.MaxFret = value;
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
                RaisePropertyChanged("MaxFret");
            }
        }

        public bool AllowOpenStrings
        {
            get
            {
                return Options.AllowOpenStrings;
            }
            set
            {
                Options.AllowOpenStrings = value;
                RaisePropertyChanged("AllowOpenStrings");
            }
        }

        public bool AllowMutedStrings
        {
            get
            {
                return Options.AllowMutedStrings;
            }
            set
            {
                Options.AllowMutedStrings = value;
                RaisePropertyChanged("AllowMutedStrings");
            }
        }

        public bool AllowRootlessChords
        {
            get
            {
                return Options.AllowRootlessChords;
            }
            set
            {
                Options.AllowRootlessChords = value;
                RaisePropertyChanged("AllowRootlessChords");
            }
        }

        public bool MirrorResults
        {
            get
            {
                return Options.MirrorResults;
            }
            set
            {
                Options.MirrorResults = value;
                RaisePropertyChanged("MirrorResults");
            }
        }

        public RelayCommand Search
        {
            get
            {
                return new RelayCommand(() =>
                {
                    try
                    {
                        FindChords();
                    }
                    catch (Exception ex)
                    {
                        ExceptionUtils.HandleException(ex);
                    }
                });
            }
        }

        public RelayCommand SaveSelected
        {
            get
            {
                return new RelayCommand(() =>
                {
                    try
                    {
                        throw new NotImplementedException();
                    }
                    catch (Exception ex)
                    {
                        ExceptionUtils.HandleException(ex);
                    }
                }, () =>
                {
                    return false;
                });
            }
        }

        public ObservableCollection<string> SvgResults
        {
            get
            {
                ObservableCollection<string> results = new ObservableCollection<string>();

                if (null != _results)
                {
                    for (int i = 0; i < _results.Count; i++)
                    {
                        results.Add(_results.DiagramAt(i, Style).ToImageMarkup(ImageMarkupType.SVG));
                    }
                }

                return results;
            }
        }

        private ChordFinderResultSet _results;

        internal ChordFinderOptions Options { get; private set; }
        internal ChordFinderStyle Style { get; private set; }

        public ChordFinderViewModel()
        {
            Options = new ChordFinderOptions(AppVM.UserConfig);
            Style = new ChordFinderStyle(AppVM.UserConfig);
        }

        private void FindChords()
        {
            IsSearching = true;
            _results = ChordFinder.FindChords(Options);
            IsSearching = false;
            RaisePropertyChanged("SvgResults");
        }
    }
}
