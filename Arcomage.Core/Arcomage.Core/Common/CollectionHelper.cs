using System;
using System.Collections.Generic;

namespace Arcomage.Core.Common
{
    public static class CollectionHelper
    {
        public static void Randomize<T>(this IList<T> target)
        {
            Random RndNumberGenerator = new Random();
            SortedList<int, T> newList = new SortedList<int, T>();
            foreach (T item in target)
            {
                newList.Add(RndNumberGenerator.Next(), item);
            }
            target.Clear();
            for (int i = 0; i < newList.Count; i++)
            {
                target.Add(newList.Values[i]);
            }
        }
    }
}
