// 
// ObservableEnums.cs
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
using System.Collections.Generic;
using System.Collections.ObjectModel;

using GalaSoft.MvvmLight;

using com.jonthysell.Chordious.Core;

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

            collection.Add("Regular");
            collection.Add("Chord Name");

            return collection;
        }

        public static ObservableCollection<string> GetOrientations()
        {
            ObservableCollection<string> collection = new ObservableCollection<string>();

            collection.Add("Vertical");
            collection.Add("Horizontal");

            return collection;
        }

        public static ObservableCollection<string> GetLabelLayoutModels()
        {
            ObservableCollection<string> collection = new ObservableCollection<string>();

            collection.Add("Overlap");
            collection.Add("Add Horizontal Padding");
            collection.Add("Add Vertical Padding");
            collection.Add("Add Horizontal & Vertical Padding");

            return collection;
        }

        public static ObservableCollection<string> GetHorizontalAlignments()
        {
            ObservableCollection<string> collection = new ObservableCollection<string>();

            collection.Add("Left");
            collection.Add("Center");
            collection.Add("Right");

            return collection;
        }

        public static ObservableCollection<string> GetVerticalAlignments()
        {
            ObservableCollection<string> collection = new ObservableCollection<string>();

            collection.Add("Top");
            collection.Add("Middle");
            collection.Add("Bottom");

            return collection;
        }

        public static ObservableCollection<string> GetTextStyles()
        {
            ObservableCollection<string> collection = new ObservableCollection<string>();

            collection.Add("Regular");
            collection.Add("Bold");
            collection.Add("Italic");
            collection.Add("Bold & Italic");

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

            collection.Add("Normal");
            collection.Add("Muted");
            collection.Add("Root");
            collection.Add("Open");
            collection.Add("Open Root");
            collection.Add("Bottom");

            return collection;
        }

        public static ObservableCollection<string> GetMarkShapes()
        {
            ObservableCollection<string> collection = new ObservableCollection<string>();

            collection.Add("None");
            collection.Add("Circle");
            collection.Add("Square");
            collection.Add("Diamond");
            collection.Add("X");

            return collection;
        }

        public static ObservableCollection<string> GetBarreStacks()
        {
            ObservableCollection<string> collection = new ObservableCollection<string>();

            collection.Add("Under Marks");
            collection.Add("Over Marks");

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
