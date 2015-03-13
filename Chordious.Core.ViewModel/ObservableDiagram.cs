// 
// ObservableDiagram.cs
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
    public class ObservableDiagram : ObservableObject
    {
        public string SvgText
        {
            get
            {
                return Diagram.ToImageMarkup(ImageMarkupType.SVG);
            }
        }

        public object ImageObject
        {
            get
            {
                return _imageObject;
            }
            private set
            {
                _imageObject = value;
                RaisePropertyChanged("ImageObject");
            }
        }
        private object _imageObject;

        #region Dimensions

        public int Width
        {
            get
            {
                return (int)Diagram.TotalWidth();
            }
        }

        public int Height
        {
            get
            {
                return (int)Diagram.TotalHeight();
            }
        }

        public int NumFrets
        {
            get
            {
                return Diagram.NumFrets;
            }
            set
            {
                try
                {
                    Diagram.NumFrets = value;
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }
                
                RaisePropertyChanged("NumFrets");
                Refresh();
            }
        }

        public int NumStrings
        {
            get
            {
                return Diagram.NumStrings;
            }
            set
            {
                try
                {
                    Diagram.NumStrings = value;
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }

                RaisePropertyChanged("NumStrings");
                Refresh();
            }
        }

        #endregion


        #region Title

        public string TitleText
        {
            get
            {
                return Diagram.Title;
            }
            set
            {
                Diagram.Title = value;
                RaisePropertyChanged("TitleText");
                Refresh();
            }
        }

        public double TitleGridPadding
        {
            get
            {
                return Diagram.TitleGridPadding;
            }
            set
            {
                try
                {
                    Diagram.TitleGridPadding = value;
                }
                catch (Exception ex)
                {
                    ExceptionUtils.HandleException(ex);
                }

                RaisePropertyChanged("TitleGridPadding");
                Refresh();
            }
        }

        public bool TitleVisible
        {
            get
            {
                return Diagram.TitleVisible;
            }
            set
            {
                Diagram.TitleVisible = value;
                RaisePropertyChanged("TitleVisible");
                Refresh();
            }
        }

        public int SelectedTitleLabelStyleIndex
        {
            get
            {
                return (int)Diagram.TitleLabelStyle;
            }
            set
            {
                Diagram.TitleLabelStyle = (DiagramLabelStyle)(value);
                RaisePropertyChanged("SelectedTitleLabelStyleIndex");
                Refresh();
            }
        }

        public ObservableCollection<string> TitleLabelStyles
        {
            get
            {
                return GetDiagramLabelStyles();
            }
        }

        #endregion

        public RelayCommand RenderImage
        {
            get
            {
                return new RelayCommand(() =>
                {
                    Render();
                });
            }
        }

        public bool AutoRender
        {
            get
            {
                return _autorender;
            }
            set
            {
                _autorender = value;
                RaisePropertyChanged("AutoRender");
            }
        }
        private bool _autorender;

        internal Diagram Diagram
        {
            get
            {
                return _diagram;
            }
            set
            {
                if (null == value)
                {
                    throw new ArgumentNullException();
                }
                _diagram = value;
                Refresh();
            }
        }
        private Diagram _diagram;

        public ObservableDiagram(Diagram diagram, bool autoRender = true) : base()
        {
            if (null == diagram)
            {
                throw new ArgumentNullException("diagram");
            }

            AutoRender = autoRender;
            Diagram = diagram;
        }

        public void Refresh()
        {
            RaisePropertyChanged("SvgText");
            RaisePropertyChanged("Height");
            RaisePropertyChanged("Width");

            if (AutoRender)
            {
                Render();
            }
        }

        private void Render()
        {
            AppViewModel.Instance.DoOnUIThread(() =>
            {
                ImageObject = AppViewModel.Instance.SvgTextToImage(SvgText);
            });
        }

        private static ObservableCollection<string> GetDiagramLabelStyles()
        {
            ObservableCollection<string> collection = new ObservableCollection<string>();

            collection.Add("Regular");
            collection.Add("Chord Name");

            return collection;
        }
    }
}
