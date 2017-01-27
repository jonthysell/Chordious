// 
// ObservableEnums.cs
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
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace com.jonthysell.Chordious.Core.ViewModel
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
                if (null == _fontFamilies)
                {
                    _fontFamilies = GetFontFamilies();
                }
                return _fontFamilies;
            }
        }
        private static ObservableCollection<string> _fontFamilies;

        public static ObservableCollection<string> GetDiagramLabelStyles()
        {
            ObservableCollection<string> collection = new ObservableCollection<string>();

            collection.Add(EnumUtils.GetFriendlyValue(DiagramLabelStyle.Regular));
            collection.Add(EnumUtils.GetFriendlyValue(DiagramLabelStyle.ChordName));

            return collection;
        }

        public static ObservableCollection<string> GetOrientations()
        {
            ObservableCollection<string> collection = new ObservableCollection<string>();

            collection.Add(EnumUtils.GetFriendlyValue(DiagramOrientation.UpDown));
            collection.Add(EnumUtils.GetFriendlyValue(DiagramOrientation.LeftRight));

            return collection;
        }

        public static ObservableCollection<string> GetLabelLayoutModels()
        {
            ObservableCollection<string> collection = new ObservableCollection<string>();

            collection.Add(EnumUtils.GetFriendlyValue(DiagramLabelLayoutModel.Overlap));
            collection.Add(EnumUtils.GetFriendlyValue(DiagramLabelLayoutModel.AddPaddingHorizontal));
            collection.Add(EnumUtils.GetFriendlyValue(DiagramLabelLayoutModel.AddPaddingVertical));
            collection.Add(EnumUtils.GetFriendlyValue(DiagramLabelLayoutModel.AddPaddingBoth));

            return collection;
        }

        public static ObservableCollection<string> GetHorizontalAlignments()
        {
            ObservableCollection<string> collection = new ObservableCollection<string>();

            collection.Add(EnumUtils.GetFriendlyValue(DiagramHorizontalAlignment.Left));
            collection.Add(EnumUtils.GetFriendlyValue(DiagramHorizontalAlignment.Center));
            collection.Add(EnumUtils.GetFriendlyValue(DiagramHorizontalAlignment.Right));

            return collection;
        }

        public static ObservableCollection<string> GetVerticalAlignments()
        {
            ObservableCollection<string> collection = new ObservableCollection<string>();

            collection.Add(EnumUtils.GetFriendlyValue(DiagramVerticalAlignment.Top));
            collection.Add(EnumUtils.GetFriendlyValue(DiagramVerticalAlignment.Middle));
            collection.Add(EnumUtils.GetFriendlyValue(DiagramVerticalAlignment.Bottom));

            return collection;
        }

        public static ObservableCollection<string> GetTextStyles()
        {
            ObservableCollection<string> collection = new ObservableCollection<string>();

            collection.Add(EnumUtils.GetFriendlyValue(DiagramTextStyle.Regular));
            collection.Add(EnumUtils.GetFriendlyValue(DiagramTextStyle.Bold));
            collection.Add(EnumUtils.GetFriendlyValue(DiagramTextStyle.Italic));
            collection.Add(EnumUtils.GetFriendlyValue(DiagramTextStyle.BoldItalic));

            return collection;
        }

        private static ObservableCollection<string> GetFontFamilies()
        {
            ObservableCollection<string> collection = new ObservableCollection<string>(AppViewModel.Instance.GetSystemFonts());

            SortedInsert(collection, "serif");
            SortedInsert(collection, "sans-serif");
            SortedInsert(collection, "monospace");

            return collection;
        }


        public static ObservableCollection<string> GetMarkTypes()
        {
            ObservableCollection<string> collection = new ObservableCollection<string>();

            collection.Add(EnumUtils.GetFriendlyValue(DiagramMarkType.Normal));
            collection.Add(EnumUtils.GetFriendlyValue(DiagramMarkType.Muted));
            collection.Add(EnumUtils.GetFriendlyValue(DiagramMarkType.Root));
            collection.Add(EnumUtils.GetFriendlyValue(DiagramMarkType.Open));
            collection.Add(EnumUtils.GetFriendlyValue(DiagramMarkType.OpenRoot));
            collection.Add(EnumUtils.GetFriendlyValue(DiagramMarkType.Bottom));

            return collection;
        }

        public static ObservableCollection<string> GetMarkShapes()
        {
            ObservableCollection<string> collection = new ObservableCollection<string>();

            collection.Add(EnumUtils.GetFriendlyValue(DiagramMarkShape.None));
            collection.Add(EnumUtils.GetFriendlyValue(DiagramMarkShape.Circle));
            collection.Add(EnumUtils.GetFriendlyValue(DiagramMarkShape.Square));
            collection.Add(EnumUtils.GetFriendlyValue(DiagramMarkShape.Diamond));
            collection.Add(EnumUtils.GetFriendlyValue(DiagramMarkShape.X));

            return collection;
        }

        public static ObservableCollection<string> GetBarreStacks()
        {
            ObservableCollection<string> collection = new ObservableCollection<string>();

            collection.Add(EnumUtils.GetFriendlyValue(DiagramBarreStack.UnderMarks));
            collection.Add(EnumUtils.GetFriendlyValue(DiagramBarreStack.OverMarks));

            return collection;
        }

        public static ObservableCollection<string> GetBarreTypeOptions()
        {
            ObservableCollection<string> collection = new ObservableCollection<string>();

            collection.Add(EnumUtils.GetFriendlyValue(BarreTypeOption.None));
            collection.Add(EnumUtils.GetFriendlyValue(BarreTypeOption.Partial));
            collection.Add(EnumUtils.GetFriendlyValue(BarreTypeOption.Full));

            return collection;
        }

        public static ObservableCollection<string> GetMarkTextOptions()
        {
            ObservableCollection<string> collection = new ObservableCollection<string>();

            collection.Add(EnumUtils.GetFriendlyValue(MarkTextOption.None));
            collection.Add(EnumUtils.GetFriendlyValue(MarkTextOption.ShowNote_ShowBoth));
            collection.Add(EnumUtils.GetFriendlyValue(MarkTextOption.ShowNote_PreferFlats));
            collection.Add(EnumUtils.GetFriendlyValue(MarkTextOption.ShowNote_PreferSharps));

            return collection;
        }

        public static ObservableCollection<string> GetColors()
        {
            ObservableCollection<string> collection = new ObservableCollection<string>();

            foreach (string color in ColorUtils.NamedColors)
            {
                collection.Add(color);
            }

            return collection;
        }

        public static void SortedInsert(ObservableCollection<string> sortedCollection, string value)
        {
            if (null == sortedCollection)
            {
                throw new ArgumentNullException("sortedCollection");
            }

            if (String.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentNullException("value");
            }

            value = value.Trim();

            List<string> tempList = new List<string>(sortedCollection);

            int index = Array.BinarySearch<string>(tempList.ToArray(), value);

            if (index < 0)
            {
                index = ~index;

                if (index == sortedCollection.Count)
                {
                    sortedCollection.Add(value);
                }
                else
                {
                    sortedCollection.Insert(index, value);
                }
            }
        }
    }
}
