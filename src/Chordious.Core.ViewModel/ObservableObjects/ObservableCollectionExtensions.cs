// Copyright (c) Jon Thysell <http://jonthysell.com>
// Licensed under the MIT License.

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
