// 
// DiagramEditorViewModelExtended.cs
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
using System.Collections.ObjectModel;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

using com.jonthysell.Chordious.Core.ViewModel;

namespace com.jonthysell.Chordious.WPF
{
    public class DiagramEditorViewModelExtended : DiagramEditorViewModel
    {
        public int SelectedEditorRenderBackgroundIndex
        {
            get
            {
                return (int)EditorRenderBackground;
            }
            set
            {
                EditorRenderBackground = (Background)(value);
                RaisePropertyChanged("SelectedEditorRenderBackgroundIndex");
            }
        }

        public ObservableCollection<string> EditorRenderBackgrounds
        {
            get
            {
                return ObservableEnums.GetBackgrounds();
            }
        }

        private Background EditorRenderBackground
        {
            get
            {
                Background result;

                if (Enum.TryParse<Background>(AppVM.GetSetting("diagrameditor.renderbackground"), out result))
                {
                    return result;
                }

                return Background.None;
            }
            set
            {
                AppVM.SetSetting("diagrameditor.renderbackground", value);
                RaisePropertyChanged("EditorRenderBackground");
                ObservableDiagram.Refresh();
            }
        }

        public DiagramEditorViewModelExtended(ObservableDiagram diagram, bool isNew) : base(diagram, isNew) { }

    }
}
