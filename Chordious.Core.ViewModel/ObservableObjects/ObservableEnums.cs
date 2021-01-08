// 
// ObservableEnums.cs
//  
// Author:
//       Jon Thysell <thysell@gmail.com>
// 
// Copyright (c) 2015, 2016, 2017, 2019, 2021 Jon Thysell <http://jonthysell.com>
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

namespace Chordious.Core.ViewModel
{
    public class ObservableEnums
    {
        public static ObservableCollection<string> GetNotes()
        {
            ObservableCollection<string> collection = new ObservableCollection<string>();

            for (int i = 0; i < Enum.GetValues(typeof(Note)).Length; i++)
            {
                collection.Add(NoteUtils.ToString((Note)i));
            }

            return collection;
        }

        public static ObservableCollection<string> GetInternalNotes()
        {
            ObservableCollection<string> collection = new ObservableCollection<string>();

            for (int i = 0; i < Enum.GetValues(typeof(InternalNote)).Length; i++)
            {
                collection.Add(NoteUtils.ToString((InternalNote)i, InternalNoteStringStyle.ShowBoth));
            }

            return collection;
        }

        public static ObservableCollection<string> FontFamilies
        {
            get
            {
                return _fontFamilies ?? (_fontFamilies = GetFontFamilies());
            }
        }
        private static ObservableCollection<string> _fontFamilies;

        public static ObservableCollection<string> GetDiagramLabelStyles()
        {
            ObservableCollection<string> collection = new ObservableCollection<string>
            {
                EnumUtils.GetFriendlyValue(DiagramLabelStyle.Regular),
                EnumUtils.GetFriendlyValue(DiagramLabelStyle.ChordName)
            };

            return collection;
        }

        public static ObservableCollection<string> GetOrientations()
        {
            ObservableCollection<string> collection = new ObservableCollection<string>
            {
                EnumUtils.GetFriendlyValue(DiagramOrientation.UpDown),
                EnumUtils.GetFriendlyValue(DiagramOrientation.LeftRight)
            };

            return collection;
        }

        public static ObservableCollection<string> GetLabelLayoutModels()
        {
            ObservableCollection<string> collection = new ObservableCollection<string>
            {
                EnumUtils.GetFriendlyValue(DiagramLabelLayoutModel.Overlap),
                EnumUtils.GetFriendlyValue(DiagramLabelLayoutModel.AddPaddingHorizontal),
                EnumUtils.GetFriendlyValue(DiagramLabelLayoutModel.AddPaddingVertical),
                EnumUtils.GetFriendlyValue(DiagramLabelLayoutModel.AddPaddingBoth)
            };

            return collection;
        }

        public static ObservableCollection<string> GetHorizontalAlignments()
        {
            ObservableCollection<string> collection = new ObservableCollection<string>
            {
                EnumUtils.GetFriendlyValue(DiagramHorizontalAlignment.Left),
                EnumUtils.GetFriendlyValue(DiagramHorizontalAlignment.Center),
                EnumUtils.GetFriendlyValue(DiagramHorizontalAlignment.Right)
            };

            return collection;
        }

        public static ObservableCollection<string> GetVerticalAlignments()
        {
            ObservableCollection<string> collection = new ObservableCollection<string>
            {
                EnumUtils.GetFriendlyValue(DiagramVerticalAlignment.Top),
                EnumUtils.GetFriendlyValue(DiagramVerticalAlignment.Middle),
                EnumUtils.GetFriendlyValue(DiagramVerticalAlignment.Bottom)
            };

            return collection;
        }

        public static ObservableCollection<string> GetTextStyles()
        {
            ObservableCollection<string> collection = new ObservableCollection<string>
            {
                EnumUtils.GetFriendlyValue(DiagramTextStyle.Regular),
                EnumUtils.GetFriendlyValue(DiagramTextStyle.Bold),
                EnumUtils.GetFriendlyValue(DiagramTextStyle.Italic),
                EnumUtils.GetFriendlyValue(DiagramTextStyle.BoldItalic)
            };

            return collection;
        }

        private static ObservableCollection<string> GetFontFamilies()
        {
            ObservableCollection<string> collection = new ObservableCollection<string>(AppViewModel.Instance.AppView.GetSystemFonts());

            collection.SortedInsert("serif");
            collection.SortedInsert("sans-serif");
            collection.SortedInsert("monospace");

            return collection;
        }


        public static ObservableCollection<string> GetMarkTypes()
        {
            ObservableCollection<string> collection = new ObservableCollection<string>
            {
                EnumUtils.GetFriendlyValue(DiagramMarkType.Normal),
                EnumUtils.GetFriendlyValue(DiagramMarkType.Muted),
                EnumUtils.GetFriendlyValue(DiagramMarkType.Root),
                EnumUtils.GetFriendlyValue(DiagramMarkType.Open),
                EnumUtils.GetFriendlyValue(DiagramMarkType.OpenRoot),
                EnumUtils.GetFriendlyValue(DiagramMarkType.Bottom)
            };

            return collection;
        }

        public static ObservableCollection<string> GetMarkShapes()
        {
            ObservableCollection<string> collection = new ObservableCollection<string>
            {
                EnumUtils.GetFriendlyValue(DiagramMarkShape.None),
                EnumUtils.GetFriendlyValue(DiagramMarkShape.Circle),
                EnumUtils.GetFriendlyValue(DiagramMarkShape.Square),
                EnumUtils.GetFriendlyValue(DiagramMarkShape.Diamond),
                EnumUtils.GetFriendlyValue(DiagramMarkShape.X)
            };

            return collection;
        }

        public static ObservableCollection<string> GetBarreStacks()
        {
            ObservableCollection<string> collection = new ObservableCollection<string>
            {
                EnumUtils.GetFriendlyValue(DiagramBarreStack.UnderMarks),
                EnumUtils.GetFriendlyValue(DiagramBarreStack.OverMarks)
            };

            return collection;
        }

        public static ObservableCollection<string> GetBarreTypeOptions()
        {
            ObservableCollection<string> collection = new ObservableCollection<string>
            {
                EnumUtils.GetFriendlyValue(BarreTypeOption.None),
                EnumUtils.GetFriendlyValue(BarreTypeOption.Partial),
                EnumUtils.GetFriendlyValue(BarreTypeOption.Full)
            };

            return collection;
        }

        public static ObservableCollection<string> GetMarkTextOptions()
        {
            ObservableCollection<string> collection = new ObservableCollection<string>
            {
                EnumUtils.GetFriendlyValue(MarkTextOption.None),
                EnumUtils.GetFriendlyValue(MarkTextOption.ShowNote_ShowBoth),
                EnumUtils.GetFriendlyValue(MarkTextOption.ShowNote_PreferFlats),
                EnumUtils.GetFriendlyValue(MarkTextOption.ShowNote_PreferSharps)
            };

            return collection;
        }

        public static ObservableCollection<string> GetFretLabelSides()
        {
            ObservableCollection<string> collection = new ObservableCollection<string>
            {
                EnumUtils.GetFriendlyValue(FretLabelSide.Left),
                EnumUtils.GetFriendlyValue(FretLabelSide.Right)
            };

            return collection;
        }
    }
}
