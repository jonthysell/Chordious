// Copyright (c) Jon Thysell <http://jonthysell.com>
// Licensed under the MIT License.

using System;
using System.Collections.Generic;

namespace Chordious.Core
{
    public class ListUtils
    {
        public static bool SortedInsert<T>(List<T> sortedList, T value) where T : IComparable
        {
            if (sortedList is null)
            {
                throw new ArgumentNullException(nameof(sortedList));
            }

            if (value is null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            int index = sortedList.BinarySearch(value);

            if (index < 0)
            {
                index = ~index;

                if (index == sortedList.Count)
                {
                    sortedList.Add(value);
                }
                else
                {
                    sortedList.Insert(index, value);
                }

                return true;
            }

            return false;
        }
    }
}
