using System;
using System.Collections.Generic;
using System.Text;

namespace NETCore.RedisKit.Shared
{
    internal static class EnumerableExtensions
    {
        /// <summary>
        /// IEnumerable extension method ForEach
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection">Colection</param>
        /// <param name="action">Action method</param>
        public static void ForEach<T>(this IEnumerable<T> collection, Action<T> action)
        {
            foreach (T item in collection)
            {
                action(item);
            }
        }
    }
}
