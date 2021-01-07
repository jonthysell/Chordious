// 
// ObservableCollectionExtensions.cs
//  
// Author:
//       Jon Thysell <thysell@gmail.com>
// 
// Copyright (c) 2021 Jon Thysell <http://jonthysell.com>
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

namespace Chordious.Core.ViewModel
{
    public static class ObservableCollectionExtensions
    {
        public static void SortedInsert(this ObservableCollection<string> sortedCollection, string value)
        {
            if (null == sortedCollection)
            {
                throw new ArgumentNullException(nameof(sortedCollection));
            }

            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentNullException(nameof(value));
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
        public static void SortedInsert<T>(this ObservableCollection<T> sortedCollection, T value) where T: IComparable<T>
        {
            if (null == sortedCollection)
            {
                throw new ArgumentNullException(nameof(sortedCollection));
            }

            if (null == value)
            {
                throw new ArgumentNullException(nameof(value));
            }

            List<T> tempList = new List<T>(sortedCollection);

            int index = Array.BinarySearch<T>(tempList.ToArray(), value);

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
