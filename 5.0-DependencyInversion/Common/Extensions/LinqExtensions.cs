using System;
using System.Collections.Generic;

namespace _5._0_DependencyInversion.Common.Extensions
{
    public static class LinqExtensions
    {
        public static IEnumerable<TFirst> ExceptBy<TFirst, TSecond, TKey>(
            this IEnumerable<TFirst> first,
            IEnumerable<TSecond> second,
            Func<TFirst, TKey> firstKeySelector,
            Func<TSecond, TKey> secondKeySelector)
        {
            var set = new HashSet<TKey>();
            foreach (TSecond element in second)
            {
                set.Add(secondKeySelector(element));
            }

            foreach (TFirst element in first)
            {
                if (set.Add(firstKeySelector(element)))
                {
                    yield return element;
                }
            }
        }

        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            var set = new HashSet<TKey>();
            foreach (TSource element in source)
            {
                if (set.Add(keySelector(element)))
                {
                    yield return element;
                }
            }
        }
    }
}
